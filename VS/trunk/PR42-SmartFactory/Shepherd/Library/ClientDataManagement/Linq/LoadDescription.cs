using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class LoadDescription
  /// </summary>
  public partial class LoadDescription
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("CommodityID", "LoadDescription2Commodity");
      _ret.Add("MarketID", "MarketTitle");
      _ret.Add("PartnerID", "LoadDescription2PartnerTitle");
      _ret.Add("ShippingID", "LoadDescription2ShippingIndex");
      return _ret;
    }
  }
}