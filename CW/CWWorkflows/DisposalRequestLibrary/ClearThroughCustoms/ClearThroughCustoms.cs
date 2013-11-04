//<summary>
//  Title   : public sealed partial class ClearThroughCustoms
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Workflow.Activities;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.ClearThroughCustoms
{
  public sealed partial class ClearThroughCustoms: SequentialWorkflowActivity
  {
    public ClearThroughCustoms()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void onCreateMessageTemplates( object sender, EventArgs e )
    {
      try
      {
        string _MasterDocumentName = String.Empty;
        using ( Entities _entities = new Entities( workflowProperties.WebUrl ) )
        {
          DisposalRequestLib _Dr = Element.GetAtIndex<DisposalRequestLib>( _entities.DisposalRequestLibrary, workflowProperties.ItemId );
          foreach ( CustomsWarehouseDisposal _cwdx in _Dr.CustomsWarehouseDisposal )
          {
            Clearence _newClearance = Clearence.CreataClearence( _entities, "Customs Warehouse Withdraw", ClearenceProcedure._5171 ); //TODO Allow selection of ClearenceProcedure http://casas:11227/sites/awt/Lists/TaskList/DispForm.aspx?ID=4023
            _cwdx.CWL_CWDisposal2Clearance = _newClearance;
            _MasterDocumentName = _newClearance.SADTemplateDocumentNameFileName( _entities );
            SAD _sad = CraeteSAD( _entities, _cwdx, _MasterDocumentName );
            SPFile _newFile = File.CreateXmlFile<SAD>( workflowProperties.Web, _sad, _MasterDocumentName, SADConsignment.IDPropertyName, SAD.StylesheetNmane );
            SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>( _entities.SADConsignment, _newFile.Item.ID );
            _newClearance.SADConsignmentLibraryIndex = _sadConsignment;
            _entities.SubmitChanges();
          }
        }
        logToHistoryListActivity_HistoryOutcome = "Success";
        logToHistoryListActivity_HistoryDescription = String.Format( "Document {0} created successfully", _MasterDocumentName );
      }
      catch ( Exception _ex )
      {
        logToHistoryListActivity_HistoryOutcome = "Exeption";
        logToHistoryListActivity_HistoryDescription = _ex.Message;
      }
    }
    private static SAD CraeteSAD( Entities entities, CustomsWarehouseDisposal item, string masterDocumentName )
    {
      SADGood _entrySAD = item.CWL_CWDisposal2CustomsWarehouseID.CWL_CW2ClearenceID.Clearence2SadGoodID;
      List<SADZgloszenieTowarDokumentWymagany> _dcsList = new List<SADZgloszenieTowarDokumentWymagany>();
      int _Pos = 1;
      _dcsList.Add( SADZgloszenieTowarDokumentWymagany.Create( _Pos++, Settings.CustomsProcedureCode9DK8, masterDocumentName, String.Empty ) );
      foreach ( SADRequiredDocuments _rdx in _entrySAD.SADRequiredDocuments )
      {
        if ( _rdx.Code == Settings.CustomsProcedureCodeA004 || _rdx.Code == Settings.CustomsProcedureCodeN865 || _rdx.Code == Settings.CustomsProcedureCodeN954 )
          _dcsList.Add( SADZgloszenieTowarDokumentWymagany.Create( _Pos++, _rdx.Code, _rdx.Number, _rdx.Title ) );
      }
      decimal value = item.CWL_CWDisposal2CustomsWarehouseID.Value.ConvertToDecimal();
      string reference = item.CWL_CWDisposal2CustomsWarehouseID.DocumentNo;
      SADZgloszenieTowar[] _good = new SADZgloszenieTowar[] 
      {
        SADZgloszenieTowar.Create( item.CW_SettledNetMass.ConvertToDecimal(), item.CW_RemainingPackage.ConvertToDecimal(), reference, _dcsList.ToArray(), value )
      };
      SADZgloszenieUC customsOffice = null;
      SADZgloszenie _application = SADZgloszenie.Create( _good, customsOffice, 
                                                         Settings.GetParameter( entities, SettingsEntry.RecipientOrganization ), 
                                                         Settings.GetParameter( entities, SettingsEntry.SenderOrganization ) );
      return SAD.Create( Settings.GetParameter( entities, SettingsEntry.OrganizationEmail ), _application );
    }

    public String logToHistoryListActivity_HistoryDescription = default( System.String );
    public String logToHistoryListActivity_HistoryOutcome = default( System.String );
  }
}
