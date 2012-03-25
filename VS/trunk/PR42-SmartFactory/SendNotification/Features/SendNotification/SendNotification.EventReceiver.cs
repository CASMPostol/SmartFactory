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
            NewPONotificationAssociation(CommonDefinition.FreightPOLibraryTitle, _web, _taskList, _historyList, POLibraryWorkflowAssociationData.FreightPOAssociationData(), "Email Freight PO");
            _state = "FreightPOLibraryName";
            NewPONotificationAssociation(CommonDefinition.EscortPOLibraryTitle, _web, _taskList, _historyList, POLibraryWorkflowAssociationData.SecurityPOAssociationData(), "Email Escort PO");
            _state = "EscortPOLibraryTitle";
            NewCreatePOAssociation(CreatePO.CreatePO.WorkflowDescription, _web, _taskList, _historyList);
            _state = "AddCreateFPOAssociation";
            NewCreatePOAssociation(CreateSecurityPO1.CreateSecurityPO1.WorkflowDescription, _web, _taskList, _historyList);
            _state = "AddCreateFPOAssociation";
            NewCreatePOAssociation(CreateSealProtocol.CreateSealProtocol.WorkflowDescription, _web, _taskList, _historyList);
            _state = "AddCreateFPOAssociation";
            NewCreatePOAssociation(ShippingStateMachine.ShippingStateMachine.WorkflowDescription, _web, _taskList, _historyList);
            _state = "ShippingStateMachine";
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
        using (SPSite siteCollection = (SPSite)properties.Feature.Parent)
        {
          using (SPWeb site = siteCollection.RootWeb)
          {
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListTitle], ShippingStateMachine.ShippingStateMachine.WorkflowId);
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListTitle], CreateSealProtocol.CreateSealProtocol.WorkflowId);
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListTitle], CreateSecurityPO1.CreateSecurityPO1.WorkflowId);
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.ShippingListTitle], CreatePO.CreatePO.WorkflowId);
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.EscortPOLibraryTitle], SendEmail.SendEmail.WorkflowId);
            RemoveWorkflowAssociation(site.Lists[CommonDefinition.FreightPOLibraryTitle], SendEmail.SendEmail.WorkflowId);
          }
        }
      }
      catch (Exception _ex)
      {
        string _frmt = "FeatureDeactivating failed in the {0} state because of the error {1}";
        throw new ApplicationException(String.Format(_frmt, _state, _ex.Message));
      }
    }
    private static void NewPONotificationAssociation(string _targetList, SPWeb _web, SPList _taskList, SPList _historyList, POLibraryWorkflowAssociationData _wfData, string _wName)
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[SendEmail.SendEmail.WorkflowId];
      // create workflow association
      SPWorkflowAssociation _wa = SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        _wName, //User-friendly name for workflow association
       _taskList,
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _wa.Description = "Send PO by email";
      _wa.AllowManual = true;
      _wa.AutoStartCreate = false;
      _wa.AutoStartChange = false;
      // add workflow association data
      _wa.AssociationData = _wfData.Serialize(_wa);
      _web.Lists[_targetList].WorkflowAssociations.Add(_wa);
    }
    private static void NewCreatePOAssociation(WorkflowDescription _dsc, SPWeb _web, SPList _taskList, SPList _historyList)
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[_dsc.WorkflowId];
      // create workflow association
      SPWorkflowAssociation _freightPOLibraryWorkflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        _dsc.Name,
        _taskList,
        _historyList);
      // configure workflow association and add to WorkflowAssociations collection
      _freightPOLibraryWorkflowAssociation.Description = _dsc.Description;
      _freightPOLibraryWorkflowAssociation.AllowManual = true;
      _freightPOLibraryWorkflowAssociation.AutoStartCreate = false;
      _freightPOLibraryWorkflowAssociation.AutoStartChange = false;
      _freightPOLibraryWorkflowAssociation.AssociationData = _dsc.Name;
      _web.Lists[CommonDefinition.ShippingListTitle].WorkflowAssociations.Add(_freightPOLibraryWorkflowAssociation);
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
