//<summary>
//  Title   : Extensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      

using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
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
    internal static void AddIfNew<TKey>(this List<TKey> list, TKey key)
    {
      if (list.Contains(key))
        return;
      list.Add(key);
    }
    internal static void AddHistoryEntry(this Table<Linq2SQL.History> sqledc, Linq2SQL.History history)
    {
      sqledc.InsertOnSubmit(history);
    }

  }
}
