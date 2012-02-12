using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart
{
  using InterfaceState = StateMachineEngine.InterfaceState;
  using CAS.SmartFactory.Shepherd.Dashboards.Entities;
  using Microsoft.SharePoint;
  public partial class LoadDescriptionWebPartUserControl : UserControl
  {
    #region public
    public LoadDescriptionWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
    }
    internal void SetInterconnectionData(Dictionary<CarrierDashboard.InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      if (_ProvidesDictionary.Keys.Contains(InboundInterconnectionData.ConnectionSelector.ShippingInterconnection))
        new ShippingInterconnectionData().SetRowData
          (_ProvidesDictionary[InboundInterconnectionData.ConnectionSelector.ShippingInterconnection], m_StateMachineEngine.NewDataEventHandler);
    }
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        switch (value)
        {
          case GlobalDefinitions.Roles.InboundOwner:
            break;
          case GlobalDefinitions.Roles.OutboundOwner:
            break;
          case GlobalDefinitions.Roles.Coordinator:
            break;
          case GlobalDefinitions.Roles.Supervisor:
            break;
          case GlobalDefinitions.Roles.Operator:
            break;
          case GlobalDefinitions.Roles.Vendor:
            break;
          case GlobalDefinitions.Roles.Forwarder:
            break;
          case GlobalDefinitions.Roles.Escort:
            break;
          case GlobalDefinitions.Roles.Guard:
            break;
          case GlobalDefinitions.Roles.None:
            break;
          default:
            break;
        };
      }
    }
    #endregion

    #region UserControl override
    [Serializable]
    private class ControlState
    {
      #region state fields
      public string ShippingID = String.Empty;
      public LocalStateMachineEngine.InterfaceState InterfaceState = LocalStateMachineEngine.InterfaceState.ViewState;
      #endregion

      #region public
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
      internal bool IsEditable()
      {
        return !ShippingID.IsNullOrEmpty();
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
      m_EDC = new EntitiesDataContext(SPContext.Current.Web.Url);
      base.OnInit(e);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        //SetVisible(m_AllButtons);
        m_StateMachineEngine.InitMahine();
        if (m_PalletTypesDropDown.Visible)
        {
          m_PalletTypesDropDown.DataSource = from _idx in m_EDC.PalletTypes
                                             orderby _idx.Tytuł ascending
                                             select new { Label = _idx.Tytuł, Index = _idx.Identyfikator };
          m_PalletTypesDropDown.DataTextField = "Label";
          m_PalletTypesDropDown.DataValueField = "Index";
          m_PalletTypesDropDown.DataBind();
          m_PalletTypesDropDown.SelectedIndex = 0;
        }
        if (m_CommodityDropDown.Visible)
        {
          m_CommodityDropDown.DataSource = from _idx in m_EDC.Commodity
                                           orderby _idx.Tytuł ascending
                                           select new { Label = _idx.Tytuł, Index = _idx.Identyfikator };
          m_CommodityDropDown.DataTextField = "Label";
          m_CommodityDropDown.DataValueField = "Index";
          m_CommodityDropDown.DataBind();
          m_CommodityDropDown.SelectedIndex = 0;
        }
        if (m_CommodityDropDown.Visible)
        {
          m_MarketDropDown.DataSource = from _idx in m_EDC.Market
                                        orderby _idx.Tytuł ascending
                                        select new { Label = _idx.Tytuł, Index = _idx.Identyfikator };
          m_MarketDropDown.DataTextField = "Label";
          m_MarketDropDown.DataValueField = "Index";
          m_MarketDropDown.DataBind();
          m_MarketDropDown.SelectedIndex = 0;
        }

      }
      //m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
      //m_NewShippingButton.Click += new EventHandler(m_StateMachineEngine.NewShippingButton_Click);
      //m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
      //m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
      //m_AbortButton.Click += new EventHandler(m_StateMachineEngine.AbortButton_Click);
      //m_AcceptButton.Click += new EventHandler(m_StateMachineEngine.AcceptButton_Click);
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
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
      m_EDC.Dispose();
      base.OnUnload(e);
    }
    #endregion
    #region State machine
    private class LocalStateMachineEngine : StateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine(LoadDescriptionWebPartUserControl _parent)
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override void ShowLoading(ShippingInterconnectionData _shipping)
      {
        Parent.ShowShipping(_shipping);
      }
      protected override void ShowLoading()
      {
        Parent.ShowShipping();
      }
      protected override void UpdateLoading()
      {
        Parent.UpdateLoading();
      }
      protected override bool Createloading()
      {
        return Parent.Createloading();
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearUserInterface();
      }
      protected override void SetEnabled(ControlsSet _buttons)
      {
        Parent.SetEnabled(_buttons);
      }
      protected override void SMError(StateMachineEngine.InterfaceEvent _interfaceEvent)
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
      #endregion

      #region private
      private LoadDescriptionWebPartUserControl Parent { get; set; }
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
    private EntitiesDataContext m_EDC = null;
    #endregion

    internal void ShowShipping(ShippingInterconnectionData _shipping)
    {
      //TODO throw new NotImplementedException();
    }

    internal void ShowShipping()
    {
      throw new NotImplementedException();
    }

    internal void ClearUserInterface()
    {
      //TODO throw new NotImplementedException();
    }

    internal void SetEnabled(StateMachineEngine.ControlsSet _buttons)
    {
      //TODO throw new NotImplementedException();
    }

    internal void UpdateLoading()
    {
      //TODO throw new NotImplementedException();
    }

    internal bool Createloading()
    {
      //TODO throw new NotImplementedException();
      return true;
    }
  }
}
