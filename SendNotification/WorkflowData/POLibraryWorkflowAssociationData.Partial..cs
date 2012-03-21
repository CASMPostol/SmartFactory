using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Workflow;
using System.IO;
using System.Xml.Serialization;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class POLibraryWorkflowAssociationData
  {
    private POLibraryWorkflowAssociationData(string _title)
    {
      Content = "Content of the message"; //TODO define content of the mail
      Title = _title;
    }
    public static POLibraryWorkflowAssociationData SecurityPOAssociationData() { return new POLibraryWorkflowAssociationData(Security); }
    public static POLibraryWorkflowAssociationData FreightPOAssociationData() { return new POLibraryWorkflowAssociationData(Freight); }
    private const string Security = "Security";
    private const string Freight = "Freight";
    internal string Serialize( SPWorkflowAssociation _wa)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
        serializer.Serialize(stream, this);
        stream.Position = 0;
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        return Encoding.UTF8.GetString(bytes);
      }
    }
  }
}
