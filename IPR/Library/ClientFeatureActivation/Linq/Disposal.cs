//<summary>
//  Title   : Disposal Entity partial class.
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
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.Client.FeatureActivation;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Disposal
  /// </summary>
  internal sealed partial class Disposal
  {
    #region public
    /// <summary>
    /// Gets or sets the settled quantity dec.
    /// </summary>
    /// <value>
    /// The settled quantity as decimal.
    /// </value>
    internal decimal SettledQuantityDec
    {
      get { return Convert.ToDecimal( this.SettledQuantity.Value ).Rount2Decimals(); }
    }
    #endregion

  }
}
