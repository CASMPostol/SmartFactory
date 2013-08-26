//<summary>
//  Title   : Archive class contain collection of function supporting archival data management
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
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation.Archival
{
  /// <summary>
  /// Archive class contain collection of function supporting archival data management
  /// </summary>
  internal static class Archive
  {

    internal static void Go( Entities edc, Action<object, EntitiesChangedEventArgs> ProgressChanged )
    {
      //foreach ( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR _iprX in edc.IPR )
      //  if (_iprX.AccountClosed.Value == true

    }
  }
}
