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
      System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      this.ExceptionHandleristoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.faultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.FinishedHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CloseManyAccountCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.StartingHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.WorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // ExceptionHandleristoryListActivity
      // 
      this.ExceptionHandleristoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ExceptionHandleristoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "CloseManyAccounts";
      activitybind1.Path = "ExceptionHandleristoryListActivity_HistoryDescription1";
      activitybind2.Name = "CloseManyAccounts";
      activitybind2.Path = "ExceptionHandleristoryListActivity_HistoryOutcome";
      this.ExceptionHandleristoryListActivity.Name = "ExceptionHandleristoryListActivity";
      this.ExceptionHandleristoryListActivity.OtherData = "";
      activitybind3.Name = "CloseManyAccounts";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.ExceptionHandleristoryListActivity.MethodInvoking += new System.EventHandler(this.onExceptionHandleristoryListActivity);
      this.ExceptionHandleristoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.ExceptionHandleristoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.ExceptionHandleristoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // faultHandlerActivity
      // 
      this.faultHandlerActivity.Activities.Add(this.ExceptionHandleristoryListActivity);
      this.faultHandlerActivity.Description = "Fault handler";
      this.faultHandlerActivity.FaultType = typeof(System.Exception);
      this.faultHandlerActivity.Name = "faultHandlerActivity";
      // 
      // faultHandlersActivity1
      // 
      this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity);
      this.faultHandlersActivity1.Name = "faultHandlersActivity1";
      // 
      // FinishedHistoryListActivity
      // 
      this.FinishedHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.FinishedHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      activitybind4.Name = "CloseManyAccounts";
      activitybind4.Path = "FinishedHistoryListActivity_HistoryDescription";
      activitybind5.Name = "CloseManyAccounts";
      activitybind5.Path = "FinishedHistoryListActivity_HistoryOutcome";
      this.FinishedHistoryListActivity.Name = "FinishedHistoryListActivity";
      this.FinishedHistoryListActivity.OtherData = "";
      activitybind6.Name = "CloseManyAccounts";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.FinishedHistoryListActivity.MethodInvoking += new System.EventHandler(this.onFinishedHistoryListActivity);
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.FinishedHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // CloseManyAccountCodeActivity
      // 
      this.CloseManyAccountCodeActivity.Description = "Workflow code.";
      this.CloseManyAccountCodeActivity.Name = "CloseManyAccountCodeActivity";
      this.CloseManyAccountCodeActivity.ExecuteCode += new System.EventHandler(this.DoCloseManyAccount);
      // 
      // StartingHistoryListActivity
      // 
      this.StartingHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartingHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      activitybind7.Name = "CloseManyAccounts";
      activitybind7.Path = "StartingHistoryListActivity_HistoryDescription";
      activitybind8.Name = "CloseManyAccounts";
      activitybind8.Path = "StartingHistoryListActivity_HistoryOutcome";
      this.StartingHistoryListActivity.Name = "StartingHistoryListActivity";
      this.StartingHistoryListActivity.OtherData = "";
      activitybind9.Name = "CloseManyAccounts";
      activitybind9.Path = "workflowProperties.OriginatorUser.ID";
      this.StartingHistoryListActivity.MethodInvoking += new System.EventHandler(this.OnStartingLogToHistory);
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      this.StartingHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      activitybind11.Name = "CloseManyAccounts";
      activitybind11.Path = "workflowId";
      // 
      // WorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CloseManyAccounts";
      this.WorkflowActivated.CorrelationToken = correlationtoken1;
      this.WorkflowActivated.Description = "Initialize the workflow";
      this.WorkflowActivated.EventName = "OnWorkflowActivated";
      this.WorkflowActivated.Name = "WorkflowActivated";
      activitybind10.Name = "CloseManyAccounts";
      activitybind10.Path = "workflowProperties";
      this.WorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.GetParameters);
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
      this.WorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      // 
      // CloseManyAccounts
      // 
      this.Activities.Add(this.WorkflowActivated);
      this.Activities.Add(this.StartingHistoryListActivity);
      this.Activities.Add(this.CloseManyAccountCodeActivity);
      this.Activities.Add(this.FinishedHistoryListActivity);
      this.Activities.Add(this.faultHandlersActivity1);
      this.Name = "CloseManyAccounts";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ExceptionHandleristoryListActivity;

    private FaultHandlerActivity faultHandlerActivity;

    private FaultHandlersActivity faultHandlersActivity1;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FinishedHistoryListActivity;

    private CodeActivity CloseManyAccountCodeActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartingHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated WorkflowActivated;

















  }
}
