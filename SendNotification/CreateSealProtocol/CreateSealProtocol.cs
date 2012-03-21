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

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSealProtocol
{
  public sealed partial class CreateSealProtocol : SequentialWorkflowActivity
  {
    public CreateSealProtocol()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("dddc274f-d1ff-4613-834a-548546f527aa");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Seal Protocol", "Create Seal Protocol"); } }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
  }
}
