using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.SharePoint.Client.SP2SQLInteroperability;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Tests
{
  [TestClass]
  public class StorageInfoComparisonTests
  {
    [TestMethod]
    public void StorageInfoComparisonTestMethod()
    {
      Synchronizer.CompareSelectedStoragesContent<Linq.BusienssDescription, Linq2SQL.BusinessDescription>(Linq.BusienssDescription.GetMappings()); //TODO Documents fields must be resolved
      Synchronizer.CompareSelectedStoragesContent<Linq.CarrierPerformanceReport, Linq2SQL.CarrierPerformanceReport>(Linq.CarrierPerformanceReport.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.CarrierType, Linq2SQL.Carrier>(Linq.CarrierType.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.CityType, Linq2SQL.City>(Linq.CityType.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Commodity, Linq2SQL.Commodity>(Linq.Commodity.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.CountryType, Linq2SQL.Country>(Linq.CountryType.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Currency, Linq2SQL.Currency>(Linq.Currency.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.DestinationMarket, Linq2SQL.DestinationMarket>(Linq.DestinationMarket.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Driver, Linq2SQL.Driver>(Linq.Driver.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.EscortPOLibraryEscortPO, Linq2SQL.EscortPOLibrary>(Linq.EscortPOLibraryEscortPO.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.FreightPayer, Linq2SQL.FreightPayer>(Linq.FreightPayer.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.FreightPOLibraryFreightPO, Linq2SQL.FreightPOLibrary>(Linq.FreightPOLibraryFreightPO.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.LoadDescription, Linq2SQL.LoadDescription>(Linq.LoadDescription.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Market, Linq2SQL.Market>(Linq.Market.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Partner, Linq2SQL.Partner>(Linq.Partner.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Route, Linq2SQL.Route>(Linq.Route.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.SAPDestinationPlant, Linq2SQL.SAPDestinationPlant>(Linq.SAPDestinationPlant.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.ScheduleTemplate, Linq2SQL.ScheduleTemplate>(Linq.ScheduleTemplate.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.SealProtocolLibrarySealProtocol, Linq2SQL.SealProtocolLibrary>(Linq.SealProtocolLibrarySealProtocol.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.SecurityEscortCatalog, Linq2SQL.SecurityEscortRoute>(Linq.SecurityEscortCatalog.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.ShipmentType, Linq2SQL.ShipmentType>(Linq.ShipmentType.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.ShippingDriversTeam, Linq2SQL.DriversTeam>(Linq.ShippingDriversTeam.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.ShippingPoint, Linq2SQL.ShippingPoint>(Linq.ShippingPoint.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.ShippingShipping, Linq2SQL.Shipping>(Linq.ShippingShipping.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.TimeSlotsTemplate, Linq2SQL.TimeSlotsTemplate>(Linq.TimeSlotsTemplate.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.TimeSlotTimeSlot, Linq2SQL.TimeSlot>(Linq.TimeSlotTimeSlot.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Trailer, Linq2SQL.Trailer>(Linq.Trailer.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.TranspotUnit, Linq2SQL.TransportUnitType>(Linq.TranspotUnit.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Truck, Linq2SQL.Truck>(Linq.Truck.GetMappings());
      Synchronizer.CompareSelectedStoragesContent<Linq.Warehouse, Linq2SQL.Warehouse>(Linq.Warehouse.GetMappings());
    }
  }
}
