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

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.CreateStatement
{
  public sealed partial class CreateStatement
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
      this.logToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onCreateDocument = new System.Workflow.Activities.CodeActivity();
      this.onStartingLogToHistoryList = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // logToHistoryListActivity
      // 
      this.logToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.logToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "CreateStatement";
      activitybind1.Path = "logToHistoryListActivity_HistoryDescription";
      activitybind2.Name = "CreateStatement";
      activitybind2.Path = "logToHistoryListActivity_HistoryOutcome";
      this.logToHistoryListActivity.Name = "logToHistoryListActivity";
      this.logToHistoryListActivity.OtherData = "";
      activitybind3.Name = "CreateStatement";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.logToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      this.logToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.logToHistoryListActivity.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
      // 
      // onCreateDocument
      // 
      this.onCreateDocument.Description = "Main code of the workflow.";
      this.onCreateDocument.Name = "onCreateDocument";
      this.onCreateDocument.ExecuteCode += new System.EventHandler(this.onCreateDocument_ExecuteCode);
      // 
      // onStartingLogToHistoryList
      // 
      this.onStartingLogToHistoryList.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.onStartingLogToHistoryList.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowStarted;
      this.onStartingLogToHistoryList.HistoryDescription = "Creation of the document started";
      this.onStartingLogToHistoryList.HistoryOutcome = "Document creation";
      this.onStartingLogToHistoryList.Name = "onStartingLogToHistoryList";
      this.onStartingLogToHistoryList.OtherData = "";
      activitybind4.Name = "CreateStatement";
      activitybind4.Path = "workflowProperties.OriginatorUser.ID";
      this.onStartingLogToHistoryList.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
      activitybind6.Name = "CreateStatement";
      activitybind6.Path = "workflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "CreateStatement";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind5.Name = "CreateStatement";
      activitybind5.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
      this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
      // 
      // CreateStatement
      // 
      this.Activities.Add(this.onWorkflowActivated);
      this.Activities.Add(this.onStartingLogToHistoryList);
      this.Activities.Add(this.onCreateDocument);
      this.Activities.Add(this.logToHistoryListActivity);
      this.Name = "CreateStatement";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity;

    private CodeActivity onCreateDocument;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity onStartingLogToHistoryList;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;








  }
}
