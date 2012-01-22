using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  using ButtonsSet = StateMachineEngine.ButtonsSet;
  using InterfaceState = StateMachineEngine.InterfaceState;
  using System.Web.UI.WebControls;
  /// <summary>
  /// Carrier Dashboard WebPart UserControl
  /// </summary>
  public partial class CarrierDashboardWebPartUserControl : UserControl
  {
    #region public
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
            m_VisibilityACL = m_AllButtons;
            m_EditbilityACL = m_AllButtons;
            break;
          case GlobalDefinitions.Roles.Coordinator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn;
            m_EditbilityACL = m_AllButtons;
            break;
          case GlobalDefinitions.Roles.Supervisor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime;
            break;
          case GlobalDefinitions.Roles.InboundOwner:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.Operator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.AbortOn ^
              ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.NewOn ^ ButtonsSet.RouteOn ^ ButtonsSet.DockOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.Vendor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.DockOn;
            SelectDefaultWarehouse();
            break;
          case GlobalDefinitions.Roles.Guard:
            m_VisibilityACL = ButtonsSet.CommentsOn | ButtonsSet.DocumentOn | ButtonsSet.RouteOn | ButtonsSet.WarehouseOn | ButtonsSet.TimeSlotOn | ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = 0;
            break;
          case GlobalDefinitions.Roles.Forwarder:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.SecurityEscortOn ^ ButtonsSet.NewOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.WarehouseOn ^ ButtonsSet.SecurityEscortOn;
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
    private class MyControlState
    {
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      public string PartnerIndex = null;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    EntitiesDataContext m_EDC = null;
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
      if (!this.IsPostBack)
        PopulateDropDownLists();
      m_SaveButton.Click += new EventHandler(m_StateMachineEngine.m_SaveButton_Click);
      m_NewShippingButton.Click += new EventHandler(m_StateMachineEngine.m_NewShippingButton_Click);
      m_CancelButton.Click += new EventHandler(m_StateMachineEngine.m_CancelButton_Click);
      m_EditButton.Click += new EventHandler(m_StateMachineEngine.m_EditButton_Click);
      m_AbortButton.Click += new EventHandler(m_StateMachineEngine.m_AbortButton_Click);
      m_EstimateDeliveryTime.DateChanged += new EventHandler(EnableSave);
      m_RouteDropDownList.SelectedIndexChanged += new EventHandler(EnableSave);
      m_SecurityDropDownList.SelectedIndexChanged += new EventHandler(EnableSave);
      m_CommentsTextBox.TextChanged += new EventHandler(EnableSave);
      m_CityDropDownList.SelectedIndexChanged += new EventHandler(m_CityDropDownList_SelectedIndexChanged);
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState(object state)
    {
      if (state != null)
      {
        m_ControlState = (MyControlState)state;
        m_StateMachineEngine = new LocalStateMachineEngine(this, m_ControlState.InterfaceState);
      }
      else
        m_StateMachineEngine = new LocalStateMachineEngine(this);
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
    private StateMachineEngine m_StateMachineEngine;
    private class LocalStateMachineEngine : StateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine(CarrierDashboardWebPartUserControl _Parent, InterfaceState _state)
        : base(_state)
      {
        Parent = _Parent;
      }
      public LocalStateMachineEngine(CarrierDashboardWebPartUserControl _Parent)
        : base()
      {
        Parent = _Parent;
      }
      #endregion

      #region abstract implementation
      protected override void UpdateShowShipping(ShippingInterconnectionData _shipping)
      {
        Parent.m_ShippingHiddenField.Value = _shipping.ID;
        try
        {
          using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
          {
            ShippingOperationInbound _sppng = ShippingOperationInbound.GetAtIndex(edc, _shipping.ID.String2Int());
            ShippingOperationOutbound _so = _sppng as ShippingOperationOutbound;
            if (_so != null)
            {
              Parent.m_EstimateDeliveryTime.SelectedDate = _so.EstimateDeliveryTime.HasValue ? _so.EstimateDeliveryTime.Value : DateTime.Now;
              Parent.SelectEscort(_so.SecurityEscort);
              Parent.SelecrRoute(_so.Route);
            }
            Parent.m_EstimateDeliveryTime.SelectedDate = _shipping.EstimateDeliveryTime;
            TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetShippingTimeSlot(edc, _shipping.ID);
            List<LoadDescription> _ld = LoadDescription.GetForShipping(edc, _shipping.ID);
            Parent.ShowTimeSlot(_cts);
            string _ldLabel = String.Empty;
            foreach (var _item in _ld)
              _ldLabel += _item.Tytuł + "; ";
            Parent.m_DocumentTextBox.TextBoxTextProperty(_ldLabel, true);
          }
        }
        catch (Exception ex)
        {
          Parent.m_TimeSlotTextBox.TextBoxTextProperty(ex.Message, true);
          Parent.m_DocumentTextBox.TextBoxTextProperty(ex.Message, true);
        }
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearUserInterface();
      }
      protected override void SetEnabled(ButtonsSet _buttons)
      {
        Parent.SetEnabled(_buttons);
      }
      protected override void ShowTimeSlot(TimeSlotInterconnectionData _data)
      {
        Parent.ShowTimeSlot(_data);
      }
      protected override void SMError(StateMachineEngine.InterfaceEvent _interfaceEvent)
      {
        Parent.Controls.Add(new LiteralControl
          (String.Format("State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString())));
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
        Parent.AbortShipping();
      }
      protected override void UpdateTimeSlot(TimeSlotInterconnectionData e)
      {
        Parent.m_TimeSlotHiddenField.Value = e.ID;
      }
      #endregion

      #region private
      private CarrierDashboardWebPartUserControl Parent { get; set; }
      #endregion
    }
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
      m_ShippingHiddenField.Value = String.Empty;
      m_TimeSlotTextBox.TextBoxTextProperty(String.Empty, true);
      m_TimeSlotHiddenField.Value = String.Empty;
      m_TruckRegistrationHiddenField.Value = String.Empty;
      m_WarehouseTextBox.TextBoxTextProperty(String.Empty, true);
      m_WarehouseHiddenField.Value = String.Empty;
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_EstimateDeliveryTime.SelectedDate = DateTime.Now;
      m_RouteDropDownList.Items.Clear();
      m_SecurityDropDownList.Items.Clear();
    }
    private void ShowTimeSlot(TimeSlotInterconnectionData _interconnectionData)
    {
      bool _same = m_TimeSlotHiddenField.Value.Equals(_interconnectionData.ID);
      if (_same)
        return;
      m_TimeSlotHiddenField.Value = _interconnectionData.ID;
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          TimeSlotTimeSlot _cts = TimeSlotTimeSlot.GetAtIndex(edc, _interconnectionData.ID, true);
          ShowTimeSlot(_cts);
          m_SaveButton.Enabled = true;
        }
      }
      catch (Exception ex)
      {
        m_TimeSlotTextBox.TextBoxTextProperty(ex.Message, true);
      }
    }
    private void ShowTimeSlot(TimeSlotTimeSlot _cts)
    {
      m_TimeSlotHiddenField.Value = _cts.Identyfikator.ToString();
      m_TimeSlotTextBox.TextBoxTextProperty(String.Format("{0:R}", _cts.StartTime), true);
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseTextBox.TextBoxTextProperty(_wrs.Tytuł, true);
    }
    private void SetVisible(StateMachineEngine.ButtonsSet _set)
    {
      _set &= m_VisibilityACL;
      m_AbortButton.Visible = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Visible = (_set & ButtonsSet.CancelOn) != 0;
      m_CommentsTextBox.Visible = (_set & ButtonsSet.CommentsOn) != 0;
      m_DockNumberTextBox.Visible = (_set & ButtonsSet.DockOn) != 0;
      m_DocumentTextBox.Visible = (_set & ButtonsSet.DocumentOn) != 0;
      m_EditButton.Visible = (_set & ButtonsSet.EditOn) != 0;
      m_NewShippingButton.Visible = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Visible = (_set & ButtonsSet.SaveOn) != 0;
      m_SecurityEscortLabel.Visible = (_set & ButtonsSet.SecurityEscortOn) != 0;
      m_TimeSlotTextBox.Visible = (_set & ButtonsSet.TimeSlotOn) != 0;
      m_WarehouseTextBox.Visible = (_set & ButtonsSet.WarehouseOn) != 0;
      m_EstimateDeliveryTime.Visible = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      SetRouteVisible((_set & ButtonsSet.RouteOn) != 0);
    }
    private void SetRouteVisible(bool _on)
    {
      m_RouteDropDownList.Visible = _on;
      m_RouteLabel.Visible = _on;
      m_CityDropDownList.Visible = _on;
      m_CityLabel.Visible = _on;
    }
    private void SetEnabled(StateMachineEngine.ButtonsSet _set)
    {
      _set &= m_EditbilityACL;
      m_AbortButton.Enabled = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Enabled = (_set & ButtonsSet.CancelOn) != 0;
      m_CommentsTextBox.Enabled = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Enabled = (_set & ButtonsSet.DocumentOn) != 0;
      m_DockNumberTextBox.Enabled = (_set & ButtonsSet.DockOn) != 0;
      m_EditButton.Enabled = (_set & ButtonsSet.EditOn) != 0;
      m_EstimateDeliveryTime.Enabled = (_set & ButtonsSet.EstimatedDeliveryTime) != 0;
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
      m_SecurityDropDownList.Enabled = (_set & ButtonsSet.SecurityEscortOn) != 0;
      m_TimeSlotTextBox.Enabled = false;
      m_WarehouseTextBox.Enabled = false;
      SetRouteEnabled((_set & ButtonsSet.RouteOn) != 0);
    }
    private void SetRouteEnabled(bool _on)
    {
      m_RouteDropDownList.Enabled = _on;
      m_CityDropDownList.Enabled = _on;
    }
    private void CreateShipping()
    {
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        Partner _prtnr = Partner.GetAtIndex(edc, m_ControlState.PartnerIndex);
        TimeSlotTimeSlot _ts = TimeSlotTimeSlot.GetAtIndex(edc, m_TimeSlotHiddenField.Value, true);
        ShippingOperationInbound _sp = null;
        if (m_DashboardType == GlobalDefinitions.Roles.OutboundOwner)
          _sp = new ShippingOperationInbound
          (
            String.Format("{0}", m_DocumentTextBox.Text),
            _prtnr,
            Entities.State.Creation,
            _ts.StartTime
          );
        else
          //TODO Escort and Route
          _sp = new ShippingOperationOutbound
          (
            m_EstimateDeliveryTime.SelectedDate,
            String.Format("{0}", m_DocumentTextBox.Text),
            _prtnr,
            Entities.State.Creation,
            _ts.StartTime
          );

        _ts.MakeBooking(_sp);
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text,
          ShippingIndex = _sp
        };
        edc.Shipping.InsertOnSubmit(_sp);
        edc.LoadDescription.InsertOnSubmit(_ld);
        edc.SubmitChanges();
      }
    }
    private void UpdateShipping()
    {
      if (!m_TimeSlotHiddenField.Value.String2Int().HasValue)
        return;
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        try
        {
          TimeSlotTimeSlot _newts = TimeSlotTimeSlot.GetAtIndex(edc, m_TimeSlotHiddenField.Value, true);
          ShippingOperationInbound _si = ShippingOperationInbound.GetAtIndex(edc, m_ShippingHiddenField.HiddenField2Int());
          TimeSlotTimeSlot _oldts = TimeSlotTimeSlot.GetShippingTimeSlot(edc, _si.Identyfikator);
          _newts.MakeBooking(_si);
          _oldts.ReleaseBooking();
          edc.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.Controls.Add(new LiteralControl(ex.Message));
        }
      }
    }
    private void SelectDefaultWarehouse()
    {
      //TODO throw new NotImplementedException();
    }
    private void PopulateDropDownLists()
    {
      PopulateCityDropDownLists();
      PopulateRouteAndSecurityDropDownLists();
    }
    private void PopulateCityDropDownLists()
    {
      if (!m_CityDropDownList.Visible)
        return;
      m_CityDropDownList.Items.Clear();
      m_CityDropDownList.Items.Add(new ListItem(" --- Select Destination ---", String.Empty));
      try
      {
        foreach (ListItem _item in from _idx in m_EDC.City select new ListItem(_idx.Tytuł, _idx.Identyfikator.IntToString()))
          m_CityDropDownList.Items.Add(_item);
      }
      catch (Exception ex)
      {
        string _tmplt = "The list boxes fill up process has been interrupted by error {0}.";
        Entities.Anons _msg = new Anons("PopulateDropDownLists", String.Format(_tmplt, ex.Message));
      }
    }
    private void PopulateRouteAndSecurityDropDownLists()
    {
      if (!m_RouteDropDownList.Visible)
        return;
      CityType _city = null;
      m_RouteDropDownList.Items.Clear();
      m_SecurityDropDownList.Items.Clear();
      m_RouteDropDownList.Items.Add(new ListItem(" --- Select forwarder ---", String.Empty));
      m_SecurityDropDownList.Items.Add(new ListItem(" --- Select security escort ---", String.Empty));
      try
      {
        if (!m_CityDropDownList.SelectedValue.IsNullOrEmpty())
          _city = CityType.GetdAtIndex(m_EDC, m_CityDropDownList.SelectedValue.String2Int());
        if (_city != null)
        {
          foreach (ListItem _li in from _rx in _city.Route select new ListItem(_rx.Tytuł, _rx.Identyfikator.Value.ToString()))
            m_RouteDropDownList.Items.Add(_li);
          foreach (ListItem _li in from _sx in _city.SecurityEscortCatalog orderby _sx.Tytuł descending select new ListItem(_sx.Tytuł, _sx.Identyfikator.Value.ToString()))
            m_SecurityDropDownList.Items.Add(_li);
        }
        else
        {
          foreach (ListItem _li in from _idx in m_EDC.Route orderby _idx.Tytuł descending select new ListItem(_idx.Tytuł, _idx.Identyfikator.IntToString()))
            m_RouteDropDownList.Items.Add(_li);
          foreach (ListItem _li in from _idx in m_EDC.SecurityEscortCatalog orderby _idx.Tytuł descending select new ListItem(_idx.Tytuł, _idx.Identyfikator.IntToString()))
            m_SecurityDropDownList.Items.Add(_li);
        }

      }
      catch (Exception ex)
      {
        string _tmplt = "The list boxes fill up process has been interrupted by error {0}.";
        Entities.Anons _msg = new Anons("PopulateRouteAndSecurityDropDownLists", String.Format(_tmplt, ex.Message));
      }
    }
    private void AbortShipping()
    {
      //TODO throw new NotImplementedException();
    }
    private void SelectEscort(ShippingDriversTeam shippingDriversTeam)
    {
      //TODO throw new NotImplementedException();
    }
    internal void SelectEscort(SecurityEscortCatalog securityEscortCatalog)
    {
      //TODO throw new NotImplementedException();
    }

    internal void SelecrRoute(Route route)
    {
      //TODO throw new NotImplementedException();
    }
    #endregion

    #region Eveny handlers
    private void EnableSave(object sender, EventArgs e)
    {
      m_SaveButton.Enabled = true;
    }
    private void m_CityDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
      PopulateRouteAndSecurityDropDownLists();
    }
    #endregion
    #region variables
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private MyControlState m_ControlState = new MyControlState();
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ButtonsSet m_AllButtons = (StateMachineEngine.ButtonsSet)int.MaxValue;
    #endregion

    #endregion

  }
}
