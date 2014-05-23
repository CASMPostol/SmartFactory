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

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseReport.DoReportWorkflow
{
  public sealed partial class DoReportWorkflow
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
      this.CompletedLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.DoReport = new System.Workflow.Activities.CodeActivity();
      this.StartingLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // CompletedLogToHistory
      // 
      this.CompletedLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.CompletedLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "DoReportWorkflow";
      activitybind1.Path = "CompletedLogToHistory_HistoryDescription";
      activitybind2.Name = "DoReportWorkflow";
      activitybind2.Path = "CompletedLogToHistory_HistoryOutcome";
      this.CompletedLogToHistory.Name = "CompletedLogToHistory";
      this.CompletedLogToHistory.OtherData = "";
      activitybind3.Name = "DoReportWorkflow";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.CompletedLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.CompletedLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.CompletedLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // DoReport
      // 
      this.DoReport.Name = "DoReport";
      this.DoReport.ExecuteCode += new System.EventHandler(this.DoReport_ExecuteCode);
      // 
      // StartingLogToHistory
      // 
      this.StartingLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartingLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind4.Name = "DoReportWorkflow";
      activitybind4.Path = "StartingLogToHistory_HistoryDescription";
      activitybind5.Name = "DoReportWorkflow";
      activitybind5.Path = "StartingLogToHistory_HistoryOutcome";
      this.StartingLogToHistory.Name = "StartingLogToHistory";
      this.StartingLogToHistory.OtherData = "";
      activitybind6.Name = "DoReportWorkflow";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.StartingLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.StartingLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      activitybind8.Name = "DoReportWorkflow";
      activitybind8.Path = "workflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "DoReportWorkflow";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind7.Name = "DoReportWorkflow";
      activitybind7.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      // 
      // DoReportWorkflow
      // 
      this.Activities.Add(this.onWorkflowActivated);
      this.Activities.Add(this.StartingLogToHistory);
      this.Activities.Add(this.DoReport);
      this.Activities.Add(this.CompletedLogToHistory);
      this.Name = "DoReportWorkflow";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity CompletedLogToHistory;

    private CodeActivity DoReport;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;








  }
}
