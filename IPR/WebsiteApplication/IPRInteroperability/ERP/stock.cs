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
namespace CAS.SmartFactory.xml.erp {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Stock.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/jti/ipr/Stock.xsd", IsNullable=false)]
    public partial class Stock {
        
        private StockRow[] rowField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("Row")]
        public StockRow[] Row {
            get {
                return this.rowField;
            }
            set {
                this.rowField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Stock.xsd")]
    public partial class StockRow {
        
        private string materialField;
        
        private string materialDescriptionField;
        
        private string plntField;
        
        private string sLocField;
        
        private string batchField;
        
        private string bUnField;
        
        private double unrestrictedField;
        
        private double inQualityInspField;
        
        private double restrictedUseField;
        
        private double blockedField;
        
        /// <uwagi/>
        public string Material {
            get {
                return this.materialField;
            }
            set {
                this.materialField = value;
            }
        }
        
        /// <uwagi/>
        public string MaterialDescription {
            get {
                return this.materialDescriptionField;
            }
            set {
                this.materialDescriptionField = value;
            }
        }
        
        /// <uwagi/>
        public string Plnt {
            get {
                return this.plntField;
            }
            set {
                this.plntField = value;
            }
        }
        
        /// <uwagi/>
        public string SLoc {
            get {
                return this.sLocField;
            }
            set {
                this.sLocField = value;
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
        public string BUn {
            get {
                return this.bUnField;
            }
            set {
                this.bUnField = value;
            }
        }
        
        /// <uwagi/>
        public double Unrestricted {
            get {
                return this.unrestrictedField;
            }
            set {
                this.unrestrictedField = value;
            }
        }
        
        /// <uwagi/>
        public double InQualityInsp {
            get {
                return this.inQualityInspField;
            }
            set {
                this.inQualityInspField = value;
            }
        }
        
        /// <uwagi/>
        public double RestrictedUse {
            get {
                return this.restrictedUseField;
            }
            set {
                this.restrictedUseField = value;
            }
        }
        
        /// <uwagi/>
        public double Blocked {
            get {
                return this.blockedField;
            }
            set {
                this.blockedField = value;
            }
        }
    }
}
