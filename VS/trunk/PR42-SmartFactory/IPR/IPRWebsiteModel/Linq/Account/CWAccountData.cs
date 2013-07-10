//<summary>
//  Title   : Customs Warehouse Account Record Data
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

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData: AccountData
  {
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="good">The good.</param>
    protected internal override void GetNetMass( SADGood good )
    {
      NetMass = good.NetMass.GetValueOrDefault( 0 );
    }
    /// <summary>
    /// Gets the customs process.
    /// </summary>
    /// <value>
    /// The customs process.
    /// </value>
    protected override CustomsProcess Process
    {
      get { return CustomsProcess.cw; }
    }
    /// <summary>
    /// Analizes the goods description.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="goodsDescription">The goods description.</param>
    protected override void AnalizeGoodsDescription( Entities edc, string goodsDescription )
    {
      base.AnalizeGoodsDescription( edc, goodsDescription );
    }
  }
}
