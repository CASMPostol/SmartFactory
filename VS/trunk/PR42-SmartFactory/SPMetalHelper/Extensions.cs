﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CAS.SmartFactory
{
  /// <summary>
  /// Extensions
  /// </summary>
  public static class Extensions
  {
    #region public
    /// <summary>
    /// Convert int to string.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string IntToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents <see cref="DateTime"/>.
    /// </summary>
    /// <param name="_val">The value to convert</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val)
    {
      return _val.HasValue ? _val.Value.ToString(CultureInfo.CurrentUICulture) : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <param name="_format">The _format.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val, string _format)
    {
      return _val.HasValue ? string.Format(_format, _val.Value.ToString(CultureInfo.CurrentUICulture)) : String.Empty;
    }
    public static const DateTime SPMinimum = new DateTime(1900, 1, 1);
    internal const string UnknownEmail = "unknown@comapny.com";
    public static string UnknownIfEmpty(this String _val)
    {
      return String.IsNullOrEmpty(_val) ? UnknownEmail : _val;
    }
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return new Nullable<int>();
    }
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
    ///   true if the value parameter is null or an empty string (""); otherwise, false.</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    public static string SPValidSubstring(this string _value)
    {
      string _goodsDescription = Microsoft.SharePoint.Utilities.SPStringUtility.RemoveControlChars(_value);
      int _gdl = _goodsDescription.Length;
      do
      {
        _goodsDescription = _goodsDescription.Replace("  ", " ");
        if (_gdl == _goodsDescription.Length)
          break;
        _gdl = _goodsDescription.Length;
      } while (true);
      int SPStringMAxLength = 250;
      if (_gdl >= SPStringMAxLength)
        _goodsDescription = _goodsDescription.Remove(SPStringMAxLength);
      return _goodsDescription;
    }
    /// <summary>
    /// Gets the first capture.
    /// </summary>
    /// <param name="_input">The string to be tested for a match. <see cref="string"/>.</param>
    /// <param name="_pattern">The regular expression pattern to match..</param>
    /// <returns>The string that match the patern</returns>
    public static string GetFirstCapture(this string _input, string _pattern)
    {
      Match _match = Regex.Match(_input, _pattern, RegexOptions.IgnoreCase);
      if (_match.Success)
        return _match.Captures[0].Value;
      else
        return "-- unrecognized name --";
    }
    #endregion
  }
}
