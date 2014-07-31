
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// Clarence
  /// </summary>
  public partial class Clearence
  {
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("Status", "SPStatus");
      return _ret;
    }
  }
}