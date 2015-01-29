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
          Synchronizer.Synchronize<Linq2SQL.Warehouse, Linq.Warehouse>(_sqledc.Warehouse, _edc.Warehouse, (x, y) => reportProgress(y), Linq.Warehouse.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Partner, Linq.Partner>(_sqledc.Partner, _edc.Partner, (x, y) => reportProgress(y), Linq.Partner.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Currency, Linq.Currency>(_sqledc.Currency, _edc.Currency, (x, y) => reportProgress(y), Linq.Currency.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.FreightPayer, Linq.FreightPayer>(_sqledc.FreightPayer, _edc.FreightPayer, (x, y) => reportProgress(y), Linq.FreightPayer.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.BusinessDescription, Linq.BusienssDescription>(_sqledc.BusinessDescription, _edc.BusinessDescription, (x, y) => reportProgress(y), Linq.BusienssDescription.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.SecurityEscortRoute, Linq.SecurityEscortCatalog>(_sqledc.SecurityEscortRoute, _edc.SecurityEscortRoute, (x, y) => reportProgress(y), Linq.SecurityEscortCatalog.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.SealProtocolLibrary, Linq.SealProtocol>(_sqledc.SealProtocolLibrary, _edc.SealProtocolLibrary, (x, y) => reportProgress(y), Linq.SealProtocol.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Country, Linq.CountryType>(_sqledc.Country, _edc.Country, (x, y) => reportProgress(y), Linq.CountryType.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.City, Linq.CityType>(_sqledc.City, _edc.City, (x, y) => reportProgress(y), Linq.CityType.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.EscortPOLibrary, Linq.EscortPO>(_sqledc.EscortPOLibrary, _edc.EscortPOLibrary, (x, y) => reportProgress(y), Linq.EscortPO.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.FreightPOLibrary, Linq.FreightPO>(_sqledc.FreightPOLibrary, _edc.FreightPOLibrary, (x, y) => reportProgress(y), Linq.FreightPO.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Carrier, Linq.CarrierType>(_sqledc.Carrier, _edc.Carrier, (x, y) => reportProgress(y), Linq.CarrierType.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.SAPDestinationPlant, Linq.SAPDestinationPlant>(_sqledc.SAPDestinationPlant, _edc.SAPDestinationPlant, (x, y) => reportProgress(y), Linq.SAPDestinationPlant.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.ShipmentType, Linq.ShipmentType>(_sqledc.ShipmentType, _edc.ShipmentType, (x, y) => reportProgress(y), Linq.ShipmentType.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.TransportUnitType, Linq.TranspotUnit>(_sqledc.TransportUnitType, _edc.TransportUnitType, (x, y) => reportProgress(y), Linq.TranspotUnit.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Route, Linq.Route>(_sqledc.Route, _edc.Route, (x, y) => reportProgress(y), Linq.Route.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Truck, Linq.Truck>(_sqledc.Truck, _edc.Truck, (x, y) => reportProgress(y), Linq.Truck.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Trailer, Linq.Trailer>(_sqledc.Trailer, _edc.Trailer, (x, y) => reportProgress(y), Linq.Trailer.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Shipping, Linq.Shipping>(_sqledc.Shipping, _edc.Shipping, (x, y) => reportProgress(y), Linq.Shipping.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.CarrierPerformanceReport, Linq.CarrierPerformanceReport>(_sqledc.CarrierPerformanceReport, _edc.CarrierPerformanceReport, (x, y) => reportProgress(y), Linq.CarrierPerformanceReport.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Market, Linq.Market>(_sqledc.Market, _edc.Market, (x, y) => reportProgress(y), Linq.Market.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.DestinationMarket, Linq.DestinationMarket>(_sqledc.DestinationMarket, _edc.DestinationMarket, (x, y) => reportProgress(y), Linq.DestinationMarket.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.Driver, Linq.Driver>(_sqledc.Driver, _edc.Driver, (x, y) => reportProgress(y), Linq.Driver.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.DriversTeam, Linq.ShippingDriversTeam>(_sqledc.DriversTeam, _edc.DriversTeam, (x, y) => reportProgress(y), Linq.ShippingDriversTeam.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.LoadDescription, Linq.LoadDescription>(_sqledc.LoadDescription, _edc.LoadDescription, (x, y) => reportProgress(y), Linq.LoadDescription.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.ShippingPoint, Linq.ShippingPoint>(_sqledc.ShippingPoint, _edc.ShippingPoint, (x, y) => reportProgress(y), Linq.ShippingPoint.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.ScheduleTemplate, Linq.ScheduleTemplate>(_sqledc.ScheduleTemplate, _edc.ScheduleTemplate, (x, y) => reportProgress(y), Linq.ScheduleTemplate.GetMappings(), false);
          Synchronizer.Synchronize<Linq2SQL.TimeSlotsTemplate, Linq.TimeSlotsTemplate>(_sqledc.TimeSlotsTemplate, _edc.TimeSlotsTemplate, (x, y) => reportProgress(y), Linq.TimeSlotsTemplate.GetMappings(), false); //TODO MP

        }
        using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
          CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.UpdateActivitiesLogs<Linq2SQL.ArchivingOperationLogs>(_sqledc, CAS.SharePoint.Client.Link2SQL.ArchivingOperationLogs.OperationName.Synchronization, reportProgress);
        reportProgress(new ProgressChangedEventArgs(1, "Finished DoSynchronizationContent"));
      }
    }
  }
}
