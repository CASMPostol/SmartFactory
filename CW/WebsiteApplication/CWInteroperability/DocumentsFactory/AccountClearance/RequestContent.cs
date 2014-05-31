﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.18052
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 
namespace CAS.SmartFactory.CW.Interoperability.DocumentsFactory.AccountClearance {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/RequestCo" +
        "ntent.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/RequestCo" +
        "ntent.xsd", IsNullable=false)]
    public partial class RequestContent {
        
        private string documentNameField;
        
        private int documentNoField;
        
        private System.DateTime documentDateField;
        
        private string introducingSADDocumentNoField;
        
        private System.DateTime introducingSADDocumentDateField;
        
        private string withdrawalSADDcoumentNoField;
        
        private System.DateTime withdrawalSADDocumentDateField;
        
        private string consentNoField;
        
        private System.DateTime consentDateField;
        
        private string tobaccoNameField;
        
        private string gradeField;
        
        private string sKUField;
        
        private string batchField;
        
        private string cNTarrifCodeField;
        
        private double quantityField;
        
        private double netMassField;
        
        private double grossMassField;
        
        private double packageUnitsField;
        
        private double valueField;
        
        private string currencyField;
        
        private double unitPriceField;
        
        private string pzNoField;
        
        private string invoiceNoField;
        
        private ArrayOfDisposalDisposalsArray[] disposalsColectionField;
        
        /// <uwagi/>
        public string DocumentName {
            get {
                return this.documentNameField;
            }
            set {
                this.documentNameField = value;
            }
        }
        
        /// <uwagi/>
        public int DocumentNo {
            get {
                return this.documentNoField;
            }
            set {
                this.documentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime DocumentDate {
            get {
                return this.documentDateField;
            }
            set {
                this.documentDateField = value;
            }
        }
        
        /// <uwagi/>
        public string IntroducingSADDocumentNo {
            get {
                return this.introducingSADDocumentNoField;
            }
            set {
                this.introducingSADDocumentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime IntroducingSADDocumentDate {
            get {
                return this.introducingSADDocumentDateField;
            }
            set {
                this.introducingSADDocumentDateField = value;
            }
        }
        
        /// <uwagi/>
        public string WithdrawalSADDcoumentNo {
            get {
                return this.withdrawalSADDcoumentNoField;
            }
            set {
                this.withdrawalSADDcoumentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime WithdrawalSADDocumentDate {
            get {
                return this.withdrawalSADDocumentDateField;
            }
            set {
                this.withdrawalSADDocumentDateField = value;
            }
        }
        
        /// <uwagi/>
        public string ConsentNo {
            get {
                return this.consentNoField;
            }
            set {
                this.consentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime ConsentDate {
            get {
                return this.consentDateField;
            }
            set {
                this.consentDateField = value;
            }
        }
        
        /// <uwagi/>
        public string TobaccoName {
            get {
                return this.tobaccoNameField;
            }
            set {
                this.tobaccoNameField = value;
            }
        }
        
        /// <uwagi/>
        public string Grade {
            get {
                return this.gradeField;
            }
            set {
                this.gradeField = value;
            }
        }
        
        /// <uwagi/>
        public string SKU {
            get {
                return this.sKUField;
            }
            set {
                this.sKUField = value;
            }
        }
        
        /// <uwagi/>
        public string Batch {
            get {
                return this.batchField;
            }
            set {
                this.batchField = value;
            }
        }
        
        /// <uwagi/>
        public string CNTarrifCode {
            get {
                return this.cNTarrifCodeField;
            }
            set {
                this.cNTarrifCodeField = value;
            }
        }
        
        /// <uwagi/>
        public double Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <uwagi/>
        public double NetMass {
            get {
                return this.netMassField;
            }
            set {
                this.netMassField = value;
            }
        }
        
        /// <uwagi/>
        public double GrossMass {
            get {
                return this.grossMassField;
            }
            set {
                this.grossMassField = value;
            }
        }
        
        /// <uwagi/>
        public double PackageUnits {
            get {
                return this.packageUnitsField;
            }
            set {
                this.packageUnitsField = value;
            }
        }
        
        /// <uwagi/>
        public double Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
        
        /// <uwagi/>
        public string Currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <uwagi/>
        public double UnitPrice {
            get {
                return this.unitPriceField;
            }
            set {
                this.unitPriceField = value;
            }
        }
        
        /// <uwagi/>
        public string PzNo {
            get {
                return this.pzNoField;
            }
            set {
                this.pzNoField = value;
            }
        }
        
        /// <uwagi/>
        public string InvoiceNo {
            get {
                return this.invoiceNoField;
            }
            set {
                this.invoiceNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DisposalsArray", IsNullable=false)]
        public ArrayOfDisposalDisposalsArray[] DisposalsColection {
            get {
                return this.disposalsColectionField;
            }
            set {
                this.disposalsColectionField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/RequestCo" +
        "ntent.xsd")]
    public partial class ArrayOfDisposalDisposalsArray {
        
        private int noField;
        
        private string sADDocumentNoField;
        
        private System.DateTime sADDateField;
        
        private double settledNetMassField;
        
        private double settledGrossMassField;
        
        private double tobaccoValueField;
        
        private string currencyField;
        
        private int packageToClearField;
        
        private string wzField;
        
        private double remainingQuantityField;
        
        private int remainingPackageField;
        
        private string cNTarrifCodeField;
        
        /// <uwagi/>
        public int No {
            get {
                return this.noField;
            }
            set {
                this.noField = value;
            }
        }
        
        /// <uwagi/>
        public string SADDocumentNo {
            get {
                return this.sADDocumentNoField;
            }
            set {
                this.sADDocumentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime SADDate {
            get {
                return this.sADDateField;
            }
            set {
                this.sADDateField = value;
            }
        }
        
        /// <uwagi/>
        public double SettledNetMass {
            get {
                return this.settledNetMassField;
            }
            set {
                this.settledNetMassField = value;
            }
        }
        
        /// <uwagi/>
        public double SettledGrossMass {
            get {
                return this.settledGrossMassField;
            }
            set {
                this.settledGrossMassField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoValue {
            get {
                return this.tobaccoValueField;
            }
            set {
                this.tobaccoValueField = value;
            }
        }
        
        /// <uwagi/>
        public string Currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <uwagi/>
        public int PackageToClear {
            get {
                return this.packageToClearField;
            }
            set {
                this.packageToClearField = value;
            }
        }
        
        /// <uwagi/>
        public string WZ {
            get {
                return this.wzField;
            }
            set {
                this.wzField = value;
            }
        }
        
        /// <uwagi/>
        public double RemainingQuantity {
            get {
                return this.remainingQuantityField;
            }
            set {
                this.remainingQuantityField = value;
            }
        }
        
        /// <uwagi/>
        public int RemainingPackage {
            get {
                return this.remainingPackageField;
            }
            set {
                this.remainingPackageField = value;
            }
        }
        
        /// <uwagi/>
        public string CNTarrifCode {
            get {
                return this.cNTarrifCodeField;
            }
            set {
                this.cNTarrifCodeField = value;
            }
        }
    }
}