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
    public SPWorkflowActivationProperties m_WorkflowProperties = new SPWorkflowActivationProperties();
    private string m_EmailSubject;
    private string m_EmailBody;
    private void m_onWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      // deserialize initiation data; 
      XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
      XmlTextReader reader = new XmlTextReader(new StringReader(m_WorkflowProperties.InitiationData));
      POLibraryWorkflowAssociationData _activationData = (POLibraryWorkflowAssociationData)serializer.Deserialize(reader);
      // assign form data values to workflow fields 
      EmailMessage(_activationData);
    }
    private void EmailMessage(POLibraryWorkflowAssociationData _activationData)
    {
      m_EmailSubject = _activationData.Title;
      StringBuilder _body = new StringBuilder();
      _body.AppendFormat("HistoryListUrl = {0}", m_WorkflowProperties.HistoryListUrl);
      _body.AppendLine();
      _body.AppendFormat("DisplayName = {0}", m_WorkflowProperties.Item.DisplayName);
      _body.AppendLine();
      _body.AppendFormat("File.Name = {0}", m_WorkflowProperties.Item.File.Name);
      _body.AppendLine();
      _body.AppendFormat("Name = {0}", m_WorkflowProperties.Item.Name);
      _body.AppendLine();
      _body.AppendFormat("Title = {0}", m_WorkflowProperties.Item.Title);
      _body.AppendLine();
      _body.AppendFormat("Url = {0}", m_WorkflowProperties.Item.Url);
      _body.AppendLine();
      _body.AppendFormat("Xml = {0}", m_WorkflowProperties.Item.Xml);
      _body.AppendLine();
      m_EmailBody = _body.ToString();
    }
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

    private void m_sendEmail1_MethodInvoking(object sender, EventArgs e)
    {
      m_sendEmail1_Body = m_EmailBody;
      m_sendEmail1_CC = m_WorkflowProperties.OriginatorEmail;
      m_sendEmail1_From = m_WorkflowProperties.OriginatorEmail;
      m_sendEmail1_Subject = m_EmailSubject;
      m_sendEmail1_To = "mpostol@case.eu";
    }
    public String m_sendEmail1_Body = default(System.String);
    public String m_sendEmail1_CC = default(System.String);
    public String m_sendEmail1_From = default(System.String);
    public String m_sendEmail1_Subject = default(System.String);
    public String m_sendEmail1_To = default(System.String);

  }
}
