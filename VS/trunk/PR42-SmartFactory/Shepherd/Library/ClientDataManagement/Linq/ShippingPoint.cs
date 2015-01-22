using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class Driver
  /// </summary>
  public partial class ShippingPoint
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("WarehouseID", "WarehouseTitle");
      return _ret;
    }
  }
}
