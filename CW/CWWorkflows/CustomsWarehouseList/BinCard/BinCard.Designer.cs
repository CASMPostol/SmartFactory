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

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.BinCard
{
  public sealed partial class BinCard
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
      this.faultHandlersActivity = new System.Workflow.ComponentModel.FaultHandlersActivity();
      this.logToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
      this.CreateBinCardActivity = new System.Workflow.Activities.CodeActivity();
      this.onBinCardWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // faultHandlersActivity
      // 
      this.faultHandlersActivity.Description = "Exception handler";
      this.faultHandlersActivity.Name = "faultHandlersActivity";
      // 
      // logToHistoryListActivity
      // 
      this.logToHistoryListActivity.Duration = System.TimeSpan.Parse( "-10675199.02:48:05.4775808" );
      this.logToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
      activitybind1.Name = "BinCard";
      activitybind1.Path = "logToHistoryListActivity_HistoryDescription";
      activitybind2.Name = "BinCard";
      activitybind2.Path = "logToHistoryListActivity_HistoryOutcome";
      this.logToHistoryListActivity.Name = "logToHistoryListActivity";
      this.logToHistoryListActivity.OtherData = "";
      activitybind3.Name = "BinCard";
      activitybind3.Path = "workflowProperties.OriginatorUser.ID";
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.UserIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind3 ) ) );
      this.logToHistoryListActivity.SetBinding( Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      // 
      // CreateBinCardActivity
      // 
      this.CreateBinCardActivity.Name = "CreateBinCardActivity";
      this.CreateBinCardActivity.ExecuteCode += new System.EventHandler( this.CreateBinCard );
      activitybind5.Name = "BinCard";
      activitybind5.Path = "workflowId";
      // 
      // onBinCardWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "BinCard";
      this.onBinCardWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onBinCardWorkflowActivated.Description = "Creates BinCard";
      this.onBinCardWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onBinCardWorkflowActivated.Name = "onBinCardWorkflowActivated";
      activitybind4.Name = "BinCard";
      activitybind4.Path = "workflowProperties";
      this.onBinCardWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind5 ) ) );
      this.onBinCardWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind4 ) ) );
      // 
      // BinCard
      // 
      this.Activities.Add( this.onBinCardWorkflowActivated );
      this.Activities.Add( this.CreateBinCardActivity );
      this.Activities.Add( this.logToHistoryListActivity );
      this.Activities.Add( this.faultHandlersActivity );
      this.Description = "Creates the BinCard for selected CW Entry";
      this.Name = "BinCard";
      this.CanModifyActivities = false;

    }

    #endregion

    private FaultHandlersActivity faultHandlersActivity;

    private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity;

    private CodeActivity CreateBinCardActivity;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onBinCardWorkflowActivated;















  }
}
