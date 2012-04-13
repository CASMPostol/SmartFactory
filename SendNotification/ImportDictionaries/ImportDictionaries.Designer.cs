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
      System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
      this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.ImportDictionaryCodeActivity = new System.Workflow.Activities.CodeActivity();
      this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.OnWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // logToHistoryListActivity2
      // 
      this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.logToHistoryListActivity2.HistoryDescription = "";
      this.logToHistoryListActivity2.HistoryOutcome = "";
      this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
      this.logToHistoryListActivity2.OtherData = "";
      this.logToHistoryListActivity2.UserId = -1;
      // 
      // ImportDictionaryCodeActivity
      // 
      this.ImportDictionaryCodeActivity.Description = "Import Dictionary Code Activity";
      this.ImportDictionaryCodeActivity.Name = "ImportDictionaryCodeActivity";
      this.ImportDictionaryCodeActivity.ExecuteCode += new System.EventHandler(this.ImportDictionaryCodeActivity_ExecuteCode);
      // 
      // logToHistoryListActivity1
      // 
      this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
      this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      this.logToHistoryListActivity1.HistoryDescription = "";
      this.logToHistoryListActivity1.HistoryOutcome = "";
      this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
      this.logToHistoryListActivity1.OtherData = "";
      this.logToHistoryListActivity1.UserId = -1;
      activitybind2.Name = "ImportDictionaries";
      activitybind2.Path = "workflowId";
      // 
      // OnWorkflowActivated
      // 
      correlationtoken1.Name = "m_WorkflowToken";
      correlationtoken1.OwnerActivityName = "ImportDictionaries";
      this.OnWorkflowActivated.CorrelationToken = correlationtoken1;
      this.OnWorkflowActivated.Description = "Import Dictionaries";
      this.OnWorkflowActivated.EventName = "OnWorkflowActivated";
      this.OnWorkflowActivated.Name = "OnWorkflowActivated";
      activitybind1.Name = "ImportDictionaries";
      activitybind1.Path = "workflowProperties";
      this.OnWorkflowActivated.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnWorkflowActivated_Invoked);
      this.OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
      this.OnWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
      // 
      // ImportDictionaries
      // 
      this.Activities.Add(this.OnWorkflowActivated);
      this.Activities.Add(this.logToHistoryListActivity1);
      this.Activities.Add(this.ImportDictionaryCodeActivity);
      this.Activities.Add(this.logToHistoryListActivity2);
      this.Name = "ImportDictionaries";
      this.CanModifyActivities = false;

    }

    #endregion

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated OnWorkflowActivated;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;
    private CodeActivity ImportDictionaryCodeActivity;
    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

  }
}
