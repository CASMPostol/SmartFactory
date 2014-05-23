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

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseAccount
{
  public sealed partial class CloseAccount
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
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      this.CompletedWorkflowLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CloseAccountActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingWorkflowLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onAccountClosingActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // CompletedWorkflowLogToHistory
      // 
      this.CompletedWorkflowLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.CompletedWorkflowLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "CloseAccount";
      activitybind1.Path = "CompletedLogToHistory_HistoryDescription";
      activitybind2.Name = "CloseAccount";
      activitybind2.Path = "CompletedLogToHistory_HistoryOutcome";
      this.CompletedWorkflowLogToHistory.Name = "CompletedWorkflowLogToHistory";
      this.CompletedWorkflowLogToHistory.OtherData = "";
      activitybind3.Name = "CloseAccount";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.CompletedWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      this.CompletedWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.CompletedWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // CloseAccountActivity
      // 
      this.CloseAccountActivity.Description = "Main code block closing the account.";
      this.CloseAccountActivity.Name = "CloseAccountActivity";
      this.CloseAccountActivity.ExecuteCode += new System.EventHandler(this.CloseAccountActivity_ExecuteCode);
      // 
      // StartingWorkflowLogToHistory
      // 
      this.StartingWorkflowLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartingWorkflowLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind4.Name = "CloseAccount";
      activitybind4.Path = "StartingWorkflowLogToHistory_HistoryDescription";
      activitybind5.Name = "CloseAccount";
      activitybind5.Path = "StartingWorkflowLogToHistory_HistoryOutcome";
      this.StartingWorkflowLogToHistory.Name = "StartingWorkflowLogToHistory";
      this.StartingWorkflowLogToHistory.OtherData = "";
      activitybind6.Name = "CloseAccount";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.StartingWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.StartingWorkflowLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      activitybind8.Name = "CloseAccount";
      activitybind8.Path = "workflowId";
      // 
      // onAccountClosingActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CloseAccount";
      this.onAccountClosingActivated.CorrelationToken = correlationtoken1;
      this.onAccountClosingActivated.EventName = "OnWorkflowActivated";
      this.onAccountClosingActivated.Name = "onAccountClosingActivated";
      activitybind7.Name = "CloseAccount";
      activitybind7.Path = "workflowProperties";
      this.onAccountClosingActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.onAccountClosingActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      // 
      // CloseAccount
      // 
      this.Activities.Add(this.onAccountClosingActivated);
      this.Activities.Add(this.StartingWorkflowLogToHistory);
      this.Activities.Add(this.CloseAccountActivity);
      this.Activities.Add(this.CompletedWorkflowLogToHistory);
      this.Name = "CloseAccount";
      this.CanModifyActivities = false;

    }

    #endregion

    private CodeActivity CloseAccountActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity CompletedWorkflowLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingWorkflowLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onAccountClosingActivated;














  }
}
