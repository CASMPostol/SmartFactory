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
      System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
      System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
      System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
      this.CreateMessageTemplates = new System.Workflow.Activities.CodeActivity();
      this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
      // 
      // CreateMessageTemplates
      // 
      this.CreateMessageTemplates.Description = "Creates new clearances ans associated custom messages..";
      this.CreateMessageTemplates.Name = "CreateMessageTemplates";
      this.CreateMessageTemplates.ExecuteCode += new System.EventHandler( this.onCreateMessageTemplates );
      activitybind2.Name = "ClearThroughCustoms";
      activitybind2.Path = "workflowId";
      // 
      // onWorkflowActivated
      // 
      correlationtoken1.Name = "workflowToken";
      correlationtoken1.OwnerActivityName = "ClearThroughCustoms";
      this.onWorkflowActivated.CorrelationToken = correlationtoken1;
      this.onWorkflowActivated.EventName = "OnWorkflowActivated";
      this.onWorkflowActivated.Name = "onWorkflowActivated";
      activitybind1.Name = "ClearThroughCustoms";
      activitybind1.Path = "workflowProperties";
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind2 ) ) );
      this.onWorkflowActivated.SetBinding( Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ( (System.Workflow.ComponentModel.ActivityBind)( activitybind1 ) ) );
      // 
      // ClearThroughCustoms
      // 
      this.Activities.Add( this.onWorkflowActivated );
      this.Activities.Add( this.CreateMessageTemplates );
      this.Name = "ClearThroughCustoms";
      this.CanModifyActivities = false;

    }

    #endregion

    private CodeActivity CreateMessageTemplates;

    private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;




  }
}
