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
    private void ReportAlarmsAndEvents(string _mssg, Priority _priority, ServiceType _partner)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URL))
      {
        ShippingShipping _sh = Element.GetAtIndex<ShippingShipping>(_EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
        Partner _principal = null;
        switch (_partner)
        {
          case ServiceType.Vendor:
          case ServiceType.Forwarder:
          case ServiceType.VendorAndForwarder:
            _principal = _sh.VendorName;
            break;
          case ServiceType.SecurityEscortProvider:
            _principal = _sh.SecurityEscortProvider;
            break;
          case ServiceType.None:
          case ServiceType.Invalid:
          default:
            break;
        }
        Entities.AlarmsAndEvents _ae = new AlarmsAndEvents()
        {
          Details = _mssg,
          Owner = _sh.ZmodyfikowanePrzez,
          Priority = _priority,
          ShippingIndex = _sh,
          VendorName = _principal,
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
      ReportAlarmsAndEvents(m_SendWarningLogToHistoryListActivity_HistoryDescription, Priority.Normal, ServiceType.None);
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
    private enum Distance { UpTo72h, UpTo24h, UpTo2h, VeryClose, Late }
    private Distance CalculateDistance(out TimeSpan _ts, Shipping _sp)
    {
      TimeSpan _2h = new TimeSpan(2, 0, 0);
      TimeSpan _24h = new TimeSpan(24, 0, 0);
      TimeSpan _72h = new TimeSpan(3, 0, 0, 0);
      _ts = TimeSpan.Zero;
      if (_sp.StartTime.Value > DateTime.Now + _72h)
      {
        _ts = _sp.StartTime.Value - DateTime.Now - _24h;
        return Distance.UpTo72h;
      }
      else if (_sp.StartTime.Value > DateTime.Now + _24h)
      {
        _ts = _sp.StartTime.Value - DateTime.Now - _24h;
        return Distance.UpTo24h;
      }
      else if (_sp.StartTime.Value > DateTime.Now + _2h)
      {
        _ts = _sp.StartTime.Value - DateTime.Now - _2h;
        return Distance.UpTo2h;
      }
      else if (_sp.StartTime.Value > DateTime.Now)
      {
        _ts = _sp.StartTime.Value - DateTime.Now;
        return Distance.VeryClose;
      }
      else
        return Distance.Late;
    }
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
              SetupEnvironment(_sp.StartTime.Value - DateTime.Now, "The truck is expected in {0} min up to {1}.", 0, _sp);
            else
              MakeDelayed(_EDC, _sp);
            break;
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
          case State.Creation:
            TimeSpan _timeDistance;
            RequiredOperations _ro = 0;
            Priority _pr = 0;
            _frmt = "Truck, trailer and drivers detailed information must be provided in {0} min up to {1:g}.";
            switch (CalculateDistance(out _timeDistance, _sp))
            {
              case Distance.UpTo72h:
                _ro = CalculateOperations2Do(_sp, false, true);
                _pr = Priority.Normal;
                break;
              case Distance.UpTo24h:
                _frmt.Insert(0, "Remainder; ");
                _ro = CalculateOperations2Do(_sp, false, true);
                _pr = Priority.Warning;
                break;
              case Distance.UpTo2h:
                _frmt.Insert(0, "Warnning !");
                _ro = CalculateOperations2Do(_sp, true, true);
                _pr = Priority.Warning;
                break;
              case Distance.VeryClose:
                _frmt.Insert(0, "It is last call !!!");
                _ro = CalculateOperations2Do(_sp, true, true);
                _pr = Priority.High;
                break;
              case Distance.Late:
                MakeDelayed(_EDC, _sp);
                break;
            }
            SetupEnvironment(_timeDistance, _frmt, _ro, _sp, _pr);
            break;
          case State.Delayed:
            break;
          case State.Underway:
            if (_sp.EndTime.Value < DateTime.Now)
            {
              _frmt = "Truck must exit in {0:g} until {1:g}";
              SetupEnvironment(_sp.EndTime.Value - DateTime.Now, _frmt, 0, _sp);
            }
            break;
          default:
            {
              _frmt = "Unsupported state {0} in CalculateTimeoutCode encountered";
              throw new ApplicationException(String.Format(_frmt, _sp.State.Value));
            }
        }
      }
    }
    private static RequiredOperations CalculateOperations2Do(Shipping _sp, bool _email, bool _alarm)
    {
      RequiredOperations _ret = 0;
      RequiredOperations _cr = 0;
      RequiredOperations _escrt = 0;
      if (_alarm)
      {
        if (_sp.VendorName != null)
          _cr = RequiredOperations.AddAlarm2Carrier;
        if (_sp.SecurityEscortProvider != null)
          _escrt = RequiredOperations.AddAlarm2Escort;
      }
      if (_email)
      {
        if (_sp.VendorName != null)
          _cr |= RequiredOperations.SendEmail2Carrier;
        if (_sp.SecurityEscortProvider != null)
          _escrt |= RequiredOperations.SendEmail2Escort;
      }
      switch (_sp.State.Value)
      {
        case State.Confirmed:
        case State.Creation:
        case State.Delayed:
        case State.Underway:
          _ret = _cr | _escrt;
          break;
        case State.WaitingForCarrierData:
          _ret = _cr;
          break;
        case State.WaitingForSecurityData:
          _ret = _escrt;
          break;
        default:
          break;
      }
      return _ret;
    }
    private void MakeDelayed(EntitiesDataContext _EDC, ShippingShipping _sp)
    {
      _sp.State = State.Delayed;
      _EDC.SubmitChanges();
      SetupEnvironmentDelayed(_sp);
    }
    private void SetupEnvironmentDelayed(Shipping _sp)
    {
      string _frmt = "Wanning !! The truck is late. Call the driver: {0}";
      _frmt = String.Format(_frmt, _sp.VendorName != null ? _sp.VendorName.NumerTelefonuKomórkowego : " ?????");
      _frmt += "The shipping is should finisch in {0:g} at {1:g}.";
      RequiredOperations _ro = CalculateOperations2Do(_sp, true, true);
      SetupEnvironment(_sp.EndTime.Value - DateTime.Now, _frmt, _ro, _sp, Priority.High);
    }
    private void SetupEnvironment(TimeSpan _delay, string _logDescription, RequiredOperations _operations, Shipping _sp, Priority _prrty)
    {
      string _msg = String.Format(_logDescription, _delay, DateTime.Now + _delay);
      if (this.InSet(_operations, RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder);
      if (this.InSet(_operations, RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder);
      SetupEnvironment(_delay, _msg, _operations, _sp);
    }
    private void SetupEnvironment(TimeSpan _delay, string _msg, RequiredOperations _operations, Shipping _sp)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      string _lm = "New time out {0} min calculated for the shipping {1} at state {2}";
      m_CalculateTimeoutLogToHistoryList_HistoryDescription = String.Format(_lm, m_TimeOutDelay_TimeoutDuration1, _sp.Title(), _sp.State.Value);
      if (InSet(_operations, RequiredOperations.SendEmail2Carrier))
      {
        try
        {
          m_CarrierNotificationSendEmail_Subject1 = _sp.Tytuł + " Delayed !!";
          m_CarrierNotificationSendEmail_Body = "Warning";
          m_CarrierNotificationSendEmail_To = _sp.VendorName != null ? _sp.VendorName.EMail : "unknown@comapny.com";
          m_CarrierNotificationSendEmail_CC = m_OnWorkflowActivated_WorkflowProperties.OriginatorEmail;
        }
        catch (Exception _ex)
        {
          ReportException("NotificationSendEmail", _ex);
        }
      }
      if (InSet(_operations, RequiredOperations.SendEmail2Escort))
      {

      }
    }
    public String m_CalculateTimeoutLogToHistoryList_HistoryDescription = default(System.String);
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
    private void m_TimeOutLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_TimeOutLogToHistoryListActivity_HistoryOutcome1 = "Timeout";
      m_TimeOutLogToHistoryListActivity_HistoryDescription1 = String.Format("Timeout expired at {0:g}", DateTime.Now);
    }
    public String m_TimeOutLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    public String m_TimeOutLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    #endregion

    #region CarrierNotificationSendEmail
    private void m_CarrierNotificationSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      Operation2Do &= RequiredOperations.SendEmail2Escort;
    }
    private void m_CarrierNotificationSendEmail_Condition(object sender, ConditionalEventArgs e)
    {
      e.Result = (Operation2Do & RequiredOperations.SendEmail2Carrier) != 0;
    }
    public String m_CarrierNotificationSendEmailLogToHistoryList_HistoryDescription = default(System.String);
    public String m_CarrierNotificationSendEmailLogToHistoryList_HistoryOutcome = default(System.String);
    private void m_CarrierNotificationSendEmailLogToHistoryList_MethodInvoking(object sender, EventArgs e)
    {

    }
    public String m_CarrierNotificationSendEmail_Body = default(System.String);
    public String m_CarrierNotificationSendEmail_CC = default(System.String);
    public String m_CarrierNotificationSendEmail_From = default(System.String);
    public String m_CarrierNotificationSendEmail_To = default(System.String);
    public String m_CarrierNotificationSendEmail_Subject1 = default(System.String);
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
    public String m_EscortSendEmailLogToHistoryList_HistoryDescription1 = default(System.String);
    public String m_EscortSendEmailLogToHistoryList_HistoryOutcome = default(System.String);
    private void m_EscortSendEmailLogToHistoryList_MethodInvoking(object sender, EventArgs e)
    {

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
      AddAlarm2Carrier,
      AddAlarm2Escort
    }
    private RequiredOperations Operation2Do { get; set; }
    private bool InSet(RequiredOperations _set, RequiredOperations _item)
    {
      return (_set & _item) != 0;
    }
    #endregion


  }
}
