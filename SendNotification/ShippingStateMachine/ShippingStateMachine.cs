using System;
using System.Collections;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine : SequentialWorkflowActivity
  {
    #region private
    private void ReportException(string _source, Exception ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      Anons _entry = new Anons() { Tytuł = _source, Treść = String.Format(_tmplt, ex.Message), Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0) };
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    private void ReportAlarmsAndEvents(string _mssg, Priority _priority, ServiceType _partner)
    {
      ShippingShipping _sh = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
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
      EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
      EDC.SubmitChanges();
    }
    private EntitiesDataContext p_EDC = null;
    private EntitiesDataContext EDC
    {
      get
      {
        if (p_EDC != null)
          return p_EDC;
        p_EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url);
        return p_EDC;
      }
    }
    protected override void Dispose(bool disposing)
    {
      if (p_EDC != null)
      {
        p_EDC.Dispose();
        p_EDC = null;
      }
      base.Dispose(disposing);
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
    { }
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
      ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
      e.Result = !(_sp.State.HasValue && (_sp.State.Value == State.Completed || _sp.State.Value == State.Canceled));
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
    private void m_CalculateTimeoutCode_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
        string _frmt = default(string);
        TimeSpan _timeDistance;
        Shipping.RequiredOperations _ro = 0;
        switch (_sp.State.Value)
        {
          case State.Confirmed:
            switch (_sp.CalculateDistance(out _timeDistance))
            {
              case Shipping.Distance.UpTo72h:
              case Shipping.Distance.UpTo24h:
              case Shipping.Distance.UpTo2h:
              case Shipping.Distance.VeryClose:
                SetupTimeOut(_timeDistance, _sp);
                break;
              case Shipping.Distance.Late:
                MakeDelayed(_sp);
                break;
            }
            break;
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
          case State.Creation:
            Priority _pr = 0;
            _frmt = "Truck, trailer and drivers detailed information must be provided in {0} min up to {1:g}.";
            switch (_sp.CalculateDistance(out _timeDistance))
            {
              case Shipping.Distance.UpTo72h:
                _ro = _sp.CalculateOperations2Do(false, true);
                _pr = Priority.Normal;
                break;
              case Shipping.Distance.UpTo24h:
                _frmt.Insert(0, "Remainder; ");
                _ro = _sp.CalculateOperations2Do(false, true);
                _pr = Priority.Warning;
                break;
              case Shipping.Distance.UpTo2h:
                _frmt.Insert(0, "Warnning !");
                _ro = _sp.CalculateOperations2Do(true, true);
                _pr = Priority.Warning;
                break;
              case Shipping.Distance.VeryClose:
                _frmt.Insert(0, "It is last call !!!");
                _ro = _sp.CalculateOperations2Do(true, true);
                _pr = Priority.High;
                break;
              case Shipping.Distance.Late:
                MakeDelayed(_sp);
                break;
            }
            SetupEnvironment(_timeDistance, _frmt, _ro, _sp, _pr);
            break;
          case State.Delayed:
            break;
          case State.Underway:
            if (_sp.EndTime.Value < DateTime.Now)
              SetupTimeOut(TimeSpan.Zero, _sp);
            break;
          default:
            {
              _frmt = "Unsupported state {0} in CalculateTimeoutCode encountered";
              throw new ApplicationException(String.Format(_frmt, _sp.State.Value));
            }
        }
      }
      catch (Exception _ex)
      {
        ReportException("SetupEnvironment", _ex);
      }
    }
    private void MakeDelayed(ShippingShipping _sp)
    {
      _sp.State = State.Delayed;
      EDC.SubmitChanges();
      string _frmt = "Wanning !! The truck is late. Call the driver: {0}";
      _frmt = String.Format(_frmt, _sp.VendorName != null ? _sp.VendorName.NumerTelefonuKomórkowego : " ?????");
      _frmt += "The shipping should finisch in {0:g} at {1:g}.";
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true) & Shipping.CarrierOperations;
      SetupEnvironment(_sp.EndTime.Value - DateTime.Now, _frmt, _ro, _sp, Priority.High);
    }
    private void SetupEnvironment(TimeSpan _delay, string _logDescription, Shipping.RequiredOperations _operations, Shipping _sp, Priority _prrty)
    {
      SetupTimeOut(_delay, _sp);
      SetupAlarmsEvents(_delay, _logDescription, _operations, _prrty);
      SetupEmail(_delay, _operations, _sp);
    }
    private void SetupAlarmsEvents(TimeSpan _delay, string _logDescription, Shipping.RequiredOperations _operations, Priority _prrty)
    {
      string _msg = String.Format(_logDescription, _delay, DateTime.Now + _delay);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder);
    }
    private void SetupEmail(TimeSpan _delay, Shipping.RequiredOperations _operations, Shipping _sp)
    {
      ShepherdRole _ccRole = _sp.IsOutbound.Value ? ShepherdRole.OutboundOwner : ShepherdRole.InboundOwner;

      var _ccdl = (from _ccx in EDC.DistributionList
                   where _ccx.ShepherdRole.Value == _ccRole
                   select new { Email = _ccx.EMail }).FirstOrDefault();
      if (_ccdl == null || String.IsNullOrEmpty(_ccdl.Email))
        _ccdl = (from _ccx in EDC.DistributionList
                 where _ccx.ShepherdRole.Value == ShepherdRole.Administrator
                 select new { Email = _ccx.EMail }).FirstOrDefault();
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Carrier))
      {
        m_CarrierNotificationSendEmail_Subject1 = _sp.Tytuł + " Delayed !!";
        m_CarrierNotificationSendEmail_Body = "Warning";
        m_CarrierNotificationSendEmail_To = _sp.VendorName != null ? _sp.VendorName.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
        m_CarrierNotificationSendEmail_CC = _ccdl == null ? CommonDefinition.UnknownEmail : _ccdl.Email.UnknownIfEmpty();
      }
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Escort))
      {
      }
    }
    private void SetupTimeOut(TimeSpan _delay, Shipping _sp)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      string _lm = "New time out {0} min calculated for the shipping {1} at state {2}";
      m_CalculateTimeoutLogToHistoryList_HistoryDescription = String.Format(_lm, m_TimeOutDelay_TimeoutDuration1, _sp.Title(), _sp.State.Value);
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

    #region LogToHistoryList
    private void m_CarrierNotificationSendEmail_Condition(object sender, ConditionalEventArgs e)
    {
      e.Result = (Operation2Do & Shipping.RequiredOperations.SendEmail2Carrier) != 0;
    }
    public String m_CarrierNotificationSendEmailLogToHistoryList_HistoryDescription = default(System.String);
    public String m_CarrierNotificationSendEmailLogToHistoryList_HistoryOutcome = default(System.String);
    #endregion

    #region SendEmail
    private void m_CarrierNotificationSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      Operation2Do ^= Shipping.RequiredOperations.SendEmail2Escort;
    }
    public String m_CarrierNotificationSendEmail_Body = default(System.String);
    public String m_CarrierNotificationSendEmail_CC = default(System.String);
    public String m_CarrierNotificationSendEmail_From = default(System.String);
    public String m_CarrierNotificationSendEmail_To = default(System.String);
    public String m_CarrierNotificationSendEmail_Subject1 = default(System.String);
    #endregion
    #endregion

    #region EscortSendEmail

    #region LogToHistoryList
    private void m_EscortSendEmail_Condition(object sender, ConditionalEventArgs e)
    {
      e.Result = Shipping.InSet(Operation2Do, Shipping.RequiredOperations.SendEmail2Escort);
    }
    public String m_EscortSendEmailLogToHistoryList_HistoryDescription1 = default(System.String);
    public String m_EscortSendEmailLogToHistoryList_HistoryOutcome = default(System.String);
    #endregion

    #region SendEmail
    private void m_EscortSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      Operation2Do ^= Shipping.RequiredOperations.SendEmail2Escort;
    }
    public String m_EscortSendEmail_Body = default(System.String);
    public String m_EscortSendEmail_CC = default(System.String);
    public String m_EscortSendEmail_From = default(System.String);
    public String m_EscortSendEmail_Subject = default(System.String);
    public String m_EscortSendEmail_To = default(System.String);
    #endregion
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
    private Shipping.RequiredOperations Operation2Do { get; set; }
    #endregion


  }
}
