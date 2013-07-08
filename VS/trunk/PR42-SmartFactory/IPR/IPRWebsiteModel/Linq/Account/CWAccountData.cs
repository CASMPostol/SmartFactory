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
    /// Initializes a new instance of the <see cref="CWAccountData"/> class.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> object.</param>
    /// <param name="good">The good.</param>
    /// <param name="messageType">Type of the customs message.</param>
    public CWAccountData( Entities edc, SADGood good, MessageType messageType )
      : base( edc, good, messageType )
    {
      //this.CWCertificate = "TBD";
      //this.CWMassPerPackage = 0;
      //this.CWPackageKg = 0;
      //this.CWPackageUnits = 0;
      //this.CWPzNo = "TBD";
      //this.CWQuantity = 0;
      //this.EntryDate = DateTime.Today.Date;
      //this.Units = "TBT";
      //TODO how to assign values. More info required.
    }
    //internal string CWCertificate { get; private set; }  //TODO from Required documents. 
    //internal double? CWMassPerPackage { get; private set; } //TODO Calculated
    //internal double? CWPackageKg { get; private set; } // Good description
    //internal double? CWPackageUnits { get; private set; } //Good description
    //internal string CWPzNo { get; private set; } // Manualy
    //internal double? CWQuantity { get; private set; } //Good descriptionc
    //internal DateTime? EntryDate { get; private set; } // Today ?
    //internal string Units { get; private set; } //TODO Good description - ??
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
