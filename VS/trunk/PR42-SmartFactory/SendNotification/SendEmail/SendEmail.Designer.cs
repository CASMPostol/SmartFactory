using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CAS.SmartFactory.Shepherd.SendNotification.SendEmail
{
  public sealed partial class SendEmail
  {
    #region Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCode]
    private void InitializeComponent()
    {
      this.CanModifyActivities = true;
      System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
      this._OnFaultLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_FaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.m_faultHandlersActivity = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.m_logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_sendEmail = new Microsoft.SharePoint.WorkflowActions.SendEmail();
      this.m_onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // _OnFaultLogToHistoryListActivity
      // 
      this._OnFaultLogToHistoryListActivity.Description = "On Fault Log To History List";
      this._OnFaultLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this._OnFaultLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
      activitybind1.Name = "SendEmail";
      activitybind1.Path = "_OnFaultLogToHistoryListActivity_HistoryDescription1";
      activitybind2.Name = "SendEmail";
      activitybind2.Path = "_OnFaultLogToHistoryListActivity_HistoryOutcome1";
      this._OnFaultLogToHistoryListActivity.Name = "_OnFaultLogToHistoryListActivity";
      this._OnFaultLogToHistoryListActivity.OtherData = "";
      this._OnFaultLogToHistoryListActivity.UserId = -1;
      this._OnFaultLogToHistoryListActivity.MethodInvoking += new System.EventHandler(this._OnFaultLogToHistoryListActivity_MethodInvoking);
      this._OnFaultLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this._OnFaultLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // m_FaultHandlerActivity
      // 
      this.m_FaultHandlerActivity.Activities.Add(this._OnFaultLogToHistoryListActivity);
      this.m_FaultHandlerActivity.Description = "Fault Handler Activity";
      this.m_FaultHandlerActivity.FaultType = typeof(System.Exception);
      this.m_FaultHandlerActivity.Name = "m_FaultHandlerActivity";
      // 
      // m_faultHandlersActivity
      // 
      this.m_faultHandlersActivity.Activities.Add(this.m_FaultHandlerActivity);
      this.m_faultHandlersActivity.Description = "Handles to deal with any errors";
      this.m_faultHandlersActivity.Name = "m_faultHandlersActivity";
      // 
      // m_logToHistoryListActivity1
      // 
      this.m_logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind3.Name = "SendEmail";
      activitybind3.Path = "m_logToHistoryListActivity1_HistoryDescription";
      activitybind4.Name = "SendEmail";
      activitybind4.Path = "m_logToHistoryListActivity1_HistoryOutcome";
      this.m_logToHistoryListActivity1.Name = "m_logToHistoryListActivity1";
      activitybind5.Name = "SendEmail";
      activitybind5.Path = "m_logToHistoryListActivity1_OtherData";
      activitybind6.Name = "SendEmail";
      activitybind6.Path = "m_logToHistoryListActivity1_UserId";
      this.m_logToHistoryListActivity1.MethodInvoking += new System.EventHandler(this.m_logToHistoryListActivity1_MethodInvoking);
      this.m_logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      this.m_logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.m_logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.m_logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.OtherDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // m_sendEmail
      // 
      this.m_sendEmail.BCC = null;
      activitybind7.Name = "SendEmail";
      activitybind7.Path = "m_sendEmail1_Body";
      activitybind8.Name = "SendEmail";
      activitybind8.Path = "m_sendEmail1_CC";
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "SendEmail";
      this.m_sendEmail.CorrelationToken = correlationtoken1;
      this.m_sendEmail.Description = "Send notification about new purchase order";
      activitybind9.Name = "SendEmail";
      activitybind9.Path = "m_sendEmail1_From";
      this.m_sendEmail.Headers = null;
      this.m_sendEmail.IncludeStatus = true;
      this.m_sendEmail.Name = "m_sendEmail";
      activitybind10.Name = "SendEmail";
      activitybind10.Path = "m_sendEmail1_Subject";
      activitybind11.Name = "SendEmail";
      activitybind11.Path = "m_sendEmail1_To";
      this.m_sendEmail.MethodInvoking += new System.EventHandler(this.m_sendEmail1_MethodInvoking);
      this.m_sendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      this.m_sendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.CCProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.m_sendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      this.m_sendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      this.m_sendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
      activitybind13.Name = "SendEmail";
      activitybind13.Path = "workflowId";
      // 
      // m_onWorkflowActivated
      // 
      this.m_onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_onWorkflowActivated.Description = "Send email workflow.\r\n";
      this.m_onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_onWorkflowActivated.Name = "m_onWorkflowActivated";
      activitybind12.Name = "SendEmail";
      activitybind12.Path = "m_WorkflowProperties";
      this.m_onWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_onWorkflowActivated_Invoked);
      this.m_onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
      this.m_onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
      // 
      // SendEmail
      // 
      this.Activities.Add(this.m_onWorkflowActivated);
      this.Activities.Add(this.m_sendEmail);
      this.Activities.Add(this.m_logToHistoryListActivity1);
      this.Activities.Add(this.m_faultHandlersActivity);
      this.Name = "SendEmail";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity _OnFaultLogToHistoryListActivity;

    private FaultHandlerActivity m_FaultHandlerActivity;

    private FaultHandlersActivity m_faultHandlersActivity;

    private Microsoft.SharePoint.WorkflowActions.SendEmail m_sendEmail;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_logToHistoryListActivity1;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_onWorkflowActivated;











































  }
}
