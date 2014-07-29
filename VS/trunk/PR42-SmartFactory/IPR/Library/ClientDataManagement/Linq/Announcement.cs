﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{

  /// <summary>
  /// Announcement
  /// </summary>
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
