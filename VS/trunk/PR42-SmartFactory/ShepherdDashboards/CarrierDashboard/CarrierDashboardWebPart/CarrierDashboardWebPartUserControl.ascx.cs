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
              ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.NewOn ^ ButtonsSet.RouteOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            break;
          case GlobalDefinitions.Roles.Vendor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
            m_EditbilityACL = m_AllButtons ^ ButtonsSet.EstimatedDeliveryTime ^ ButtonsSet.RouteOn ^ ButtonsSet.SecurityEscortOn;
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
      #region state fields
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      public string PartnerIndex = null;
      public string Route = string.Empty;
      public string Security = string.Empty;
      public string ShippingID = String.Empty;
      public bool Outbound = false;
      public bool HasChanges = false;
      public bool CommentsTextBoxChanged = false;
      public bool EstimateDeliveryTimeChanged = false;
      #endregion

      #region public methods
      internal void MarkCommentsTextBoxChanged()
      {
        CommentsTextBoxChanged = true;
        HasChanges = true;
      }
      internal void MarkEstimateDeliveryTimeChanged()
      {
        EstimateDeliveryTimeChanged = true;
        HasChanges = true;
      }
      internal void ClearModifications()
      {
        CommentsTextBoxChanged = false;
        EstimateDeliveryTimeChanged = false;
      }
      internal void ClearShippingID()
      {
        ShippingID = String.Empty;
        Outbound = false;
      }
      #endregion
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    private EntitiesDataContext m_EDC = null;
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
      m_EstimateDeliveryTimeDateTimeControl.DateChanged += new EventHandler(m_EstimateDeliveryTime_DateChanged);
      m_CommentsTextBox.TextChanged += new EventHandler(m_CommentsTextBox_TextChanged);
      m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
      m_NewShippingButton.Click += new EventHandler(m_StateMachineEngine.NewShippingButton_Click);
      m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
      m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
      m_AbortButton.Click += new EventHandler(m_StateMachineEngine.AbortButton_Click);
      m_AcceptButton.Click += new EventHandler(m_AcceptButton_Click);
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
      protected override void UpdateShowShipping(ShippingInterconnectionData _shipping)
      {
        Parent.m_ControlState.ShippingID = _shipping.ID;
        try
        {
          ShippingOperationInbound _sppng = ShippingOperationInbound.GetAtIndex(Parent.m_EDC, _shipping.ID.String2Int());
          ShippingOperationOutbound _so = _sppng as ShippingOperationOutbound;
          if (_so != null)
          {
            Parent.m_ControlState.Outbound = true;
            Parent.m_DocumentLabel.Text = Parent.m_DeliveryNoLabetText;
            Parent.m_EstimateDeliveryTimeDateTimeControl.SelectedDate = _so.EstimateDeliveryTime.HasValue ? _so.EstimateDeliveryTime.Value : DateTime.Now;
            Parent.m_RouteLabel.Text = _so.Route != null ? _so.Route.Tytuł : String.Empty;
            Parent.m_SecurityEscortLabel.Text = _so.SecurityEscort != null ? _so.SecurityEscort.Tytuł : string.Empty;
            Parent.m_EstimateDeliveryTimeDateTimeControl.SelectedDate = _shipping.EstimateDeliveryTime;
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
      protected override void ShowRoute(RouteInterconnectionnData e)
      {
        Parent.m_RouteLabel.Text = e.Title;
        Parent.m_ControlState.Route = e.ID;
      }
      protected override void ShowSecurityEscortCatalog(SecurityEscortCatalogInterconnectionData e)
      {
        Parent.m_SelectedSecurityEscortLabel.Text = e.Title;
        Parent.m_ControlState.Security = e.ID;
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
        Parent.m_TimeSlotHiddenField.Value = e.ID;
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
      m_ControlState.ClearShippingID();
      m_TimeSlotTextBox.TextBoxTextProperty(String.Empty, true);
      m_TimeSlotHiddenField.Value = String.Empty;
      m_TruckRegistrationHiddenField.Value = String.Empty;
      m_WarehouseTextBox.TextBoxTextProperty(String.Empty, true);
      m_WarehouseHiddenField.Value = String.Empty;
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_EstimateDeliveryTimeDateTimeControl.SelectedDate = DateTime.Now;
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
    private void CreateShipping()
    {
      try
      {
        Partner _prtnr = Partner.GetAtIndex(m_EDC, m_ControlState.PartnerIndex);
        TimeSlotTimeSlot _ts = TimeSlotTimeSlot.GetAtIndex(m_EDC, m_TimeSlotHiddenField.Value, true);
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
          if (!m_ControlState.Route.IsNullOrEmpty())
            _spo.Route = (from _rx in m_EDC.Route
                          where _rx.Identyfikator == m_ControlState.Route.String2Int()
                          select _rx).First();
          if (!m_ControlState.Security.IsNullOrEmpty())
            _spo.SecurityEscort = (from _sx in m_EDC.SecurityEscortCatalog
                                   where _sx.Identyfikator == m_ControlState.Security.String2Int()
                                   select _sx).First();
          if (m_ControlState.EstimateDeliveryTimeChanged)
            _spo.EstimateDeliveryTime = m_EstimateDeliveryTimeDateTimeControl.SelectedDate;
        }
        //TODO _sp.CancelationReason. = m_CommentsTextBox.Text; wait till: http://itrserver/Bugs/BugDetail.aspx?bid=3018
        _ts.MakeBooking(_sp);
        LoadDescription _ld = new LoadDescription()
        {
          Tytuł = m_DocumentTextBox.Text,
          ShippingIndex = _sp
        };
        m_EDC.Shipping.InsertOnSubmit(_sp);
        m_EDC.LoadDescription.InsertOnSubmit(_ld);
        m_EDC.SubmitChanges();
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
            TimeSlot _ts = (from _tsx in _si.TimeSlot orderby _tsx.StartTime.Value descending select _tsx).First();
            ((TimeSlotTimeSlot)_ts).ReleaseBooking();
            ReportAlert();
            break;
          case State.Confirmed:
            ReportAlert();
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
      }
      catch (Exception ex)
      {
        ReportException("AbortShipping", ex);
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
          ShippingOperationInbound _si = ShippingOperationInbound.GetAtIndex(edc, m_ControlState.ShippingID.String2Int());
          TimeSlotTimeSlot _oldts = TimeSlotTimeSlot.GetShippingTimeSlot(edc, _si.Identyfikator);
          _newts.MakeBooking(_si);
          _oldts.ReleaseBooking();
          edc.SubmitChanges();
        }
        catch (Exception ex)
        {
          m_StateMachineEngine.ExceptionCatched(m_EDC, "UpdateShipping", ex.Message);
        }
      }
    }
    private void ReportException(string _source, Exception ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      m_StateMachineEngine.ExceptionCatched(m_EDC, _source, String.Format(_tmplt, ex.Message));
    }
    private void ReportAlert()
    {
      //TODO throw new NotImplementedException();
    }
    #endregion

    #region Eveny handlers
    void m_CommentsTextBox_TextChanged(object sender, EventArgs e)
    {
      m_ControlState.CommentsTextBoxChanged = true;
    }
    void m_EstimateDeliveryTime_DateChanged(object sender, EventArgs e)
    {
      m_ControlState.EstimateDeliveryTimeChanged = true; ;
    }
    #endregion

    #region variables
    private const string m_DeliveryNoLabetText = "Delivery No";
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private MyControlState m_ControlState = new MyControlState();
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ControlsSet m_AllButtons = (StateMachineEngine.ControlsSet)int.MaxValue;
    #endregion

    #endregion
  }
}
