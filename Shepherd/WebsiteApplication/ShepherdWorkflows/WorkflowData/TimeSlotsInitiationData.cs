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
namespace CAS.SmartFactory.Shepherd.Workflows.WorkflowData {
    using System.Xml.Serialization;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cas.eu/schemas/SmartFactory/Shepherd/SendNotification/WorkflowData/TimeSlo" +
        "tsInitiationData.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cas.eu/schemas/SmartFactory/Shepherd/SendNotification/WorkflowData/TimeSlo" +
        "tsInitiationData.xsd", IsNullable=false)]
    public partial class TimeSlotsInitiationData {
        
        private System.DateTime startDateField;
        
        private int durationField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime StartDate {
            get {
                return this.startDateField;
            }
            set {
                this.startDateField = value;
            }
        }
        
        /// <uwagi/>
        public int Duration {
            get {
                return this.durationField;
            }
            set {
                this.durationField = value;
            }
        }


    }
}
