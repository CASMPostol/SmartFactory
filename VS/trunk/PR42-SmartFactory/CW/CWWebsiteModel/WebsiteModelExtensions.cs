//<summary>
//  Title   : static class WebsiteModelExtensions
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

namespace CAS.SmartFactory.CW.WebsiteModel
{
  /// <summary>
  /// static class WebsiteModelExtensions
  /// </summary>
  public static class WebsiteModelExtensions
  {
    /// <summary>
    /// Rounds the currency.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundValue(this double value)
    {
      return Math.Round(value, 2);
    }
    internal static Decimal DecimalValue(this Nullable<double> value)
    {
      return Convert.ToDecimal(value.GetValueOrDefault(0));
    }
    internal static double DoubleValue(this decimal value)
    {
      return Convert.ToDouble(value);
    }
    /// <summary>
    /// Convert an <see cref="Linq.ClearenceProcedure"/> instance to the string.
    /// </summary>
    /// <param name="procedure">The procedure.</param>
    public static string Convert2String(this Linq.ClearenceProcedure procedure)
    {
      string _ret = "unKnown";
      switch (procedure)
      {
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._3151:
          _ret = "3151";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._3171:
          _ret = "3171";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._4051:
          _ret = "4051";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._4071:
          _ret = "4071";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._5100:
          _ret = "5100";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._5171:
          _ret = "5171";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._7100:
          _ret = "7100";
          break;
        case CAS.SmartFactory.CW.WebsiteModel.Linq.ClearenceProcedure._7171:
          _ret = "7171";
          break;
        default:
          break;
      }
      return _ret;
    }
    /// <summary>
    /// Convert rounded double to the Int32.
    /// </summary>
    /// <param name="value">The _value.</param>
    public static Int32 Convert2Int(this double value) { return Convert.ToInt32(Math.Round(value, 0)); }
    public static int? GetTargetId<type>(this type _cw)
      where type : CAS.SmartFactory.CW.WebsiteModel.Linq.Element
    {
      try
      {
        return _cw == null ? new Nullable<int>() : _cw.Id.Value;
      }
      catch (Exception)
      {
        return new Nullable<int>();
      }
    }
  }
}
