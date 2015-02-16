//<summary>
//  Title   : sealed partial class CreateStatement
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
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement;
using CAS.SmartFactory.CW.WebsiteModel;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Workflow.Activities;

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.CreateStatement
{
  /// <summary>
  /// sealed partial class CreateStatement workflow
  /// </summary>
  public sealed partial class CreateStatement : SequentialWorkflowActivity
  {
    public CreateStatement()
    {
      InitializeComponent();
    }

    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void onCreateDocument_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        string _MasterDocumentName = String.Empty;
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          DisposalRequestLib _Dr = Element.GetAtIndex<DisposalRequestLib>(_entities.DisposalRequestLibrary, workflowProperties.ItemId);
          _MasterDocumentName = _Dr.StatementDocumentNameFileName(_entities);
          StatementContent _sc = CraeteStatement(_entities, _Dr, _MasterDocumentName);
          SPFile _newFile = File.CreateXmlFile<StatementContent>(workflowProperties.Web, _sc, _MasterDocumentName, StatementLib.LibraryTitle, StatementContent.StylesheetNmane);
          StatementLib _StatementLib = Element.GetAtIndex<StatementLib>(_entities.StatementLibrary, _newFile.Item.ID);
          _StatementLib.Archival = true;
          _StatementLib.CWL_Statement2DisposalRequestID = _Dr;
          _entities.SubmitChanges();
        }
        logToHistoryListActivity_HistoryOutcome = "Success";
        logToHistoryListActivity_HistoryDescription = String.Format("Document {0} created successfully", _MasterDocumentName);
      }
      catch (ArgumentNullException _ane)
      {
        logToHistoryListActivity_HistoryOutcome = "Inconsistent data";
        logToHistoryListActivity_HistoryDescription = "Cannot create the document because of error: " + _ane.Message;
      }
      catch (Exception _ex)
      {
        logToHistoryListActivity_HistoryOutcome = "Exception";
        logToHistoryListActivity_HistoryDescription = _ex.Message;
      }
    }
    private StatementContent CraeteStatement(Entities entities, DisposalRequestLib disposalRequestLib, string _MasterDocumentName)
    {
      List<Statement> _SADDocuments = new List<Statement>();
      foreach (CustomsWarehouseDisposal _cwdx in disposalRequestLib.CustomsWarehouseDisposal(entities, false))
      {
        if (_cwdx.SADDocumentNo.IsNullOrEmpty())
          throw new ArgumentNullException("SADDocumentNo", "SAD Document No cannot be empty");
        Statement _newItem = new Statement
        {
          DutyAndVAT = _cwdx.DutyAndVAT.GetValueOrDefault(),
          DutyPerSettledAmount = _cwdx.DutyPerSettledAmount.Value,
          No = -1,
          SADDocumentNo = _cwdx.SADDocumentNo,
          ReferenceNumber = _cwdx.CWL_CWDisposal2ClearanceID.ReferenceNumber,
          VATPerSettledAmount = _cwdx.VATPerSettledAmount.Value,
        };
        _SADDocuments.Add(_newItem);
      }
      _SADDocuments.Sort((Statement x, Statement y) => { return x.SADDocumentNo.CompareTo(y.SADDocumentNo); });
      for (int i = 0; i < _SADDocuments.Count; i++)
        _SADDocuments[i].No = i + 1;
      StatementContent _NewSC = new StatementContent()
      {
        DocumentDate = DateTime.Today,
        CustomsProcedure = disposalRequestLib.ClearenceProcedure.Value.Convert2String(),
        StatementOfDuties = _SADDocuments.ToArray()
      };
      return _NewSC;
    }
    public String logToHistoryListActivity_HistoryDescription = default(System.String);
    public String logToHistoryListActivity_HistoryOutcome = default(System.String);
  }
}
