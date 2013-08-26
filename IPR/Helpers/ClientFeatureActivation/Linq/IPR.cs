//<summary>
//  Title   : IPR partial class
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
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// IPR partial class 
  /// </summary>
  public sealed partial class IPR
  {

    #region public
    internal void RecalculateClearedRecords()
    {
      if ( this.AccountClosed.Value )
        throw new ApplicationException( "IPR.RecalculateClearedRecords cannot be excuted for closed account" );
      List<Disposal> _2Calculate = ( from _dx in this.Disposal where _dx.CustomsStatus.Value == Linq.CustomsStatus.Finished orderby _dx.No.Value ascending select _dx ).ToList<Disposal>();
      this.AccountBalance = this.NetMass;
      foreach ( Disposal _dx in _2Calculate )
        _dx.CalculateRemainingQuantity();
    }
    #endregion

  }
}
