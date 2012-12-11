﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class CW
  {
    public CW( Account.CWAccountData data, Clearence clearence, SADDocumentType declaration )
      : this()
    {
      this.AccountBalance = data.NetMass;
      this.Batch = data.Batch;
      this.ConsentPeriod = data.ConsentLookup.ConsentPeriod;
      this.CW2Clearence = clearence;
      this.ClosingDate = Extensions.DateTimeNull;
      this.Currency = declaration.Currency;
      this.CW2Consent = data.ConsentLookup;
      //TODO this.CW2ConsentTitleIdentyfikator == 
      this.CW2ConsentTitleTitle = data.ConsentLookup.Title;
      this.CW2PCNTID = data.PCNTariffCode;
      this.CW2PCWLibraryID = data.CWLibraryID;
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
      //this.IPRLibraryIndex = declaration.SADDocumenLibrarytIndex;
      NetMass = data.NetMass;
      SKU = data.SKU;
      TobaccoName = data.TobaccoName;
      Title = "-- creating -- ";
      this.Units = data.Units;
      ValidToDate = data.CustomsDebtDate;
      this.Value = data.Value;
    }
    public Clearence CW2Clearence { get; set; }
    public Consent CW2Consent { get; private set; }

    public void UpdateTitle()
    {
      Title = String.Format( "CW-{0:D4}{1:D6}", this.EntryDate.Value.Year, Identyfikator.Value );
    }
  }
}
