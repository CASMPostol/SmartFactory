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
    private void ReportAlarmsAndEvents(string _mssg, Priority _Priority)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
      {
        ShippingShipping _sh = Element.GetAtIndex<ShippingShipping>(_EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
        Entities.AlarmsAndEvents _ae = new AlarmsAndEvents()
        {
          Details = _mssg,
          Owner = _sh.ZmodyfikowanePrzez,
          Priority = _Priority,
          ShippingIndex = _sh,
          VendorName = _sh.VendorName,
          Tytuł = _sh.Title(),
        };
        _EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
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
    internal static Guid WorkflowId = new Guid("cd61e1a0-3401-40f9-9eb1-c7428f6f2516");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "Shipping State Machine", "Shipping State Machine Workflow"); } }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_OnWorkflowActivated_WorkflowProperties = new Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (SPSite _st = m_OnWorkflowActivated_WorkflowProperties.Site)
      {
        m_URL = _st.Url;
      }
      m_DeadlineLogToHistoryListActivity_HistoryOutcome1 = "New dedline";
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
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
      {
        ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(_EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
        e.Result = !(_sp.State.HasValue && (_sp.State.Value == State.Completed || _sp.State.Value == State.Canceled));
      }
    }
    #endregion

    #region WorkflowItemChanged
    private void m_OnWorkflowItemChanged_Invoked(object sender, ExternalDataEventArgs e)
    {
      m_SendWarningLogToHistoryListActivity_HistoryOutcome = "Shipping";
      m_SendWarningLogToHistoryListActivity_HistoryDescription = "The shipping has been modified and the schedule wiil be updated.";
      ReportAlarmsAndEvents(m_SendWarningLogToHistoryListActivity_HistoryDescription, Priority.Normal);
      //if (m_OnWorkflowItemChanged_BeforeProperties1.Count == 0)
      //{
      //  string _msg = "Connot display changes because the BeforeProperties is empty.";
      //  m_SendWarningLogToHistoryListActivity_HistoryDescription = _msg;
      //}
      //else
      //{
      //  m_SendWarningLogToHistoryListActivity_HistoryDescription = "The following coluns have been changed: ";
      //  foreach (var item in m_OnWorkflowItemChanged_AfterProperties1.Keys)
      //  {
      //    try
      //    {
      //      if (m_OnWorkflowItemChanged_AfterProperties1[item] == null)
      //        throw new ApplicationException(String.Format("AfterProperties for key {0} is null.", item));
      //      if (m_OnWorkflowItemChanged_BeforeProperties1[item] == null)
      //        continue;
      //      if (m_OnWorkflowItemChanged_AfterProperties1[item].ToString() != m_OnWorkflowItemChanged_BeforeProperties1[item].ToString())
      //        m_SendWarningLogToHistoryListActivity_HistoryDescription += item.ToString() + ", ";
      //    }
      //    catch (Exception ex)
      //    {
      //      ReportException("m_OnWorkflowItemChanged_Invoked", ex);
      //    }
      //  }
      //}
    }
    public static DependencyProperty m_OnWorkflowItemChanged_BeforeProperties1Property = DependencyProperty.Register("m_OnWorkflowItemChanged_BeforeProperties1", typeof(System.Collections.Hashtable), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public Hashtable m_OnWorkflowItemChanged_BeforeProperties1
    {
      get
      {
        return ((System.Collections.Hashtable)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_OnWorkflowItemChanged_BeforeProperties1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_OnWorkflowItemChanged_BeforeProperties1Property, value);
      }
    }
    public static DependencyProperty m_OnWorkflowItemChanged_AfterProperties1Property = DependencyProperty.Register("m_OnWorkflowItemChanged_AfterProperties1", typeof(System.Collections.Hashtable), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public Hashtable m_OnWorkflowItemChanged_AfterProperties1
    {
      get
      {
        return ((System.Collections.Hashtable)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_OnWorkflowItemChanged_AfterProperties1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_OnWorkflowItemChanged_AfterProperties1Property, value);
      }
    }
    #endregion

    #region m_WhileRoundLogToHistoryListActivity
    public String m_SendWarningLogToHistoryListActivity_HistoryOutcome = default(System.String);
    public String m_SendWarningLogToHistoryListActivity_HistoryDescription = default(System.String);
    #endregion

    #region CalculateTimeoutCode
    private static TimeSpan _5min = new TimeSpan(0, 5, 0);
    private void m_CalculateTimeoutCode_ExecuteCode(object sender, EventArgs e)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
      {
        ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(_EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
        string _frmt = default(string);
        switch (_sp.State.Value)
        {
          case State.Confirmed:
            if (_sp.StartTime.Value > DateTime.Now)
            {
              _frmt = "Wanning: The truck is late. Call the driver: {0}";
              SetupEnvironment(
                _sp.StartTime.Value - DateTime.Now,
                "We expect the truck in {0} up to {1}.",
                String.Format(_frmt, _sp.VendorName != null ? _sp.VendorName.NumerTelefonuKomórkowego : " ?????"),
                "Timeout warning"
                );
            }
            else
            {
              _sp.State = State.Delayed;
              _EDC.SubmitChanges();
              SetupEnvironmentDelayed();
            }
            break;
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
          case State.Creation:
            TimeSpan _2h = new TimeSpan(2, 0, 0);
            TimeSpan _24h = new TimeSpan(24, 0, 0);
            TimeSpan _72h = new TimeSpan(3, 0, 0, 0);
            if (_sp.StartTime.Value > DateTime.Now + _72h)
            {
              _frmt = "Truck, trailer and drivers detailed information must be provided in {0:H:mm} up to {1:g}.";
              SetupEnvironment
                (
                  _sp.StartTime.Value - DateTime.Now - _72h,
                  _frmt,
                  "Time out while waiting on missing data. Please verify shipping data and supplement it.",
                  "Timeout notification"
                );
            }
            else if (_sp.StartTime.Value > DateTime.Now + _24h)
            {
              _frmt = "Truck, trailer and drivers detailed information must be provided in {0:H:mm} up to {1:g}.";
              SetupEnvironment
                (
                  _sp.StartTime.Value - DateTime.Now - _24h,
                  _frmt,
                  "Time out while waiting on missing data. Please verify shipping data and supplement it.",
                  "Timeout notification"
                );
            }
            else if (_sp.StartTime.Value > DateTime.Now + _2h)
            {
              _frmt = "Truck, trailer and drivers detailed information must be provided in {0:H:mm} up to {1:g}.";
              SetupEnvironment
                (
                  _sp.StartTime.Value - DateTime.Now - _2h,
                  _frmt,
                  "Time out while waiting on missing data. Please verify shipping data and supplement it.",
                  "Timeout warning"
                );
            }
            else if (_sp.StartTime.Value > DateTime.Now)
            {
              _frmt = "Missing data warning !! Truck, trailer and drivers detailed information must be provided as soon as posible but not letter than in {0:H:mm} up to {1:g}.";
              SetupEnvironment
                (
                  _2h,
                  _frmt,
                  "Time out waiting on missing data. Please verify shipping data and supplement them",
                  "Timeout warning"
                );
            }
            else
            {
              _sp.State = State.Delayed;
              _EDC.SubmitChanges();
              SetupEnvironmentDelayed();
            }
            break;
          case State.Delayed:
            SetupEnvironmentDelayed();
            break;
          case State.Underway:
            if (_sp.EndTime.Value < DateTime.Now)
              SetupEnvironment(_sp.EndTime.Value - DateTime.Now, "Truck must exit in {0:g} until {1:g}", "Wanning: The truck has not exited until the deadline.", "Timeout notification");
            else
            {
              m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, 30, 0);
              SetupEnvironment
                (
                  new TimeSpan(0, 30, 0),
                  "Warning: the truck is still inside and should have leaved until {0:g}. Next remainder will be issued at: {1:g}. To stop sending remainders change the state of the shipping or manually terminate the warkflow.",
                  "Wanning: The truck has not exited until the deadline.",
                  "Timeout warning"
                 );
            }
            break;
          default:
            {
              _frmt = "Unsupported state {0} in CalculateTimeoutCode encountered";
              throw new ApplicationException(String.Format(_frmt, _sp.State.Value));
            }
        }
        try
        {
          m_NotificationSendEmail_Subject = _sp.Tytuł + " Delayed !!";
          m_NotificationSendEmail_Body = "Warning";
          m_NotificationSendEmail_To1 = _sp.VendorName != null ? _sp.VendorName.EMail : "unknown@comapny.com";
          m_NotificationSendEmail_CC = m_OnWorkflowActivated_WorkflowProperties.OriginatorEmail;
          ReportAlarmsAndEvents(_sp.Tytuł + " Delayed !!", Priority.Warning);
        }
        catch (Exception _ex)
        {
          ReportException("NotificationSendEmail", _ex);
        }
      }
    }
    private void SetupEnvironmentDelayed()
    {
      string _frmt = "The shipping is delayed and the state must be updatede. Next remainder will be issued in {0:g} at {1:g}. To stop sending remainders change the state of the shipping or manually terminate the warkflow.";
      SetupEnvironment
        (
        _5min,
        _frmt,
        "Time out while waiting on shipping state change.",
        "Timeout warning"
        );
    }
    private void SetupEnvironment(TimeSpan _delay, string _logDescription, string _afterDelayDescription, string _afterDelayOutcome)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      m_DeadlineLogToHistoryListActivity_HistoryDescription1 = String.Format(_logDescription, m_TimeOutDelay_TimeoutDuration1, DateTime.Now + _delay);
      m_TimeOutLogToHistoryListActivity_HistoryDescription1 = _afterDelayDescription;
      m_TimeOutLogToHistoryListActivity_HistoryOutcome1 = _afterDelayOutcome;
    }
    #endregion

    #region FaultHandlersActivity
    public String m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void m_FaultHandlerLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = "Error";
      m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = m_MainFaultHandlerActivity.Fault.Message;
      ReportException("FaultHandlersActivity", m_MainFaultHandlerActivity.Fault);
    }
    #endregion

    #region TimeOutLogToHistoryList
    public String m_TimeOutLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    public String m_TimeOutLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    #endregion

    #region NotificationSendEmail

    private void m_CarrierNotificationSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      Operation2Do &= RequiredOperations.SendEmail2Escort;
    }
    private void m_CarrierNotificationSendEmail_Condition(object sender, ConditionalEventArgs e)
    {
      e.Result = (Operation2Do & RequiredOperations.SendEmail2Carrier) != 0;
    }
    public static DependencyProperty m_NotificationSendEmail_To1Property = DependencyProperty.Register("m_NotificationSendEmail_To1", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_NotificationSendEmail_To1
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_To1Property)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_To1Property, value);
      }
    }
    public static DependencyProperty m_NotificationSendEmail_BCCProperty = DependencyProperty.Register("m_NotificationSendEmail_BCC", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_NotificationSendEmail_BCC
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_BCCProperty)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_BCCProperty, value);
      }
    }
    public static DependencyProperty m_NotificationSendEmail_BodyProperty = DependencyProperty.Register("m_NotificationSendEmail_Body", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_NotificationSendEmail_Body
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_BodyProperty)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_BodyProperty, value);
      }
    }
    public static DependencyProperty m_NotificationSendEmail_CCProperty = DependencyProperty.Register("m_NotificationSendEmail_CC", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_NotificationSendEmail_CC
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_CCProperty)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_CCProperty, value);
      }
    }
    public static DependencyProperty m_NotificationSendEmail_SubjectProperty = DependencyProperty.Register("m_NotificationSendEmail_Subject", typeof(System.String), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Misc")]
    public String m_NotificationSendEmail_Subject
    {
      get
      {
        return ((string)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_SubjectProperty)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.m_NotificationSendEmail_SubjectProperty, value);
      }
    }
    #endregion

    #region EscortSend
    private void m_EscortSendEmail_Condition(object sender, ConditionalEventArgs e)
    {
      e.Result = (Operation2Do & RequiredOperations.SendEmail2Escort) != 0;
    }
    private void m_EscortSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      Operation2Do &= RequiredOperations.SendEmail2Escort;
    }
    #endregion

    #region AlarmsEventsAddEntry
    private void m_AlarmsEventsAddEntryCodeActivityCondition(object sender, ConditionalEventArgs e)
    {
      e.Result = (Operation2Do & RequiredOperations.AddAlarm) != 0;
    }
    private void m_AlarmsEventsAddEntryCodeActivity_ExecuteCode(object sender, EventArgs e)
    {
      Operation2Do &= RequiredOperations.AddAlarm;
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

    #region TimeOutDelay
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
    #endregion

    #region private
    [Flags]
    private enum RequiredOperations
    {
      SendEmail2Carrier,
      SendEmail2Escort,
      AddAlarm
    }
    private RequiredOperations Operation2Do { get; set; }
    #endregion
  }
}
