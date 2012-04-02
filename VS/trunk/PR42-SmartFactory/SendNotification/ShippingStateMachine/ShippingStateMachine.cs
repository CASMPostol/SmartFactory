using System;
using System.Collections;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using Microsoft.SharePoint;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using System.Collections.Generic;

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
    private Shipping.RequiredOperations Operation2Do { get; set; }
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

    #region WhileActivity
    private void m_MainLoopWhileActivity_ConditionEventHandler(object sender, ConditionalEventArgs e)
    {
      ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
      e.Result = !(_sp.State.HasValue && (_sp.State.Value == State.Completed || _sp.State.Value == State.Canceled));
    }
    #endregion

    #region OnWorkflowItemChanged
    private void m_OnWorkflowItemChanged_Invoked(object sender, ExternalDataEventArgs e)
    {
      try
      {
        string _msg = "The shipping at CURRENT state {0} has been modified by {1} and the schedule wiil be updated.";
        ShippingShipping _sp = Element.GetAtIndex(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.Item.ID);
        m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription = string.Format(_msg, _sp.State, _sp.ZmodyfikowanePrzez);
        ReportAlarmsAndEvents(m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription, Priority.Normal, ServiceType.None);
        MakeCalculation(_sp);
        MakePerformanceReport(_sp);
      }
      catch (Exception ex)
      {
        ReportException("m_OnWorkflowItemChanged_Invoked", ex);
      }
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
    private void MakePerformanceReport(ShippingShipping _sp)
    {
      try
      {
        DateTime _sDate = _sp.StartTime.Value.Date;
        CarrierPerformanceReport _rprt = (from _rx in _sp.VendorName.CarrierPerformanceReport
                                          where _rx.Date.Value == _sDate
                                          select _rx).FirstOrDefault();
        if (_rprt == null)
        {
          _rprt = new CarrierPerformanceReport()
          {
            Date = _sp.StartTime.Value.Date,
            Carrier = _sp.VendorName,
            Tytuł = _sp.VendorName.Title(),
            NumberTUDelayed = 0,
            NumberTUDelayed1Hour = 0,
            NumberTUNotDeliveredNotShowingUp = 0,
            NumberTUOnTime = 0,
            NumberTUOrdered = 0,
            NumberTURejectedBadQuality = 0
          };
          EDC.CarrierPerformanceReport.InsertOnSubmit(_rprt);
          EDC.SubmitChanges();
          _rprt.NumberTUOrdered++;
          if (_sp.TrailerCondition.Value == TrailerCondition._1Unexceptable)
            _rprt.NumberTURejectedBadQuality++;
          _rprt.NumberTUNotDeliveredNotShowingUp += (from _ts in _sp.TimeSlot
                                                     where _ts.Occupied.Value == Occupied.Delayed
                                                     select new { }).Count();
          var _Start = (from _tsx in _sp.TimeSlot
                        where _tsx.Occupied.Value == Occupied.Free
                        orderby _tsx.StartTime ascending
                        select new { Start = _tsx.StartTime.Value }).First();
          switch (CalculateDelay(_sp.StartTime.Value - _Start.Start))
          {
            case Delay.JustInTime:
              _rprt.NumberTUOnTime++;
              break;
            case Delay.Delayed:
              _rprt.NumberTUDelayed++;
              break;
            case Delay.VeryLate:
              _rprt.NumberTUDelayed1Hour++;
              break;
          }
          EDC.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        ReportException("MakePerformanceReport", ex);
      }
    }
    private enum Delay { JustInTime, Delayed, VeryLate }
    private Delay CalculateDelay(TimeSpan _value)
    {
      if (_value < TimeSpan.Zero || _value < new TimeSpan(0, 15, 0))
        return Delay.JustInTime;
      else if (_value < new TimeSpan(1, 0, 0))
        return Delay.Delayed;
      else
        return Delay.VeryLate;
    }
    private void MakeCalculation(ShippingShipping _sp)
    {
      try
      {
        if (!_sp.IsOutbound.GetValueOrDefault(false) || (_sp.State.Value != State.Completed))
          return;
        foreach (LoadDescription _ld in _sp.LoadDescription)
        {
          switch (_ld.PalletType.Value)
          {
            case PalletType.Euro:
              _sp.EuroPalletsQuantity += _ld.NumberOfPallets.GetValueOrDefault(0);
              break;
            case PalletType.Industrial:
              _sp.InduPalletsQuantity = _ld.NumberOfPallets.GetValueOrDefault(0);
              break;
            case PalletType.None:
            case PalletType.Invalid:
            case PalletType.Other:
            default:
              break;
          }
          _sp.TotalQuantityInKU += _ld.GoodsQuantity.GetValueOrDefault(0);
        }
        _sp.ForwarderOceanAir = _sp.Route.CarrierTitle;
        _sp.EscortCostsCurrency = _sp.SecurityEscort.Currency;
        _sp.TotalCostsPerKUCurrency = (from Currency _cu in EDC.Currency
                                       where String.IsNullOrEmpty(_cu.Tytuł) && _cu.Tytuł.Contains(CommonDefinition.DefaultCurrency)
                                       select _cu).FirstOrDefault();
        //Costs calculation
        if (_sp.Route != null && _sp.Route.Currency != null)
          _sp.FreightCost = _sp.Route.TransportCosts * _sp.Route.Currency.ExchangeRate;
        _sp.SecurityEscortCost = _sp.SecurityEscort.SecurityCost * _sp.SecurityEscort.Currency.ExchangeRate;
        double? _totalCost = default(double?);
        if (_sp.AdditionalCostsCurrency != null)
          _totalCost = _sp.FreightCost + _sp.SecurityEscortCost + _sp.AdditionalCosts * _sp.AdditionalCostsCurrency.ExchangeRate;
        _sp.TotalCostsPerKU = _sp.TotalQuantityInKU.HasValue && _sp.TotalQuantityInKU.Value > 0 ? _totalCost / _sp.TotalQuantityInKU.Value : new Nullable<double>();
        EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        ReportException("MakeCalculation", ex);
      }
    }
    public Hashtable m_OnWorkflowItemChanged_BeforeProperties = new System.Collections.Hashtable();
    public Hashtable m_OnWorkflowItemChanged_AfterProperties = new System.Collections.Hashtable();
    public String m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription = default(System.String);
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
                _ro = _sp.CalculateOperations2Do(false, true);
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
          case State.Underway:
            if (_sp.EndTime.Value < DateTime.Now)
              SetupTimeOut(TimeSpan.Zero, _sp);
            break;
          //TODO add Sanceling http://itrserver/Bugs/BugDetail.aspx?bid=3251
          default:
            SetupTimeOut(TimeSpan.Zero, _sp);
            break;
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
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true) & Shipping.CarrierOperations; //TODO http://itrserver/Bugs/BugDetail.aspx?bid=3251
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
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Escort))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.SecurityEscortProvider);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder);
    }
    private void SetupEmail(TimeSpan _delay, Shipping.RequiredOperations _operations, Shipping _sp)
    {
      ShepherdRole _ccRole = _sp.IsOutbound.Value ? ShepherdRole.OutboundOwner : ShepherdRole.InboundOwner;
      var _ccdl = (from _ccx in EDC.DistributionList
                   where _ccx.ShepherdRole.GetValueOrDefault(ShepherdRole.Invalid) == _ccRole
                   select new { Email = _ccx.EMail }).FirstOrDefault();
      if (_ccdl == null || String.IsNullOrEmpty(_ccdl.Email))
        _ccdl = (from _ccx in EDC.DistributionList
                 where _ccx.ShepherdRole.GetValueOrDefault(ShepherdRole.Invalid) == ShepherdRole.Administrator
                 select new { Email = _ccx.EMail }).FirstOrDefault();
      string _cc = _ccdl == null ? CommonDefinition.UnknownEmail : _ccdl.Email.UnknownIfEmpty();
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Carrier))
      {
        SupplementData2hVendorTemplate _msg = default(SupplementData2hVendorTemplate);
        switch (_sp.State.Value)
        {
          case State.Delayed:
            _msg = new SupplementData2hVendorTemplate(); //Change according to the state http://itrserver/Bugs/BugDetail.aspx?bid=3251
            break;
          case State.Creation:
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
            _msg = new SupplementData2hVendorTemplate();
            break;
          //TODO add Sanceling http://itrserver/Bugs/BugDetail.aspx?bid=3251
          case State.None:
          case State.Invalid:
          case State.Canceled:
          case State.Completed:
          case State.Confirmed:
          case State.Underway:
          default:
            break;
        }
        _msg.PartnerTitle = _sp.VendorName.Title();
        _msg.Title = _sp.Title();
        _msg.StartTime = _sp.StartTime.Value;
        m_CarrierNotificationSendEmail_To = _sp.VendorName != null ? _sp.VendorName.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
        m_CarrierNotificationSendEmail_Subject1 = _sp.Tytuł + " Delayed !!";
        m_CarrierNotificationSendEmail_Body = _msg.TransformText();
        m_CarrierNotificationSendEmail_CC = _cc;
        m_CarrierNotificationSendEmail_From = _cc;
      }
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Escort))
      {
        SupplementData2hEscortTemplate _msg = new SupplementData2hEscortTemplate() //Change according to the state http://itrserver/Bugs/BugDetail.aspx?bid=3251
        {
          ShippingOperationOutband2PartnerTitle = _sp.SecurityEscortProvider.Title(),
          StartTime = _sp.StartTime.Value,
          Title = _sp.Title()
        };
        m_EscortSendEmail_To = _sp.SecurityEscortProvider != null ? _sp.SecurityEscortProvider.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
        m_EscortSendEmail_Subject = _sp.Tytuł + " Delayed !!";
        m_EscortSendEmail_Body = _msg.TransformText();
        m_EscortSendEmail_CC = _cc;
        m_EscortSendEmail_From = _cc;
      }
    }
    private void SetupTimeOut(TimeSpan _delay, Shipping _sp)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      string _lm = "New timeout {0} min calculated for the shipping {1} at state {2}";
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
      e.Result = Shipping.InSet(Operation2Do, Shipping.RequiredOperations.SendEmail2Carrier);
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


  }
}
