using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    #region private
    internal void ChangeRout(Route _nr, EntitiesDataContext _EDC)
    {
      if (this.Route == _nr)
        return;
      this.Route = _nr;
      this.TrailerRegistrationNumber = null;
      this.TruckCarRegistrationNumber = null;
      if (this.VendorName != null)
        foreach (ShippingDriversTeam _drv in this.ShippingDriversTeam)
        {
          if (this.VendorName == _drv.Driver.VendorName)
          {
            _drv.ShippingIndex = null;
            _drv.Driver = null;
            _EDC.DriversTeam.DeleteOnSubmit(_drv);
          }
          _EDC.SubmitChanges();
        }
      if (this.Route == null)
      {
        this.BusinessDescription = String.Empty;
        this.VendorName = null;
        return;
      }
      this.BusinessDescription = Route.BusinessDescription == null ? String.Empty : Route.BusinessDescription.Tytuł;
      this.VendorName = Route.VendorName;
    }
    internal bool IsEditable()
    {
      switch (this.State.Value)
      {
        case Entities.State.Canceled:
        case Entities.State.Completed:
        case Entities.State.Cancelation:
          return false;
        case Entities.State.Confirmed:
        case Entities.State.Creation:
        case Entities.State.Delayed:
        case Entities.State.WaitingForCarrierData:
        case Entities.State.WaitingForSecurityData:
        case Entities.State.Underway:
          return true;
        case Entities.State.Invalid:
        case Entities.State.None:
        default:
          throw new ApplicationException("Wrong Shipping state");
      }
    }
    internal bool Fixed()
    {
      return this.StartTime.Value - _12h < DateTime.Now;
    }
    internal bool ReleaseBooking(int? _newTimeSlot)
    {
      if (this.TimeSlot == null || this.TimeSlot.Count == 0)
        return true;
      List<TimeSlot> _2release = new List<TimeSlot>();
      foreach (var item in this.TimeSlot)
      {
        if (item.Occupied != Entities.Occupied.Occupied0)
        {
          if (_newTimeSlot == item.Identyfikator.Value) //consistency check 
          {
            string _frmt = "ReleaseBooking tries to release the Time Slot ID = {0}, which has a wrong value of the field Occupied.";
            throw new ApplicationException(String.Format(_frmt, item.Identyfikator.Value));
          }
          continue;
        }
        if (item.Identyfikator == _newTimeSlot)
          return false; //we are tring to allocate the same time slot. 
        _2release.Add(item);
      }
      foreach (var item in _2release)
      {
        //item.Tytuł = "-- not assigned --";
        if (Fixed())
          item.Occupied = Entities.Occupied.Delayed;
        else
        {
          item.Occupied = Entities.Occupied.Free;
          item.ShippingIndex = null;
          item.IsDouble = false;
        }
      }
      return true;
    }
    internal void MakeBooking(TimeSlotTimeSlot _ts, bool _isDouble)
    {
      if (_ts.Occupied.Value == Entities.Occupied.Occupied0)
        throw new ApplicationException("Time slot has been aleady reserved");
      _ts.Occupied = Entities.Occupied.Occupied0;
      _ts.ShippingIndex = this;
      _ts.IsDouble = _isDouble;
      //if (IsOutbound.Value)
      //  _ts.Tytuł = String.Format("Outbound No. {0} to {1}", this.Tytuł, this.City == null ? "--not assigned--" : City.Tytuł);
      //else
      //  _ts.Tytuł = String.Format("Inbound No. {0} by {1}", this.Tytuł, this.VendorName == null ? "--not assigned--" : VendorName.Tytuł);
      this.StartTime = _ts.StartTime;
      this.EndTime = _ts.EndTime;
      this.Duration = Convert.ToDouble((_ts.EndTime.Value - _ts.StartTime.Value).TotalMinutes);
      this.Warehouse = _ts.GetWarehouse();
      if (!_isDouble)
      {
        this.LoadingType = Entities.LoadingType.Pallet;
        return;
      }
      this.LoadingType = Entities.LoadingType.Manual;
      EntitySet<TimeSlot> _tslots = _ts.ShippingPoint.TimeSlot;
      DateTime _tdy = _ts.StartTime.Value.Date;
      List<TimeSlot> _avlblTmslts = (from _tsidx in _tslots
                                     let _idx = _tsidx.StartTime.Value.Date
                                     where _tsidx.Occupied.Value == Entities.Occupied.Free && _idx >= _tdy && _idx <= _tdy.AddDays(1)
                                     orderby _tsidx.StartTime ascending
                                     select _tsidx).ToList<TimeSlot>();
      TimeSlot _next = _ts.FindAdjacent(_avlblTmslts);
      _next.Occupied = Entities.Occupied.Occupied0;
      _next.ShippingIndex = this;
      _next.IsDouble = true;
      this.EndTime = _next.EndTime;
      this.Duration = Convert.ToDouble((_ts.EndTime.Value - _ts.EndTime.Value).TotalMinutes);
    }
    internal void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
    internal void CalculateState()
    {
      int _seDrivers = 0;
      int _crDrivers = 0;
      foreach (var _dr in this.ShippingDriversTeam)
        if (_dr.Driver.VendorName.ServiceType.Value == ServiceType.SecurityEscortProvider)
          _seDrivers++;
        else
          _crDrivers++;
      this.State = Entities.State.Creation;
      if (_crDrivers > 0 && this.TruckCarRegistrationNumber != null)
      {
        if (this.SecurityEscort == null || (_seDrivers > 0 && this.SecurityEscortCarRegistrationNumber != null))
          this.State = Entities.State.Confirmed;
        else
          this.State = Entities.State.WaitingForSecurityData;
      }
      else if (this.SecurityEscort == null || (_seDrivers > 0 && this.SecurityEscortCarRegistrationNumber != null))
        this.State = Entities.State.WaitingForCarrierData;
    }
    #endregion

    #region private
    private TimeSpan _12h = new TimeSpan(12, 0, 0);
    #endregion
  }
}
