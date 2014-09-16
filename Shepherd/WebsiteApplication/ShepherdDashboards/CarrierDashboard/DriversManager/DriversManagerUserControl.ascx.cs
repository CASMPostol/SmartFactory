//<summary>
//  Title   : class DriversManagerUserControl
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
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.DriversManager
{
  /// <summary>
  /// DriversManagerUserControl <see cref="UserControl"/>
  /// </summary>
  public partial class DriversManagerUserControl : UserControl
  {

    #region public
    internal void SetInterconnectionData(Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      if (_ProvidesDictionary.Keys.Contains(InterconnectionData.ConnectionSelector.DriverInterconnection))
        new DriverInterconnectionData().SetRowData
          (_ProvidesDictionary[InterconnectionData.ConnectionSelector.DriverInterconnection], m_StateMachineEngine.NewDataEventHandler);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DriversManagerUserControl"/> class.
    /// </summary>
    public DriversManagerUserControl()
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
          m_StateMachineEngine.InitMahine();
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
    private class LocalStateMachineEngine : GenericStateMachineEngine<DriverInterconnectionData>
    {
      #region ctor
      internal LocalStateMachineEngine(DriversManagerUserControl _parent)
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override ActionResult Show(DriverInterconnectionData _id)
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
      private DriversManagerUserControl Parent { get; set; }
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
    private EntitiesDataContext EDC
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
        Driver _drv = Element.GetAtIndex<Driver>(EDC.Driver, m_ControlState.ItemID);
        Show(_drv);
      }
      catch (Exception ex)
      {
        return new LocalStateMachineEngine.ActionResult(ex, "Show");
      }
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Show(DriverInterconnectionData _interconnectionData)
    {
      if (m_ControlState.ItemID == _interconnectionData.ID)
        return LocalStateMachineEngine.ActionResult.Success;
      m_ControlState.ItemID = _interconnectionData.ID;
      return Show();
    }
    private void Show(Driver _drv)
    {
      m_DriverIDNumber.Text = _drv.IdentityDocumentNumber;
      m_DriverMobileNo.Text = _drv.CellPhone;
      m_DriverTitle.Text = _drv.Title;
    }
    private LocalStateMachineEngine.ActionResult Update()
    {
      try
      {
        if (m_ControlState.ItemID.IsNullOrEmpty())
          return new LocalStateMachineEngine.ActionResult(new ApplicationException("UpdateDriverNotSelected".GetShepherdLocalizedString()), "Update");
        Driver _drv = Element.GetAtIndex<Driver>(EDC.Driver, m_ControlState.ItemID);
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
    private LocalStateMachineEngine.ActionResult Update(Driver _drv)
    {
      _drv.IdentityDocumentNumber = m_DriverIDNumber.Text;
      _drv.CellPhone = m_DriverMobileNo.Text;
      _drv.ToBeDeleted = false;
      if (m_DriverTitle.Text.IsNullOrEmpty())
        return LocalStateMachineEngine.ActionResult.NotValidated(m_DriverNameLabel.Text + "MustBeProvided".GetShepherdLocalizedString());
      _drv.Title = m_DriverTitle.Text;
      return LocalStateMachineEngine.ActionResult.Success;
    }
    private LocalStateMachineEngine.ActionResult Create()
    {
      if (!m_ControlState.ItemID.IsNullOrEmpty())
        return new LocalStateMachineEngine.ActionResult(new ApplicationException("Create error: a driver is selected"), "Create");
      try
      {
        Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
        if (_Partner == null)
          return LocalStateMachineEngine.ActionResult.NotValidated("CreateuserMustBeExternalPartner".GetShepherdLocalizedString());
        Driver _drv = new Driver() { Driver2PartnerTitle = _Partner };
        LocalStateMachineEngine.ActionResult _rr = Update(_drv);
        if (!_rr.ActionSucceeded)
          return _rr;
        EDC.Driver.InsertOnSubmit(_drv);
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
        return new LocalStateMachineEngine.ActionResult(new ApplicationException("DeleteDriverNotSelected".GetShepherdLocalizedString()), "Delete");
      try
      {
        Driver _drv = Element.GetAtIndex<Driver>(EDC.Driver, m_ControlState.ItemID);
        _drv.ToBeDeleted = true;
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
      m_DriverIDNumber.Text = String.Empty;
      m_DriverMobileNo.Text = String.Empty;
      m_DriverTitle.Text = String.Empty;
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
