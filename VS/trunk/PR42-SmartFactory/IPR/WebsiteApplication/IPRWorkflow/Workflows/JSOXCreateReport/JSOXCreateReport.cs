//_______________________________________________________________
//  Title   : JSOXCreateReport
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SmartFactory.IPR.DocumentsFactory;
using CAS.SmartFactory.IPR.WebsiteModel;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using System;
using System.Workflow.Activities;

namespace CAS.SmartFactory.IPR.Workflows.JSOXCreateReport
{
  /// <summary>
  /// JSOXCreateReport
  /// </summary>
  public sealed partial class JSOXCreateReport : SequentialWorkflowActivity
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
    public Guid workflowId = default(System.Guid);
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    /// <summary>
    /// The completed log to history_ history outcome
    /// </summary>
    public String CompletedLogToHistory_HistoryOutcome = "Finished";
    /// <summary>
    /// The completed log to history_ history description
    /// </summary>
    public String CompletedLogToHistory_HistoryDescription = default(System.String);

    private void CreateJSOXReport(object sender, EventArgs e)
    {
      try
      {
        TraceEvent("Entering JSOXCreateReport.CreateJSOXReport", 58, TraceSeverity.Monitorable);
        BalanceSheetContentFactory.CreateReport(workflowProperties.Web, workflowProperties.WebUrl, workflowProperties.ItemId, TraceEvent);
        CompletedLogToHistory_HistoryDescription = "JSOX report created successfully";
        TraceEvent("Finished JSOXCreateReport.CreateJSOXReport", 61, TraceSeverity.Monitorable);
      }
      catch (Exception ex)
      {
        CompletedLogToHistory_HistoryOutcome = "Report fatal error";
        string _patt = "Cannot create JSOX report sheet because of fatal error {0} at {1}";
        CompletedLogToHistory_HistoryDescription = String.Format(_patt, ex.Message, ex.StackTrace);
        CompletedLogToHistory.EventId = SPWorkflowHistoryEventType.WorkflowError;
        TraceEvent(CompletedLogToHistory_HistoryDescription, 69, TraceSeverity.High);
      }
    }
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.ReportCreation);
    }

  }
}
