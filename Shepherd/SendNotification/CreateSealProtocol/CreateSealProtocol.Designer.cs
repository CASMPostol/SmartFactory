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

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSealProtocol
{
  public sealed partial class CreateSealProtocol
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
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
      this.m_AfterCreationLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_CreatePO = new System.Workflow.Activities.CodeActivity();
      this.m_OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // m_AfterCreationLogToHistoryList
      // 
      this.m_AfterCreationLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_AfterCreationLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "CreateSealProtocol";
      activitybind1.Path = "m_AfterCreationLogToHistoryList_HistoryDescription1";
      activitybind2.Name = "CreateSealProtocol";
      activitybind2.Path = "m_AfterCreationLogToHistoryList_HistoryOutcome1";
      this.m_AfterCreationLogToHistoryList.Name = "m_AfterCreationLogToHistoryList";
      this.m_AfterCreationLogToHistoryList.OtherData = "";
      activitybind3.Name = "CreateSealProtocol";
      activitybind3.Path = "m_AfterCreationLogToHistoryList_UserId1";
      this.m_AfterCreationLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.m_AfterCreationLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.m_AfterCreationLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // m_CreatePO
      // 
      this.m_CreatePO.Name = "m_CreatePO";
      this.m_CreatePO.ExecuteCode += new System.EventHandler(this.m_CreatePO_ExecuteCode);
      activitybind5.Name = "CreateSealProtocol";
      activitybind5.Path = "workflowId";
      // 
      // m_OnWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CreateSealProtocol";
      this.m_OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated.Description = "On Workflow Activated";
      this.m_OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated.Name = "m_OnWorkflowActivated";
      activitybind4.Name = "CreateSealProtocol";
      activitybind4.Path = "workflowProperties";
      this.m_OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated1_Invoked);
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      // 
      // CreateSealProtocol
      // 
      this.Activities.Add(this.m_OnWorkflowActivated);
      this.Activities.Add(this.m_CreatePO);
      this.Activities.Add(this.m_AfterCreationLogToHistoryList);
      this.Name = "CreateSealProtocol";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_AfterCreationLogToHistoryList;

    private CodeActivity m_CreatePO;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated;

  }
}
