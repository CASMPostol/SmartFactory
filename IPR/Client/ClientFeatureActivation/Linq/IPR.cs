//<summary>
//  Title   : IPR partial class
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
    internal void RecalculateClearedRecords(Entities entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      if (this.AccountClosed.Value)
        return;
      List<Disposal> _my = (from _dxAll in this.Disposal.ToList<Disposal>() where _dxAll.DisposalStatus.Value != Linq.DisposalStatus.Cartons select _dxAll).ToList<Disposal>();
      this.TobaccoNotAllocated = Convert.ToDouble(Convert.ToDecimal(this.NetMass) - _my.Sum<Disposal>(x => x.SettledQuantityDec));
      if (this.TobaccoNotAllocated < 0)
        this.TobaccoNotAllocated = 0;
      this.AccountBalance = Convert.ToDouble(Convert.ToDecimal(this.NetMass) - _my.Where(y => y.CustomsStatus.Value == Linq.CustomsStatus.Finished).Sum<Disposal>(y => y.SettledQuantityDec));
      if (this.AccountBalance < 0)
        this.AccountBalance = 0;
      progress(this, new Client.FeatureActivation.EntitiesChangedEventArgs(1, null, entities));
    }
    #endregion

  }
}
