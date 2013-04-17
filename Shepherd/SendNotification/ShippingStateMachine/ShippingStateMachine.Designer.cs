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
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind21 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind23 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind22 = new System.Workflow.ComponentModel.ActivityBind();
      this.m_TimeOutLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_TimeOutDelay = new System.Workflow.Activities.DelayActivity();
      this.m_OnWorkflowItemChangedLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_OnWorkflowItemChanged = new Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged();
      this.m_CarrierNotificationSendEmail = new Microsoft.SharePoint.WorkflowActions.SendEmail();
      this.m_CarrierNotificationSendEmailLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_WaitTimeOutEventDrivenActivity = new System.Workflow.Activities.EventDrivenActivity();
      this.m_ItemChangedEventDrivenActivity = new System.Workflow.Activities.EventDrivenActivity();
      this.SendingEmailsSequence = new System.Workflow.Activities.SequenceActivity();
      this.m_FaultHandlerLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_WaiyForEventListenActivity = new System.Workflow.Activities.ListenActivity();
      this.SendingEmailsReplicator = new System.Workflow.Activities.ReplicatorActivity();
      this.m_CalculateTimeoutLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_CalculateTimeoutCode = new System.Workflow.Activities.CodeActivity();
      this.m_MainFaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.m_SequenceActivity = new System.Workflow.Activities.SequenceActivity();
      this.m_FaultHandlersActivity = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.m_OnWokflowCompletedLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_MainLoopWhileActivity = new System.Workflow.Activities.WhileActivity();
      this.m_OnStartedLogToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.m_OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // m_TimeOutLogToHistoryListActivity
      // 
      this.m_TimeOutLogToHistoryListActivity.Description = "Warning: time out for shipping encountered.";
      this.m_TimeOutLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_TimeOutLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "ShippingStateMachine";
      activitybind1.Path = "m_TimeOutLogToHistoryListActivity_HistoryDescription";
      this.m_TimeOutLogToHistoryListActivity.HistoryOutcome = "Timeout";
      this.m_TimeOutLogToHistoryListActivity.Name = "m_TimeOutLogToHistoryListActivity";
      this.m_TimeOutLogToHistoryListActivity.OtherData = "";
      activitybind2.Name = "ShippingStateMachine";
      activitybind2.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_TimeOutLogToHistoryListActivity.MethodInvoking += new System.EventHandler(this.m_TimeOutLogToHistoryListActivity_MethodInvoking);
      this.m_TimeOutLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.m_TimeOutLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // m_TimeOutDelay
      // 
      this.m_TimeOutDelay.Description = "Timeout to the dedline.";
      this.m_TimeOutDelay.Name = "m_TimeOutDelay";
      activitybind3.Name = "ShippingStateMachine";
      activitybind3.Path = "m_TimeOutDelay_TimeoutDuration";
      this.m_TimeOutDelay.SetBinding(System.Workflow.Activities.DelayActivity.TimeoutDurationProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // m_OnWorkflowItemChangedLogToHistoryList
      // 
      this.m_OnWorkflowItemChangedLogToHistoryList.Description = "Warning: time out for shipping encountered.";
      this.m_OnWorkflowItemChangedLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_OnWorkflowItemChangedLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind4.Name = "ShippingStateMachine";
      activitybind4.Path = "m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription";
      this.m_OnWorkflowItemChangedLogToHistoryList.HistoryOutcome = "Shipping Changed";
      this.m_OnWorkflowItemChangedLogToHistoryList.Name = "m_OnWorkflowItemChangedLogToHistoryList";
      this.m_OnWorkflowItemChangedLogToHistoryList.OtherData = "";
      activitybind5.Name = "ShippingStateMachine";
      activitybind5.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_OnWorkflowItemChangedLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      this.m_OnWorkflowItemChangedLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      // 
      // m_OnWorkflowItemChanged
      // 
      activitybind6.Name = "ShippingStateMachine";
      activitybind6.Path = "m_OnWorkflowItemChanged_AfterProperties";
      activitybind7.Name = "ShippingStateMachine";
      activitybind7.Path = "m_OnWorkflowItemChanged_BeforeProperties";
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "ShippingStateMachine";
      this.m_OnWorkflowItemChanged.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowItemChanged.Description = "On workflow item changed";
      this.m_OnWorkflowItemChanged.Name = "m_OnWorkflowItemChanged";
      this.m_OnWorkflowItemChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowItemChanged_Invoked);
      this.m_OnWorkflowItemChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.m_OnWorkflowItemChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      // 
      // m_CarrierNotificationSendEmail
      // 
      this.m_CarrierNotificationSendEmail.BCC = "";
      activitybind8.Name = "ShippingStateMachine";
      activitybind8.Path = "m_CarrierNotificationSendEmail_Body";
      activitybind9.Name = "ShippingStateMachine";
      activitybind9.Path = "m_CarrierNotificationSendEmail_CC";
      this.m_CarrierNotificationSendEmail.CorrelationToken = correlationtoken1;
      this.m_CarrierNotificationSendEmail.Description = "Send warning by email";
      activitybind10.Name = "ShippingStateMachine";
      activitybind10.Path = "m_CarrierNotificationSendEmail_From";
      this.m_CarrierNotificationSendEmail.Headers = null;
      this.m_CarrierNotificationSendEmail.IncludeStatus = true;
      this.m_CarrierNotificationSendEmail.Name = "m_CarrierNotificationSendEmail";
      activitybind11.Name = "ShippingStateMachine";
      activitybind11.Path = "m_CarrierNotificationSendEmail_Subject1";
      activitybind12.Name = "ShippingStateMachine";
      activitybind12.Path = "m_CarrierNotificationSendEmail_To";
      this.m_CarrierNotificationSendEmail.MethodInvoking += new System.EventHandler(this.m_CarrierNotificationSendEmail_MethodInvoking);
      this.m_CarrierNotificationSendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.m_CarrierNotificationSendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.CCProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
      this.m_CarrierNotificationSendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
      this.m_CarrierNotificationSendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
      this.m_CarrierNotificationSendEmail.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
      // 
      // m_CarrierNotificationSendEmailLogToHistoryList
      // 
      this.m_CarrierNotificationSendEmailLogToHistoryList.Description = "Logs information about sending the email.";
      this.m_CarrierNotificationSendEmailLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_CarrierNotificationSendEmailLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind13.Name = "ShippingStateMachine";
      activitybind13.Path = "m_CarrierNotificationSendEmailLogToHistoryList_HistoryDescription";
      this.m_CarrierNotificationSendEmailLogToHistoryList.HistoryOutcome = "Email sending";
      this.m_CarrierNotificationSendEmailLogToHistoryList.Name = "m_CarrierNotificationSendEmailLogToHistoryList";
      this.m_CarrierNotificationSendEmailLogToHistoryList.OtherData = "";
      activitybind14.Name = "ShippingStateMachine";
      activitybind14.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_CarrierNotificationSendEmailLogToHistoryList.MethodInvoking += new System.EventHandler(this.m_CarrierNotificationSendEmailLogToHistoryList_MethodInvoking);
      this.m_CarrierNotificationSendEmailLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
      this.m_CarrierNotificationSendEmailLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
      // 
      // m_WaitTimeOutEventDrivenActivity
      // 
      this.m_WaitTimeOutEventDrivenActivity.Activities.Add(this.m_TimeOutDelay);
      this.m_WaitTimeOutEventDrivenActivity.Activities.Add(this.m_TimeOutLogToHistoryListActivity);
      this.m_WaitTimeOutEventDrivenActivity.Description = "Wait until time out";
      this.m_WaitTimeOutEventDrivenActivity.Name = "m_WaitTimeOutEventDrivenActivity";
      // 
      // m_ItemChangedEventDrivenActivity
      // 
      this.m_ItemChangedEventDrivenActivity.Activities.Add(this.m_OnWorkflowItemChanged);
      this.m_ItemChangedEventDrivenActivity.Activities.Add(this.m_OnWorkflowItemChangedLogToHistoryList);
      this.m_ItemChangedEventDrivenActivity.Description = "Wait until item chnged.";
      this.m_ItemChangedEventDrivenActivity.Name = "m_ItemChangedEventDrivenActivity";
      // 
      // SendingEmailsSequence
      // 
      this.SendingEmailsSequence.Activities.Add(this.m_CarrierNotificationSendEmailLogToHistoryList);
      this.SendingEmailsSequence.Activities.Add(this.m_CarrierNotificationSendEmail);
      this.SendingEmailsSequence.Description = "Send email and log information to history list.";
      this.SendingEmailsSequence.Name = "SendingEmailsSequence";
      // 
      // m_FaultHandlerLogToHistoryListActivity
      // 
      this.m_FaultHandlerLogToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_FaultHandlerLogToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind15.Name = "ShippingStateMachine";
      activitybind15.Path = "m_FaultHandlerLogToHistoryListActivity_HistoryDescription1";
      activitybind16.Name = "ShippingStateMachine";
      activitybind16.Path = "m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1";
      this.m_FaultHandlerLogToHistoryListActivity.Name = "m_FaultHandlerLogToHistoryListActivity";
      this.m_FaultHandlerLogToHistoryListActivity.OtherData = "";
      this.m_FaultHandlerLogToHistoryListActivity.UserId = -1;
      this.m_FaultHandlerLogToHistoryListActivity.MethodInvoking += new System.EventHandler(this.m_FaultHandlerLogToHistoryListActivity_MethodInvoking);
      this.m_FaultHandlerLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
      this.m_FaultHandlerLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
      // 
      // m_WaiyForEventListenActivity
      // 
      this.m_WaiyForEventListenActivity.Activities.Add(this.m_ItemChangedEventDrivenActivity);
      this.m_WaiyForEventListenActivity.Activities.Add(this.m_WaitTimeOutEventDrivenActivity);
      this.m_WaiyForEventListenActivity.Description = "Waiting until item chnged or time out occured.";
      this.m_WaiyForEventListenActivity.Name = "m_WaiyForEventListenActivity";
      activitybind17.Name = "ShippingStateMachine";
      activitybind17.Path = "SendingEmailsReplicator_InitialChildData";
      // 
      // SendingEmailsReplicator
      // 
      this.SendingEmailsReplicator.Activities.Add(this.SendingEmailsSequence);
      this.SendingEmailsReplicator.Description = "Sending Warning Emails to Selected Roles";
      this.SendingEmailsReplicator.ExecutionType = System.Workflow.Activities.ExecutionType.Sequence;
      this.SendingEmailsReplicator.Name = "SendingEmailsReplicator";
      this.SendingEmailsReplicator.ChildInitialized += new System.EventHandler<System.Workflow.Activities.ReplicatorChildEventArgs>(this.SendingEmailsReplicator_ChildInitialized);
      this.SendingEmailsReplicator.ChildCompleted += new System.EventHandler<System.Workflow.Activities.ReplicatorChildEventArgs>(this.SendingEmailsReplicator_ChildCompleted);
      this.SendingEmailsReplicator.Completed += new System.EventHandler(this.SendingEmailsReplicator_Completed);
      this.SendingEmailsReplicator.Initialized += new System.EventHandler(this.SendingEmailsReplicator_Initialized);
      this.SendingEmailsReplicator.SetBinding(System.Workflow.Activities.ReplicatorActivity.InitialChildDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
      // 
      // m_CalculateTimeoutLogToHistoryList
      // 
      this.m_CalculateTimeoutLogToHistoryList.Description = "Log information about new dedline.";
      this.m_CalculateTimeoutLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.m_CalculateTimeoutLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind18.Name = "ShippingStateMachine";
      activitybind18.Path = "m_CalculateTimeoutLogToHistoryList_HistoryDescription";
      this.m_CalculateTimeoutLogToHistoryList.HistoryOutcome = "New Timeout";
      this.m_CalculateTimeoutLogToHistoryList.Name = "m_CalculateTimeoutLogToHistoryList";
      this.m_CalculateTimeoutLogToHistoryList.OtherData = "";
      activitybind19.Name = "ShippingStateMachine";
      activitybind19.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_CalculateTimeoutLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
      this.m_CalculateTimeoutLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
      // 
      // m_CalculateTimeoutCode
      // 
      this.m_CalculateTimeoutCode.Description = "Calculate timeout nad make reports if completed.";
      this.m_CalculateTimeoutCode.Name = "m_CalculateTimeoutCode";
      this.m_CalculateTimeoutCode.ExecuteCode += new System.EventHandler(this.m_CalculateTimeoutCode_ExecuteCode);
      // 
      // m_MainFaultHandlerActivity
      // 
      this.m_MainFaultHandlerActivity.Activities.Add(this.m_FaultHandlerLogToHistoryListActivity);
      this.m_MainFaultHandlerActivity.Description = "Shipping state machine fault handler.";
      this.m_MainFaultHandlerActivity.FaultType = typeof(System.Exception);
      this.m_MainFaultHandlerActivity.Name = "m_MainFaultHandlerActivity";
      // 
      // m_SequenceActivity
      // 
      this.m_SequenceActivity.Activities.Add(this.m_CalculateTimeoutCode);
      this.m_SequenceActivity.Activities.Add(this.m_CalculateTimeoutLogToHistoryList);
      this.m_SequenceActivity.Activities.Add(this.SendingEmailsReplicator);
      this.m_SequenceActivity.Activities.Add(this.m_WaiyForEventListenActivity);
      this.m_SequenceActivity.Description = "Main Sequence Activity";
      this.m_SequenceActivity.Name = "m_SequenceActivity";
      // 
      // m_FaultHandlersActivity
      // 
      this.m_FaultHandlersActivity.Activities.Add(this.m_MainFaultHandlerActivity);
      this.m_FaultHandlersActivity.Description = "Main handler of errors";
      this.m_FaultHandlersActivity.Name = "m_FaultHandlersActivity";
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
      activitybind20.Name = "ShippingStateMachine";
      activitybind20.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_OnWokflowCompletedLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
      // 
      // m_MainLoopWhileActivity
      // 
      this.m_MainLoopWhileActivity.Activities.Add(this.m_SequenceActivity);
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.m_MainLoopWhileActivity_ConditionEventHandler);
      this.m_MainLoopWhileActivity.Condition = codecondition1;
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
      activitybind21.Name = "ShippingStateMachine";
      activitybind21.Path = "m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID";
      this.m_OnStartedLogToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind21)));
      activitybind23.Name = "ShippingStateMachine";
      activitybind23.Path = "workflowId";
      // 
      // m_OnWorkflowActivated
      // 
      this.m_OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.m_OnWorkflowActivated.Description = "On workflow activated";
      this.m_OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.m_OnWorkflowActivated.Name = "m_OnWorkflowActivated";
      activitybind22.Name = "ShippingStateMachine";
      activitybind22.Path = "m_OnWorkflowActivated_WorkflowProperties";
      this.m_OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.m_OnWorkflowActivated_Invoked);
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind23)));
      this.m_OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind22)));
      // 
      // ShippingStateMachine
      // 
      this.Activities.Add(this.m_OnWorkflowActivated);
      this.Activities.Add(this.m_OnStartedLogToHistoryListActivity);
      this.Activities.Add(this.m_MainLoopWhileActivity);
      this.Activities.Add(this.m_OnWokflowCompletedLogToHistoryListActivity);
      this.Activities.Add(this.m_FaultHandlersActivity);
      this.Name = "ShippingStateMachine";
      this.CanModifyActivities = false;

    }

    #endregion

    private SequenceActivity SendingEmailsSequence;

    private ReplicatorActivity SendingEmailsReplicator;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_CarrierNotificationSendEmailLogToHistoryList;

    private DelayActivity m_TimeOutDelay;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_CalculateTimeoutLogToHistoryList;

    private CodeActivity m_CalculateTimeoutCode;

    private Microsoft.SharePoint.WorkflowActions.SendEmail m_CarrierNotificationSendEmail;

    private FaultHandlerActivity m_MainFaultHandlerActivity;

    private FaultHandlersActivity m_FaultHandlersActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_FaultHandlerLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_TimeOutLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_OnWokflowCompletedLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_OnWorkflowItemChangedLogToHistoryList;

    private EventDrivenActivity m_WaitTimeOutEventDrivenActivity;

    private EventDrivenActivity m_ItemChangedEventDrivenActivity;

    private ListenActivity m_WaiyForEventListenActivity;

    private WhileActivity m_MainLoopWhileActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity m_OnStartedLogToHistoryListActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowItemChanged m_OnWorkflowItemChanged;

    private SequenceActivity m_SequenceActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated m_OnWorkflowActivated;





































































































































































































  }
}
