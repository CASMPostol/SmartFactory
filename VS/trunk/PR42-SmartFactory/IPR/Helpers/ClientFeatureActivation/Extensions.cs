using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation
{
  internal static class Extensions
  {
    /// <summary>
    /// Rounts the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static decimal Rount2Decimals( this decimal value )
    {
      return Math.Round( value, 2 );
    }
  }
}
