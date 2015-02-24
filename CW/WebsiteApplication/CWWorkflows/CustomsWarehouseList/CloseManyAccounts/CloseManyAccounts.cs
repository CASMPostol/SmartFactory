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
using CAS.SharePoint.Serialization;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  public sealed partial class CloseManyAccounts : SequentialWorkflowActivity
  {
    public CloseManyAccounts()
    {
      InitializeComponent();
    }

    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    InitializationFormData m_InitializationData = null;
    private void GetParameters(object sender, ExternalDataEventArgs e)
    {
      try
      {
        m_InitializationData = JsonSerializer.Deserialize<InitializationFormData>(workflowProperties.InitiationData);
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in StartLogToHistory because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    private void DoCloseManyAccount(object sender, EventArgs e)
    {
      using (Entities _entities = new Entities(workflowProperties.WebUrl))
      {

      }

    }
    private void OnStartingLogToHistory(object sender, EventArgs e)
    {
      StartingHistoryListActivity_HistoryOutcome = "Starting";
      StartingHistoryListActivity_HistoryDescription = "Starting closing the accounts: " + String.Join(", ", m_InitializationData.AccountsArray.Select<int, string>(x => x.ToString()).ToArray<string>());
    }
    private void onFinishedHistoryListActivity(object sender, EventArgs e)
    {
      StartingHistoryListActivity_HistoryOutcome = "Starting";
      StartingHistoryListActivity_HistoryDescription = "Starting closing the accounts:";
    }
    private void onExceptionHandleristoryListActivity(object sender, EventArgs e)
    {
      ExceptionHandleristoryListActivity_HistoryDescription1 = "Aborted by exception";
      ExceptionHandleristoryListActivity_HistoryOutcome = "Exception";
    }
    public String StartingHistoryListActivity_HistoryDescription = default(System.String);
    public String StartingHistoryListActivity_HistoryOutcome = default(System.String);
    public String FinishedHistoryListActivity_HistoryDescription = default(System.String);
    public String FinishedHistoryListActivity_HistoryOutcome = default(System.String);
    public String ExceptionHandleristoryListActivity_HistoryDescription1 = default(System.String);
    public String ExceptionHandleristoryListActivity_HistoryOutcome = default(System.String);



  }
}
