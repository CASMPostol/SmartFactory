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

namespace CAS.SmartFactory.Shepherd.SendNotification.ImportDictionaries
{
  public sealed partial class ImportDictionaries
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
      System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
      this.FaultHandlerLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.TemplatesCreatedLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateTemplates = new System.Workflow.Activities.CodeActivity();
      this.TeplatesCreationLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.FaultHandlerActivity = new System.Workflow.ComponentModel.FaultHandlerActivity();
      this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
      this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
      this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
      this.ImportFinischedLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ImportDictionaryCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.StartimgImportLog = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // FaultHandlerLog
      // 
      this.FaultHandlerLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.FaultHandlerLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "ImportDictionaries";
      activitybind1.Path = "FaultHandlerLog_HistoryDescription";
      this.FaultHandlerLog.HistoryOutcome = "FaultHandler";
      this.FaultHandlerLog.Name = "FaultHandlerLog";
      this.FaultHandlerLog.OtherData = "";
      activitybind2.Name = "ImportDictionaries";
      activitybind2.Path = "workflowProperties.OriginatorUser.ID";
      this.FaultHandlerLog.MethodInvoking += new System.EventHandler(this.FaultHandlerLog_MethodInvoking);
      this.FaultHandlerLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.FaultHandlerLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      // 
      // TemplatesCreatedLog
      // 
      this.TemplatesCreatedLog.Description = "Time Slot templates have been successfully created.";
      this.TemplatesCreatedLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.TemplatesCreatedLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.TemplatesCreatedLog.HistoryDescription = "Time Slot templates have been successfully created.";
      this.TemplatesCreatedLog.HistoryOutcome = "TS Templates";
      this.TemplatesCreatedLog.Name = "TemplatesCreatedLog";
      this.TemplatesCreatedLog.OtherData = "";
      activitybind3.Name = "ImportDictionaries";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.TemplatesCreatedLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // CreateTemplates
      // 
      this.CreateTemplates.Name = "CreateTemplates";
      this.CreateTemplates.ExecuteCode += new System.EventHandler(this.CreateTemplates_ExecuteCode);
      // 
      // TeplatesCreationLog
      // 
      this.TeplatesCreationLog.Description = "Starting to create time slots templates.";
      this.TeplatesCreationLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.TeplatesCreationLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.TeplatesCreationLog.HistoryDescription = "Starting to create time slots templates.";
      this.TeplatesCreationLog.HistoryOutcome = "TS Templates";
      this.TeplatesCreationLog.Name = "TeplatesCreationLog";
      this.TeplatesCreationLog.OtherData = "";
      activitybind4.Name = "ImportDictionaries";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.TeplatesCreationLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      // 
      // FaultHandlerActivity
      // 
      this.FaultHandlerActivity.Activities.Add(this.FaultHandlerLog);
      this.FaultHandlerActivity.Description = "Fault Handler Activity";
      this.FaultHandlerActivity.FaultType = typeof(System.Exception);
      this.FaultHandlerActivity.Name = "FaultHandlerActivity";
      // 
      // ifElseBranchActivity2
      // 
      this.ifElseBranchActivity2.Name = "ifElseBranchActivity2";
      // 
      // ifElseBranchActivity1
      // 
      this.ifElseBranchActivity1.Activities.Add(this.TeplatesCreationLog);
      this.ifElseBranchActivity1.Activities.Add(this.CreateTemplates);
      this.ifElseBranchActivity1.Activities.Add(this.TemplatesCreatedLog);
      codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.TemplateCreationCondition);
      this.ifElseBranchActivity1.Condition = codecondition1;
      this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
      // 
      // faultHandlersActivity1
      // 
      this.faultHandlersActivity1.Activities.Add(this.FaultHandlerActivity);
      this.faultHandlersActivity1.Name = "faultHandlersActivity1";
      // 
      // ifElseActivity1
      // 
      this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity1);
      this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity2);
      this.ifElseActivity1.Name = "ifElseActivity1";
      // 
      // ImportFinischedLog
      // 
      this.ImportFinischedLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.ImportFinischedLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.ImportFinischedLog.HistoryDescription = "Import Dictionaries successfully finished.";
      this.ImportFinischedLog.HistoryOutcome = "Import Dictionaries";
      this.ImportFinischedLog.Name = "ImportFinischedLog";
      this.ImportFinischedLog.OtherData = "";
      activitybind5.Name = "ImportDictionaries";
      activitybind5.Path = "workflowProperties.OriginatorUser.ID";
      this.ImportFinischedLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // ImportDictionaryCodeActivity
      // 
      this.ImportDictionaryCodeActivity.Description = "Import Dictionary Code Activity";
      this.ImportDictionaryCodeActivity.Name = "ImportDictionaryCodeActivity";
      this.ImportDictionaryCodeActivity.ExecuteCode += new System.EventHandler(this.ImportDictionaryCodeActivity_ExecuteCode);
      // 
      // StartimgImportLog
      // 
      this.StartimgImportLog.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.StartimgImportLog.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.StartimgImportLog.HistoryDescription = "Starting import of the dictionaries data.";
      this.StartimgImportLog.HistoryOutcome = "Import Dictionaries";
      this.StartimgImportLog.Name = "StartimgImportLog";
      this.StartimgImportLog.OtherData = "";
      activitybind6.Name = "ImportDictionaries";
      activitybind6.Path = "workflowProperties.OriginatorUser.ID";
      this.StartimgImportLog.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      activitybind8.Name = "ImportDictionaries";
      activitybind8.Path = "workflowId";
      // 
      // onWorkflowActivated1
      // 
      correlationtoken1.Name = "m_WorkflowToken";
      correlationtoken1.OwnerActivityName = "ImportDictionaries";
      this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated1.Description = "Import Dictionaries";
      this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated1.Name = "onWorkflowActivated1";
      activitybind7.Name = "ImportDictionaries";
      activitybind7.Path = "workflowProperties";
      this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnWorkflowActivated_Invoked);
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
      this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
      // 
      // ImportDictionaries
      // 
      this.Activities.Add(this.onWorkflowActivated1);
      this.Activities.Add(this.StartimgImportLog);
      this.Activities.Add(this.ImportDictionaryCodeActivity);
      this.Activities.Add(this.ImportFinischedLog);
      this.Activities.Add(this.ifElseActivity1);
      this.Activities.Add(this.faultHandlersActivity1);
      this.Name = "ImportDictionaries";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity FaultHandlerLog;
    private FaultHandlerActivity FaultHandlerActivity;
    private FaultHandlersActivity faultHandlersActivity1;
    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity TemplatesCreatedLog;
    private CodeActivity CreateTemplates;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity TeplatesCreationLog;
    private IfElseBranchActivity ifElseBranchActivity2;
    private IfElseBranchActivity ifElseBranchActivity1;
    private IfElseActivity ifElseActivity1;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity ImportFinischedLog;
    private CodeActivity ImportDictionaryCodeActivity;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity StartimgImportLog;

  }
}
