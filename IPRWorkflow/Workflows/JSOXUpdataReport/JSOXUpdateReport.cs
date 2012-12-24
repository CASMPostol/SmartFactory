using System;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.Workflows.JSOXUpdateReport
{
  public sealed partial class JSOXUpdateReport: SequentialWorkflowActivity
  {
    public JSOXUpdateReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void CreateReport( object sender, EventArgs e )
    {
      try
      {
        BalanceSheetContent _content = null;
        using ( Entities edc = new Entities( workflowProperties.WebUrl ) )
        {
          JSOXLib _list = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, workflowProperties.ItemId );
          _list.UpdateJSOXReport( edc );
          edc.SubmitChanges();
          string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( _list.Identyfikator.Value );
          _content = DocumentsFactory.BalanceSheetContentFactory.CreateRequestContent( _list, _documentName );
        }
        _content.UpdateDocument( workflowProperties.Item.File );
        workflowProperties.List.Update();
        EndLogToHistory_HistoryDescription = "Report updated successfully";
      }
      catch ( Exception ex )
      {
        EndLogToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot create JSOX report sheet because of fata error {0} at {1}";
        EndLogToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
        EndLogToHistory.EventId = SPWorkflowHistoryEventType.WorkflowError;
      }
    }
    /// <summary>
    /// The end log to history outcome
    /// </summary>
    public String EndLogToHistory_HistoryOutcome = "Finisched";
    /// <summary>
    /// The end log to history description
    /// </summary>
    public String EndLogToHistory_HistoryDescription = default( System.String );

    private void onWorkflowActivated_Invoked( object sender, ExternalDataEventArgs e )
    {

    }
  }
}
