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

namespace CAS.SmartFactory.IPR.Workflows.IPRClosing
{
  public sealed partial class IPRClosing
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
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      this.LogFinalMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.Closeing = new System.Workflow.Activities.CodeActivity();
      this.LogClosingMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // LogFinalMessageToHistory
      // 
      this.LogFinalMessageToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.LogFinalMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "IPRClosing";
      activitybind1.Path = "LogFinalMessageToHistory_HistoryDescription";
      activitybind2.Name = "IPRClosing";
      activitybind2.Path = "LogFinalMessageToHistory_HistoryOutcome";
      this.LogFinalMessageToHistory.Name = "LogFinalMessageToHistory";
      this.LogFinalMessageToHistory.OtherData = "";
      activitybind3.Name = "IPRClosing";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.LogFinalMessageToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.LogFinalMessageToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.LogFinalMessageToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // Closeing
      // 
      this.Closeing.Description = "Closing IPR record procedure";
      this.Closeing.Name = "Closeing";
      this.Closeing.ExecuteCode += new System.EventHandler(this.ClosingExecuteCode);
      // 
      // LogClosingMessageToHistory
      // 
      this.LogClosingMessageToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.LogClosingMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.LogClosingMessageToHistory.HistoryDescription = "The record is ready to be closed. Closing procedure started.";
      this.LogClosingMessageToHistory.HistoryOutcome = "Close starting";
      this.LogClosingMessageToHistory.Name = "LogClosingMessageToHistory";
      this.LogClosingMessageToHistory.OtherData = "";
      activitybind4.Name = "IPRClosing";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.LogClosingMessageToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      activitybind6.Name = "IPRClosing";
      activitybind6.Path = "workflowId";
      // 
      // onWorkflowActivated1
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "IPRClosing";
      this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated1.Name = "onWorkflowActivated1";
      activitybind5.Name = "IPRClosing";
      activitybind5.Path = "workflowProperties";
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // IPRClosing
      // 
      this.Activities.Add(this.onWorkflowActivated1);
      this.Activities.Add(this.LogClosingMessageToHistory);
      this.Activities.Add(this.Closeing);
      this.Activities.Add(this.LogFinalMessageToHistory);
      this.Name = "IPRClosing";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogFinalMessageToHistory;

    private CodeActivity Closeing;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogClosingMessageToHistory;





  }
}
