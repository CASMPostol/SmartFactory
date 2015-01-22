
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class TimeSlotTimeSlot
  /// </summary>
  public partial class TimeSlotTimeSlot
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("ShippingID", "TimeSlot2ShippingIndex");
      _ret.Add("ShippingPointID", "TimeSlot2ShippingPointLookup");
      return _ret;
    }
  }
}

