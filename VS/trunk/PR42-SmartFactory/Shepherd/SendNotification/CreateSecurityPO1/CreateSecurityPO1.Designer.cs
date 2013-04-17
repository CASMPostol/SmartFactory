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

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSecurityPO1
{
  public sealed partial class CreateSecurityPO1
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
      this.m_AfterCreateLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_CreatePOMethod = new System.Workflow.Activities.CodeActivity();
      this.m_OnWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // m_AfterCreateLogToHistoryList
      // 
      this.m_AfterCreateLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_AfterCreateLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "CreateSecurityPO1";
      activitybind1.Path = "m_AfterCreateLogToHistoryList_HistoryDescription";
      activitybind2.Name = "CreateSecurityPO1";
      activitybind2.Path = "m_AfterCreateLogToHistoryList_HistoryOutcome";
      this.m_AfterCreateLogToHistoryList.Name = "m_AfterCreateLogToHistoryList";
      this.m_AfterCreateLogToHistoryList.OtherData = "";
      activitybind3.Name = "CreateSecurityPO1";
      activitybind3.Path = "m_AfterCreateLogToHistoryList_UserId";
      this.m_AfterCreateLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.m_AfterCreateLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.m_AfterCreateLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // m_CreatePOMethod
      // 
      this.m_CreatePOMethod.Name = "m_CreatePOMethod";
      this.m_CreatePOMethod.ExecuteCode += new System.EventHandler(this.m_CreatePOMethod_Run);
      activitybind5.Name = "CreateSecurityPO1";
      activitybind5.Path = "workflowId";
      // 
      // m_OnWorkflowActivated1
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CreateSecurityPO1";
      this.m_OnWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated1.Description = "On Workflow Activated";
      this.m_OnWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated1.Name = "m_OnWorkflowActivated1";
      activitybind4.Name = "CreateSecurityPO1";
      activitybind4.Path = "workflowProperties";
      this.m_OnWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated_Invoked);
      this.m_OnWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.m_OnWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      // 
      // CreateSecurityPO1
      // 
      this.Activities.Add(this.m_OnWorkflowActivated1);
      this.Activities.Add(this.m_CreatePOMethod);
      this.Activities.Add(this.m_AfterCreateLogToHistoryList);
      this.Name = "CreateSecurityPO1";
      this.CanModifyActivities = false;

    }

    #endregion

    private CodeActivity m_CreatePOMethod;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_AfterCreateLogToHistoryList;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated1;











  }
}
