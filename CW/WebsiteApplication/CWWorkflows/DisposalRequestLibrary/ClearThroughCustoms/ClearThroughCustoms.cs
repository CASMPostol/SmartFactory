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

using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using CAS.SmartFactory.CW.WebsiteModel;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Activities;

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.ClearThroughCustoms
{
  /// <summary>
  /// partial class ClearThroughCustoms as <see cref="SequentialWorkflowActivity"/>
  /// </summary>
  public sealed partial class ClearThroughCustoms : SequentialWorkflowActivity
  {
    public ClearThroughCustoms()
    {
      InitializeComponent();
    }

    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void onCreateMessageTemplates(object sender, EventArgs e)
    {
      try
      {
        string _MasterDocumentName = String.Empty;
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          DisposalRequestLib _Dr = Element.GetAtIndex<DisposalRequestLib>(_entities.DisposalRequestLibrary, workflowProperties.ItemId);
          foreach (CustomsWarehouseDisposal _cwdx in _Dr.CustomsWarehouseDisposal(_entities, false))
          {
            if (_cwdx.CustomsStatus.Value != CustomsStatus.NotStarted)
              continue;
            Clearence _newClearance = Clearence.CreataClearence(_entities, "Customs Warehouse Withdraw", _Dr.ClearenceProcedure.Value, (x, y, z) => { });
            _cwdx.CWL_CWDisposal2ClearanceID = _newClearance;
            _cwdx.CustomsStatus = CustomsStatus.Started;
            if (_cwdx.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value > 0 ||
                _cwdx.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal(_entities, false).Where<CustomsWarehouseDisposal>(x => x.CustomsStatus.Value == CustomsStatus.NotStarted).Any<CustomsWarehouseDisposal>())
              _cwdx.ClearingType = ClearingType.PartialWindingUp;
            else
            {
              _cwdx.ClearingType = ClearingType.TotalWindingUp;
              _cwdx.TobaccoValue += _cwdx.CWL_CWDisposal2CustomsWarehouseID.Value.Value - _cwdx.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal(_entities, false).Sum<CustomsWarehouseDisposal>(x => x.TobaccoValue.Value);
              _cwdx.TobaccoValue = _cwdx.TobaccoValue.Value.RoundValue();
              _cwdx.CW_SettledNetMass += _cwdx.CWL_CWDisposal2CustomsWarehouseID.CW_Quantity.Value - _cwdx.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal(_entities, false).Sum<CustomsWarehouseDisposal>(x => x.CW_SettledNetMass.Value);
              _cwdx.CW_SettledNetMass = _cwdx.CW_SettledNetMass.Value.RoundValue();
              _cwdx.CW_SettledGrossMass += _cwdx.CWL_CWDisposal2CustomsWarehouseID.GrossMass.Value - _cwdx.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal(_entities, false).Sum<CustomsWarehouseDisposal>(x => x.CW_SettledGrossMass.Value);
              _cwdx.CW_SettledGrossMass = _cwdx.CW_SettledGrossMass.Value.RoundValue();
            }
            _MasterDocumentName = _newClearance.SADTemplateDocumentNameFileName(_entities);
            SAD _sad = CraeteSAD(_entities, _cwdx, _MasterDocumentName);
            SPFile _newFile = File.CreateXmlFile<SAD>(workflowProperties.Web, _sad, _MasterDocumentName, SADConsignment.IPRSADConsignmentLibraryTitle, SAD.StylesheetNmane);
            SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>(_entities.SADConsignment, _newFile.Item.ID);
            _sadConsignment.Archival = true;
            _newClearance.SADConsignmentLibraryIndex = _sadConsignment;
            _entities.SubmitChanges();
          }
        }
        logToHistoryListActivity_HistoryOutcome = "Success";
        logToHistoryListActivity_HistoryDescription = String.Format("Document {0} created successfully", _MasterDocumentName);
      }
      catch (Exception _ex)
      {
        logToHistoryListActivity_HistoryOutcome = "Exception";
        logToHistoryListActivity_HistoryDescription = _ex.Message;
      }
    }
    private static SAD CraeteSAD(Entities entities, CustomsWarehouseDisposal disposal, string masterDocumentName)
    {
      SADGood _entrySAD = disposal.CWL_CWDisposal2CustomsWarehouseID.CWL_CW2ClearenceID.Clearence2SadGoodID;
      List<SADZgloszenieTowarDokumentWymagany> _dcsList = new List<SADZgloszenieTowarDokumentWymagany>();
      int _Pos = 1;
      _dcsList.Add(SADZgloszenieTowarDokumentWymagany.Create(_Pos++, Settings.CustomsProcedureCode9DK8, masterDocumentName, String.Empty));
      foreach (SADRequiredDocuments _rdx in _entrySAD.SADRequiredDocuments(entities, false))
      {
        if (Required(_rdx.Code))
          _dcsList.Add(SADZgloszenieTowarDokumentWymagany.Create(_Pos++, _rdx.Code, _rdx.Number, _rdx.Title));
      }
      decimal _IloscTowaruId = 1;
      SADZgloszenieTowarIloscTowaru[] _IloscTowaruArray = new SADZgloszenieTowarIloscTowaru[]
      {
         SADZgloszenieTowarIloscTowaru.Create(ref _IloscTowaruId, disposal.CW_SettledNetMass.ConvertToDecimal(), disposal.CW_SettledGrossMass.ConvertToDecimal() )
      };
      decimal _Value = disposal.TobaccoValue.ConvertToDecimal();
      decimal _SADZgloszenieTowarId = 1;
      string _CWDocumentNo = disposal.CWL_CWDisposal2CustomsWarehouseID.DocumentNo;
      string _CustomsProcedure = String.IsNullOrEmpty(disposal.CustomsProcedure) ? disposal.CWL_CWDisposal2DisposalRequestLibraryID.ClearenceProcedure.Value.Convert2String() : disposal.CustomsProcedure;
      SADZgloszenieTowar[] _good = new SADZgloszenieTowar[]
      {
        SADZgloszenieTowar.Create
          ( disposal.GoodsName(entities), disposal.CW_PackageToClear.ConvertToDecimal(), _CWDocumentNo, _Value, ref _SADZgloszenieTowarId, disposal.ProductCode, 
            disposal.ProductCodeTaric,  _CustomsProcedure, _dcsList.ToArray(), _IloscTowaruArray)
      };
      SADZgloszenieUC _CustomsOffice = SADZgloszenieUC.Create(Settings.GetParameter(entities, SettingsEntry.DefaultCustomsOffice));
      SADZgloszenie _application = SADZgloszenie.Create(_good, _CustomsOffice,
                                                         Settings.GetParameter(entities, SettingsEntry.RecipientOrganization),
                                                         Vendor.SenderOrganization(entities));
      return SAD.Create(Settings.GetParameter(entities, SettingsEntry.OrganizationEmail), _application);
    }
    private static bool Required(string Code)
    {
      return Code.Contains(Settings.CustomsProcedureCodeA004) || Code.Contains(Settings.CustomsProcedureCodeN865) ||
             Code.Contains(Settings.CustomsProcedureCodeN954) || Code.Contains(Settings.CustomsProcedureCodeN935);
    }

    public String logToHistoryListActivity_HistoryDescription = default(System.String);
    public String logToHistoryListActivity_HistoryOutcome = default(System.String);
  }
}
