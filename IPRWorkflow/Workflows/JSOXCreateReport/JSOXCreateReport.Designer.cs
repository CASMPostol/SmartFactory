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

namespace CAS.SmartFactory.IPR.Workflows.JSOXCreateReport
{
  public sealed partial class JSOXCreateReport
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
      this.CompletedLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateJSOXReportActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // CompletedLogToHistory
      // 
      this.CompletedLogToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.CompletedLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "JSOXCreateReport";
      activitybind1.Path = "CompletedLogToHistory_HistoryDescription";
      activitybind2.Name = "JSOXCreateReport";
      activitybind2.Path = "CompletedLogToHistory_HistoryOutcome";
      this.CompletedLogToHistory.Name = "CompletedLogToHistory";
      this.CompletedLogToHistory.OtherData = "";
      activitybind3.Name = "JSOXCreateReport";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.CompletedLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.CompletedLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      this.CompletedLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // CreateJSOXReportActivity
      // 
      this.CreateJSOXReportActivity.Name = "CreateJSOXReportActivity";
      this.CreateJSOXReportActivity.ExecuteCode += new System.EventHandler( this.CreateJSOXReport );
      // 
      // StartingLogToHistory
      // 
      this.StartingLogToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartingLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.StartingLogToHistory.HistoryDescription = "JSOX creation starting";
      this.StartingLogToHistory.HistoryOutcome = "Starting";
      this.StartingLogToHistory.Name = "StartingLogToHistory";
      this.StartingLogToHistory.OtherData = "";
      activitybind4.Name = "JSOXCreateReport";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      activitybind6.Name = "JSOXCreateReport";
      activitybind6.Path = "workflowId";
      // 
      // onWorkflowActivated1
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "JSOXCreateReport";
      this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated1.Description = "Creates JSOX report";
      this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated1.Name = "onWorkflowActivated1";
      activitybind5.Name = "JSOXCreateReport";
      activitybind5.Path = "workflowProperties";
      this.onWorkflowActivated1.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ) );
      this.onWorkflowActivated1.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      // 
      // JSOXCreateReport
      // 
      this.Activities.Add( this.onWorkflowActivated1 );
      this.Activities.Add( this.StartingLogToHistory );
      this.Activities.Add( this.CreateJSOXReportActivity );
      this.Activities.Add( this.CompletedLogToHistory );
      this.Name = "JSOXCreateReport";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity CompletedLogToHistory;

    private CodeActivity CreateJSOXReportActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;









  }
}
