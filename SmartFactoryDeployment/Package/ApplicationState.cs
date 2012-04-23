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
namespace CAS.SmartFactory.Deployment.Package {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd", IsNullable=true)]
    public partial class InstallationStateData {
        
        private string ownerLoginField;
        
        private string ownerNameField;
        
        private string ownerEmailField;
        
        private string siteCollectionURLField;
        
        private bool siteCollectionCreatedField;
        
        private string webApplicationURLField;
        
        private string siteTemplateField;
        
        private uint lCIDField;
        
        private string titleField;
        
        private string descriptionField;
        
        private Solution[] solutionsField;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationStateData"/> class.
        /// </summary>
        public InstallationStateData() {
            this.siteCollectionCreatedField = false;
            this.lCIDField = ((uint)(1033));
        }
        
        /// <uwagi/>
        public string OwnerLogin {
            get {
                return this.ownerLoginField;
            }
            set {
                this.ownerLoginField = value;
            }
        }
        
        /// <uwagi/>
        public string OwnerName {
            get {
                return this.ownerNameField;
            }
            set {
                this.ownerNameField = value;
            }
        }
        
        /// <uwagi/>
        public string OwnerEmail {
            get {
                return this.ownerEmailField;
            }
            set {
                this.ownerEmailField = value;
            }
        }
        
        /// <uwagi/>
        public string SiteCollectionURL {
            get {
                return this.siteCollectionURLField;
            }
            set {
                this.siteCollectionURLField = value;
            }
        }
        
        /// <uwagi/>
        public bool SiteCollectionCreated {
            get {
                return this.siteCollectionCreatedField;
            }
            set {
                this.siteCollectionCreatedField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
        public string WebApplicationURL {
            get {
                return this.webApplicationURLField;
            }
            set {
                this.webApplicationURLField = value;
            }
        }
        
        /// <uwagi/>
        public string SiteTemplate {
            get {
                return this.siteTemplateField;
            }
            set {
                this.siteTemplateField = value;
            }
        }
        
        /// <uwagi/>
        public uint LCID {
            get {
                return this.lCIDField;
            }
            set {
                this.lCIDField = value;
            }
        }
        
        /// <uwagi/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
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
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Solution[] Solutions {
            get {
                return this.solutionsField;
            }
            set {
                this.solutionsField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd")]
    public partial class Solution {
        
        private bool activatedField;
        
        private bool deployedField;
        
        private string fileNameField;
        
        private Feature[] feturesField;
        
        private FeatureDefinitionScope featureDefinitionScopeField;
        
        private int priorityField;
        
        private string solutionIDField;
        
        private bool globalField;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution() {
            this.activatedField = false;
            this.deployedField = false;
        }
        
        /// <uwagi/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Activated {
            get {
                return this.activatedField;
            }
            set {
                this.activatedField = value;
            }
        }
        
        /// <uwagi/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool Deployed {
            get {
                return this.deployedField;
            }
            set {
                this.deployedField = value;
            }
        }
        
        /// <uwagi/>
        public string FileName {
            get {
                return this.fileNameField;
            }
            set {
                this.fileNameField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Feature[] Fetures {
            get {
                return this.feturesField;
            }
            set {
                this.feturesField = value;
            }
        }
        
        /// <uwagi/>
        public FeatureDefinitionScope FeatureDefinitionScope {
            get {
                return this.featureDefinitionScopeField;
            }
            set {
                this.featureDefinitionScopeField = value;
            }
        }
        
        /// <uwagi/>
        public int Priority {
            get {
                return this.priorityField;
            }
            set {
                this.priorityField = value;
            }
        }
        
        /// <uwagi/>
        public string SolutionID {
            get {
                return this.solutionIDField;
            }
            set {
                this.solutionIDField = value;
            }
        }
        
        /// <uwagi/>
        public bool Global {
            get {
                return this.globalField;
            }
            set {
                this.globalField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd")]
    public partial class Feature {
        
        private string displayNameField;
        
        private string definitionIdField;
        
        private string versionField;
        
        private FeatureScope scopeField;
        
        private bool scopeFieldSpecified;
        
        /// <uwagi/>
        public string DisplayName {
            get {
                return this.displayNameField;
            }
            set {
                this.displayNameField = value;
            }
        }
        
        /// <uwagi/>
        public string DefinitionId {
            get {
                return this.definitionIdField;
            }
            set {
                this.definitionIdField = value;
            }
        }
        
        /// <uwagi/>
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <uwagi/>
        public FeatureScope Scope {
            get {
                return this.scopeField;
            }
            set {
                this.scopeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ScopeSpecified {
            get {
                return this.scopeFieldSpecified;
            }
            set {
                this.scopeFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd")]
    public enum FeatureScope {
        
        /// <uwagi/>
        ScopeInvalid,
        
        /// <uwagi/>
        Farm,
        
        /// <uwagi/>
        WebApplication,
        
        /// <uwagi/>
        Site,
        
        /// <uwagi/>
        Web,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cas.eu/schemas/SharePoint/Deployment/2/ApplicationState.xsd")]
    public enum FeatureDefinitionScope {
        
        /// <uwagi/>
        None,
        
        /// <uwagi/>
        Farm,
        
        /// <uwagi/>
        Site,
    }
}
