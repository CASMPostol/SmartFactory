﻿//<summary>
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
            Clearence _newClearance = Clearence.CreataClearence( _entities, "Customs Warehouse Withdraw", _Dr.ClearenceProcedure.Value );
            _cwdx.CWL_CWDisposal2ClearanceID = _newClearance;
            _MasterDocumentName = _newClearance.SADTemplateDocumentNameFileName( _entities );
            SAD _sad = CraeteSAD( _entities, _cwdx, _MasterDocumentName, _Dr.ClearenceProcedure.Value );
            SPFile _newFile = File.CreateXmlFile<SAD>( workflowProperties.Web, _sad, _MasterDocumentName, SADConsignment.IPRSADConsignmentLibraryTitle, SAD.StylesheetNmane );
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
    private static SAD CraeteSAD( Entities entities, CustomsWarehouseDisposal item, string masterDocumentName, ClearenceProcedure clearenceProcedure )
    {
      SADGood _entrySAD = item.CWL_CWDisposal2CustomsWarehouseID.CWL_CW2ClearenceID.Clearence2SadGoodID;
      List<SADZgloszenieTowarDokumentWymagany> _dcsList = new List<SADZgloszenieTowarDokumentWymagany>();
      int _Pos = 1;
      _dcsList.Add( SADZgloszenieTowarDokumentWymagany.Create( _Pos++, Settings.CustomsProcedureCode9DK8, masterDocumentName, String.Empty ) );
      foreach ( SADRequiredDocuments _rdx in _entrySAD.SADRequiredDocuments )
      {
        if ( Required( _rdx.Code ) )
          _dcsList.Add( SADZgloszenieTowarDokumentWymagany.Create( _Pos++, _rdx.Code, _rdx.Number, _rdx.Title ) );
      }
      decimal _IloscTowaruId = 1;
      SADZgloszenieTowarIloscTowaru[] _IloscTowaruArray = new SADZgloszenieTowarIloscTowaru[]
      {
         SADZgloszenieTowarIloscTowaru.Create(ref _IloscTowaruId, item.CW_SettledNetMass.ConvertToDecimal(), item.CW_SettledGrossMass.ConvertToDecimal() )
      };
      decimal _Value = item.CWL_CWDisposal2CustomsWarehouseID.Value.ConvertToDecimal();
      decimal _SADZgloszenieTowarId = 1;
      string _CWDocumentNo = item.CWL_CWDisposal2CustomsWarehouseID.DocumentNo;
      string _CustomsProcedure = Entities.ToString( clearenceProcedure );
      SADZgloszenieTowar[] _good = new SADZgloszenieTowar[]
      {
        SADZgloszenieTowar.Create
          ( item.GoodsName(entities), item.CW_PackageToClear.ConvertToDecimal(), _CWDocumentNo, _Value, ref _SADZgloszenieTowarId, item.ProductCode, item.ProductCodeTaric,  _CustomsProcedure, _dcsList.ToArray(), 
           _IloscTowaruArray)
      };
      SADZgloszenieUC _CustomsOffice = SADZgloszenieUC.Create( Settings.GetParameter( entities, SettingsEntry.DefaultCustomsOffice ) );
      SADZgloszenie _application = SADZgloszenie.Create( _good, _CustomsOffice,
                                                         Settings.GetParameter( entities, SettingsEntry.RecipientOrganization ),
                                                         Vendor.SenderOrganization( entities ) );
      return SAD.Create( Settings.GetParameter( entities, SettingsEntry.OrganizationEmail ), _application );
    }
    private static bool Required( string Code )
    {
      return Code.Contains( Settings.CustomsProcedureCodeA004 ) || Code.Contains( Settings.CustomsProcedureCodeN865 ) ||
             Code.Contains( Settings.CustomsProcedureCodeN954 ) || Code.Contains( Settings.CustomsProcedureCodeN935 );
    }

    public String logToHistoryListActivity_HistoryDescription = default( System.String );
    public String logToHistoryListActivity_HistoryOutcome = default( System.String );
  }
}
