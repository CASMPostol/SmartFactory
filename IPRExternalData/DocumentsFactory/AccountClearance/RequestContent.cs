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
namespace CAS.SmartFactory.xml.DocumentsFactory.AccountClearance {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd", IsNullable=false)]
    public partial class RequestContent {
        
        private System.DateTime documentDateField;
        
        private string documentNoField;
        
        private RequestForAccountClearance requestField;
        
        private AccountBalance balanceField;
        
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
        public RequestForAccountClearance Request {
            get {
                return this.requestField;
            }
            set {
                this.requestField = value;
            }
        }
        
        /// <uwagi/>
        public AccountBalance Balance {
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd")]
    public partial class RequestForAccountClearance {
        
        private string documentNoField;
        
        private System.DateTime customsDebtDateField;
        
        private string sKUField;
        
        private string batchField;
        
        private double netMassField;
        
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
        public System.DateTime CustomsDebtDate {
            get {
                return this.customsDebtDateField;
            }
            set {
                this.customsDebtDateField = value;
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
        public double NetMass {
            get {
                return this.netMassField;
            }
            set {
                this.netMassField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd")]
    public partial class AccountBalance {
    }
}
