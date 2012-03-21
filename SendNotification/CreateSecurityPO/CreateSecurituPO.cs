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

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSecurityPO
{
  public sealed partial class CreateSecurityPO : SequentialWorkflowActivity
  {
    public CreateSecurityPO()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("b0d933a7-f1b4-4659-9181-b314ea3a0d29");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Security PO", "Create Security Escort Purchae Order"); } }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
  }
}
