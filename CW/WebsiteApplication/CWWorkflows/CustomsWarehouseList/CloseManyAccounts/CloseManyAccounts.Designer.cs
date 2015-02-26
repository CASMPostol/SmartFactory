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

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  public sealed partial class CloseManyAccounts
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
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
      this.ExceptionHandlerHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ApplicationErrorLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CloseManyAccountCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.WhileLogToHistory = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.GeneralFaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.ApplicationErrorfaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.WhileSequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.FinishedHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
      this.StartingHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.WorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // ExceptionHandlerHistoryListActivity
      // 
      this.ExceptionHandlerHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ExceptionHandlerHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "CloseManyAccounts";
      activitybind1.Path = "ExceptionHandlerHistoryListActivity_HistoryDescription";
      activitybind2.Name = "CloseManyAccounts";
      activitybind2.Path = "ExceptionHandlerHistoryListActivity_HistoryOutcome";
      this.ExceptionHandlerHistoryListActivity.Name = "ExceptionHandlerHistoryListActivity";
      this.ExceptionHandlerHistoryListActivity.OtherData = "";
      activitybind3.Name = "CloseManyAccounts";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.ExceptionHandlerHistoryListActivity.MethodInvoking += new System.EventHandler(this.onExceptionHandlerHistoryListActivity);
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.ExceptionHandlerHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // ApplicationErrorLogToHistory
      // 
      this.ApplicationErrorLogToHistory.Description = "ApplicationError Log To History";
      this.ApplicationErrorLogToHistory.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ApplicationErrorLogToHistory.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind4.Name = "CloseManyAccounts";
      activitybind4.Path = "ApplicationErrorLogToHistory_HistoryDescription1";
      activitybind5.Name = "CloseManyAccounts";
      activitybind5.Path = "ApplicationErrorLogToHistory_HistoryOutcome";
      this.ApplicationErrorLogToHistory.Name = "ApplicationErrorLogToHistory";
      this.ApplicationErrorLogToHistory.OtherData = "";
      activitybind6.Name = "CloseManyAccounts";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.ApplicationErrorLogToHistory.MethodInvoking += new System.EventHandler(this.onApplicationErrorLogToHistory);
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.ApplicationErrorLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
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
      activitybind7.Name = "CloseManyAccounts";
      activitybind7.Path = "WhileLogToHistory_HistoryDescription";
      activitybind8.Name = "CloseManyAccounts";
      activitybind8.Path = "WhileLogToHistory_HistoryOutcome";
      this.WhileLogToHistory.Name = "WhileLogToHistory";
      this.WhileLogToHistory.OtherData = "";
      activitybind9.Name = "CloseManyAccounts";
      activitybind9.Path = "workflowProperties.OriginatorUser.ID";
      this.WhileLogToHistory.MethodInvoking += new System.EventHandler(this.onWhileLogToHistory);
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      this.WhileLogToHistory.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      // 
      // GeneralFaultHandlerActivity
      // 
      this.GeneralFaultHandlerActivity.Activities.Add(this.ExceptionHandlerHistoryListActivity);
      this.GeneralFaultHandlerActivity.Description = "Fault handler";
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
      this.FinishedHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind10.Name = "CloseManyAccounts";
      activitybind10.Path = "FinishedHistoryListActivity_HistoryDescription";
      activitybind11.Name = "CloseManyAccounts";
      activitybind11.Path = "FinishedHistoryListActivity_HistoryOutcome";
      this.FinishedHistoryListActivity.Name = "FinishedHistoryListActivity";
      this.FinishedHistoryListActivity.OtherData = "";
      activitybind12.Name = "CloseManyAccounts";
      activitybind12.Path = "workflowProperties.OriginatorUser.ID";
      this.FinishedHistoryListActivity.MethodInvoking += new System.EventHandler(this.onFinishedHistoryListActivity);
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
      // 
      // whileActivity1
      // 
      this.whileActivity1.Activities.Add(this.WhileSequenceActivity);
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.WhileActivityCondition);
      this.whileActivity1.Condition = codecondition1;
      this.whileActivity1.Name = "whileActivity1";
      // 
      // StartingHistoryListActivity
      // 
      this.StartingHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartingHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind13.Name = "CloseManyAccounts";
      activitybind13.Path = "StartingHistoryListActivity_HistoryDescription";
      activitybind14.Name = "CloseManyAccounts";
      activitybind14.Path = "StartingHistoryListActivity_HistoryOutcome";
      this.StartingHistoryListActivity.Name = "StartingHistoryListActivity";
      this.StartingHistoryListActivity.OtherData = "";
      activitybind15.Name = "CloseManyAccounts";
      activitybind15.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingHistoryListActivity.MethodInvoking += new System.EventHandler(this.OnStartingLogToHistory);
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
      activitybind17.Name = "CloseManyAccounts";
      activitybind17.Path = "workflowId";
      // 
      // WorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CloseManyAccounts";
      this.WorkflowActivated.CorrelationToken = correlationtoken1;
      this.WorkflowActivated.Description = "Initialize the workflow";
      this.WorkflowActivated.EventName = "OnWorkflowActivated";
      this.WorkflowActivated.Name = "WorkflowActivated";
      activitybind16.Name = "CloseManyAccounts";
      activitybind16.Path = "workflowProperties";
      this.WorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.GetParameters);
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
      // 
      // CloseManyAccounts
      // 
      this.Activities.Add(this.WorkflowActivated);
      this.Activities.Add(this.StartingHistoryListActivity);
      this.Activities.Add(this.whileActivity1);
      this.Activities.Add(this.FinishedHistoryListActivity);
      this.Activities.Add(this.faultHandlersActivity1);
      this.Description = "Close Many Accounts";
      this.Name = "CloseManyAccounts";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ApplicationErrorLogToHistory;
    private FaultHandlerActivity ApplicationErrorfaultHandlerActivity;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity WhileLogToHistory;
    private SequenceActivity WhileSequenceActivity;
    private WhileActivity whileActivity1;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ExceptionHandlerHistoryListActivity;
    private FaultHandlerActivity GeneralFaultHandlerActivity;
    private FaultHandlersActivity faultHandlersActivity1;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FinishedHistoryListActivity;
    private CodeActivity CloseManyAccountCodeActivity;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingHistoryListActivity;
    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated WorkflowActivated;


  }
}
