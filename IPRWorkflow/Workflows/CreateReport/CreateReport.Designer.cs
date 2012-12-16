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

namespace CAS.SmartFactory.IPR.Workflows.CreateReport
{
  public sealed partial class CreateReport
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
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      this.CreateReportRecordsActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CalculatioFailedLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ReportCreatedLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateReportActivity = new System.Workflow.Activities.CodeActivity();
      this.StartinReportCreationLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateListsItems = new System.Workflow.Activities.SequenceActivity();
      this.CalculationFailedBypassIfElseBranch = new System.Workflow.Activities.IfElseBranchActivity();
      this.CreationReportIfElseBranch = new System.Workflow.Activities.IfElseBranchActivity();
      this.CreatedIfElseActivity = new System.Workflow.Activities.IfElseActivity();
      this.CreateReportsSequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.CalculateReportsActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingCalculateReportsActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ValidationFailedLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.MainReportCreationSequence = new System.Workflow.Activities.SequenceActivity();
      this.ValiationFailedBypassIfElseBranch = new System.Workflow.Activities.IfElseBranchActivity();
      this.ValidatedBranch = new System.Workflow.Activities.IfElseBranchActivity();
      this.ValidateActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingValidationLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.IfValidatedActivity = new System.Workflow.Activities.IfElseActivity();
      this.ValidationSequence = new System.Workflow.Activities.SequenceActivity();
      this.CreateReportActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // CreateReportRecordsActivity
      // 
      this.CreateReportRecordsActivity.Name = "CreateReportRecordsActivity";
      this.CreateReportRecordsActivity.ExecuteCode += new System.EventHandler( this.CreateRecorts );
      // 
      // StartingLog
      // 
      this.StartingLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartingLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.StartingLog.HistoryDescription = "Starting reports creation process.";
      this.StartingLog.HistoryOutcome = "Starting";
      this.StartingLog.Name = "StartingLog";
      this.StartingLog.OtherData = "";
      activitybind1.Name = "CreateReport";
      activitybind1.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      // 
      // CalculatioFailedLog
      // 
      this.CalculatioFailedLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.CalculatioFailedLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      this.CalculatioFailedLog.HistoryDescription = "";
      this.CalculatioFailedLog.HistoryOutcome = "Calculatio Failed";
      this.CalculatioFailedLog.Name = "CalculatioFailedLog";
      this.CalculatioFailedLog.OtherData = "";
      this.CalculatioFailedLog.UserId = -1;
      // 
      // ReportCreatedLog
      // 
      this.ReportCreatedLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.ReportCreatedLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      this.ReportCreatedLog.HistoryDescription = "";
      this.ReportCreatedLog.HistoryOutcome = "Completed successfully";
      this.ReportCreatedLog.Name = "ReportCreatedLog";
      this.ReportCreatedLog.OtherData = "";
      activitybind2.Name = "CreateReport";
      activitybind2.Path = "workflowProperties.OriginatorUser.ID";
      this.ReportCreatedLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      // 
      // CreateReportActivity
      // 
      this.CreateReportActivity.Description = "Create Report Activity";
      this.CreateReportActivity.Name = "CreateReportActivity";
      this.CreateReportActivity.ExecuteCode += new System.EventHandler( this.GenerateReport );
      // 
      // StartinReportCreationLog
      // 
      this.StartinReportCreationLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartinReportCreationLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.StartinReportCreationLog.HistoryDescription = "";
      this.StartinReportCreationLog.HistoryOutcome = "Creating";
      this.StartinReportCreationLog.Name = "StartinReportCreationLog";
      this.StartinReportCreationLog.OtherData = "";
      activitybind3.Name = "CreateReport";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.StartinReportCreationLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // CreateListsItems
      // 
      this.CreateListsItems.Activities.Add( this.StartingLog );
      this.CreateListsItems.Activities.Add( this.CreateReportRecordsActivity );
      this.CreateListsItems.Name = "CreateListsItems";
      // 
      // CalculationFailedBypassIfElseBranch
      // 
      this.CalculationFailedBypassIfElseBranch.Activities.Add( this.CalculatioFailedLog );
      this.CalculationFailedBypassIfElseBranch.Name = "CalculationFailedBypassIfElseBranch";
      // 
      // CreationReportIfElseBranch
      // 
      this.CreationReportIfElseBranch.Activities.Add( this.CreateListsItems );
      this.CreationReportIfElseBranch.Activities.Add( this.StartinReportCreationLog );
      this.CreationReportIfElseBranch.Activities.Add( this.CreateReportActivity );
      this.CreationReportIfElseBranch.Activities.Add( this.ReportCreatedLog );
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>( this.Calculated );
      this.CreationReportIfElseBranch.Condition = codecondition1;
      this.CreationReportIfElseBranch.Name = "CreationReportIfElseBranch";
      // 
      // CreatedIfElseActivity
      // 
      this.CreatedIfElseActivity.Activities.Add( this.CreationReportIfElseBranch );
      this.CreatedIfElseActivity.Activities.Add( this.CalculationFailedBypassIfElseBranch );
      this.CreatedIfElseActivity.Name = "CreatedIfElseActivity";
      // 
      // CreateReportsSequenceActivity
      // 
      this.CreateReportsSequenceActivity.Activities.Add( this.CreatedIfElseActivity );
      this.CreateReportsSequenceActivity.Description = "Create Reports Sequence Activity";
      this.CreateReportsSequenceActivity.Name = "CreateReportsSequenceActivity";
      // 
      // CalculateReportsActivity
      // 
      this.CalculateReportsActivity.Name = "CalculateReportsActivity";
      this.CalculateReportsActivity.ExecuteCode += new System.EventHandler( this.CalculateReports );
      // 
      // StartingCalculateReportsActivity
      // 
      this.StartingCalculateReportsActivity.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartingCalculateReportsActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.StartingCalculateReportsActivity.HistoryDescription = "";
      this.StartingCalculateReportsActivity.HistoryOutcome = "Calculating";
      this.StartingCalculateReportsActivity.Name = "StartingCalculateReportsActivity";
      this.StartingCalculateReportsActivity.OtherData = "";
      activitybind4.Name = "CreateReport";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingCalculateReportsActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      // 
      // ValidationFailedLog
      // 
      this.ValidationFailedLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.ValidationFailedLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind5.Name = "CreateReport";
      activitybind5.Path = "ValidationFailedLog_HistoryDescription";
      this.ValidationFailedLog.HistoryOutcome = "Validation Failed";
      this.ValidationFailedLog.Name = "ValidationFailedLog";
      this.ValidationFailedLog.OtherData = "";
      activitybind6.Name = "CreateReport";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.ValidationFailedLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind6 ) ) );
      this.ValidationFailedLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      // 
      // MainReportCreationSequence
      // 
      this.MainReportCreationSequence.Activities.Add( this.StartingCalculateReportsActivity );
      this.MainReportCreationSequence.Activities.Add( this.CalculateReportsActivity );
      this.MainReportCreationSequence.Activities.Add( this.CreateReportsSequenceActivity );
      this.MainReportCreationSequence.Name = "MainReportCreationSequence";
      // 
      // ValiationFailedBypassIfElseBranch
      // 
      this.ValiationFailedBypassIfElseBranch.Activities.Add( this.ValidationFailedLog );
      this.ValiationFailedBypassIfElseBranch.Name = "ValiationFailedBypassIfElseBranch";
      // 
      // ValidatedBranch
      // 
      this.ValidatedBranch.Activities.Add( this.MainReportCreationSequence );
      codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>( this.Validated );
      this.ValidatedBranch.Condition = codecondition2;
      this.ValidatedBranch.Description = "If Validated Branch";
      this.ValidatedBranch.Name = "ValidatedBranch";
      // 
      // ValidateActivity
      // 
      this.ValidateActivity.Name = "ValidateActivity";
      this.ValidateActivity.ExecuteCode += new System.EventHandler( this.Validate );
      // 
      // StartingValidationLog
      // 
      this.StartingValidationLog.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.StartingValidationLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.StartingValidationLog.HistoryDescription = "Starting data validation procedure.";
      this.StartingValidationLog.HistoryOutcome = "Validating";
      this.StartingValidationLog.Name = "StartingValidationLog";
      this.StartingValidationLog.OtherData = "";
      activitybind7.Name = "CreateReport";
      activitybind7.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingValidationLog.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind7 ) ) );
      // 
      // IfValidatedActivity
      // 
      this.IfValidatedActivity.Activities.Add( this.ValidatedBranch );
      this.IfValidatedActivity.Activities.Add( this.ValiationFailedBypassIfElseBranch );
      this.IfValidatedActivity.Name = "IfValidatedActivity";
      // 
      // ValidationSequence
      // 
      this.ValidationSequence.Activities.Add( this.StartingValidationLog );
      this.ValidationSequence.Activities.Add( this.ValidateActivity );
      this.ValidationSequence.Name = "ValidationSequence";
      activitybind9.Name = "CreateReport";
      activitybind9.Path = "workflowId";
      // 
      // CreateReportActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CreateReport";
      this.CreateReportActivated.CorrelationToken = correlationtoken1;
      this.CreateReportActivated.Description = "Create requiring reports";
      this.CreateReportActivated.EventName = "OnWorkflowActivated";
      this.CreateReportActivated.Name = "CreateReportActivated";
      activitybind8.Name = "CreateReport";
      activitybind8.Path = "workflowProperties";
      this.CreateReportActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind9 ) ) );
      this.CreateReportActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind8 ) ) );
      // 
      // CreateReport
      // 
      this.Activities.Add( this.CreateReportActivated );
      this.Activities.Add( this.ValidationSequence );
      this.Activities.Add( this.IfValidatedActivity );
      this.Name = "CreateReport";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity CalculatioFailedLog;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ReportCreatedLog;

    private CodeActivity CreateReportActivity;

    private IfElseBranchActivity CalculationFailedBypassIfElseBranch;

    private IfElseBranchActivity CreationReportIfElseBranch;

    private IfElseActivity CreatedIfElseActivity;

    private SequenceActivity CreateReportsSequenceActivity;

    private SequenceActivity MainReportCreationSequence;

    private IfElseBranchActivity ValiationFailedBypassIfElseBranch;

    private IfElseBranchActivity ValidatedBranch;

    private CodeActivity ValidateActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingValidationLog;

    private IfElseActivity IfValidatedActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ValidationFailedLog;

    private SequenceActivity ValidationSequence;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartinReportCreationLog;

    private CodeActivity CalculateReportsActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingCalculateReportsActivity;

    private CodeActivity CreateReportRecordsActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingLog;

    private SequenceActivity CreateListsItems;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated CreateReportActivated;


































  }
}
