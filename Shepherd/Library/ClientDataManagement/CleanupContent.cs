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

using CAS.SharePoint.Client.Link2SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;

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
    /// <param name="sqlConnectionString">The SQL connection string.</param>
    /// <param name="reportProgress">An action to report the progress.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    /// <exception cref="System.ArgumentException">
    /// Cannot connect to SQL database.;SQLConnectionString
    /// </exception>
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
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          //Warehouse
          //Partner

          _breakingIssueEncountered &= DoTimeSlotsTemplate(_spedc, _sqledc, reportProgress, trace);
          _breakingIssueEncountered &= DoShippingPoint(_spedc, _sqledc, reportProgress, trace);
        }
      }
      using (Linq2SQL.SHRARCHIVE _sqledc = new Linq2SQL.SHRARCHIVE(_connection))
      {
        if (!_sqledc.DatabaseExists())
          throw new ArgumentException("Cannot connect to SQL database.", "SQLConnectionString");
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _breakingIssueEncountered &= DoLoadDescription(_spedc, reportProgress, trace);
        }
      }
      using (Linq2SQL.SHRARCHIVE _sqledc = new Linq2SQL.SHRARCHIVE(_connection))
      {
        if (!_sqledc.DatabaseExists())
          throw new ArgumentException("Cannot connect to SQL database.", "SQLConnectionString");
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _breakingIssueEncountered &= DoDriversTeam(_spedc, _sqledc, reportProgress, trace);
        }
      }
      using (Linq2SQL.SHRARCHIVE _sqledc = new Linq2SQL.SHRARCHIVE(_connection))
      {
        if (!_sqledc.DatabaseExists())
          throw new ArgumentException("Cannot connect to SQL database.", "SQLConnectionString");
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _breakingIssueEncountered &= DoDriver(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoTruck(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoDestinationMarket(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoCarrierPerformanceReport(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoRoute(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoCity(_spedc, reportProgress, trace);
          _breakingIssueEncountered &= DoSecurityEscortRoute(_spedc, reportProgress, trace);
        }
      }
      using (Linq2SQL.SHRARCHIVE _sqledc = new Linq2SQL.SHRARCHIVE(_connection))
        CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.UpdateActivitiesLogs<Linq2SQL.ArchivingOperationLogs>(_sqledc, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName.Cleanup, reportProgress);
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoCleanupContent"));
      if (_breakingIssueEncountered)
        throw new ApplicationException("DoCleanupContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.");
    }

    #region private
    /// <summary>
    /// Does the cleanup of the lists <see cref="Linq.ScheduleTemplate"/>, <see cref="Linq.TimeSlotsTemplateTimeSlotsTemplate"/>, <see cref="Linq.TimeSlotTimeSlot"/>.
    /// </summary>
    /// <param name="spedc">The spedc.</param>
    /// <param name="sqledc">The sqledc.</param>
    /// <param name="reportProgress">The report progress.</param>
    /// <param name="trace">The trace.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    private static bool DoTimeSlotsTemplate(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      trace("Entering DoTimeSlotsTemplate");
      bool _ret = true;
      reportProgress(new ProgressChangedEventArgs(1, "Starting ScheduleTemplate and TimeSlotsTemplate lists processing."));
      IEnumerable<Linq.ScheduleTemplate> _ScheduleTemplate2BeDeleted = spedc.ScheduleTemplate.ToList<Linq.ScheduleTemplate>().Where<Linq.ScheduleTemplate>(x => x.ShippingPointLookupTitle == null);
      List<Linq.TimeSlotsTemplate> _TimeSlotsTemplateAll = spedc.TimeSlotsTemplate.ToList<Linq.TimeSlotsTemplate>();
      List<Linq.TimeSlotsTemplate> _TimeSlotsTemplate2BeDeleted = new List<Linq.TimeSlotsTemplate>();
      foreach (Linq.ScheduleTemplate _item in _ScheduleTemplate2BeDeleted)
        _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplate>(x => x.ScheduleTemplateTitle == _item));
      _TimeSlotsTemplate2BeDeleted.AddRange(_TimeSlotsTemplateAll.Where<Linq.TimeSlotsTemplate>(x => x.ScheduleTemplateTitle == null));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} TimeSlotsTemplate entries to be deleted.", _TimeSlotsTemplate2BeDeleted.Count)));
      spedc.TimeSlotsTemplate.Delete<Linq.TimeSlotsTemplate, Linq2SQL.History>
         (_TimeSlotsTemplate2BeDeleted, null, x => sqledc.TimeSlotsTemplate.GetAt<Linq2SQL.TimeSlotsTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ScheduleTemplate entries to be deleted.", _ScheduleTemplate2BeDeleted.Count())));
      spedc.ScheduleTemplate.Delete<Linq.ScheduleTemplate, Linq2SQL.History>
         (_ScheduleTemplate2BeDeleted, null, x => sqledc.ScheduleTemplate.GetAt<Linq2SQL.ScheduleTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      //TimeSlotTimeSlot
      reportProgress(new ProgressChangedEventArgs(1, "TimeSlot processing."));
      List<Linq.TimeSlotTimeSlot> _TimeSlotAll = spedc.TimeSlot.ToList<Linq.TimeSlotTimeSlot>().Where<Linq.TimeSlotTimeSlot>(x => x.StartTime.Value < DateTime.Now).ToList<Linq.TimeSlotTimeSlot>();
      List<Linq.TimeSlotTimeSlot> _TimeSlot3BeDeleted = (from _tsx in _TimeSlotAll
                                                         let _notUsed = _tsx.TimeSlot2ShippingIndex != null && (_tsx.TimeSlot2ShippingIndex.ShippingState == Linq.ShippingState.Canceled ||
                                                                                                                _tsx.TimeSlot2ShippingIndex.ShippingState == Linq.ShippingState.Completed)
                                                         where _notUsed && _tsx.StartTime.Value < DateTime.Now
                                                         select _tsx).ToList<Linq.TimeSlotTimeSlot>();
      _TimeSlot3BeDeleted.AddRange(_TimeSlotAll.Where<Linq.TimeSlotTimeSlot>(x => x.TimeSlot2ShippingIndex == null));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Time Slot entries to be deleted.", _TimeSlot3BeDeleted.Count())));
      spedc.TimeSlot.Delete<Linq.TimeSlotTimeSlot, Linq2SQL.History>
         (_TimeSlot3BeDeleted, null, x => sqledc.TimeSlot.GetAt<Linq2SQL.TimeSlot>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, "Starting SubmitChanges."));
      Link2SQLExtensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      trace("Successfully finished cleanup of the following lists ScheduleTemplate, TimeSlotsTemplateTimeSlotsTemplate, TimeSlotTimeSlot");
      return _ret;
    }
    private static bool DoShippingPoint(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      trace("Entering DoShippingPoint");
      IEnumerable<Linq.ShippingPoint> _ShippingPointAllCandidates = spedc.ShippingPoint.ToList<Linq.ShippingPoint>().Where<Linq.ShippingPoint>(x => x.WarehouseTitle == null);
      if (_ShippingPointAllCandidates.Count() == 0)
      {
        trace("Finished DoShippingPoint - there is no shipping point entries to be deleted.");
        return true;
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("ShippingPoint processing. There are {0} candidate entries to be deleted.", _ShippingPointAllCandidates.Count())));
      List<Linq.TimeSlotTimeSlot> _TimeSlotAll = spedc.TimeSlot.ToList<Linq.TimeSlotTimeSlot>();
      List<Linq.ShippingPoint> _ShippingPoint2BeDeleted = new List<Linq.ShippingPoint>();
      foreach (Linq.ShippingPoint _item in _ShippingPointAllCandidates)
        if (!_TimeSlotAll.Where<Linq.TimeSlotTimeSlot>(x => x.TimeSlot2ShippingPointLookup == _item).Any<Linq.TimeSlotTimeSlot>())
          _ShippingPoint2BeDeleted.Add(_item);
      if (_ShippingPoint2BeDeleted.Count() == 0)
      {
        trace("Finished DoShippingPoint - there is no shipping point entries to be deleted.");
        return true;
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ShippingPoint entries to be deleted.", _ShippingPoint2BeDeleted.Count())));
      List<Linq.ScheduleTemplate> _ScheduleTemplateAll = spedc.ScheduleTemplate.ToList<Linq.ScheduleTemplate>();
      List<Linq.ScheduleTemplate> _ScheduleTemplate2BeDeleted = new List<Linq.ScheduleTemplate>();
      foreach (Linq.ShippingPoint _item in _ShippingPoint2BeDeleted)
        _ScheduleTemplate2BeDeleted.AddRange(_ScheduleTemplateAll.Where(z => z.ShippingPointLookupTitle == _item));
      List<Linq.TimeSlotsTemplate> _TimeSlotsTemplateAll = spedc.TimeSlotsTemplate.ToList<Linq.TimeSlotsTemplate>();
      List<Linq.TimeSlotsTemplate> _TimeSlotsTemplate22BeDeleted = new List<Linq.TimeSlotsTemplate>();
      foreach (Linq.ScheduleTemplate _item in _ScheduleTemplate2BeDeleted)
        _TimeSlotsTemplate22BeDeleted.AddRange(_TimeSlotsTemplateAll.Where(z => z.ScheduleTemplateTitle == _item));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} TimeSlotsTemplate entries to be deleted.", _TimeSlotsTemplate22BeDeleted.Count())));
      spedc.TimeSlotsTemplate.Delete<Linq.TimeSlotsTemplate, Linq2SQL.History>
         (_TimeSlotsTemplate22BeDeleted, null, x => sqledc.TimeSlotsTemplate.GetAt<Linq2SQL.TimeSlotsTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ScheduleTemplate entries to be deleted.", _ScheduleTemplate2BeDeleted.Count())));
      spedc.ScheduleTemplate.Delete<Linq.ScheduleTemplate, Linq2SQL.History>
         (_ScheduleTemplate2BeDeleted, null, x => sqledc.ScheduleTemplate.GetAt<Linq2SQL.ScheduleTemplate>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ShippingPoint entries to be deleted.", _ShippingPoint2BeDeleted.Count())));
      spedc.ShippingPoint.Delete<Linq.ShippingPoint, Linq2SQL.History>
         (_ShippingPoint2BeDeleted, null, x => sqledc.ShippingPoint.GetAt<Linq2SQL.ShippingPoint>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, "Starting SubmitChanges."));
      Link2SQLExtensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      trace("Finished DoShippingPoint - cleanup of the following list TimeSlotsTemplate, ScheduleTemplate, ShippingPoint");
      return true;
    }
    private static bool DoLoadDescription(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoLoadDescription");
      List<Linq.LoadDescription> _LoaderOptimizationAll = spedc.LoadDescription.ToList<Linq.LoadDescription>();
      try
      {
        //_breakingIssueEncountered &= Assumption<Linq.LoadDescription>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.LoadDescription2PartnerTitle == null);
        _breakingIssueEncountered &= Assumption<Linq.LoadDescription>(_LoaderOptimizationAll, typeof(Linq.Shipping).Name, reportProgress, x => x.LoadDescription2ShippingIndex == null);
      }
      catch (Exception ex)
      {
        _LoaderOptimizationAll = null;
        _breakingIssueEncountered = false;
        reportProgress(new ProgressChangedEventArgs(1, String.Format("DoLoadDescription - Shipping lookup test aborter by exception: {0}.", ex.Message)));
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Load Description with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoDriversTeam(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoDriversTeam");
      List<Linq.Shipping> _ShippingShippingAll = spedc.Shipping.ToList<Linq.Shipping>();
      List<Linq.ShippingDriversTeam> _ShippingDriversTeamAll = spedc.DriversTeam.ToList<Linq.ShippingDriversTeam>();
      List<Linq.ShippingDriversTeam> _ShippingDriversTeam2BeDeleted = _ShippingDriversTeamAll.Where<Linq.ShippingDriversTeam>(x => x.DriverTitle == null).ToList<Linq.ShippingDriversTeam>();
      try
      {
        _ShippingDriversTeam2BeDeleted.AddRange(_ShippingDriversTeamAll.Where<Linq.ShippingDriversTeam>(x => x.ShippingIndex == null));
      }
      catch (Exception ex)
      {
        _ShippingDriversTeamAll = null;
        reportProgress(new ProgressChangedEventArgs(1, String.Format("DoDriversTeam - Shipping lookup test aborter by the exception: {0}.", ex.Message)));
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} ShippingDriversTeam entries to be deleted because they have empty lookup on Shipping or Driver.", _ShippingDriversTeam2BeDeleted.Count())));
      spedc.DriversTeam.Delete<Linq.ShippingDriversTeam, Linq2SQL.History>
         (_ShippingDriversTeam2BeDeleted, null, x => sqledc.DriversTeam.GetAt<Linq2SQL.DriversTeam>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, "Starting SubmitChanges."));
      Link2SQLExtensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      trace("Successfully finished cleanup of the Drivers Team list.");
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Drivers Team with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));    
      return _breakingIssueEncountered;
    }
    private static bool DoDriver(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoDriver");
      List<Linq.Driver> _LoaderOptimizationAll = spedc.Driver.ToList<Linq.Driver>();
      _breakingIssueEncountered &= Assumption<Linq.Driver>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.Driver2PartnerTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Driver with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoTrailer(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoTrailer");
      List<Linq.Trailer> _LoaderOptimizationAll = spedc.Trailer.ToList<Linq.Trailer>();
      _breakingIssueEncountered &= Assumption<Linq.Trailer>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.Trailer2PartnerTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Trailer with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoTruck(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoTruck");
      List<Linq.Truck> _LoaderOptimizationAll = spedc.Truck.ToList<Linq.Truck>();
      _breakingIssueEncountered &= Assumption<Linq.Truck>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.Truck2PartnerTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Truck with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoDestinationMarket(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoDestinationMarket");
      List<Linq.DestinationMarket> _LoaderOptimizationAll = spedc.DestinationMarket.ToList<Linq.DestinationMarket>();
      _breakingIssueEncountered &= Assumption<Linq.DestinationMarket>(_LoaderOptimizationAll, typeof(Linq.Market).Name, reportProgress, x => x.MarketTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.DestinationMarket>(_LoaderOptimizationAll, typeof(Linq.CityType).Name, reportProgress, x => x.DestinationMarket2CityTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Destination Market with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoCarrierPerformanceReport(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoCarrierPerformanceReport");
      List<Linq.CarrierPerformanceReport> _LoaderOptimizationAll = spedc.CarrierPerformanceReport.ToList<Linq.CarrierPerformanceReport>();
      _breakingIssueEncountered &= Assumption<Linq.CarrierPerformanceReport>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.CPR2PartnerTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Carier Performance Report with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoRoute(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoRoute");
      List<Linq.Route> _LoaderOptimizationAll = spedc.Route.ToList<Linq.Route>();
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.FreightPayer).Name, reportProgress, x => x.FreightPayerTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.CityType).Name, reportProgress, x => x.Route2CityTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.Currency).Name, reportProgress, x => x.CurrencyTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.BusienssDescription).Name, reportProgress, x => x.Route2BusinessDescriptionTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.ShipmentType).Name, reportProgress, x => x.ShipmentTypeTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.TranspotUnit).Name, reportProgress, x => x.TransportUnitTypeTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.Route>(_LoaderOptimizationAll, typeof(Linq.SAPDestinationPlant).Name, reportProgress, x => x.SAPDestinationPlantTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Route with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoCity(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoCity");
      List<Linq.CityType> _LoaderOptimizationAll = spedc.City.ToList<Linq.CityType>();
      _breakingIssueEncountered &= Assumption<Linq.CityType>(_LoaderOptimizationAll, typeof(Linq.CountryType).Name, reportProgress, x => x.CountryTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list City with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool DoSecurityEscortRoute(Linq.Entities spedc, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      bool _breakingIssueEncountered = true;
      trace("Entering DoSecurityEscortRoute");
      List<Linq.SecurityEscortCatalog> _LoaderOptimizationAll = spedc.SecurityEscortRoute.ToList<Linq.SecurityEscortCatalog>();
      _breakingIssueEncountered &= Assumption<Linq.SecurityEscortCatalog>(_LoaderOptimizationAll, typeof(Linq.Partner).Name, reportProgress, x => x.PartnerTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.SecurityEscortCatalog>(_LoaderOptimizationAll, typeof(Linq.FreightPayer).Name, reportProgress, x => x.FreightPayerTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.SecurityEscortCatalog>(_LoaderOptimizationAll, typeof(Linq.Currency).Name, reportProgress, x => x.CurrencyTitle == null);
      _breakingIssueEncountered &= Assumption<Linq.SecurityEscortCatalog>(_LoaderOptimizationAll, typeof(Linq.BusienssDescription).Name, reportProgress, x => x.SecurityEscortCatalog2BusinessDescriptionTitle == null);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Finished consistency check of list Security Escort Route with result {0}.", _breakingIssueEncountered ? "Failed" : "Success")));
      return _breakingIssueEncountered;
    }
    private static bool Assumption<TEntity>(List<TEntity> entitiesList, string targetListName, Action<ProgressChangedEventArgs> reportProgress, Func<TEntity, bool> predicate)
      where TEntity : Linq.Item
    {
      List<TEntity> _LoaderOptimizationNoComodity = entitiesList.Where<TEntity>(x => predicate(x)).ToList<TEntity>();
      if (_LoaderOptimizationNoComodity.Count == 0)
        return true;
      string _tmpl = "The following entities: {0}{1} on the list {2} do not have lookup on the list {3}.";
      string _entitiesList = String.Join(", ", _LoaderOptimizationNoComodity.Select<TEntity, String>(x => String.Format("[{0}]", x.Id)).ToArray());
      string _sufix = String.Empty;
      if (_entitiesList.Length > 150)
      {
        _entitiesList = _entitiesList.Remove(150);
        _sufix = ", ...";
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format(_tmpl, _entitiesList, _sufix, typeof(TEntity).Name, targetListName)));
      return false;
    }

    #endregion

  }
}
