//<summary>
//  Title   : SynchronizationContent
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
using System.ComponentModel;
using CAS.SharePoint.Client.SP2SQLInteroperability;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class SynchronizationContent provides service to synchronize SQL backup database and SharePoint content.
  /// </summary>
  public static class SynchronizationContent
  {
    /// <summary>
    /// Runs the synchronization of the SharePoint and SQL.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="sqlConnectionString">The SQL connection string.</param>
    /// <param name="reportProgress">An action to trace progress in a log stream.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    public static void DoSynchronizationContent(string URL, string sqlConnectionString, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoSynchronizationContent URL: {0}, connection string: {1}", URL, sqlConnectionString)));
      trace("Establishing connection with the SP site and SQL database.");
      using (Linq.Entities _edc = new Linq.Entities(trace, URL))
      {
        using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
        {
          Synchronizer.Synchronize<Linq2SQL.Commodity, Linq.Commodity>(_sqledc.Commodity, _edc.Commodity, (x, y) => reportProgress(y), Linq.Commodity.GetMappings(), false);
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
          //Market
          //DestinationMarket
          //Driver
          //DriversTeam
          //LoadDescription
          //ShippingPoint
          //ScheduleTemplate
          //TimeSlot
          //TimeSlotsTemplate

        }
      }
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoSynchronizationContent"));
    }
  }
}
