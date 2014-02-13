using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS.SmartFactory.Shepherd.RouteEditor
{
  internal static class Extensions
  {
    /// <summary>
    /// String2s the double.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static double? String2Double( this string _val )
    {
      double _res = 0;
      if ( double.TryParse( _val, out _res ) )
        return _res;
      else
        return null;
    }
    /// <summary>
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty( this string _val )
    {
      return String.IsNullOrEmpty( _val );
    }
  }
}
