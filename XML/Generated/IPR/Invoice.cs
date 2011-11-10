﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:2.0.50727.5448
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace CAS.SmartFactory.xml.IPR {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Invoice.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/jti/ipr/Invoice.xsd", IsNullable=true)]
    public partial class Invoice {
        
        private InvoiceItem[] itemField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public InvoiceItem[] Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Invoice.xsd")]
    public partial class InvoiceItem {
        
        private decimal bill_docField;
        
        private System.Nullable<decimal> itemField;
        
        private bool itemFieldSpecified;
        
        private System.Nullable<decimal> hgLvItField;
        
        private bool hgLvItFieldSpecified;
        
        private decimal billed_quantityField;
        
        private string suField;
        
        private System.Nullable<decimal> bill_qty_in_SKUField;
        
        private string bUnField;
        
        private string ref_docField;
        
        private string refItmField;
        
        private string prCatField;
        
        private string sales_docField;
        
        private string item2Field;
        
        private string sales_doc__has_resulted_from_rField;
        
        private string materialField;
        
        private string descriptionField;
        
        private string batchField;
        
        private string itCaField;
        
        private System.DateTime created_onField;
        
        private System.Nullable<System.DateTime> timeField;
        
        private bool timeFieldSpecified;
        
        private string sLocField;
        
        private string ship_toField;
        
        /// <uwagi/>
        public decimal Bill_doc {
            get {
                return this.bill_docField;
            }
            set {
                this.bill_docField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ItemSpecified {
            get {
                return this.itemFieldSpecified;
            }
            set {
                this.itemFieldSpecified = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> HgLvIt {
            get {
                return this.hgLvItField;
            }
            set {
                this.hgLvItField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HgLvItSpecified {
            get {
                return this.hgLvItFieldSpecified;
            }
            set {
                this.hgLvItFieldSpecified = value;
            }
        }
        
        /// <uwagi/>
        public decimal Billed_quantity {
            get {
                return this.billed_quantityField;
            }
            set {
                this.billed_quantityField = value;
            }
        }
        
        /// <uwagi/>
        public string SU {
            get {
                return this.suField;
            }
            set {
                this.suField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> Bill_qty_in_SKU {
            get {
                return this.bill_qty_in_SKUField;
            }
            set {
                this.bill_qty_in_SKUField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string BUn {
            get {
                return this.bUnField;
            }
            set {
                this.bUnField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Ref_doc {
            get {
                return this.ref_docField;
            }
            set {
                this.ref_docField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string RefItm {
            get {
                return this.refItmField;
            }
            set {
                this.refItmField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string PrCat {
            get {
                return this.prCatField;
            }
            set {
                this.prCatField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Sales_doc {
            get {
                return this.sales_docField;
            }
            set {
                this.sales_docField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Item2 {
            get {
                return this.item2Field;
            }
            set {
                this.item2Field = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Sales_doc__has_resulted_from_r {
            get {
                return this.sales_doc__has_resulted_from_rField;
            }
            set {
                this.sales_doc__has_resulted_from_rField = value;
            }
        }
        
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
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ItCa {
            get {
                return this.itCaField;
            }
            set {
                this.itCaField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime Created_on {
            get {
                return this.created_onField;
            }
            set {
                this.created_onField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time", IsNullable=true)]
        public System.Nullable<System.DateTime> Time {
            get {
                return this.timeField;
            }
            set {
                this.timeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimeSpecified {
            get {
                return this.timeFieldSpecified;
            }
            set {
                this.timeFieldSpecified = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string SLoc {
            get {
                return this.sLocField;
            }
            set {
                this.sLocField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Ship_to {
            get {
                return this.ship_toField;
            }
            set {
                this.ship_toField = value;
            }
        }
    }
}
