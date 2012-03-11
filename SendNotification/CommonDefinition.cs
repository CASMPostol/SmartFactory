using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactorySendNotification
{
  internal static class CommonDefinition
  {
    internal static Guid SendNotificationWorkflowTemplateId = new Guid("1bb10ba9-70da-4064-95b3-cd048ce4c3cd");
    internal const string FreightPOLibrary = "Freight PO Library";
    internal const string SendNotificationWorkflowTasks = "Send Notification Workflow Tasks";
    internal const string SendNotificationWorkflowHistory = "Send Notification Workflow History";
    /// <summary>
    ///User-friendly name for workflow association
    /// </summary>
    internal const string SendNotificationWorkflowAssociationName = "Notify on Fright PO created";
  }
}
