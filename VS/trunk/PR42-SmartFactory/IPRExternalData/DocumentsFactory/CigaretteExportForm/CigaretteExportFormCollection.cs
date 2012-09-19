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
namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd", IsNullable=true)]
    public partial class CigaretteExportFormCollection {
        
        private string documentNoField;
        
        private System.DateTime documentDateField;
        
        private string invoiceNoField;
        
        private double numberOfDocumentsField;
        
        private CigaretteExportForm[] cigaretteExportFormsField;
        
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
        public System.DateTime DocumentDate {
            get {
                return this.documentDateField;
            }
            set {
                this.documentDateField = value;
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
        public double NumberOfDocuments {
            get {
                return this.numberOfDocumentsField;
            }
            set {
                this.numberOfDocumentsField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public CigaretteExportForm[] CigaretteExportForms {
            get {
                return this.cigaretteExportFormsField;
            }
            set {
                this.cigaretteExportFormsField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public partial class CigaretteExportForm {
        
        private string finishedGoodSKUField;
        
        private string finishedGoodBatchField;
        
        private double finishedGoodQantityField;
        
        private string finishedGoodUnitField;
        
        private string familyDescriptionField;
        
        private string brandDescriptionField;
        
        private double materialTotalField;
        
        private string finishedGoodSKUDescriptionField;
        
        private double tobaccoTotalField;
        
        private double iPTMaterialQuantityTotalField;
        
        private TotalAmountOfMoney iPTDutyVatTotalsField;
        
        private double regularMaterialQuantityTotalField;
        
        private double cTFUsageMinField;
        
        private double cTFUsageMaxField;
        
        private double cTFUsagePerUnitMinField;
        
        private double cTFUsagePerUnitMaxField;
        
        private double cTFUsagePer1MFinishedGoodsMinField;
        
        private double cTFUsagePer1MFinishedGoodsMaxField;
        
        private double wasteCoefficientField;
        
        private string customsProcedureField;
        
        private string documentNoField;
        
        private string productFormatField;
        
        private double dustKgField;
        
        private double sHMentholKgField;
        
        private double wasteKgField;
        
        private double portionField;
        
        private ProductType productField;
        
        private Ingredient[] ingredientsField;
        
        private double iPRRestMaterialQantityTotalField;
        
        /// <uwagi/>
        public string FinishedGoodSKU {
            get {
                return this.finishedGoodSKUField;
            }
            set {
                this.finishedGoodSKUField = value;
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
        public double FinishedGoodQantity {
            get {
                return this.finishedGoodQantityField;
            }
            set {
                this.finishedGoodQantityField = value;
            }
        }
        
        /// <uwagi/>
        public string FinishedGoodUnit {
            get {
                return this.finishedGoodUnitField;
            }
            set {
                this.finishedGoodUnitField = value;
            }
        }
        
        /// <uwagi/>
        public string FamilyDescription {
            get {
                return this.familyDescriptionField;
            }
            set {
                this.familyDescriptionField = value;
            }
        }
        
        /// <uwagi/>
        public string BrandDescription {
            get {
                return this.brandDescriptionField;
            }
            set {
                this.brandDescriptionField = value;
            }
        }
        
        /// <uwagi/>
        public double MaterialTotal {
            get {
                return this.materialTotalField;
            }
            set {
                this.materialTotalField = value;
            }
        }
        
        /// <uwagi/>
        public string FinishedGoodSKUDescription {
            get {
                return this.finishedGoodSKUDescriptionField;
            }
            set {
                this.finishedGoodSKUDescriptionField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoTotal {
            get {
                return this.tobaccoTotalField;
            }
            set {
                this.tobaccoTotalField = value;
            }
        }
        
        /// <uwagi/>
        public double IPTMaterialQuantityTotal {
            get {
                return this.iPTMaterialQuantityTotalField;
            }
            set {
                this.iPTMaterialQuantityTotalField = value;
            }
        }
        
        /// <uwagi/>
        public TotalAmountOfMoney IPTDutyVatTotals {
            get {
                return this.iPTDutyVatTotalsField;
            }
            set {
                this.iPTDutyVatTotalsField = value;
            }
        }
        
        /// <uwagi/>
        public double RegularMaterialQuantityTotal {
            get {
                return this.regularMaterialQuantityTotalField;
            }
            set {
                this.regularMaterialQuantityTotalField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsageMin {
            get {
                return this.cTFUsageMinField;
            }
            set {
                this.cTFUsageMinField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsageMax {
            get {
                return this.cTFUsageMaxField;
            }
            set {
                this.cTFUsageMaxField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsagePerUnitMin {
            get {
                return this.cTFUsagePerUnitMinField;
            }
            set {
                this.cTFUsagePerUnitMinField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsagePerUnitMax {
            get {
                return this.cTFUsagePerUnitMaxField;
            }
            set {
                this.cTFUsagePerUnitMaxField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsagePer1MFinishedGoodsMin {
            get {
                return this.cTFUsagePer1MFinishedGoodsMinField;
            }
            set {
                this.cTFUsagePer1MFinishedGoodsMinField = value;
            }
        }
        
        /// <uwagi/>
        public double CTFUsagePer1MFinishedGoodsMax {
            get {
                return this.cTFUsagePer1MFinishedGoodsMaxField;
            }
            set {
                this.cTFUsagePer1MFinishedGoodsMaxField = value;
            }
        }
        
        /// <uwagi/>
        public double WasteCoefficient {
            get {
                return this.wasteCoefficientField;
            }
            set {
                this.wasteCoefficientField = value;
            }
        }
        
        /// <uwagi/>
        public string CustomsProcedure {
            get {
                return this.customsProcedureField;
            }
            set {
                this.customsProcedureField = value;
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
        public string ProductFormat {
            get {
                return this.productFormatField;
            }
            set {
                this.productFormatField = value;
            }
        }
        
        /// <uwagi/>
        public double DustKg {
            get {
                return this.dustKgField;
            }
            set {
                this.dustKgField = value;
            }
        }
        
        /// <uwagi/>
        public double SHMentholKg {
            get {
                return this.sHMentholKgField;
            }
            set {
                this.sHMentholKgField = value;
            }
        }
        
        /// <uwagi/>
        public double WasteKg {
            get {
                return this.wasteKgField;
            }
            set {
                this.wasteKgField = value;
            }
        }
        
        /// <uwagi/>
        public double Portion {
            get {
                return this.portionField;
            }
            set {
                this.portionField = value;
            }
        }
        
        /// <uwagi/>
        public ProductType Product {
            get {
                return this.productField;
            }
            set {
                this.productField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(IPRIngredient), IsNullable=false)]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(RegularIngredient), IsNullable=false)]
        public Ingredient[] Ingredients {
            get {
                return this.ingredientsField;
            }
            set {
                this.ingredientsField = value;
            }
        }
        
        /// <uwagi/>
        public double IPRRestMaterialQantityTotal {
            get {
                return this.iPRRestMaterialQantityTotalField;
            }
            set {
                this.iPRRestMaterialQantityTotalField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public partial class TotalAmountOfMoney {
        
        private AmountOfMoney[] amountOfMoneyField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfTotals")]
        public AmountOfMoney[] AmountOfMoney {
            get {
                return this.amountOfMoneyField;
            }
            set {
                this.amountOfMoneyField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public partial class AmountOfMoney {
        
        private double iPRMaterialValueTotalField;
        
        private double iPRMaterialDutyTotalField;
        
        private double iPRMaterialVATTotalField;
        
        private string currencyField;
        
        /// <uwagi/>
        public double IPRMaterialValueTotal {
            get {
                return this.iPRMaterialValueTotalField;
            }
            set {
                this.iPRMaterialValueTotalField = value;
            }
        }
        
        /// <uwagi/>
        public double IPRMaterialDutyTotal {
            get {
                return this.iPRMaterialDutyTotalField;
            }
            set {
                this.iPRMaterialDutyTotalField = value;
            }
        }
        
        /// <uwagi/>
        public double IPRMaterialVATTotal {
            get {
                return this.iPRMaterialVATTotalField;
            }
            set {
                this.iPRMaterialVATTotalField = value;
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
    
    /// <uwagi/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegularIngredient))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IPRIngredient))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public abstract partial class Ingredient {
        
        private string tobaccoSKUField;
        
        private string tobaccoBatchField;
        
        private double tobaccoQuantityField;
        
        /// <uwagi/>
        public string TobaccoSKU {
            get {
                return this.tobaccoSKUField;
            }
            set {
                this.tobaccoSKUField = value;
            }
        }
        
        /// <uwagi/>
        public string TobaccoBatch {
            get {
                return this.tobaccoBatchField;
            }
            set {
                this.tobaccoBatchField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoQuantity {
            get {
                return this.tobaccoQuantityField;
            }
            set {
                this.tobaccoQuantityField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public partial class RegularIngredient : Ingredient {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public partial class IPRIngredient : Ingredient {
        
        private string documentNoumberField;
        
        private System.DateTime dateField;
        
        private double tobaccoUnitPriceField;
        
        private double tobaccoValueField;
        
        private ClearingType itemClearingTypeField;
        
        private string currencyField;
        
        private double dutyField;
        
        private double vATField;
        
        /// <uwagi/>
        public string DocumentNoumber {
            get {
                return this.documentNoumberField;
            }
            set {
                this.documentNoumberField = value;
            }
        }
        
        /// <uwagi/>
        public System.DateTime Date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <uwagi/>
        public double TobaccoUnitPrice {
            get {
                return this.tobaccoUnitPriceField;
            }
            set {
                this.tobaccoUnitPriceField = value;
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
        public ClearingType ItemClearingType {
            get {
                return this.itemClearingTypeField;
            }
            set {
                this.itemClearingTypeField = value;
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
        public double Duty {
            get {
                return this.dutyField;
            }
            set {
                this.dutyField = value;
            }
        }
        
        /// <uwagi/>
        public double VAT {
            get {
                return this.vATField;
            }
            set {
                this.vATField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public enum ClearingType {
        
        /// <uwagi/>
        PartialWindingUp,
        
        /// <uwagi/>
        TotalWindingUp,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd")]
    public enum ProductType {
        
        /// <uwagi/>
        Cutfiller,
        
        /// <uwagi/>
        Cigarette,
    }
}
