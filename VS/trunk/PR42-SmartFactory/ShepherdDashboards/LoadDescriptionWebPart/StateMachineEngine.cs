using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using System.Web.UI.WebControls;

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
      DeleteOn = 0x10
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, EnterState, AbortClick };
    internal enum InterfaceState { ViewState, EditState, NewState }

    #region Connection call back
    internal void NewDataEventHandler(object sender, ShippingInterconnectionData _InterconnectionData)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          Show(_InterconnectionData);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          break;
        default:
          break;
      }
    }
    #endregion

    #endregion

    #region private

    #region Event Handlers
    internal void NewButton_Click(object sender, EventArgs e)
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
          Update();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.NewState:
          if (this.Create())
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
          Show();
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
    internal void DeleteButton_Click(object sender, EventArgs e)
    {
      //TODO zaimplementować
    }
    internal void m_LoadDescriptionGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          GridView _gw = (GridView)sender;
          Show(_gw.SelectedDataKey);
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
          Show();
          CurrentMachineState = InterfaceState.ViewState;
          break;
      }
    }
    #endregion

    #region protected
    protected abstract void Show(ShippingInterconnectionData _shipping);
    protected abstract void Show();
    protected abstract void Update();
    protected abstract bool Create();
    protected abstract void ClearUserInterface();
    protected abstract void SetEnabled(ControlsSet _buttons);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected abstract InterfaceState CurrentMachineState { get; set; }
    protected abstract void Show(DataKey dataKey);
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
              ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.DeleteOn
            );
          break;
        case InterfaceState.NewState:
          SetEnabled
            (
              ControlsSet.CancelOn | ControlsSet.SaveOn
            );
          ClearUserInterface();
          break;
      }
    }
    #endregion

    #endregion
  }
}
