﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  public partial class CarrierDashboardWebPartUserControl : UserControl
  {
    #region public
    internal void GetData(Dictionary<InterconnectionDataBase.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        switch (item.Key)
        {
          //case InterconnectionDataBase.ConnectionSelector.TrailerInterconnection:
          //  new TrailerInterconnectionData().GetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
          //  break;
          case InterconnectionDataBase.ConnectionSelector.TruckInterconnection:
            new TruckInterconnectionData().GetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.ShippingInterconnection:
            new ShippingInterconnectionData().GetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.TimeSlotInterconnection:
            new TimeSlotInterconnectionData().GetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.PartnerInterconnection:
            new PartnerInterconnectionData().GetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          default:
            break;
        }
    }
    #endregion

    #region private

    #region state management
    [Serializable]
    private class MyControlState
    {
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      public string PartnerIndex = null;
    }
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        StateMachineEngine(InterfaceEvent.FirstRound);
      m_SaveButton.Click += new EventHandler(m_SaveButton_Click);
      m_NewShippingButton.Click += new EventHandler(m_NewShippingButton_Click);
      m_CancelButton.Click += new EventHandler(m_CancelButton_Click);
      m_EditButton.Click += new EventHandler(m_EditButton_Click);
    }
    protected override void LoadControlState(object state)
    {
      if (state != null)
        m_MyControlState = (MyControlState)state;
    }
    protected override object SaveControlState()
    {
      return m_MyControlState; ;
    }
    protected override void OnPreRender(EventArgs e)
    {
      m_StateLiteral.Text = m_MyControlState.InterfaceState.ToString();
      base.OnPreRender(e);
    }
    #endregion

    #region State machine

    #region Connection call back
    private void NewDataEventHandler(object sender, TimeSlotInterconnectionData e)
    {
      bool _same = m_TimeSlotHiddenField.Value.Equals(e.ID);
      if (_same)
        return;
      m_TimeSlotHiddenField.Value = e.ID;
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          break;
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          ShowTimeSlot(e);
          break;
        default:
          break;
      }
    }
    private void NewDataEventHandler(object sender, ShippingInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          bool _same = m_ShippingHiddenField.Value.Equals(e.ID);
          if (_same)
            return;
          m_ShippingHiddenField.Value = e.ID;
          ShowShipping(e);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          break;
        default:
          break;
      }
    }
    private void NewDataEventHandler(object sender, TruckInterconnectionData e)
    {
      bool _same = m_TruckRegistrationHiddenField.Value.Equals(e.ID);
      if (_same)
        return;
      m_TruckRegistrationHiddenField.Value = e.ID;
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          break;
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          m_TruckRegistrationNumberTextBox.LabelTextProperty(e.Title, false);
          break;
        default:
          break;
      }
    }
    //private void NewDataEventHandler(object sender, TrailerInterconnectionData e)
    //{
    //  bool _same = m_TrailerHiddenField.Value.Equals(e.ID);
    //  if (_same)
    //    return;
    //  m_TrailerHiddenField.Value = e.ID;
    //  switch (CurrentMachineState)
    //  {
    //    case InterfaceState.ViewState:
    //      break;
    //    case InterfaceState.NewState:
    //    case InterfaceState.EditState:
    //      m_TrailerRegistrationNumberTextBox.TextBoxTextProperty(e.Title, false);
    //      break;
    //    default:
    //      break;
    //  }
    //}
    private void NewDataEventHandler(object sender, PartnerInterconnectionData e)
    {
      m_MyControlState.PartnerIndex = e.ID;
      return;
    }
    #endregion

    private enum InterfaceState { ViewState, EditState, NewState }
    private enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, FirstRound };
    [Flags]
    private enum ButtonsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08, DocumentOn = 0x10
    }
    private InterfaceState CurrentMachineState
    {
      get { return m_MyControlState.InterfaceState; }
      set { m_MyControlState.InterfaceState = value; }
    }
    private void StateMachineEngine(InterfaceEvent _event)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          {
            switch (_event)
            {
              case InterfaceEvent.FirstRound:
                SetButtons(ButtonsSet.EditOn | ButtonsSet.NewOn);
                ClearUserInterface();
                break;
              case InterfaceEvent.NewClick:
                ClearUserInterface();
                CurrentMachineState = InterfaceState.NewState;
                StateMachineEngine(InterfaceEvent.FirstRound);
                break;
              case InterfaceEvent.EditClick:
                CurrentMachineState = InterfaceState.EditState;
                StateMachineEngine(InterfaceEvent.FirstRound);
                break;
              default:
                SMError(_event);
                break;
            }
            break;
          }
        case InterfaceState.EditState:
          switch (_event)
          {
            case InterfaceEvent.FirstRound:
              SetButtons(ButtonsSet.CancelOn | ButtonsSet.SaveOn);
              break;
            case InterfaceEvent.SaveClick:
              try
              {
                UpdateShipping();
                CurrentMachineState = InterfaceState.ViewState;
                StateMachineEngine(InterfaceEvent.FirstRound);
              }
              catch (Exception ex)
              {
                this.Controls.Add(new LiteralControl(ex.Message));
              }
              break;
            case InterfaceEvent.CancelClick:
              ClearUserInterface();
              CurrentMachineState = InterfaceState.ViewState;
              StateMachineEngine(InterfaceEvent.FirstRound);
              break;
            default:
              SMError(_event);
              break;
          }
          break;
        case InterfaceState.NewState:
          switch (_event)
          {
            case InterfaceEvent.FirstRound:
              SetButtons(ButtonsSet.CancelOn | ButtonsSet.SaveOn | ButtonsSet.DocumentOn);
              ClearUserInterface();
              break;
            case InterfaceEvent.SaveClick:
              try
              {
                this.CreateShipping();
                CurrentMachineState = InterfaceState.ViewState;
                StateMachineEngine(InterfaceEvent.FirstRound);
              }
              catch (Exception ex)
              {
                this.Controls.Add(new LiteralControl(ex.Message));
              }
              break;
            case InterfaceEvent.CancelClick:
              ClearUserInterface();
              CurrentMachineState = InterfaceState.ViewState;
              StateMachineEngine(InterfaceEvent.FirstRound);
              break;
            default:
              SMError(_event);
              break;
          }
          break;
      }
    }
    private void SMError(InterfaceEvent _event)
    {
      this.Controls.Add(new LiteralControl(String.Format("State machine error, in {0} the event {1} occured", m_MyControlState.InterfaceState.ToString(), _event.ToString())));
    }
    #endregion

    #region Interface actions
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void ClearUserInterface()
    {
      m_ShippingHiddenField.Value = String.Empty;
      m_TimeSlotTextBox.TextBoxTextProperty(String.Empty, true);
      m_TimeSlotHiddenField.Value = String.Empty;
      //m_TrailerRegistrationNumberTextBox.TextBoxTextProperty(String.Empty, false);
      //m_TrailerHiddenField.Value = String.Empty;
      m_TruckRegistrationNumberTextBox.LabelTextProperty(String.Empty, false);
      m_TruckRegistrationHiddenField.Value = String.Empty;
      m_WarehouseTextBox.TextBoxTextProperty(String.Empty, true);
      m_WarehouseHiddenField.Value = String.Empty;
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
    }
    private void ShowShipping(ShippingInterconnectionData _interconnectionData)
    {
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetShippingTimeSlot(edc, _interconnectionData.ID);
          List<LoadDescription> _ld = LoadDescription.GetForShipping(edc, _interconnectionData.ID);
          ShowTimeSlot(_cts);
          string _ldLabel = String.Empty;
          foreach (var _item in _ld)
            _ldLabel += _item.Tytuł + "; ";
          m_DocumentTextBox.TextBoxTextProperty(_ldLabel, true);
        }
      }
      catch (Exception ex)
      {
        m_TimeSlotTextBox.TextBoxTextProperty(ex.Message, true);
        m_DocumentTextBox.TextBoxTextProperty(ex.Message, true);
      }
    }
    private void ShowTimeSlot(TimeSlotInterconnectionData _interconnectionData)
    {
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetAtIndex(edc, _interconnectionData.ID);
          ShowTimeSlot(_cts);
        }
      }
      catch (Exception ex)
      {
        m_TimeSlotTextBox.TextBoxTextProperty(ex.Message, true);
      }
    }
    private void ShowTimeSlot(TimeSlotTimeSlot _cts)
    {
      m_TimeSlotHiddenField.Value = _cts.Identyfikator.ToString();
      m_TimeSlotTextBox.TextBoxTextProperty(String.Format("{0:R}", _cts.StartTime), true);
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseTextBox.TextBoxTextProperty(_wrs.Tytuł, true);
    }
    private void SetButtons(ButtonsSet _set)
    {
      m_DocumentTextBox.Enabled = (_set & ButtonsSet.DocumentOn) != 0;
      m_CancelButton.Enabled = (_set & ButtonsSet.CancelOn) != 0;
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0; ;
      m_EditButton.Enabled = (_set & ButtonsSet.EditOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
    }
    private void CreateShipping()
    {
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        Partner _prtnr = Partner.GetAtIndex(edc, m_MyControlState.PartnerIndex);
        TimeSlotTimeSlot _ts = TimeSlotTimeSlot.GetAtIndex(edc, m_TimeSlotHiddenField.Value);
        ShippingOperationInbound _sp = new ShippingOperationInbound
        {
          Tytuł = String.Format("{0}", m_DocumentTextBox.Text),
          VendorName = _prtnr,
          State = Entities.State.Scheduled,
          StartTime = _ts.StartTime
        };
        _ts.MakeBooking(_sp);
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text,
          ShippingIndex = _sp
        };
        edc.Shipping.InsertOnSubmit(_sp);
        edc.LoadDescription.InsertOnSubmit(_ld);
        edc.SubmitChanges();
      }
    }
    private void UpdateShipping()
    {
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        TimeSlotTimeSlot _newts = TimeSlotTimeSlot.GetAtIndex(edc, m_TimeSlotHiddenField.Value);
        ShippingOperationInbound _si = ShippingOperationInbound.GetAtIndex(edc, m_ShippingHiddenField.HiddenField2Int());
        TimeSlotTimeSlot _oldts = TimeSlotTimeSlot.GetShippingTimeSlot(edc, _si.Identyfikator);
        _newts.MakeBooking(_si);
        _oldts.ReleaseBooking();
        edc.SubmitChanges();
      }
    }
    #endregion

    #region variables
    private MyControlState m_MyControlState = new MyControlState();
    #endregion

    #region Event Handlers
    private void m_NewShippingButton_Click(object sender, EventArgs e)
    {
      StateMachineEngine(InterfaceEvent.NewClick);
    }
    private void m_SaveButton_Click(object sender, EventArgs e)
    {
      StateMachineEngine(InterfaceEvent.SaveClick);
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      StateMachineEngine(InterfaceEvent.CancelClick);
    }
    private void m_EditButton_Click(object sender, EventArgs e)
    {
      StateMachineEngine(InterfaceEvent.EditClick);
    }

    #endregion

    #endregion
  }
}
