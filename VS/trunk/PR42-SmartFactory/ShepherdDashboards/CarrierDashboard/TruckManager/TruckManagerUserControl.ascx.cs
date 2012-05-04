using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TruckManager
{
  public partial class TruckManagerUserControl : UserControl
  {
    #region public
    internal void SetInterconnectionData(Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      if (_ProvidesDictionary.Keys.Contains(InboundInterconnectionData.ConnectionSelector.TruckInterconnection))
        new TruckInterconnectionData().SetRowData
          (_ProvidesDictionary[InboundInterconnectionData.ConnectionSelector.TruckInterconnection], m_StateMachineEngine.NewDataEventHandler);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TruckManagerUserControl"/> class.
    /// </summary>
    public TruckManagerUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
    }
    #endregion

    #region UserControl override
    [Serializable]
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
      internal LocalStateMachineEngine(TruckManagerUserControl _parent)
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override StateMachine.ActionResult Show(TruckInterconnectionData _shipping)
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
      private TruckManagerUserControl Parent { get; set; }
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
    private GenericStateMachineEngine<TruckInterconnectionData>.ActionResult Show(TruckInterconnectionData _shipping)
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TruckInterconnectionData>.ActionResult Show()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TruckInterconnectionData>.ActionResult Update()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TruckInterconnectionData>.ActionResult Create()
    {
      throw new NotImplementedException();
    }
    private GenericStateMachineEngine<TruckInterconnectionData>.ActionResult Delete()
    {
      throw new NotImplementedException();
    }
    private void ClearUserInterface()
    {
      m_Comments.Text = String.Empty;
      m_TruckTitle.Text = String.Empty;
        using (EntitiesDataContext _EDC = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          m_VehicleType.DataSource = from _etx in _EDC.TransportUnitType orderby _etx.Tytuł ascending select new { Title = _etx.Tytuł, ID = _etx.Identyfikator };
          m_VehicleType.DataTextField = "Title";
          m_VehicleType.DataValueField = "ID";
        }
    }
    private void ShowActionResult(LocalStateMachineEngine.ActionResult _rslt)
    {
      if (_rslt.ActionSucceeded)
        return;
      this.Controls.Add(new LiteralControl(String.Format(GlobalDefinitions.ErrorMessageFormat, _rslt.ActionException.Message)));
    }
    #endregion

  }
}
