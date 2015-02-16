//<summary>
//  Title   : class ClearenceWebPartUserControl
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

using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Logging;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.Dashboards.Clearance;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Clearance Web PartUser Control
  /// </summary>
  public partial class ClearenceWebPartUserControl : UserControl
  {
    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ClearenceWebPartUserControl" /> class.
    /// </summary>
    public ClearenceWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
      m_DataContextManagement = new DataContextManagement<Entities>(this);
      m_AvailableGridViewDataSource = new ObjectDataSource();
      m_AvailableGridViewDataSource.ID = "DATASOURCEID";
      m_AvailableGridViewDataSource.SelectMethod = "SelectData";
      m_AvailableGridViewDataSource.TypeName = this.GetType().AssemblyQualifiedName;
      m_AvailableGridViewDataSource.ObjectCreating += m_GridViewDataSource_ObjectCreating;
      this.Controls.Add(m_AvailableGridViewDataSource);

      m_AssignedGridViewDataSource = new ObjectDataSource();
      m_AssignedGridViewDataSource.ID = "ASSIGNEDGRIDVIEWDATASOURCE";
      m_AssignedGridViewDataSource.SelectMethod = "SelectDataAssigned";
      m_AssignedGridViewDataSource.TypeName = this.GetType().AssemblyQualifiedName;
      m_AssignedGridViewDataSource.ObjectCreating += m_GridViewDataSource_ObjectCreating;
      this.Controls.Add(m_AssignedGridViewDataSource);
    }
    /// <summary>
    /// Selects the available items.
    /// </summary>
    /// <returns></returns>
    public DataTable SelectData()
    {
      return m_ControlState.AvailableItems.SelectionTable;
    }
    /// <summary>
    /// Selects the just assigned items.
    /// </summary>
    /// <returns></returns>
    public DataTable SelectDataAssigned()
    {
      return m_ControlState.AssignedItems.SelectionTable;
    }
    #endregion

    #region public
    internal void SetInterconnectionData(Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary)
    {
      foreach (var _item in m_ProvidersDictionary)
      {
        try
        {
          switch (_item.Key)
          {
            case ConnectionSelector.ClearenceInterconnection:
              new ClearenceInterconnectionnData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
              break;
            default:
              break;
          }
        }
        catch (Exception ex)
        {
          string _at = _item.Key.ToString();
          ApplicationError _ae = new ApplicationError("Page_Load", _at, ex.Message, ex);
          this.Controls.Add(_ae.CreateMessage(_at, true));
        }
      }
    }
    #endregion

    #region UserControl override
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      m_AvailableGridView.DataSourceID = m_AvailableGridViewDataSource.ID;
      m_AssignedGridView.DataSourceID = m_AssignedGridViewDataSource.ID;
      base.OnInit(e);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      string at = "Starting";
      try
      {
        if (!IsPostBack)
        {
          at = "InitMahine";
          m_StateMachineEngine.InitMahine();
        }
        at = "Event handlers";
        m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_NewButton.Click += new EventHandler(m_StateMachineEngine.NewButton_Click);
        m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_DeleteButton.Click += new EventHandler(m_StateMachineEngine.DeleteButton_Click);
        m_ClearButton.Click += new EventHandler(m_StateMachineEngine.m_ClearButton_Click);
        m_GridViewAddAll.Click += m_GridViewAddAll_Click;
        m_GridViewAddDisplayed.Click += m_GridViewAddDisplayed_Click;
        m_GridViewRemoveAll.Click += m_GridViewRemoveAll_Click;
        m_GridViewRemoveDisplayed.Click += m_GridViewRemoveDisplayed_Click;
      }
      catch (Exception ex)
      {
        ApplicationError _ae = new ApplicationError("Page_Load", at, ex.Message, ex);
        this.Controls.Add(_ae.CreateMessage(at, true));
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
      {
        m_ControlState = new ControlState(null);
        m_StateMachineEngine.InitMahine();
      }
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
        SetEnabled(m_ControlState.SetEnabled);
        Show();
        if (!m_AvailableGridViewSkipBinding)
          m_AvailableGridView.DataBind();
        m_AssignedGridView.DataBind();
      }
      catch (Exception _ex)
      {
        LocalStateMachineEngine.ActionResult _errr = LocalStateMachineEngine.ActionResult.Exception(_ex, _ex.Message);
        this.ShowActionResult(_errr);
      }
      base.OnPreRender(e);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
      m_DataContextManagement.Dispose();
      base.OnUnload(e);
    }
    #endregion

    #region StateManagement
    [Serializable]
    private class ControlState
    {
      #region state fields
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      public GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      public string ClearanceID = String.Empty;
      public string ClearanceTitle;
      public bool IsModified { get; set; }
      public Selection AvailableItems = new Selection();
      public Selection AssignedItems = new Selection();
      public bool ReadOnly = false;
      #endregion

      #region public
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
      internal void ClearAssigned()
      {
        AssignedItems.SelectionTable.Clear();
        ClearanceID = String.Empty;
        ClearanceTitle = String.Empty;
      }
      internal void ClearAvailable()
      {
        AvailableItems.SelectionTable.Clear();
      }
      #endregion
    }
    private class LocalStateMachineEngine : WEB.WebpartStateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine(ClearenceWebPartUserControl parent)
      {
        Parent = parent;
      }
      #endregion

      #region public
      internal void InitMahine(InterfaceState _ControlState)
      {
        Parent.m_ControlState.InterfaceState = _ControlState;
      }
      internal void InitMahine()
      {
        Parent.m_ControlState.InterfaceState = InterfaceState.ViewState;
        EnterState();
      }
      internal InterfaceState CurrentState { get { return this.CurrentMachineState; } }
      #endregion

      #region NewDataEventHandlers
      internal void NewDataEventHandler(object sender, ClearenceInterconnectionnData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.EditState:
          case InterfaceState.NewState:
            break;
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData(e);
            break;
          default:
            break;
        }
      }
      #endregion

      #region GenericStateMachineEngine implementation
      protected override GenericStateMachineEngine.ActionResult Update()
      {
        try
        {
          Parent.TraceEvent("Entering LocalStateMachineEngine.Update", 292, TraceSeverity.Monitorable);
          Parent.Update();
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch (GenericStateMachineEngine.ActionResult ex)
        {
          return ex;
        }
        catch (Exception ex)
        {
          Parent.TraceEvent(String.Format("Exception at LocalStateMachineEngine.Update: {0} StockTrace {1}", ex.Message, ex.StackTrace), 308, TraceSeverity.High);
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Update");
        }
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        try
        {
          Parent.TraceEvent("Entering LocalStateMachineEngine.Create", 309, TraceSeverity.Monitorable);
          Parent.Create((x, y, z) => Parent.TraceEvent(x, y, z));
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch (GenericStateMachineEngine.ActionResult ex)
        {
          return ex;
        }
        catch (Exception ex)
        {
          Parent.TraceEvent(String.Format("Exception at LocalStateMachineEngine.Create: {0} StockTrace {1}", ex.Message, ex.StackTrace), 308, TraceSeverity.High);
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Create");
        }
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        try
        {
          Parent.Delete();
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch (GenericStateMachineEngine.ActionResult ex)
        {
          return ex;
        }
        catch (Exception ex)
        {
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Create");
        }
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearAssigned();
      }
      protected override void SetEnabled(GenericStateMachineEngine.ControlsSet _buttons)
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError(GenericStateMachineEngine.InterfaceEvent interfaceEvent)
      {
        ShowActionResult
          (ActionResult.Exception(new ApplicationError("SMError", CurrentMachineState.ToString(), "State maschine internal error", null), "State maschine internal error"));
        CurrentMachineState = InterfaceState.ViewState;
      }
      protected override void ShowActionResult(GenericStateMachineEngine.ActionResult _rslt)
      {
        Parent.ShowActionResult(_rslt);
        base.ShowActionResult(_rslt);
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
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
      protected override void EnterState()
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            Parent.ClearAvailable();
            Parent.QueryAssigned();
            break;
          case InterfaceState.EditState:
            Parent.QueryAvailable();
            break;
          case InterfaceState.NewState:
            Parent.QueryAvailable();
            Parent.ClearAssigned();
            break;
        }
        base.EnterState();
      }
      #endregion

      #region Event handlers
      internal void m_ClearButton_Click(object sender, EventArgs e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            ActionResult _resu = Parent.ClearThroughCustom();
            switch (_resu.LastActionResult)
            {
              case ActionResult.Result.Success:
                break;
              case ActionResult.Result.NotValidated:
                CurrentMachineState = InterfaceState.EditState;
                ShowActionResult(_resu);
                break;
              case ActionResult.Result.Exception:
              default:
                ShowActionResult(_resu);
                break;
            }
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            SMError(InterfaceEvent.NewClick);
            break;
        }
      }
      #endregion

      #region private
      private ClearenceWebPartUserControl Parent { get; set; }
      #endregion

    }
    #endregion

    #region private

    #region SetInterconnectionData
    private void SetInterconnectionData(ClearenceInterconnectionnData e)
    {
      if (m_ControlState.ClearanceID.CompareTo(e.ID) == 0)
        return;
      try
      {
        m_ControlState.ClearanceID = e.ID;
        ListItem _cs = m_SelectGroupRadioButtonList.Items.FindByValue(CurrentClearence.ProcedureCode);
        if (_cs == null)
        {
          this.ShowActionResult(GenericStateMachineEngine.ActionResult.NotValidated("ThisClearanceCannotBeEditedItIsNotCompensationGoodClearance".GetLocalizedString()));
          m_ControlState.ReadOnly = true;
          m_ControlState.ClearanceID = String.Empty;
          m_ControlState.ClearanceTitle = String.Empty;
          m_ClearenceTextBox.Text = String.Empty;
          return;
        }
        m_ControlState.IsModified = true;
        m_ControlState.ClearanceTitle = e.Title;
        m_ClearenceTextBox.Text = e.Title;
        m_SelectGroupRadioButtonList.SelectedIndex = -1;
        _cs.Selected = true;
        m_ControlState.ReadOnly = false;
        QueryAssigned();
        string _export = "3151";
        switch (CurrentClearence.ClearenceProcedure.Value)
        {
          case ClearenceProcedure._3151:
          case ClearenceProcedure._3171:
            break;
          case ClearenceProcedure._4051:
          case ClearenceProcedure._4071:
            _export = "4051";
            break;
          case ClearenceProcedure._5100:
          case ClearenceProcedure._5171:
          case ClearenceProcedure._7100:
          case ClearenceProcedure._7171:
          case ClearenceProcedure.Invalid:
          case ClearenceProcedure.None:
          default:
            break;
        }
        m_ProcedureRadioButtonList.SelectedValue = _export;
      }
      catch (Exception _ex)
      {
        ApplicationError _errr = new ApplicationError("SetInterconnectionData", "ClearenceInterconnectionnData", _ex.Message, _ex);
        this.Controls.Add(_errr.CreateMessage(_errr.At, true));
      }
    }
    #endregion

    #region business logic
    private void Update()
    {
      if (CurrentClearence == null)
        throw GenericStateMachineEngine.ActionResult.Exception(null, "Internal error - ClearanceID is null or empty at Update");
      Entities _edc = m_DataContextManagement.DataContext;
      CurrentClearence.UpdateClerance(_edc, m_SelectGroupRadioButtonList.SelectedValue, SelectedClearenceProcedure);
      //remove from clearance
      foreach (Selection.SelectionTableRow _row in m_ControlState.AvailableItems.SelectionTable.OnlyAdded)
      {
        Disposal _dspsl = Element.GetAtIndex<Disposal>(_edc.Disposal, _row.Id);
        RemoveDisposalFromClearance(_edc, _dspsl);
      }
      //add to clearance
      foreach (Selection.SelectionTableRow _row in m_ControlState.AssignedItems.SelectionTable.OnlyAdded)
      {
        if (_row.Disposal)
        {
          Disposal _dspsl = Element.GetAtIndex<Disposal>(_edc.Disposal, _row.Id);
          _dspsl.Disposal2ClearenceIndex = CurrentClearence;
        }
        else
        {
          IPRClass _ipr = Element.GetAtIndex<IPRClass>(_edc.IPR, _row.Id);
          _ipr.AddDisposal(_edc, Convert.ToDecimal(_row.Quantity), CurrentClearence);
        }
      }
      CurrentClearence.UpdateTitle(_edc);
      m_DataContextManagement.DataContext.SubmitChanges();
    }
    private void Create(NamedTraceLogger.TraceAction trace)
    {
      CurrentClearence = Clearence.CreateClearance(m_DataContextManagement.DataContext, m_SelectGroupRadioButtonList.SelectedValue, SelectedClearenceProcedure, trace);
      Update();
      Response.Redirect(Request.RawUrl);
    }
    private void Delete()
    {
      Entities _edc = m_DataContextManagement.DataContext;
      foreach (Disposal _dx in CurrentClearence.Disposal(_edc))
        RemoveDisposalFromClearance(_edc, _dx);
      m_DataContextManagement.DataContext.SubmitChanges();
      m_DataContextManagement.DataContext.Clearence.DeleteOnSubmit(CurrentClearence);
      ClearAssigned();
      _edc.SubmitChanges();
    }
    private static void RemoveDisposalFromClearance(Entities edc, Disposal dspsl)
    {
      dspsl.Disposal2ClearenceIndex = null;
      if (dspsl.DisposalStatus.Value == DisposalStatus.Tobacco)
      {
        dspsl.Disposal2IPRIndex.RevertWithdraw(dspsl.SettledQuantity.Value);
        edc.Disposal.DeleteOnSubmit(dspsl);
      }
    }
    private void SetEnabled(GenericStateMachineEngine.ControlsSet controlsSet)
    {
      if (m_ControlState.ReadOnly || m_ControlState.ClearanceID.IsNullOrEmpty())
        controlsSet &= ~(GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.DeleteOn);
      //Buttons
      m_EditButton.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.EditOn) != 0;
      m_ClearButton.Enabled = m_EditButton.Enabled;
      m_CancelButton.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_NewButton.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.NewOn) != 0;
      m_SaveButton.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_DeleteButton.Enabled = (controlsSet & GenericStateMachineEngine.ControlsSet.DeleteOn) != 0;
      switch (m_StateMachineEngine.CurrentState)
      {
        case GenericStateMachineEngine.InterfaceState.ViewState:
          m_FiltersPanel.Enabled = true;
          m_AvailableGridView.Enabled = false;
          m_AssignedGridView.Enabled = false;
          m_GridViewActionsPanel.Enabled = false;
          break;
        case GenericStateMachineEngine.InterfaceState.EditState:
          m_FiltersPanel.Enabled = false;
          m_AvailableGridView.Enabled = true;
          m_AssignedGridView.Enabled = true;
          m_GridViewActionsPanel.Enabled = true;
          break;
        case GenericStateMachineEngine.InterfaceState.NewState:
          m_FiltersPanel.Enabled = false;
          m_AvailableGridView.Enabled = true;
          m_AssignedGridView.Enabled = true;
          m_GridViewActionsPanel.Enabled = true;
          break;
      }
    }
    private void ShowActionResult(GenericStateMachineEngine.ActionResult _rslt)
    {
      Controls.Add(ControlExtensions.CreateMessage(_rslt.ActionException.Message));
    }
    private GenericStateMachineEngine.ActionResult ClearThroughCustom()
    {
      try
      {
        Entities _edc = m_DataContextManagement.DataContext;
        string _masterDocumentName = CurrentClearence.FinishedGoodsExportFormFileName(_edc);
        int _sadConsignmentIdentifier = default(int);
        Func<IEnumerable<Disposal>> _dspslLst = () => CurrentClearence.Disposal(_edc);
        switch (ToSelectedGroup(CurrentClearence.ProcedureCode))
        {
          case Group.Tobacco:
          case Group.TobaccoNotAllocated:
            DocumentContent _newTobaccoDoc =
              DisposalsFormFactory.GetTobaccoFreeCirculationFormContent(_dspslLst(), CurrentClearence.ClearenceProcedure.Value, _masterDocumentName);
            _sadConsignmentIdentifier = SPDocumentFactory.Prepare(SPContext.Current.Web, _newTobaccoDoc, _masterDocumentName, CompensatiionGood.Tobacco);
            break;
          case Group.Waste:
          case Group.Dust:
            DocumentContent _newDustWasteDoc = DisposalsFormFactory.GetDustWasteFormContent(_dspslLst(), CurrentClearence.ClearenceProcedure.Value, _masterDocumentName);
            CompensatiionGood _compensatiionGood = SelectedGroup == Group.Waste ? CompensatiionGood.Waste : CompensatiionGood.Dust;
            _sadConsignmentIdentifier = SPDocumentFactory.Prepare(SPContext.Current.Web, _newDustWasteDoc, _masterDocumentName, _compensatiionGood);
            break;
          case Group.Cartons:
            DocumentContent _newBoxFormContent = DisposalsFormFactory.GetBoxFormContent(_dspslLst(), CurrentClearence.ClearenceProcedure.Value, _masterDocumentName);
            _sadConsignmentIdentifier = SPDocumentFactory.Prepare(SPContext.Current.Web, _newBoxFormContent, _masterDocumentName, CompensatiionGood.Cartons);
            break;
        }
        SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>(_edc.SADConsignment, _sadConsignmentIdentifier);
        CurrentClearence.ClearThroughCustom(_edc, _sadConsignment, (x, y, z) => { });  //TODO implement tracing
        _edc.SubmitChanges();
        Response.Redirect(Request.RawUrl);
        return GenericStateMachineEngine.ActionResult.Success;
      }
      catch (GenericStateMachineEngine.ActionResult _ar)
      {
        return _ar;
      }
      catch (Exception ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(ex, "ClearThroughCustom");
      }
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      double _availableSum = m_ControlState.AvailableItems.SelectionTable.Count > 0 ? (from _avrx in m_ControlState.AvailableItems.SelectionTable select _avrx).Sum(x => x.Quantity) : 0;
      double _assignedSum = m_ControlState.AssignedItems.SelectionTable.Count > 0 ? (from _avrx in m_ControlState.AssignedItems.SelectionTable select _avrx).Sum(x => x.Quantity) : 0;
      m_AvailableGridViewQuntitySumLabel.Text = String.Format("QuantityFormat".GetLocalizedString(), _availableSum);
      m_AssignedGridViewQuantitySumLabel.Text = String.Format("QuantityFormat".GetLocalizedString(), _assignedSum);
      m_ClearenceTextBox.Text = m_ControlState.ClearanceTitle;
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void ClearAvailable()
    {
      m_ControlState.ClearAvailable();
      m_AvailableGridViewQuntitySumLabel.Text = String.Empty;
    }
    private void ClearAssigned()
    {
      m_ControlState.ClearAssigned();
      m_AssignedGridViewQuantitySumLabel.Text = String.Empty;
    }
    private void QueryAssigned()
    {
      if (CurrentClearence == null)
        return;
      m_ControlState.AssignedItems.SelectionTable.Clear();
      List<Selection.SelectionTableRowWraper> _dsposals = (from _dsx in CurrentClearence.Disposal(m_DataContextManagement.DataContext)
                                                           select new Selection.SelectionTableRowWraper(_dsx)).ToList();
      m_ControlState.AssignedItems.SelectionTable.Add(_dsposals);
    }
    private void QueryAvailable()
    {
      DateTime? _start = m_AllDate.Checked || m_StartDateTimeControl.IsDateEmpty ? new Nullable<DateTime>() : m_StartDateTimeControl.SelectedDate.Date;
      DateTime? _finish = m_AllDate.Checked || m_EndTimeControl1.IsDateEmpty ? new Nullable<DateTime>() : m_EndTimeControl1.SelectedDate.Date + TimeSpan.FromDays(1);
      string _currency = m_SelectCurrencyRadioButtonList.SelectedValue.Contains("All") ? String.Empty : m_SelectCurrencyRadioButtonList.SelectedValue;
      List<Selection.SelectionTableRowWraper> _available = null;
      switch (SelectedGroup)
      {
        case Group.Tobacco:
          _available = GetDisposals(new DisposalStatus[] { DisposalStatus.Overuse, DisposalStatus.SHMenthol }, false, _start, _finish, _currency);
          break;
        case Group.TobaccoNotAllocated:
          _available = GetMaterial(m_ControlState.AvailableItems, _start, _finish, _currency);
          break;
        case Group.Dust:
          _available = GetDisposals(new DisposalStatus[] { DisposalStatus.Dust }, true, _start, _finish, _currency);
          break;
        case Group.Waste:
          _available = GetDisposals(new DisposalStatus[] { DisposalStatus.Waste }, true, _start, _finish, _currency);
          break;
        case Group.Cartons:
          _available = GetDisposals(new DisposalStatus[] { DisposalStatus.Cartons }, false, _start, _finish, _currency);
          break;
        default:
          throw new SharePoint.ApplicationError("SelectDataDS", "switch", "Internal error - wrong switch case.", null);
      };
      m_ControlState.AvailableItems.SelectionTable.Add(_available);
    }
    private List<Selection.SelectionTableRowWraper> GetMaterial(Selection data, DateTime? start, DateTime? finisch, string currency)
    {
      return (from _iprx in m_DataContextManagement.DataContext.IPR
              let _batch = _iprx.Batch
              where (!_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0) &&
                    (!start.HasValue || _iprx.CustomsDebtDate >= start.Value) &&
                    (!finisch.HasValue || _iprx.CustomsDebtDate < finisch.Value) &&
                    (String.IsNullOrEmpty(currency) || _iprx.Currency.Contains(currency))
              orderby _batch ascending
              select new Selection.SelectionTableRowWraper(_iprx)).ToList();
    }
    private List<Selection.SelectionTableRowWraper> GetDisposals(DisposalStatus[] status, bool creationDataFilter, DateTime? start, DateTime? finisch, string currency)
    {
      DisposalStatus _status0 = status[0];
      DisposalStatus _status1 = status.Length == 2 ? status[1] : DisposalStatus.Invalid;
      List<Disposal> _ListOfDisposals = (from _dspslx in m_DataContextManagement.DataContext.Disposal
                                         where _dspslx.CustomsStatus.Value == CustomsStatus.NotStarted &&
                                               (_dspslx.DisposalStatus.Value == _status0 || _dspslx.DisposalStatus.Value == _status1) &&
                                               _dspslx.SettledQuantity > 0
                                         select _dspslx).ToList<Disposal>();
      _ListOfDisposals = _ListOfDisposals.Where<Disposal>(x => Disposal2ClearenceIndexIsNull(x)).ToList<Disposal>();
      List<Selection.SelectionTableRowWraper> _ret = (from _dspslx in _ListOfDisposals
                                                      let _ogl = _dspslx.Disposal2IPRIndex.DocumentNo
                                                      let _currency = _dspslx.Disposal2IPRIndex.Currency
                                                      where String.IsNullOrEmpty(currency) || _currency.Contains(currency)
                                                      orderby _ogl ascending
                                                      select new Selection.SelectionTableRowWraper(_dspslx)).ToList();
      if (creationDataFilter)
        _ret = (from _itmx in _ret
                where (!start.HasValue || _itmx.Created >= start) && (!finisch.HasValue || _itmx.Created < finisch)
                orderby _itmx.Created ascending
                select _itmx).ToList();
      else
        _ret = (from _itmx in _ret
                where (!start.HasValue || _itmx.DebtDate >= start) && (!finisch.HasValue || _itmx.DebtDate < finisch)
                orderby _itmx.DebtDate ascending
                select _itmx).ToList();
      return _ret;
    }
    private bool Disposal2ClearenceIndexIsNull(Disposal dspsl)
    {
      bool _ret = true;
      try
      {
        _ret = !(dspsl.Disposal2ClearenceIndex != null && dspsl.Disposal2ClearenceIndex.Id.HasValue);
      }
      catch (Exception) { }
      return _ret;
    }
    private ClearenceProcedure SelectedClearenceProcedure
    {
      get
      {
        if (m_ProcedureRadioButtonList.SelectedIndex < 0)
          throw GenericStateMachineEngine.ActionResult.NotValidated("CustomsProcedureMustBeSelected".GetLocalizedString());
        switch (m_ProcedureRadioButtonList.SelectedValue)
        {
          case "4051":
            return ClearenceProcedure._4051;
          case "3151":
            return ClearenceProcedure._3151;
          default:
            throw GenericStateMachineEngine.ActionResult.Exception(null, "InternalErrorWrongCustomsProcedureIsSelected".GetLocalizedString());
        }
      }
    }
    private enum Group { Tobacco, TobaccoNotAllocated, Dust, Waste, Cartons }
    private Group SelectedGroup
    {
      get
      {
        return ToSelectedGroup(m_SelectGroupRadioButtonList.SelectedValue);
      }
    }
    private Group ToSelectedGroup(string group)
    {
      switch (group)
      {
        case "Tobacco":
          return Group.Tobacco;
        case "TobaccoNotAllocated":
          return Group.TobaccoNotAllocated;
        case "Dust":
          return Group.Dust;
        case "Waste":
          return Group.Waste;
        case "Cartons":
          return Group.Cartons;
        default:
          throw new SharePoint.ApplicationError("SelectedGroup", "switch", "Internal error - wrong switch case.", null);
      };
    }
    private void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.Clearance);
    }
    #endregion

    #region vars
    private bool m_AvailableGridViewSkipBinding = false;
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = new ControlState(null);
    private DataContextManagement<Entities> m_DataContextManagement = null;
    private const string m_IDItemLabel = "IDItemLabel";
    private const string m_IDEditLabel = "IDEditLabel";
    private const string m_QuantityNewValue = "QuantityNewValue";
    private ObjectDataSource m_AvailableGridViewDataSource;
    private ObjectDataSource m_AssignedGridViewDataSource;
    private Clearence _clearence = default(Clearence);
    private Clearence CurrentClearence
    {
      get
      {
        if (_clearence != null)
          return _clearence;
        if (m_ControlState.ClearanceID.IsNullOrEmpty())
          return null;
        _clearence = Element.GetAtIndex<Clearence>(m_DataContextManagement.DataContext.Clearence, m_ControlState.ClearanceID);
        return _clearence;
        ;
      }
      set
      {
        _clearence = value;
        m_ControlState.ClearanceID = value.Id.Value.ToString();
        m_ControlState.ClearanceTitle = value.Title;
      }
    }
    #endregion


    #region Event handlers

    private static void GetRow(SPGridView sender, GridViewSelectEventArgs e, Selection.SelectionTableDataTable destination, Selection.SelectionTableDataTable source)
    {
      if (sender == null)
        return;
      GridViewRow row = sender.Rows[e.NewSelectedIndex];
      string _id = ((Label)row.FindControlRecursive(m_IDItemLabel)).Text;
      destination.GetRow(source, _id);
      e.NewSelectedIndex = -1;
      e.Cancel = true;
    }

    #region AvailableGridView event handlers
    /// <summary>
    /// Handles the RowEditing event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewEditEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
      SPGridView _sender = sender as SPGridView;
      if (_sender == null)
        return;
      if (!_sender.FilterFieldValue.IsNullOrEmpty())
      {
        e.Cancel = true;
        string _msg = "SplittingEntryIsNotSupportedWhenFiltering".GetLocalizedString();
        m_AvailablePanel.Controls.Add(ControlExtensions.CreateMessage(_msg));
        return;
      }
      Label _idLabel = (Label)_sender.Rows[e.NewEditIndex].FindControlRecursive(m_IDItemLabel);
      if (Selection.SelectionTableRow.IsDisposal(_idLabel.Text))
      {
        e.Cancel = true;
        string _msg = "SplittingTheItemForTheSelectedGoodsGroupIsNotAllowed".GetLocalizedString();
        m_AvailablePanel.Controls.Add(ControlExtensions.CreateMessage(_msg));
        return;
      }
      _sender.EditIndex = e.NewEditIndex;  //NewEditIndex is index of the selectet item in filtering mode, but unfortunately entering editing mode the filter is not active.
      m_AvailableGridViewSkipBinding = true;
    }
    /// <summary>
    /// Handles the RowCancelingEdit event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewCancelEditEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      SPGridView _sender = sender as SPGridView;
      if (_sender == null)
        return;
      _sender.EditIndex = -1;
      e.Cancel = true;
    }
    /// <summary>
    /// Handles the RowUpdating event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewUpdateEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      SPGridView _sender = sender as SPGridView;
      if (_sender == null)
        return;
      //Update the values.
      GridViewRow row = _sender.Rows[e.RowIndex];
      Label _idLabel = (Label)row.FindControlRecursive(m_IDEditLabel);
      double _qtty = default(double);
      Selection.SelectionTableRow _slctdItem = m_ControlState.AvailableItems.SelectionTable.FindByID(_idLabel.Text);
      TextBox _quantityTB = (TextBox)row.FindControlRecursive(m_QuantityNewValue);
      if (!double.TryParse(_quantityTB.Text, out _qtty) || (_slctdItem.Quantity < _qtty))
      {
        _quantityTB.Text = _slctdItem.Quantity.ToString("F2");
        _quantityTB.BorderColor = System.Drawing.Color.Red;
        _quantityTB.BackColor = System.Drawing.Color.Yellow;
        e.Cancel = true;
        return;
      }
      Selection.SelectionTableRow _assignedRow = m_ControlState.AssignedItems.SelectionTable.FindByID(_idLabel.Text);
      if (_assignedRow == null)
      {
        m_ControlState.AssignedItems.SelectionTable.ImportRow(_slctdItem);
        _assignedRow = m_ControlState.AssignedItems.SelectionTable.FindByID(_idLabel.Text);
        _assignedRow.Quantity = _qtty;
      }
      else
        _assignedRow.Quantity += _qtty;
      _assignedRow.AcceptChanges();
      _assignedRow.SetAdded();
      _slctdItem.Quantity -= _qtty;
      _sender.EditIndex = -1;
      e.Cancel = true;
    }
    /// <summary>
    /// Handles the SelectedIndexChanging event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewSelectEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
      GetRow(sender as SPGridView, e, m_ControlState.AssignedItems.SelectionTable, m_ControlState.AvailableItems.SelectionTable);
    }
    #endregion

    #region AssignedGridView
    /// <summary>
    /// Handles the SelectedIndexChanging event of the m_AssignedGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewSelectEventArgs" /> instance containing the event data.</param>
    protected void m_AssignedGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
      GetRow(sender as SPGridView, e, m_ControlState.AvailableItems.SelectionTable, m_ControlState.AssignedItems.SelectionTable);
    }
    #endregion

    #region m_GridViewActionsPanel
    private void m_GridViewRemoveDisplayed_Click(object sender, EventArgs e)
    {
      try
      {
        MovePage(m_AssignedGridView, m_ControlState.AvailableItems.SelectionTable, m_ControlState.AssignedItems.SelectionTable);
      }
      catch (Exception _ex)
      {
        LocalStateMachineEngine.ActionResult _errr = LocalStateMachineEngine.ActionResult.Exception(_ex, _ex.Message);
        this.ShowActionResult(_errr);
      }
    }
    private void m_GridViewRemoveAll_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (Selection.SelectionTableRow _rw in m_ControlState.AssignedItems.SelectionTable.ToList<Selection.SelectionTableRow>())
          m_ControlState.AvailableItems.SelectionTable.GetRow(_rw);
      }
      catch (Exception _ex)
      {
        LocalStateMachineEngine.ActionResult _errr = LocalStateMachineEngine.ActionResult.Exception(_ex, _ex.Message);
        this.ShowActionResult(_errr);
      }
    }
    private void m_GridViewAddDisplayed_Click(object sender, EventArgs e)
    {
      try
      {
        MovePage(m_AvailableGridView, m_ControlState.AssignedItems.SelectionTable, m_ControlState.AvailableItems.SelectionTable);
      }
      catch (Exception _ex)
      {
        LocalStateMachineEngine.ActionResult _errr = LocalStateMachineEngine.ActionResult.Exception(_ex, _ex.Message);
        this.ShowActionResult(_errr);
      }
    }
    private void m_GridViewAddAll_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (Selection.SelectionTableRow _rw in m_ControlState.AvailableItems.SelectionTable.ToList<Selection.SelectionTableRow>())
          m_ControlState.AssignedItems.SelectionTable.GetRow(_rw);
      }
      catch (Exception _ex)
      {
        LocalStateMachineEngine.ActionResult _errr = LocalStateMachineEngine.ActionResult.Exception(_ex, _ex.Message);
        this.ShowActionResult(_errr);
      }
    }
    private void MovePage(SPGridView gridView, Selection.SelectionTableDataTable destination, Selection.SelectionTableDataTable source)
    {
      foreach (GridViewRow _cr in gridView.Rows)
      {
        Label _idLabel = (Label)_cr.FindControlRecursive(m_IDItemLabel);
        Selection.SelectionTableRow _slctdItem = source.FindByID(_idLabel.Text);
        destination.GetRow(_slctdItem);
      }
    }
    #endregion

    /// <summary>
    /// Handles the ObjectCreating event of the m_GridViewDataSource control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ObjectDataSourceEventArgs" /> instance containing the event data.</param>
    protected void m_GridViewDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
      e.ObjectInstance = this;
    }

    #endregion

    #endregion
  }
}
