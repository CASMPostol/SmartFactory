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
using CAS.SmartFactory.CW.WebsiteModel;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using System;
using System.Linq;
using System.Workflow.Activities;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  public sealed partial class CloseManyAccounts : SequentialWorkflowActivity
  {

    public CloseManyAccounts()
    {
      InitializeComponent();
    }

    //var
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private int m_LoopCounter = 0;
    private InitializationFormData m_InitializationData = null;

    //Activation
    private void GetParameters(object sender, ExternalDataEventArgs e)
    {
      try
      {
        TraceEvent("Entering CloseManyAccounts.GetParameters: ", 47, TraceSeverity.Verbose);
        m_InitializationData = JsonSerializer.Deserialize<InitializationFormData>(workflowProperties.InitiationData);
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyAccounts.GetParameters"), 51, TraceSeverity.High);
        string _tmp = "Workflow aborted at GetParameters because of the error: {0}";
        throw new ApplicationException(String.Format(_tmp, _ex.Message));
      }
    }
    //Starting log
    private void OnStartingLogToHistory(object sender, EventArgs e)
    {
      StartingHistoryListActivity_HistoryOutcome = "Starting";
      StartingHistoryListActivity_HistoryDescription = "Starting closing the accounts: " + String.Join(", ", m_InitializationData.AccountsArray.Select<int, string>(x => x.ToString()).ToArray<string>());
      TraceEvent("CloseManyAccounts.OnStartingLogToHistory: " + StartingHistoryListActivity_HistoryDescription, 61, TraceSeverity.Verbose);
    }
    public String StartingHistoryListActivity_HistoryDescription = default(System.String);
    public String StartingHistoryListActivity_HistoryOutcome = default(System.String);

    #region while
    private void WhileActivityCondition(object sender, ConditionalEventArgs e)
    {
      e.Result = m_LoopCounter < m_InitializationData.AccountsArray.Length;
      TraceEvent(String.Format("CloseManyAccounts.WhileActivityCondition: Item: {0}/{1}", m_LoopCounter + 1, m_InitializationData.AccountsArray.Length), 70, TraceSeverity.Verbose);
    }
    private void onWhileLogToHistory(object sender, EventArgs e)
    {
      WhileLogToHistory_HistoryOutcome = "Closing account";
      using (Entities _entities = new Entities(workflowProperties.WebUrl))
      {
        CustomsWarehouse _cw = Element.GetAtIndex<CustomsWarehouse>(_entities.CustomsWarehouse, m_InitializationData.AccountsArray[m_LoopCounter]);
        WhileLogToHistory_HistoryDescription = String.Format("Closing account: {0}", _cw.Title);
        TraceEvent("CloseManyAccounts.onWhileLogToHistory: " + WhileLogToHistory_HistoryDescription, 79, TraceSeverity.Verbose);
      }
    }
    public String WhileLogToHistory_HistoryOutcome = default(System.String);
    public String WhileLogToHistory_HistoryDescription = default(System.String);
    private void DoCloseManyAccount(object sender, EventArgs e)
    {
      try
      {
        TraceEvent("Entering CloseManyAccounts.DoCloseManyAccount: ", 88, TraceSeverity.Verbose);
        CloseAccount.CloseAccount.Close(workflowProperties.Web, workflowProperties.WebUrl, m_InitializationData.AccountsArray[m_LoopCounter++]);
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyAccounts.onApplicationErrorLogToHistory"), 93, TraceSeverity.High);
        throw;
      }
    }
    #endregion

    //Finished
    private void onFinishedHistoryListActivity(object sender, EventArgs e)
    {
      FinishedHistoryListActivity_HistoryDescription = "Finished";
      FinishedHistoryListActivity_HistoryOutcome = "Finished closing the accounts";
      TraceEvent("Finished CloseManyAccounts.onFinishedHistoryListActivity: ", 104, TraceSeverity.Monitorable);
    }
    public String FinishedHistoryListActivity_HistoryDescription = default(System.String);
    public String FinishedHistoryListActivity_HistoryOutcome = default(System.String);

    //Exception handler 
    private void onExceptionHandlerHistoryListActivity(object sender, EventArgs e)
    {
      Exception _ex = GeneralFaultHandlerActivity.Fault;
      ExceptionHandlerHistoryListActivity_HistoryDescription = "Aborted by an exception: " + _ex.Message;
      ExceptionHandlerHistoryListActivity_HistoryOutcome = "Exception";
      TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyAccounts.onExceptionHandlerHistoryListActivity"), 115, TraceSeverity.High);
    }
    public String ExceptionHandlerHistoryListActivity_HistoryDescription = default(System.String);
    public String ExceptionHandlerHistoryListActivity_HistoryOutcome = default(System.String);
    //ApplicationError 
    private void onApplicationErrorLogToHistory(object sender, EventArgs e)
    {
      try
      {
        ApplicationErrorLogToHistory_HistoryOutcome = "Application Error";
        ApplicationError _ex = (ApplicationError)ApplicationErrorfaultHandlerActivity.Fault;
        ApplicationErrorLogToHistory_HistoryOutcome = String.Format("ApplicationError {0} at: {1}/{2}.", _ex.Message, _ex.Source, _ex.At);
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyAccounts.onApplicationErrorLogToHistory"), 127, TraceSeverity.High);
      }
      catch (Exception _ex)
      {
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyAccounts.onApplicationErrorLogToHistory"), 131, TraceSeverity.High);
      }
    }
    public String ApplicationErrorLogToHistory_HistoryDescription1 = default(System.String);
    public String ApplicationErrorLogToHistory_HistoryOutcome = default(System.String);
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.CloseAccount);
    }
  }
}
