using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Workflow;
using System.Xml.Serialization;
using System.IO;
using CAS.SmartFactorySendNotification.WorkflowData;
using System.Text;

namespace CAS.SmartFactorySendNotification.Features.SendNotification
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>
  [Guid("e10c5c4a-3464-4ade-861e-f91f23955dea")]
  public class SendNotificationEventReceiver : SPFeatureReceiver
  {
    /// <summary>
    /// Occurs after a Feature is activated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      SPSite _siteCollection = (SPSite)properties.Feature.Parent;
      SPWeb _web = _siteCollection.RootWeb;
      // obtain referecnes to lists
      SPList _targetList = _web.Lists[CommonDefinition.FreightPOLibrary];
      SPList _taskList = _web.Lists[CommonDefinition.SendNotificationWorkflowTasks];
      SPList _historyList = _web.Lists[CommonDefinition.SendNotificationWorkflowHistory];
      _taskList.UseFormsForDisplay = false;
      _taskList.Update();
      // obtain reference to workflow template
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[CommonDefinition.SendNotificationWorkflowTemplateId];
      // create workflow association
      SPWorkflowAssociation _freightPOLibraryWorkflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate, 
        CommonDefinition.SendNotificationWorkflowAssociationName, 
        _taskList, 
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _freightPOLibraryWorkflowAssociation.Description = "Used to send the PO by email.";
      _freightPOLibraryWorkflowAssociation.AllowManual = true;
      _freightPOLibraryWorkflowAssociation.AutoStartCreate = true;
      _freightPOLibraryWorkflowAssociation.AutoStartChange = true;
      // add workflow association data
      POLibraryWorkflowAssociationData _wfData = new POLibraryWorkflowAssociationData()
      {
        Content = "Content of the message", //TODO define content of the mail
        Title = "New fright purchase order"
      };
      using (MemoryStream stream = new MemoryStream())
      {
        XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
        serializer.Serialize(stream, _wfData);
        stream.Position = 0;
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        _freightPOLibraryWorkflowAssociation.AssociationData = Encoding.UTF8.GetString(bytes);
      }
      _targetList.WorkflowAssociations.Add(_freightPOLibraryWorkflowAssociation);
    }
    /// <summary>
    /// Occurs when a Feature is deactivated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    {
      SPSite siteCollection = (SPSite)properties.Feature.Parent;
      SPWeb site = siteCollection.RootWeb;

      // remove association
      try
      {
        SPList _productProposalsList = site.Lists[CommonDefinition.FreightPOLibrary];
        Guid _workflowTemplateId = CommonDefinition.SendNotificationWorkflowTemplateId;
        SPWorkflowAssociation _wfa = _productProposalsList.WorkflowAssociations.GetAssociationByBaseID(_workflowTemplateId);
        _productProposalsList.WorkflowAssociations.Remove(_wfa.Id);
      }
      catch { }
    }
  }
}
