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
        
        private BalanceBatchContent[] balanceBatchField;
        
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
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public BalanceBatchContent[] BalanceBatch {
            get {
                return this.balanceBatchField;
            }
            set {
                this.balanceBatchField = value;
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
    public partial class BalanceBatchContent {
        
        private BalanceIPRContent[] balanceIPRField;
        
        private decimal totalIPRBookField;
        
        private decimal totalSHWasteOveruseCSNotStartedField;
        
        private decimal totalDustCSNotStartedField;
        
        private decimal totalTobaccoAvailableField;
        
        private decimal totalTobaccoInWarehouseField;
        
        private decimal totalTobaccoInCigarettesWarehouseField;
        
        private decimal totalTobaccoInCigarettesProductionField;
        
        private decimal totalTobaccoInCutfillerWarehouseField;
        
        private decimal totalBalanceField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public BalanceIPRContent[] BalanceIPR {
            get {
                return this.balanceIPRField;
            }
            set {
                this.balanceIPRField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalIPRBook {
            get {
                return this.totalIPRBookField;
            }
            set {
                this.totalIPRBookField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalSHWasteOveruseCSNotStarted {
            get {
                return this.totalSHWasteOveruseCSNotStartedField;
            }
            set {
                this.totalSHWasteOveruseCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalDustCSNotStarted {
            get {
                return this.totalDustCSNotStartedField;
            }
            set {
                this.totalDustCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalTobaccoAvailable {
            get {
                return this.totalTobaccoAvailableField;
            }
            set {
                this.totalTobaccoAvailableField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalTobaccoInWarehouse {
            get {
                return this.totalTobaccoInWarehouseField;
            }
            set {
                this.totalTobaccoInWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalTobaccoInCigarettesWarehouse {
            get {
                return this.totalTobaccoInCigarettesWarehouseField;
            }
            set {
                this.totalTobaccoInCigarettesWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalTobaccoInCigarettesProduction {
            get {
                return this.totalTobaccoInCigarettesProductionField;
            }
            set {
                this.totalTobaccoInCigarettesProductionField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalTobaccoInCutfillerWarehouse {
            get {
                return this.totalTobaccoInCutfillerWarehouseField;
            }
            set {
                this.totalTobaccoInCutfillerWarehouseField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TotalBalance {
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
    public partial class BalanceIPRContent {
        
        private string entryDocumentNoField;
        
        private string sKUField;
        
        private string batchField;
        
        private decimal iPRBookField;
        
        private decimal sHWasteOveruseCSNotStartedField;
        
        private decimal dustCSNotStartedField;
        
        private decimal tobaccoAvailableField;
        
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
        public decimal IPRBook {
            get {
                return this.iPRBookField;
            }
            set {
                this.iPRBookField = value;
            }
        }
        
        /// <uwagi/>
        public decimal SHWasteOveruseCSNotStarted {
            get {
                return this.sHWasteOveruseCSNotStartedField;
            }
            set {
                this.sHWasteOveruseCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public decimal DustCSNotStarted {
            get {
                return this.dustCSNotStartedField;
            }
            set {
                this.dustCSNotStartedField = value;
            }
        }
        
        /// <uwagi/>
        public decimal TobaccoAvailable {
            get {
                return this.tobaccoAvailableField;
            }
            set {
                this.tobaccoAvailableField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class JSOXCustomsSummaryContent {
        
        private string exportOrFreeCirculationSADField;
        
        private string entryDocumentNoField;
        
        private System.DateTime sADDateField;
        
        private string invoiceNoField;
        
        private string compensationGoodField;
        
        private string procedureField;
        
        private decimal quantityField;
        
        private decimal balanceField;
        
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
        public string Procedure {
            get {
                return this.procedureField;
            }
            set {
                this.procedureField = value;
            }
        }
        
        /// <uwagi/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <uwagi/>
        public decimal Balance {
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
    public partial class JSOXCustomsSummaryContentOGLGroup {
        
        private JSOXCustomsSummaryContent[] jSOXCustomsSummaryArrayField;
        
        private decimal subtotalQuantityField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("JSOXCustomsSummary", IsNullable=false)]
        public JSOXCustomsSummaryContent[] JSOXCustomsSummaryArray {
            get {
                return this.jSOXCustomsSummaryArrayField;
            }
            set {
                this.jSOXCustomsSummaryArrayField = value;
            }
        }
        
        /// <uwagi/>
        public decimal SubtotalQuantity {
            get {
                return this.subtotalQuantityField;
            }
            set {
                this.subtotalQuantityField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd")]
    public partial class JSOXCustomsSummaryContentList {
        
        private JSOXCustomsSummaryContentOGLGroup[] jSOXCustomsSummaryOGLGroupArrayField;
        
        private decimal subtotalQuantityField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("JSOXCustomsSummaryOGLGroup", IsNullable=false)]
        public JSOXCustomsSummaryContentOGLGroup[] JSOXCustomsSummaryOGLGroupArray {
            get {
                return this.jSOXCustomsSummaryOGLGroupArrayField;
            }
            set {
                this.jSOXCustomsSummaryOGLGroupArrayField = value;
            }
        }
        
        /// <uwagi/>
        public decimal SubtotalQuantity {
            get {
                return this.subtotalQuantityField;
            }
            set {
                this.subtotalQuantityField = value;
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
        
        private JSOXCustomsSummaryContentList jSOXCustomsSummaryListField;
        
        private System.DateTime previousMonthDateField;
        
        private decimal previousMonthQuantityField;
        
        private System.DateTime introducingDateStartField;
        
        private System.DateTime introducingDateEndField;
        
        private decimal introducingQuantityField;
        
        private System.DateTime outboundDateStartField;
        
        private System.DateTime outboundDateEndField;
        
        private decimal outboundQuantityField;
        
        private System.DateTime balanceDateField;
        
        private decimal balanceQuantityField;
        
        private System.DateTime situationDateField;
        
        private decimal situationQuantityField;
        
        private decimal reassumeQuantityField;
        
        /// <uwagi/>
        public JSOXCustomsSummaryContentList JSOXCustomsSummaryList {
            get {
                return this.jSOXCustomsSummaryListField;
            }
            set {
                this.jSOXCustomsSummaryListField = value;
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
        public decimal PreviousMonthQuantity {
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
        public decimal IntroducingQuantity {
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
        public decimal OutboundQuantity {
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
        public decimal BalanceQuantity {
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
        public decimal SituationQuantity {
            get {
                return this.situationQuantityField;
            }
            set {
                this.situationQuantityField = value;
            }
        }
        
        /// <uwagi/>
        public decimal ReassumeQuantity {
            get {
                return this.reassumeQuantityField;
            }
            set {
                this.reassumeQuantityField = value;
            }
        }
    }
}
