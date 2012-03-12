using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.Xml.Serialization;
using CAS.SmartFactorySendNotification.WorkflowData;
using System.Xml;
using System.IO;
using System.Text;

namespace CAS.SmartFactorySendNotification.SendEmail
{
  public sealed partial class SendEmail : SequentialWorkflowActivity
  {
    public SendEmail()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_WorkflowProperties = default(SPWorkflowActivationProperties);
    private string m_EmailSubject;
    private FreightPurchaseOrderTemplate _emailBodyObject = new FreightPurchaseOrderTemplate();
    private void m_onWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      POLibraryWorkflowAssociationData _activationData = default(POLibraryWorkflowAssociationData);
      try
      {
        // deserialize initiation data; 
        XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
        XmlTextReader reader = new XmlTextReader(new StringReader(m_WorkflowProperties.InitiationData));
        _activationData = (POLibraryWorkflowAssociationData)serializer.Deserialize(reader);
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in WorkflowActivated because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
      _emailBodyObject = CreateEmailMessage(_activationData);
    }
    private FreightPurchaseOrderTemplate CreateEmailMessage(POLibraryWorkflowAssociationData _activationData)
    {
      try
      {
        m_EmailSubject = _activationData.Title;
        return new FreightPurchaseOrderTemplate()
        {
          Encodedabsurl = new Uri((string)m_WorkflowProperties.Item["EncodedAbsUrl"]),
          FPO2CityTitle = m_WorkflowProperties.Item["FPO2CityTitle"].Cast2String(),
          FPO2CommodityTitle = m_WorkflowProperties.Item["FPO2CommodityTitle"].Cast2String(),
          FPO2CountryTitle = m_WorkflowProperties.Item["FPO2CountryTitle"].Cast2String(),
          FPO2RouteGoodsHandlingPO = m_WorkflowProperties.Item["FPO2RouteGoodsHandlingPO"].Cast2String(),
          FPO2TransportUnitTypeTitle = m_WorkflowProperties.Item["FPO2TransportUnitTypeTitle"].Cast2String(),
          FPOLoadingDate = (DateTime)m_WorkflowProperties.Item["FPOLoadingDate"],
          Modified = (DateTime)m_WorkflowProperties.Item["Modified"],
          ModifiedBy = m_WorkflowProperties.Item["Editor"].Cast2String(),
          DocumentName = m_WorkflowProperties.Item.File.Name,
        };
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in CreateEmailMessage because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    private void m_sendEmail1_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        m_sendEmail1_Body = _emailBodyObject.TransformText();
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in TransformText because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
      m_sendEmail1_CC = m_WorkflowProperties.OriginatorEmail;
      m_sendEmail1_From = m_WorkflowProperties.OriginatorEmail;
      m_sendEmail1_Subject = m_EmailSubject;
      m_sendEmail1_To = "mpostol@cas.eu";
    }
    public String m_sendEmail1_Body = default(System.String);
    public String m_sendEmail1_CC = default(System.String);
    public String m_sendEmail1_From = default(System.String);
    public String m_sendEmail1_Subject = default(System.String);
    public String m_sendEmail1_To = default(System.String);

    private void m_logToHistoryListActivity1_MethodInvoking(object sender, EventArgs e)
    {
      m_logToHistoryListActivity1_HistoryDescription = String.Format("Sending notification to: {0}", m_WorkflowProperties.Item["Carrier"]);
      m_logToHistoryListActivity1_HistoryOutcome = m_WorkflowProperties.Workflow.ParentAssociation.Name;
      m_logToHistoryListActivity1_UserId = m_WorkflowProperties.OriginatorUser.ID;
    }
    public Int32 m_logToHistoryListActivity1_UserId = default(System.Int32);
    public String m_logToHistoryListActivity1_HistoryDescription = default(System.String);
    public String m_logToHistoryListActivity1_HistoryOutcome = default(System.String);
    public String m_logToHistoryListActivity1_OtherData = default(System.String);



    public String _OnFaultLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String _OnFaultLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void _OnFaultLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      _OnFaultLogToHistoryListActivity_HistoryOutcome1 = "Error";
      _OnFaultLogToHistoryListActivity_HistoryDescription1 = m_FaultHandlerActivity.Fault.Message;
    }
  }
}
