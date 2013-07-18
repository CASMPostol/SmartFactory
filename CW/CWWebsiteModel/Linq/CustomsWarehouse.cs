//<summary>
//  Title   : Customs Warehousing Account Class
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
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Customs Warehousing Account Class
  /// </summary>
  public partial class CustomsWarehouse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CW" /> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="declaration">The declaration.</param>
    public CustomsWarehouse( Linq.Entities edc, Account.CWAccountData data )
      : this()
    {
      this.AccountBalance = data.CommonAccountData.NetMass;
      this.Batch = data.CommonAccountData.BatchId;
      this.ConsentPeriod = data.ConsentLookup.ConsentPeriod;// To be removed
      //this.CWL_2Clearence = clearence; //TODO - the column must be defined 
      this.ClosingDate = Extensions.DateTimeNull;
      this.Currency = "PLN";
      this.CWL_CW2ConsentTitle = data.ConsentLookup;
      this.CWL_CW2PCNID = GetAtIndex<PCNCode>( edc.PCNCode, data.CommonAccountData.PCNTariffCodeLookup );
      this.CWL_CW2CWLibraryIDId = null;
      this.CustomsDebtDate = data.CommonAccountData.CustomsDebtDate;
      this.CW_MassPerPackage = data.CWMassPerPackage;
      this.CW_PackageKg = data.CWPackageKg;
      this.CW_PackageUnits = data.CWPackageUnits;
      this.CW_PzNo = "M/A";
      this.CW_Quantity = data.CWQuantity;
      this.DocumentNo = data.ClearenceLookup.DocumentNo;
      this.CWC_EntryDate = data.EntryDate;
      this.Grade = data.CommonAccountData.GradeName;
      this.GrossMass = data.CommonAccountData.GrossMass;
      this.InvoiceNo = data.CommonAccountData.Invoice;
      this.NetMass = data.CommonAccountData.NetMass;
      this.SKU = data.CommonAccountData.SKU;
      this.TobaccoName = data.CommonAccountData.TobaccoName;
      this.Title = "-- creating -- ";
      this.Units = data.Units;
      this.ValidToDate = data.ValidToDate;
      //Certificate
      this.CW_CertificateOfOrgin = data.CW_CertificateOfOrgin;
      this.CW_CertificateOfAuthenticity = data.CW_CertificateOfAuthenticity;
      this.CW_COADate = data.CW_COADate;
      this.CW_CODate = data.CW_CODate;
      this.Vendor = data.VendorLookup; 
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      Title = String.Format( "CW-{0:D4}{1:D6}", this.CWC_EntryDate.Value.Year, Id.Value );
    }
    internal static bool RecordExist( Entities entities, string documentNo )
    {
      return ( from CustomsWarehouse _cwx in entities.CustomsWarehouse where _cwx.DocumentNo.Contains( documentNo ) select _cwx ).Any();
    }
  }
}
