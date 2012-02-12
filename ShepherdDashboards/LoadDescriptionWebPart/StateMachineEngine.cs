using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart
{
  internal abstract class StateMachineEngine
  {
    #region public
    public StateMachineEngine() { }
    [Flags]
    internal enum ControlsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08,
      AbortOn = 0x10
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, EnterState, AbortClick };
    internal enum InterfaceState { ViewState, EditState, NewState }
    #endregion

    #region Connection call back
    internal void NewDataEventHandler(object sender, ShippingInterconnectionData e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ShowLoading(e);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          break;
        default:
          break;
      }
    }
    #endregion

    #region private

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
          UpdateLoading();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.NewState:
          if (this.Createloading())
            CurrentMachineState = InterfaceState.ViewState;
          break;
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
          ShowLoading();
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
    internal void ExceptionCatched(EntitiesDataContext _EDC, string _source, string _message)
    {
      Entities.Anons _entry = new Anons(_source, _message);
      _EDC.EventLogList.InsertOnSubmit(_entry);
      _EDC.SubmitChanges();
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          ShowLoading();
          CurrentMachineState = InterfaceState.ViewState;
          break;
      }
    }
    #endregion

    #region protected
    protected abstract void ShowLoading(ShippingInterconnectionData _shipping);
    protected abstract void ShowLoading();
    protected abstract void UpdateLoading();
    protected abstract bool Createloading();
    protected abstract void ClearUserInterface();
    protected abstract void SetEnabled(ControlsSet _buttons);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected abstract InterfaceState CurrentMachineState { get; set; }
    #endregion

    private LoadDescriptionWebPartUserControl m_Parent;

    #endregion
  }
}
