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
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      this.LogFinalMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.Closeing = new System.Workflow.Activities.CodeActivity();
      this.LogClosingMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.LogWarningMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ClosingSequence = new System.Workflow.Activities.SequenceActivity();
      this.IfNotValid = new System.Workflow.Activities.IfElseBranchActivity();
      this.IfValid = new System.Workflow.Activities.IfElseBranchActivity();
      this.IfRecordValid = new System.Workflow.Activities.IfElseActivity();
      this.RecordValidation = new System.Workflow.Activities.CodeActivity();
      this.LogStartingMessageToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // LogFinalMessageToHistory
      // 
      this.LogFinalMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogFinalMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "IPRClosing";
      activitybind1.Path = "LogFinalMessageToHistory_HistoryDescription";
      activitybind2.Name = "IPRClosing";
      activitybind2.Path = "LogFinalMessageToHistory_HistoryOutcome";
      this.LogFinalMessageToHistory.Name = "LogFinalMessageToHistory";
      this.LogFinalMessageToHistory.OtherData = "";
      activitybind3.Name = "IPRClosing";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.LogFinalMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.LogFinalMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      this.LogFinalMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // Closeing
      // 
      this.Closeing.Description = "Closing IPR record procedure";
      this.Closeing.Name = "Closeing";
      this.Closeing.ExecuteCode += new System.EventHandler( this.Closeing_ExecuteCode );
      // 
      // LogClosingMessageToHistory
      // 
      this.LogClosingMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogClosingMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.LogClosingMessageToHistory.HistoryDescription = "The record is ready to be closed. Closing procedure started.";
      this.LogClosingMessageToHistory.HistoryOutcome = "Close starting";
      this.LogClosingMessageToHistory.Name = "LogClosingMessageToHistory";
      this.LogClosingMessageToHistory.OtherData = "";
      activitybind4.Name = "IPRClosing";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.LogClosingMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      // 
      // LogWarningMessageToHistory
      // 
      this.LogWarningMessageToHistory.Description = "Log Warning Message To History";
      this.LogWarningMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogWarningMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind5.Name = "IPRClosing";
      activitybind5.Path = "LogWarningMessageToHistory_HistoryDescription";
      activitybind6.Name = "IPRClosing";
      activitybind6.Path = "LogWarningMessageToHistory_HistoryOutcome";
      this.LogWarningMessageToHistory.Name = "LogWarningMessageToHistory";
      this.LogWarningMessageToHistory.OtherData = "";
      activitybind7.Name = "IPRClosing";
      activitybind7.Path = "workflowProperties.OriginatorUser.ID";
      this.LogWarningMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind7 ) ) );
      this.LogWarningMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      this.LogWarningMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ) );
      // 
      // ClosingSequence
      // 
      this.ClosingSequence.Activities.Add( this.LogClosingMessageToHistory );
      this.ClosingSequence.Activities.Add( this.Closeing );
      this.ClosingSequence.Activities.Add( this.LogFinalMessageToHistory );
      this.ClosingSequence.Description = "Closing Sequence.";
      this.ClosingSequence.Name = "ClosingSequence";
      // 
      // IfNotValid
      // 
      this.IfNotValid.Activities.Add( this.LogWarningMessageToHistory );
      this.IfNotValid.Description = "Proceed if not valid.";
      this.IfNotValid.Name = "IfNotValid";
      // 
      // IfValid
      // 
      this.IfValid.Activities.Add( this.ClosingSequence );
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>( this.ProcessIfValid );
      this.IfValid.Condition = codecondition1;
      this.IfValid.Description = "Valid To Be Closed";
      this.IfValid.Name = "IfValid";
      // 
      // IfRecordValid
      // 
      this.IfRecordValid.Activities.Add( this.IfValid );
      this.IfRecordValid.Activities.Add( this.IfNotValid );
      this.IfRecordValid.Description = "Check if the record is ready to be closed.";
      this.IfRecordValid.Name = "IfRecordValid";
      // 
      // RecordValidation
      // 
      this.RecordValidation.Description = "To validate the IPR record";
      this.RecordValidation.Name = "RecordValidation";
      this.RecordValidation.ExecuteCode += new System.EventHandler( this.RecordValidation_ExecuteCode );
      // 
      // LogStartingMessageToHistory
      // 
      this.LogStartingMessageToHistory.Description = "Log Starting Message To History";
      this.LogStartingMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogStartingMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.LogStartingMessageToHistory.HistoryDescription = "Starting validation of the IPR record.";
      this.LogStartingMessageToHistory.HistoryOutcome = "Starting";
      this.LogStartingMessageToHistory.Name = "LogStartingMessageToHistory";
      this.LogStartingMessageToHistory.OtherData = "";
      activitybind8.Name = "IPRClosing";
      activitybind8.Path = "workflowProperties.OriginatorUser.ID";
      this.LogStartingMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind8 ) ) );
      activitybind10.Name = "IPRClosing";
      activitybind10.Path = "workflowId";
      // 
      // onWorkflowActivated1
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "IPRClosing";
      this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated1.Name = "onWorkflowActivated1";
      activitybind9.Name = "IPRClosing";
      activitybind9.Path = "workflowProperties";
      this.onWorkflowActivated1.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind10 ) ) );
      this.onWorkflowActivated1.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind9 ) ) );
      // 
      // IPRClosing
      // 
      this.Activities.Add( this.onWorkflowActivated1 );
      this.Activities.Add( this.LogStartingMessageToHistory );
      this.Activities.Add( this.RecordValidation );
      this.Activities.Add( this.IfRecordValid );
      this.Name = "IPRClosing";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated OnWorkflowActivated;
    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogWarningMessageToHistory;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogFinalMessageToHistory;
    private CodeActivity Closeing;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogClosingMessageToHistory;
    private SequenceActivity ClosingSequence;
    private IfElseBranchActivity IfNotValid;
    private IfElseBranchActivity IfValid;
    private IfElseActivity IfRecordValid;
    private CodeActivity RecordValidation;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogStartingMessageToHistory;

  }
}
