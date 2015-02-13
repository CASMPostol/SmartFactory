//<summary>
//  Title   : NameJSOXUpdateReport - Sequential Workflow Activity
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

using CAS.SmartFactory.IPR.WebsiteModel;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using System;
using System.Workflow.Activities;

namespace CAS.SmartFactory.IPR.Workflows.JSOXUpdateReport
{
  /// <summary>
  /// JSOXUpdateReport - Sequential Workflow Activity
  /// </summary>
  public sealed partial class JSOXUpdateReport : SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="JSOXUpdateReport"/> class.
    /// </summary>
    public JSOXUpdateReport()
    {
      InitializeComponent();
    }
    /// <summary>
    /// The workflow id
    /// </summary>
    public Guid workflowId = default(System.Guid);
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    /// <summary>
    /// The end log to history outcome
    /// </summary>
    public String EndLogToHistory_HistoryOutcome = "Finished";
    /// <summary>
    /// The end log to history description
    /// </summary>
    public String EndLogToHistory_HistoryDescription = default(System.String);

    private void CreateReport(object sender, EventArgs e)
    {
      try
      {
        TraceEvent("Entering JSOXUpdateReport.CreateReport", 57, TraceSeverity.Monitorable);
        DocumentsFactory.BalanceSheetContentFactory.UpdateReport(workflowProperties.Item, workflowProperties.WebUrl, workflowProperties.ItemId, TraceEvent);
        EndLogToHistory_HistoryDescription = "Report updated successfully";
        TraceEvent("Finishing JSOXUpdateReport.CreateReport", 60, TraceSeverity.Monitorable);
      }
      catch (Exception ex)
      {
        EndLogToHistory_HistoryOutcome = "Report fatal error";
        string _patt = "Cannot create JSOX report sheet because of fatal error {0} at {1}";
        EndLogToHistory_HistoryDescription = String.Format(_patt, ex.Message, ex.StackTrace);
        EndLogToHistory.EventId = SPWorkflowHistoryEventType.WorkflowError;
        TraceEvent(EndLogToHistory_HistoryDescription, 66, TraceSeverity.High);
      }
    }
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.ReportCreation);
    }

  }
}
