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
    public CW( Linq.Entities edc, Account.CWAccountData data, Clearence clearence, SADDocumentType declaration )
      : this()
    {
      Linq.Consent _consentLookup = GetAtIndex<Consent>( edc.Consent, data.ConsentLookup.Value );
      this.AccountBalance = data.NetMass;
      this.Batch = data.BatchId;
      this.ConsentPeriod = _consentLookup.ConsentPeriod;
      //this.CW2Clearence = clearence;
      this.ClosingDate = Extensions.DateTimeNull;
      this.Currency = "PLN";
      this.CW2ConsentTitle = _consentLookup;
      this.CW2PCNID = GetAtIndex<PCNCode>( edc.PCNCode, data.PCNTariffCodeLookup );
      this.CW2CWLibraryID = null;
      this.CWCertificate = data.CWCertificate;
      this.CustomsDebtDate = data.CustomsDebtDate;
      this.CWMassPerPackage = data.CWMassPerPackage;
      this.CWPackageKg = data.CWPackageKg;
      this.CWPackageUnits = data.CWPackageUnits;
      this.CWPzNo = data.CWPzNo;
      this.CWQuantity = data.CWQuantity;
      this.CWUnitPrice = data.UnitPrice;
      this.DocumentNo = clearence.DocumentNo;
      this.EntryDate = data.EntryDate;
      this.Grade = data.GradeName;
      this.GrossMass = data.GrossMass;
      this.InvoiceNo = data.Invoice;
      this.NetMass = data.NetMass;
      this.SKU = data.SKU;
      this.TobaccoName = data.TobaccoName;
      this.Title = "-- creating -- ";
      this.Units = data.Units;
      this.ValidToDate = data.CustomsDebtDate;
      this.Value = data.Value;
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
