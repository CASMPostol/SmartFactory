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
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Customs Warehousing Account Class
  /// </summary>
  public partial class CW
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CW" /> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="declaration">The declaration.</param>
    public CW( Linq.Entities edc, Account.CWAccountData data )
      : this()
    {
      Linq.Consent _consentLookup = GetAtIndex<Consent>( edc.Consent, data.CommonAccountData.ConsentLookup.Value );
      this.AccountBalance = data.CommonAccountData.NetMass;
      this.Batch = data.CommonAccountData.BatchId;
      this.ConsentPeriod = _consentLookup.ConsentPeriod;
      //this.CW2Clearence = clearence;
      this.ClosingDate = Extensions.DateTimeNull;

      this.Currency = "PLN";
      this.CW2ConsentTitle = _consentLookup;
      this.CW2PCNID = GetAtIndex<PCNCode>( edc.PCNCode, data.CommonAccountData.PCNTariffCodeLookup );
      this.CW2CWLibraryID = null;
      this.CWCertificate = data.CWCertificate;
      this.CustomsDebtDate = data.CommonAccountData.CustomsDebtDate;
      this.CWMassPerPackage = data.CWMassPerPackage;
      this.CWPackageKg = data.CWPackageKg;
      this.CWPackageUnits = data.CWPackageUnits;
      this.CWPzNo = data.CWPzNo;
      this.CWQuantity = data.CWQuantity;
      this.CWUnitPrice = data.CommonAccountData.UnitPrice;
      this.DocumentNo = data.Clearence.DocumentNo;
      this.EntryDate = data.EntryDate;
      this.Grade = data.CommonAccountData.GradeName;
      this.GrossMass = data.CommonAccountData.GrossMass;
      this.InvoiceNo = data.CommonAccountData.Invoice;
      this.NetMass = data.CommonAccountData.NetMass;
      this.SKU = data.CommonAccountData.SKU;
      this.TobaccoName = data.CommonAccountData.TobaccoName;
      this.Title = "-- creating -- ";
      this.Units = data.Units;
      this.ValidToDate = data.CommonAccountData.CustomsDebtDate;
      this.Value = data.CommonAccountData.Value;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      Title = String.Format( "CW-{0:D4}{1:D6}", this.EntryDate.Value.Year, Identyfikator.Value );
    }
  }
}
