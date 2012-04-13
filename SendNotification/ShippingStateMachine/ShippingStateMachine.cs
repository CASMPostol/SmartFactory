﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine
{
  public sealed partial class ShippingStateMachine : SequentialWorkflowActivity
  {
    #region private
    private void ReportException(string _source, Exception ex)
    {
      try
      {
        using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
        {
          string _tmplt = "The current operation has been interrupted by error {0}.";
          Anons _entry = new Anons() { Tytuł = _source, Treść = String.Format(_tmplt, ex.Message), Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0) };
          EDC.EventLogList.InsertOnSubmit(_entry);
          EDC.SubmitChanges();
        }
      }
      catch (Exception) { }
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
        //ShippingIndex = _sh,
        VendorName = _principal,
        Tytuł = _sh.Title(),
      };
      EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
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
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "Shipment State Machine", "Shipment State Machine Workflow"); } }
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
        ReportException("m_MainLoopWhileActivity_ConditionEventHandler", ex);
      }
    }
    #endregion

    #region OnWorkflowItemChanged
    private void m_OnWorkflowItemChanged_Invoked(object sender, ExternalDataEventArgs e)
    {
      try
      {
        ActionResult _ar = new ActionResult();
        using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
        {
          string _msg = "The Shipment at current state {0} has been modified by {1} and the schedule wiil be updated.";
          ShippingShipping _sp = Element.GetAtIndex(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.Item.ID);
          m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription = string.Format(_msg, _sp.State, _sp.ZmodyfikowanePrzez);
          //ReportAlarmsAndEvents(m_OnWorkflowItemChangedLogToHistoryList_HistoryDescription, Priority.Normal, ServiceType.None, EDC, _sp);
          if (_sp.IsOutbound.GetValueOrDefault(false) && (_sp.State.Value == State.Completed))
            MakeShippingReport(_sp, EDC, _ar);
          if (_sp.State.Value == State.Completed || _sp.State.Value == State.Cancelation)
            MakePerformanceReport(_sp, EDC, _ar);
          try
          {
            EDC.SubmitChanges(ConflictMode.ContinueOnConflict);
          }
          catch (ChangeConflictException)
          {
            EDC.ResolveChangeConflicts(_ar);
            EDC.SubmitChanges();
          }
        }
        _ar.ReportActionResult(m_OnWorkflowActivated_WorkflowProperties.Site.Url);
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
    private void MakePerformanceReport(ShippingShipping _sp, EntitiesDataContext EDC, ActionResult _result)
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
            throw new ApplicationException("Data is inconsistent - there is no timeslot allocated to the Shipment.");
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
        try
        {
          EDC.SubmitChanges();
        }
        catch (ChangeConflictException)
        {
          EDC.ResolveChangeConflicts(_result);
          EDC.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        _result.AddException("MakePerformanceReport", ex);
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
    private void MakeShippingReport(ShippingShipping _sp, EntitiesDataContext EDC, ActionResult _result)
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
      }
      catch (Exception ex)
      {
        _result.AddException("MakeShippingReport", ex);
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
        this.SendingEmailsReplicator_InitialChildData = null;
        using (EntitiesDataContext EDC = new EntitiesDataContext(m_OnWorkflowActivated_WorkflowProperties.Site.Url))
        {
          ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, m_OnWorkflowActivated_WorkflowProperties.ItemId);
          TimeSpan _timeDistance;
          switch (_sp.State.Value)
          {
            case State.Confirmed:
              switch (_sp.CalculateDistance(out _timeDistance))
              {
                case Shipping.Distance.UpTo72h:
                case Shipping.Distance.UpTo24h:
                case Shipping.Distance.UpTo2h:
                case Shipping.Distance.VeryClose:
                  SetupTimeout(_timeDistance, _sp);
                  break;
                case Shipping.Distance.Late:
                  MakeDelayed(_sp, EDC, m_TimeOutReached);
                  break;
              }
              break;
            case State.WaitingForCarrierData:
            case State.WaitingForSecurityData:
            case State.Creation:
              switch (_sp.CalculateDistance(out _timeDistance))
              {
                case Shipping.Distance.UpTo72h:
                  RequestData(_timeDistance, _sp, Priority.Normal, EDC, m_TimeOutReached);
                  break;
                case Shipping.Distance.UpTo24h:
                  RequestData(_timeDistance, _sp, Priority.Warning, EDC, m_TimeOutReached);
                  break;
                case Shipping.Distance.UpTo2h:
                  RequestData(_timeDistance, _sp, Priority.Warning, EDC, m_TimeOutReached);
                  break;
                case Shipping.Distance.VeryClose:
                  RequestData(_timeDistance, _sp, Priority.High, EDC, m_TimeOutReached);
                  break;
                case Shipping.Distance.Late:
                  MakeDelayed(_sp, EDC, m_TimeOutReached);
                  break;
              }
              break;
            case State.Cancelation:
              MakeCanceled(_sp, EDC);
              break;
            case State.Underway:
            default:
              SetupTimeout(TimeSpan.FromHours(5), _sp);
              break;
          }// switch (_sp.State.Value)
          try
          {
            EDC.SubmitChanges(ConflictMode.ContinueOnConflict);
          }
          catch (ChangeConflictException)
          {
            ActionResult _ar = new ActionResult();
            EDC.ResolveChangeConflicts(_ar);
            EDC.SubmitChanges();
            _ar.ReportActionResult(m_OnWorkflowActivated_WorkflowProperties.Site.Url);
          }
          finally { m_TimeOutReached = false; }
        } //using (EntitiesDataContext EDC
      }
      catch (Exception _ex)
      {
        ReportException("m_CalculateTimeoutCode_ExecuteCode", _ex);
      }
    }
    private bool m_TimeOutReached = false;
    private void RequestData(TimeSpan _delay, ShippingShipping _sp, Priority _pr, EntitiesDataContext EDC, bool _TimeOutExpired)
    {
      string _frmt = "Truck, trailer and drivers detailed information must be provided in {0} min up to {1:g}.";
      _frmt = String.Format(_frmt, _delay, DateTime.Now + _delay);
      Shipping.RequiredOperations _ro = 0;
      switch (_pr)
      {
        case Priority.Normal:
          _ro = _sp.CalculateOperations2Do(false, true, _TimeOutExpired);
          _frmt.Insert(0, "Remainder; ");
          break;
        case Priority.High:
          _ro = _sp.CalculateOperations2Do(true, true, _TimeOutExpired);
          _frmt.Insert(0, "Warnning ! ");
          break;
        case Priority.Warning:
          _ro = _sp.CalculateOperations2Do(false, true, _TimeOutExpired);
          _frmt.Insert(0, "It is last call !!!");
          break;
        case Priority.None:
        case Priority.Invalid:
        default:
          break;
      }
      SetupEnvironment(_delay, _ro, _sp, _pr, EDC, _frmt, EmailType.RequestData);
    }
    private void MakeCanceled(ShippingShipping _sp, EntitiesDataContext EDC)
    {
      _sp.State = State.Canceled;
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true, true);
      string _frmt = "Wanning !! The Shipment has been cancelled by {0}";
      _frmt = String.Format(_frmt, _sp.ZmodyfikowanePrzez);
      SetupEnvironment(ShippingShipping.WatchTolerance, _ro, _sp, Priority.High, EDC, _frmt, EmailType.Canceled);
    }
    private void MakeDelayed(ShippingShipping _sp, EntitiesDataContext EDC, bool _TimeOutExpired)
    {
      _sp.State = State.Delayed;
      string _frmt = "Wanning !! The truck is late. Call the driver: {0}";
      _frmt = String.Format(_frmt, _sp.VendorName != null ? _sp.VendorName.NumerTelefonuKomórkowego : " ?????");
      Shipping.RequiredOperations _ro = _sp.CalculateOperations2Do(true, true, _TimeOutExpired) & Shipping.CarrierOperations;
      SetupEnvironment(ShippingShipping.WatchTolerance, _ro, _sp, Priority.High, EDC, _frmt, EmailType.Delayed);
    }
    private void SetupEnvironment(TimeSpan _delay, Shipping.RequiredOperations _operations, ShippingShipping _sp, Priority _prrty, EntitiesDataContext _EDC, string _logDescription, EmailType _etype)
    {
      SetupAlarmsEvents(_logDescription, _operations, _prrty, _EDC, _sp);
      SetupEmail(_operations, _sp, _etype);
      SetupTimeout(_delay, _sp);
    }
    private void SetupAlarmsEvents(string _msg, Shipping.RequiredOperations _operations, Priority _prrty, EntitiesDataContext EDC, ShippingShipping _sh)
    {
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Escort))
        //TODO the messge must depend on the receiver roele 
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.SecurityEscortProvider, EDC, _sh);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.AddAlarm2Carrier))
        ReportAlarmsAndEvents(_msg, _prrty, ServiceType.VendorAndForwarder, EDC, _sh);
    }
    private void SetupEmail(Shipping.RequiredOperations _operations, Shipping _sp, EmailType _etype)
    {
      List<MailData> _Operarion2Do = new List<MailData>();
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Carrier))
        CreateMailData(_sp, _etype, ExternalRole.Vendor, _Operarion2Do);
      if (Shipping.InSet(_operations, Shipping.RequiredOperations.SendEmail2Escort))
        CreateMailData(_sp, _etype, ExternalRole.Escort, _Operarion2Do);
      this.SendingEmailsReplicator_InitialChildData = _Operarion2Do;
    }
    private void CreateMailData(Shipping _sp, EmailType _etype, ExternalRole _role, List<MailData> _Operarion2Do)
    {
      MailData _ced = new MailData()
      {
        EmailType = _etype,
        Role = _role,
        ShippmentID = _sp.Identyfikator.Value,
        URL = this.m_OnWorkflowActivated_WorkflowProperties.Site.Url
      };
      _Operarion2Do.Add(_ced);
    }
    private void SetupTimeout(TimeSpan _delay, Shipping _sp)
    {
      m_TimeOutDelay_TimeoutDuration = new TimeSpan(0, Convert.ToInt32(_delay.TotalMinutes), 0);
      string _lm = "New timeout at: {0} is set up for the shipment {1} at state {2}";
      m_CalculateTimeoutLogToHistoryList_HistoryDescription = String.Format(_lm, DateTime.Now + m_TimeOutDelay_TimeoutDuration, _sp.Title(), _sp.State.Value);
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
      m_TimeOutLogToHistoryListActivity_HistoryDescription = String.Format("Timeout expired at {0:g}", DateTime.Now);
      m_TimeOutReached = true;
    }
    public String m_TimeOutLogToHistoryListActivity_HistoryDescription = default(System.String);
    #endregion

    #region SendingEmailsReplicator
    private void SendingEmailsReplicator_ChildInitialized(object sender, ReplicatorChildEventArgs e)
    {
      MailData _md = (MailData)e.InstanceData;
      try
      {

        using (EntitiesDataContext EDC = new EntitiesDataContext(_md.URL) { ObjectTrackingEnabled = false })
        {
          ShippingShipping _sp = Element.GetAtIndex<ShippingShipping>(EDC.Shipping, _md.ShippmentID);
          IEmailGrnerator _msg = default(IEmailGrnerator);
          string _cause = default(string);
          switch (_md.EmailType)
          {
            case EmailType.Delayed:
              _msg = new DelayedShippingVendorTemplate()
             {
               TruckTitle = _sp.TruckCarRegistrationNumber.Title(),
             };
              _cause = "Shipment delayed: ";
              break;
            case EmailType.RequestData:
              switch (_md.Role)
              {
                case ExternalRole.Vendor:
                case ExternalRole.Forwarder:
                  _msg = new SupplementData2hVendorTemplate();
                  break;
                case ExternalRole.Escort:
                  _msg = new SupplementData2hEscortTemplate();
                  break;
                default:
                  break;
              }
              _cause = "Data request for shipment: ";
              break;
            case EmailType.Canceled:
              _msg = new CanceledShippingVendorTemplate();
              _cause = "Shipment canceled: ";
              break;
          } //switch (_md.EmailType)
          switch (_md.Role)
          {
            case ExternalRole.Vendor:
            case ExternalRole.Forwarder:
              _msg.PartnerTitle = _sp.VendorName.Title();
              m_CarrierNotificationSendEmail_To = _sp.VendorName != null ? _sp.VendorName.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
              break;
            case ExternalRole.Escort:
              _msg.PartnerTitle = _sp.SecurityEscortProvider.Title();
              m_CarrierNotificationSendEmail_To = _sp.SecurityEscortProvider != null ? _sp.SecurityEscortProvider.EMail.UnknownIfEmpty() : CommonDefinition.UnknownEmail;
              break;
            default:
              break;
          } //switch (_md.Role)
          _msg.ShippingTitle = _sp.Title();
          _msg.StartTime = _sp.StartTime.Value;
          _msg.Subject = _sp.Title().Insert(0, _cause);
          ShepherdRole _ccRole = _sp.IsOutbound.Value ? ShepherdRole.OutboundOwner : ShepherdRole.InboundOwner;
          string _cc = DistributionList.GetEmail(_ccRole, EDC);
          m_CarrierNotificationSendEmail_Subject1 = _sp.Title().Insert(0, _cause);
          m_CarrierNotificationSendEmail_CC = _cc;
          m_CarrierNotificationSendEmail_From = _cc;
          m_CarrierNotificationSendEmail_Body = _msg.TransformText();
        } //using
      }
      catch (Exception ex)
      {
        ReportException("", ex);
      }
    }
    private void SendingEmailsReplicator_ChildCompleted(object sender, ReplicatorChildEventArgs e) { }
    private void SendingEmailsReplicator_Completed(object sender, EventArgs e) { }
    private void SendingEmailsReplicator_Initialized(object sender, EventArgs e) { }

    #region InitialChildData
    public static DependencyProperty SendingEmailsReplicator_InitialChildDataProperty = DependencyProperty.Register("SendingEmailsReplicator_InitialChildData", typeof(System.Collections.IList), typeof(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine));
    [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
    [BrowsableAttribute(true)]
    [CategoryAttribute("Properties")]
    public IList SendingEmailsReplicator_InitialChildData
    {
      get
      {
        return ((System.Collections.IList)(base.GetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.SendingEmailsReplicator_InitialChildDataProperty)));
      }
      set
      {
        base.SetValue(CAS.SmartFactory.Shepherd.SendNotification.ShippingStateMachine.ShippingStateMachine.SendingEmailsReplicator_InitialChildDataProperty, value);
      }
    }
    #endregion

    #region CarrierNotificationSendEmail

    #region LogToHistoryList
    private const string m_CarrierNotificationSendEmailFormat = "Sending {4} warning message To: {0}, CC: {1}, From: {2}, Subject: {3}";
    public String m_CarrierNotificationSendEmailLogToHistoryList_HistoryDescription = default(System.String);
    private void m_CarrierNotificationSendEmailLogToHistoryList_MethodInvoking(object sender, EventArgs e)
    {
      m_CarrierNotificationSendEmailLogToHistoryList_HistoryDescription =
        String.Format(m_CarrierNotificationSendEmailFormat, m_CarrierNotificationSendEmail_To, m_CarrierNotificationSendEmail_CC, m_CarrierNotificationSendEmail_From, m_CarrierNotificationSendEmail_Subject1, "Carrier");
    }
    #endregion

    #region SendEmail
    private void m_CarrierNotificationSendEmail_MethodInvoking(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(m_CarrierNotificationSendEmail_To))
        throw new ApplicationException("Assertion Carrier failed: To is empty");
    }
    public String m_CarrierNotificationSendEmail_Body = default(System.String);
    public String m_CarrierNotificationSendEmail_CC = default(System.String);
    public String m_CarrierNotificationSendEmail_From = default(System.String);
    public String m_CarrierNotificationSendEmail_To = default(System.String);
    public String m_CarrierNotificationSendEmail_Subject1 = default(System.String);
    #endregion
    #endregion

    #endregion

    #region TimeOutDelay
    public TimeSpan m_TimeOutDelay_TimeoutDuration = default(System.TimeSpan);
    #endregion

  }
}
