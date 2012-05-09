using System;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;

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
      DeleteOn = 0x10, EditModeOn = 0x20
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, NewData };
    internal enum InterfaceState { ViewState, EditState, NewState }
    internal class ActionResult
    {
      #region public
      internal ActionResult(Exception _excptn)
      {
        ActionException = _excptn;
        LastActionResult = Result.Exception;
      }
      internal enum Result { Success, NotValidated, Exception }
      internal Result LastActionResult { get; private set; }
      internal Exception ActionException { get; private set; }
      internal bool ActionSucceeded { get { return LastActionResult == Result.Success; } }
      internal static ActionResult Success { get { return new ActionResult(Result.Success); } }
      internal static ActionResult NotValidated { get { return new ActionResult(Result.NotValidated); } }
      #endregion

      #region private
      private ActionResult(Result _rslt)
      {
        LastActionResult = _rslt;
      }
      #endregion
    }

    #region Connection call back
    internal void NewDataEventHandler(object sender, ShippingInterconnectionData _InterconnectionData)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ShowShipping(_InterconnectionData);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
          break;
        default:
          SMError(InterfaceEvent.NewData);
          break;
      }
    }
    #endregion

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
          ActionResult _ur = Update();
          switch (_ur.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
              break;
            case ActionResult.Result.Exception:
              ExceptionCatched("Update action", _ur.ActionException.Message);
              break;
          }
          break;
        case InterfaceState.NewState:
          ActionResult _cr = this.Create();
          switch (_cr.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
              break;
            case ActionResult.Result.Exception:
              ClearUserInterface();
              CurrentMachineState = InterfaceState.ViewState;
              ExceptionCatched("Create action", _cr.ActionException.Message);
              break;
            default:
              break;
          }
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
          ShowShipping();
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
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ActionResult _dr = Delete();
          switch (_dr.LastActionResult)
          {
            case ActionResult.Result.Success:
              break;
            case ActionResult.Result.NotValidated:
              break;
            case ActionResult.Result.Exception:
              ExceptionCatched("Create action", _dr.ActionException.Message);
              break;
          }
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    internal void LoadDescriptionGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          GridView _gw = (GridView)sender;
          ShowLoadDescription(_gw.SelectedDataKey);
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    #endregion

    #endregion

    #region private

    #region Actions
    protected abstract ActionResult ShowShipping(ShippingInterconnectionData _shipping);
    protected abstract ActionResult ShowShipping();
    protected abstract ActionResult ShowLoadDescription(DataKey dataKey);
    protected abstract ActionResult Update();
    protected abstract ActionResult Create();
    protected abstract ActionResult Delete();
    protected abstract void ClearUserInterface();
    protected abstract void SetEnabled(ControlsSet _buttons);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected abstract void ExceptionCatched(string _source, string _message);
    #endregion

    protected abstract InterfaceState CurrentMachineState { get; set; }
    protected void EnterState()
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SetEnabled(ControlsSet.EditOn | ControlsSet.NewOn | ControlsSet.DeleteOn);
          break;
        case InterfaceState.EditState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn);
          break;
        case InterfaceState.NewState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn);
          ClearUserInterface();
          break;
      }
    }

    #endregion
  }
}
