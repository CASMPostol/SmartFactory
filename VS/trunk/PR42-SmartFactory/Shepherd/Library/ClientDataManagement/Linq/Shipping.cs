using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class Shipping
  /// </summary>
  public partial class Shipping
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("Comments", "CancelationReason");
      _ret.Add("EscortPartnerID", "Shipping2PartnerTitle");
      _ret.Add("SecurityEscortCatalogID", "SecurityEscortCatalogTitle");
      _ret.Add("SecuritySealProtocolID", "SecuritySealProtocolIndex");
      _ret.Add("CityID", "Shipping2City");
      _ret.Add("Currency4AddCostsID", "Shipping2Currency4AddCosts");
      _ret.Add("Currency4CostsPerKUID", "Shipping2Currency4CostsPerKU");
      _ret.Add("CurrencyForEscortID", "Shipping2CurrencyForEscort");
      _ret.Add("CurrencyForFreightID", "Shipping2CurrencyForFreight");
      _ret.Add("EscortPOID", "Shipping2EscortPOIndex");
      _ret.Add("FreightPOID", "Shipping2FreightPOIndex");
      _ret.Add("WarehouseID", "Shipping2WarehouseTitle");
      _ret.Add("PartnerID", "PartnerTitle");
      _ret.Add("RouteID", "Shipping2RouteTitle");
      _ret.Add("TransportUnitTypeID", "Shipping2TransportUnitType");
      _ret.Add("TruckID", "TruckTitle");
      _ret.Add("TrailerID", "TrailerTitle");
      _ret.Add("EscortCarID", "Shipping2TruckTitle");
      return _ret;
    }
  }
}
