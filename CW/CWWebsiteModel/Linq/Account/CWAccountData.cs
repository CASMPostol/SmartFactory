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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Customs.Account;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData: CommonAccountData
  {

    public CWAccountData( CommonAccountData commonData, MessageType messageType )
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
    internal void GetNetMass( SADGood good )
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
    protected  void AnalizeGoodsDescription( string goodsDescription )
    {
      CWQuantity = 0; //TODO
    }
  }
}
