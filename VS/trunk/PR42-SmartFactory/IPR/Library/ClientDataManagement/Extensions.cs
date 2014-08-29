//<summary>
//  Title   : class Extensions
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
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement
{
  internal static class Extensions
  {
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    internal static decimal Round2Decimals(this decimal value)
    {
      return Math.Round(value, 2);
    }
    internal static bool IsLatter(this DateTime? start, int delay)
    {
      return start.HasValue ? start + TimeSpan.FromDays(delay) < DateTime.Today : false;
    }
    internal static string UserName()
    {
      return String.Format(Properties.Resources.ActivitiesLogsUserNamePattern, Environment.UserName, Environment.MachineName);
    }
    internal static void AddIfNotNull<T>(this List<T> list, T item)
      where T : class
    {
      if (item == null)
        return;
      list.Add(item);
    }
  }
}
