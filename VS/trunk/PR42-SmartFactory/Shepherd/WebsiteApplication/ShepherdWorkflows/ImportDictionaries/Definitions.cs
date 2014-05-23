using System;

namespace CAS.SmartFactory.Shepherd.Workflows.ImportDictionaries
{
  internal static class Definitions
  {
    internal const string ID = "889555fc-cf88-4c9c-ba0a-c0f6496cb109";
    internal static Guid IDGuid = new Guid(ID);
    internal const string Name = "ImportDictionaries";
    internal const string Description = "Import data to dictionaries.";
    internal static WorkflowDescription WorkflowDescription
    {
      get
      {
        return new WorkflowDescription(IDGuid, "Import Dictionaries", Description);
      }
    }
  }
}
