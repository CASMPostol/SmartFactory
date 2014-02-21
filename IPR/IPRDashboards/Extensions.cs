using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Dashboards
{
  public static class Extensions
  {
    #region public

    public static string GetIPRLocalizationExpresion(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, GlobalDefinitions.IPRResourceFileName, val);
    }
    #endregion
  }
}
