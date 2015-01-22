using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class DestinationMarket
  /// </summary>
  public partial class DestinationMarket
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("CityID", "DestinationMarket2CityTitle");
      _ret.Add("MarketID", "MarketTitle");
      return _ret;
    }
  }
}
