﻿using System;
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
      if (this.ShippingPoint == null)
        throw new ApplicationException(m_ShippingNotFpundMessage);
      if (this.ShippingPoint.Warehouse == null)
        throw new ApplicationException("Warehouse not found");
      return this.ShippingPoint.Warehouse;
    }
    internal TimeSlot FindAdjacent(List<TimeSlot> _avlblTmslts)
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
