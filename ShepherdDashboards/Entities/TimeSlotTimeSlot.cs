using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
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
        throw new ApplicationException("Cannot found the Time Slot because the index is null");
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
  }
}
