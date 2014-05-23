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

namespace CAS.SmartFactory.Shepherd.Workflows.CreatePO
{
  public sealed partial class CreatePO
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
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_CreatePOFaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.m_FaultHandlersActivity = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.m_LogAfterCreateToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_CodeActivity = new System.Workflow.Activities.CodeActivity();
      this.m_OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // logToHistoryListActivity1
      // 
      this.logToHistoryListActivity1.Description = "Write information about exception to the worflow history list.";
      this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
      activitybind1.Name = "CreatePO";
      activitybind1.Path = "m_LogAfterCreateToHistoryList_HistoryDescription1";
      activitybind2.Name = "CreatePO";
      activitybind2.Path = "m_LogAfterCreateToHistoryList_HistoryOutcome1";
      this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
      activitybind3.Name = "CreatePO";
      activitybind3.Path = "m_LogAfterCreateToHistoryList_OtherData1";
      activitybind4.Name = "CreatePO";
      activitybind4.Path = "m_LogAfterCreateToHistoryList_UserId1";
      this.logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.logToHistoryListActivity1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.OtherDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // m_CreatePOFaultHandlerActivity
      // 
      this.m_CreatePOFaultHandlerActivity.Activities.Add(this.logToHistoryListActivity1);
      this.m_CreatePOFaultHandlerActivity.Description = "Eception handler";
      this.m_CreatePOFaultHandlerActivity.FaultType = typeof(System.Exception);
      this.m_CreatePOFaultHandlerActivity.Name = "m_CreatePOFaultHandlerActivity";
      // 
      // m_FaultHandlersActivity
      // 
      this.m_FaultHandlersActivity.Activities.Add(this.m_CreatePOFaultHandlerActivity);
      this.m_FaultHandlersActivity.Name = "m_FaultHandlersActivity";
      // 
      // m_LogAfterCreateToHistoryList
      // 
      this.m_LogAfterCreateToHistoryList.Description = "Log After Create To History List";
      this.m_LogAfterCreateToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_LogAfterCreateToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind5.Name = "CreatePO";
      activitybind5.Path = "m_LogAfterCreateToHistoryList_HistoryDescription1";
      activitybind6.Name = "CreatePO";
      activitybind6.Path = "m_LogAfterCreateToHistoryList_HistoryOutcome1";
      this.m_LogAfterCreateToHistoryList.Name = "m_LogAfterCreateToHistoryList";
      activitybind7.Name = "CreatePO";
      activitybind7.Path = "m_LogAfterCreateToHistoryList_OtherData1";
      activitybind8.Name = "CreatePO";
      activitybind8.Path = "m_LogAfterCreateToHistoryList_UserId1";
      this.m_LogAfterCreateToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.m_LogAfterCreateToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.m_LogAfterCreateToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.m_LogAfterCreateToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.OtherDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      // 
      // m_CodeActivity
      // 
      this.m_CodeActivity.Description = "Create new PO entry";
      this.m_CodeActivity.Name = "m_CodeActivity";
      this.m_CodeActivity.ExecuteCode += new System.EventHandler(this.CreatePOItem);
      activitybind10.Name = "CreatePO";
      activitybind10.Path = "workflowId";
      // 
      // m_OnWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CreatePO";
      this.m_OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated.Name = "m_OnWorkflowActivated";
      activitybind9.Name = "CreatePO";
      activitybind9.Path = "m_WorkflowProperties";
      this.m_OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated_Invoked);
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      // 
      // CreatePO
      // 
      this.Activities.Add(this.m_OnWorkflowActivated);
      this.Activities.Add(this.m_CodeActivity);
      this.Activities.Add(this.m_LogAfterCreateToHistoryList);
      this.Activities.Add(this.m_FaultHandlersActivity);
      this.Name = "CreatePO";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

    private FaultHandlerActivity m_CreatePOFaultHandlerActivity;

    private FaultHandlersActivity m_FaultHandlersActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_LogAfterCreateToHistoryList;

    private CodeActivity m_CodeActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated;






































  }
}
