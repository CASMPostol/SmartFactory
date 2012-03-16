using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    internal void ChangeRout(Route _nr)
    {
      this.Route = _nr;
      if (this.Route == null)
      {
        this.BusinessDescription = String.Empty;
        this.VendorName = null;
        return;
      }
      this.BusinessDescription = Route.BusinessDescription == null ? String.Empty : Route.BusinessDescription.Tytuł;
      this.VendorName = Route.VendorName;
    }
    internal static Shipping GetAtIndex(EntitiesDataContext edc, int? _index)
    {
      if (!_index.HasValue)
        throw new ApplicationException("ShippingOperationInbound index is null"); ;
      try
      {
        return (
              from idx in edc.Shipping
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("ShippingOperationInbound cannot be found at specified index{0}", _index.Value));
      }
    }
    internal bool IsEditable()
    {
      switch (this.State.Value)
      {
        case Entities.State.Canceled:
        case Entities.State.Completed:
          return false;
        case Entities.State.Confirmed:
        case Entities.State.Creation:
        case Entities.State.Delayed:
        case Entities.State.Waiting4ExternalApproval:
        case Entities.State.Waiting4InternalApproval:
        case Entities.State.Underway:
          return true;
        case Entities.State.Invalid:
        case Entities.State.None:
        default:
          throw new ApplicationException("Wrong Shipping state");
      }
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
        item.Occupied = Entities.Occupied.Free;
        item.ShippingIndex = null;
        item.IsDouble = false;
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
      this.StartTime = _ts.CzasRozpoczęcia;
      this.EndTime = _ts.CzasZakończenia;
      this.Warehouse = _ts.GetWarehouse().Tytuł;
      if (!_isDouble)
      {
        this.LoadingType = Entities.LoadingType.Pallet;
        return;
      }
      this.LoadingType = Entities.LoadingType.Manual;
      EntitySet<TimeSlot> _tslots = _ts.ShippingPoint.TimeSlot;
      DateTime _tdy = _ts.CzasRozpoczęcia.Value.Date;
      List<TimeSlot> _avlblTmslts = (from _tsidx in _tslots
                                     let _idx = _tsidx.CzasRozpoczęcia.Value.Date
                                     where _tsidx.Occupied.Value == Entities.Occupied.Free && _idx >= _tdy && _idx <= _tdy.AddDays(1)
                                     orderby _tsidx.CzasRozpoczęcia ascending
                                     select _tsidx).ToList<TimeSlot>();
      TimeSlot _next = _ts.FindAdjacent(_avlblTmslts);
      _next.Occupied = Entities.Occupied.Occupied0;
      _next.ShippingIndex = this;
      _next.IsDouble = true;
    }
    internal void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
  }
}
