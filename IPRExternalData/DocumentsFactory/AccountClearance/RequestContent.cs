﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:2.0.50727.5466
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace CAS.SmartFactory.xml.DocumentsFactory.AccountClearance
{
  using System.Xml.Serialization;


  /// <uwagi/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute( "xsd", "2.0.50727.3038" )]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute( "code" )]
  [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true, Namespace = "http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd" )]
  [System.Xml.Serialization.XmlRootAttribute( Namespace = "http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd", IsNullable = false )]
  public partial class RequestContent
  {

    private System.DateTime documentDateField;

    private string documentNoField;

    private string entryDocumentNoField;

    private System.DateTime customsDebtDateField;

    private string sKUField;

    private string batchField;

    private string gradeField;

    private double netMassField;

    private string pCNField;

    private System.DateTime consentDateField;

    private System.DateTime validFromDateField;

    private System.DateTime validToDateField;

    private double productivityRateMinField;

    private double productivityRateMaxField;

    private double consentPeriodField;

    private string consentNoField;

    private double dutyField;

    private double dutyPerUnitField;

    private double vATField;

    private double vATPerUnitField;

    private double grossMassField;

    private double cartonsField;

    private string tobaccoNameField;

    private string dutyNameField;

    private string vATNameField;

    private string invoiceNoField;

    private ArrayOfDIsposalsDisposalsArray[] disposalsColectionField;

    /// <uwagi/>
    [System.Xml.Serialization.XmlElementAttribute( DataType = "date" )]
    public System.DateTime DocumentDate
    {
      get
      {
        return this.documentDateField;
      }
      set
      {
        this.documentDateField = value;
      }
    }

    /// <uwagi/>
    public string DocumentNo
    {
      get
      {
        return this.documentNoField;
      }
      set
      {
        this.documentNoField = value;
      }
    }

    /// <uwagi/>
    public string EntryDocumentNo
    {
      get
      {
        return this.entryDocumentNoField;
      }
      set
      {
        this.entryDocumentNoField = value;
      }
    }

    /// <uwagi/>
    [System.Xml.Serialization.XmlElementAttribute( DataType = "date" )]
    public System.DateTime CustomsDebtDate
    {
      get
      {
        return this.customsDebtDateField;
      }
      set
      {
        this.customsDebtDateField = value;
      }
    }

    /// <uwagi/>
    public string SKU
    {
      get
      {
        return this.sKUField;
      }
      set
      {
        this.sKUField = value;
      }
    }

    /// <uwagi/>
    public string Batch
    {
      get
      {
        return this.batchField;
      }
      set
      {
        this.batchField = value;
      }
    }

    /// <uwagi/>
    public string Grade
    {
      get
      {
        return this.gradeField;
      }
      set
      {
        this.gradeField = value;
      }
    }

    /// <uwagi/>
    public double NetMass
    {
      get
      {
        return this.netMassField;
      }
      set
      {
        this.netMassField = value;
      }
    }

    /// <uwagi/>
    public string PCN
    {
      get
      {
        return this.pCNField;
      }
      set
      {
        this.pCNField = value;
      }
    }

    /// <uwagi/>
    public System.DateTime ConsentDate
    {
      get
      {
        return this.consentDateField;
      }
      set
      {
        this.consentDateField = value;
      }
    }

    /// <uwagi/>
    public System.DateTime ValidFromDate
    {
      get
      {
        return this.validFromDateField;
      }
      set
      {
        this.validFromDateField = value;
      }
    }

    /// <uwagi/>
    public System.DateTime ValidToDate
    {
      get
      {
        return this.validToDateField;
      }
      set
      {
        this.validToDateField = value;
      }
    }

    /// <uwagi/>
    public double ProductivityRateMin
    {
      get
      {
        return this.productivityRateMinField;
      }
      set
      {
        this.productivityRateMinField = value;
      }
    }

    /// <uwagi/>
    public double ProductivityRateMax
    {
      get
      {
        return this.productivityRateMaxField;
      }
      set
      {
        this.productivityRateMaxField = value;
      }
    }

    /// <uwagi/>
    public double ConsentPeriod
    {
      get
      {
        return this.consentPeriodField;
      }
      set
      {
        this.consentPeriodField = value;
      }
    }

    /// <uwagi/>
    public string ConsentNo
    {
      get
      {
        return this.consentNoField;
      }
      set
      {
        this.consentNoField = value;
      }
    }

    /// <uwagi/>
    public double Duty
    {
      get
      {
        return this.dutyField;
      }
      set
      {
        this.dutyField = value;
      }
    }

    /// <uwagi/>
    public double DutyPerUnit
    {
      get
      {
        return this.dutyPerUnitField;
      }
      set
      {
        this.dutyPerUnitField = value;
      }
    }

    /// <uwagi/>
    public double VAT
    {
      get
      {
        return this.vATField;
      }
      set
      {
        this.vATField = value;
      }
    }

    /// <uwagi/>
    public double VATPerUnit
    {
      get
      {
        return this.vATPerUnitField;
      }
      set
      {
        this.vATPerUnitField = value;
      }
    }

    /// <uwagi/>
    public double GrossMass
    {
      get
      {
        return this.grossMassField;
      }
      set
      {
        this.grossMassField = value;
      }
    }

    /// <uwagi/>
    public double Cartons
    {
      get
      {
        return this.cartonsField;
      }
      set
      {
        this.cartonsField = value;
      }
    }

    /// <uwagi/>
    public string TobaccoName
    {
      get
      {
        return this.tobaccoNameField;
      }
      set
      {
        this.tobaccoNameField = value;
      }
    }

    /// <uwagi/>
    public string DutyName
    {
      get
      {
        return this.dutyNameField;
      }
      set
      {
        this.dutyNameField = value;
      }
    }

    /// <uwagi/>
    public string VATName
    {
      get
      {
        return this.vATNameField;
      }
      set
      {
        this.vATNameField = value;
      }
    }

    /// <uwagi/>
    public string InvoiceNo
    {
      get
      {
        return this.invoiceNoField;
      }
      set
      {
        this.invoiceNoField = value;
      }
    }
    public decimal VATDutyTotal { get; set; }
    /// <uwagi/>
    [System.Xml.Serialization.XmlArrayItemAttribute( "DisposalsArray", IsNullable = false )]
    public ArrayOfDIsposalsDisposalsArray[] DisposalsColection
    {
      get
      {
        return this.disposalsColectionField;
      }
      set
      {
        this.disposalsColectionField = value;
      }
    }
    public ProductCodeNumberDesscription[] PCNRecord { get; set; }

  }
  public class ProductCodeNumberDesscription
  {
    public string CodeNumber { get; set; }
    public string Description { get; set; }
  }
  /// <uwagi/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute( "xsd", "2.0.50727.3038" )]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute( "code" )]
  [System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true, Namespace = "http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd" )]
  public partial class ArrayOfDIsposalsDisposalsArray
  {

    private double noField;

    private string sADDocumentNoField;

    private System.DateTime sADDateField;

    private string invoiceNoField;

    private double settledQuantityField;

    private double dutyPerSettledAmountField;

    private double vATPerSettledAmountField;

    private double dutyAndVATField;

    private double remainingQuantityField;

    private string customsProcedureField;

    private string productCodeNumberField;

    /// <uwagi/>
    public double No
    {
      get
      {
        return this.noField;
      }
      set
      {
        this.noField = value;
      }
    }

    /// <uwagi/>
    public string SADDocumentNo
    {
      get
      {
        return this.sADDocumentNoField;
      }
      set
      {
        this.sADDocumentNoField = value;
      }
    }

    /// <uwagi/>
    public System.DateTime SADDate
    {
      get
      {
        return this.sADDateField;
      }
      set
      {
        this.sADDateField = value;
      }
    }

    /// <uwagi/>
    public string InvoiceNo
    {
      get
      {
        return this.invoiceNoField;
      }
      set
      {
        this.invoiceNoField = value;
      }
    }

    /// <uwagi/>
    public double SettledQuantity
    {
      get
      {
        return this.settledQuantityField;
      }
      set
      {
        this.settledQuantityField = value;
      }
    }

    /// <uwagi/>
    public double DutyPerSettledAmount
    {
      get
      {
        return this.dutyPerSettledAmountField;
      }
      set
      {
        this.dutyPerSettledAmountField = value;
      }
    }

    /// <uwagi/>
    public double VATPerSettledAmount
    {
      get
      {
        return this.vATPerSettledAmountField;
      }
      set
      {
        this.vATPerSettledAmountField = value;
      }
    }

    /// <uwagi/>
    public double DutyAndVAT
    {
      get
      {
        return this.dutyAndVATField;
      }
      set
      {
        this.dutyAndVATField = value;
      }
    }

    /// <uwagi/>
    public double RemainingQuantity
    {
      get
      {
        return this.remainingQuantityField;
      }
      set
      {
        this.remainingQuantityField = value;
      }
    }

    /// <uwagi/>
    public string CustomsProcedure
    {
      get
      {
        return this.customsProcedureField;
      }
      set
      {
        this.customsProcedureField = value;
      }
    }

    /// <uwagi/>
    public string ProductCodeNumber
    {
      get
      {
        return this.productCodeNumberField;
      }
      set
      {
        this.productCodeNumberField = value;
      }
    }
  }
}
