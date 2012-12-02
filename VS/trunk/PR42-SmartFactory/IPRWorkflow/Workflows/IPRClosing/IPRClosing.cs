using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace CAS.SmartFactory.IPR.Workflows.IPRClosing
{
  public sealed partial class IPRClosing: SequentialWorkflowActivity
  {
    public IPRClosing()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void RecordValidation_ExecuteCode( object sender, EventArgs e )
    {

    }
    private bool Valid = false;
    private void ProcessIfValid( object sender, ConditionalEventArgs e )
    {
      e.Result = Valid;
    }

    private void Closeing_ExecuteCode( object sender, EventArgs e )
    {

    }
  }
}
