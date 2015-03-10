//_______________________________________________________________
//  Title   : CloseManyAccounts
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

using CAS.SharePoint;
using CAS.SharePoint.Serialization;
using CAS.SmartFactory.IPR.WebsiteModel;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using System;
using System.ComponentModel;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using IPRLinq = CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Workflows.CloseManyIPRAccounts
{
  /// <summary>
  /// Class CloseManyIPRAccounts. This class cannot be inherited.
  /// </summary>
  public sealed partial class CloseManyIPRAccounts : SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseManyIPRAccounts"/> class.
    /// </summary>
    public CloseManyIPRAccounts()
    {
      InitializeComponent();
    }

    /// <summary>
    /// The workflow identifier
    /// </summary>
    public Guid workflowId = default(System.Guid);
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private int m_LoopCounter = 0;
    private InitializationFormData m_InitializationData = null;

    //Activation
    private void GetParameters(object sender, ExternalDataEventArgs e)
    {
      try
      {
        TraceEvent("Entering CloseManyIPRAccounts.GetParameters: ", 47, TraceSeverity.Verbose);
        m_InitializationData = JsonSerializer.Deserialize<InitializationFormData>(workflowProperties.InitiationData);
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccounts.GetParameters"), 51, TraceSeverity.High);
        string _tmp = "Workflow aborted at GetParameters because of the error: {0}";
        throw new ApplicationException(String.Format(_tmp, _ex.Message));
      }
    }
    //Starting log
    private void OnStartingLogToHistory(object sender, EventArgs e)
    {
      StartingHistoryListActivity_HistoryOutcome = "Starting";
      StartingHistoryListActivity_HistoryDescription = "Starting closing the accounts: " + String.Join(", ", m_InitializationData.AccountsArray.Select<int, string>(x => x.ToString()).ToArray<string>());
      TraceEvent("CloseManyIPRAccounts.OnStartingLogToHistory: " + StartingHistoryListActivity_HistoryDescription, 61, TraceSeverity.Verbose);
    }
    /// <summary>
    /// The starting log to history list activity - history description
    /// </summary>
    public String StartingHistoryListActivity_HistoryDescription = default(System.String);
    /// <summary>
    /// The starting log to history list activity - history outcome
    /// </summary>
    public String StartingHistoryListActivity_HistoryOutcome = default(System.String);

    #region while
    private void WhileActivityCondition(object sender, ConditionalEventArgs e)
    {
      e.Result = m_LoopCounter < m_InitializationData.AccountsArray.Length;
    }
    private void onWhileLogToHistory(object sender, EventArgs e)
    {
      TraceEvent("Entering CloseManyIPRAccounts.onWhileLogToHistory", 74, TraceSeverity.Verbose);
      WhileLogToHistory_HistoryOutcome = "Closing account";
      using (IPRLinq.Entities _entities = new IPRLinq.Entities(workflowProperties.WebUrl))
      {
        IPRLinq.IPR _ipr = IPRLinq.Element.GetAtIndex<IPRLinq.IPR>(_entities.IPR, m_InitializationData.AccountsArray[m_LoopCounter]);
        WhileLogToHistory_HistoryDescription = String.Format("Closing account: {0} - Item: {1}/{2}", _ipr.Title, m_LoopCounter + 1, m_InitializationData.AccountsArray.Length);
        TraceEvent("CloseManyIPRAccounts.onWhileLogToHistory: " + WhileLogToHistory_HistoryDescription, 79, TraceSeverity.Verbose);
      }
    }
    /// <summary>
    /// The while log to history - history outcome
    /// </summary>
    public String WhileLogToHistory_HistoryOutcome = default(System.String);
    /// <summary>
    /// The while log to history - history description
    /// </summary>
    public String WhileLogToHistory_HistoryDescription = default(System.String);
    private void DoCloseManyAccount(object sender, EventArgs e)
    {
      try
      {
        TraceEvent("Entering CloseManyIPRAccounts.DoCloseManyAccount: ", 88, TraceSeverity.Verbose);
        switch (IPRClosing.IPRClosing.CloseAccount(workflowProperties.Web, workflowProperties.WebUrl, m_InitializationData.AccountsArray[m_LoopCounter++]))
        {
          case IPRClosing.IPRClosing.CloseAccountResult.Closed:
            lLgClosingResult_HistoryDescription = "IPR record has been successfully closed.";
            lLgClosingResult_HistoryOutcome = "Finished successfully";
            break;
          case IPRClosing.IPRClosing.CloseAccountResult.AccountBalanceError:
            lLgClosingResult_HistoryDescription = String.Format(IPRClosing.IPRClosing.LogWarningTemplate, "AccountBalance must be equal 0");
            lLgClosingResult_HistoryOutcome = "Closing error";
            break;
          case IPRClosing.IPRClosing.CloseAccountResult.DisposalError:
            lLgClosingResult_HistoryDescription = String.Format(IPRClosing.IPRClosing.LogWarningTemplate, "All disposals must be cleared through customs before closing account.");
            lLgClosingResult_HistoryOutcome = "Closing error";
            break;
        }
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccounts.onApplicationErrorLogToHistory"), 93, TraceSeverity.High);
        throw;
      }
    }
    /// <summary>
    /// The log closing result -  history description
    /// </summary>
    public String lLgClosingResult_HistoryDescription = default(System.String);
    /// <summary>
    /// The log closing result_ history outcome
    /// </summary>
    public String lLgClosingResult_HistoryOutcome = default(System.String);
    #endregion

    //Finished
    private void onFinishedHistoryListActivity(object sender, EventArgs e)
    {
      FinishedHistoryListActivity_HistoryDescription = "Finished";
      FinishedHistoryListActivity_HistoryOutcome = "Finished closing the accounts";
      TraceEvent("Finished CloseManyIPRAccounts.onFinishedHistoryListActivity: ", 104, TraceSeverity.Monitorable);
    }
    /// <summary>
    /// The finished log to history list - history description
    /// </summary>
    public String FinishedHistoryListActivity_HistoryDescription = default(System.String);
    /// <summary>
    /// The finished log to history list - history outcome
    /// </summary>
    public String FinishedHistoryListActivity_HistoryOutcome = default(System.String);

    //Exception handler 
    private void onExceptionHandlerHistoryListActivity(object sender, EventArgs e)
    {
      Exception _ex = GeneralFaultHandlerActivity.Fault;
      ExceptionHandlerHistoryListActivity_HistoryDescription = "Aborted by an exception: " + _ex.Message;
      ExceptionHandlerHistoryListActivity_HistoryOutcome = "Exception";
      TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccounts.onExceptionHandlerHistoryListActivity"), 115, TraceSeverity.High);
    }
    /// <summary>
    /// The exception handler log to history list - history description
    /// </summary>
    public String ExceptionHandlerHistoryListActivity_HistoryDescription = default(System.String);
    /// <summary>
    /// The exception handler log to history list -  history outcome
    /// </summary>
    public String ExceptionHandlerHistoryListActivity_HistoryOutcome = default(System.String);
    //ApplicationError 
    private void onApplicationErrorLogToHistory(object sender, EventArgs e)
    {
      try
      {
        ApplicationErrorLogToHistory_HistoryOutcome = "Application Error";
        ApplicationError _ex = (ApplicationError)ApplicationErrorfaultHandlerActivity.Fault;
        ApplicationErrorLogToHistory_HistoryOutcome = String.Format("ApplicationError {0} at: {1}/{2}.", _ex.Message, _ex.Source, _ex.At);
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccounts.onApplicationErrorLogToHistory"), 127, TraceSeverity.High);
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccounts.onApplicationErrorLogToHistory"), 131, TraceSeverity.High);
      }
    }
    /// <summary>
    /// The application error log to history - history description1
    /// </summary>
    public String ApplicationErrorLogToHistory_HistoryDescription1 = default(System.String);
    /// <summary>
    /// The application error log to history - history outcome
    /// </summary>
    public String ApplicationErrorLogToHistory_HistoryOutcome = default(System.String);
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.CloseManyAccounts);
    }


  }
}
