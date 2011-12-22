using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class TimeSlotTimeSlot
  {
    internal static TimeSlotTimeSlot GetTimeSlot(EntitiesDataContext edc, string _id)
    {
      try
      {
        int _intid = int.Parse(_id);
        return (
              from idx in edc.TimeSlot
              where idx.ShippingIndex.Identyfikator == _intid
              orderby idx.StartTime descending
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
