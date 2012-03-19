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

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine
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
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_TimeOutDelayActivity = new System.Workflow.Activities.DelayActivity();
      this.m_WhileRoundLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_OnWorkflowItemChanged = new Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged();
      this.m_WaitTimeOutEventDrivenActivity = new System.Workflow.Activities.EventDrivenActivity();
      this.m_ItemChangedEventDrivenActivity = new System.Workflow.Activities.EventDrivenActivity();
      this.m_WaiyForEventListenActivity = new System.Workflow.Activities.ListenActivity();
      this.m_SequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.m_Nothing2DoIfElseBranchActivity = new System.Workflow.Activities.IfElseBranchActivity();
      this.m_TimeOutIfElseBranchActivity = new System.Workflow.Activities.IfElseBranchActivity();
      this.m_IfElseActivity = new System.Workflow.Activities.IfElseActivity();
      this.m_MainBodySequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.m_OnWokflowCompletedLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_MainLoopWhileActivity = new System.Workflow.Activities.WhileActivity();
      this.m_OnStartedLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // logToHistoryListActivity1
      // 
      this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.logToHistoryListActivity1.HistoryDescription = "Ble ble";
      this.logToHistoryListActivity1.HistoryOutcome = "Time out";
      this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
      this.logToHistoryListActivity1.OtherData = "";
      this.logToHistoryListActivity1.UserId = -1;
      this.logToHistoryListActivity1.MethodInvoking += new System.EventHandler(this.logToHistoryListActivity1_MethodInvoking);
      // 
      // m_TimeOutDelayActivity
      // 
      this.m_TimeOutDelayActivity.Description = "Activate the timeout.";
      this.m_TimeOutDelayActivity.Name = "m_TimeOutDelayActivity";
      this.m_TimeOutDelayActivity.TimeoutDuration = System.TimeSpan.Parse("00:05:00");
      this.m_TimeOutDelayActivity.InitializeTimeoutDuration += new System.EventHandler(this.m_TimeOutDelayActivity_InitializeTimeoutDuration);
      // 
      // m_WhileRoundLogToHistoryListActivity
      // 
      this.m_WhileRoundLogToHistoryListActivity.Description = "Warning: time out for shipping encountered.";
      this.m_WhileRoundLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_WhileRoundLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "ShippingStateMachine";
      activitybind1.Path = "m_SendWarningLogToHistoryListActivity_HistoryDescription";
      activitybind2.Name = "ShippingStateMachine";
      activitybind2.Path = "m_SendWarningLogToHistoryListActivity_HistoryOutcome";
      this.m_WhileRoundLogToHistoryListActivity.Name = "m_WhileRoundLogToHistoryListActivity";
      this.m_WhileRoundLogToHistoryListActivity.OtherData = "";
      activitybind3.Name = "ShippingStateMachine";
      activitybind3.Path = "m_OnStartedLogToHistoryListActivity_UserId1";
      this.m_WhileRoundLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.m_WhileRoundLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.m_WhileRoundLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // m_OnWorkflowItemChanged
      // 
      activitybind4.Name = "ShippingStateMachine";
      activitybind4.Path = "m_AfterProperties";
      activitybind5.Name = "ShippingStateMachine";
      activitybind5.Path = "m_BeforeProperties";
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "ShippingStateMachine";
      this.m_OnWorkflowItemChanged.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowItemChanged.Description = "On workflow item changed";
      this.m_OnWorkflowItemChanged.Name = "m_OnWorkflowItemChanged";
      this.m_OnWorkflowItemChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowItemChanged_Invoked);
      this.m_OnWorkflowItemChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      this.m_OnWorkflowItemChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // m_WaitTimeOutEventDrivenActivity
      // 
      this.m_WaitTimeOutEventDrivenActivity.Activities.Add(this.m_TimeOutDelayActivity);
      this.m_WaitTimeOutEventDrivenActivity.Activities.Add(this.logToHistoryListActivity1);
      this.m_WaitTimeOutEventDrivenActivity.Description = "Wait until time out";
      this.m_WaitTimeOutEventDrivenActivity.Name = "m_WaitTimeOutEventDrivenActivity";
      // 
      // m_ItemChangedEventDrivenActivity
      // 
      this.m_ItemChangedEventDrivenActivity.Activities.Add(this.m_OnWorkflowItemChanged);
      this.m_ItemChangedEventDrivenActivity.Activities.Add(this.m_WhileRoundLogToHistoryListActivity);
      this.m_ItemChangedEventDrivenActivity.Description = "Wait until item chnged.";
      this.m_ItemChangedEventDrivenActivity.Name = "m_ItemChangedEventDrivenActivity";
      // 
      // m_WaiyForEventListenActivity
      // 
      this.m_WaiyForEventListenActivity.Activities.Add(this.m_ItemChangedEventDrivenActivity);
      this.m_WaiyForEventListenActivity.Activities.Add(this.m_WaitTimeOutEventDrivenActivity);
      this.m_WaiyForEventListenActivity.Description = "Waiting until item chnged or time out occured.";
      this.m_WaiyForEventListenActivity.Name = "m_WaiyForEventListenActivity";
      // 
      // m_SequenceActivity
      // 
      this.m_SequenceActivity.Activities.Add(this.m_WaiyForEventListenActivity);
      this.m_SequenceActivity.Name = "m_SequenceActivity";
      // 
      // m_Nothing2DoIfElseBranchActivity
      // 
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.OnIfEvaluationNotTimeOutRequired);
      this.m_Nothing2DoIfElseBranchActivity.Condition = codecondition1;
      this.m_Nothing2DoIfElseBranchActivity.Description = "If there is nothing to do.";
      this.m_Nothing2DoIfElseBranchActivity.Name = "m_Nothing2DoIfElseBranchActivity";
      // 
      // m_TimeOutIfElseBranchActivity
      // 
      this.m_TimeOutIfElseBranchActivity.Activities.Add(this.m_SequenceActivity);
      codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.OnIfEvaluationTimeOutRequired);
      this.m_TimeOutIfElseBranchActivity.Condition = codecondition2;
      this.m_TimeOutIfElseBranchActivity.Description = "If there is something to do.";
      this.m_TimeOutIfElseBranchActivity.Name = "m_TimeOutIfElseBranchActivity";
      // 
      // m_IfElseActivity
      // 
      this.m_IfElseActivity.Activities.Add(this.m_TimeOutIfElseBranchActivity);
      this.m_IfElseActivity.Activities.Add(this.m_Nothing2DoIfElseBranchActivity);
      this.m_IfElseActivity.Name = "m_IfElseActivity";
      // 
      // m_MainBodySequenceActivity
      // 
      this.m_MainBodySequenceActivity.Activities.Add(this.m_IfElseActivity);
      this.m_MainBodySequenceActivity.Description = "The body of the shipping state machine  loop";
      this.m_MainBodySequenceActivity.Name = "m_MainBodySequenceActivity";
      // 
      // m_OnWokflowCompletedLogToHistoryListActivity
      // 
      this.m_OnWokflowCompletedLogToHistoryListActivity.Description = "Log to the history that the workflow has been completed.";
      this.m_OnWokflowCompletedLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_OnWokflowCompletedLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowCompleted;
      this.m_OnWokflowCompletedLogToHistoryListActivity.HistoryDescription = "The workflow has been completed successfully.";
      this.m_OnWokflowCompletedLogToHistoryListActivity.HistoryOutcome = "Completed";
      this.m_OnWokflowCompletedLogToHistoryListActivity.Name = "m_OnWokflowCompletedLogToHistoryListActivity";
      this.m_OnWokflowCompletedLogToHistoryListActivity.OtherData = "";
      activitybind6.Name = "ShippingStateMachine";
      activitybind6.Path = "m_OnStartedLogToHistoryListActivity_UserId1";
      this.m_OnWokflowCompletedLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      // 
      // m_MainLoopWhileActivity
      // 
      this.m_MainLoopWhileActivity.Activities.Add(this.m_MainBodySequenceActivity);
      codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.m_MainLoopWhileActivity_ConditionEventHandler);
      this.m_MainLoopWhileActivity.Condition = codecondition3;
      this.m_MainLoopWhileActivity.Description = "Main loop of the shipping state machine.";
      this.m_MainLoopWhileActivity.Name = "m_MainLoopWhileActivity";
      // 
      // m_OnStartedLogToHistoryListActivity
      // 
      this.m_OnStartedLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_OnStartedLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.m_OnStartedLogToHistoryListActivity.HistoryDescription = "Monitoring of shipping started";
      this.m_OnStartedLogToHistoryListActivity.HistoryOutcome = "New shipping";
      this.m_OnStartedLogToHistoryListActivity.Name = "m_OnStartedLogToHistoryListActivity";
      this.m_OnStartedLogToHistoryListActivity.OtherData = "";
      activitybind7.Name = "ShippingStateMachine";
      activitybind7.Path = "m_OnStartedLogToHistoryListActivity_UserId1";
      this.m_OnStartedLogToHistoryListActivity.MethodInvoking += new System.EventHandler(this.m_OnStartedLogToHistoryListActivity_MethodInvoking);
      this.m_OnStartedLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      activitybind9.Name = "ShippingStateMachine";
      activitybind9.Path = "workflowId";
      // 
      // m_OnWorkflowActivated
      // 
      this.m_OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated.Description = "On workflow activated";
      this.m_OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated.Name = "m_OnWorkflowActivated";
      activitybind8.Name = "ShippingStateMachine";
      activitybind8.Path = "m_OnWorkflowActivated_WorkflowProperties";
      this.m_OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated_Invoked);
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      // 
      // ShippingStateMachine
      // 
      this.Activities.Add(this.m_OnWorkflowActivated);
      this.Activities.Add(this.m_OnStartedLogToHistoryListActivity);
      this.Activities.Add(this.m_MainLoopWhileActivity);
      this.Activities.Add(this.m_OnWokflowCompletedLogToHistoryListActivity);
      this.Name = "ShippingStateMachine";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_OnWokflowCompletedLogToHistoryListActivity;

    private DelayActivity m_TimeOutDelayActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_WhileRoundLogToHistoryListActivity;

    private EventDrivenActivity m_WaitTimeOutEventDrivenActivity;

    private EventDrivenActivity m_ItemChangedEventDrivenActivity;

    private ListenActivity m_WaiyForEventListenActivity;

    private SequenceActivity m_MainBodySequenceActivity;

    private WhileActivity m_MainLoopWhileActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_OnStartedLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged m_OnWorkflowItemChanged;

    private SequenceActivity m_SequenceActivity;

    private IfElseBranchActivity m_Nothing2DoIfElseBranchActivity;

    private IfElseBranchActivity m_TimeOutIfElseBranchActivity;

    private IfElseActivity m_IfElseActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated;















































  }
}
