﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAS.SmartFactory.Deployment.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sites/shr")]
        public string SiteCollectionURL {
            get {
                return ((string)(this["SiteCollectionURL"]));
            }
            set {
                this["SiteCollectionURL"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{693191fd-6a7a-4db6-ac8e-a3f747ddf439}")]
        public string SiteCollectionFeatureGuid {
            get {
                return ((string)(this["SiteCollectionFeatureGuid"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Solutions\\ShepherdWebsite.wsp")]
        public string SiteCollectionSolutionFileName {
            get {
                return ((string)(this["SiteCollectionSolutionFileName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Solutions\\ShepherdDashboards.wsp")]
        public string FarmSolutionFileName {
            get {
                return ((string)(this["FarmSolutionFileName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{3d8f8cb5-f8bb-496a-b561-fb3d36e8d84d}")]
        public string FarmFeatureGuid {
            get {
                return ((string)(this["FarmFeatureGuid"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ManualMode {
            get {
                return ((bool)(this["ManualMode"]));
            }
            set {
                this["ManualMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SmartFactoryInstallationState.cas.xml")]
        public string InstallationStateFileName {
            get {
                return ((string)(this["InstallationStateFileName"]));
            }
            set {
                this["InstallationStateFileName"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public short FeatureActivationTimeOut {
            get {
                return ((short)(this["FeatureActivationTimeOut"]));
            }
        }
    }
}
