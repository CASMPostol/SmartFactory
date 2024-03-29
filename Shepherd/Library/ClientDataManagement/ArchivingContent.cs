﻿//<summary>
//  Title   : Name of Application
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
using CAS.SharePoint.Client.SP2SQLInteroperability;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class ArchivingContent - provides service to archive the Shepherd website to SQL backup database.
  /// </summary>
  public static class ArchivingContent
  {
    /// <summary>
    /// Does the archiving of the Shepherd content.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="sqlConnectionString">The SQL connection string.</param>
    /// <param name="archivalDelay">The archival delay.</param>
    /// <param name="rowLimit">The row limit.</param>
    /// <param name="reportProgress">An action to trace progress in a log stream.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    public static void DoArchivingContent(string URL, string sqlConnectionString, int archivalDelay, int rowLimit, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoArchivingContent URL: {0}, connection string: {1}, ArchivalDelay: {2}, RowLimit: {3}", URL, sqlConnectionString, archivalDelay, rowLimit)));
      trace("Establishing connection with the SP site and SQL database to execute ArchiveShipping.");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveShipping(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
      }
      trace("Establishing connection with the SP site and SQL database to execute ArchiveDictionaries .");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveDictionaries(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
      }
      trace("Establishing connection with the SP site and SQL database to execute ArchiveRouts.");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveRouts(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
      }
      trace("Establishing connection with the SP site and SQL database to execute ArchiveBusinessDescription.");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveBusinessDescription(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
      }
      trace("Establishing connection with the SP site and SQL database to execute ArchiveCarrierPerformanceReport.");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y =>trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveCarrierPerformanceReport(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
      }
      //Business Description
      reportProgress(new ProgressChangedEventArgs(1, "Updating Activities Logs"));
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
        ArchivingOperationLogs.UpdateActivitiesLogs<Linq2SQL.ArchivingOperationLogs>(_sqledc, ArchivingOperationLogs.OperationName.Archiving, reportProgress);
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoArchivingContent"));
    }

    private static void ArchiveCarrierPerformanceReport(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      //CarrierPerformanceReport
      trace("Loading the List CarrierPerformanceReport at ArchiveCarrierPerformanceReport");
      List<Linq.CarrierPerformanceReport> _CarrierPerformanceReport2BeDeleted =
        spedc.CarrierPerformanceReport.ToList<Linq.CarrierPerformanceReport>().Where<Linq.CarrierPerformanceReport>(x => x.Created.IsLatter(archivalDelay)).ToList<Linq.CarrierPerformanceReport>();
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} CarrierPerformanceReport entries to be deleted.", _CarrierPerformanceReport2BeDeleted.Count)));
      spedc.CarrierPerformanceReport.Delete<Linq.CarrierPerformanceReport, Linq2SQL.History>
         (_CarrierPerformanceReport2BeDeleted, null, x => sqledc.CarrierPerformanceReport.GetAt<Linq2SQL.CarrierPerformanceReport>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      trace("SubmitChanges for the list: CarrierPerformanceReport have been archived.");
      CAS.SharePoint.Client.SP2SQLInteroperability.Extensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      reportProgress(new ProgressChangedEventArgs(1, "The list: CarrierPerformanceReport have been archived."));
    }
    private static void ArchiveBusinessDescription(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      //BusienssDescription
      trace("Loading the List BusienssDescription at ArchiveBusinessDescription");
      List<Linq.BusienssDescription> _BusinessDescription2BeDeleted = 
        spedc.BusinessDescription.ToList<Linq.BusienssDescription>().Where<Linq.BusienssDescription>(x => x.Route.Count == 0 && x.SecurityEscortCatalog.Count == 0).ToList<Linq.BusienssDescription>();
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} BusienssDescription entries to be deleted.", _BusinessDescription2BeDeleted.Count)));
      spedc.BusinessDescription.Delete<Linq.BusienssDescription, Linq2SQL.History>
         (_BusinessDescription2BeDeleted, null, x => sqledc.BusinessDescription.GetAt<Linq2SQL.BusinessDescription>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      trace("SubmitChanges for the list: BusienssDescription have been archived.");
      CAS.SharePoint.Client.SP2SQLInteroperability.Extensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      reportProgress(new ProgressChangedEventArgs(1, "The list: BusienssDescription have been archived."));
    }
    private static void ArchiveRouts(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      int _year = 365;
      reportProgress(new ProgressChangedEventArgs(1, "Starting archive the lists: Route, SecurityEscortCatalog"));
      //Route
      trace("Loading the List Route at ArchiveRouts");
      List<Linq.Route> _Route2BeDeleted = spedc.Route.ToList<Linq.Route>().Where<Linq.Route>(x => x.Shipping.Count == 0 && x.Created.IsLatter(new TimeSpan(_year, 0, 0, 0).Days + archivalDelay)).ToList<Linq.Route>();
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Route entries to be deleted.", _Route2BeDeleted.Count)));
      spedc.Route.Delete<Linq.Route, Linq2SQL.History>
         (_Route2BeDeleted, null, x => sqledc.Route.GetAt<Linq2SQL.Route>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      //SecurityEscortCatalog
      trace("Loading the List Route at ArchiveRouts");
      List<Linq.SecurityEscortCatalog> _SecurityEscortCatalog2BeDeleted =
        spedc.SecurityEscortRoute.ToList<Linq.SecurityEscortCatalog>().Where<Linq.SecurityEscortCatalog>(x => x.Shipping.Count == 0 && x.Created.IsLatter(new TimeSpan(_year, 0, 0, 0).Days + archivalDelay)).ToList<Linq.SecurityEscortCatalog>();
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Route entries to be deleted.", _SecurityEscortCatalog2BeDeleted.Count)));
      spedc.SecurityEscortRoute.Delete<Linq.SecurityEscortCatalog, Linq2SQL.History>
         (_SecurityEscortCatalog2BeDeleted, null, x => sqledc.SecurityEscortRoute.GetAt<Linq2SQL.SecurityEscortRoute>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      trace("SubmitChanges for the lists: Route, SecurityEscortCatalog have been archived.");
      CAS.SharePoint.Client.SP2SQLInteroperability.Extensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      reportProgress(new ProgressChangedEventArgs(1, "The lists: Route, SecurityEscortCatalog have been archived."));
    }
    /// <summary>
    /// Archives the dictionaries.
    /// </summary>
    /// <param name="spedc">The <see cref="Linq.Entities"/> representing SharePoint application content database</param>
    /// <param name="sqledc">The <see cref="Linq2SQL.SHRARCHIVE"/> representing the SQL database containing backup of SharePoint content.</param>
    /// <param name="archivalDelay">The archival delay.</param>
    /// <param name="reportProgress">The report progress.</param>
    /// <param name="trace">The trace.</param>
    private static void ArchiveDictionaries(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, "Starting archive the lists: Truck, Trailer, Driver"));
      //Truck
      List<Linq.Truck> _Truck2MarketDeleted = spedc.Truck.ToList<Linq.Truck>().Where<Linq.Truck>(x => x.ToBeDeleted.GetValueOrDefault(false)).ToList<Linq.Truck>();
      trace(String.Format("List of Truck loaded and contains {0} items.", _Truck2MarketDeleted.Count));
      List<Linq.Truck> _Truck2BeDeleted = new List<Linq.Truck>();
      foreach (Linq.Truck _TruckItem in _Truck2MarketDeleted)
        if (_TruckItem.Shipping.Count == 0 && _TruckItem.Shipping0.Count == 0)
          _Truck2BeDeleted.Add(_TruckItem);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Truck entries to be deleted.", _Truck2BeDeleted.Count)));
      spedc.Truck.Delete<Linq.Truck, Linq2SQL.History>
         (_Truck2BeDeleted, null, x => sqledc.Truck.GetAt<Linq2SQL.Truck>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      _Truck2MarketDeleted = null;
      _Truck2BeDeleted = null;
      //Trailer
      List<Linq.Trailer> _Trailer2MarketDeleted = spedc.Trailer.ToList<Linq.Trailer>().Where<Linq.Trailer>(x => x.ToBeDeleted.GetValueOrDefault(false)).ToList<Linq.Trailer>();
      trace(String.Format("List of Trailer loaded and contains {0} items.", _Trailer2MarketDeleted.Count));
      List<Linq.Trailer> _Trailer2BeDeleted = new List<Linq.Trailer>();
      foreach (Linq.Trailer _TrailerItem in _Trailer2MarketDeleted)
        if (_TrailerItem.Shipping.Count == 0)
          _Trailer2BeDeleted.Add(_TrailerItem);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Trailer entries to be deleted.", _Trailer2BeDeleted.Count)));
      spedc.Trailer.Delete<Linq.Trailer, Linq2SQL.History>
         (_Trailer2BeDeleted, null, x => sqledc.Trailer.GetAt<Linq2SQL.Trailer>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      _Trailer2MarketDeleted = null;
      _Trailer2BeDeleted = null;
      //Driver
      List<Linq.Driver> _DriverMarketDeleted = spedc.Driver.ToList<Linq.Driver>().Where<Linq.Driver>(x => x.ToBeDeleted.GetValueOrDefault(false)).ToList<Linq.Driver>();
      trace(String.Format("List of Driver loaded and contains {0} items.", _DriverMarketDeleted.Count));
      List<Linq.Driver> _Driver2BeDeleted = new List<Linq.Driver>();
      foreach (Linq.Driver _DriverItem in _DriverMarketDeleted)
        if (_DriverItem.ShippingDriversTeam.Count == 0)
          _Driver2BeDeleted.Add(_DriverItem);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Driver entries to be deleted.", _Driver2BeDeleted.Count)));
      spedc.Driver.Delete<Linq.Driver, Linq2SQL.History>
         (_Driver2BeDeleted, null, x => sqledc.Driver.GetAt<Linq2SQL.Driver>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      trace("SubmitChanges for the lists: Truck, Trailer, Driver have been archived.");
      CAS.SharePoint.Client.SP2SQLInteroperability.Extensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      reportProgress(new ProgressChangedEventArgs(1, "The lists: Truck, Trailer, Driver have been archived."));
    }
    private static void ArchiveShipping(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, "Starting archive the lists: TimeSlot, AlarmsAndEvents, LoadDescription, DriversTeam, Shipping"));
      trace("Starting GoShipping");
      List<Linq.Shipping> _Shipping2Delete = spedc.Shipping.ToList<Linq.Shipping>().Where<Linq.Shipping>(x => (x.ShippingState == Linq.ShippingState.Completed ||
                                                                                                               x.ShippingState == Linq.ShippingState.Canceled) &&
                                                                                                               x.EndTime.IsLatter(archivalDelay)).ToList<Linq.Shipping>();
      trace(String.Format("List of Shipping loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.TimeSlotTimeSlot> _TimeSlot2Delete = new List<Linq.TimeSlotTimeSlot>();
      trace(String.Format("List of TimeSlotTimeSlot loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.AlarmsAndEvents> _AlarmsAndEvents2Delete = new List<Linq.AlarmsAndEvents>();
      trace(String.Format("List of AlarmsAndEvents loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.LoadDescription> _LoadDescription2Delete = new List<Linq.LoadDescription>();
      trace(String.Format("List of LoadDescription loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.ShippingDriversTeam> _ShippingDriversTeam2Delete = new List<Linq.ShippingDriversTeam>();
      trace(String.Format("List of ShippingDriversTeam loaded and contains {0} items.", _Shipping2Delete.Count));
      //TODO http://casas:11227/sites/awt/_layouts/listform.aspx?PageType=4&ListId={72C511B5-8B63-4DFA-AD34-133A97EBA469}&ID=4568&ContentTypeID=0x01005D39260836CE498D8E0D443AD5CAD3AC00456AB372ACF9DA41B8AE870CD1954927
      //TODO workaround to avoid usage of the reverse lookup for doubled type definition.
      List<Linq.TimeSlotTimeSlot> _TimeSlotTimeSlotAll = spedc.TimeSlot.ToList<Linq.TimeSlotTimeSlot>(); ;
      foreach (Linq.Shipping _shipping in _Shipping2Delete)
      {
        _TimeSlot2Delete.AddRange(_TimeSlotTimeSlotAll.Where<Linq.TimeSlotTimeSlot>(x => x.TimeSlot2ShippingIndex == _shipping));
        _AlarmsAndEvents2Delete.AddRange(_shipping.AlarmsAndEvents);
        _LoadDescription2Delete.AddRange(_shipping.LoadDescription);
        _ShippingDriversTeam2Delete.AddRange(_shipping.ShippingDriversTeam);
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} TimeSlot entries to be deleted.", _TimeSlot2Delete.Count)));
      spedc.TimeSlot.Delete<Linq.TimeSlotTimeSlot, Linq2SQL.History>
         (_TimeSlot2Delete, null, x => null, (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} AlarmsAndEvents entries to be deleted.", _AlarmsAndEvents2Delete.Count)));
      spedc.AlarmsAndEvents.Delete<Linq.AlarmsAndEvents, Linq2SQL.History>
         (_AlarmsAndEvents2Delete, null, x => null, (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} LoadDescription entries to be deleted.", _LoadDescription2Delete.Count)));
      spedc.LoadDescription.Delete<Linq.LoadDescription, Linq2SQL.History>
         (_LoadDescription2Delete, null, x => sqledc.LoadDescription.GetAt<Linq2SQL.LoadDescription>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} DriversTeam entries to be deleted.", _ShippingDriversTeam2Delete.Count)));
      spedc.DriversTeam.Delete<Linq.ShippingDriversTeam, Linq2SQL.History>
         (_ShippingDriversTeam2Delete, null, x => sqledc.DriversTeam.GetAt<Linq2SQL.DriversTeam>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Shipping entries to be deleted.", _Shipping2Delete.Count)));
      spedc.Shipping.Delete<Linq.Shipping, Linq2SQL.History>
         (_Shipping2Delete, null, x => sqledc.Shipping.GetAt<Linq2SQL.Shipping>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      trace("SubmitChanges for the lists: Truck, Trailer, Driver have been archived.");
      CAS.SharePoint.Client.SP2SQLInteroperability.Extensions.SubmitChanges(spedc, sqledc, (x, y) => reportProgress(y));
      reportProgress(new ProgressChangedEventArgs(1, "The lists: TimeSlot, AlarmsAndEvents, LoadDescription, DriversTeam, Shipping have been archived."));
    }

  }
}
