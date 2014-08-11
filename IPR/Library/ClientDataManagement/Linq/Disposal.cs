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
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// Disposal
  /// </summary>
  public sealed partial class Disposal
  {
    #region public
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      return _ret;
    }
    /// <summary>
    /// Gets or sets the settled quantity as decimal value.
    /// </summary>
    /// <value>
    /// The settled quantity as decimal.
    /// </value>
    internal decimal SettledQuantityDec
    {
      get { return Convert.ToDecimal( this.SettledQuantity.Value ).Round2Decimals(); }
    }
    #endregion

  }
}
