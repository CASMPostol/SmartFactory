using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TrailerManager
{
  /// <summary>
  /// Trailer ManagerUser Control
  /// </summary>
  public partial class TrailerManagerUserControl : UserControl
  {
    #region public
    internal void SetInterconnectionData(Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      if (_ProvidesDictionary.Keys.Contains(InboundInterconnectionData.ConnectionSelector.TrailerInterconnection))
        new TrailerInterconnectionData().SetRowData
          (_ProvidesDictionary[InboundInterconnectionData.ConnectionSelector.TrailerInterconnection], m_StateMachineEngine.NewDataEventHandler);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TrailerManagerUserControl"/> class.
    /// </summary>
    public TrailerManagerUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
    }
    #endregion

    #region UserControl override
    private class ControlState
    {
      #region state fields
      public string ItemID = String.Empty;
      internal StateMachine.ControlsSet SetEnabled = 0;
      public LocalStateMachineEngine.InterfaceState InterfaceState = LocalStateMachineEngine.InterfaceState.ViewState;
      #endregion

      #region public
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {

      }
      catch (Exception _ex)
      {
        throw new ApplicationException("Page_Load exception: " + _ex.Message, _ex);
      }
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState(object state)
    {
      if (state != null)
      {
        m_ControlState = (ControlState)state;
        m_StateMachineEngine.InitMahine(m_ControlState.InterfaceState);
      }
      else
        m_StateMachineEngine.InitMahine();
    }
    /// <summary>
    /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
    /// </summary>
    /// <returns>
    /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
    /// </returns>
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
    }
    #endregion

    #region State machine
    private class LocalStateMachineEngine : StateMachine
    {
      #region ctor
      internal LocalStateMachineEngine(TrailerManagerUserControl _parent)
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override StateMachine.ActionResult Show(TrailerInterconnectionData _shipping)
      {
        return Parent.Show(_shipping);
      }
      protected override StateMachine.ActionResult Show()
      {
        return Parent.Show();
      }
      protected override ActionResult Update()
      {
        return Parent.Update();
      }
      protected override ActionResult Create()
      {
        return Parent.Create();
      }
      protected override StateMachine.ActionResult Delete()
      {
        return Parent.Delete();
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearUserInterface();
      }
      protected override void SetEnabled(ControlsSet _buttons)
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError(StateMachine.InterfaceEvent _interfaceEvent)
      {
        Parent.Controls.Add(new LiteralControl
          (String.Format("State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString())));
      }
      protected override InterfaceState CurrentMachineState
      {
        get
        {
          return Parent.m_ControlState.InterfaceState;
        }
        set
        {
          if (Parent.m_ControlState.InterfaceState == value)
            return;
          Parent.m_ControlState.InterfaceState = value;
          EnterState();
        }
      }
      protected override void ShowActionResult(LocalStateMachineEngine.ActionResult _rslt)
      {
        Parent.ShowActionResult(_rslt);
        base.ShowActionResult(_rslt);
      }
      #endregion

      #region private
      private TrailerManagerUserControl Parent { get; set; }
      internal void InitMahine(InterfaceState _ControlState)
      {
        Parent.m_ControlState.InterfaceState = _ControlState;
      }
      internal void InitMahine()
      {
        Parent.m_ControlState.InterfaceState = InterfaceState.ViewState;
        EnterState();
      }
      #endregion

    }//LocalStateMachineEngine
    #endregion

    #region Variables
    private ControlState m_ControlState = new ControlState(null);
    private LocalStateMachineEngine m_StateMachineEngine = null;
    #endregion

    #region state machine actions
    private  GenericStateMachineEngine<TrailerInterconnectionData>.ActionResult Show(TrailerInterconnectionData _shipping)
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TrailerInterconnectionData>.ActionResult Show()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TrailerInterconnectionData>.ActionResult Update()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TrailerInterconnectionData>.ActionResult Create()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TrailerInterconnectionData>.ActionResult Delete()
    {
      throw new NotImplementedException();
    }
    private void ClearUserInterface()
    {
      m_Comments.Text = String.Empty;
      m_TrailerTitle.Text = String.Empty;
    }
    internal void ShowActionResult(LocalStateMachineEngine.ActionResult _rslt)
    {
      if (_rslt.ActionSucceeded)
        return;
      this.Controls.Add(new LiteralControl(String.Format(GlobalDefinitions.ErrorMessageFormat, _rslt.ActionException.Message)));
    }
    #endregion
  }
}
