
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  
  public partial class Announcement
  {
    internal override Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = base.GetMappings();
      _ret.Add("SPPropert", "SQLProperty");
      return _ret;
    }
  }
}
