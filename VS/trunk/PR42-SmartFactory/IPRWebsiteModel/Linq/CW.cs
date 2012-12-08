using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class CWCW
  {
    public CWCW( Account.CWAccountData data, Clearence clearence, SADDocumentType declaration )
      : this()
    {
      this.AccountBalance = data.NetMass;
      this.Batch = data.Batch;
      ConsentPeriod = data.ConsentLookup.ConsentPeriod;
      this.CW2Clearence = clearence;
      //ClosingDate = CAS.SharePoint.Extensions.SPMinimum;
      this.Currency = declaration.Currency;
      this.CW2Consent = data.ConsentLookup;
      //TODO this.CW2ConsentTitleIdentyfikator == 
      this.CW2ConsentTitleTitle = data.ConsentLookup.Title;
      this.CW2PCNTID = data.PCNTariffCode;
      this.CW2PCWLibraryID = data.cwLibraryID;
      this.CWCertificate = data.CWCertificate;
      this.CustomsDebtDate = data.CustomsDebtDate;
      this.CWClosingDate = Extensions.DateTimeNull;
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
    public double? ConsentPeriod { get; set; }
    public CWLibraryCWLib CW2Library { get; set; }


    public void UpdateTitle()
    {
      throw new NotImplementedException();
    }
  }
}
