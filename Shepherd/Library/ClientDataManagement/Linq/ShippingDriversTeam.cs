using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class ShippingDriversTeam
  /// </summary>
  public partial class ShippingDriversTeam
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("DriverID", "DriverTitle");
      _ret.Add("ShippingID", "ShippingIndex");
      return _ret;
    }
  }
}
