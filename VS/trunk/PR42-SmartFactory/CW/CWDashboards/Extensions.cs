using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.Dashboards
{
  /// <summary>
  /// class Extensions
  /// </summary>
  public static class Extensions
  {
    #region public

    /// <summary>
    /// Gets the ipr localization expression.
    /// </summary>
    /// <param name="val">The value.</param>
    /// <returns></returns>
    public static string GetCWLocalizationExpression(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, GlobalDefinitions.CWResourceFileName, val);
    }
    #endregion
  }
}
