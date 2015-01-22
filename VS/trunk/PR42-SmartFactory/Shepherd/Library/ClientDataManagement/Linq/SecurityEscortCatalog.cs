using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class SecurityEscortCatalog
  /// </summary>
  public partial class SecurityEscortCatalog
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("CurrencyID", "CurrencyTitle");
      _ret.Add("PartnerID", "PartnerTitle");
      _ret.Add("FreightPayerID", "FreightPayerTitle");
      _ret.Add("BusinessDescriptionID", "SecurityEscortCatalog2BusinessDescriptionTitle");
      return _ret;
    }
  }
}
