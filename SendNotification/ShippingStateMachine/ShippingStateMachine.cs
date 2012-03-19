using System;
using System.Collections;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Workflow.ComponentModel;

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine : SequentialWorkflowActivity
  {
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

    #region public
    public ShippingStateMachine()
    {
      InitializeComponent();
    }
    #endregion

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
    #endregion

    #region OnStartedLogToHistoryList
    public Int32 m_OnStartedLogToHistoryListActivity_UserId1 = default(System.Int32);
    private void m_OnStartedLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_OnStartedLogToHistoryListActivity_UserId1 = m_OnWorkflowActivated_WorkflowProperties.OriginatorUser.ID;
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
          if (m_AfterProperties[item] == null)
            throw new ApplicationException("AfterProperties is null.");
          if (m_BeforeProperties[item] == null)
            throw new ApplicationException("BeforeProperties is null.");
          if (m_AfterProperties[item].ToString() != m_BeforeProperties[item].ToString())
            m_SendWarningLogToHistoryListActivity_HistoryDescription += item.ToString() + ", ";
        }
        catch (Exception ex)
        {
          ReportException("m_OnWorkflowItemChanged_Invoked", ex);
        }
      }
    }
    #endregion

    #region m_WhileRoundLogToHistoryListActivity
    public String m_SendWarningLogToHistoryListActivity_HistoryOutcome = default(System.String);
    public String m_SendWarningLogToHistoryListActivity_HistoryDescription = default(System.String);
    #endregion

    #region CalculateTimeoutCode
    private void m_CalculateTimeoutCode_ExecuteCode(object sender, EventArgs e)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, 5, 0);
      m_DeadlineLogToHistoryListActivity_HistoryOutcome1 = "New dedline";
      string _frmt = "Waiting for {0} until: {1}.";
      m_DeadlineLogToHistoryListActivity_HistoryDescription1 = String.Format(_frmt, "Shipping data", DateTime.Now + m_TimeOutDelay_TimeoutDuration1);
    }
    #endregion

    #region FaultHandlersActivity
    public String m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void m_FaultHandlerLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = "Error";
      m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = m_MainFaultHandlerActivity.Fault.Message;
    }
    #endregion

    #region TimeOutLogToHistoryList
    private void m_TimeOutLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        m_TimeOutLogToHistoryListActivity_HistoryDescription1 = m_TimeOutDelay_TimeoutDuration1.ToString() + "/" +
          m_TimeOutLogToHistoryListActivity.Duration.ToString();
      }
      catch (Exception _ex)
      {
        ReportException("TimeOutLogToHistoryListActivity", _ex);
      }
    }
    public String m_TimeOutLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    public String m_TimeOutLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    #endregion

    #region NotificationSendEmail
    private void m_NotificationSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
        {
          Shipping _sp = Element.GetAtIndex<Shipping>(_EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
          m_NotificationSendEmail.Subject = _sp.Tytuł + " Delayed !!";
          m_NotificationSendEmail.WorkflowId = m_OnWorkflowActivated_WorkflowProperties.WorkflowId;
          m_NotificationSendEmail.Body = "Warning";
          m_NotificationSendEmail.To = _sp.VendorName != null ? _sp.VendorName.EMail : "unknown@comapny.com";
          m_NotificationSendEmail.CC = m_OnWorkflowActivated_WorkflowProperties.OriginatorEmail;
        }
      }
      catch (Exception _ex)
      {
        ReportException("NotificationSendEmail", _ex);
      }
    }
    #endregion

    #region DeadlineLogToHistoryList
    public static DependencyProperty m_DeadlineLogToHistoryListActivity_HistoryDescription1Property = DependencyProperty.Register("m_DeadlineLogToHistoryListActivity_HistoryDescription1", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_DeadlineLogToHistoryListActivity_HistoryDescription1
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_DeadlineLogToHistoryListActivity_HistoryDescription1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_DeadlineLogToHistoryListActivity_HistoryDescription1Property, value);
      }
    }
    public static DependencyProperty m_DeadlineLogToHistoryListActivity_HistoryOutcome1Property = DependencyProperty.Register("m_DeadlineLogToHistoryListActivity_HistoryOutcome1", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_DeadlineLogToHistoryListActivity_HistoryOutcome1
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_DeadlineLogToHistoryListActivity_HistoryOutcome1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_DeadlineLogToHistoryListActivity_HistoryOutcome1Property, value);
      }
    }
    #endregion

    public static DependencyProperty m_TimeOutDelay_TimeoutDuration1Property = DependencyProperty.Register("m_TimeOutDelay_TimeoutDuration1", typeof(System.TimeSpan), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));

    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public TimeSpan m_TimeOutDelay_TimeoutDuration1
    {
      get
      {
        return ((System.TimeSpan)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_TimeOutDelay_TimeoutDuration1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_TimeOutDelay_TimeoutDuration1Property, value);
      }
    }

  }
}
