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
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;

namespace CAS.SmartFactory.Shepherd.SendNotification.AddTimeSlots
{
  public sealed partial class AddTimeSlots : SequentialWorkflowActivity
  {
    public AddTimeSlots()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    public String StartLogToHistory_HistoryDescription = default(System.String);

    private void StartLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      TimeSlotsInitiationData _data = TimeSlotsInitiationData.Deserialize(workflowProperties.InitiationData);
      StartLogToHistory_HistoryDescription = String.Format("Starting applayin the template. From: {0}, Weeks: {1}", _data.StartDate, _data.Duration); ;
    }
    private void AddTimeslotsActivity_ExecuteCode(object sender, EventArgs e)
    {

    }
    public String FinischLogToHistory_HistoryDescription2 = default(System.String);

    private void FinischLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      FinischLogToHistory_HistoryDescription2 = "The template has been applied and timeslots created";
    }

    public String FaultHandlerLogToHistory_HistoryDescription = default(System.String);

    private void FaultHandlerLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      FaultHandlerLogToHistory_HistoryDescription = FaultHandler.Fault.Message;
    }
  }
}
