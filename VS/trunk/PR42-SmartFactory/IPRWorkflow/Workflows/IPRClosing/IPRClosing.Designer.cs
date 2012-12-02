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
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
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
      this.OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // LogFinalMessageToHistory
      // 
      this.LogFinalMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogFinalMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.LogFinalMessageToHistory.HistoryDescription = "The IPR record has been successfully closed.";
      this.LogFinalMessageToHistory.HistoryOutcome = "Finished successfully";
      this.LogFinalMessageToHistory.Name = "LogFinalMessageToHistory";
      this.LogFinalMessageToHistory.OtherData = "";
      this.LogFinalMessageToHistory.UserId = -1;
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
      activitybind1.Name = "IPRClosing";
      activitybind1.Path = "workflowProperties.OriginatorUser.ID";
      this.LogClosingMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      // 
      // LogWarningMessageToHistory
      // 
      this.LogWarningMessageToHistory.Description = "Log Warning Message To History";
      this.LogWarningMessageToHistory.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.LogWarningMessageToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.LogWarningMessageToHistory.HistoryDescription = "Cannot close the IPR account, update the content and try again.";
      this.LogWarningMessageToHistory.HistoryOutcome = "Closing failed";
      this.LogWarningMessageToHistory.Name = "LogWarningMessageToHistory";
      this.LogWarningMessageToHistory.OtherData = "";
      activitybind2.Name = "IPRClosing";
      activitybind2.Path = "workflowProperties.OriginatorUser.ID";
      this.LogWarningMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
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
      activitybind3.Name = "IPRClosing";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.LogStartingMessageToHistory.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      activitybind5.Name = "IPRClosing";
      activitybind5.Path = "workflowId";
      // 
      // OnWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "IPRClosing";
      this.OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.OnWorkflowActivated.Name = "OnWorkflowActivated";
      activitybind4.Name = "IPRClosing";
      activitybind4.Path = "workflowProperties";
      this.OnWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      this.OnWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      // 
      // IPRClosing
      // 
      this.Activities.Add( this.OnWorkflowActivated );
      this.Activities.Add( this.LogStartingMessageToHistory );
      this.Activities.Add( this.RecordValidation );
      this.Activities.Add( this.IfRecordValid );
      this.Name = "IPRClosing";
      this.CanModifyActivities = false;

    }

    #endregion

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

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated OnWorkflowActivated;











  }
}
