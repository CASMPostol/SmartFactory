using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class CW
  {
    public CW( Account.CWAccountData iprdata, Clearence clearence, SADDocumentType declaration, DateTime customsDebtDate )
      : this()
    {
      this.AccountBalance = iprdata.NetMass;
      this.Batch = iprdata.Batch;
      ConsentPeriod = iprdata.ConsentLookup.ConsentPeriod;
      this.CW2Clearence = clearence;
      //ClosingDate = CAS.SharePoint.Extensions.SPMinimum;
      this.Currency = declaration.Currency;
      this.CW2Consent = iprdata.ConsentLookup;
      //TODO this.CW2ConsentTitleIdentyfikator == 
      this.CW2ConsentTitleTitle = iprdata.ConsentLookup.Title;
      this.CW2PCNTID = iprdata.PCNTariffCode;
      this.CW2PCWLibraryID = iprdata.cwLibraryID;
      this.CWCertificate = iprdata.CWCertificate;
      this.CustomsDebtDate = customsDebtDate;
      this.CWClosingDate = Extensions.DateTimeNull;
      this.CWMassPerPackage = iprdata.CWMassPerPackage;
      this.CWPackageKg = iprdata.CWPackageKg;
      this.CWPackageUnits = iprdata.CWPackageUnits;
      this.CWPzNo = iprdata.CWPzNo;
      this.CWQuantity = iprdata.CWQuantity;
      this.CWUnitPrice = iprdata.UnitPrice;
      this.DocumentNo = clearence.DocumentNo;
      this.EntryDate = iprdata.EntryDate;
      this.Grade = iprdata.GradeName;
      this.GrossMass = iprdata.GrossMass;
      this.InvoiceNo = iprdata.Invoice;
      //this.IPRLibraryIndex = declaration.SADDocumenLibrarytIndex;
      NetMass = iprdata.NetMass;
      SKU = iprdata.SKU;
      TobaccoName = iprdata.TobaccoName;
      Title = "-- creating -- ";
      this.Units = iprdata.Units;
      ValidToDate = customsDebtDate + TimeSpan.FromDays( iprdata.ConsentLookup.ConsentPeriod.Value );
      this.Value = iprdata.Value;
    }
    public Clearence CW2Clearence { get; set; }
    public Consent CW2Consent { get; private set; }
    public double? ConsentPeriod { get; set; }
    public CWLibraryCWLib CW2Library { get; set; }

  }
}
