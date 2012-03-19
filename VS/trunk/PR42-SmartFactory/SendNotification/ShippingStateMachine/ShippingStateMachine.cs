using System;
using System.Collections;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine : SequentialWorkflowActivity
  {
    public ShippingStateMachine()
    {
      InitializeComponent();
    }

    #region OnWorkflowActivated
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_OnWorkflowActivated_WorkflowProperties = new Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (SPSite _st = m_OnWorkflowActivated_WorkflowProperties.Site)
      {
        m_URL = _st.Url;
      } 
    }
    private bool m_TimeOutRequired = true;
    #endregion

    #region OnStartedLogToHistoryList
    public Int32 m_OnStartedLogToHistoryListActivity_UserId1 = default(System.Int32);
    private void m_OnStartedLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_OnStartedLogToHistoryListActivity_UserId1 = m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID;
    }
    #endregion

    #region m_IfElseActivity
    private void OnIfEvaluationTimeOutRequired(object sender, ConditionalEventArgs e)
    {
      e.Result = m_TimeOutRequired;
    }
    private void OnIfEvaluationNotTimeOutRequired(object sender, ConditionalEventArgs e)
    {
      e.Result = !m_TimeOutRequired;
    }
    #endregion

    #region WhileActivity
    private void m_MainLoopWhileActivity_ConditionEventHandler(object sender, ConditionalEventArgs e)
    {

      e.Result = true;
    }
    #endregion

    #region WorkflowItemChanged
    public Hashtable m_AfterProperties = default(Hashtable);
    public Hashtable m_BeforeProperties = default(Hashtable);
    private void m_OnWorkflowItemChanged_Invoked(object sender, ExternalDataEventArgs e)
    {
      m_SendWarningLogToHistoryListActivity_HistoryOutcome = "Shiping changed";
      m_SendWarningLogToHistoryListActivity_HistoryDescription = "The following coluns have been changed: ";
      foreach (var item in m_AfterProperties.Keys)
      {
        try
        {
          if (m_AfterProperties[item].ToString() != m_BeforeProperties[item].ToString())
            m_SendWarningLogToHistoryListActivity_HistoryDescription += item.ToString() + ", ";
        }
        catch (Exception)
        {
          m_SendWarningLogToHistoryListActivity_HistoryDescription += item.ToString() + "#, ";
        }
      }
    }
    #endregion

    #region m_WhileRoundLogToHistoryListActivity
    public String m_SendWarningLogToHistoryListActivity_HistoryOutcome = default(System.String);
    public String m_SendWarningLogToHistoryListActivity_HistoryDescription = default(System.String);
    #endregion

    #region TimeOutDelayActivity
    private void m_TimeOutDelayActivity_InitializeTimeoutDuration(object sender, EventArgs e)
    {
      m_SendWarningLogToHistoryListActivity_HistoryDescription = "Time Out Delay Activity";
      m_SendWarningLogToHistoryListActivity_HistoryOutcome = "TimeOut";
      m_TimeOutDelayActivity.TimeoutDuration = new TimeSpan(0, 5, 0);
    }
    #endregion

    private void logToHistoryListActivity1_MethodInvoking(object sender, EventArgs e)
    {
      logToHistoryListActivity1.HistoryDescription =
        m_TimeOutDelayActivity.TimeoutDuration.ToString() + "/" +
        logToHistoryListActivity1.Duration.ToString();
    }
    #region private
    private string m_URL = default(string);
    private void ReportException(string _source, Exception ex)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
      {
        string _tmplt = "The current operation has been interrupted by error {0}.";
        Anons _entry = new Anons() { Tytuł = _source, Treść = String.Format(_tmplt, ex.Message), Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0) };
        _EDC.EventLogList.InsertOnSubmit(_entry);
        _EDC.SubmitChanges();
      }
    }
    #endregion
  }
}
