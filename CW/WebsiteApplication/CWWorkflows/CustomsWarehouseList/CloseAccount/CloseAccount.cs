//<summary>
//  Title   : partial class CloseAccount : SequentialWorkflowActivity
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

using CAS.SharePoint;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.AccountClearance;
using CAS.SmartFactory.CW.WebsiteModel;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Workflow.Activities;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseAccount
{
  /// <summary>
  /// partial class CloseAccount : SequentialWorkflowActivity
  /// </summary>
  public sealed partial class CloseAccount : SequentialWorkflowActivity
  {
    #region public
    public CloseAccount()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    public String StartingWorkflowLogToHistory_HistoryDescription = "Starting";
    public String StartingWorkflowLogToHistory_HistoryOutcome = "Closing account started.";
    public String CompletedLogToHistory_HistoryDescription = "Document created successfully";
    public String CompletedLogToHistory_HistoryOutcome = "Success";
    #endregion

    #region private
    private void CloseAccountActivity_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          CustomsWarehouse _cw = Element.GetAtIndex<CustomsWarehouse>(_entities.CustomsWarehouse, workflowProperties.ItemId);
          RequestContent _newRequestContent = CreateContent(_entities, _cw);
          if (_cw.AccountClosed.GetValueOrDefault(false))
          {
            this.CompletedLogToHistory_HistoryDescription = "Aborted";
            this.CompletedLogToHistory_HistoryOutcome = "The account has been already closed.";
            return;
          }
          if (_cw.AccountBalance.GetValueOrDefault(-1) == 0)
          {
            //TODO check packages and value on last disposal.
            _cw.AccountClosed = true;
            _cw.ClosingDate = DateTime.Today;
            _entities.SubmitChanges();
          }
          int? _requestId = _cw.CWL_CW2CWLibraryID.GetTargetId<CustomsWarehouseLib>();
          if (_requestId.HasValue)
          {
            File.WriteXmlFile<RequestContent>(workflowProperties.Web, _requestId.Value, Entities.CustomsWarehouseLibName, _newRequestContent, RequestContent.StylesheetName);
          }
          else
          {
            string _documentName = Settings.ClearanceRequestFileName(_entities, workflowProperties.ItemId);
            SPFile _newFile = File.CreateXmlFile<RequestContent>(workflowProperties.Web, _newRequestContent, _documentName, Entities.CustomsWarehouseLibName, RequestContent.StylesheetName);
            CustomsWarehouseLib _CustomsWarehouseLibEntry = Element.GetAtIndex<CustomsWarehouseLib>(_entities.CustomsWarehouseLibrary, _newFile.Item.ID);
            _CustomsWarehouseLibEntry.Archival = false;
            _cw.CWL_CW2CWLibraryID = _CustomsWarehouseLibEntry;
            _entities.SubmitChanges();
          }
        }
      }
      catch (CAS.SharePoint.ApplicationError _ap)
      {
        CompletedLogToHistory_HistoryOutcome = "ApplicationError";
        CompletedLogToHistory_HistoryDescription = _ap.Message;
      }
      catch (Exception _ex)
      {
        CompletedLogToHistory_HistoryOutcome = "Exception";
        CompletedLogToHistory_HistoryDescription = _ex.Message;
      }
    }
    private RequestContent CreateContent(Entities edc, CustomsWarehouse customsWarehouse)
    {
      string _WithdrawalSADDcoumentNo = String.Empty;
      DateTime _WithdrawalSADDocumentDate = default(DateTime);
      List<ArrayOfDisposalDisposalsArray> _listOfDisposals = new List<ArrayOfDisposalDisposalsArray>();
      foreach (CustomsWarehouseDisposal _cwdx in customsWarehouse.CustomsWarehouseDisposal(edc, false))
      {
        List<string> _wz = new List<string>();
        if (!_cwdx.CW_Wz1.IsNullOrEmpty())
          _wz.Add(_cwdx.CW_Wz1);
        if (!_cwdx.CW_Wz2.IsNullOrEmpty())
          _wz.Add(_cwdx.CW_Wz2);
        if (!_cwdx.CW_Wz3.IsNullOrEmpty())
          _wz.Add(_cwdx.CW_Wz3);
        ArrayOfDisposalDisposalsArray _newItem = new ArrayOfDisposalDisposalsArray()
        {
          CNTarrifCode = _cwdx.CWL_CWDisposal2PCNTID.ProductCodeNumber,
          Currency = _cwdx.CWL_CWDisposal2CustomsWarehouseID.Currency,
          No = _cwdx.SPNo.GetValueOrDefault().Convert2Int(),
          PackageToClear = _cwdx.CW_PackageToClear.GetValueOrDefault().Convert2Int(),
          RemainingPackage = _cwdx.CW_RemainingPackage.GetValueOrDefault(-1).Convert2Int(),
          RemainingQuantity = _cwdx.RemainingQuantity.GetValueOrDefault(-1).Convert2Int(),
          SADDate = _cwdx.SADDate.GetValueOrNull(),
          SADDocumentNo = _cwdx.SADDocumentNo,
          SettledGrossMass = _cwdx.CW_SettledGrossMass.GetValueOrDefault(),
          SettledNetMass = _cwdx.CW_SettledNetMass.GetValueOrDefault(),
          TobaccoValue = _cwdx.TobaccoValue.GetValueOrDefault(),
          WZ = String.Join(",", _wz.ToArray())
        };
        _listOfDisposals.Add(_newItem);
        if (_cwdx.ClearingType.Value == ClearingType.TotalWindingUp)
        {
          _WithdrawalSADDcoumentNo = _cwdx.SADDocumentNo;
          _WithdrawalSADDocumentDate = _cwdx.SADDate.GetValueOrDefault(Extensions.SPMinimum);
        }
      }
      if (customsWarehouse.AccountBalance.Value > 0)
      {
        _WithdrawalSADDcoumentNo = String.Empty.NotAvailable();
        _WithdrawalSADDocumentDate = Extensions.SPMinimum;
      }
      _listOfDisposals.Sort((x, y) => {
                                        int _yNo = y.No == 0 ? 9999 : y.No;
                                        return x.No.CompareTo(_yNo); 
                                      });
      RequestContent _new = new RequestContent()
      {
        Batch = customsWarehouse.Batch,
        CNTarrifCode = customsWarehouse.CWL_CW2PCNID.ProductCodeNumber,
        ConsentDate = customsWarehouse.CWL_CW2ConsentTitle == null ? Extensions.SPMinimum : customsWarehouse.CWL_CW2ConsentTitle.ConsentDate.GetValueOrNull(),
        ConsentNo = customsWarehouse.CWL_CW2ConsentTitle == null ? String.Empty.NotAvailable() : customsWarehouse.CWL_CW2ConsentTitle.Title,
        Currency = customsWarehouse.Currency,
        DisposalsColection = _listOfDisposals.ToArray(),
        DocumentDate = DateTime.Today,
        DocumentName = customsWarehouse.Title,
        DocumentNo = customsWarehouse.Id.Value,
        Grade = customsWarehouse.Grade,
        GrossMass = customsWarehouse.GrossMass.GetValueOrDefault(-1),
        IntroducingSADDocumentDate = customsWarehouse.CustomsDebtDate.GetValueOrNull(),
        IntroducingSADDocumentNo = customsWarehouse.DocumentNo,
        InvoiceNo = customsWarehouse.InvoiceNo,
        NetMass = customsWarehouse.NetMass.GetValueOrDefault(-1), //TODO Clossing the account
        PackageUnits = customsWarehouse.CW_PackageUnits.GetValueOrDefault(-1),
        PzNo = customsWarehouse.CW_PzNo,
        Quantity = customsWarehouse.CW_Quantity.GetValueOrDefault(-1),
        SKU = customsWarehouse.SKU,
        TobaccoName = customsWarehouse.TobaccoName,
        UnitPrice = customsWarehouse.CW_UnitPrice.GetValueOrDefault(-1),
        Value = customsWarehouse.Value.GetValueOrDefault(-1),
        WithdrawalSADDcoumentNo = _WithdrawalSADDcoumentNo,
        WithdrawalSADDocumentDate = _WithdrawalSADDocumentDate
      };
      return _new;
    }
    #endregion

  }
}
