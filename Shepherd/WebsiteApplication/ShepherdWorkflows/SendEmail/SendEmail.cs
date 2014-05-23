using System;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.SendEmail
{
  public sealed partial class SendEmail : SequentialWorkflowActivity
  {
    public SendEmail()
    {
      InitializeComponent();
    }
    #region Activation
    private void m_onWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e) { }

    internal static Guid WorkflowId = new Guid("1bb10ba9-70da-4064-95b3-cd048ce4c3cd");
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_WorkflowProperties = default(SPWorkflowActivationProperties);
    #endregion

    #region SentEmail
    private void m_sendEmail1_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        POLibraryWorkflowAssociationData _activationData = POLibraryWorkflowAssociationData.Deserialize(m_WorkflowProperties.InitiationData);
        IPurchaseOrderTemplate _emailBodyObject = null;
        using (EntitiesDataContext _EDC = new EntitiesDataContext(m_WorkflowProperties.SiteUrl))
        {
          if (_activationData.Carrier)
            _emailBodyObject = FreightPurchaseOrderTemplate.CreateEmailMessage(m_WorkflowProperties.Item, _EDC);
          else
            _emailBodyObject = SecurityEscortPurchaseOrderTemplate.CreateEmailMessage(m_WorkflowProperties.Item, _EDC);
          m_sendEmail1_CC = DistributionList.GetEmail(ShepherdRole.Coordinator, _EDC);
        }
        m_sendEmail1_From = m_WorkflowProperties.OriginatorEmail;
        m_sendEmail1_Subject = _activationData.Name;
        m_sendEmail1_To = _emailBodyObject.EmaiAddressTo;
        m_sendEmail1_Body = _emailBodyObject.TransformText();
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in TransformText because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    public String m_sendEmail1_Body = default(System.String);
    public String m_sendEmail1_CC = default(System.String);
    public String m_sendEmail1_From = default(System.String);
    public String m_sendEmail1_Subject = default(System.String);
    public String m_sendEmail1_To = default(System.String);
    #endregion

    #region LogToHistory
    private void m_logToHistoryListActivity1_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        m_logToHistoryListActivity1_HistoryDescription = String.Format("Sending notification to: {0}", m_sendEmail1_To);
        m_logToHistoryListActivity1_HistoryOutcome = m_WorkflowProperties.Workflow.ParentAssociation.Name;
        m_logToHistoryListActivity1_UserId = m_WorkflowProperties.OriginatorUser.ID;
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in LogToHistoryListActivity because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    public Int32 m_logToHistoryListActivity1_UserId = default(System.Int32);
    public String m_logToHistoryListActivity1_HistoryDescription = default(System.String);
    public String m_logToHistoryListActivity1_HistoryOutcome = default(System.String);
    public String m_logToHistoryListActivity1_OtherData = default(System.String);
    #endregion

    #region OnFaultLog
    public String _OnFaultLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String _OnFaultLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void _OnFaultLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      _OnFaultLogToHistoryListActivity_HistoryOutcome1 = "Error";
      _OnFaultLogToHistoryListActivity_HistoryDescription1 = m_FaultHandlerActivity.Fault.Message;
    }
    #endregion
  }
}
