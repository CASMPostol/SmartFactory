//<summary>
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
      trace("Establishing connection with the SP site and SQL database for ArchiveShipping .");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveShipping(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
        ArchivingOperationLogs.UpdateActivitiesLogs<Linq2SQL.ArchivingOperationLogs>(_sqledc, ArchivingOperationLogs.OperationName.Archiving, reportProgress);
      }
      trace("Establishing connection with the SP site and SQL database for ArchiveDictionaries .");
      using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
      {
        using (Linq.Entities _spedc = new Linq.Entities(trace, URL))
        {
          _spedc.RowLimit = rowLimit;
          ArchiveDictionaries(_spedc, _sqledc, archivalDelay, reportProgress, trace);
        }
        ArchivingOperationLogs.UpdateActivitiesLogs<Linq2SQL.ArchivingOperationLogs>(_sqledc, ArchivingOperationLogs.OperationName.Archiving, reportProgress);
      }
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoArchivingContent"));
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
      List<Linq.Truck> _Truck2MarketDeleted = spedc.Truck.ToList<Linq.Truck>().Where<Linq.Truck>(x => x.ToBeDeleted.Value).ToList<Linq.Truck>();
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
      List<Linq.Trailer> _Trailer2MarketDeleted = spedc.Trailer.ToList<Linq.Trailer>().Where<Linq.Trailer>(x => x.ToBeDeleted.Value).ToList<Linq.Trailer>();
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
      List<Linq.Driver> _DriverMarketDeleted = spedc.Driver.ToList<Linq.Driver>().Where<Linq.Driver>(x => x.ToBeDeleted.Value).ToList<Linq.Driver>();
      trace(String.Format("List of Driver loaded and contains {0} items.", _DriverMarketDeleted.Count));
      List<Linq.Driver> _Driver2BeDeleted = new List<Linq.Driver>();
      foreach (Linq.Driver _DriverItem in _DriverMarketDeleted)
        if (_DriverItem.ShippingDriversTeam.Count == 0)
          _Driver2BeDeleted.Add(_DriverItem);
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Driver entries to be deleted.", _Trailer2BeDeleted.Count)));
      spedc.Driver.Delete<Linq.Driver, Linq2SQL.History>
         (_Driver2BeDeleted, null, x => sqledc.Driver.GetAt<Linq2SQL.Driver>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, "The lists: Truck, Trailer, Driver have been archived."));
    }
    private static void ArchiveShipping(Linq.Entities spedc, Linq2SQL.SHRARCHIVE sqledc, int archivalDelay, Action<ProgressChangedEventArgs> reportProgress, Action<string> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, "Starting archive the lists: TimeSlot, AlarmsAndEvents, LoadDescription, DriversTeam, Shipping"));
      trace("Starting GoShipping");
      List<Linq.Shipping> _Shipping2Delete = spedc.Shipping.ToList<Linq.Shipping>().Where<Linq.Shipping>(x => (x.ShippingState2 == Linq.ShippingState2.Completed ||
                                                                                                               x.ShippingState2 == Linq.ShippingState2.Canceled) &&
                                                                                                               x.TSEndTime.IsLatter(archivalDelay)).ToList<Linq.Shipping>();
      trace(String.Format("List of Shipping loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.TimeSlotTimeSlot> _TimeSlot2Delete = new List<Linq.TimeSlotTimeSlot>();
      trace(String.Format("List of TimeSlotTimeSlot loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.AlarmsAndEvents> _AlarmsAndEvents2Delete = new List<Linq.AlarmsAndEvents>();
      trace(String.Format("List of AlarmsAndEvents loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.LoadDescription> _LoadDescription2Delete = new List<Linq.LoadDescription>();
      trace(String.Format("List of LoadDescription loaded and contains {0} items.", _Shipping2Delete.Count));
      List<Linq.ShippingDriversTeam> _ShippingDriversTeam2Delete = new List<Linq.ShippingDriversTeam>();
      trace(String.Format("List of ShippingDriversTeam loaded and contains {0} items.", _Shipping2Delete.Count));
      foreach (Linq.Shipping _shipping in _Shipping2Delete)
      {
        _TimeSlot2Delete.AddRange(_shipping.TimeSlot.ToList<Linq.TimeSlot>().Cast<Linq.TimeSlotTimeSlot>());
        _AlarmsAndEvents2Delete.AddRange(_shipping.AlarmsAndEvents);
        _LoadDescription2Delete.AddRange(_shipping.LoadDescription);
        _ShippingDriversTeam2Delete.AddRange(_shipping.ShippingDriversTeam);
      }
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} TimeSlot entries to be deleted.", _TimeSlot2Delete.Count)));
      spedc.TimeSlot.Delete<Linq.TimeSlotTimeSlot, Linq2SQL.History>
         (_TimeSlot2Delete, null, x => null , (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} AlarmsAndEvents entries to be deleted.", _TimeSlot2Delete.Count)));
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
      reportProgress(new ProgressChangedEventArgs(1, String.Format("There are {0} Shipping entries to be deleted.", _ShippingDriversTeam2Delete.Count)));
      spedc.Shipping.Delete<Linq.Shipping, Linq2SQL.History>
         (_Shipping2Delete, null, x => sqledc.Shipping.GetAt<Linq2SQL.Shipping>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          x => sqledc.History.AddHistoryEntry(x));
      reportProgress(new ProgressChangedEventArgs(1, "The lists: TimeSlot, AlarmsAndEvents, LoadDescription, DriversTeam, Shipping have been archived."));
    }

  }
}
