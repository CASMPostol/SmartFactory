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
namespace CAS.SmartFactory.xml.erp {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/SKUCigarettesSchema.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/jti/ipr/SKUCigarettesSchema.xsd", IsNullable=true)]
    public partial class Cigarettes {
        
        private CigarettesMaterial[] materialField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("Material", IsNullable=true)]
        public CigarettesMaterial[] Material {
            get {
                return this.materialField;
            }
            set {
                this.materialField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/jti/ipr/SKUCigarettesSchema.xsd")]
    public partial class CigarettesMaterial {
        
        private string plantField;
        
        private string materialField;
        
        private string material_DescriptionField;
        
        private string familyField;
        
        private string family_DesField;
        
        private string brandField;
        
        private string brand_DescriptionField;
        
        private string cigarette_LengthField;
        
        private string filter_Segment_LengthField;
        
        private string pack_Style_CodeField;
        
        private string pack_StyleField;
        
        private string pack_TypeField;
        
        private string pack_Type_DescriptioField;
        
        private string pack_ProfileField;
        
        private string pack_Profile_DescripField;
        
        private string outer_Type_CodeField;
        
        private string outer_TypeField;
        
        private string prime_MarketField;
        
        private string prime_MarkField;
        
        private string graphic_Feature_CodeField;
        
        private string graphic_Feature_Code2Field;
        
        private string mentholField;
        
        private string menthol_DeField;
        
        private string cigarettes_Per_PackField;
        
        private string cigarettes_per_OuterField;
        
        private string cigarettes_Per_CaseField;
        
        private string foil_Embossing_Art_Work_NoField;
        
        private string foil_Embossing_Art_WField;
        
        private string foil_Embossing_PositionField;
        
        private string foil_EmbosField;
        
        private string coupon_Insert_CodeField;
        
        private string coupon_Insert_Code_DescriptionField;
        
        private string cases_per_PalletField;
        
        private string pallet_TypeField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Plant {
            get {
                return this.plantField;
            }
            set {
                this.plantField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Material {
            get {
                return this.materialField;
            }
            set {
                this.materialField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Material_Description {
            get {
                return this.material_DescriptionField;
            }
            set {
                this.material_DescriptionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Family {
            get {
                return this.familyField;
            }
            set {
                this.familyField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Family_Des {
            get {
                return this.family_DesField;
            }
            set {
                this.family_DesField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Brand {
            get {
                return this.brandField;
            }
            set {
                this.brandField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Brand_Description {
            get {
                return this.brand_DescriptionField;
            }
            set {
                this.brand_DescriptionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Cigarette_Length {
            get {
                return this.cigarette_LengthField;
            }
            set {
                this.cigarette_LengthField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Filter_Segment_Length {
            get {
                return this.filter_Segment_LengthField;
            }
            set {
                this.filter_Segment_LengthField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Style_Code {
            get {
                return this.pack_Style_CodeField;
            }
            set {
                this.pack_Style_CodeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Style {
            get {
                return this.pack_StyleField;
            }
            set {
                this.pack_StyleField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Type {
            get {
                return this.pack_TypeField;
            }
            set {
                this.pack_TypeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Type_Descriptio {
            get {
                return this.pack_Type_DescriptioField;
            }
            set {
                this.pack_Type_DescriptioField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Profile {
            get {
                return this.pack_ProfileField;
            }
            set {
                this.pack_ProfileField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pack_Profile_Descrip {
            get {
                return this.pack_Profile_DescripField;
            }
            set {
                this.pack_Profile_DescripField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Outer_Type_Code {
            get {
                return this.outer_Type_CodeField;
            }
            set {
                this.outer_Type_CodeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Outer_Type {
            get {
                return this.outer_TypeField;
            }
            set {
                this.outer_TypeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Prime_Market {
            get {
                return this.prime_MarketField;
            }
            set {
                this.prime_MarketField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Prime_Mark {
            get {
                return this.prime_MarkField;
            }
            set {
                this.prime_MarkField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Graphic_Feature_Code {
            get {
                return this.graphic_Feature_CodeField;
            }
            set {
                this.graphic_Feature_CodeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Graphic_Feature_Code2 {
            get {
                return this.graphic_Feature_Code2Field;
            }
            set {
                this.graphic_Feature_Code2Field = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Menthol {
            get {
                return this.mentholField;
            }
            set {
                this.mentholField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Menthol_De {
            get {
                return this.menthol_DeField;
            }
            set {
                this.menthol_DeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Cigarettes_Per_Pack {
            get {
                return this.cigarettes_Per_PackField;
            }
            set {
                this.cigarettes_Per_PackField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Cigarettes_per_Outer {
            get {
                return this.cigarettes_per_OuterField;
            }
            set {
                this.cigarettes_per_OuterField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Cigarettes_Per_Case {
            get {
                return this.cigarettes_Per_CaseField;
            }
            set {
                this.cigarettes_Per_CaseField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Foil_Embossing_Art_Work_No {
            get {
                return this.foil_Embossing_Art_Work_NoField;
            }
            set {
                this.foil_Embossing_Art_Work_NoField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Foil_Embossing_Art_W {
            get {
                return this.foil_Embossing_Art_WField;
            }
            set {
                this.foil_Embossing_Art_WField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Foil_Embossing_Position {
            get {
                return this.foil_Embossing_PositionField;
            }
            set {
                this.foil_Embossing_PositionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Foil_Embos {
            get {
                return this.foil_EmbosField;
            }
            set {
                this.foil_EmbosField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Coupon_Insert_Code {
            get {
                return this.coupon_Insert_CodeField;
            }
            set {
                this.coupon_Insert_CodeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Coupon_Insert_Code_Description {
            get {
                return this.coupon_Insert_Code_DescriptionField;
            }
            set {
                this.coupon_Insert_Code_DescriptionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Cases_per_Pallet {
            get {
                return this.cases_per_PalletField;
            }
            set {
                this.cases_per_PalletField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Pallet_Type {
            get {
                return this.pallet_TypeField;
            }
            set {
                this.pallet_TypeField = value;
            }
        }
    }
}
