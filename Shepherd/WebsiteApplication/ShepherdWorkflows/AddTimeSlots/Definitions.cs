using System;

namespace CAS.SmartFactory.Shepherd.Workflows.AddTimeSlots
{
  internal static class Definitions
  {
    internal const string ID = "2e240fb3-f93c-4b87-9170-b5443815793b";
    internal static Guid IDGuid = new Guid(ID);
    internal const string Name = "AddTimeSlots";
    internal const string Description = "Adds Time Slots according to the selected template.";
    internal static WorkflowDescription WorkflowDescription
    {
      get
      {
        return new WorkflowDescription(IDGuid, "Create Timeslots", Description);
      }
    }
  }
}
