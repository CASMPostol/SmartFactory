﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// SADGood
  /// </summary>
  public partial class SADGood
  {
    internal override Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = base.GetMappings();
      _ret.Add("Procedure", "SPProcedure");
      return _ret;
    }
  }
}
