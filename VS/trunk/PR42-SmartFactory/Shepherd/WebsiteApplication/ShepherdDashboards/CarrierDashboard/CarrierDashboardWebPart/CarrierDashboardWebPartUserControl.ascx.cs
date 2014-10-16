//<summary>
//  Title   : class CarrierDashboardWebPartUserControl
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  using Microsoft.SharePoint;
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
    internal void SetInterconnectionData(Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        try
        {
          switch (item.Key)
          {
            case InterconnectionData.ConnectionSelector.ShippingInterconnection:
              new ShippingInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            case InterconnectionData.ConnectionSelector.TimeSlotInterconnection:
              new TimeSlotInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            case InterconnectionData.ConnectionSelector.PartnerInterconnection:
              new PartnerInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            case InterconnectionData.ConnectionSelector.CityInterconnection:
              new CityInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            case InterconnectionData.ConnectionSelector.RouteInterconnection:
              new RouteInterconnectionnData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            case InterconnectionData.ConnectionSelector.SecurityEscortCatalogInterconnection:
              new SecurityEscortCatalogInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], m_StateMachineEngine.NewDataEventHandler);
              break;
            default:
              break;
          }
        }
        catch (Exception ex)
        {
          Anons.ReportException(EDC, "SetInterconnectionData at: " + item.Key.ToString(), ex);
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
        ButtonsSet _inbound = m_AllButtons ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.CityOn ^ ButtonsSet.EstimatedDeliveryTimeOn ^ ButtonsSet.CoordinatorPanelOn ^ ButtonsSet.PartnerOn ^ ButtonsSet.LoadingUnloadingTimeOn;
        switch (value)
        {
          case GlobalDefinitions.Roles.OutboundOwner:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.CoordinatorPanelOn ^ ButtonsSet.OperatorControlsOn ^ ButtonsSet.ContainerNoOn ^ ButtonsSet.PartnerOn ^ ButtonsSet.LoadingUnloadingTimeOn;
            m_EditbilityACL = m_VisibilityACL ^ ButtonsSet.PartnerOn;
            m_DocumentLabel.Text = m_LabetTextLike_DeliveryNo;
            m_ShowDocumentLabel = ShowDocumentLabelOutbound;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Coordinator:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.OperatorControlsOn ^ ButtonsSet.PartnerOn;
            m_EditbilityACL = m_VisibilityACL;
            m_ShowDocumentLabel = ShowDocumentLabelDefault;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Supervisor:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.NewOn ^ ButtonsSet.AbortOn ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.CityOn ^ ButtonsSet.CoordinatorPanelOn ^ ButtonsSet.PartnerOn;
            m_EditbilityACL = m_VisibilityACL;
            m_ShowDocumentLabel = ShowDocumentLabelDefault;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.InboundOwner:
            m_VisibilityACL = (_inbound ^ ButtonsSet.OperatorControlsOn) | ButtonsSet.PartnerOn;
            m_EditbilityACL = m_VisibilityACL;
            m_ShowDocumentLabel = ShowDocumentLabelInboundVendor;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Operator:
            m_VisibilityACL = (_inbound ^ ButtonsSet.AbortOn ^ ButtonsSet.NewOn) | ButtonsSet.OperatorControlsOn | ButtonsSet.LoadingUnloadingTimeOn;
            m_EditbilityACL = m_VisibilityACL ^ ButtonsSet.CommentsOn;
            m_ShowDocumentLabel = ShowDocumentLabelDefault;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Vendor:
            m_VisibilityACL = _inbound ^ ButtonsSet.OperatorControlsOn;
            m_EditbilityACL = m_VisibilityACL;
            m_SecurityEscortHeaderLabel.Text = m_LabetTextLike_Vendor;
            m_ShowDocumentLabel = ShowDocumentLabelInboundVendor;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Guard:
            m_VisibilityACL = ButtonsSet.CommentsOn | ButtonsSet.DocumentOn | ButtonsSet.WarehouseOn | ButtonsSet.TimeSlotOn;
            m_EditbilityACL = 0;
            m_ShowDocumentLabel = ShowDocumentLabelDefault;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Forwarder:
            m_VisibilityACL = m_AllButtons ^ ButtonsSet.CoordinatorPanelOn ^ ButtonsSet.TransportUnitOn ^ ButtonsSet.OperatorControlsOn ^
              ButtonsSet.NewOn ^ ButtonsSet.AbortOn ^ ButtonsSet.PartnerOn ^ ButtonsSet.LoadingUnloadingTimeOn;
            m_EditbilityACL = m_VisibilityACL;
            m_ShowDocumentLabel = ShowDocumentLabelForwarder;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.Escort:
            m_VisibilityACL = ButtonsSet.TimeSlotOn | ButtonsSet.CoordinatorPanelOn | ButtonsSet.DocumentOn;
            m_EditbilityACL = m_VisibilityACL;
            m_ShowDocumentLabel = ShowDocumentLabelEscort;
            m_ShowDocumentLabel(null);
            break;
          case GlobalDefinitions.Roles.None:
            m_VisibilityACL = m_AllButtons;
            m_EditbilityACL = 0;
            m_ShowDocumentLabel = ShowDocumentLabelDefault;
            m_ShowDocumentLabel(null);
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
      public bool TimeSlotIsDouble = false;
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      internal StateMachineEngine.ControlsSet SetEnabled = 0;
      public bool TimeSlotChanged = false;
      public bool WarehouseEndTimeChanged = false;
      public bool WarehouseEndOperation = false;
      public bool WarehouseStartTimeChanged = false;
      public bool ETATimeChanged = false;
      public DateTime? WarehouseEndTime = new Nullable<DateTime>();
      public DateTime? WarehouseStartTime = new Nullable<DateTime>();
      public DateTime? ETATime = new Nullable<DateTime>();
      #endregion

      #region public
      internal void ClearShipping()
      {
        PartnerID = String.Empty;
        RouteID = String.Empty;
        CityID = String.Empty;
        SecurityCatalogID = String.Empty;
        TimeSlotID = String.Empty;
        TimeSlotChanged = false;
        WarehouseEndTime = new Nullable<DateTime>();
        WarehouseStartTime = new Nullable<DateTime>();
        ETATime = new Nullable<DateTime>();
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
      try
      {
        if (!IsPostBack)
        {
          SetVisible(m_AllButtons);
          m_StateMachineEngine.InitMahine();
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownSelect".GetShepherdLocalizedString(), "-1") { Selected = true });
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownUnacceptable".GetShepherdLocalizedString(), ((int)TrailerCondition._1Unexceptable).ToString()));
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownBad".GetShepherdLocalizedString(), ((int)TrailerCondition._2).ToString()));
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownPoor".GetShepherdLocalizedString(), ((int)TrailerCondition._3).ToString()));
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownGood".GetShepherdLocalizedString(), ((int)TrailerCondition._4).ToString()));
          m_TrailerConditionDropdown.Items.Add(new ListItem("TrailerConditionDropdownExcellent".GetShepherdLocalizedString(), ((int)TrailerCondition._5Excellent).ToString()));
          m_TransportUnitTypeDropDownList.DataSource = from _idx in EDC.TransportUnitType
                                                       orderby _idx.Title ascending
                                                       select new { Title = _idx.Title, Index = _idx.Id };
          m_TransportUnitTypeDropDownList.DataTextField = "Title";
          m_TransportUnitTypeDropDownList.DataValueField = "Index";
          m_TransportUnitTypeDropDownList.DataBind();
          m_TransportUnitTypeDropDownList.SelectedIndex = 0;
        }
        m_EstimateDeliveryTimeDateTimeControl.LocaleId = CultureInfo.CurrentCulture.LCID;
        m_WarehouseEndTimeControl.LocaleId = CultureInfo.CurrentCulture.LCID;
        m_WarehouseStartTimeControl.LocaleId = CultureInfo.CurrentCulture.LCID;
        m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_NewShippingButton.Click += new EventHandler(m_StateMachineEngine.NewShippingButton_Click);
        m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_AbortButton.Click += new EventHandler(m_StateMachineEngine.AbortButton_Click);
        m_CoordinatorEditCheckBox.CheckedChanged += new EventHandler(m_StateMachineEngine.m_CoordinatorEditCheckBox_CheckedChanged);
        m_SecurityRequiredChecbox.CheckedChanged += new EventHandler(m_StateMachineEngine.m_SecurityRequiredChecbox_CheckedChanged);
        m_LoadingUnloadingTime.Enabled = true;
      }
      catch (Exception ex)
      {
        ActionResult _ar = new ActionResult();
        _ar.AddException(ex);
        this.ShowActionResult(_ar);
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
      try
      {
        m_StateLiteral.Text = ("InterfaceState" + m_ControlState.InterfaceState.ToString()).GetShepherdLocalizedString();
        SetEnabled(m_ControlState.SetEnabled);
        m_WarehouseEndTimeControl.SetTimePicker(m_ControlState.WarehouseEndTime);
        m_WarehouseStartTimeControl.SetTimePicker(m_ControlState.WarehouseStartTime);
        m_EstimateDeliveryTimeDateTimeControl.SetTimePicker(m_ControlState.ETATime);
        if (m_ControlState.ShippingID.IsNullOrEmpty())
        {
          m_AbortButton.Enabled = false;
          m_EditButton.Enabled = false;
          m_AbortButton.Enabled = false;
          m_CoordinatorPanel.Enabled = false;
          m_CoordinatorEditCheckBox.Checked = false;
          m_LoadingUnloadingTime.Enabled = false;
        }
        else
        {
          if (VendorFixed(CurrentShipping))
            m_AbortButton.Enabled = false;
          if (!CurrentShipping.IsEditable())
          {
            m_AbortButton.Enabled = false;
            m_CoordinatorPanel.Enabled = false;
            m_CoordinatorEditCheckBox.Checked = false;
          }
        }
        m_SecurityRequiredChecbox.Enabled = m_CoordinatorEditCheckBox.Checked;
      }
      catch (Exception ex)
      {
        using (EntitiesDataContext _edc = new EntitiesDataContext())
          Anons.ReportException(_edc, "OnPreRender", new Exception(String.Format("The operation OnPreRender has been interrupted by the error: '{0}'", ex.Message)));
      }
      base.OnPreRender(e);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
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

      #region public
      internal override InterfaceState CurrentMachineState
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
        Parent.SetInterconnectionData(_partner);
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
        Parent.CancelShipping();
      }
      protected override void ClearUserInterface()
      {
        Parent.NewShippment();
      }
      protected override void SetEnabled(ControlsSet _buttons)
      {
        Parent.m_ControlState.SetEnabled = _buttons;
        Parent.m_ShowDocumentLabel(null);
      }
      protected override void SMError(StateMachineEngine.InterfaceEvent _interfaceEvent)
      {
        Parent.Controls.Add(new LiteralControl
          (String.Format("State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString())));
      }
      protected override void ShowActionResult(ActionResult _rslt)
      {
        Parent.ShowActionResult(_rslt);
      }
      protected override void UpdateEscxortRequired()
      {
        Parent.UpdateEscxortRequired();
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
      m_ControlState.ShippingID = _interconnectionData.ID;
      ActionResult _rslt = ShowShipping();
      if (!_rslt.Valid)
        ShowActionResult(_rslt);
    }
    private bool VendorFixed(Shipping CurrentShipping)
    {
      return (m_DashboardType == GlobalDefinitions.Roles.Vendor) && (CurrentShipping != null) && CurrentShipping.ForFuture();
    }
    private void SetInterconnectionData(TimeSlotInterconnectionData _interconnectionData)
    {
      if (m_ControlState.TimeSlotID == _interconnectionData.ID)
        return;
      try
      {
        if (VendorFixed(CurrentShipping))
        {
          ActionResult _rst = new ActionResult();
          _rst.AddMessage("SetInterconnectionDataItIsTooLate".GetShepherdLocalizedString());
          ShowActionResult(_rst);
          return;
        }
        TimeSlotTimeSlot _cts = Element.GetAtIndex(EDC.TimeSlot, _interconnectionData.ID);
        Debug.Assert(_cts.Occupied.Value == Occupied.Free, "SetInterconnectionDataTimeSlotInUse".GetShepherdLocalizedString());
        m_ControlState.TimeSlotID = _interconnectionData.ID;
        m_ControlState.TimeSlotIsDouble = _interconnectionData.IsDouble;
        m_ControlState.TimeSlotChanged = true;
        Show(_cts, true, _interconnectionData.IsDouble);
      }
      catch (Exception ex)
      {
        Anons.ReportException(EDC, "SetInterconnectionData-TimeSlotInterconnectionData", ex);
      }
    }
    private void SetInterconnectionData(CityInterconnectionData _city)
    {
      m_ControlState.CityID = _city.ID;
      m_CityLabel.Text = _city.Title;
      ClearRoute();
    }
    internal void SetInterconnectionData(PartnerInterconnectionData _partner)
    {
      m_ControlState.PartnerID = _partner.ID;
      m_PartnerHeaderLabel.Text = m_LabetTextLike_Vendor;
      m_PartnerLabel.Text = _partner.Title;
    }
    private void SetInterconnectionData(RouteInterconnectionnData _route)
    {
      if (!m_CoordinatorEditCheckBox.Checked)
        return;
      m_ControlState.RouteID = _route.ID;
      m_RouteLabel.Text = _route.Title;
    }
    private void SetInterconnectionData(SecurityEscortCatalogInterconnectionData _Escort)
    {
      if (!m_CoordinatorEditCheckBox.Checked || !m_SecurityRequiredChecbox.Checked)
        return;
      m_ControlState.SecurityCatalogID = _Escort.ID;
      m_SecurityEscortLabel.Text = _Escort.Title;
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
    private void NewShippment()
    {
      ClearUserInterface();
      m_ControlState.ShippingID = String.Empty;
    }
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void ClearUserInterface()
    {
      m_ControlState.ClearShipping();
      m_TimeSlotTextBox.Text = String.Empty;
      m_WarehouseLabel.Text = String.Empty;
      m_DocumentTextBox.TextBoxTextProperty(String.Empty, true);
      m_CommentsTextBox.TextBoxTextProperty(String.Empty, false);
      m_TransportUnitTypeDropDownList.SelectedIndex = -1;
      m_DockNumberTextBox.Text = String.Empty;
      m_TrailerConditionCommentsTextBox.Text = String.Empty;
      if (m_TrailerConditionDropdown.Visible && m_TrailerConditionDropdown.Items.Count > 0)
        m_TrailerConditionDropdown.SelectedIndex = 0;
      m_CityLabel.Text = String.Empty;
      m_PartnerLabel.Text = String.Empty;
      m_ContainerNoTextBox.Text = String.Empty;
      ClearCoordinatorPanel();
    }
    private void ClearCoordinatorPanel()
    {
      m_CoordinatorEditCheckBox.Checked = false;
      m_SecurityRequiredChecbox.Checked = false;
      m_RouteLabel.Text = String.Empty;
      m_SecurityEscortLabel.Text = String.Empty;
    }
    #endregion

    #region Show
    internal void UpdateEscxortRequired()
    {
      if (!m_SecurityRequiredChecbox.Checked)
        ClearSecurityEscor();
    }
    private ActionResult ShowShipping()
    {
      string _at = "ClearUserInterface";
      ClearUserInterface();
      ActionResult _rsult = new ActionResult();
      try
      {
        if (m_ControlState.ShippingID.IsNullOrEmpty())
        {
          _rsult.AddLabel("Interconnection error; ShippingID");
          return _rsult;
        }
        _at = "SendShippingData";
        SendShippingData(CurrentShipping);
        _at = "_timeSlot = ( from _ts in this.EDC.TimeSlot";
        TimeSlotTimeSlot _timeSlot = CurrentShipping.OccupiedTimeSlots(EDC).FirstOrDefault();
        _at = "m_ControlState.TimeSlotChanged";
        m_ControlState.TimeSlotChanged = false;
        if (_timeSlot == null)
          m_ControlState.TimeSlotID = String.Empty;
        else
        {
          m_ControlState.TimeSlotID = _timeSlot.Id.IntToString();
          _at = "Show( _timeSlot, CurrentShipping";
          Show(_timeSlot, CurrentShipping.IsEditable(), _timeSlot.IsDouble.GetValueOrDefault(false));
        }
        m_CommentsTextBox.TextBoxTextProperty(CurrentShipping.CancelationReason, false);
        m_ContainerNoTextBox.TextBoxTextProperty(CurrentShipping.ContainerNo, false);
        m_ShowDocumentLabel(CurrentShipping);
        _at = "ShowOperatorStuff";
        ShowOperatorStuff(CurrentShipping);
        ShowLoadingUnloadingTimePanel(CurrentShipping);
        if (CurrentShipping.IsOutbound.Value)
        {
          _at = "ETATime";
          m_ControlState.ETATime = CurrentShipping.EstimateDeliveryTime;
          _at = "Shipping2RouteTitle";
          Show(CurrentShipping.Shipping2RouteTitle);
          _at = "Show( CurrentShipping.SecurityEscortCatalogTitle";
          Show(CurrentShipping.SecurityEscortCatalogTitle);
          _at = "Show( CurrentShipping.Shipping2TransportUnitType";
          Show(CurrentShipping.Shipping2TransportUnitType);
          _at = "Show( CurrentShipping.Shipping2City";
          Show(CurrentShipping.Shipping2City);
        }
        else
          Show(CurrentShipping.PartnerTitle);
      }
      catch (Exception ex)
      {
        string _src = String.Format("ShowShipping at {0}", _at);
        Anons.ReportException(EDC, _src, ex);
        _rsult.AddException(ex);
      }
      return _rsult;
    }
    private void ShowLoadingUnloadingTimePanel(Shipping shipping)
    {
      m_ControlState.WarehouseEndTime = shipping.WarehouseEndTime;
      m_ControlState.WarehouseStartTime = shipping.WarehouseStartTime;
    }
    private void ShowOperatorStuff(Shipping _sppng)
    {
      if (m_TrailerConditionDropdown.Visible && _sppng.TrailerCondition.HasValue)
        SelectDropdown(m_TrailerConditionDropdown, (int)_sppng.TrailerCondition.Value, 0);
      if (m_DockNumberTextBox.Visible)
        m_DockNumberTextBox.Text = _sppng.DockNumber;
      if (m_TrailerConditionCommentsTextBox.Visible)
        m_TrailerConditionCommentsTextBox.Text = _sppng.TrailerConditionComments;
    }
    private void Show(TimeSlotTimeSlot _cts, bool _isEditable, bool _isDouble)
    {
      if (_cts == null)
      {
        m_TimeSlotTextBox.Text = _isEditable ? "ShowSelectTimeSlot".GetShepherdLocalizedString() : "ShowShippingLocked".GetShepherdLocalizedString();
        return;
      }
      m_TimeSlotTextBox.Text = String.Format("{0}{1}{2}", _cts.StartTime.Value.ToString(CultureInfo.CurrentCulture), _isEditable ? "" : " ! ", _isDouble ? "x2" : "");
      Warehouse _wrs = _cts.GetWarehouse();
      m_WarehouseLabel.Text = _wrs.Title;
    }
    private void Show(CityType _city)
    {
      if (_city == null)
        return;
      m_CityLabel.Text = _city.Title;
      m_ControlState.CityID = _city.Id.IntToString();
    }
    private void Show(Route _route)
    {
      if (_route == null)
        return;
      m_RouteLabel.Text = _route.Title;
      m_ControlState.RouteID = _route.Id.IntToString();
    }
    private void Show(Partner _partner)
    {
      if (_partner == null)
        return;
      m_PartnerHeaderLabel.Text = m_LabetTextLike_Vendor;
      m_PartnerLabel.Text = _partner.Title;
      m_ControlState.PartnerID = _partner.Id.IntToString();
    }
    private void Show(SecurityEscortCatalog _security)
    {
      m_SecurityEscortHeaderLabel.Text = m_LabetTextLike_SecurityEscort;
      if (_security == null)
        return;
      m_SecurityEscortLabel.Text = _security.Title;
      m_ControlState.SecurityCatalogID = _security.Id.IntToString();
      m_SecurityRequiredChecbox.Checked = true;
    }
    private void Show(TranspotUnit _unitType)
    {
      if (_unitType == null)
        return;
      SelectDropdown(m_TransportUnitTypeDropDownList, _unitType.Id.Value, -1);
    }
    private void SelectDropdown(DropDownList _list, int _value, int _default)
    {
      _list.SelectedIndex = -1;
      foreach (ListItem _item in _list.Items)
        if (_item.Value.String2Int() == _value)
        {
          _item.Selected = true;
          return;
        }
      if (_default >= 0 && _default < _list.Items.Count)
        _list.SelectedIndex = _default;
    }
    #endregion

    #region Shipping management
    private ActionResult CreateShipping()
    {
      ActionResult _rsult = new ActionResult();
      string _checkPoint = "CreateShipping";
      try
      {
        if (m_DocumentTextBox.Text.IsNullOrEmpty())
          _rsult.AddLabel(m_DocumentLabel.Text);
        if (m_ControlState.TimeSlotID.IsNullOrEmpty())
          _rsult.AddLabel(m_TimeSlotLabel.Text);
        if (!_rsult.Valid)
          return _rsult;
        {
          _checkPoint = "Shipping.CreateShipping";
          Shipping _sppng = Shipping.CreateShipping(m_DashboardType == GlobalDefinitions.Roles.OutboundOwner);
          _sppng.EstimateDeliveryTime = m_EstimateDeliveryTimeDateTimeControl.SelectedDate;
          _checkPoint = "UpdateShipping";
          UpdateShipping(_sppng, _rsult, EDC);
          if (!_rsult.Valid)
            return _rsult;
          _checkPoint = "BookTimeSlots";
          List<TimeSlotTimeSlot> _Tss = TimeSlotTimeSlot.BookTimeSlots(EDC, m_ControlState.TimeSlotID, m_ControlState.TimeSlotIsDouble);
          _checkPoint = "Shipping.MakeBooking";
          _sppng.MakeBooking(_Tss, m_ControlState.TimeSlotIsDouble);
          m_ControlState.ShippingID = String.Empty;
          _checkPoint = "LoadDescription";
          LoadDescription _ld = new LoadDescription()
          {
            Title = m_DocumentTextBox.Text,
            DeliveryNumber = m_DocumentTextBox.Text,
            LoadDescription2ShippingIndex = _sppng,
            LoadDescription2PartnerTitle = _sppng.PartnerTitle
          };
          EDC.Shipping.InsertOnSubmit(_sppng);
          EDC.LoadDescription.InsertOnSubmit(_ld);
          _checkPoint = "SubmitChanges #1";
          EDC.SubmitChanges();
          _checkPoint = "UpdateTitle";
          _sppng.UpdateLoadDescriptionInfo(EDC, null, null);
          _sppng.UpdateTitle();
          _checkPoint = "CalculateState";
          _sppng.CalculateState(EDC, x => { _checkPoint = x; });
          _checkPoint = "SubmitChanges #2";
          EDC.SubmitChanges();
          _checkPoint = "SendShippingData";
          SendShippingData(_sppng);
        }
      }
      catch (ChangeConflictException)
      {
        _rsult.AddLabel("Change conflict try again");
      }
      catch (TimeSlotTimeSlot.TimeSlotException _tse)
      {
        _rsult.AddLabel(_tse.Message);
      }
      catch (Exception ex)
      {
        _rsult.AddException(ex);
        Anons.ReportException(EDC, "CreateShipping at: " + _checkPoint, ex);
      }
      return _rsult;
    }
    private ActionResult UpdateShipping()
    {
      ActionResult _rst = new ActionResult();
      string _checkPoint = "Starting";
      try
      {
        _checkPoint = "UpdateShipping";
        UpdateShipping(CurrentShipping, _rst, EDC);
        _checkPoint = "UpdateTimeSlot";
        UpdateTimeSlot(CurrentShipping, _rst);
        _checkPoint = "CurrentShipping";
        CurrentShipping.CalculateState(EDC, x => { _checkPoint = x; });
        if (m_ControlState.WarehouseEndTimeChanged)
          CurrentShipping.WarehouseEndTime = m_ControlState.WarehouseEndTime;
        if (m_ControlState.WarehouseStartTimeChanged)
          CurrentShipping.WarehouseStartTime = m_ControlState.WarehouseStartTime;
        if (m_ControlState.WarehouseEndOperation)
          CurrentShipping.SetWarehouseEndTime();
        if (m_ControlState.ETATimeChanged)
          CurrentShipping.EstimateDeliveryTime = m_ControlState.ETATime;
        _checkPoint = "SubmitChanges";
        EDC.SubmitChanges();
        m_ControlState.WarehouseEndTimeChanged = false;
        m_ControlState.TimeSlotChanged = false;
        m_ControlState.WarehouseEndOperation = false;
        m_ControlState.ETATimeChanged = false;
      }
      catch (ChangeConflictException)
      {
        _rst.AddLabel("Change conflict - try again");
      }
      catch (TimeSlotTimeSlot.TimeSlotException _tse)
      {
        _rst.AddLabel(_tse.Message);
      }
      catch (Exception ex)
      {
        _rst.AddException(ex);
        Anons.ReportException(EDC, "UpdateShipping at " + _checkPoint, ex);
      }
      return _rst;
    }
    private void UpdateShipping(Shipping _sppng, ActionResult _rsult, EntitiesDataContext _EDC)
    {
      if (_sppng == null)
      {
        _rsult.AddLabel("Shipping".GetShepherdLocalizedString());
        return;
      }
      _sppng.CancelationReason = m_CommentsTextBox.Text;
      if (m_ContainerNoTextBox.Visible && m_ContainerNoTextBox.Enabled)
        _sppng.ContainerNo = m_ContainerNoTextBox.Text;
      UpdateTransportUnitType(_sppng, _EDC);
      UpdateOperatorPanel(_sppng);
      if (_sppng.IsOutbound.Value)
      {
        UpdateCity(_sppng, _rsult, _EDC);
        UpdateRoute(_sppng, _EDC);
        UpdateSecurityEscort(_sppng, _EDC);
      }
      else
        UpdateVendor(_sppng, _rsult, _EDC);
      return;
    }
    private void UpdateOperatorPanel(Shipping _sppng)
    {
      if (m_TrailerConditionCommentsTextBox.Enabled)
        _sppng.TrailerConditionComments = m_TrailerConditionCommentsTextBox.Text;
      if (m_TrailerConditionDropdown.Enabled && m_TrailerConditionDropdown.SelectedIndex > 0)
        _sppng.TrailerCondition = (TrailerCondition)m_TrailerConditionDropdown.SelectedValue.String2Int().Value;
      else
        _sppng.TrailerCondition = null;
      if (m_DockNumberTextBox.Enabled)
        _sppng.DockNumber = m_DockNumberTextBox.Text;
    }
    private void UpdateCity(Shipping _sppng, ActionResult _rsult, EntitiesDataContext _EDC)
    {
      if (m_ControlState.CityID.IsNullOrEmpty())
      {
        _rsult.AddLabel(m_CityHeaderLabel.Text);
        return;
      }
      _sppng.Shipping2City = Element.GetAtIndex(_EDC.City, m_ControlState.CityID);
    }
    private void UpdateTransportUnitType(Shipping _sppng, EntitiesDataContext _EDC)
    {
      if (!m_TransportUnitTypeDropDownList.Enabled)
        return;
      if (m_TransportUnitTypeDropDownList.SelectedIndex < 0)
      {
        _sppng.Shipping2TransportUnitType = null;
        return;
      }
      _sppng.Shipping2TransportUnitType = Element.GetAtIndex(_EDC.TransportUnitType, m_TransportUnitTypeDropDownList.SelectedValue);
    }
    private void UpdateTimeSlot(Shipping _shipping, ActionResult _rslt)
    {
      if (!m_ControlState.TimeSlotChanged)
        return;
      if (m_ControlState.TimeSlotID.IsNullOrEmpty())
      {
        _rslt.AddLabel(m_TimeSlotLabel.Text);
        return;
      }
      _shipping.ReleaseBooking(EDC);
      List<TimeSlotTimeSlot> timeSlots = TimeSlotTimeSlot.BookTimeSlots(EDC, m_ControlState.TimeSlotID, m_ControlState.TimeSlotIsDouble);
      _shipping.MakeBooking(timeSlots, m_ControlState.TimeSlotIsDouble);
    }
    private void UpdateSecurityEscort(Shipping _sipping, EntitiesDataContext _EDC)
    {
      if (!m_CoordinatorEditCheckBox.Checked)
        return;
      _sipping.ChangeEscort(Element.TryGetAtIndex<SecurityEscortCatalog>(_EDC.SecurityEscortRoute, m_ControlState.SecurityCatalogID), _EDC);
    }
    private void UpdateRoute(Shipping _sipping, EntitiesDataContext _EDC)
    {
      if (!m_CoordinatorEditCheckBox.Checked)
        return;
      _sipping.ChangeRout(Element.TryGetAtIndex<Route>(_EDC.Route, m_ControlState.RouteID), _EDC);
    }
    private void UpdateVendor(Shipping _sipping, ActionResult _rsult, EntitiesDataContext _EDC)
    {
      if (m_ControlState.PartnerID.IsNullOrEmpty())
      {
        _rsult.AddLabel(m_PartnerHeaderLabel.Text);
        return;
      }
      if ((_sipping.PartnerTitle != null) && (m_ControlState.PartnerID.String2Int() == _sipping.PartnerTitle.Id.Value))
        return;
      _sipping.PartnerTitle = Element.GetAtIndex(_EDC.Partner, m_ControlState.PartnerID);
    }
    private void CancelShipping()
    {
      try
      {
        Shipping _sppng = CurrentShipping;
        switch (_sppng.ShippingState.Value)
        {
          case ShippingState.Confirmed:
          case ShippingState.None:
          case ShippingState.Invalid:
          case ShippingState.Creation:
          case ShippingState.Delayed:
          case ShippingState.WaitingForCarrierData:
          case ShippingState.WaitingForConfirmation:
          case ShippingState.Underway:
            _sppng.ReleaseBooking(EDC);
            _sppng.ShippingState = ShippingState.Cancelation;
            break;
          case ShippingState.Completed:
          case ShippingState.Cancelation:
          case ShippingState.Canceled:
          default:
            break;
        }
        try
        {
          EDC.SubmitChanges();
        }
        catch (ChangeConflictException) { }
      }
      catch (Exception ex)
      {
        Anons.ReportException(EDC, "ChangeShippingState", ex);
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
        Anons.ReportException(EDC, "SendShippingData", ex);
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
      m_EditButton.Visible = (_set & ButtonsSet.EditOn) != 0;
      m_AbortButton.Visible = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Visible = (_set & ButtonsSet.CancelOn) != 0;
      m_NewShippingButton.Visible = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Visible = (_set & ButtonsSet.SaveOn) != 0;
      //outbound
      m_CityHeaderLabel.Visible = (_set & ButtonsSet.CityOn) != 0;
      m_CityLabel.Visible = (_set & ButtonsSet.CityOn) != 0;
      //TransportUnit
      m_TransportUnitTypeDropDownList.Visible = (_set & ButtonsSet.TransportUnitOn) != 0;
      m_TransportUnitTypeLabel.Visible = (_set & ButtonsSet.TransportUnitOn) != 0;
      //EstimateDeliveryTime
      m_EstimateDeliveryTimeLabel.Visible = (_set & ButtonsSet.EstimatedDeliveryTimeOn) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Visible = (_set & ButtonsSet.EstimatedDeliveryTimeOn) != 0;
      m_CoordinatorPanel.Visible = (_set & ButtonsSet.CoordinatorPanelOn) != 0;
      m_LoadingUnloadingTime.Visible = (_set & ButtonsSet.LoadingUnloadingTimeOn) != 0;
      //Operator
      m_DockNumberTextBox.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_DocNumberLabel.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      //TrailerCondition
      m_TrailerConditionDropdownLabel.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_TrailerConditionDropdown.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_TrailerConditionCommentsLabel.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_TrailerConditionCommentsTextBox.Visible = (_set & ButtonsSet.OperatorControlsOn) != 0;
      //Container
      m_ContainerNoLabel.Visible = (_set & ButtonsSet.ContainerNoOn) != 0;
      m_ContainerNoTextBox.Visible = (_set & ButtonsSet.ContainerNoOn) != 0;
      //
      m_PartnerHeaderLabel.Visible = (_set & ButtonsSet.PartnerOn) != 0;
      m_PartnerLabel.Visible = (_set & ButtonsSet.PartnerOn) != 0;
    }
    private void SetEnabled(StateMachineEngine.ControlsSet _set)
    {
      _set &= m_EditbilityACL;
      m_CommentsTextBox.Enabled = (_set & ButtonsSet.CommentsOn) != 0;
      m_DocumentTextBox.Enabled = (_set & ButtonsSet.DocumentOn) != 0;
      m_EstimateDeliveryTimeDateTimeControl.Enabled = (_set & ButtonsSet.EstimatedDeliveryTimeOn) != 0;
      m_LoadingUnloadingTime.Enabled = (_set & ButtonsSet.LoadingUnloadingTimeOn) != 0;
      m_TimeSlotTextBox.Enabled = false;
      m_TransportUnitTypeDropDownList.Enabled = (_set & ButtonsSet.TransportUnitOn) != 0;
      //Buttons
      m_AbortButton.Enabled = (_set & ButtonsSet.AbortOn) != 0;
      m_CancelButton.Enabled = (_set & ButtonsSet.CancelOn) != 0;
      m_EditButton.Enabled = (_set & ButtonsSet.EditOn) != 0;
      m_NewShippingButton.Enabled = (_set & ButtonsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & ButtonsSet.SaveOn) != 0;
      m_CoordinatorPanel.Enabled = (_set & ButtonsSet.CoordinatorPanelOn) != 0;
      if (!m_CoordinatorPanel.Enabled)
        m_CoordinatorEditCheckBox.Checked = false;
      //Operator
      m_DockNumberTextBox.Enabled = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_TrailerConditionCommentsTextBox.Enabled = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_TrailerConditionDropdown.Enabled = (_set & ButtonsSet.OperatorControlsOn) != 0;
      m_ContainerNoTextBox.Enabled = (_set & ButtonsSet.ContainerNoOn) != 0;
    }
    #endregion

    #region Reports
    private void ShowActionResult(ActionResult _rslt)
    {
      foreach (var item in _rslt)
        Parent.Controls.Add(new LiteralControl(item));
    }
    #endregion

    #region DocumentLabel
    private delegate void ShowDocumentLabel(Shipping _shppng);
    private ShowDocumentLabel m_ShowDocumentLabel;
    private void ShowDocumentLabelInboundVendor(Shipping _shppng)
    {
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentLabel.Text = m_LabetTextLike_ShippingNo;
          break;
        case InterfaceState.NewState:
          m_DocumentLabel.Text = m_LabetTextLike_PurchaseOrder;
          break;
        default:
          break;
      }
      if (_shppng == null)
        return;
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentTextBox.Text = _shppng.Title;
          break;
        case InterfaceState.NewState:
        default:
          break;
      }
    }
    private void ShowDocumentLabelOutbound(Shipping _shppng)
    {
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentLabel.Text = m_LabetTextLike_ShippingNo;
          break;
        case InterfaceState.NewState:
          m_DocumentLabel.Text = m_LabetTextLike_DeliveryNo;
          break;
        default:
          break;
      }
      if (_shppng == null)
        return;
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentTextBox.Text = _shppng.Title;
          break;
        case InterfaceState.NewState:
        default:
          break;
      }
    }
    private void ShowDocumentLabelForwarder(Shipping _shppng)
    {
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentLabel.Text = m_LabetTextLike_PurchaseOrder;
          break;
        case InterfaceState.NewState:
        default:
          m_DocumentLabel.Text = m_LabetTextLike_ShippingNo;
          break;
      }
      if (_shppng == null)
        return;
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentTextBox.Text = _shppng.Shipping2RouteTitle != null ? _shppng.Shipping2RouteTitle.GoodsHandlingPO : "";
          break;
        case InterfaceState.NewState:
        default:
          m_DocumentTextBox.Text = _shppng.Title;
          break;
      }
    }
    private void ShowDocumentLabelEscort(Shipping _shppng)
    {
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentLabel.Text = m_LabetTextLike_PurchaseOrder;
          break;
        case InterfaceState.NewState:
        default:
          m_DocumentLabel.Text = m_LabetTextLike_ShippingNo;
          break;
      }
      if (_shppng == null)
        return;
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
          m_DocumentTextBox.Text = _shppng.SecurityEscortCatalogTitle != null ? _shppng.SecurityEscortCatalogTitle.SecurityEscrotPO : "";
          break;
        case InterfaceState.NewState:
        default:
          m_DocumentTextBox.Text = _shppng.Title;
          break;
      }
    }
    private void ShowDocumentLabelDefault(Shipping _shppng)
    {
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          m_DocumentLabel.Text = m_LabetTextLike_ShippingNo;
          break;
      }
      if (_shppng == null)
        return;
      switch (m_StateMachineEngine.CurrentMachineState)
      {
        case InterfaceState.ViewState:
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          m_DocumentTextBox.Text = _shppng.Title;
          break;
      }
    }
    #endregion

    #region variables
    private string m_LabetTextLike_PurchaseOrder = "PO_No".GetShepherdLocalizedString();
    private string m_LabetTextLike_DeliveryNo = "DeliveryNumber".GetShepherdLocalizedString();
    private string m_LabetTextLike_Vendor = "Vendor".GetShepherdLocalizedString();
    private string m_LabetTextLike_SecurityEscort = "Escort".GetShepherdLocalizedString();
    private string m_LabetTextLike_ShippingNo = "ShippingNumber".GetShepherdLocalizedString();
    private ButtonsSet m_VisibilityACL;
    private ButtonsSet m_EditbilityACL;
    private ControlState m_ControlState = new ControlState(null);
    private GlobalDefinitions.Roles m_DashboardType = GlobalDefinitions.Roles.None;
    private const StateMachineEngine.ControlsSet m_AllButtons = (StateMachineEngine.ControlsSet)ulong.MaxValue;
    private event InterconnectionDataTable<Shipping>.SetDataEventArg m_ShippintInterconnectionEvent;
    #endregion

    #region EventHandlers
    /// <summary>
    /// Handles the DateChanged event of the m_WarehouseEndTimeControl control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void m_WarehouseEndTimeControl_DateChanged(object sender, EventArgs e)
    {
      m_ControlState.WarehouseEndTimeChanged = true;
      m_ControlState.WarehouseEndTime = m_WarehouseEndTimeControl.SelectedDate;
    }
    /// <summary>
    /// Handles the DateChanged event of the m_WarehouseStartTimeControl control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void m_WarehouseStartTimeControl_DateChanged(object sender, EventArgs e)
    {
      m_ControlState.WarehouseStartTimeChanged = true;
      m_ControlState.WarehouseStartTime = m_WarehouseStartTimeControl.SelectedDate;
    }
    /// <summary>
    /// Handles the DateChanged event of the m_EstimateDeliveryTimeDateTimeControl control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void m_EstimateDeliveryTimeDateTimeControl_DateChanged(object sender, EventArgs e)
    {
      m_ControlState.ETATimeChanged = true;
      m_ControlState.ETATime = m_EstimateDeliveryTimeDateTimeControl.SelectedDate;
    }
    /// <summary>
    /// Handles the Click event of the m_WarehouseEndTimeButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void m_WarehouseEndTimeButton_Click(object sender, EventArgs e)
    {
      m_ControlState.WarehouseEndTime = DateTime.Now;
      m_ControlState.WarehouseEndTimeChanged = true;
      m_ControlState.WarehouseEndOperation = true;
    }
    #endregion

    #region Entities management
    private EntitiesDataContext EDC
    {
      get
      {
        return DataContextManagementAutoDispose<EntitiesDataContext>.GetDataContextManagement(this).DataContext;
      }
    }
    private Shipping myShipping;
    private Shipping CurrentShipping
    {
      get
      {
        if (m_ControlState.ShippingID.IsNullOrEmpty())
          return null;
        if (myShipping == null)
          myShipping = Element.GetAtIndex<Shipping>(EDC.Shipping, m_ControlState.ShippingID);
        return myShipping;
      }
    }
    #endregion

    #endregion
  }
}
