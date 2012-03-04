﻿using System;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  internal abstract class StateMachineEngine
  {
    #region public

    [Flags]
    internal enum ControlsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08,
      DocumentOn = 0x10, AbortOn = 0x20, CommentsOn = 0x40, EstimatedDeliveryTime = 0x80,
      RouteOn = 0x100, SecurityEscortOn = 0x200, WarehouseOn = 0x400, TimeSlotOn = 0x800,
      AcceptOn = 0x1000, TransportUnitOn = 0x2000, CityOn = 0x4000, EscortRequiredOn = 0x8000,
      OperatorControlsOn= 0x10000
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, EnterState, AbortClick }
    internal enum InterfaceState { ViewState, EditState, NewState }

    #region Event Handlers
    internal void NewShippingButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.NewState;
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.NewClick);
          break;
      }
    }
    internal void SaveButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          {
            ActionResult _rslt = UpdateShipping();
            if (_rslt.Valid)
              CurrentMachineState = InterfaceState.ViewState;
            else
              ShowActionResult(_rslt);
            break;
          }
        case InterfaceState.NewState:
          {
            ActionResult _rslt = this.CreateShipping();
            if (_rslt.Valid)
              CurrentMachineState = InterfaceState.ViewState;
            else
              ShowActionResult(_rslt);
            break;
          }
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.SaveClick);
          break;
      };
    }
    internal void CancelButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          ActionResult _rslt = ShowShipping();
          if (!_rslt.Valid)
            ShowActionResult(_rslt);
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.CancelClick);
          break;
      }
    }
    internal void EditButton_Click(object sender, EventArgs e)
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
    internal void AbortButton_Click(object sender, EventArgs e)
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
    internal void AcceptButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          AcceptShipping();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.AbortClick);
          break;
      }
    }
    internal void m_SecurityRequiredChecbox_CheckedChanged(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          UpdateEscxort();
          break;
        case InterfaceState.ViewState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.AbortClick);
          break;
      }
    }
    internal abstract InterfaceState CurrentMachineState { get; set; }
    #endregion

    #region Connection call back
    internal void NewDataEventHandler(object sender, TimeSlotInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          break;
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          SetInterconnectionData(e);
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
          SetInterconnectionData(e);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          break;
        default:
          break;
      }
    }
    internal void NewDataEventHandler(object sender, RouteInterconnectionnData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          SetInterconnectionData(e);
          break;
        case InterfaceState.NewState:
        case InterfaceState.ViewState:
          break;
        default:
          break;
      }
    }
    internal void NewDataEventHandler(object sender, SecurityEscortCatalogInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          SetInterconnectionData(e);
          break;
        case InterfaceState.NewState:
        case InterfaceState.ViewState:
          break;
        default:
          break;
      }
    }
    internal void NewDataEventHandler(object sender, PartnerInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          SetInterconnectionData(e);
          break;
        case InterfaceState.ViewState:
          break;
        default:
          break;
      }
    }
    internal void NewDataEventHandler(object sender, CityInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
          SetInterconnectionData(e);
          break;
        case InterfaceState.EditState:
        case InterfaceState.ViewState:
          break;
        default:
          break;
      }
    }
    #endregion

    #endregion

    #region protected abstract InterconnectionData
    protected abstract void SetInterconnectionData(ShippingInterconnectionData _shipping);
    protected abstract void SetInterconnectionData(TimeSlotInterconnectionData e);
    protected abstract void SetInterconnectionData(RouteInterconnectionnData e);
    protected abstract void SetInterconnectionData(SecurityEscortCatalogInterconnectionData e);
    protected abstract void SetInterconnectionData(PartnerInterconnectionData e);
    protected abstract void SetInterconnectionData(CityInterconnectionData e);
    #endregion

    #region protected abstract
    protected abstract ActionResult ShowShipping();
    protected abstract void ClearUserInterface();
    protected abstract ActionResult UpdateShipping();
    protected abstract ActionResult CreateShipping();
    protected abstract void AcceptShipping();
    protected abstract void AbortShipping();
    protected abstract void SetEnabled(ControlsSet _buttons);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected abstract void ShowActionResult(ActionResult _rslt);
    protected abstract void UpdateEscxort();
    #endregion

    #region protected
    protected void EnterState()
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SetEnabled(ControlsSet.EditOn | ControlsSet.NewOn);
          break;
        case InterfaceState.EditState:
          SetEnabled
            (
              ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.CommentsOn | ControlsSet.EstimatedDeliveryTime |
              ControlsSet.AbortOn | ControlsSet.AcceptOn | ControlsSet.TransportUnitOn | ControlsSet.SecurityEscortOn | 
              ControlsSet.OperatorControlsOn
            );
          break;
        case InterfaceState.NewState:
          SetEnabled
            (
              ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.CommentsOn | ControlsSet.EstimatedDeliveryTime |
              ControlsSet.DocumentOn | ControlsSet.TransportUnitOn | ControlsSet.SecurityEscortOn
            );
          ClearUserInterface();
          break;
      }
    }
    #endregion

  }
}
