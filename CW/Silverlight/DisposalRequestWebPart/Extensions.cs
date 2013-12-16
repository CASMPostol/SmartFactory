//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
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
  }
}
