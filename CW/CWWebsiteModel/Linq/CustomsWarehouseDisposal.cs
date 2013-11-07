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
    internal void UpdateTitle( DateTime dateTime )
    {
      Title = String.Format( "CW-{0:D4}{1:D6}", dateTime.Year, "XXXXXX" ); //TODO Id.Value);
    }
    public string GoodsName( Entities entities )
    {
      CustomsWarehouse _cw = CWL_CWDisposal2CustomsWarehouseID;
      return Settings.FormatGoodsName( entities, _cw.TobaccoName, _cw.Grade, _cw.SKU, _cw.Batch, _cw.DocumentNo );
    }
    /// <summary>
    /// Gets the goods code.
    /// </summary>
    /// <value>
    /// The goods code.
    /// </value>
    public string ProductCode
    {
      get
      {
        string _code = this.CWL_CWDisposal2CustomsWarehouseID.CWL_CW2PCNID.ProductCodeNumber;
        return _code.Substring( _code.Length - 2, 2 );
      }
    }
    /// <summary>
    /// Gets the taric.
    /// </summary>
    /// <value>
    /// The taric.
    /// </value>
    public string ProductCodeTaric
    {
      get
      {
        string _code = this.CWL_CWDisposal2CustomsWarehouseID.CWL_CW2PCNID.ProductCodeNumber;
        return _code.Substring( 0, _code.Length - 2 );
      }
    }
  }
}
