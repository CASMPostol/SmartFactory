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
  [Guid( "e10c5c4a-3464-4ade-861e-f91f23955dea" )]
  public class SendNotificationEventReceiver: SPFeatureReceiver
  {
    /// <summary>
    /// Occurs after a Feature is activated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureActivated( SPFeatureReceiverProperties properties )
    {
      string _state = default( string );
      try
      {
        SPSite _siteCollection = (SPSite)properties.Feature.Parent;
        _state = "Feature.Parent";
        //This best practice addresses the issue identified by the SharePoint Dispose Checker Tool as SPDisposeCheckID_140.
        {
          //More Information: http://blogs.msdn.com/rogerla/archive/2008/02/12/sharepoint-2007-and-wss-3-0-dispose-patterns-by-example.aspx#SPDisposeCheckID_140
          SPWeb _web = _siteCollection.RootWeb;
          // obtain referecnes to lists
          SPList _taskList = _web.Lists[ CommonDefinition.SendNotificationWorkflowTasks ];
          SPList _historyList = _web.Lists[ CommonDefinition.SendNotificationWorkflowHistory ];
          _taskList.UseFormsForDisplay = false;
          _state = "Update";
          _taskList.Update();
          _state = "FreightPOLibraryTitle";
          NewSendEmailAssociation( CommonDefinition.FreightPOLibraryTitle, _web, _taskList, _historyList, POLibraryWorkflowAssociationData.FreightPOAssociationData() );
          _state = "EscortPOLibraryTitle";
          NewSendEmailAssociation( CommonDefinition.EscortPOLibraryTitle, _web, _taskList, _historyList, POLibraryWorkflowAssociationData.SecurityPOAssociationData() );
          _state = "CreatePO";
          NewCreatePOAssociation( CreatePO.CreatePO.WorkflowDescription, _web, _taskList, _historyList, false );
          _state = "CreateSecurityPO1";
          NewCreatePOAssociation( CreateSecurityPO1.CreateSecurityPO1.WorkflowDescription, _web, _taskList, _historyList, false );
          _state = "CreateSealProtocol";
          NewCreatePOAssociation( CreateSealProtocol.CreateSealProtocol.WorkflowDescription, _web, _taskList, _historyList, false );
          _state = "ShippingStateMachine";
          NewCreatePOAssociation( ShippingStateMachine.ShippingStateMachine.WorkflowDescription, _web, _taskList, _historyList, true );
          _state = "ScheduleTemplateListTitle";
          NewWorkflowAssociation( CommonDefinition.ScheduleTemplateListTitle, AddTimeSlots.Definitions.WorkflowDescription, _web, _taskList, _historyList );
          _state = "DataImportLibraryTitle";
          NewWorkflowAssociation( CommonDefinition.DataImportLibraryTitle, ImportDictionaries.Definitions.WorkflowDescription, _web, _taskList, _historyList );
        }
      }
      catch ( Exception _ex )
      {
        string _frmt = "ActivationContext failed in the {0} state because of the error {1}";
        throw new ApplicationException( String.Format( _frmt, _state, _ex.Message ) );
      }
    }
    /// <summary>
    /// Occurs when a Feature is deactivated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureDeactivating( SPFeatureReceiverProperties properties )
    {
      string _state = default( string );
      try
      {
        {
          SPSite _site = (SPSite)properties.Feature.Parent;
          {
            //This best practice addresses the issue identified by the SharePoint Dispose Checker Tool as SPDisposeCheckID_140.
            //More Information: http://blogs.msdn.com/rogerla/archive/2008/02/12/sharepoint-2007-and-wss-3-0-dispose-patterns-by-example.aspx#SPDisposeCheckID_140
            SPWeb _web = _site.RootWeb;
            SPList _taskList = _web.Lists[ CommonDefinition.SendNotificationWorkflowTasks ];
            _taskList.Delete();
            SPList _historyList = _web.Lists[ CommonDefinition.SendNotificationWorkflowHistory ];
            _historyList.Delete();
            _web.Update();
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.DataImportLibraryTitle ], ImportDictionaries.Definitions.IDGuid );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.ScheduleTemplateListTitle ], AddTimeSlots.Definitions.IDGuid );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.ShippingListTitle ], ShippingStateMachine.ShippingStateMachine.WorkflowId );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.ShippingListTitle ], CreateSealProtocol.CreateSealProtocol.WorkflowId );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.ShippingListTitle ], CreateSecurityPO1.CreateSecurityPO1.WorkflowId );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.ShippingListTitle ], CreatePO.CreatePO.WorkflowId );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.EscortPOLibraryTitle ], SendEmail.SendEmail.WorkflowId );
            RemoveWorkflowAssociation( _web.Lists[ CommonDefinition.FreightPOLibraryTitle ], SendEmail.SendEmail.WorkflowId );
          }
        }
      }
      catch ( Exception _ex )
      {
        string _frmt = "FeatureDeactivating failed in the {0} state because of the error {1}";
        throw new ApplicationException( String.Format( _frmt, _state, _ex.Message ) );
      }
    }
    private static void NewSendEmailAssociation( string _targetList, SPWeb _web, SPList _taskList, SPList _historyList, POLibraryWorkflowAssociationData _wfData )
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[ SendEmail.SendEmail.WorkflowId ];
      // create workflow association
      SPWorkflowAssociation _wa = SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        _wfData.Name,
       _taskList,
        _historyList );
      // configure workflow association and add to WorkflowAssociations collection
      _wa.Description = "Send PO by email";
      _wa.AllowManual = true;
      _wa.AutoStartCreate = true;
      _wa.AutoStartChange = false;
      // add workflow association data
      _wa.AssociationData = _wfData.Serialize( _wa );
      _web.Lists[ _targetList ].WorkflowAssociations.Add( _wa );
    }
    private static void NewCreatePOAssociation( WorkflowDescription _dsc, SPWeb _web, SPList _taskList, SPList _historyList, bool _autoStartCreate )
    {
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[ _dsc.WorkflowId ];
      // create workflow association
      SPWorkflowAssociation _freightPOLibraryWorkflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        _dsc.Name,
        _taskList,
        _historyList );
      // configure workflow association and add to WorkflowAssociations collection
      _freightPOLibraryWorkflowAssociation.Description = _dsc.Description;
      _freightPOLibraryWorkflowAssociation.AllowManual = true;
      _freightPOLibraryWorkflowAssociation.AutoStartCreate = _autoStartCreate;
      _freightPOLibraryWorkflowAssociation.AutoStartChange = false;
      _freightPOLibraryWorkflowAssociation.AssociationData = _dsc.Name;
      _web.Lists[ CommonDefinition.ShippingListTitle ].WorkflowAssociations.Add( _freightPOLibraryWorkflowAssociation );
    }
    private static void NewWorkflowAssociation( string _targetList, WorkflowDescription _dsc, SPWeb _web, SPList _taskList, SPList _historyList )
    {
      if ( string.IsNullOrEmpty( _targetList ) )
        throw new ApplicationException( "The parameter _targetList of the NewWorkflowAssociation cannot be null or empty" );
      SPWorkflowTemplate _workflowTemplate = _web.WorkflowTemplates[ _dsc.WorkflowId ];
      // create workflow association
      SPWorkflowAssociation _workflowAssociation =
        SPWorkflowAssociation.CreateListAssociation(
        _workflowTemplate,
        _dsc.Name,
        _taskList,
        _historyList );
      // configure workflow association and add to WorkflowAssociations collection
      _workflowAssociation.Description = _dsc.Description;
      _workflowAssociation.AllowManual = true;
      _workflowAssociation.AutoStartCreate = false;
      _workflowAssociation.AutoStartChange = false;
      _workflowAssociation.AssociationData = _dsc.Name;
      _web.Lists[ _targetList ].WorkflowAssociations.Add( _workflowAssociation );
    }
    /// <summary>
    /// Removes the workflow association.
    /// </summary>
    /// <param name="_list">The _product proposals list.</param>
    /// <param name="_workflowTemplateId">The _workflow template id.</param>
    private static void RemoveWorkflowAssociation( SPList _list, Guid _workflowTemplateId )
    {
      SPWorkflowAssociation _wfa = _list.WorkflowAssociations.GetAssociationByBaseID( _workflowTemplateId );
      _list.WorkflowAssociations.Remove( _wfa.Id );
    }
  }
}
