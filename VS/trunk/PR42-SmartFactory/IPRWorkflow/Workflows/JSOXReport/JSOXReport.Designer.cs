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

namespace CAS.SmartFactory.IPR.Workflows.JSOXReport
{
  public sealed partial class JSOXReport
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
      this.EndLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.JSOXCreateAivity = new System.Workflow.Activities.CodeActivity();
      this.StartingLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // EndLogToHistory
      // 
      this.EndLogToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.EndLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind1.Name = "JSOXReport";
      activitybind1.Path = "EndLogToHistory_HistoryDescription";
      activitybind2.Name = "JSOXReport";
      activitybind2.Path = "EndLogToHistory_HistoryOutcome";
      this.EndLogToHistory.Name = "EndLogToHistory";
      this.EndLogToHistory.OtherData = "";
      activitybind3.Name = "JSOXReport";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.EndLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.EndLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      this.EndLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // JSOXCreateAivity
      // 
      this.JSOXCreateAivity.Description = "Creates the JSOX report sheet.";
      this.JSOXCreateAivity.Name = "JSOXCreateAivity";
      this.JSOXCreateAivity.ExecuteCode += new System.EventHandler( this.CreateReport );
      // 
      // StartingLogToHistory
      // 
      this.StartingLogToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartingLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.StartingLogToHistory.HistoryDescription = "JSOX creation starting";
      this.StartingLogToHistory.HistoryOutcome = "Starting";
      this.StartingLogToHistory.Name = "StartingLogToHistory";
      this.StartingLogToHistory.OtherData = "";
      activitybind4.Name = "JSOXReport";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingLogToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      activitybind6.Name = "JSOXReport";
      activitybind6.Path = "workflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "JSOXReport";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind5.Name = "JSOXReport";
      activitybind5.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ) );
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      // 
      // JSOXReport
      // 
      this.Activities.Add( this.onWorkflowActivated );
      this.Activities.Add( this.StartingLogToHistory );
      this.Activities.Add( this.JSOXCreateAivity );
      this.Activities.Add( this.EndLogToHistory );
      this.Name = "JSOXReport";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity EndLogToHistory;

    private CodeActivity JSOXCreateAivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;






  }
}
