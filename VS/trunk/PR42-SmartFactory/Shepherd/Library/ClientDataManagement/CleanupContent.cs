//<summary>
//  Title   : CleanupContent
//  System  : Microsoft VisualStudio 2013 / C#
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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using CAS.SharePoint.Client.Link2SQL;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class CleanupContent - provides service to cleanup the website content.
  /// </summary>
  public static class CleanupContent
  {
    /// <summary>
    /// Does the content cleanup.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="reportProgress">An action to report the progress.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    /// <exception cref="System.ApplicationException">DoCleanupContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.</exception>
    public static void DoCleanupContent(string URL, string sqlConnectionString, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      bool _breakingIssueEncountered = false;
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoCleanupContent and establishing connection with the site {0}.", URL)));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Connection string {0}", sqlConnectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(sqlConnectionString);
      using (Linq2SQL.SHRARCHIVE _sqledc = new Linq2SQL.SHRARCHIVE(_connection))
      {
        if (!_sqledc.DatabaseExists())
          throw new ArgumentException("Cannot connect to SQL database.", "SQLConnectionString");
        using (Linq.Entities _edc = new Linq.Entities(trace, URL))
        {
          //Commodity
          //Warehouse
          //Partner
          //Currency
          //FreightPayer
          //BusinessDescription
          //SecurityEscortRoute
          //SealProtocolLibrary
          //Country
          //City
          //EscortPOLibrary
          //FreightPOLibrary
          //Carrier
          //SAPDestinationPlant
          //ShipmentType
          //TransportUnitType
          //Route
          //Truck
          //Trailer
          //Shipping
          //AlarmsAndEvents
          //CarrierPerformanceReport
          _breakingIssueEncountered &= DoDestinationMarket(_edc);
          //Market
          //DestinationMarket
          //Driver
          //DriversTeam
          //LoadDescription
          //ShippingPoint
          //TimeSlot

          //ArchivingOperationLogs
          //ArchivingLogs
          //History
          _breakingIssueEncountered &= DoTimeSlotsTemplate(_edc, _sqledc, reportProgress, trace);

        }
        reportProgress(new ProgressChangedEventArgs(1, "Finished DoCleanupContent"));
        if (_breakingIssueEncountered)
          throw new ApplicationException("DoCleanupContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.");
      }
    }
    /// <summary>
    /// Does the cleanup of the lists <see cref="Linq.ScheduleTemplate"/>, <see cref="Linq.TimeSlotsTemplateTimeSlotsTemplate>"/>.
    /// </summary>
    /// <param name="spedc">The spedc.</param>
    /// <param name="sqledc">The sqledc.</param>
    /// <param name="reportProgress">The report progress.</param>
    /// <param name="trace">The trace.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private static bool DoTimeSlotsTemplate(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      trace("Entering DoTimeSlotsTemplate");
      bool _ret = true;
      IEnumerable<Linq.ScheduleTemplate> _ScheduleTemplate2BeDeleted = spedc.ScheduleTemplate.ToList<Linq.ScheduleTemplate>().Where<Linq.ScheduleTemplate>(x => x.ShippingPointLookupTitle == null);
      List<Linq.TimeSlotsTemplateTimeSlotsTemplate> _TimeSlotsTemplateAll = spedc.TimeSlotsTemplate.ToList<Linq.TimeSlotsTemplateTimeSlotsTemplate>();
      List<Linq.TimeSlotsTemplateTimeSlotsTemplate> _TimeSlotsTemplate2BeDeleted = new List<Linq.TimeSlotsTemplateTimeSlotsTemplate>();
      foreach (Linq.ScheduleTemplate _item in _ScheduleTemplate2BeDeleted)
        _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplateTimeSlotsTemplate>(x => x.ScheduleTemplateTitle == _item));
      _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplateTimeSlotsTemplate>(x => x.ScheduleTemplateTitle == null));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} TimeSlotsTemplate entries to be deleted.", _TimeSlotsTemplate2BeDeleted.Count)));
      spedc.TimeSlotsTemplate.Delete<Linq.TimeSlotsTemplateTimeSlotsTemplate, Linq2SQL.History>
         (_TimeSlotsTemplate2BeDeleted, null, x => sqledc.TimeSlotsTemplate.GetAt<Linq2SQL.TimeSlotsTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ScheduleTemplate entries to be deleted.", _ScheduleTemplate2BeDeleted.Count())));
      spedc.ScheduleTemplate.Delete<Linq.ScheduleTemplate, Linq2SQL.History>
         (_ScheduleTemplate2BeDeleted, null, x => sqledc.ScheduleTemplate.GetAt<Linq2SQL.ScheduleTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      //TimeSlotTimeSlot
      List<Linq.TimeSlotTimeSlot> _TimeSlotAll = spedc.TimeSlot.ToList<Linq.TimeSlotTimeSlot>().Where<Linq.TimeSlotTimeSlot>(x => x.StartDate.Value < DateTime.Now).ToList<Linq.TimeSlotTimeSlot>();
      List<Linq.TimeSlotTimeSlot> _TimeSlot3BeDeleted = (from _tsx in _TimeSlotAll
                                                         let _notUsed = _tsx.TimeSlot2ShippingIndex != null && (_tsx.TimeSlot2ShippingIndex.ShippingState == Linq.ShippingState.Canceled ||
                                                                                                                _tsx.TimeSlot2ShippingIndex.ShippingState == Linq.ShippingState.Completed)
                                                         where _notUsed && _tsx.StartDate.Value < DateTime.Now
                                                         select _tsx).ToList<Linq.TimeSlotTimeSlot>();
      _TimeSlot3BeDeleted.AddRange(_TimeSlotAll.Where<Linq.TimeSlotTimeSlot>(x => x.TimeSlot2ShippingIndex == null));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ScheduleTemplate entries to be deleted.", _TimeSlot3BeDeleted.Count())));
      spedc.TimeSlot.Delete<Linq.TimeSlotTimeSlot, Linq2SQL.History>
         (_TimeSlot3BeDeleted, null, x => sqledc.TimeSlot.GetAt<Linq2SQL.TimeSlot>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      //Link2SQLExtensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      
      return _ret;
    }
    #region private

    private static bool DoDestinationMarket(Linq.Entities edc)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
