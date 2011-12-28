using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
    internal class DateTimeClass
    {
      public DateTime StartTime { get; set; }
    }
    internal static IQueryable<TimeSlotTimeSlot> GetForSelectedDay(EntitiesDataContext _edc, DateTime _day, string _warehouseID)
    {
      int _intWrhs = int.Parse(_warehouseID);
      return from _idx in _edc.TimeSlot
             where IsExpected(_idx, _intWrhs) && (_idx.StartTime.Value.Date == _day.Date )
             orderby _idx.StartTime ascending 
             select _idx;
    }
    internal static IQueryable<DateTime> GetForSelectedMonth(EntitiesDataContext _edc, DateTime _day, string _warehouseID)
    {
      int _intWrhs = int.Parse(_warehouseID);
      return from _idx in _edc.TimeSlot
             where IsExpected(_idx, _intWrhs) && (_idx.StartTime.Value.Year == _day.Year) && (_idx.StartTime.Value.Month == _day.Month)
             select _idx.StartTime.Value;
    }
    internal static TimeSlotTimeSlot GetShippingTimeSlot(EntitiesDataContext edc, int? _id)
    {
      try
      {
        return (
              from idx in edc.TimeSlot
              where idx.ShippingIndex.Identyfikator == _id
              orderby idx.StartTime descending
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException("Time slot not found");
      }
    }
    internal static TimeSlotTimeSlot GetShippingTimeSlot(EntitiesDataContext _edc, string _id)
    {
      if (string.IsNullOrEmpty(_id))
        throw new ApplicationException("Cannot found the Time Slot because the index is null");
      int _intid = int.Parse(_id);
      return GetShippingTimeSlot(_edc, _intid);
    }
    internal static TimeSlotTimeSlot GetAtIndex(EntitiesDataContext edc, string _id)
    {
      if (string.IsNullOrEmpty(_id))
        throw new ApplicationException("Cannot find the Time Slot because the index is null");
      try
      {
        int _intid = int.Parse(_id);
        return (
              from idx in edc.TimeSlot
              where idx.Identyfikator == _intid
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException("Time slot not found");
      }
    }
    internal Warehouse GetWarehouse()
    {
      if (this.ShippingPoint == null)
        throw new ApplicationException("Shipping Point not found");
      if (this.ShippingPoint.Warehouse == null)
        throw new ApplicationException("Warehouse not found");
      return this.ShippingPoint.Warehouse;
    }
    internal void MakeBooking(ShippingOperationInbound _si)
    {
      if (this.Occupied.Value)
        throw new ApplicationException("Time slot has been aleady reserved");
      this.Occupied = true;
      this.ShippingIndex = _si;
    }
    internal void ReleaseBooking()
    {
      this.Occupied = false;
      this.ShippingIndex = null;
    }
    private static bool IsExpected(TimeSlotTimeSlot _ts, int _warehouseID)
    {
      return _ts.ShippingPoint == null ? false : _ts.ShippingPoint.Warehouse == null ? false : _ts.ShippingPoint.Warehouse.Identyfikator == _warehouseID;
    }
  }
}
