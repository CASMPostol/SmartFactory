﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:2.0.50727.5472
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/BinCard.x" +
        "sd")]
    [System.Xml.Serialization.XmlRootAttribute("BinCardContent", Namespace="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/BinCard.x" +
        "sd", IsNullable=false)]
    public partial class BinCardContentType {
        
        private string tobaccoNameField;
        
        private string tobaccoTypeField;
        
        private string sKUField;
        
        private string batchField;
        
        private string sADField;
        
        private System.DateTime sADDateField;
        
        private string pzNoField;
        
        private double netWeightField;
        
        private bool netWeightFieldSpecified;
        
        private double packageQuantityField;
        
        private bool packageQuantityFieldSpecified;
        
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
        public string TobaccoType {
            get {
                return this.tobaccoTypeField;
            }
            set {
                this.tobaccoTypeField = value;
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
        public string SAD {
            get {
                return this.sADField;
            }
            set {
                this.sADField = value;
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
        public string PzNo {
            get {
                return this.pzNoField;
            }
            set {
                this.pzNoField = value;
            }
        }
        
        /// <uwagi/>
        public double NetWeight {
            get {
                return this.netWeightField;
            }
            set {
                this.netWeightField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NetWeightSpecified {
            get {
                return this.netWeightFieldSpecified;
            }
            set {
                this.netWeightFieldSpecified = value;
            }
        }
        
        /// <uwagi/>
        public double PackageQuantity {
            get {
                return this.packageQuantityField;
            }
            set {
                this.packageQuantityField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PackageQuantitySpecified {
            get {
                return this.packageQuantityFieldSpecified;
            }
            set {
                this.packageQuantityFieldSpecified = value;
            }
        }
    }
}
