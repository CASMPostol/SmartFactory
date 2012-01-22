using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  internal abstract class StateMachineEngine
  {
    public StateMachineEngine()
    {
      m_ControlState = InterfaceState.ViewState;
      EnterState();
    }
    public StateMachineEngine(InterfaceState _state)
    {
      m_ControlState = _state;
    }
    [Flags]
    internal enum ButtonsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08,
      DocumentOn = 0x10, AbortOn = 0x20, CommentsOn = 0x40, EstimatedDeliveryTime = 0x80,
      RouteOn = 0x100, SecurityEscortOn = 0x200, WarehouseOn = 0x400, TimeSlotOn = 0x800,
      DockOn = 0x1000
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, EnterState, AbortClick };
    internal enum InterfaceState { ViewState, EditState, NewState }
    internal InterfaceState CurrentMachineState
    {
      get { return m_ControlState; }
      private set
      {
        if (m_ControlState == value)
          return;
        m_ControlState = value;
        EnterState();
      }
    }
    private void EnterState()
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SetEnabled(ButtonsSet.EditOn | ButtonsSet.NewOn);
          ClearUserInterface();
          break;
        case InterfaceState.EditState:
          SetEnabled(ButtonsSet.CancelOn | ButtonsSet.CommentsOn | ButtonsSet.AbortOn | ButtonsSet.DocumentOn | ButtonsSet.EstimatedDeliveryTime);
          break;
        case InterfaceState.NewState:
          ClearUserInterface();
          SetEnabled(ButtonsSet.CancelOn | ButtonsSet.SaveOn | ButtonsSet.DocumentOn | ButtonsSet.CommentsOn | ButtonsSet.EstimatedDeliveryTime);
          break;
      }
    }

    #region protected
    protected abstract void UpdateShowShipping(ShippingInterconnectionData _shipping);
    protected abstract void ClearUserInterface();
    protected abstract void SetEnabled(ButtonsSet _buttons);
    protected abstract void ShowTimeSlot(TimeSlotInterconnectionData e);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected abstract void UpdateShipping();
    protected abstract void CreateShipping();
    protected abstract void AbortShipping();
    protected abstract void UpdateTimeSlot(TimeSlotInterconnectionData e);
    #endregion

    #region Connection call back
    internal void NewDataEventHandler(object sender, TimeSlotInterconnectionData e)
    {
      UpdateTimeSlot(e);
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
    internal void NewDataEventHandler(object sender, ShippingInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SendShippingData(e.ID);
          UpdateShowShipping(e);
          break;
        case InterfaceState.EditState:
          SendShippingData(e.ID);
          break;
        case InterfaceState.NewState:
          break;
        default:
          break;
      }
    }
    private void SendShippingData(string _ID)
    {
      if (m_ShippintInterconnectionEvent == null)
        return;
      int? _intID = _ID.String2Int();
      if (!_intID.HasValue)
        return;
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
          m_ShippintInterconnectionEvent
            (this, new InterconnectionDataTable<ShippingOperationInbound>.InterconnectionEventArgs
              ((from idx in edc.Shipping where idx.Identyfikator == _intID select idx).First()));
      }
      catch (Exception) { }
    }
    internal event InterconnectionDataTable<ShippingOperationInbound>.SetDataEventArg m_ShippintInterconnectionEvent;
    #endregion

    #region Event Handlers
    internal void m_NewShippingButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ClearUserInterface();
          CurrentMachineState = InterfaceState.NewState;
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.NewClick);

          break;
      }
    }
    internal void m_SaveButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          UpdateShipping();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.NewState:
          this.CreateShipping();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.SaveClick);
          break;
      };
    }
    internal void m_CancelButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          ClearUserInterface();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.CancelClick);
          break;
      }
    }
    internal void m_EditButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.EditState;
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    internal void m_AbortButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          AbortShipping();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.AbortClick);
          break;
      }
    }
    internal void ExceptionCatched(EntitiesDataContext _EDC, string _source, string _message)
    {
      Entities.Anons _entry = new Anons(_source, _message);
      _EDC.EventLogList.InsertOnSubmit(_entry);
      switch (m_ControlState)
      {
        case InterfaceState.ViewState:
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          ClearUserInterface();
          CurrentMachineState = InterfaceState.ViewState;
          break;
      }
    }
    #endregion

    private InterfaceState m_ControlState;
  }
}
