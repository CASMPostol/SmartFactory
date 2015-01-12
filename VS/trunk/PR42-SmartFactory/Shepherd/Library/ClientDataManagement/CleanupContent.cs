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
          //ScheduleTemplate
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
    private static bool DoTimeSlotsTemplate(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      bool _ret = true;
      throw new NotImplementedException();
      IEnumerable<Linq.ScheduleTemplate> _ScheduleTemplate2BeDeleted = spedc.ScheduleTemplate.ToList<Linq.ScheduleTemplate>().Where<Linq.ScheduleTemplate>(x => x.ShippingPointLookupTitle == null);
      List<Linq.TimeSlotsTemplateTimeSlotsTemplate> _TimeSlotsTemplateAll = spedc.TimeSlotsTemplate.ToList<Linq.TimeSlotsTemplateTimeSlotsTemplate>();
      List<Linq.TimeSlotsTemplateTimeSlotsTemplate> _TimeSlotsTemplate2BeDeleted = new List<Linq.TimeSlotsTemplateTimeSlotsTemplate>();
      foreach (Linq.ScheduleTemplate _item in _ScheduleTemplate2BeDeleted)
        _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplateTimeSlotsTemplate>(x => x.ScheduleTemplateTitle == _item));
      _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplateTimeSlotsTemplate>(x => x.ScheduleTemplateTitle == null));
      //spedc.TimeSlotsTemplate.Delete<Linq.TimeSlotsTemplateTimeSlotsTemplate, Linq2SQL.History>
      //   (_TimeSlotsTemplate2BeDeleted, null, x => sqledc.TimeSlotsTemplate.GetAt<Linq2SQL.TimeSlotsTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
      //    x => sqledc.History.AddHistoryEntry(x));
      //Link2SQLExtensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y.UserState));


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
