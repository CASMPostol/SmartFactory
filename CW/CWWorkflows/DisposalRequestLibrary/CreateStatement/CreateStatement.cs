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
          CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement.Statement _sad = CraeteStatement(_entities, _cwdx, _MasterDocumentName);
          
          foreach (CustomsWarehouseDisposal _cwdx in _Dr.CustomsWarehouseDisposal)
          {
            SPFile _newFile = File.CreateXmlFile<SAD>(workflowProperties.Web, _sad, _MasterDocumentName, SADConsignment.IPRSADConsignmentLibraryTitle, SAD.StylesheetNmane);
            SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>(_entities.SADConsignment, _newFile.Item.ID);
            _newClearance.SADConsignmentLibraryIndex = _sadConsignment;
            _entities.SubmitChanges();
          }
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

    public String logToHistoryListActivity_HistoryDescription = default(System.String);
    public String logToHistoryListActivity_HistoryOutcome = default(System.String);
  }
}
