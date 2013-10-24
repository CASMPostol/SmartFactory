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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Entity partial class of CustomsWarehouseDisposal.
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {
    /// <summary>
    /// CustomsWarehouseDisposal data obtained from xml message.
    /// </summary>
    public struct XmlData
    {
      /// <summary>
      /// Additional quantity declared to dispose
      /// </summary>
      public decimal AdditionalQuantity;
      /// <summary>
      /// The declared quantity
      /// </summary>
      public decimal DeclaredQuantity;
      /// <summary>
      /// The sku description
      /// </summary>
      public string SKUDescription;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle(DateTime dateTime)
    {
      Title = String.Format("CW-{0:D4}{1:D6}", dateTime.Year, "XXXXXX"); //TODO Id.Value);
    }

  }
}
