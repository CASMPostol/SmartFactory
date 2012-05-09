using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
    internal const string NameOfIsDouble = "IsDouble";
    internal static TimeSpan Span15min = new TimeSpan(0, 15, 0);
    internal Warehouse GetWarehouse()
    {
      if (this.TimeSlot2ShippingPointLookup == null)
        throw new ApplicationException(m_ShippingNotFpundMessage);
      if (this.TimeSlot2ShippingPointLookup.WarehouseTitle == null)
        throw new ApplicationException("Warehouse not found");
      return this.TimeSlot2ShippingPointLookup.WarehouseTitle;
    }
    internal TimeSlotTimeSlot FindAdjacent(List<TimeSlotTimeSlot> _avlblTmslts)
    {
      for (int _i = 0; _i < _avlblTmslts.Count; _i++)
      {
        if ((_avlblTmslts[_i].StartTime.Value - this.EndTime.Value).Duration() <= Span15min)
          return _avlblTmslts[_i];
      }
      throw new ApplicationException("Cannot find the time slot to make the couple.");
    }
    private const string m_ShippingNotFpundMessage = "Shipping slot is not selected";
    internal List<TimeSlotTimeSlot> MakeBooking(Shipping _sp, bool _isDouble)
    {
      if (this.Occupied.Value == Entities.Occupied.Occupied0)
        throw new ApplicationException("Time slot has been aleady reserved");
      List<TimeSlotTimeSlot> _ret = new List<TimeSlotTimeSlot>();
      _ret.Add(this);
      this.Occupied = Entities.Occupied.Occupied0;
      this.TimeSlot2ShippingIndex = _sp;
      this.IsDouble = _isDouble;
      //TODO cleanup commented code.
      //if (IsOutbound.Value)
      //  _ts.Tytuł = String.Format("Outbound No. {0} to {1}", this.Tytuł, this.City == null ? "--not assigned--" : City.Tytuł);
      //else
      //  _ts.Tytuł = String.Format("Inbound No. {0} by {1}", this.Tytuł, this.VendorName == null ? "--not assigned--" : VendorName.Tytuł);
      if (!_isDouble)
        return _ret;
      EntitySet<TimeSlot> _tslots = this.TimeSlot2ShippingPointLookup.TimeSlot;
      DateTime _tdy = this.StartTime.Value.Date;
      List<TimeSlotTimeSlot> _avlblTmslts = (from _tsidx in _tslots
                                     let _idx = _tsidx.StartTime.Value.Date
                                     where _tsidx.Occupied.Value == Entities.Occupied.Free && _idx >= _tdy && _idx <= _tdy.AddDays(1)
                                     orderby _tsidx.StartTime ascending
                                             select _tsidx).Cast<TimeSlotTimeSlot>().ToList<TimeSlotTimeSlot>();
      TimeSlotTimeSlot _next = this.FindAdjacent(_avlblTmslts);
      _ret.Add(_next);
      _next.Occupied = Entities.Occupied.Occupied0;
      _next.TimeSlot2ShippingIndex = _sp;
      _next.IsDouble = true;
      //this.EndTime = _next.EndTime;
      //this.Duration = Convert.ToDouble((_ts.EndTime.Value - _ts.EndTime.Value).TotalMinutes);
      return _ret;
    }
    internal double? Duration()
    {
      if (!EndTime.HasValue || !StartTime.HasValue)
        return null;
      return (EndTime.Value - StartTime.Value).TotalMinutes;
    }
  }
}
