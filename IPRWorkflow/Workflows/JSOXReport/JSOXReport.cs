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
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Workflows.JSOXReport
{
  public sealed partial class JSOXReport: SequentialWorkflowActivity
  {
    public JSOXReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void CreateReport( object sender, EventArgs e )
    {
      try
      {
        using ( Entities edc = new Entities( workflowProperties.WebUrl ) )
        {
          JSOXLib _list = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, workflowProperties.ItemId );
        }
      }
      catch ( Exception ex )
      {

        EndLogToHistory_HistoryDescription = "Fatal error: {0}";
        EndLogToHistory_HistoryOutcome = "Exception";
      }
    }
    public String EndLogToHistory_HistoryOutcome = default( System.String );
    public String EndLogToHistory_HistoryDescription = default( System.String );
  }
}
