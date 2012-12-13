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
  /// <summary>
  /// Create Report
  /// </summary>
  public sealed partial class CreateReport: SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateReport" /> class.
    /// </summary>
    public CreateReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = new System.Guid( "{0B735E12-FEE5-48E7-9727-D11CEE7866C6}" );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    #region CreateListsItems
    private void CreateRecorts( object sender, EventArgs e )
    {

    }
    #endregion

    #region ValidationSequence
    private void Validate( object sender, EventArgs e )
    {
      ValidationPaased = false;
    }
    private bool ValidationPaased = false;
    private void Validated( object sender, ConditionalEventArgs e )
    {
      e.Result = ValidationPaased;
    }
    #endregion

    #region MainReportCreationSequence
    private void CalculateReports( object sender, EventArgs e )
    {
      CalculationPassed = false;
    }
    private bool CalculationPassed = false;
    private void Calculated( object sender, ConditionalEventArgs e )
    {
      e.Result = CalculationPassed;
    }
    private void GenerateReport( object sender, EventArgs e )
    {

    }
    #endregion

  }
}
