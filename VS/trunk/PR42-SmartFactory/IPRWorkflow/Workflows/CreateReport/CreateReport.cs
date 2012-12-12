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

namespace CAS.SmartFactory.IPR.Workflows.CreateReport
{
  public sealed partial class CreateReport: SequentialWorkflowActivity
  {
    public CreateReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void CreateRecorts( object sender, EventArgs e )
    {

    }

    private void Validated( object sender, ConditionalEventArgs e )
    {

    }

    private void CalculateReports( object sender, EventArgs e )
    {

    }

    private void GenerateReport( object sender, EventArgs e )
    {

    }

    private void Calculated( object sender, ConditionalEventArgs e )
    {

    }

    private void Validate( object sender, EventArgs e )
    {

    }
  }
}
