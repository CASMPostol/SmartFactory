
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// Clearence
  /// </summary>
  public partial class Clearence
  {
    internal override Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = base.GetMappings();
      _ret.Add("Status", "SPStatus");
      return _ret;
    }
  }
}