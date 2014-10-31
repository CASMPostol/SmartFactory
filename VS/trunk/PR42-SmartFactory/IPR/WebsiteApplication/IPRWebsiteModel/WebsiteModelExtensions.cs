//<summary>
//  Title   : WebsiteModelExtensions
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

namespace CAS.SmartFactory.IPR.WebsiteModel
{
  /// <summary>
  /// Website Model Extensions
  /// </summary>
  public static class WebsiteModelExtensions
  {
    /// <summary>
    /// Rounds a double-precision floating-point value to a specified number of fractional digits or returns the default value.
    /// </summary>
    /// <param name="value"> A  <see cref="double"/> number to be rounded.</param>
    /// <returns></returns>
    public static decimal Rount2Double(this double value)
    {
      return Convert.ToDecimal(Math.Round(value, 2));
    }
    /// <summary>
    /// Rounds a double-precision floating-point value to a specified number of fractional digits or returns the default value.
    /// </summary>
    /// <param name="value"> A  <see cref="double"/> number to be rounded.</param>
    /// <returns></returns>
    public static decimal Rount2DecimalOrDefault(this double? value)
    {
      return Convert.ToDecimal(Math.Round(value.GetValueOrDefault(), 2));
    }
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double Rount2Decimals(this double value)
    {
      return Math.Round(value, 2);
    }
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static decimal Rount2Decimals(this decimal value)
    {
      return Math.Round(value, 2);
    }
    /// <summary>
    ///   Converts the value of the specified decimal number to an equivalent double-precision floating-point 
    ///   number and rounds it to 2 fractional digits in the return value.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The <paramref name="value"/> converted and rounded; the number of fractional digits in the return value is 2.</returns>
    public static double Convert2Double2Decimals(this decimal value)
    {
      return Math.Round(Convert.ToDouble(value), 2);
    }
    /// <summary>
    /// Rounds the mass upper.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RountMassUpper(this double value)
    {
      return Math.Round(value + 0.005, 2);
    }
    /// <summary>
    /// Rounds the currency.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundCurrency(this double value)
    {
      return Math.Round(value, 3);
    }

    //The number of fractional digits in the return value.
    private const int m_MassFractionalDigits = 2;
  }
}
