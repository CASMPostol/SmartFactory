using System;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.DocumentsFactory;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.Workflows.JSOXCreateReport
{
  /// <summary>
  /// JSOXCreateReport
  /// </summary>
  public sealed partial class JSOXCreateReport: SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="JSOXCreateReport" /> class.
    /// </summary>
    public JSOXCreateReport()
    {
      InitializeComponent();
    }
    /// <summary>
    /// The workflow id
    /// </summary>
    public Guid workflowId = default( System.Guid );
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void CreateJSOXReport( object sender, EventArgs e )
    {
      try
      {
        BalanceSheetContent _content = DocumentsFactory.BalanceSheetContentFactory.CreateEmptyRequestContent();
        string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( workflowProperties.ItemId + 1 );
        int _newItem = SPDocumentFactory.Prepare( this.workflowProperties.Web, _content, _documentName );
        using ( Entities edc = new Entities( workflowProperties.WebUrl ) )
        {
          JSOXLib _old = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, workflowProperties.ItemId );
          JSOXLib _new = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, _newItem );
          _new.UpdateJSOXReport( edc, _old );
          edc.SubmitChanges();
          _content = DocumentsFactory.BalanceSheetContentFactory.CreateRequestContent(_new, _documentName);
        }
        _content.UpdateDocument( workflowProperties.Item.File );
        workflowProperties.List.Update();
        CompletedLogToHistory_HistoryDescription = "Report updated successfully";
      }
      catch ( Exception ex )
      {
        CompletedLogToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot create JSOX report sheet because of fata error {0} at {1}";
        CompletedLogToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
        CompletedLogToHistory.EventId = SPWorkflowHistoryEventType.WorkflowError;
      }
    }
    /// <summary>
    /// The completed log to history_ history outcome
    /// </summary>
    public String CompletedLogToHistory_HistoryOutcome = default( System.String );
    /// <summary>
    /// The completed log to history_ history description
    /// </summary>
    public String CompletedLogToHistory_HistoryDescription = default( System.String );
  }
}
