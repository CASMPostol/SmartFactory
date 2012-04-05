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

namespace CAS.SmartFactory.Shepherd.SendNotification.AddTimeSlots
{
  public sealed partial class AddTimeSlots
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
      this.FinischLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.AddTimeslotsActivity = new System.Workflow.Activities.CodeActivity();
      this.StartLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // FinischLogToHistory
      // 
      this.FinischLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.FinischLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "AddTimeSlots";
      activitybind1.Path = "FinischLogToHistory_HistoryDescription2";
      this.FinischLogToHistory.HistoryOutcome = "Finisched";
      this.FinischLogToHistory.Name = "FinischLogToHistory";
      this.FinischLogToHistory.OtherData = "";
      activitybind2.Name = "AddTimeSlots";
      activitybind2.Path = "workflowProperties.OriginatorUser.ID";
      this.FinischLogToHistory.MethodInvoking += new System.EventHandler(this.FinischLogToHistory_MethodInvoking);
      this.FinischLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.FinischLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // AddTimeslotsActivity
      // 
      this.AddTimeslotsActivity.Name = "AddTimeslotsActivity";
      this.AddTimeslotsActivity.ExecuteCode += new System.EventHandler(this.AddTimeslotsActivity_ExecuteCode);
      // 
      // StartLogToHistory
      // 
      this.StartLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind3.Name = "AddTimeSlots";
      activitybind3.Path = "StartLogToHistory_HistoryDescription";
      this.StartLogToHistory.HistoryOutcome = "Starting";
      this.StartLogToHistory.Name = "StartLogToHistory";
      this.StartLogToHistory.OtherData = "";
      activitybind4.Name = "AddTimeSlots";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.StartLogToHistory.MethodInvoking += new System.EventHandler(this.StartLogToHistory_MethodInvoking);
      this.StartLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      this.StartLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      activitybind6.Name = "AddTimeSlots";
      activitybind6.Path = "workflowId";
      // 
      // onWorkflowActivated1
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "AddTimeSlots";
      this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated1.Name = "onWorkflowActivated1";
      activitybind5.Name = "AddTimeSlots";
      activitybind5.Path = "workflowProperties";
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // AddTimeSlots
      // 
      this.Activities.Add(this.onWorkflowActivated1);
      this.Activities.Add(this.StartLogToHistory);
      this.Activities.Add(this.AddTimeslotsActivity);
      this.Activities.Add(this.FinischLogToHistory);
      this.Name = "AddTimeSlots";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FinischLogToHistory;

    private CodeActivity AddTimeslotsActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;








  }
}
