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
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using CAS.SmartFactory.IPR.DocumentsFactory;

namespace CAS.SmartFactory.IPR.Workflows.JSOXReport
{
  public sealed partial class JSOXReport: SequentialWorkflowActivity
  {
    public JSOXReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void CreateReport( object sender, EventArgs e )
    {
      try
      {
        using ( Entities edc = new Entities( workflowProperties.WebUrl ) )
        {
          JSOXLib _list = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, workflowProperties.ItemId );
          _list.UpdateJSOXReport( edc );
          string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( _list.Identyfikator.Value );
          BalanceSheetContent _content = DocumentsFactory.BalanceSheetContentFactory.CreateRequestContent( _list, _list.Identyfikator.Value, _documentName );
          int _id = SPDocumentFactory.Prepare( this.workflowProperties.Web, _content, _documentName );

          edc.SubmitChanges();
        }
      }
      catch ( Exception ex )
      {
        EndLogToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot create JSOX report sheet because of fata error {0} at {1}";
        EndLogToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
      }
    }
    public String EndLogToHistory_HistoryOutcome = default( System.String );
    public String EndLogToHistory_HistoryDescription = default( System.String );
  }
}
