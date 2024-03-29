﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 
namespace CAS.SmartFactory.xml.Dictionaries {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd", IsNullable=false)]
    public partial class Configuration {
        
        private ConfigurationFormatItem[] formatField;
        
        private ConfigurationWasteItem[] wasteField;
        
        private ConfigurationDustItem[] dustField;
        
        private ConfigurationSHMentholItem[] sHMentholField;
        
        private ConfigurationUsageItem[] usageField;
        
        private ConfigurationCutfillerCoefficientItem[] cutfillerCoefficientField;
        
        private ConfigurationConsentItem[] consentField;
        
        private ConfigurationPCNCodeItem[] pCNCodeField;
        
        private ConfigurationWarehouseItem[] warehouseField;
        
        private ConfigurationCustomsUnionItem[] customsUnionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FormatItem")]
        public ConfigurationFormatItem[] Format {
            get {
                return this.formatField;
            }
            set {
                this.formatField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("WasteItem")]
        public ConfigurationWasteItem[] Waste {
            get {
                return this.wasteField;
            }
            set {
                this.wasteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DustItem")]
        public ConfigurationDustItem[] Dust {
            get {
                return this.dustField;
            }
            set {
                this.dustField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SHMentholItem")]
        public ConfigurationSHMentholItem[] SHMenthol {
            get {
                return this.sHMentholField;
            }
            set {
                this.sHMentholField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("UsageItem")]
        public ConfigurationUsageItem[] Usage {
            get {
                return this.usageField;
            }
            set {
                this.usageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CutfillerCoefficientItem")]
        public ConfigurationCutfillerCoefficientItem[] CutfillerCoefficient {
            get {
                return this.cutfillerCoefficientField;
            }
            set {
                this.cutfillerCoefficientField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ConsentItem")]
        public ConfigurationConsentItem[] Consent {
            get {
                return this.consentField;
            }
            set {
                this.consentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("PCNCodeItem")]
        public ConfigurationPCNCodeItem[] PCNCode {
            get {
                return this.pCNCodeField;
            }
            set {
                this.pCNCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("WarehouseItem")]
        public ConfigurationWarehouseItem[] Warehouse {
            get {
                return this.warehouseField;
            }
            set {
                this.warehouseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CustomsUnionItem")]
        public ConfigurationCustomsUnionItem[] CustomsUnion {
            get {
                return this.customsUnionField;
            }
            set {
                this.customsUnionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationFormatItem {
        
        private string titleField;
        
        private string cigaretteLenghtField;
        
        private string filterLenghtField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CigaretteLenght {
            get {
                return this.cigaretteLenghtField;
            }
            set {
                this.cigaretteLenghtField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string FilterLenght {
            get {
                return this.filterLenghtField;
            }
            set {
                this.filterLenghtField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationWasteItem {
        
        private string productTypeField;
        
        private double wasteRatioField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ProductType {
            get {
                return this.productTypeField;
            }
            set {
                this.productTypeField = value;
            }
        }
        
        /// <remarks/>
        public double WasteRatio {
            get {
                return this.wasteRatioField;
            }
            set {
                this.wasteRatioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationDustItem {
        
        private string productTypeField;
        
        private double dustRatioField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ProductType {
            get {
                return this.productTypeField;
            }
            set {
                this.productTypeField = value;
            }
        }
        
        /// <remarks/>
        public double DustRatio {
            get {
                return this.dustRatioField;
            }
            set {
                this.dustRatioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationSHMentholItem {
        
        private string titleField;
        
        private string productTypeField;
        
        private double sHMentholRatioField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ProductType {
            get {
                return this.productTypeField;
            }
            set {
                this.productTypeField = value;
            }
        }
        
        /// <remarks/>
        public double SHMentholRatio {
            get {
                return this.sHMentholRatioField;
            }
            set {
                this.sHMentholRatioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationUsageItem {
        
        private string format_lookupField;
        
        private double usageMinField;
        
        private double usageMaxField;
        
        private double cTFUsageMinField;
        
        private double cTFUsageMaxField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Format_lookup {
            get {
                return this.format_lookupField;
            }
            set {
                this.format_lookupField = value;
            }
        }
        
        /// <remarks/>
        public double UsageMin {
            get {
                return this.usageMinField;
            }
            set {
                this.usageMinField = value;
            }
        }
        
        /// <remarks/>
        public double UsageMax {
            get {
                return this.usageMaxField;
            }
            set {
                this.usageMaxField = value;
            }
        }
        
        /// <remarks/>
        public double CTFUsageMin {
            get {
                return this.cTFUsageMinField;
            }
            set {
                this.cTFUsageMinField = value;
            }
        }
        
        /// <remarks/>
        public double CTFUsageMax {
            get {
                return this.cTFUsageMaxField;
            }
            set {
                this.cTFUsageMaxField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationCutfillerCoefficientItem {
        
        private double cFTProductivityRateMinField;
        
        private double cFTProductivityRateMaxField;
        
        /// <remarks/>
        public double CFTProductivityRateMin {
            get {
                return this.cFTProductivityRateMinField;
            }
            set {
                this.cFTProductivityRateMinField = value;
            }
        }
        
        /// <remarks/>
        public double CFTProductivityRateMax {
            get {
                return this.cFTProductivityRateMaxField;
            }
            set {
                this.cFTProductivityRateMaxField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationConsentItem {
        
        private string consentNoField;
        
        private System.DateTime consentDateField;
        
        private bool consentDateFieldSpecified;
        
        private System.DateTime validFromDateField;
        
        private bool validFromDateFieldSpecified;
        
        private System.DateTime validToDateField;
        
        private bool validToDateFieldSpecified;
        
        private double productivityRateMinField;
        
        private double productivityRateMaxField;
        
        private double consentPeriodField;
        
        private bool isIPRField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ConsentNo {
            get {
                return this.consentNoField;
            }
            set {
                this.consentNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime ConsentDate {
            get {
                return this.consentDateField;
            }
            set {
                this.consentDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConsentDateSpecified {
            get {
                return this.consentDateFieldSpecified;
            }
            set {
                this.consentDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime ValidFromDate {
            get {
                return this.validFromDateField;
            }
            set {
                this.validFromDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidFromDateSpecified {
            get {
                return this.validFromDateFieldSpecified;
            }
            set {
                this.validFromDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime ValidToDate {
            get {
                return this.validToDateField;
            }
            set {
                this.validToDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ValidToDateSpecified {
            get {
                return this.validToDateFieldSpecified;
            }
            set {
                this.validToDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double ProductivityRateMin {
            get {
                return this.productivityRateMinField;
            }
            set {
                this.productivityRateMinField = value;
            }
        }
        
        /// <remarks/>
        public double ProductivityRateMax {
            get {
                return this.productivityRateMaxField;
            }
            set {
                this.productivityRateMaxField = value;
            }
        }
        
        /// <remarks/>
        public double ConsentPeriod {
            get {
                return this.consentPeriodField;
            }
            set {
                this.consentPeriodField = value;
            }
        }
        
        /// <remarks/>
        public bool IsIPR {
            get {
                return this.isIPRField;
            }
            set {
                this.isIPRField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationPCNCodeItem {
        
        private string titleField;
        
        private string productCodeNumberField;
        
        private string compensationGoodField;
        
        private bool disposalField;
        
        private bool disposalFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ProductCodeNumber {
            get {
                return this.productCodeNumberField;
            }
            set {
                this.productCodeNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CompensationGood {
            get {
                return this.compensationGoodField;
            }
            set {
                this.compensationGoodField = value;
            }
        }
        
        /// <remarks/>
        public bool Disposal {
            get {
                return this.disposalField;
            }
            set {
                this.disposalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DisposalSpecified {
            get {
                return this.disposalFieldSpecified;
            }
            set {
                this.disposalFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationWarehouseItem {
        
        private string titleField;
        
        private string warehouseNameField;
        
        private string productTypeField;
        
        private string itemTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string WarehouseName {
            get {
                return this.warehouseNameField;
            }
            set {
                this.warehouseNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ProductType {
            get {
                return this.productTypeField;
            }
            set {
                this.productTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ItemType {
            get {
                return this.itemTypeField;
            }
            set {
                this.itemTypeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/Batch.xsd")]
    public partial class ConfigurationCustomsUnionItem {
        
        private string titleField;
        
        private string eUPrimeMarketField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string EUPrimeMarket {
            get {
                return this.eUPrimeMarketField;
            }
            set {
                this.eUPrimeMarketField = value;
            }
        }
    }
}
