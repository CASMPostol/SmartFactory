﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 
namespace CAS.SmartFactory.xml.erp {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/cw/DisposalRequest.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/jti/cw/DisposalRequest.xsd", IsNullable=false)]
    public partial class DisposalRequest {
        
        private string sKUField;
        
        private string sKUDescriptionField;
        
        private string batchField;
        
        private decimal cW_DeclaredToClearField;
        
        private bool cW_DeclaredToClearFieldSpecified;
        
        private decimal cW_AddedKgField;
        
        private bool cW_AddedKgFieldSpecified;
        
        /// <remarks/>
        public string SKU {
            get {
                return this.sKUField;
            }
            set {
                this.sKUField = value;
            }
        }
        
        /// <remarks/>
        public string SKUDescription {
            get {
                return this.sKUDescriptionField;
            }
            set {
                this.sKUDescriptionField = value;
            }
        }
        
        /// <remarks/>
        public string Batch {
            get {
                return this.batchField;
            }
            set {
                this.batchField = value;
            }
        }
        
        /// <remarks/>
        public decimal CW_DeclaredToClear {
            get {
                return this.cW_DeclaredToClearField;
            }
            set {
                this.cW_DeclaredToClearField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CW_DeclaredToClearSpecified {
            get {
                return this.cW_DeclaredToClearFieldSpecified;
            }
            set {
                this.cW_DeclaredToClearFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal CW_AddedKg {
            get {
                return this.cW_AddedKgField;
            }
            set {
                this.cW_AddedKgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CW_AddedKgSpecified {
            get {
                return this.cW_AddedKgFieldSpecified;
            }
            set {
                this.cW_AddedKgFieldSpecified = value;
            }
        }
    }
}