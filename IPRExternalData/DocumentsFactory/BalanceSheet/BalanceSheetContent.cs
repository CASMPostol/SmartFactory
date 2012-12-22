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
namespace CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd", IsNullable=false)]
    public partial class BalanceSheetContent {
        
        private System.DateTime documentDateField;
        
        private string documentNoField;
        
        private System.DateTime situationAtDateField;
        
        private System.DateTime startDateField;
        
        private System.DateTime endDateField;
        
        private IPRStockContent iPRStockField;
        
        private JSOContent jSOXField;
        
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
        public string DocumentNo {
            get {
                return this.documentNoField;
            }
            set {
                this.documentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime SituationAtDate {
            get {
                return this.situationAtDateField;
            }
            set {
                this.situationAtDateField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime StartDate {
            get {
                return this.startDateField;
            }
            set {
                this.startDateField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime EndDate {
            get {
                return this.endDateField;
            }
            set {
                this.endDateField = value;
            }
        }
        
        /// <uwagi/>
        public IPRStockContent IPRStock {
            get {
                return this.iPRStockField;
            }
            set {
                this.iPRStockField = value;
            }
        }
        
        /// <uwagi/>
        public JSOContent JSOX {
            get {
                return this.jSOXField;
            }
            set {
                this.jSOXField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class IPRStockContent {
        
        private IPRRow[] iPRListField;
        
        private double totalIPRBookField;
        
        private double totalSHWasteOveruseCSNotStartedField;
        
        private double totalDustCSNotStartedField;
        
        private double totalTobaccoAvailableField;
        
        private double totalTobaccoInWarehouseField;
        
        private double totalTobaccoInCigarettesWarehouseField;
        
        private double totalTobaccoInCigarettesProductionField;
        
        private double totalTobaccoInCutfillerWarehouseField;
        
        private double totalBalanceField;
        
        /// <uwagi/>
        public IPRRow[] IPRList {
            get {
                return this.iPRListField;
            }
            set {
                this.iPRListField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalIPRBook {
            get {
                return this.totalIPRBookField;
            }
            set {
                this.totalIPRBookField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalSHWasteOveruseCSNotStarted {
            get {
                return this.totalSHWasteOveruseCSNotStartedField;
            }
            set {
                this.totalSHWasteOveruseCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalDustCSNotStarted {
            get {
                return this.totalDustCSNotStartedField;
            }
            set {
                this.totalDustCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalTobaccoAvailable {
            get {
                return this.totalTobaccoAvailableField;
            }
            set {
                this.totalTobaccoAvailableField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalTobaccoInWarehouse {
            get {
                return this.totalTobaccoInWarehouseField;
            }
            set {
                this.totalTobaccoInWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalTobaccoInCigarettesWarehouse {
            get {
                return this.totalTobaccoInCigarettesWarehouseField;
            }
            set {
                this.totalTobaccoInCigarettesWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalTobaccoInCigarettesProduction {
            get {
                return this.totalTobaccoInCigarettesProductionField;
            }
            set {
                this.totalTobaccoInCigarettesProductionField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalTobaccoInCutfillerWarehouse {
            get {
                return this.totalTobaccoInCutfillerWarehouseField;
            }
            set {
                this.totalTobaccoInCutfillerWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalBalance {
            get {
                return this.totalBalanceField;
            }
            set {
                this.totalBalanceField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class IPRRow {
        
        private string entryDocumentNoField;
        
        private string sKUField;
        
        private string batchField;
        
        private double iPRBookField;
        
        private double sHWasteOveruseCSNotStartedField;
        
        private double dustCSNotStartedField;
        
        private double tobaccoAvailableField;
        
        private double tobaccoInWarehouseField;
        
        private double tobaccoInCigarettesWarehouseField;
        
        private double tobaccoInCigarettesProductionField;
        
        private double tobaccoInCutfillerWarehouseField;
        
        private double balanceField;
        
        /// <uwagi/>
        public string EntryDocumentNo {
            get {
                return this.entryDocumentNoField;
            }
            set {
                this.entryDocumentNoField = value;
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
        public double IPRBook {
            get {
                return this.iPRBookField;
            }
            set {
                this.iPRBookField = value;
            }
        }
        
        /// <uwagi/>
        public double SHWasteOveruseCSNotStarted {
            get {
                return this.sHWasteOveruseCSNotStartedField;
            }
            set {
                this.sHWasteOveruseCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public double DustCSNotStarted {
            get {
                return this.dustCSNotStartedField;
            }
            set {
                this.dustCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoAvailable {
            get {
                return this.tobaccoAvailableField;
            }
            set {
                this.tobaccoAvailableField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoInWarehouse {
            get {
                return this.tobaccoInWarehouseField;
            }
            set {
                this.tobaccoInWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoInCigarettesWarehouse {
            get {
                return this.tobaccoInCigarettesWarehouseField;
            }
            set {
                this.tobaccoInCigarettesWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoInCigarettesProduction {
            get {
                return this.tobaccoInCigarettesProductionField;
            }
            set {
                this.tobaccoInCigarettesProductionField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoInCutfillerWarehouse {
            get {
                return this.tobaccoInCutfillerWarehouseField;
            }
            set {
                this.tobaccoInCutfillerWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public double Balance {
            get {
                return this.balanceField;
            }
            set {
                this.balanceField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class DisposalRow {
        
        private string exportOrFreeCirculationSADField;
        
        private string entryDocumentNoField;
        
        private System.DateTime sADDateField;
        
        private string invoiceNoField;
        
        private string compensationGoodField;
        
        private double totalAmountField;
        
        /// <uwagi/>
        public string ExportOrFreeCirculationSAD {
            get {
                return this.exportOrFreeCirculationSADField;
            }
            set {
                this.exportOrFreeCirculationSADField = value;
            }
        }
        
        /// <uwagi/>
        public string EntryDocumentNo {
            get {
                return this.entryDocumentNoField;
            }
            set {
                this.entryDocumentNoField = value;
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
        public string InvoiceNo {
            get {
                return this.invoiceNoField;
            }
            set {
                this.invoiceNoField = value;
            }
        }
        
        /// <uwagi/>
        public string CompensationGood {
            get {
                return this.compensationGoodField;
            }
            set {
                this.compensationGoodField = value;
            }
        }
        
        /// <uwagi/>
        public double TotalAmount {
            get {
                return this.totalAmountField;
            }
            set {
                this.totalAmountField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class JSOContent {
        
        private DisposalRow[] disposalsField;
        
        private System.DateTime previousMonthDateField;
        
        private double previousMonthQuantityField;
        
        private System.DateTime introducingDateStartField;
        
        private System.DateTime introducingDateEndField;
        
        private double introducingQuantityField;
        
        private System.DateTime outboundDateStartField;
        
        private System.DateTime outboundDateEndField;
        
        private double outboundQuantityField;
        
        private System.DateTime balanceDateField;
        
        private double balanceQuantityField;
        
        private System.DateTime situationDateField;
        
        private double situationQuantityField;
        
        private double reassumeQuantityField;
        
        /// <uwagi/>
        public DisposalRow[] Disposals {
            get {
                return this.disposalsField;
            }
            set {
                this.disposalsField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime PreviousMonthDate {
            get {
                return this.previousMonthDateField;
            }
            set {
                this.previousMonthDateField = value;
            }
        }
        
        /// <uwagi/>
        public double PreviousMonthQuantity {
            get {
                return this.previousMonthQuantityField;
            }
            set {
                this.previousMonthQuantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime IntroducingDateStart {
            get {
                return this.introducingDateStartField;
            }
            set {
                this.introducingDateStartField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime IntroducingDateEnd {
            get {
                return this.introducingDateEndField;
            }
            set {
                this.introducingDateEndField = value;
            }
        }
        
        /// <uwagi/>
        public double IntroducingQuantity {
            get {
                return this.introducingQuantityField;
            }
            set {
                this.introducingQuantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime OutboundDateStart {
            get {
                return this.outboundDateStartField;
            }
            set {
                this.outboundDateStartField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime OutboundDateEnd {
            get {
                return this.outboundDateEndField;
            }
            set {
                this.outboundDateEndField = value;
            }
        }
        
        /// <uwagi/>
        public double OutboundQuantity {
            get {
                return this.outboundQuantityField;
            }
            set {
                this.outboundQuantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime BalanceDate {
            get {
                return this.balanceDateField;
            }
            set {
                this.balanceDateField = value;
            }
        }
        
        /// <uwagi/>
        public double BalanceQuantity {
            get {
                return this.balanceQuantityField;
            }
            set {
                this.balanceQuantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime SituationDate {
            get {
                return this.situationDateField;
            }
            set {
                this.situationDateField = value;
            }
        }
        
        /// <uwagi/>
        public double SituationQuantity {
            get {
                return this.situationQuantityField;
            }
            set {
                this.situationQuantityField = value;
            }
        }
        
        /// <uwagi/>
        public double ReassumeQuantity {
            get {
                return this.reassumeQuantityField;
            }
            set {
                this.reassumeQuantityField = value;
            }
        }
    }
}
