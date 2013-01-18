using System;
using System.Workflow.Activities;
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
        DocumentsFactory.BalanceSheetContentFactory.UpdateReport( workflowProperties.Item, workflowProperties.WebUrl, workflowProperties.ItemId );
        EndLogToHistory_HistoryDescription = "Report updated successfully";
      }
      catch ( Exception ex )
      {
        EndLogToHistory_HistoryOutcome = "Report fatal error";
        string _patt = "Cannot create JSOX report sheet because of fata error {0} at {1}";
        EndLogToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
        EndLogToHistory.EventId = SPWorkflowHistoryEventType.WorkflowError;
      }
    }
    /// <summary>
    /// The end log to history outcome
    /// </summary>
    public String EndLogToHistory_HistoryOutcome = "Finished";
    /// <summary>
    /// The end log to history description
    /// </summary>
    public String EndLogToHistory_HistoryDescription = default( System.String );
  }
}
