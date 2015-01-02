//<summary>
//  Title   : Extensions
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  internal static class Extensions
  {
    /// <summary>
    /// String2s the double.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static double? String2Double(this string _val)
    {
      double _res = 0;
      if (double.TryParse(_val, out _res))
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
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    /// <summary>
    /// Convert the <see cref="Nullable{DateTime}"/> to localized string.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>Localized <see cref="System.String"/>.</returns>
    public static string LocalizedString(this Nullable<DateTime> value)
    {
      return value.HasValue ? value.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture) : Properties.Settings.Default.RunDateUnknown;
    }
    /// <summary>
    /// Gets the value or default.
    /// </summary>
    /// <param name="value">The value if not bull or empty.</param>
    /// <param name="defaultString">The default string returned if <paramref name="value"/> is null or default.</param>
    public static string GetValueOrDefault(this string value, string defaultString)
    {
      return string.IsNullOrEmpty(value) ? defaultString : value;
    }

  }
}
