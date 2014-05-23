using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Workflow;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class POLibraryWorkflowAssociationData
  {
    //User-friendly name for workflow association
    private const string Security = "Email Escort PO";
    private const string Freight = "Email Freight PO";
    private POLibraryWorkflowAssociationData(string _name, bool _carrier)
    {
      this.Carrier = _carrier;
      this.Name = _name;
    }
    public POLibraryWorkflowAssociationData() { }
    public static POLibraryWorkflowAssociationData SecurityPOAssociationData() { return new POLibraryWorkflowAssociationData(Security, false); }
    public static POLibraryWorkflowAssociationData FreightPOAssociationData() { return new POLibraryWorkflowAssociationData(Freight, true); }
    internal string Serialize(SPWorkflowAssociation _wa)
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
    internal static POLibraryWorkflowAssociationData Deserialize(string _input)
    {
      try
      {

        // deserialize initiation data; 
        XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
        XmlTextReader reader = new XmlTextReader(new StringReader(_input));
        return (POLibraryWorkflowAssociationData)serializer.Deserialize(reader);
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in WorkflowActivated because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
  }
}
