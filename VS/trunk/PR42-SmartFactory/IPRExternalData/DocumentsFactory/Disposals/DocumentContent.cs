﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:2.0.50727.5456
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace CAS.SmartFactory.xml.DocumentsFactory.Disposals {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd", IsNullable=false)]
    public partial class DocumentContent {
        
        private System.DateTime documentDateField;
        
        private string documentNoField;
        
        private System.DateTime startDateField;
        
        private System.DateTime endDateField;
        
        private string customProcedureCodeField;
        
        private double totalField;
        
        private MaterialsOnOneAccount[] accountDescriptionField;
        
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
        public string CustomProcedureCode {
            get {
                return this.customProcedureCodeField;
            }
            set {
                this.customProcedureCodeField = value;
            }
        }
        
        /// <uwagi/>
        public double Total {
            get {
                return this.totalField;
            }
            set {
                this.totalField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public MaterialsOnOneAccount[] AccountDescription {
            get {
                return this.accountDescriptionField;
            }
            set {
                this.accountDescriptionField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd")]
    public partial class MaterialsOnOneAccount {
        
        private double totalField;
        
        private MaterialRecord[] materialRecordsField;
        
        /// <uwagi/>
        public double Total {
            get {
                return this.totalField;
            }
            set {
                this.totalField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public MaterialRecord[] MaterialRecords {
            get {
                return this.materialRecordsField;
            }
            set {
                this.materialRecordsField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd")]
    public partial class MaterialRecord {
        
        private string customDocumentNoField;
        
        private System.DateTime dateField;
        
        private string materialSKUField;
        
        private string materialBatchField;
        
        private string finishedGoodBatchField;
        
        private double qantityField;
        
        private System.Nullable<double> unitPriceField;
        
        private bool unitPriceFieldSpecified;
        
        private System.Nullable<double> tobaccoValueField;
        
        private bool tobaccoValueFieldSpecified;
        
        private string currencyField;
        
        /// <uwagi/>
        public string CustomDocumentNo {
            get {
                return this.customDocumentNoField;
            }
            set {
                this.customDocumentNoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime Date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <uwagi/>
        public string MaterialSKU {
            get {
                return this.materialSKUField;
            }
            set {
                this.materialSKUField = value;
            }
        }
        
        /// <uwagi/>
        public string MaterialBatch {
            get {
                return this.materialBatchField;
            }
            set {
                this.materialBatchField = value;
            }
        }
        
        /// <uwagi/>
        public string FinishedGoodBatch {
            get {
                return this.finishedGoodBatchField;
            }
            set {
                this.finishedGoodBatchField = value;
            }
        }
        
        /// <uwagi/>
        public double Qantity {
            get {
                return this.qantityField;
            }
            set {
                this.qantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> UnitPrice {
            get {
                return this.unitPriceField;
            }
            set {
                this.unitPriceField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnitPriceSpecified {
            get {
                return this.unitPriceFieldSpecified;
            }
            set {
                this.unitPriceFieldSpecified = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> TobaccoValue {
            get {
                return this.tobaccoValueField;
            }
            set {
                this.tobaccoValueField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TobaccoValueSpecified {
            get {
                return this.tobaccoValueFieldSpecified;
            }
            set {
                this.tobaccoValueFieldSpecified = value;
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
    }
}
