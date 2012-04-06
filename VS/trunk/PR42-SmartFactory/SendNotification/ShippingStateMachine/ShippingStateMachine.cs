using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine : SequentialWorkflowActivity
  {
    #region private
    private void ReportException(string _source, Exception ex, EntitiesDataContext EDC)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      Anons _entry = new Anons() { Tytuł = _source, Treść = String.Format(_tmplt, ex.Message), Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0) };
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    private void ReportAlarmsAndEvents(string _mssg, Priority _priority, ServiceType _partner, EntitiesDataContext EDC, ShippingShipping _sh)
    {
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
      try
      {
        using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url) { ObjectTrackingEnabled = false })
        {
          ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
          e.Result = !(_sp.State.HasValue && (_sp.State.Value == State.Completed || _sp.State.Value == State.Canceled));
        }
      }
      catch (Exception ex)
      {
        using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
        {
          ReportException("m_OnWorkflowItemChanged_Invoked", ex, EDC);
        }
      }
    }
    #endregion

    #region OnWorkflowItemChanged
    private void m_OnWorkflowItemChanged_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
      {
        try
        {
          string _msg = "The shipping at current state {0} has been modified by {1} and the schedule wiil be updated.";
          ShippingShipping _sp = Element.GetAtIndex(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.Item.ID);
          m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription = string.Format(_msg, _sp.State, _sp.ZmodyfikowanePrzez);
          ReportAlarmsAndEvents(m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription, Priority.Normal, ServiceType.None, EDC, _sp);
          if (_sp.IsOutbound.GetValueOrDefault(false) && (_sp.State.Value == State.Completed))
            MakeShippingReport(_sp, EDC);
          if (_sp.State.Value == State.Completed || _sp.State.Value == State.Cancelation)
            MakePerformanceReport(_sp, EDC);
        }
        catch (Exception ex)
        {
          ReportException("m_OnWorkflowItemChanged_Invoked", ex, EDC);
        }
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
    private void MakePerformanceReport(ShippingShipping _sp, EntitiesDataContext EDC)
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
            NumberTURejectedBadQuality = 0,
            ReportPeriod = _sp.StartTime.Value.ToMonthString()
          };
          EDC.CarrierPerformanceReport.InsertOnSubmit(_rprt);
          EDC.SubmitChanges();
        }
        _rprt.NumberTUOrdered++;
        _rprt.NumberTUNotDeliveredNotShowingUp += (from _ts in _sp.TimeSlot
                                                   where _ts.Occupied.Value == Occupied.Delayed
                                                   select new { }).Count();
        if (_sp.State.Value == State.Cancelation)
          _rprt.NumberTUNotDeliveredNotShowingUp++;
        else
        {
          if (_sp.TrailerCondition.GetValueOrDefault(TrailerCondition.None) == TrailerCondition._1Unexceptable)
            _rprt.NumberTURejectedBadQuality++;
          var _Start = (from _tsx in _sp.TimeSlot
                        where _tsx.Occupied.Value == Occupied.Occupied0
                        orderby _tsx.StartTime ascending
                        select new { Start = _tsx.StartTime.Value }).FirstOrDefault();
          if (_Start == null)
            throw new ApplicationException("Data is inconsistent - there is no timeslot allocated to the shipping.");
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
        }
        EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        ReportException("MakePerformanceReport", ex, EDC);
      }
    }
    private enum Delay { JustInTime, Delayed, VeryLate }
    private Delay CalculateDelay(TimeSpan _value)
    {
      if (_value < new TimeSpan(0, 15, 0))
        return Delay.JustInTime;
      else if (_value < new TimeSpan(1, 0, 0))
        return Delay.Delayed;
      else
        return Delay.VeryLate;
    }
    private void MakeShippingReport(ShippingShipping _sp, EntitiesDataContext EDC)
    {
      try
      {
        _sp.EuroPalletsQuantity = 0;
        _sp.InduPalletsQuantity = 0;
        _sp.TotalQuantityInKU = 0;
        foreach (LoadDescription _ld in _sp.LoadDescription)
        {
          switch (_ld.PalletType.Value)
          {
            case PalletType.Euro:
              _sp.EuroPalletsQuantity += _ld.NumberOfPallets.GetValueOrDefault(0);
              break;
            case PalletType.Industrial:
              _sp.InduPalletsQuantity += _ld.NumberOfPallets.GetValueOrDefault(0);
              break;
            case PalletType.None:
            case PalletType.Invalid:
            case PalletType.Other:
            default:
              break;
          }
          _sp.TotalQuantityInKU += _ld.GoodsQuantity.GetValueOrDefault(0);
        }
        string _na = "N/A?";
        _sp.ForwarderOceanAir = _sp.Route == null ? _na : _sp.Route.CarrierTitle;
        _sp.EscortCostsCurrency = _sp.SecurityEscort == null ? null : _sp.SecurityEscort.Currency;
        _sp.TotalCostsPerKUCurrency = (from Currency _cu in EDC.Currency
                                       where !String.IsNullOrEmpty(_cu.Tytuł) && _cu.Tytuł.ToUpper().Contains(CommonDefinition.DefaultCurrency)
                                       select _cu).FirstOrDefault();
        //Costs calculation
        if (_sp.Route != null)
        {
          _sp.FreightCostsCurrency = _sp.Route.Currency;
          if (_sp.Route.Currency != null)
            _sp.FreightCost = _sp.Route.TransportCosts * _sp.Route.Currency.ExchangeRate;
          _sp.Commodity = _sp.Route.Commodity.Title();
          _sp.Consignee = _sp.Route.FreightPayer == null ? String.Empty.NotAvailable() : _sp.Route.FreightPayer.Title();
          _sp.DepartureCity = _sp.Route.CityOfDeparture;
          _sp.DeliveryToCountry = _sp.Route.CityName == null ? String.Empty.NotAvailable() : _sp.Route.CityName.CountryName.Title();
        }
        if (_sp.SecurityEscort != null && _sp.SecurityEscort.Currency != null)
          _sp.SecurityEscortCost = _sp.SecurityEscort.SecurityCost * _sp.SecurityEscort.Currency.ExchangeRate;
        double? _totalCost = default(double?);
        double _addCost = 0;
        if (_sp.AdditionalCostsCurrency != null)
          _addCost = (_sp.AdditionalCosts * _sp.AdditionalCostsCurrency.ExchangeRate).GetValueOrDefault(0);
        _totalCost = _sp.FreightCost.GetValueOrDefault(0) + _sp.SecurityEscortCost.GetValueOrDefault(0) + _addCost;
        _sp.TotalCostsPerKU = _sp.TotalQuantityInKU.HasValue && _sp.TotalQuantityInKU.Value > 1 ? _totalCost / _sp.TotalQuantityInKU.Value : new Nullable<double>();
        _sp.ReportPeriod = _sp.StartTime.Value.ToMonthString();
        EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        ReportException("MakeShippingReport", ex, EDC);
      }
    }
    public Hashtable m_OnWorkflowItemChanged_BeforeProperties = new System.Collections.Hashtable();
    public Hashtable m_OnWorkflowItemChanged_AfterProperties = new System.Collections.Hashtable();
    public String m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription = default(System.String);
    #endregion

    #region CalculateTimeoutCode
    private void m_CalculateTimeoutCode_ExecuteCode(object sender, EventArgs e)
    {
      using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
      {
        try
        {
          ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
          Shipping.RequiredOperations _ro = 0;
          switch (_sp.State.Value)
          {
            case State.Confirmed:
              TimeSpan _timeDistance;
              switch (_sp.CalculateDistance(out _timeDistance))
              {
                case Shipping.Distance.UpTo72h:
                case Shipping.Distance.UpTo24h:
                case Shipping.Distance.UpTo2h:
                case Shipping.Distance.VeryClose:
                  SetupTimeOut(_timeDistance, _sp);
                  break;
                case Shipping.Distance.Late:
                  MakeDelayed(_sp, EDC);
                  break;
              }
              break;
            case State.WaitingForCarrierData:
            case State.WaitingForSecurityData:
            case State.Creation:
              switch (_sp.CalculateDistance(out _timeDistance))
              {
                case Shipping.Distance.UpTo72h:
                  _ro = _sp.CalculateOperations2Do(false, true);
                  RequestData(_sp, _timeDistance, _ro, Priority.Normal, EDC);
                  break;
                case Shipping.Distance.UpTo24h:
                  _ro = _sp.CalculateOperations2Do(false, true);
                  RequestData(_sp, _timeDistance, _ro, Priority.Warning, EDC);
                  break;
                case Shipping.Distance.UpTo2h:
                  _ro = _sp.CalculateOperations2Do(false, true);
                  RequestData(_sp, _timeDistance, _ro, Priority.Warning, EDC);
                  break;
                case Shipping.Distance.VeryClose:
                  _ro = _sp.CalculateOperations2Do(true, true);
                  RequestData(_sp, _timeDistance, _ro, Priority.High, EDC);
                  break;
                case Shipping.Distance.Late:
                  MakeDelayed(_sp, EDC);
                  break;
              }
              break;
            case State.Cancelation:
              MakeCanceled(_sp, EDC);
              break;
            case State.Underway:
            default:
              SetupTimeOut(TimeSpan.FromHours(5), _sp);
              break;
          }
        }
        catch (Exception _ex)
        {
          ReportException("m_CalculateTimeoutCode_ExecuteCode", _ex, EDC);
        }
      }
    }
    private void RequestData(ShippingShipping _sp, TimeSpan _delay, Shipping.RequiredOperations _ro, Priority _pr, EntitiesDataContext EDC)
    {
      string _frmt = "Truck, trailer and drivers detailed information must be provided in {0} min up to {1:g}.";
      _frmt = String.Format(_frmt, _delay, DateTime.Now + _delay);
      switch (_pr)
      {
        case Priority.Normal:
          _frmt.Insert(0, "Remainder; ");
          break;
        case Priority.High:
          _frmt.Insert(0, "Warnning !");
          break;
        case Priority.Warning:
          _frmt.Insert(0, "It is last call !!!");
          break;
        case Priority.None:
        case Priority.Invalid:
        default:
          break;
      }
      SupplementData2hVendorTemplate _msg = new SupplementData2hVendorTemplate()
      {
        PartnerTitle = _sp.VendorName.Title(),
        Title = _sp.Title(),
        StartTime = _sp.StartTime.Value,
        Subject = _sp.Tytuł + " data request !!"
      };
      SetupEnvironment(_delay, _frmt, _ro, _sp, _pr, _msg, EDC);
    }
    private void MakeCanceled(ShippingShipping _sp, EntitiesDataContext EDC)
    {
      _sp.State = State.Canceled;
      EDC.SubmitChanges();
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true);
      string _frmt = "Wanning !! The shipping has been cancelled by {0}";
      _frmt = String.Format(_frmt, _sp.ZmodyfikowanePrzez);
      CanceledShippingVendorTemplate _msg = new CanceledShippingVendorTemplate()
      {
        PartnerTitle = _sp.VendorName.Title(),
        StartTime = _sp.StartTime.Value,
        Title = _sp.Title(),
        Subject = _sp.Tytuł + " is canceled !!"
      };
      SetupEnvironment(ShippingShipping.WatchTolerance, _frmt, _ro, _sp, Priority.High, _msg, EDC);
    }
    private void MakeDelayed(ShippingShipping _sp, EntitiesDataContext EDC)
    {
      _sp.State = State.Delayed;
      EDC.SubmitChanges();
      string _frmt = "Wanning !! The truck is late. Call the driver: {0}";
      _frmt = String.Format(_frmt, _sp.VendorName != null ? _sp.VendorName.NumerTelefonuKomórkowego : " ?????");
      _frmt += "The shipping should finisch in {0:g} at {1:g}.";
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true) & Shipping.CarrierOperations;
      DelayedShippingVendorTemplate _msg = new DelayedShippingVendorTemplate()
      {
        PartnerTitle = _sp.VendorName.Title(),
        StartTime = _sp.StartTime.Value,
        Title = _sp.Title(),
        TruckTitle = _sp.TruckCarRegistrationNumber.Title(),
        Subject = _sp.Tytuł + " is delayed !!"
      };
      SetupEnvironment(ShippingShipping.WatchTolerance, _frmt, _ro, _sp, Priority.High, _msg, EDC);
    }
    private void SetupEnvironment(TimeSpan _delay, string _logDescription, Shipping.RequiredOperations _operations, ShippingShipping _sp, Priority _prrty, IEmailGrnerator _msg, EntitiesDataContext EDC)
    {
      SetupTimeOut(_delay, _sp);
      SetupAlarmsEvents(_delay, _logDescription, _operations, _prrty, EDC, _sp);
      SetupEmail(_delay, _operations, _sp, _msg, EDC);
    }
    private void SetupAlarmsEvents(TimeSpan _delay, string _msg, Shipping.RequiredOperations _operations, Priority _prrty, EntitiesDataContext EDC, ShippingShipping _sh)
    {
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Escort))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.SecurityEscortProvider, EDC, _sh);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder, EDC, _sh);
    }
    private void SetupEmail(TimeSpan _delay, Shipping.RequiredOperations _operations, Shipping _sp, IEmailGrnerator _body, EntitiesDataContext EDC)
    {
      Operation2Do = _operations;
      ShepherdRole _ccRole = _sp.IsOutbound.Value ? ShepherdRole.OutboundOwner : ShepherdRole.InboundOwner;
      string _cc = DistributionList.GetEmail(_ccRole, EDC);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Carrier))
      {
        _body.PartnerTitle = _sp.VendorName.Title();
        m_CarrierNotificationSendEmail_To = _sp.VendorName != null ? _sp.VendorName.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
        m_CarrierNotificationSendEmail_Subject1 = _body.Subject;
        m_CarrierNotificationSendEmail_Body = _body.TransformText();
        m_CarrierNotificationSendEmail_CC = _cc;
        m_CarrierNotificationSendEmail_From = _cc;
      }
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Escort))
      {
        _body.PartnerTitle = _sp.SecurityEscortProvider.Title();
        m_EscortSendEmail_To = _sp.SecurityEscortProvider != null ? _sp.SecurityEscortProvider.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
        m_EscortSendEmail_Subject = _body.Subject;
        m_EscortSendEmail_Body = _body.TransformText();
        m_EscortSendEmail_CC = _cc;
        m_EscortSendEmail_From = _cc;
      }
    }
    private void SetupTimeOut(TimeSpan _delay, Shipping _sp)
    {
      m_TimeOutDelay_TimeoutDuration1 = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      string _lm = "New timeout {0} min calculated for the shipping {1} at state {2}, Request: {3}";
      m_CalculateTimeoutLogToHistoryList_HistoryDescription = String.Format(_lm, m_TimeOutDelay_TimeoutDuration1, _sp.Title(), _sp.State.Value, _2do);
    }
    public String m_CalculateTimeoutLogToHistoryList_HistoryDescription = default(System.String);
    #endregion

    #region FaultHandlersActivity
    public String m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void m_FaultHandlerLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
      {
        m_FaultHandlerLogToHistoryListActivity_HistoryOutcome1 = "Error";
        m_FaultHandlerLogToHistoryListActivity_HistoryDescription1 = m_MainFaultHandlerActivity.Fault.Message;
        ReportException("FaultHandlersActivity", m_MainFaultHandlerActivity.Fault, EDC);
      }
    }
    #endregion

    #region TimeOutLogToHistoryList
    private void m_TimeOutLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      m_TimeOutLogToHistoryListActivity_HistoryDescription = String.Format("Timeout expired at {0:g}", DateTime.Now);
    }
    public String m_TimeOutLogToHistoryListActivity_HistoryDescription = default(System.String);
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
