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

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.ClearThroughCustoms
{
  public sealed partial class ClearThroughCustoms
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
      System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
      this.logToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateMessageTemplates = new System.Workflow.Activities.CodeActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // logToHistoryListActivity
      // 
      this.logToHistoryListActivity.Description = "Logs workflow result";
      this.logToHistoryListActivity.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.logToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "ClearThroughCustoms";
      activitybind1.Path = "logToHistoryListActivity_HistoryDescription";
      activitybind2.Name = "ClearThroughCustoms";
      activitybind2.Path = "logToHistoryListActivity_HistoryOutcome";
      this.logToHistoryListActivity.Name = "logToHistoryListActivity";
      this.logToHistoryListActivity.OtherData = "";
      activitybind3.Name = "ClearThroughCustoms";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      // 
      // CreateMessageTemplates
      // 
      this.CreateMessageTemplates.Description = "Creates new clearances ans associated custom messages..";
      this.CreateMessageTemplates.Name = "CreateMessageTemplates";
      this.CreateMessageTemplates.ExecuteCode += new System.EventHandler( this.onCreateMessageTemplates );
      activitybind5.Name = "ClearThroughCustoms";
      activitybind5.Path = "workflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "ClearThroughCustoms";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind4.Name = "ClearThroughCustoms";
      activitybind4.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      // 
      // ClearThroughCustoms
      // 
      this.Activities.Add( this.onWorkflowActivated );
      this.Activities.Add( this.CreateMessageTemplates );
      this.Activities.Add( this.logToHistoryListActivity );
      this.Name = "ClearThroughCustoms";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity;

    private CodeActivity CreateMessageTemplates;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;








  }
}
