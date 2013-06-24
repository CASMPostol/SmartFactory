using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData: AccountData
  {

    public CWAccountData( Entities edc, SADGood good, MessageType messageType )
      : base( edc, good, messageType )
    {
      this.CWCertificate = "TBD";
      this.CWMassPerPackage = 0;
      this.CWPackageKg = 0;
      this.CWPackageUnits = 0;
      this.CWPzNo = "TBD";
      this.CWQuantity = 0;
      this.EntryDate = DateTime.Today.Date;
      this.Units = "TBT";
      //TODO how to assign values. More info required.
    }
    internal string CWCertificate { get; private set; }  //TODO from Required documents. 
    internal double? CWMassPerPackage { get; private set; } //TODO Calculated
    internal double? CWPackageKg { get; private set; } // Good description
    internal double? CWPackageUnits { get; private set; } //Good description
    internal string CWPzNo { get; private set; } // Manualy
    internal double? CWQuantity { get; private set; } //Good descriptionc
    internal DateTime? EntryDate { get; private set; } // Today ?
    internal string Units { get; private set; } //TODO Good description - ??
    protected internal override void GetNetMass( SADGood good )
    {
      NetMass = good.NetMass.GetValueOrDefault( 0 );
    }
    protected internal override Consent.CustomsProcess Process
    {
      get { return Consent.CustomsProcess.cw; }
    }
    protected override void AnalizeGoodsDescription( Entities edc, string goodsDescription )
    {
      base.AnalizeGoodsDescription( edc, goodsDescription );
      CWQuantity = 0; //TODO
    }
  }
}
