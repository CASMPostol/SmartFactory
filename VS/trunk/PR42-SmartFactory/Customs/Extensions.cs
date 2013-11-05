//<summary>
//  Title   : public static class Extensions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SmartFactory.Customs
{
  /// <summary>
  /// public static class Extensions
  /// </summary>
  public static class Extensions
  {
    #region Private
    /// <summary>
    /// Converts double to decimal.
    /// </summary>
    /// <param name="val">The value.</param>
    public static decimal ConvertToDecimal( this double? val )
    {
      return Convert.ToDecimal( val.GetValueOrDefault(0) ) ;
    }
    /// <summary>
    /// Converts decimal to  double.
    /// </summary>
    /// <param name="val">The value.</param>
    public static double? ConvertToDouble(this decimal? val)
    {
      return val.HasValue ? new Nullable<Double>(Convert.ToDouble(val)) : new Nullable<Double>();
    }
    /// <summary>
    /// Converts the Decimal to double.
    /// </summary>
    /// <param name="val">The value.</param>
    /// <param name="specified">if set to <c>true</c> the is specified.</param>
    public static double? ConvertToDouble(this Decimal val, bool specified)
    {
      return specified ? new Nullable<Double>(Convert.ToDouble(val)) : new Nullable<Double>();
    }
    /// <summary>
    /// Check if the value is null or empty (Length == 0).
    /// </summary>
    /// <typeparam name="type">The type of the base type of the array.</typeparam>
    /// <param name="val">The value.</param>
    /// <returns>true if val== null or has length of 0.</returns>
    public static bool NullOrEmpty<type>(this type[] val)
    {
      return val == null || val.Length == 0;
    }
    #endregion
  }
}
