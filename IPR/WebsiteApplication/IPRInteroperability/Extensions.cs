using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml
{
  public static class Extensions
  {
    #region Private
    ////public static double? ConvertToDouble(this decimal? val)
    ////{
    ////  return val.HasValue ? new Nullable<Double>(Convert.ToDouble(val)) : new Nullable<Double>();
    ////}
    public static double? ConvertToDouble(this Decimal val, bool specified)
    {
      return specified ? new Nullable<Double>(Convert.ToDouble(val)) : new Nullable<Double>();
    }
    //public static bool NullOrEmpty<type>(this type[] val)
    //{
    //  return val == null || val.Length == 0;
    //}
    #endregion
  }
}
