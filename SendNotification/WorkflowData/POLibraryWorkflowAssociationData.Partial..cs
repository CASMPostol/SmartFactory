using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class POLibraryWorkflowAssociationData
  {
    private POLibraryWorkflowAssociationData()
    {
      Content = "Content of the message"; //TODO define content of the mail
      Title = "New fright purchase order";
    }
    public static POLibraryWorkflowAssociationData SecurityPOAssociationData() { return new POLibraryWorkflowAssociationData(); }
    public static POLibraryWorkflowAssociationData FreightPOAssociationData() { return new POLibraryWorkflowAssociationData(); }

  }
}
