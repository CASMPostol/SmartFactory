using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  using ButtonsSet = StateMachineEngine.ControlsSet;
  using InterfaceState = StateMachineEngine.InterfaceState;

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
          case InboundInterconnectionData.ConnectionSelector.CityInterconnection:
            new CityInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
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
      m_ShippintInterconnectionEvent += _interface.SetData;
      return _interface;
    }
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        m_DashboardType = value;
        ButtonsSet _inbound = m_AllButtons ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.CityOn ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn;
        switch (value)
        {
          case GlobalDefinitions.Roles.OutboundOwner:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.RouteOn;
            m_EditbilityACL = m_AllButtons;
            m_DocumentLabel.Text = m_DeliveryNoHeaderLabetText;
            break;
          case GlobalDefinitions.Roles.Coordinator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn;
            m_EditbilityACL = m_AllButtons;
            break;
          case GlobalDefinitions.Roles.Supervisor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.CityOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.AcceptOn ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.CityOn;
            break;
          case GlobalDefinitions.Roles.InboundOwner:
            m_SecurityEscortHeaderLabel.Text = m_VendorHeaderLabelText;
            m_VisibilityACL = _inbound;
            m_EditbilityACL = _inbound;
            break;
          case GlobalDefinitions.Roles.Operator:
            m_VisibilityACL = _inbound ^ ButtonsSet.AbortOn ^ ButtonsSet.NewOn;
            m_EditbilityACL = _inbound;
            break;
          case GlobalDefinitions.Roles.Vendor:
            m_SecurityEscortHeaderLabel.Text = m_VendorHeaderLabelText;
            m_VisibilityACL = _inbound;
            m_EditbilityACL = _inbound;
            break;
          case GlobalDefinitions.Roles.Guard:
            m_VisibilityACL = ButtonsSet.CommentsOn | ButtonsSet.DocumentOn | ButtonsSet.RouteOn | ButtonsSet.WarehouseOn
              | ButtonsSet.TimeSlotOn | ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = 0;
            break;
          case GlobalDefinitions.Roles.Forwarder:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.NewOn ^ ButtonsSet.AcceptOn ^ ButtonsSet.TransportUnitOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AcceptOn ^ ButtonsSet.TransportUnitOn;
            m_DocumentLabel.Text = m_DeliveryNoHeaderLabetText;
            break;
          case GlobalDefinitions.Roles.Escort:
            m_VisibilityACL = ButtonsSet.TimeSlotOn | ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn ^ ButtonsSet.AcceptOn ^ ButtonsSet.SecurityEscortOn;
            m_DocumentLabel.Text = m_DeliveryNoHeaderLabetText;
            m_SecurityEscortHeaderLabel.Text = m_SecurityEscortHeaderLabelText;
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
      public string CityID = String.Empty;
      public string SecurityCatalogID = String.Empty;
      public string ShippingID = String.Empty;
      public string TimeSlotID = String.Empty;
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      internal StateMachineEngine.ControlsSet SetEnabled = 0;
      public bool TimeSlotChanged = false;
      public bool Editable = false;
      #endregion

      #region public
      internal void ClearShipping()
      {
        PartnerID = String.Empty;
        RouteID = String.Empty;
        CityID = String.Empty;
        SecurityCatalogID = String.Empty;
        ShippingID = String.Empty;
        TimeSlotID = String.Empty;
        TimeSlotChanged = false;
        Editable = false;
      }
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
      if (!IsPostBack)
      {
        SetVisible(m_AllButtons);
        m_StateMachineEngine.InitMahine();
        if (m_TransportUnitTypeDropDownList.Visible)
        {
          m_TransportUnitTypeDropDownList.DataSource = from _idx in EDC.TransportUnitType
                                                       orderby _idx.Tytuł ascending
                                                       select _idx;
          m_TransportUnitTypeDropDownList.DataTextField = Element.TitlePropertyName;
          m_TransportUnitTypeDropDownList.DataValueField = Element.IDPropertyName;
          m_TransportUnitTypeDropDownList.DataBind();
          m_TransportUnitTypeDropDownList.SelectedIndex = 0;
        }
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
      SetEnabled(m_ControlState.SetEnabled);
      if (!m_ControlState.Editable)
      {
        m_EditButton.Enabled = false;
        m_AbortButton.Enabled = false;
        m_AcceptButton.Enabled = false;
      }
      base.OnPreRender(e);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
      if (_EDC != null)
      {
        try
        {
          _EDC.SubmitChanges();
          _EDC.Dispose();
          _EDC = null;
        }
        catch (Exception ex)
        {
          ReportException("OnUnload", ex);
        }
      }
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

      #region SetInterconnectionData
      protected override void SetInterconnectionData(ShippingInterconnectionData _shipping)
      {
        Parent.SetInterconnectionData(_shipping);
      }
      protected override void SetInterconnectionData(TimeSlotInterconnectionData _data)
      {
        Parent.SetInterconnectionData(_data);
      }
      protected override void SetInterconnectionData(RouteInterconnectionnData _route)
      {
        Parent.SetInterconnectionData(_route);
      }
      protected override void SetInterconnectionData(PartnerInterconnectionData _partner)
      {
        Parent.m_ControlState.PartnerID = _partner.ID;
        Parent.m_SecurityEscortLabel.Text = _partner.Title;
      }
      protected override void SetInterconnectionData(CityInterconnectionData _city)
      {
        Parent.SetInterconnectionData(_city);
      }
      protected override void SetInterconnectionData(SecurityEscortCatalogInterconnectionData _Escort)
      {
        Parent.SetInterconnectionData(_Escort);
      }
      #endregion

      protected override ActionResult ShowShipping()
      {
        return Parent.ShowShipping();
      }
      protected override void AcceptShipping()
      {
        Parent.ChangeShippingState(State.Confirmed);
      }
      protected override ActionResult UpdateShipping()
      {
        return Parent.UpdateShipping();
      }
      protected override ActionResult CreateShipping()
      {
        return Parent.CreateShipping();
      }
      protected override void AbortShipping()
      {
        Parent.ChangeShippingState(State.Canceled);
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearUserInterface();
      }
      protected override void SetEnabled(ControlsSet _buttons)
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError(StateMachineEngine.InterfaceEvent _interfaceEvent)
      {
        Parent.Controls.Add(new LiteralControl
          (String.Format("State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString())));
      }
      protected override void ShowActionResult(ActionResult _rslt)
      {
        foreach (var item in _rslt)
          Parent.Controls.Add(new LiteralControl(item));
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

    #region Interconnection
    private void SetInterconnectionData(ShippingInterconnectionData _interconnectionData)
    {
      if (m_ControlState.ShippingID == _interconnectionData.ID)
        return;
      ClearUserInterface();
      m_ControlState.ShippingID = _interconnectionData.ID;
      ShowShipping(CurrentShipping);
    }
    private void SetInterconnectionData(TimeSlotInterconnectionData _interconnectionData)
    {
      m_ControlState.TimeSlotID = _interconnectionData.ID;
      m_ControlState.TimeSlotChanged = true;
      try
      {
        TimeSlotTimeSlot _cts = Element.GetAtIndex(EDC.TimeSlot, _interconnectionData.ID);
        Show(_cts, true);
      }
      catch (Exception ex)
      {
        m_TimeSlotTextBox.Text = ex.Message;
      }
    }
    private void SetInterconnectionData(CityInterconnectionData _city)
    {
      m_ControlState.CityID = _city.ID;
      m_CityLabel.Text = _city.Title;
      ClearRoute();
    }
    private void SetInterconnectionData(RouteInterconnectionnData _route)
    {
      m_ControlState.RouteID = _route.ID;
      m_RouteLabel.Text = _route.Title;
      ClearSecurityEscor();
    }
    private void SetInterconnectionData(SecurityEscortCatalogInterconnectionData _Escort)
    {
      m_SecurityEscortLabel.Text = _Escort.Title;
      m_ControlState.SecurityCatalogID = _Escort.ID;
    }
    #endregion

    #region Clear
    private void ClearCity()
    {
      m_ControlState.CityID = String.Empty;
      m_CityLabel.Text = String.Empty;
      ClearRoute();
    }
    private void ClearRoute()
    {
      m_ControlState.RouteID = String.Empty;
      m_ControlState.SecurityCatalogID = String.Empty;
      ClearSecurityEscor();
    }
    private void ClearSecurityEscor()
    {
      m_SecurityEscortLabel.Text = String.Empty;
      m_ControlState.SecurityCatalogID = String.Empty;
    }
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void ClearUserInterface()
    {
      m_ControlState.ClearShipping();
      m_TimeSlotTextBox.Text = String.Empty;
      m_WarehouseLabel.Text = String.Empty;
      m_CityLabel.Text = String.Empty;
      m_RouteLabel.Text = String.Empty;
      m_SecurityEscortLabel.Text = String.Empty;
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_CommentsTextBox.TextBoxTextProperty(String.Empty, false);
      m_EstimateDeliveryTimeDateTimeControl.SelectedDate = DateTime.Now;
      m_TransportUnitTypeDropDownList.SelectedIndex = -1;
    }
    #endregion

    #region Show
    private ActionResult ShowShipping()
    {
      ClearUserInterface();
      return ShowShipping(CurrentShipping);
    }
    private ActionResult ShowShipping(Shipping _sppng)
    {
      ActionResult _rsult = new ActionResult();
      if (_sppng == null)
        return _rsult;
      SendShippingData(_sppng);
      try
      {
        TimeSlotTimeSlot _timeSlot = null;
        try
        {
          _timeSlot = (TimeSlotTimeSlot)(from _ts in _sppng.TimeSlot orderby _ts.StartTime ascending select _ts).First();
        }
        catch (Exception) { }
        Show(_timeSlot, _sppng.IsEditable());
        Show(_sppng.LoadDescription);
        m_CommentsTextBox.TextBoxTextProperty(_sppng.CancelationReason, false);
        if (_sppng.IsOutbound.Value)
        {
          m_DocumentLabel.Text = m_DeliveryNoHeaderLabetText;
          m_EstimateDeliveryTimeDateTimeControl.SelectedDate = _sppng.EstimateDeliveryTime.HasValue ? _sppng.EstimateDeliveryTime.Value : DateTime.Now;
          Show(_sppng.Route);
          Show(_sppng.SecurityEscort);
          Show(_sppng.TransportUnit);
          Show(_sppng.City);
        }
        else
        {
          m_DocumentLabel.Text = m_PONoumberHeaderLabetText;
          Show(_sppng.VendorName);
        }
      }
      catch (Exception ex)
      {
        ReportException("UpdateShowShipping", ex);
        _rsult.AddException(ex);
      }
      return _rsult;
    }
    private void Show(TimeSlotTimeSlot _cts, bool _isEditable)
    {
      if (_cts == null)
      {
        m_TimeSlotTextBox.Text = "Select Time Slot";
      }
      m_ControlState.TimeSlotID = _cts.Identyfikator.IntToString();
      m_TimeSlotTextBox.Text = String.Format("{0:R}{1}", _cts.StartTime, _isEditable ? "" : "!");
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseLabel.Text = _wrs.Tytuł;
    }
    private void Show(CityType _city)
    {
      if (_city == null)
        return;
      m_CityLabel.Text = _city.Tytuł;
      m_ControlState.CityID = _city.Identyfikator.IntToString();
    }
    private void Show(Route _route)
    {
      if (_route == null)
        return;
      m_RouteLabel.Text = _route.Tytuł;
      m_ControlState.RouteID = _route.Identyfikator.IntToString();
    }
    private void Show(Partner _partner)
    {
      m_SecurityEscortHeaderLabel.Text = m_VendorHeaderLabelText;
      if (_partner == null)
        return;
      m_SecurityEscortLabel.Text = _partner.Tytuł;
      m_ControlState.PartnerID = _partner.Identyfikator.IntToString();
    }
    private void Show(SecurityEscortCatalog _security)
    {
      m_SecurityEscortHeaderLabel.Text = m_SecurityEscortHeaderLabelText;
      if (_security == null)
        return;
      m_SecurityEscortLabel.Text = _security.Tytuł;
      m_ControlState.SecurityCatalogID = _security.Identyfikator.IntToString();
    }
    private void Show(EntitySet<LoadDescription> _loadDescription)
    {
      if (_loadDescription == null || _loadDescription.Count == 0)
        return;
      string _ldLabel = String.Empty;
      foreach (var _item in _loadDescription)
        _ldLabel += _item.Tytuł + "; ";
      m_DocumentTextBox.TextBoxTextProperty(_ldLabel, true);
    }
    private void Show(TransportUnitTypeTranspotUnit _unitType)
    {
      if (_unitType == null)
        return;
      m_TransportUnitTypeDropDownList.SelectedIndex = -1;
      foreach (ListItem _item in m_TransportUnitTypeDropDownList.Items)
        if (_item.Value.String2Int() == _unitType.Identyfikator)
        {
          _item.Selected = true;
          break;
        }
    }
    #endregion

    #region Shipping management
    private ActionResult CreateShipping()
    {
      ActionResult _rsult = new ActionResult();
      try
      {
        if (m_DocumentTextBox.Text.IsNullOrEmpty())
          _rsult.Add(m_DocumentLabel.Text);
        Shipping _sppng = new Shipping()
        {
          IsOutbound = m_DashboardType == GlobalDefinitions.Roles.OutboundOwner,
          State = Entities.State.Creation
        };
        if (m_ControlState.TimeSlotID.IsNullOrEmpty())
          _rsult.Add(m_TimeSlotLabel.Text);
        UpdateShipping(_sppng, _rsult);
        if (!_rsult.Valid)
          return _rsult;
        EDC.Shipping.InsertOnSubmit(_sppng);
        EDC.SubmitChanges();
        m_ControlState.ShippingID = _sppng.Identyfikator.Value.ToString();
        m_ControlState.Editable = _sppng.IsEditable();
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text, //TODO http://itrserver/Bugs/BugDetail.aspx?bid=3057
          DeliveryNumber = m_DocumentTextBox.Text,
          ShippingIndex = _sppng,
        };
        EDC.LoadDescription.InsertOnSubmit(_ld);
        MakeBooking(_sppng);
        EDC.SubmitChanges();
        ReportAlert(_sppng, "Shipping Created");
        SendShippingData(_sppng);
      }
      catch (Exception ex)
      {
        _rsult.AddException(ex);
        this.ReportException("CreateShipping", ex);
      }
      return _rsult;
    }
    private ActionResult UpdateShipping()
    {
      ActionResult _rst = new ActionResult();
      try
      {
        UpdateShipping(CurrentShipping, _rst);
        UpdateTimeSlot(CurrentShipping, _rst);
        EDC.SubmitChanges();
        m_ControlState.TimeSlotChanged = false;
        ReportAlert(CurrentShipping, "Shipping updated");
      }
      catch (Exception ex)
      {
        _rst.AddException(ex);
        this.ReportException("UpdateShipping", ex);
      }
      return _rst;
    }
    private void UpdateShipping(Shipping _sppng, ActionResult _rsult)
    {
      if (_sppng == null)
      {
        _rsult.Add("Shipping");
        return;
      }
      _sppng.CancelationReason = m_CommentsTextBox.Text;
      UpdateTransportUnitType(_sppng);
      if (_sppng.IsOutbound.Value)
      {
        UpdateEstimateDeliveryTime(_sppng);
        UpdateCity(_sppng, _rsult);
        UpdateRoute(_sppng);
        UpdateSecurityEscort(_sppng);
      }
      else
        UpdateVendor(_sppng, _rsult);
      return;
    }
    private void UpdateEstimateDeliveryTime(Shipping _sppng)
    {
      if (!m_EstimateDeliveryTimeDateTimeControl.IsValid)
      {
        _sppng.EstimateDeliveryTime = null;
        return;
      }
      _sppng.EstimateDeliveryTime = m_EstimateDeliveryTimeDateTimeControl.SelectedDate;
    }
    private void UpdateCity(Shipping _sppng, ActionResult _rsult)
    {
      if (m_ControlState.CityID.IsNullOrEmpty())
      {
        _rsult.Add(m_CityHeaderLabel.Text);
        return;
      }
      _sppng.City = Element.GetAtIndex(EDC.City, m_ControlState.CityID);
    }
    private void UpdateTransportUnitType(Shipping _sppng)
    {
      if (m_TransportUnitTypeDropDownList.SelectedIndex < 0)
      {
        _sppng.TransportUnit = null;
        return;
      }
      _sppng.TransportUnit = Element.GetAtIndex(EDC.TransportUnitType, m_TransportUnitTypeDropDownList.SelectedValue);
    }
    private void UpdateTimeSlot(Shipping _shipping, ActionResult _rslt)
    {
      if (!m_ControlState.TimeSlotChanged)
        return;
      if (m_ControlState.TimeSlotID.IsNullOrEmpty())
      {
        _rslt.Add(m_TimeSlotLabel.Text);
        return;
      }
      _shipping.ReleaseBooking();
      MakeBooking(_shipping);
    }
    private void MakeBooking(Shipping _shipping)
    {
      TimeSlotTimeSlot _newts = Element.GetAtIndex<TimeSlotTimeSlot>(EDC.TimeSlot, m_ControlState.TimeSlotID);
      _newts.MakeBooking(_shipping);
      _shipping.StartTime = _newts.StartTime;
      _shipping.Warehouse = m_WarehouseLabel.Text;
    }
    private void UpdateSecurityEscort(Shipping _sipping)
    {
      if (m_ControlState.SecurityCatalogID.IsNullOrEmpty())
      {
        _sipping.SecurityEscort = null;
        _sipping.SecurityEscortProvider = null;
      }
      else
      {
        _sipping.SecurityEscort = Element.GetAtIndex<SecurityEscortCatalog>(EDC.SecurityEscortCatalog, m_ControlState.SecurityCatalogID);
        _sipping.SecurityEscortProvider = _sipping.SecurityEscort.VendorName;
      }
    }
    private void UpdateRoute(Shipping _sipping)
    {
      if (m_ControlState.RouteID.IsNullOrEmpty())
        _sipping.ChangeRout(null);
      else
        _sipping.ChangeRout(Element.GetAtIndex<Route>(EDC.Route, m_ControlState.RouteID));
    }
    private void UpdateVendor(Shipping _sipping, ActionResult _rsult)
    {
      if (m_ControlState.PartnerID.IsNullOrEmpty())
      {
        _rsult.Add(m_SecurityEscortHeaderLabel.Text);
        return;
      }
      if ((_sipping.VendorName != null) && (m_ControlState.PartnerID.String2Int() == _sipping.VendorName.Identyfikator.Value))
        return;
      _sipping.VendorName = Element.GetAtIndex(EDC.JTIPartner, m_ControlState.PartnerID);
    }
    private void ChangeShippingState(State _newState)
    {
      try
      {
        CurrentShipping.State = _newState;
        switch (_newState)
        {
          case State.Canceled:
            CurrentShipping.ReleaseBooking();
            ReportAlert(CurrentShipping, "The shipping has been canceled.");
            break;
          case State.Confirmed:
            ReportAlert(CurrentShipping, "The shipping has been confirmed.");
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
        EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        ReportException("AbortShipping", ex);
      }
    }
    private void SendShippingData(Shipping _sppng)
    {
      try
      {
        if (m_ShippintInterconnectionEvent == null)
          return;
        m_ShippintInterconnectionEvent(this, new InterconnectionDataTable<Shipping>.InterconnectionEventArgs(_sppng));
      }
      catch (Exception ex)
      {
        ReportException("SendShippingData", ex);
      }
    }

    #endregion

    #region Controls management
    private void SetVisible(StateMachineEngine.ControlsSet _set)
    {
      _set &= m_VisibilityACL;
      m_CommentsTextBox.Visible = m_CommentsLabel.Visible = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Visible = m_DocumentLabel.Visible = (_set & ButtonsSet.DocumentOn) != 0;
      m_TimeSlotTextBox.Visible = m_TimeSlotLabel.Visible = (_set & ButtonsSet.TimeSlotOn) != 0;
      m_WarehouseLabel.Visible = m_WarehousehHeaderLabel.Visible = (_set & ButtonsSet.WarehouseOn) != 0;
      //buttons
      m_AcceptButton.Visible = (_set & ButtonsSet.AcceptOn) != 0;
      m_EditButton.Visible = (_set & ButtonsSet.EditOn) != 0;
      m_AbortButton.Visible = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Visible = (_set & ButtonsSet.CancelOn) != 0;
      m_NewShippingButton.Visible = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Visible = (_set & ButtonsSet.SaveOn) != 0;
      //outbound
      m_CityHeaderLabel.Visible = (_set & ButtonsSet.CityOn) != 0;
      m_CityLabel.Visible = (_set & ButtonsSet.CityOn) != 0;
      m_TransportUnitTypeDropDownList.Visible = (_set & ButtonsSet.TransportUnitOn) != 0;
      m_TransportUnitTypeLabel.Visible = (_set & ButtonsSet.TransportUnitOn) != 0;
      m_EstimateDeliveryTimeLabel.Visible = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Visible = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_RouteLabel.Visible = m_RouteHeaderLabel.Visible = (_set & ButtonsSet.RouteOn) != 0;
      m_SecurityEscortLabel.Visible = (_set & ButtonsSet.SecurityEscortOn) != 0;
      m_SecurityEscortHeaderLabel.Visible = (_set & ButtonsSet.SecurityEscortOn) != 0;
    }
    private void SetEnabled(StateMachineEngine.ControlsSet _set)
    {
      _set &= m_EditbilityACL;
      m_CommentsTextBox.Enabled = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Enabled = (_set & ButtonsSet.DocumentOn) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Enabled = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_TimeSlotTextBox.Enabled = false;
      m_WarehouseLabel.Enabled = false;
      m_TransportUnitTypeDropDownList.Enabled = (_set & ButtonsSet.TransportUnitOn) != 0;
      m_TransportUnitTypeLabel.Enabled = (_set & ButtonsSet.TransportUnitOn) != 0;
      //Buttons
      m_AcceptButton.Enabled = (_set & ButtonsSet.AcceptOn) != 0;
      m_AbortButton.Enabled = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Enabled = (_set & ButtonsSet.CancelOn) != 0;
      m_EditButton.Enabled = (_set & ButtonsSet.EditOn) != 0;
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
    }
    #endregion

    #region Reports
    private void ReportException(string _source, Exception ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      Entities.Anons _entry = new Anons(_source, String.Format(_tmplt, ex.Message));
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    private void ReportAlert(Shipping _shipping, string _msg)
    {
      Entities.AlarmsAndEvents _ae = new Entities.AlarmsAndEvents()
      {
        ShippingIndex = _shipping,
        VendorName = _shipping.VendorName,
        Tytuł = _msg,
      };
      EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
      EDC.SubmitChanges();
    }
    #endregion

    #region variables
    private const string m_PONoumberHeaderLabetText = "Purchase Order";
    private const string m_DeliveryNoHeaderLabetText = "Delivery No";
    private const string m_VendorHeaderLabelText = "Vendor";
    private const string m_SecurityEscortHeaderLabelText = "Security Escort";
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private ControlState m_ControlState = new ControlState(null);
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ControlsSet m_AllButtons = (StateMachineEngine.ControlsSet)int.MaxValue;
    private EntitiesDataContext _EDC = null;
    private EntitiesDataContext EDC
    {
      get
      {
        if (_EDC != null)
          return _EDC;
        _EDC = new EntitiesDataContext(SPContext.Current.Web.Url);
        return _EDC;
      }
    }
    private Shipping _Shipping;
    private Shipping CurrentShipping
    {
      get
      {
        if (_Shipping != null)
          return _Shipping;
        if (m_ControlState.ShippingID.IsNullOrEmpty())
        {
          m_ControlState.Editable = false;
          return null;
        }
        _Shipping = Element.GetAtIndex<Shipping>(EDC.Shipping, m_ControlState.ShippingID);
        m_ControlState.Editable = _Shipping.IsEditable();
        return _Shipping;
      }
    }
    internal event InterconnectionDataTable<Shipping>.SetDataEventArg m_ShippintInterconnectionEvent;
    #endregion

    #endregion
  }
}
