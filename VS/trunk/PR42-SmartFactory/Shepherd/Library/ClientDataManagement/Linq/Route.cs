using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class Route
  /// </summary>
  public partial class Route
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("CarrierID", "CarrierTitle");
      _ret.Add("CurrencyID", "CurrencyTitle");
      _ret.Add("FreightPayerID", "FreightPayerTitle");
      _ret.Add("PartnerID", "PartnerTitle");
      _ret.Add("BusinessDescriptionID", "Route2BusinessDescriptionTitle");
      _ret.Add("CityID", "Route2CityTitle");
      _ret.Add("CommodityID", "Route2Commodity");
      _ret.Add("SAPDestinationPlantID", "SAPDestinationPlantTitle");
      _ret.Add("ShipmentTypeID", "ShipmentTypeTitle");
      _ret.Add("TransportUnitTypeID", "TransportUnitTypeTitle");
      return _ret;
    }
  }
}
