using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel
{
  internal static class WebsiteModelExtensions
  {
    public static Decimal DecimalValue(this Nullable<double> value)
    {
      return Convert.ToDecimal(value.Value);
    }
    public static double DoubleValue(this decimal value)
    {
      return Convert.ToDouble(value);
    }
  }
}
