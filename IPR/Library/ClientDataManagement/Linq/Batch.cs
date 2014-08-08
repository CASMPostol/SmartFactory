using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  public partial class Batch
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    /// <returns></returns>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("Batch1", "Batch0");
      return _ret;
    }
  }
}
