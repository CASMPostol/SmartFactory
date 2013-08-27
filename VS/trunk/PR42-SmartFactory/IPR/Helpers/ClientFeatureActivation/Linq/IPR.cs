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
using CAS.SmartFactory.IPR.Client.FeatureActivation;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// IPR partial class 
  /// </summary>
  public sealed partial class IPR
  {

    #region public
    internal void RecalculateClearedRecords( Entities entities, List<Linq.Disposal> _dl, EntitiesChangedEventHandler progress )
    {
      if ( this.AccountClosed.Value )
        return; 
      List<Disposal> _2Calculate = ( from _dx in _dl 
                                     where (_dx.Disposal2IPRIndex == this) && (_dx.CustomsStatus.Value == Linq.CustomsStatus.Finished) 
                                     orderby _dx.No.Value ascending select _dx ).ToList<Disposal>();
      this.AccountBalance = this.NetMass;
      foreach ( Disposal _dx in _2Calculate )
      {
        _dx.CalculateRemainingQuantity();
        progress( this, new Client.FeatureActivation.EntitiesChangedEventArgs( 1, null, entities ) );
      }
    }
    #endregion

  }
}
