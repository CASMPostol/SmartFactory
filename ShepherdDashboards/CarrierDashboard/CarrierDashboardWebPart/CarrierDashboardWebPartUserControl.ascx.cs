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
            new PartnerInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
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
    internal InterconnectionDataTable<ShippingOperationInbound> GetSelectedShippingOperationInboundInterconnectionData()
    {
      string _tn = typeof(ShippingOperationInbound).Name;
      InterconnectionDataTable<ShippingOperationInbound> _interface = new InterconnectionDataTable<ShippingOperationInbound>(_tn);
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
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.AcceptOn;
            break;
          case GlobalDefinitions.Roles.Coordinator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn;
            m_EditbilityACL = m_AllButtons;
            break;
          case GlobalDefinitions.Roles.Supervisor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.AcceptOn;
            break;
          case GlobalDefinitions.Roles.InboundOwner:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
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
      public string PartnerIndex = String.Empty;
      public string Route = String.Empty;
      public string SecurityCatalog = String.Empty;
      public string SecurityPartner = String.Empty;
      public string ShippingID = String.Empty;
      public string TimeSlot = String.Empty;
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      public bool Outbound = false;
      public bool TimeSlotChanged = false;
      #endregion
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
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
    private LocalStateMachineEngine m_StateMachineEngine;
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
        if (Parent.m_ControlState.ShippingID == _shipping.ID) return;
        Parent.m_ControlState.ShippingID = _shipping.ID;
        try
        {
          ShippingOperationInbound _sppng = Element.GetAtIndex<ShippingOperationInbound>(Parent.m_EDC.Shipping, _shipping.ID);
          ShippingOperationOutbound _so = _sppng as ShippingOperationOutbound;
          if (_so != null)
          {
            Parent.m_ControlState.Outbound = true;
            Parent.m_DocumentLabel.Text = CarrierDashboardWebPartUserControl.m_DeliveryNoLabetText;
            Parent.m_EstimateDeliveryTimeDateTimeControl.SelectedDate = _so.EstimateDeliveryTime.HasValue ? _so.EstimateDeliveryTime.Value : DateTime.Now;
            Parent.m_RouteLabel.Text = _so.Route != null ? _so.Route.Tytuł : String.Empty;
            Parent.m_SecurityEscortLabel.Text = _so.SecurityEscort != null ? _so.SecurityEscort.Tytuł : string.Empty;
          }
          TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetShippingTimeSlot(Parent.m_EDC, _shipping.ID);
          List<LoadDescription> _ld = LoadDescription.GetForShipping(Parent.m_EDC, _shipping.ID);
          Parent.ShowTimeSlot(_cts);
          string _ldLabel = String.Empty;
          foreach (var _item in _ld)
            _ldLabel += _item.Tytuł + "; ";
          Parent.m_DocumentTextBox.TextBoxTextProperty(_ldLabel, true);
        }
        catch (Exception ex)
        {
          Parent.m_StateMachineEngine.ExceptionCatched(Parent.m_EDC, "UpdateShowShipping", ex.Message);
        }
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
      protected override void CreateShipping()
      {
        Parent.CreateShipping();
      }
      protected override void AbortShipping()
      {
        Parent.ChangeShippingState(State.Canceled);
      }
      protected override void UpdateTimeSlot(TimeSlotInterconnectionData e)
      {
        Parent.m_ControlState.TimeSlot = e.ID;
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
    #endregion

    #region Interface actions
    private void NewDataEventHandler(object sender, PartnerInterconnectionData e)
    {
      m_ControlState.PartnerIndex = e.ID;
      return;
    }
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void ClearUserInterface()
    {
      m_TimeSlotTextBox.TextBoxTextProperty(String.Empty, true);
      m_WarehouseTextBox.TextBoxTextProperty(String.Empty, true);
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_EstimateDeliveryTimeDateTimeControl.SelectedDate = DateTime.Now;
    }
    private void ShowTimeSlot(TimeSlotInterconnectionData _interconnectionData)
    {
      m_ControlState.TimeSlot = _interconnectionData.ID;
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
      m_ControlState.TimeSlot = _cts.Identyfikator.ToString();
      m_TimeSlotTextBox.TextBoxTextProperty(String.Format("{0:R}", _cts.StartTime), true);
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseTextBox.TextBoxTextProperty(_wrs.Tytuł, true);
    }
    private void ShowRoute(RouteInterconnectionnData _route)
    {
      m_RouteLabel.Text = _route.Title;
      m_ControlState.Route = _route.ID;
      Route _rt = Entities.Element.GetAtIndex<Route>(m_EDC.Route, _route.ID);
      m_ControlState.PartnerIndex = _rt.VendorName.Identyfikator.IntToString();
    }
    private void ShowSecurityEscortCatalog(SecurityEscortCatalogInterconnectionData _Escort)
    {
      m_SelectedSecurityEscortLabel.Text = _Escort.Title;
      m_ControlState.SecurityCatalog = _Escort.ID;
      SecurityEscortCatalog _sec = Element.GetAtIndex<SecurityEscortCatalog>(m_EDC.SecurityEscortCatalog, _Escort.ID);
      m_ControlState.SecurityPartner = _sec.Identyfikator.IntToString();
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
      m_EditButton.Enabled = (_set & ButtonsSet.EditOn) != 0;
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
    }
    #endregion

    #region Shipping management
    private void CreateShipping()
    {
      try
      {
        Partner _prtnr = Element.GetAtIndex<Partner>(m_EDC.JTIPartner, m_ControlState.PartnerIndex);
        TimeSlotTimeSlot _ts = Element.GetAtIndex<TimeSlotTimeSlot>(m_EDC.TimeSlot, m_ControlState.TimeSlot);
        ShippingOperationInbound _sp = null;
        if (m_DashboardType != GlobalDefinitions.Roles.OutboundOwner)
          _sp = new ShippingOperationInbound
          (
            String.Format("{0}", m_DocumentTextBox.Text),
            _prtnr,
            Entities.State.Creation,
            _ts.StartTime
          );
        else
        {
          ShippingOperationOutbound _spo = new ShippingOperationOutbound
            (
              m_EstimateDeliveryTimeDateTimeControl.SelectedDate,
              String.Format("{0}", m_DocumentTextBox.Text),
              _prtnr,
              Entities.State.Creation,
              _ts.StartTime
            );
          _sp = _spo;
          AssignPartners2Shipping(_spo);
          _spo.EstimateDeliveryTime = m_EstimateDeliveryTimeDateTimeControl.SelectedDate;
        }
        _sp.CancelationReason = m_CommentsTextBox.Text;
        _ts.MakeBooking(_sp);
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text,
          ShippingIndex = _sp
        };
        m_EDC.Shipping.InsertOnSubmit(_sp);
        m_EDC.LoadDescription.InsertOnSubmit(_ld);
        m_EDC.SubmitChanges();
        ReportAlert(_sp, "Created shipping");
      }
      catch (Exception ex)
      {
        ReportException("CreateShipping", ex);
      }
    }
    private void ChangeShippingState(State _newState)
    {
      try
      {
        ShippingOperationInbound _si = ShippingOperationInbound.GetAtIndex(m_EDC, m_ControlState.ShippingID.String2Int());
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
      if (!m_ControlState.TimeSlot.String2Int().HasValue)
        return;
      try
      {
        ShippingOperationInbound _si = Element.GetAtIndex<ShippingOperationInbound>(m_EDC.Shipping, m_ControlState.ShippingID);
        if (m_ControlState.TimeSlotChanged)
        {
          TimeSlotTimeSlot _newts = (TimeSlotTimeSlot)(from _ts in _si.TimeSlot orderby _ts.StartTime descending select _ts).First();
          TimeSlotTimeSlot _oldts = TimeSlotTimeSlot.GetShippingTimeSlot(m_EDC, _si.Identyfikator);
          _newts.MakeBooking(_si);
          _oldts.ReleaseBooking();
          _si.StartTime = _newts.StartTime;
        }
        _si.CancelationReason = m_CommentsTextBox.Text;
        ShippingOperationOutbound _so = _si as ShippingOperationOutbound;
        AssignPartners2Shipping(_so);
        m_EDC.SubmitChanges();
        m_ControlState.TimeSlotChanged = false;
        ReportAlert(_si, "Shipping updated");
      }
      catch (Exception ex)
      {
        m_StateMachineEngine.ExceptionCatched(m_EDC, "UpdateShipping", ex.Message);
      }
    }
    private void AssignPartners2Shipping(ShippingOperationOutbound _spo)
    {
      if (m_ControlState.SecurityCatalog.IsNullOrEmpty())
      {
        _spo.SecurityEscort = null;
        _spo.SecurityEscortProvider = null;
      }
      else
      {
        _spo.SecurityEscort = Element.GetAtIndex<SecurityEscortCatalog>(m_EDC.SecurityEscortCatalog, m_ControlState.SecurityCatalog);
        _spo.SecurityEscortProvider = _spo.SecurityEscort.VendorName;
      }
      if (m_ControlState.Route.IsNullOrEmpty())
      {
        _spo.Route = null;
        _spo.VendorName = null;
      }
      else
      {
        _spo.Route = Element.GetAtIndex<Route>(m_EDC.Route, m_ControlState.Route);
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
    private void ReportAlert(ShippingOperationInbound _shipping, string _msg)
    {
      ReportAlert(_shipping, _shipping.VendorName, _msg);
    }
    private void ReportAlert(ShippingOperationInbound _shipping, Partner _partner, string _msg)
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
    private const string m_DeliveryNoLabetText = "Delivery No";
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private ControlState m_ControlState = new ControlState(null);
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ControlsSet m_AllButtons = (StateMachineEngine.ControlsSet)int.MaxValue;
    #endregion

    #endregion
  }
}
