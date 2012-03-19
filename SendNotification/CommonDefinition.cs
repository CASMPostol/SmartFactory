using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  internal static class CommonDefinition
  {
    //Workflow templates
    internal static Guid SendNotificationWorkflowTemplateId = new Guid("1bb10ba9-70da-4064-95b3-cd048ce4c3cd");
    internal static Guid CreatePOWorkflowTemplateId = new Guid("54732fdd-0178-406a-aae1-7fdfb11ed7e7");
    internal static Guid ShippingStateMachineTemplateID = new Guid("cd61e1a0-3401-40f9-9eb1-c7428f6f2516");
    //Target lists
    internal const string FreightPOLibraryName = "Freight PO Library";
    internal const string ShippingListName = "Shipping";
    //Workflow working lists
    internal const string SendNotificationWorkflowTasks = "Send Notification Workflow Tasks";
    internal const string SendNotificationWorkflowHistory = "Send Notification Workflow History";
    internal const string PartnerSentToBackupEmail = "oferty@cas.eu";
  }
}
