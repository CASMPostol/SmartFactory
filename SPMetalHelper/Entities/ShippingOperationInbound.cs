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
      if (this.Shipping2RouteTitle == _nr)
        return;
      this.TrailerTitle = null;
      this.TruckTitle = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.PartnerTitle);
      this.Shipping2RouteTitle = _nr;
      if (_nr == null)
      {
        this.BusinessDescription = String.Empty;
        this.PartnerTitle = null;
        return;
      }
      this.BusinessDescription = Shipping2RouteTitle.Route2BusinessDescriptionTitle == null ? String.Empty : Shipping2RouteTitle.Route2BusinessDescriptionTitle.Tytuł;
      this.PartnerTitle = Shipping2RouteTitle.PartnerTitle;
    }
    internal void ChangeEscort(SecurityEscortCatalog _nr, EntitiesDataContext _EDC)
    {
      if (this.SecurityEscortCatalogTitle == _nr)
        return;
      this.Shipping2TruckTitle = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.Shipping2PartnerTitle);
      this.SecurityEscortCatalogTitle = _nr;
      if (_nr == null)
      {
        this.Shipping2PartnerTitle = null;
        return;
      }
      this.SecurityEscortCatalogTitle = _nr;
      this.Shipping2PartnerTitle = _nr == null ? null : _nr.PartnerTitle;
    }
    private void RemoveDrivers(EntitiesDataContext _EDC, Partner _prtne)
    {
      if (_prtne == null)
        return;
      List<ShippingDriversTeam> _2Delete = new List<ShippingDriversTeam>();
      foreach (ShippingDriversTeam _drv in this.ShippingDriversTeam)
      {
        if (_prtne == _drv.DriverTitle.Driver2PartnerTitle)
        {
          //TODO clenup the code
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
      switch (this.ShippingState.Value)
      {
        case Entities.ShippingState.Canceled:
        case Entities.ShippingState.Completed:
        case Entities.ShippingState.Cancelation:
          return false;
        case Entities.ShippingState.Confirmed:
        case Entities.ShippingState.Creation:
        case Entities.ShippingState.Delayed:
        case Entities.ShippingState.WaitingForCarrierData:
        case Entities.ShippingState.WaitingForSecurityData:
        case Entities.ShippingState.Underway:
          return true;
        case Entities.ShippingState.Invalid:
        case Entities.ShippingState.None:
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
          item.TimeSlot2ShippingIndex = null;
          item.IsDouble = false;
        }
      }
      return true;
    }
    internal void SetupTiming(TimeSlotTimeSlot _ts, bool _isDouble)
    {
      this.StartTime = _ts.StartTime;
      this.EndTime = _ts.EndTime;
      this.ShippingDuration = Convert.ToDouble((_ts.EndTime.Value - _ts.StartTime.Value).TotalMinutes);
      this.Shipping2WarehouseTitle = _ts.GetWarehouse();
      this.LoadingType = _isDouble ? Entities.LoadingType.Manual : Entities.LoadingType.Pallet;
    }
    internal void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
    internal void CalculateState()
    {
      switch (this.ShippingState.Value)
      {
        case Entities.ShippingState.Confirmed:
        case Entities.ShippingState.Creation:
        case Entities.ShippingState.Delayed:
        case Entities.ShippingState.WaitingForCarrierData:
        case Entities.ShippingState.WaitingForSecurityData:
          int _seDrivers = 0;
          int _crDrivers = 0;
          foreach (var _dr in this.ShippingDriversTeam)
            if (_dr.DriverTitle.Driver2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider)
              _seDrivers++;
            else
              _crDrivers++;
          this.ShippingState = Entities.ShippingState.Creation;
          if (_crDrivers > 0 && this.TruckTitle != null)
          {
            if (this.SecurityEscortCatalogTitle == null || (_seDrivers > 0 && this.Shipping2TruckTitle != null))
              this.ShippingState = Entities.ShippingState.Confirmed;
            else
              this.ShippingState = Entities.ShippingState.WaitingForSecurityData;
          }
          else if (this.SecurityEscortCatalogTitle == null || (_seDrivers > 0 && this.Shipping2TruckTitle != null))
            this.ShippingState = Entities.ShippingState.WaitingForCarrierData;
          break;
        case Entities.ShippingState.Underway:
        case Entities.ShippingState.None:
        case Entities.ShippingState.Invalid:
        case Entities.ShippingState.Cancelation:
        case Entities.ShippingState.Canceled:
        case Entities.ShippingState.Completed:
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
