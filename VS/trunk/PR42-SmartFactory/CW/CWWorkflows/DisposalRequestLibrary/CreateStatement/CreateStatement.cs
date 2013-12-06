//<summary>
//  Title   : sealed partial class CreateStatement
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
using System.Collections.Generic;
using System.Workflow.Activities;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

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
          _StatementLib.CWL_Statement2DisposalRequestID = _Dr;
          _entities.SubmitChanges();
        }
        logToHistoryListActivity_HistoryOutcome = "Success";
        logToHistoryListActivity_HistoryDescription = String.Format("Document {0} created successfully", _MasterDocumentName);
      }
      catch (Exception _ex)
      {
        logToHistoryListActivity_HistoryOutcome = "Exeption";
        logToHistoryListActivity_HistoryDescription = _ex.Message;
      }
    }
    private StatementContent CraeteStatement(Entities _entities, DisposalRequestLib _Dr, string _MasterDocumentName)
    {
      List<OneSADDeclaration> _SADDocuments = new List<OneSADDeclaration>();
      foreach (CustomsWarehouseDisposal _cwdx in _Dr.CustomsWarehouseDisposal)
      {
        int _ix = 0;
        OneSADDeclaration _newItem = new OneSADDeclaration
        {
          IntroducingSADDate = _cwdx.CWL_CWDisposal2CustomsWarehouseID.CustomsDebtDate.Value.Date,
          IntroducingSADNo = _cwdx.CWL_CWDisposal2CustomsWarehouseID.DocumentNo,
          No = _ix++,
          SADDocumentDate = _cwdx.SADDate.Value.Date,
          SADDocumentNo = _cwdx.SADDocumentNo
        };
        _SADDocuments.Add(_newItem);
      }
      _SADDocuments.Sort( (OneSADDeclaration x, OneSADDeclaration y) => { return x.No.CompareTo(y.No); } );
      StatementContent _NewSC = new StatementContent()
      {
        DocumentDate = DateTime.Today,
        SADDocuments = _SADDocuments.ToArray()
      };
      return _NewSC;
    }
    public String logToHistoryListActivity_HistoryDescription = default(System.String);
    public String logToHistoryListActivity_HistoryOutcome = default(System.String);
  }
}
