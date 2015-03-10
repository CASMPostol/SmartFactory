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

namespace CAS.SmartFactory.IPR.Workflows.CloseManyIPRAccounts
{
  public sealed partial class CloseManyIPRAccounts
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
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
      this.ExceptionHandlerHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ApplicationErrorLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.lLgClosingResult = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CloseManyAccountCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.WhileLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.GeneralFaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.ApplicationErrorfaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.WhileSequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.FinishedHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.WhileActivity = new System.Workflow.Activities.WhileActivity();
      this.StartingHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.WorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // ExceptionHandlerHistoryListActivity
      // 
      this.ExceptionHandlerHistoryListActivity.Description = "Log to the history information about teh exception.";
      this.ExceptionHandlerHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ExceptionHandlerHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
      activitybind1.Name = "CloseManyIPRAccounts";
      activitybind1.Path = "ExceptionHandlerHistoryListActivity_HistoryDescription";
      activitybind2.Name = "CloseManyIPRAccounts";
      activitybind2.Path = "ExceptionHandlerHistoryListActivity_HistoryOutcome";
      this.ExceptionHandlerHistoryListActivity.Name = "ExceptionHandlerHistoryListActivity";
      this.ExceptionHandlerHistoryListActivity.OtherData = "";
      activitybind3.Name = "CloseManyIPRAccounts";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.ExceptionHandlerHistoryListActivity.MethodInvoking += new System.EventHandler(this.onExceptionHandlerHistoryListActivity);
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // ApplicationErrorLogToHistory
      // 
      this.ApplicationErrorLogToHistory.Description = "ApplicationErrorLogToHistory";
      this.ApplicationErrorLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ApplicationErrorLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowError;
      activitybind4.Name = "CloseManyIPRAccounts";
      activitybind4.Path = "ApplicationErrorLogToHistory_HistoryDescription1";
      activitybind5.Name = "CloseManyIPRAccounts";
      activitybind5.Path = "ApplicationErrorLogToHistory_HistoryOutcome";
      this.ApplicationErrorLogToHistory.Name = "ApplicationErrorLogToHistory";
      this.ApplicationErrorLogToHistory.OtherData = "";
      activitybind6.Name = "CloseManyIPRAccounts";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.ApplicationErrorLogToHistory.MethodInvoking += new System.EventHandler(this.onApplicationErrorLogToHistory);
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      // 
      // lLgClosingResult
      // 
      this.lLgClosingResult.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.lLgClosingResult.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind7.Name = "CloseManyIPRAccounts";
      activitybind7.Path = "lLgClosingResult_HistoryDescription";
      activitybind8.Name = "CloseManyIPRAccounts";
      activitybind8.Path = "lLgClosingResult_HistoryOutcome";
      this.lLgClosingResult.Name = "lLgClosingResult";
      this.lLgClosingResult.OtherData = "";
      activitybind9.Name = "CloseManyIPRAccounts";
      activitybind9.Path = "workflowProperties.OriginatorUser.ID";
      this.lLgClosingResult.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      this.lLgClosingResult.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.lLgClosingResult.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      // 
      // CloseManyAccountCodeActivity
      // 
      this.CloseManyAccountCodeActivity.Description = "Workflow code.";
      this.CloseManyAccountCodeActivity.Name = "CloseManyAccountCodeActivity";
      this.CloseManyAccountCodeActivity.ExecuteCode += new System.EventHandler(this.DoCloseManyAccount);
      // 
      // WhileLogToHistory
      // 
      this.WhileLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.WhileLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind10.Name = "CloseManyIPRAccounts";
      activitybind10.Path = "StartingHistoryListActivity_HistoryDescription";
      activitybind11.Name = "CloseManyIPRAccounts";
      activitybind11.Path = "WhileLogToHistory_HistoryOutcome";
      this.WhileLogToHistory.Name = "WhileLogToHistory";
      this.WhileLogToHistory.OtherData = "";
      activitybind12.Name = "CloseManyIPRAccounts";
      activitybind12.Path = "workflowProperties.OriginatorUser.ID";
      this.WhileLogToHistory.MethodInvoking += new System.EventHandler(this.onWhileLogToHistory);
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
      // 
      // GeneralFaultHandlerActivity
      // 
      this.GeneralFaultHandlerActivity.Activities.Add(this.ExceptionHandlerHistoryListActivity);
      this.GeneralFaultHandlerActivity.Description = "General fault handler";
      this.GeneralFaultHandlerActivity.FaultType = typeof(System.Exception);
      this.GeneralFaultHandlerActivity.Name = "GeneralFaultHandlerActivity";
      // 
      // ApplicationErrorfaultHandlerActivity
      // 
      this.ApplicationErrorfaultHandlerActivity.Activities.Add(this.ApplicationErrorLogToHistory);
      this.ApplicationErrorfaultHandlerActivity.Description = "Application Error Handler";
      this.ApplicationErrorfaultHandlerActivity.FaultType = typeof(CAS.SharePoint.ApplicationError);
      this.ApplicationErrorfaultHandlerActivity.Name = "ApplicationErrorfaultHandlerActivity";
      // 
      // WhileSequenceActivity
      // 
      this.WhileSequenceActivity.Activities.Add(this.WhileLogToHistory);
      this.WhileSequenceActivity.Activities.Add(this.CloseManyAccountCodeActivity);
      this.WhileSequenceActivity.Activities.Add(this.lLgClosingResult);
      this.WhileSequenceActivity.Description = "Content of the while loop";
      this.WhileSequenceActivity.Name = "WhileSequenceActivity";
      // 
      // faultHandlersActivity1
      // 
      this.faultHandlersActivity1.Activities.Add(this.ApplicationErrorfaultHandlerActivity);
      this.faultHandlersActivity1.Activities.Add(this.GeneralFaultHandlerActivity);
      this.faultHandlersActivity1.Name = "faultHandlersActivity1";
      // 
      // FinishedHistoryListActivity
      // 
      this.FinishedHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.FinishedHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind13.Name = "CloseManyIPRAccounts";
      activitybind13.Path = "FinishedHistoryListActivity_HistoryDescription";
      activitybind14.Name = "CloseManyIPRAccounts";
      activitybind14.Path = "FinishedHistoryListActivity_HistoryOutcome";
      this.FinishedHistoryListActivity.Name = "FinishedHistoryListActivity";
      this.FinishedHistoryListActivity.OtherData = "";
      activitybind15.Name = "CloseManyIPRAccounts";
      activitybind15.Path = "workflowProperties.OriginatorUser.ID";
      this.FinishedHistoryListActivity.MethodInvoking += new System.EventHandler(this.onFinishedHistoryListActivity);
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
      // 
      // WhileActivity
      // 
      this.WhileActivity.Activities.Add(this.WhileSequenceActivity);
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.WhileActivityCondition);
      this.WhileActivity.Condition = codecondition1;
      this.WhileActivity.Description = "Iretase closing operation for all selected accounts.";
      this.WhileActivity.Name = "WhileActivity";
      // 
      // StartingHistoryListActivity
      // 
      this.StartingHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartingHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind16.Name = "CloseManyIPRAccounts";
      activitybind16.Path = "StartingHistoryListActivity_HistoryDescription";
      activitybind17.Name = "CloseManyIPRAccounts";
      activitybind17.Path = "StartingHistoryListActivity_HistoryOutcome";
      this.StartingHistoryListActivity.Name = "StartingHistoryListActivity";
      this.StartingHistoryListActivity.OtherData = "";
      activitybind18.Name = "CloseManyIPRAccounts";
      activitybind18.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingHistoryListActivity.MethodInvoking += new System.EventHandler(this.OnStartingLogToHistory);
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
      activitybind20.Name = "CloseManyIPRAccounts";
      activitybind20.Path = "workflowId";
      // 
      // WorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CloseManyIPRAccounts";
      this.WorkflowActivated.CorrelationToken = correlationtoken1;
      this.WorkflowActivated.Description = "Initialize the workflow";
      this.WorkflowActivated.EventName = "OnWorkflowActivated";
      this.WorkflowActivated.Name = "WorkflowActivated";
      activitybind19.Name = "CloseManyIPRAccounts";
      activitybind19.Path = "workflowProperties";
      this.WorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.GetParameters);
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
      // 
      // CloseManyIPRAccounts
      // 
      this.Activities.Add(this.WorkflowActivated);
      this.Activities.Add(this.StartingHistoryListActivity);
      this.Activities.Add(this.WhileActivity);
      this.Activities.Add(this.FinishedHistoryListActivity);
      this.Activities.Add(this.faultHandlersActivity1);
      this.Name = "CloseManyIPRAccounts";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FinishedHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ExceptionHandlerHistoryListActivity;

    private FaultHandlerActivity GeneralFaultHandlerActivity;

    private FaultHandlerActivity ApplicationErrorfaultHandlerActivity;

    private FaultHandlersActivity faultHandlersActivity1;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ApplicationErrorLogToHistory;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity WhileLogToHistory;

    private SequenceActivity WhileSequenceActivity;

    private WhileActivity WhileActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity lLgClosingResult;

    private CodeActivity CloseManyAccountCodeActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated WorkflowActivated;






















  }
}
