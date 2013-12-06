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
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement;
using System.Collections.Generic;

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.CreateStatement
{
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
