//<summary>
//  Title   : Name of Application
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

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  internal static class Extensions
  {

    /// <summary>
    /// The minimum date for - to avoid setting today 
    /// </summary>
    public static DateTime SPMinimum = new DateTime(1930, 1, 1);
    /// <summary>
    /// Rounds the currency.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundValue(this double value)
    {
      return Math.Round(value, 2);
    }

  }
}
