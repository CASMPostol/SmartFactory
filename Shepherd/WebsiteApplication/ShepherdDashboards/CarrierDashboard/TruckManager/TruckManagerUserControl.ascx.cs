//<summary>
//  Title   : class TruckManagerUserControl
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TruckManager
{
  /// <summary>
  /// TruckManagerUserControl <see cref="UserControl"/>
  /// </summary>
  public partial class TruckManagerUserControl : UserControl
  {
    #region public
    internal void SetInterconnectionData(Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      if (_ProvidesDictionary.Keys.Contains(InterconnectionData.ConnectionSelector.TruckInterconnection))
        new TruckInterconnectionData().SetRowData
          (_ProvidesDictionary[InterconnectionData.ConnectionSelector.TruckInterconnection], m_StateMachineEngine.NewDataEventHandler);
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
      public LocalStateMachineEngine.ControlsSet SetEnabled = 0;
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
        if (!IsPostBack)
        {
          m_StateMachineEngine.InitMahine();
          m_VehicleType.Items.Add(new ListItem(VehicleType.SecurityEscortCar.ToString(), ((int)VehicleType.SecurityEscortCar).ToString()));
          m_VehicleType.Items.Add(new ListItem(VehicleType.Truck.ToString(), ((int)VehicleType.Truck).ToString()) { Selected = true });
          m_VehicleType.Items.Add(new ListItem(VehicleType.Van.ToString(), ((int)VehicleType.Van).ToString()));
        }
        m_CancelButton.Click += m_StateMachineEngine.CancelButton_Click;
        m_AddNewButton.Click += m_StateMachineEngine.NewButton_Click;
        m_DeleteButton.Click += m_StateMachineEngine.DeleteButton_Click;
        m_EditButton.Click += m_StateMachineEngine.EditButton_Click;
        m_SaveButton.Click += m_StateMachineEngine.SaveButton_Click;
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
      SetEnabled(m_ControlState.SetEnabled);
      if (m_ControlState.ItemID.IsNullOrEmpty())
      {
        m_EditButton.Enabled = false;
        m_DeleteButton.Enabled = false;
      }
      base.OnPreRender(e);
    }
    #endregion

    #region State machine
    private class LocalStateMachineEngine : GenericStateMachineEngine<TruckInterconnectionData>
    {
      #region ctor
      internal LocalStateMachineEngine(TruckManagerUserControl _parent)
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override ActionResult Show(TruckInterconnectionData _id)
      {
        return Parent.Show(_id);
      }
      protected override ActionResult Show()
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
      protected override ActionResult Delete()
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
      protected override void SMError(InterfaceEvent _interfaceEvent)
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
      #endregion
      #region internal
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
    /// <summary>
    /// Gets the edc.
    /// </summary>
    /// <value>
    /// The edc.
    /// </value>
    public EntitiesDataContext EDC
    {
      get
      {
        return DataContextManagementAutoDispose<EntitiesDataContext>.GetDataContextManagement(this).DataContext;
      }
    }
    #endregion

    #region state machine actions
    private LocalStateMachineEngine.ActionResult Show()
    {
      if (m_ControlState.ItemID.IsNullOrEmpty())
        return LocalStateMachineEngine.ActionResult.Success;
      try
      {
        Truck _drv = Element.GetAtIndex<Truck>(EDC.Truck, m_ControlState.ItemID);
        Show(_drv);
      }
      catch (Exception ex)
      {
        return new LocalStateMachineEngine.ActionResult(ex, "Show");
      }
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Show(TruckInterconnectionData _interconnectionData)
    {
      if (m_ControlState.ItemID == _interconnectionData.ID)
        return LocalStateMachineEngine.ActionResult.Success;
      m_ControlState.ItemID = _interconnectionData.ID;
      return Show();
    }
    private void Show(Truck _item)
    {
      m_TruckTitle.Text = _item.Title;
      string _id = ((int)_item.VehicleType.GetValueOrDefault(VehicleType.None)).ToString();
      m_VehicleTypeSelect(_item.VehicleType.GetValueOrDefault(VehicleType.None));
      m_Comments.Text = _item.AdditionalComments;
    }
    private LocalStateMachineEngine.ActionResult Update()
    {

      try
      {
        if (m_ControlState.ItemID.IsNullOrEmpty())
          return new LocalStateMachineEngine.ActionResult(new ApplicationException("UpdateTruckNotSelected".GetShepherdLocalizedString()), "Update");
        Truck _drv = Element.GetAtIndex<Truck>(EDC.Truck, m_ControlState.ItemID);
        LocalStateMachineEngine.ActionResult _rr = Update(_drv);
        if (!_rr.ActionSucceeded)
          return _rr;
        EDC.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
      }
      catch (Exception ex)
      {
        return new LocalStateMachineEngine.ActionResult(ex, "Update");
      }
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Update(Truck _drv)
    {
      _drv.AdditionalComments = m_Comments.Text;
      if (m_TruckTitle.Text.IsNullOrEmpty())
        return LocalStateMachineEngine.ActionResult.NotValidated(m_TruckNameLabel.Text + "MustBeProvided".GetShepherdLocalizedString());
      _drv.Title = m_TruckTitle.Text;
      _drv.ToBeDeleted = false;
      if (m_VehicleType.SelectedIndex < 0)
        return LocalStateMachineEngine.ActionResult.NotValidated(m_VehicleTypeLabel.Text + "MustBeProvided".GetShepherdLocalizedString());
      _drv.VehicleType = (VehicleType)m_VehicleType.SelectedValue.String2Int().GetValueOrDefault(0);
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Create()
    {
      if (!m_ControlState.ItemID.IsNullOrEmpty())
        return new LocalStateMachineEngine.ActionResult(new ApplicationException("Create error: a truck is selected"), "Create");
      try
      {
        Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
        if (_Partner == null)
          return LocalStateMachineEngine.ActionResult.NotValidated("CreateuserMustBeExternalPartner".GetShepherdLocalizedString());
        Truck _drv = new Truck() { Truck2PartnerTitle = _Partner };
        LocalStateMachineEngine.ActionResult _rr = Update(_drv);
        if (!_rr.ActionSucceeded)
          return _rr;
        EDC.Truck.InsertOnSubmit(_drv);
        EDC.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
      }
      catch (Exception ex)
      {
        return new LocalStateMachineEngine.ActionResult(ex, "Create");
      }
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Delete()
    {
      if (m_ControlState.ItemID.IsNullOrEmpty())
        return new LocalStateMachineEngine.ActionResult(new ApplicationException("DeleteTruckNotSelected".GetShepherdLocalizedString()), "Delete");
      try
      {
        Truck _itm = Element.GetAtIndex<Truck>(EDC.Truck, m_ControlState.ItemID);
        _itm.ToBeDeleted = true;
        EDC.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
      }
      catch (Exception ex)
      {
        return new LocalStateMachineEngine.ActionResult(ex, "Delete");
      }
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private void ClearUserInterface()
    {
      m_ControlState.ItemID = string.Empty;
      m_VehicleTypeSelect(VehicleType.Truck);
      m_TruckTitle.Text = String.Empty;
      m_Comments.Text = String.Empty;
    }
    private void m_VehicleTypeSelect(VehicleType _vt)
    {
      m_VehicleType.SelectedIndex = -1;
      ListItem _li = m_VehicleType.Items.FindByText(_vt.ToString());
      if (_li != null)
        _li.Selected = true;
    }
    private void SetEnabled(LocalStateMachineEngine.ControlsSet _set)
    {
      m_SaveButton.Enabled = (_set & LocalStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_DeleteButton.Enabled = (_set & LocalStateMachineEngine.ControlsSet.DeleteOn) != 0;
      m_CancelButton.Enabled = (_set & LocalStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_EditButton.Enabled = (_set & LocalStateMachineEngine.ControlsSet.EditOn) != 0;
      m_AddNewButton.Enabled = (_set & LocalStateMachineEngine.ControlsSet.NewOn) != 0;
      m_Panel.Enabled = m_SaveButton.Enabled;
    }
    private void ShowActionResult(LocalStateMachineEngine.ActionResult _rslt)
    {
      if (_rslt.ActionSucceeded)
        return;
      this.Controls.Add(GlobalDefinitions.ErrorLiteralControl(_rslt.ActionException.Message));
    }
    #endregion
  }
}
