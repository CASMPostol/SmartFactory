using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.Features
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
      string _state = default(string);
      try
      {
        using (SPSite _siteCollection = (SPSite)properties.Feature.Parent)
        {
          _state = "Feature.Parent";
          using (SPWeb _web = _siteCollection.RootWeb)
          {
            // obtain referecnes to lists
            SPList _taskList = _web.Lists[CommonDefinition.SendNotificationWorkflowTasks];
            SPList _historyList = _web.Lists[CommonDefinition.SendNotificationWorkflowHistory];
            _taskList.UseFormsForDisplay = false;
            _taskList.Update();
            _state = "_taskList.Update";
            // obtain reference to workflow template
            AddFPONotificationAssociation(_web, _taskList, _historyList);
            _state = "AddFPONotificationAssociation";
            AddCreateFPOAssociation(_web, _taskList, _historyList);
            _state = "AddCreateFPOAssociation";
            AddShippingStateMachineWorkflowAssociation(_web, _taskList, _historyList);
            _state = "AddShippingStateMachineWorkflowAssociation";
          }
        }
      }
      catch (Exception _ex)
      {
        string _frmt = "ActivationContext failed in the {0} state because of the error {1}";
        throw new ApplicationException(String.Format(_frmt, _state, _ex.Message));
      }
    }
    /// <summary>
    /// Occurs when a Feature is deactivated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    {
      string _state = default(string);
      try
      {
        SPSite siteCollection = (SPSite)properties.Feature.Parent;
        SPWeb site = siteCollection.RootWeb;
        RemoveWorkflowAssociation(site.Lists[CommonDefinition.FreightPOLibraryName], CommonDefinition.SendNotificationWorkflowTemplateId);
        RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListName], CommonDefinition.CreatePOWorkflowTemplateId);
        RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListName], CommonDefinition.ShippingStateMachineTemplateID);
      }
      catch (Exception _ex)
      {
        string _frmt = "FeatureDeactivating failed in the {0} state because of the error {1}";
        throw new ApplicationException(String.Format(_frmt, _state, _ex.Message));
      }
    }
    private static void AddFPONotificationAssociation(SPWeb _web, SPList _taskList, SPList _historyList)
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[CommonDefinition.SendNotificationWorkflowTemplateId];
      // create workflow association
      SPWorkflowAssociation _freightPOLibraryWorkflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        "Notify on Fright PO created", //User-friendly name for workflow association
       _taskList,
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _freightPOLibraryWorkflowAssociation.Description = "Send PO by email";
      _freightPOLibraryWorkflowAssociation.AllowManual = true;
      _freightPOLibraryWorkflowAssociation.AutoStartCreate = false;
      _freightPOLibraryWorkflowAssociation.AutoStartChange = false;
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
      SPList _targetList = _web.Lists[CommonDefinition.FreightPOLibraryName];
      _targetList.WorkflowAssociations.Add(_freightPOLibraryWorkflowAssociation);
    }
    private static void AddCreateFPOAssociation(SPWeb _web, SPList _taskList, SPList _historyList)
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[CommonDefinition.CreatePOWorkflowTemplateId];
      // create workflow association
      SPWorkflowAssociation _freightPOLibraryWorkflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        "It creates new  freight PO for selected shipping.",
        _taskList,
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _freightPOLibraryWorkflowAssociation.Description = "Create new Fraight PO";
      _freightPOLibraryWorkflowAssociation.AllowManual = true;
      _freightPOLibraryWorkflowAssociation.AutoStartCreate = false;
      _freightPOLibraryWorkflowAssociation.AutoStartChange = false;
      _freightPOLibraryWorkflowAssociation.AssociationData = "Any data if needed.";
      SPList _targetList = _web.Lists[CommonDefinition.ShippingListName];
      _targetList.WorkflowAssociations.Add(_freightPOLibraryWorkflowAssociation);
    }
    private static void AddShippingStateMachineWorkflowAssociation(SPWeb _web, SPList _taskList, SPList _historyList)
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[CommonDefinition.ShippingStateMachineTemplateID];
      // create workflow association
      SPWorkflowAssociation _association = SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        "Shipping  State Machine Workflow.",
        _taskList,
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _association.Description = "Shipping state machine";
      _association.AllowManual = true;
      _association.AutoStartCreate = false;
      _association.AutoStartChange = false;
      _association.AssociationData = "Any data if needed.";
      SPList _targetList = _web.Lists[CommonDefinition.ShippingListName];
      _targetList.WorkflowAssociations.Add(_association);
    }
    /// <summary>
    /// Removes the workflow association.
    /// </summary>
    /// <param name="_list">The _product proposals list.</param>
    /// <param name="_workflowTemplateId">The _workflow template id.</param>
    private static void RemoveWorkflowAssociation(SPList _list, Guid _workflowTemplateId)
    {
      SPWorkflowAssociation _wfa = _list.WorkflowAssociations.GetAssociationByBaseID(_workflowTemplateId);
      _list.WorkflowAssociations.Remove(_wfa.Id);
    }
  }
}
