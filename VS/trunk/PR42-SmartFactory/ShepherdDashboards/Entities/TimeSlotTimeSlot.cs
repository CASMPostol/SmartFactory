﻿using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
    internal class DateTimeClass
    {
      public DateTime StartTime { get; set; }
    }
    internal static IQueryable<TimeSlotTimeSlot> GetForSelectedDay(EntitiesDataContext _edc, DateTime _day, int _warehouseID)
    {
      return from _idx in _edc.TimeSlot
             where IsExpected(_idx, _warehouseID) && (_idx.StartTime.Value.Date == _day.Date)
             orderby _idx.StartTime ascending
             select _idx;
    }
    internal static IQueryable<DateTime> GetFreeForSelectedMonth(EntitiesDataContext _edc, DateTime _day, string _warehouseID)
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
        throw new ApplicationException(m_TSNotFpundMessage);
      }
    }
    internal static TimeSlotTimeSlot GetShippingTimeSlot(EntitiesDataContext _edc, string _id)
    {
      if (string.IsNullOrEmpty(_id))
        throw new ApplicationException("Shipping is not selected.");
      int _intid = int.Parse(_id);
      return GetShippingTimeSlot(_edc, _intid);
    }
    internal static IQueryable<TimeSlotTimeSlot> GetAtIndex(EntitiesDataContext edc, int _id, bool free)
    {
      try
      {
        return (
              from idx in edc.TimeSlot
              where (idx.Identyfikator == _id) && (idx.Occupied != free)

              select idx);
      }
      catch (Exception)
      {
        throw new ApplicationException(m_TSNotFpundMessage);
      }
    }
    internal static TimeSlotTimeSlot GetAtIndex(EntitiesDataContext edc, string _id, bool free)
    {
      if (string.IsNullOrEmpty(_id))
        throw new ApplicationException(m_TSNotFpundMessage);
      int _intid = -1;
      if (!int.TryParse(_id, out _intid))
        throw new ApplicationException("Wrong Time Slot index syntax");
      return GetAtIndex(edc, _intid, free).First<TimeSlotTimeSlot>();
    }
    internal Warehouse GetWarehouse()
    {
      if (this.ShippingPoint == null)
        throw new ApplicationException(m_ShippingNotFpundMessage);
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
      if (_ts.Occupied.Value)
        return false;
      return _ts.ShippingPoint == null ? false : _ts.ShippingPoint.Warehouse == null ? false : _ts.ShippingPoint.Warehouse.Identyfikator == _warehouseID;
    }
    private const string m_TSNotFpundMessage = "Time slot is not selected";
    private const string m_ShippingNotFpundMessage = "Shipping slot is not selected";
  }
}
