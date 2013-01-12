using System;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.DocumentsFactory;
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
        BalanceSheetContentFactory.CreateReport( workflowProperties.Web, workflowProperties.WebUrl, workflowProperties.ItemId );
        CompletedLogToHistory_HistoryDescription = "JSOX report created successfully";
      }
      catch ( Exception ex )
      {
        CompletedLogToHistory_HistoryOutcome = "Report fatal error";
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
