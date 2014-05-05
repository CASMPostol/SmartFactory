﻿//<summary>
//  Title   : partial class TimeSlotTimeSlot
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  ///  Extend the TimeSlotTimeSlot autogenerated class
  /// </summary>
  public partial class TimeSlotTimeSlot
  {
    /// <summary>
    /// The name of is double
    /// </summary>
    public const string NameOfIsDouble = "IsDouble";
    /// <summary>
    /// The span15min
    /// </summary>
    public static TimeSpan Span15min = new TimeSpan(0, 15, 0);
    /// <summary>
    /// Gets the warehouse.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">
    /// Warehouse not found
    /// </exception>
    public Warehouse GetWarehouse()
    {
      if (this.TimeSlot2ShippingPointLookup == null)
        throw new ApplicationException(m_ShippingNotFoundMessage);
      if (this.TimeSlot2ShippingPointLookup.WarehouseTitle == null)
        throw new ApplicationException("Warehouse not found");
      return this.TimeSlot2ShippingPointLookup.WarehouseTitle;
    }
    /// <summary>
    /// Durations this instance.
    /// </summary>
    /// <returns></returns>
    public double? Duration()
    {
      if (!EndTime.HasValue || !StartTime.HasValue)
        return null;
      return (EndTime.Value - StartTime.Value).TotalMinutes;
    }
    /// <summary>
    /// Books the time slots.
    /// </summary>
    /// <param name="EDC">The EDC.</param>
    /// <param name="timeSlot">The time slot.</param>
    /// <param name="isDouble">if set to <c>true</c> [is double].</param>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">Time slot has been aleady reserved</exception>
    public static List<TimeSlotTimeSlot> BookTimeSlots(EntitiesDataContext EDC, string timeSlot, bool isDouble)
    {
      TimeSlotTimeSlot _timeSlot = Element.GetAtIndex<TimeSlotTimeSlot>(EDC.TimeSlot, timeSlot);
      if (_timeSlot.Occupied.GetValueOrDefault(Entities.Occupied.None) == Entities.Occupied.Occupied0)
        throw new TimeSlotException("Time slot has been aleady reserved");
      List<TimeSlotTimeSlot> _ret = new List<TimeSlotTimeSlot>();
      _ret.Add(_timeSlot);
      _timeSlot.Occupied = Entities.Occupied.Occupied0;
      if (isDouble)
      {
        Debug.Assert(_timeSlot.StartTime.HasValue, "TimeSlot StartTime has to have Value");
        DateTime _tdy = _timeSlot.StartTime.Value.Date;
        List<TimeSlotTimeSlot> _avlblTmslts = (from _tsidx in EDC.TimeSlot.ToList()
                                               let _idx = _tsidx.StartTime.Value.Date
                                               where _tsidx.Occupied.GetValueOrDefault(Entities.Occupied.None) == Entities.Occupied.Free &&
                                                     _idx >= _tdy &&
                                                     _idx <= _tdy.AddDays(1)
                                               orderby _tsidx.StartTime ascending
                                               select _tsidx).Where<TimeSlotTimeSlot>(x => x.TimeSlot2ShippingPointLookup == _timeSlot.TimeSlot2ShippingPointLookup).ToList<TimeSlotTimeSlot>();
        TimeSlotTimeSlot _next = FindAdjacent(_avlblTmslts, _timeSlot);
        _ret.Add(_next);
        _next.Occupied = Entities.Occupied.Occupied0;
      }
      return _ret;
    }
    /// <summary>
    /// Deletes all not used time slots.
    /// </summary>
    /// <param name="EDC">The <see cref="EntitiesDataContext "/> object representing Linq entities.</param> DeletesExpired
    public static void DeleteExpired(EntitiesDataContext EDC)
    {
      IEnumerable<TimeSlotTimeSlot> _2Delete =
          from _tsx in EDC.TimeSlot
          where (_tsx.StartTime < DateTime.Now - new TimeSpan(72, 0, 0)) && _tsx.Occupied.Value == Entities.Occupied.Free
          select _tsx;
      EDC.TimeSlot.DeleteAllOnSubmit(_2Delete);
    }
    #region private
    /// <summary>
    /// TimeSlotException
    /// </summary>
    [Serializable]
    public class TimeSlotException : Exception
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="TimeSlotException"/> class.
      /// </summary>
      /// <param name="message">The message that describes the error.</param>
      public TimeSlotException(string message) : base(message, null) { }
      /// <summary>
      /// Initializes a new instance of the <see cref="TimeSlotException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
      protected TimeSlotException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }
    }
    private static TimeSlotTimeSlot FindAdjacent(List<TimeSlotTimeSlot> _avlblTmslts, TimeSlotTimeSlot timeSlot)
    {
      for (int _i = 0; _i < _avlblTmslts.Count; _i++)
      {
        if ((_avlblTmslts[_i].StartTime.Value - timeSlot.EndTime.Value).Duration() <= TimeSlotTimeSlot.Span15min)
          return _avlblTmslts[_i];
      }
      throw new TimeSlotException("Cannot find the time slot to make the couple.");
    }
    /// <summary>
    /// The m_ shipping not fpund message
    /// </summary>
    private const string m_ShippingNotFoundMessage = "Shipping slot is not selected";
    #endregion

  }//TimeSlotTimeSlot
}
