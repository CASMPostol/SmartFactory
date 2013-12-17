//<summary>
//  Title   : partial class CloseAccount : SequentialWorkflowActivity
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.AccountClearance;
using CAS.SharePoint.DocumentsFactory;
using CAS.SharePoint;
using CAS.SmartFactory.CW;
using System.Collections.Generic;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseAccount
{
  /// <summary>
  /// partial class CloseAccount : SequentialWorkflowActivity
  /// </summary>
  public sealed partial class CloseAccount : SequentialWorkflowActivity
  {
    public CloseAccount()
    {
      InitializeComponent();
    }

    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    public String StartingWorkflowLogToHistory_HistoryDescription = default(System.String);
    public String StartingWorkflowLogToHistory_HistoryOutcome = default(System.String);
    public String CompletedLogToHistory_HistoryDescription = default(System.String);
    public String CompletedLogToHistory_HistoryOutcome = default(System.String);
    private void CloseAccountActivity_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          CustomsWarehouse _cw = Element.GetAtIndex<CustomsWarehouse>(_entities.CustomsWarehouse, workflowProperties.ItemId);
          RequestContent _newRequestContent = CreateContent(_cw);
          if (_cw.AccountClosed.GetValueOrDefault(false))
          {
            this.CompletedLogToHistory_HistoryDescription = "Aborted";
            this.CompletedLogToHistory_HistoryOutcome = "The account has been already closed.";
            return;
          }
          if (_cw.AccountBalance.GetValueOrDefault(-1) == 0)
          {
            _cw.AccountClosed = true;
            _entities.SubmitChanges();
          }
          int? _binCardId = default(int?);
          try
          {
            _binCardId = _cw.CWL_CW2BinCardTitle == null ? new Nullable<int>() : _cw.CWL_CW2BinCardTitle.Id.Value;
          }
          catch (Exception)
          {
            _binCardId = new Nullable<int>();
          }
          if (_binCardId.HasValue)
          {
            SPDocumentLibrary _lib = (SPDocumentLibrary)workflowProperties.Web.Lists[BinCardLib.LibraryName];
            SPFile _file = _lib.GetItemByIdSelectedFields(_binCardId.Value).File;
            File.WriteXmlFile<RequestContent>(_file, _newRequestContent, RequestContent.StylesheetNmane);
          }
          else
          {
            string _documentName = Settings.BinCardDocumentName(_entities, workflowProperties.ItemId);
            SPFile _newFile = File.CreateXmlFile<RequestContent>(workflowProperties.Web, _newRequestContent, _documentName, Entities.CustomsWarehouseLibName, RequestContent.StylesheetNmane);
            BinCardLib _BinCardLibRntry = Element.GetAtIndex<BinCardLib>(_entities.BinCardLibrary, _newFile.Item.ID);
            _cw.CWL_CW2BinCardTitle = _BinCardLibRntry;
            _entities.SubmitChanges();
          }
        }
        this.CompletedLogToHistory_HistoryDescription = "Success";
        this.CompletedLogToHistory_HistoryOutcome = "Document created successfully";
      }
      catch (CAS.SharePoint.ApplicationError _ap)
      {
        CompletedLogToHistory_HistoryOutcome = "ApplicationError";
        CompletedLogToHistory_HistoryDescription = _ap.Message;
      }
      catch (Exception _ex)
      {
        CompletedLogToHistory_HistoryOutcome = "Exeption";
        CompletedLogToHistory_HistoryDescription = _ex.Message;
      }
    }

    private RequestContent CreateContent(CustomsWarehouse _cw)
    {
      List<ArrayOfDIsposalsDisposalsArray> _listOfDisposals = new List<ArrayOfDIsposalsDisposalsArray>();
      RequestContent _new = new RequestContent()
      {
        Batch = _cw.Batch,
        CNTarrifCode = _cw.CWL_CW2PCNID.ProductCodeNumber,
        ConsentDate = _cw.CWL_CW2ConsentTitle == null ? Extensions.SPMinimum : _cw.CWL_CW2ConsentTitle.ConsentDate.GetValueOrDefault(Extensions.SPMinimum),
        ConsentNo = _cw.CWL_CW2ConsentTitle == null ? String.Empty.NotAvailable() : _cw.CWL_CW2ConsentTitle.Title,
        Currency = _cw.Currency,
        DisposalsColection = _listOfDisposals.ToArray(),
        DocumentDate = DateTime.Today,
        DocumentName = _cw.Title,
        DocumentNo = _cw.Id.Value,
        Grade = _cw.Grade,
        GrossMass = _cw.GrossMass.GetValueOrDefault(-1),
        IntroducingSADDocumentDate = _cw.CustomsDebtDate.GetValueOrDefault(Extensions.SPMinimum),
        IntroducingSADDocumentNo = _cw.DocumentNo,
        InvoiceNo = _cw.InvoiceNo,
        NetMass = _cw.NetMass.GetValueOrDefault(-1).RoundCurrency(),
        PackageUnits = _cw.CW_PackageUnits.GetValueOrDefault(-1).RoundCurrency(),
        PzNo = _cw.CW_PzNo,
        Quantity = _cw.CW_Quantity.GetValueOrDefault(-1).RountMass(),
        SKU = _cw.SKU,
        TobaccoName = _cw.TobaccoName,
        UnitPrice = _cw.CW_UnitPrice.GetValueOrDefault(-1).RoundCurrency(),
        Value = _cw.Value.GetValueOrDefault(-1).RoundCurrency(),
        WithdrawalSADDcoumentNo = "TBD",
        //TODO WithdrawalSADDocumentDate = 
      };
      return _new;
    }
  }
}
