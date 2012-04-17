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
      this.TrailerRegistrationNumber = null;
      this.TruckCarRegistrationNumber = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.VendorName);
      this.Route = _nr;
      if (_nr == null)
      {
        this.BusinessDescription = String.Empty;
        this.VendorName = null;
        return;
      }
      this.BusinessDescription = Route.BusinessDescription == null ? String.Empty : Route.BusinessDescription.Tytuł;
      this.VendorName = Route.VendorName;
    }
    internal void ChangeEscort(SecurityEscortCatalog _nr, EntitiesDataContext _EDC)
    {
      if (this.SecurityEscort == _nr)
        return;
      this.SecurityEscortCarRegistrationNumber = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.SecurityEscortProvider);
      this.SecurityEscort = _nr;
      if (_nr == null)
      {
        this.SecurityEscortProvider = null;
        return;
      }
      this.SecurityEscort = _nr;
      this.SecurityEscortProvider = _nr == null ? null : _nr.VendorName;
    }
    private void RemoveDrivers(EntitiesDataContext _EDC, Partner _prtne)
    {
      if (_prtne == null)
        return;
      List<ShippingDriversTeam> _2Delete = new List<ShippingDriversTeam>();
      foreach (ShippingDriversTeam _drv in this.ShippingDriversTeam)
      {
        if (_prtne == _drv.Driver.VendorName)
        {
          //_drv.ShippingIndex = null;
          //_drv.Driver = null;
          //_2Delete.Add(_drv);
          //_EDC.SubmitChanges();
          _2Delete.Add(_drv);
        }
      }
      _EDC.DriversTeam.DeleteAllOnSubmit(_2Delete);
      _EDC.SubmitChanges();
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
            string _frmt = "ReleaseBooking tries to release the Timeslot ID = {0}, which has a wrong value of the field Occupied.";
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
    internal void SetupTiming(TimeSlotTimeSlot _ts, bool _isDouble)
    {
      this.StartTime = _ts.StartTime;
      this.EndTime = _ts.EndTime;
      this.Duration = Convert.ToDouble((_ts.EndTime.Value - _ts.StartTime.Value).TotalMinutes);
      this.Warehouse = _ts.GetWarehouse();
      this.LoadingType = _isDouble ? Entities.LoadingType.Manual : Entities.LoadingType.Pallet;
    }
    internal void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
    internal void CalculateState()
    {
      switch (this.State.Value)
      {
        case Entities.State.Confirmed:
        case Entities.State.Creation:
        case Entities.State.Delayed:
        case Entities.State.WaitingForCarrierData:
        case Entities.State.WaitingForSecurityData:
        case Entities.State.Underway:
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
          break;
        case Entities.State.None:
        case Entities.State.Invalid:
        case Entities.State.Cancelation:
        case Entities.State.Canceled:
        case Entities.State.Completed:
        default:
          break;
      }
    }
    #endregion

    #region private
    private TimeSpan _12h = new TimeSpan(12, 0, 0);
    #endregion
  }
}
