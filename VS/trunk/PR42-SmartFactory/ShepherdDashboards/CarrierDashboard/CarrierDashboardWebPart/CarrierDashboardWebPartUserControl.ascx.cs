using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  using ButtonsSet = StateMachineEngine.ControlsSet;
  using InterfaceState = StateMachineEngine.InterfaceState;
  using System.Web.UI.WebControls;
  using System.Drawing;
  /// <summary>
  /// Carrier Dashboard WebPart UserControl
  /// </summary>
  public partial class CarrierDashboardWebPartUserControl : UserControl
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="CarrierDashboardWebPartUserControl"/> class.
    /// </summary>
    public CarrierDashboardWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
    }
    internal void SetInterconnectionData(Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        switch (item.Key)
        {
          case InboundInterconnectionData.ConnectionSelector.ShippingInterconnection:
            new ShippingInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.TimeSlotInterconnection:
            new TimeSlotInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.PartnerInterconnection:
            new PartnerInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.MarketInterconnection:
            new MarketInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.RouteInterconnection:
            new RouteInterconnectionnData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.SecurityEscortCatalogInterconnection:
            new SecurityEscortCatalogInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
            break;
          default:
            break;
        }
    }
    internal InterconnectionDataTable<Shipping> GetSelectedShippingOperationInboundInterconnectionData()
    {
      string _tn = typeof(Shipping).Name;
      InterconnectionDataTable<Shipping> _interface = new InterconnectionDataTable<Shipping>(_tn);
      m_StateMachineEngine.m_ShippintInterconnectionEvent += _interface.SetData;
      return _interface;
    }
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        m_DashboardType = value;
        switch (value)
        {
          case GlobalDefinitions.Roles.OutboundOwner:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.Coordinator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn;
            m_EditbilityACL = m_AllButtons;
            break;
          case GlobalDefinitions.Roles.Supervisor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.EstimatedDeliveryTime;
            break;
          case GlobalDefinitions.Roles.InboundOwner:
            m_SecurityEscortLabel.Text = m_PartnerHeaderLabelText;
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.AcceptOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.Operator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AbortOn ^
              ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.NewOn ^ ButtonsSet.RouteOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
            break;
          case GlobalDefinitions.Roles.Vendor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
            break;
          case GlobalDefinitions.Roles.Guard:
            m_VisibilityACL = ButtonsSet.CommentsOn | ButtonsSet.DocumentOn | ButtonsSet.RouteOn | ButtonsSet.WarehouseOn
              | ButtonsSet.TimeSlotOn | ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = 0;
            break;
          case GlobalDefinitions.Roles.Forwarder:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.NewOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.WarehouseOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
            break;
          case GlobalDefinitions.Roles.Escort:
            m_VisibilityACL = ButtonsSet.CommentsOn | ButtonsSet.RouteOn | ButtonsSet.TimeSlotOn | ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.WarehouseOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.None:
            m_VisibilityACL = m_AllButtons;
            m_EditbilityACL = 0;
            break;
          default:
            break;
        }
      }
    }
    #endregion

    #region private

    #region UserControl override
    [Serializable]
    private class ControlState
    {
      #region state fields
      public string PartnerID = String.Empty;
      public string RouteID = String.Empty;
      public string MarketID = String.Empty;
      public string SecurityCatalogID = String.Empty;
      public string SecurityPartnerID = String.Empty;
      public string ShippingID = String.Empty;
      public string TimeSlotID = String.Empty;
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      public bool TimeSlotChanged = false;
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
        SetVisible(m_AllButtons);
        m_StateMachineEngine.InitMahine();
      }
      m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
      m_NewShippingButton.Click += new EventHandler(m_StateMachineEngine.NewShippingButton_Click);
      m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
      m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
      m_AbortButton.Click += new EventHandler(m_StateMachineEngine.AbortButton_Click);
      m_AcceptButton.Click += new EventHandler(m_StateMachineEngine.AcceptButton_Click);
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
      m_StateLiteral.Text = m_ControlState.InterfaceState.ToString();
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
      public LocalStateMachineEngine(CarrierDashboardWebPartUserControl _Parent)
        : base()
      {
        Parent = _Parent;
      }
      #endregion

      #region abstract implementation
      protected override void ShowShipping(ShippingInterconnectionData _shipping)
      {
        Parent.ShowShipping(_shipping);
      }
      protected override void ShowShipping()
      {
        Parent.ShowShipping();
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearUserInterface();
      }
      protected override void SetEnabled(ControlsSet _buttons)
      {
        Parent.SetEnabled(_buttons);
      }
      protected override void ShowTimeSlot(TimeSlotInterconnectionData _data)
      {
        Parent.ShowTimeSlot(_data);
      }
      protected override void ShowRoute(RouteInterconnectionnData _route)
      {
        Parent.ShowRoute(_route);
      }
      protected override void ShowPartner(PartnerInterconnectionData _partner)
      {
        Parent.m_ControlState.PartnerID = _partner.ID;
        Parent.m_SelectedSecurityEscortLabel.Text = _partner.Title;
      }
      protected override void ShowMarket(MarketInterconnectionData _market)
      {
        Parent.ShowMarket(_market);
      }
      protected override void ShowSecurityEscortCatalog(SecurityEscortCatalogInterconnectionData _Escort)
      {
        Parent.ShowSecurityEscortCatalog(_Escort);
      }
      protected override void SMError(StateMachineEngine.InterfaceEvent _interfaceEvent)
      {
        Parent.Controls.Add(new LiteralControl
          (String.Format("State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString())));
      }
      protected override void AcceptShipping()
      {
        Parent.ChangeShippingState(State.Confirmed);
      }
      protected override void UpdateShipping()
      {
        Parent.UpdateShipping();
      }
      protected override bool CreateShipping()
      {
        return Parent.CreateShipping();
      }
      protected override void AbortShipping()
      {
        Parent.ChangeShippingState(State.Canceled);
      }
      protected override void UpdateTimeSlot(TimeSlotInterconnectionData e)
      {
        Parent.m_ControlState.TimeSlotID = e.ID;
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
      private CarrierDashboardWebPartUserControl Parent { get; set; }
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
    private LocalStateMachineEngine m_StateMachineEngine;
    #endregion

    #region Interface actions

    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void ClearUserInterface()
    {
      m_TimeSlotTextBox.TextBoxTextProperty(String.Empty, true);
      m_WarehouseTextBox.TextBoxTextProperty(String.Empty, true);
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_CommentsTextBox.TextBoxTextProperty(String.Empty, false);
      m_EstimateDeliveryTimeDateTimeControl.SelectedDate = DateTime.Now;
    }
    private void ShowShipping()
    {
      if (m_ControlState.ShippingID.IsNullOrEmpty())
        ClearUserInterface();
      else
        ShowShipping(m_ControlState.ShippingID);
    }
    private void ShowShipping(ShippingInterconnectionData _interconnectionData)
    {
      if (m_ControlState.ShippingID == _interconnectionData.ID) return;
      m_ControlState.ShippingID = _interconnectionData.ID;
      ShowShipping(_interconnectionData.ID);
    }
    private void ShowShipping(string _shippingID)
    {
      try
      {
        Shipping _sppng = Element.GetAtIndex<Shipping>(m_EDC.Shipping, _shippingID);
        Shipping _sOutbound = _sppng as Shipping;
        if (_sOutbound == null)
          m_SecurityEscortLabel.Text = m_PartnerHeaderLabelText;
        else
        {
          m_DocumentLabel.Text = m_DeliveryNoHeaderLabetText;
          m_EstimateDeliveryTimeDateTimeControl.SelectedDate = _sOutbound.EstimateDeliveryTime.HasValue ? _sOutbound.EstimateDeliveryTime.Value : DateTime.Now;
          m_RouteLabel.Text = _sOutbound.Route != null ? _sOutbound.Route.Tytuł : String.Empty;
          m_SecurityEscortLabel.Text = _sOutbound.SecurityEscort != null ? _sOutbound.SecurityEscort.Tytuł : string.Empty;
        }
        TimeSlotTimeSlot _cts = (TimeSlotTimeSlot)(from _ts in _sppng.TimeSlot orderby _ts.StartTime descending select _ts).First();
        List<LoadDescription> _ld = _sppng.LoadDescription.ToList<LoadDescription>();
        ShowTimeSlot(_cts);
        string _ldLabel = String.Empty;
        string _marketsLabel = String.Empty;
        if (_ld != null)
          foreach (var _item in _ld)
          {
            _ldLabel += _item.Tytuł + "; ";
            _marketsLabel += (_item.Market == null ? "not set" : _item.Market.Tytuł) + "; ";
          }
        m_DocumentTextBox.TextBoxTextProperty(_ldLabel, true);
        m_CommentsTextBox.TextBoxTextProperty(_sppng.CancelationReason, false);
        if (m_RouteLabel.Text.IsNullOrEmpty())
        {
          m_RouteLabel.Text = _marketsLabel;
          m_RouteHeaderLabel.Text = m_SelectedMarketsHeaderLabelText;
        }
        EnableSaveButton();
      }
      catch (Exception ex)
      {
        m_StateMachineEngine.ExceptionCatched(m_EDC, "UpdateShowShipping", ex.Message);
      }
    }
    private void ShowTimeSlot(TimeSlotInterconnectionData _interconnectionData)
    {
      m_ControlState.TimeSlotID = _interconnectionData.ID;
      m_ControlState.TimeSlotChanged = true;
      try
      {
        TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetAtIndex(m_EDC, _interconnectionData.ID, true);
        ShowTimeSlot(_cts);
      }
      catch (Exception ex)
      {
        m_TimeSlotTextBox.TextBoxTextProperty(ex.Message, true);
      }
    }
    private void ShowTimeSlot(TimeSlotTimeSlot _cts)
    {
      m_ControlState.TimeSlotID = _cts.Identyfikator.ToString();
      m_TimeSlotTextBox.TextBoxTextProperty(String.Format("{0:R}", _cts.StartTime), true);
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseTextBox.TextBoxTextProperty(_wrs.Tytuł, true);
    }
    private void ShowRoute(RouteInterconnectionnData _route)
    {
      m_ControlState.RouteID = _route.ID;
      m_ControlState.MarketID = String.Empty;
      m_RouteLabel.Text = _route.Title;
      m_RouteHeaderLabel.Text = m_RouteHeaderLabelText;
      Route _rt = Entities.Element.GetAtIndex<Route>(m_EDC.Route, _route.ID);
      m_ControlState.PartnerID = _rt.VendorName.Identyfikator.IntToString();
    }
    private void ShowMarket(MarketInterconnectionData _market)
    {
      m_ControlState.MarketID = _market.ID;
      m_ControlState.RouteID = String.Empty;
      m_RouteLabel.Text = _market.Title;
      m_RouteHeaderLabel.Text = m_MarketHeaderLabelText;
    }
    private void ShowSecurityEscortCatalog(SecurityEscortCatalogInterconnectionData _Escort)
    {
      m_SelectedSecurityEscortLabel.Text = _Escort.Title;
      m_ControlState.SecurityCatalogID = _Escort.ID;
      SecurityEscortCatalog _sec = Element.GetAtIndex<SecurityEscortCatalog>(m_EDC.SecurityEscortCatalog, _Escort.ID);
      m_ControlState.SecurityPartnerID = _sec.Identyfikator.IntToString();
    }
    private void SetVisible(StateMachineEngine.ControlsSet _set)
    {
      _set &= m_VisibilityACL;
      m_CommentsTextBox.Visible = m_CommentsLabel.Visible = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Visible = m_DocumentLabel.Visible = (_set & ButtonsSet.DocumentOn) != 0;
      m_TimeSlotTextBox.Visible = m_TimeSlotLabel.Visible = (_set & ButtonsSet.TimeSlotOn) != 0;
      m_WarehouseTextBox.Visible = m_WarehouseLabel.Visible = (_set & ButtonsSet.WarehouseOn) != 0;
      //buttons
      m_AcceptButton.Visible = (_set & ButtonsSet.AcceptOn) != 0;
      m_EditButton.Visible = (_set & ButtonsSet.EditOn) != 0;
      m_AbortButton.Visible = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Visible = (_set & ButtonsSet.CancelOn) != 0;
      m_NewShippingButton.Visible = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Visible = (_set & ButtonsSet.SaveOn) != 0;
      //outbound
      m_EstimateDeliveryTimeLabel.Visible = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Visible = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_RouteLabel.Visible = m_RouteHeaderLabel.Visible = (_set & ButtonsSet.RouteOn) != 0;
      m_SecurityEscortLabel.Visible = (_set & ButtonsSet.SecurityEscortOn) != 0;
      m_SelectedSecurityEscortLabel.Visible = (_set & ButtonsSet.SecurityEscortOn) != 0;
    }
    private void SetEnabled(StateMachineEngine.ControlsSet _set)
    {
      _set &= m_EditbilityACL;
      m_CommentsTextBox.Enabled = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Enabled = (_set & ButtonsSet.DocumentOn) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Enabled = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_TimeSlotTextBox.Enabled = false;
      m_WarehouseTextBox.Enabled = false;
      //Buttons
      m_AcceptButton.Enabled = (_set & ButtonsSet.AcceptOn) != 0;
      m_AbortButton.Enabled = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Enabled = (_set & ButtonsSet.CancelOn) != 0;
      m_EditButton.Enabled = ((_set & ButtonsSet.EditOn) != 0) && m_ControlState.IsEditable();
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
    }
    private void EnableSaveButton()
    {
      m_EditButton.Enabled = (m_EditbilityACL & ButtonsSet.SaveOn) != 0;
    }

    #endregion

    #region Shipping management
    private bool CreateShipping()
    {
      bool validated = true;
      try
      {
        if (m_ControlState.TimeSlotID.IsNullOrEmpty())
        {
          this.Controls.Add(
            new Label()
              {
                ForeColor = Color.Red,
                Text = String.Format("{0} must be selected", m_TimeSlotLabel.Text)
              });
          validated = false;
        }
        if (m_DocumentTextBox.Text.IsNullOrEmpty())
        {
          this.Controls.Add(
            new Label()
              {
                ForeColor = Color.Red,
                Text = String.Format("{0} must be provided", m_DocumentLabel.Text)
              });
          validated = false;
        }
        Partner _prtnr = Element.GetAtIndex<Partner>(m_EDC.JTIPartner, m_ControlState.PartnerID);
        TimeSlotTimeSlot _ts = Element.GetAtIndex<TimeSlotTimeSlot>(m_EDC.TimeSlot, m_ControlState.TimeSlotID);
        Shipping _sp = null;
        if (m_DashboardType != GlobalDefinitions.Roles.OutboundOwner)
        {
          if (!validated)
            return false;
          _sp = new Shipping
             (false, String.Format("{0}", m_DocumentTextBox.Text), _prtnr, Entities.State.Creation, _ts.StartTime);
        }
        else
        {
          if (m_ControlState.MarketID.IsNullOrEmpty() && m_ControlState.RouteID.IsNullOrEmpty())
          {
            this.Controls.Add
              (new Label()
                {
                  ForeColor = Color.Red,
                  Text = String.Format("{0} must be provided", m_RouteHeaderLabel.Text)
                });
            validated = false;
          }
          if (!validated)
            return false;
          _sp = new Shipping
             (true,
             String.Format("{0}", m_DocumentTextBox.Text),
             _prtnr,
             Entities.State.Creation,
             m_ControlState.RouteID.IsNullOrEmpty() ? null : Element.GetAtIndex<Route>(m_EDC.Route, m_ControlState.RouteID),
             m_EstimateDeliveryTimeDateTimeControl.SelectedDate,
             _ts.StartTime);
          AssignPartners2Shipping(_sp);
        }
        _sp.CancelationReason = m_CommentsTextBox.Text;
        _ts.MakeBooking(_sp);
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text, //TODO http://itrserver/Bugs/BugDetail.aspx?bid=3057
          DeliveryNumber = m_DocumentTextBox.Text,
          ShippingIndex = _sp,
          Market = m_ControlState.MarketID.IsNullOrEmpty() ? null : Element.GetAtIndex<MarketMarket>(m_EDC.Market, m_ControlState.MarketID)
        };
        m_EDC.Shipping.InsertOnSubmit(_sp);
        m_EDC.LoadDescription.InsertOnSubmit(_ld);
        m_EDC.SubmitChanges();
        m_ControlState.ShippingID = _sp.Identyfikator.Value.ToString();
        ReportAlert(_sp, "Created shipping");
        return true;
      }
      catch (Exception ex)
      {
        ReportException("CreateShipping", ex);
        return false;
      }
    }
    private void ChangeShippingState(State _newState)
    {
      try
      {
        Shipping _si = Element.GetAtIndex(m_EDC.Shipping, m_ControlState.ShippingID);
        _si.State = _newState;
        switch (_newState)
        {
          case State.Canceled:
            TimeSlotTimeSlot _ts = (TimeSlotTimeSlot)(from _tsx in _si.TimeSlot orderby _tsx.StartTime.Value descending select _tsx).First();
            _ts.ReleaseBooking();
            ReportAlert(_si, _si.VendorName, "The shipping has been canceled.");
            break;
          case State.Confirmed:
            ReportAlert(_si, _si.VendorName, "The outbound has been confirmed.");
            break;
          case State.None:
          case State.Invalid:
          case State.Completed:
          case State.Creation:
          case State.Delayed:
          case State.Waiting4ExternalApproval:
          case State.Waiting4InternalApproval:
          case State.Underway:
            throw new ApplicationException("Wrong state");
          default:
            break;
        }
        m_EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        ReportException("AbortShipping", ex);
      }
    }
    private void UpdateShipping()
    {
      if (!m_ControlState.TimeSlotID.String2Int().HasValue)
        return;
      try
      {
        Shipping _si = Element.GetAtIndex<Shipping>(m_EDC.Shipping, m_ControlState.ShippingID);
        if (m_ControlState.TimeSlotChanged)
        {
          TimeSlotTimeSlot _newts = Element.GetAtIndex<TimeSlotTimeSlot>(m_EDC.TimeSlot, m_ControlState.TimeSlotID);
          TimeSlotTimeSlot _oldts = (TimeSlotTimeSlot)(from _ts in _si.TimeSlot orderby _si.StartTime descending select _ts).First();
          _newts.MakeBooking(_si);
          _oldts.ReleaseBooking();
          _si.StartTime = _newts.StartTime;
        }
        _si.CancelationReason = m_CommentsTextBox.Text;
        AssignPartners2Shipping(_si);
        m_EDC.SubmitChanges();
        m_ControlState.TimeSlotChanged = false;
        ReportAlert(_si, "Shipping updated");
      }
      catch (Exception ex)
      {
        m_StateMachineEngine.ExceptionCatched(m_EDC, "UpdateShipping", ex.Message);
      }
    }
    private void AssignPartners2Shipping(Shipping _spo)
    {
      if (m_ControlState.SecurityCatalogID.IsNullOrEmpty())
      {
        _spo.SecurityEscort = null;
        _spo.SecurityEscortProvider = null;
      }
      else
      {
        _spo.SecurityEscort = Element.GetAtIndex<SecurityEscortCatalog>(m_EDC.SecurityEscortCatalog, m_ControlState.SecurityCatalogID);
        _spo.SecurityEscortProvider = _spo.SecurityEscort.VendorName;
      }
      if (m_ControlState.RouteID.IsNullOrEmpty())
      {
        _spo.Route = null;
        _spo.VendorName = null;
      }
      else
      {
        _spo.Route = Element.GetAtIndex<Route>(m_EDC.Route, m_ControlState.RouteID);
        _spo.VendorName = _spo.Route.VendorName;
      }
    }
    #endregion

    #region Reports
    private void ReportException(string _source, Exception ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      m_StateMachineEngine.ExceptionCatched(m_EDC, _source, String.Format(_tmplt, ex.Message));
    }
    private void ReportAlert(Shipping _shipping, string _msg)
    {
      ReportAlert(_shipping, _shipping.VendorName, _msg);
    }
    private void ReportAlert(Shipping _shipping, Partner _partner, string _msg)
    {
      Entities.AlarmsAndEvents _ae = new Entities.AlarmsAndEvents()
      {
        ShippingIndex = _shipping,
        VendorName = _partner,
        Tytuł = _msg,
      };
      m_EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
      m_EDC.SubmitChanges();
    }
    #endregion

    #region variables
    private EntitiesDataContext m_EDC = null;
    private const string m_DeliveryNoHeaderLabetText = "Delivery No";
    private const string m_PartnerHeaderLabelText = "Vendor";
    private const string m_MarketHeaderLabelText = "Primery market";
    private const string m_SelectedMarketsHeaderLabelText = "Selected markets";
    private const string m_RouteHeaderLabelText = "Route";
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private ControlState m_ControlState = new ControlState(null);
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ControlsSet m_AllButtons = (StateMachineEngine.ControlsSet)int.MaxValue;
    #endregion

    #endregion
  }
}
