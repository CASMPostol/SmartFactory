using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
    internal static TimeSpan Span15min = new TimeSpan(0, 15, 0);
    internal Warehouse GetWarehouse()
    {
      if (this.ShippingPoint == null)
        throw new ApplicationException(m_ShippingNotFpundMessage);
      if (this.ShippingPoint.Warehouse == null)
        throw new ApplicationException("Warehouse not found");
      return this.ShippingPoint.Warehouse;
    }
    internal void MakeBooking(Shipping _si)
    {
      if (this.Occupied.Value == Entities.Occupied.Occupied0)
        throw new ApplicationException("Time slot has been aleady reserved");
      this.Occupied = Entities.Occupied.Occupied0;
      this.ShippingIndex = _si;
      if (!this.IsDouble.HasValue || !this.IsDouble.Value)
        return;
      EntitySet<TimeSlot> _tslots = this.ShippingPoint.TimeSlot;
      DateTime _tdy = this.StartTime.Value.Date;
      List<TimeSlot> _avlblTmslts = (from _tsidx in _tslots
                                     let _idx = _tsidx.StartTime.Value.Date
                                     where _tsidx.Occupied.Value == Entities.Occupied.Free && _idx >= _tdy && _idx <= _tdy.AddDays(1)
                                     orderby _tsidx.StartTime ascending
                                     select _tsidx).ToList<TimeSlot>();
      TimeSlot _next = FindAdjacent(_avlblTmslts);
      _next.Occupied = Entities.Occupied.Occupied0;
      _next.ShippingIndex = _si;
      _next.IsDouble = true;
    }
    private TimeSlot FindAdjacent(List<TimeSlot> _avlblTmslts)
    {
      for (int _i = 0; _i < _avlblTmslts.Count; _i++)
      {
        if ((_avlblTmslts[_i].StartTime.Value - this.EndTime.Value).Duration() <= Span15min)
          return _avlblTmslts[_i];
      }
      throw new ApplicationException("Cannot find the time slot to make the couple.");
    }
    private const string m_ShippingNotFpundMessage = "Shipping slot is not selected";
  }
}
