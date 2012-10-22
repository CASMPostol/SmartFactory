﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Clearence Web PartUser Control
  /// </summary>
  public partial class ClearenceWebPartUserControl: UserControl
  {
    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ClearenceWebPartUserControl" /> class.
    /// </summary>
    public ClearenceWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine( this );
      m_DataContextManagement = new DataContextManagement<Entities>( this );
      m_AvailableGridViewDataSource = new ObjectDataSource();
      m_AvailableGridViewDataSource.ID = "DATASOURCEID";
      m_AvailableGridViewDataSource.SelectMethod = "SelectData";
      m_AvailableGridViewDataSource.TypeName = this.GetType().AssemblyQualifiedName;
      m_AvailableGridViewDataSource.ObjectCreating += m_GridViewDataSource_ObjectCreating;
      this.Controls.Add( m_AvailableGridViewDataSource );

      m_AssignedGridViewDataSource = new ObjectDataSource();
      m_AssignedGridViewDataSource.ID = "ASSIGNEDGRIDVIEWDATASOURCE";
      m_AssignedGridViewDataSource.SelectMethod = "SelectDataAssigned";
      m_AssignedGridViewDataSource.TypeName = this.GetType().AssemblyQualifiedName;
      m_AssignedGridViewDataSource.ObjectCreating += m_GridViewDataSource_ObjectCreating;
      this.Controls.Add( m_AssignedGridViewDataSource );
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
    internal void SetInterconnectionData( Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary )
    {
      foreach ( var _item in m_ProvidersDictionary )
      {
        try
        {
          switch ( _item.Key )
          {
            case ConnectionSelector.ClearenceInterconnection:
              new ClearenceInterconnectionnData().SetRowData( _item.Value, m_StateMachineEngine.NewDataEventHandler );
              break;
            default:
              break;
          }
        }
        catch ( Exception ex )
        {
          string _at = _item.Key.ToString();
          ApplicationError _ae = new ApplicationError( "Page_Load", _at, ex.Message, ex );
          this.Controls.Add( _ae.CreateMessage( _at, true ) );
        }
      }
    }
    #endregion

    #region UserControl override
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit( EventArgs e )
    {
      Page.RegisterRequiresControlState( this );
      m_AvailableGridView.DataSourceID = m_AvailableGridViewDataSource.ID;
      m_AssignedGridView.DataSourceID = m_AssignedGridViewDataSource.ID;
      base.OnInit( e );
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load( object sender, EventArgs e )
    {
      string at = "Starting";
      try
      {
        if ( !IsPostBack )
        {
          at = "InitMahine";
          m_StateMachineEngine.InitMahine();
        }
        at = "Event handlers";
        m_SaveButton.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
        m_NewButton.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
        m_CancelButton.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
        m_EditButton.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
        m_DeleteButton.Click += new EventHandler( m_StateMachineEngine.DeleteButton_Click );
        m_ClearButton.Click += new EventHandler( m_StateMachineEngine.m_ClearButton_Click );
        m_GridViewAddAll.Click += m_GridViewAddAll_Click;
        m_GridViewAddDisplayed.Click += m_GridViewAddDisplayed_Click;
        m_GridViewRemoveAll.Click += m_GridViewRemoveAll_Click;
        m_GridViewRemoveDisplayed.Click += m_GridViewRemoveDisplayed_Click;
      }
      catch ( Exception ex )
      {
        ApplicationError _ae = new ApplicationError( "Page_Load", at, ex.Message, ex );
        this.Controls.Add( _ae.CreateMessage( at, true ) );
      }
    }

    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState( object state )
    {
      if ( state != null )
      {
        m_ControlState = (ControlState)state;
        m_StateMachineEngine.InitMahine( m_ControlState.InterfaceState );
      }
      else
      {
        m_ControlState = new ControlState( null );
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
    protected override void OnPreRender( EventArgs e )
    {
      SetEnabled( m_ControlState.SetEnabled );
      Show();
      m_AvailableGridView.DataBind();
      m_AssignedGridView.DataBind();
      base.OnPreRender( e );
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload( EventArgs e )
    {
      base.OnUnload( e );
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
      #endregion

      #region public
      public ControlState( ControlState _old )
      {
        if ( _old == null )
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
    private class LocalStateMachineEngine: WEB.WebpartStateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine( ClearenceWebPartUserControl parent )
      {
        Parent = parent;
      }
      #endregion

      #region public
      internal void InitMahine( InterfaceState _ControlState )
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
      internal void NewDataEventHandler( object sender, ClearenceInterconnectionnData e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.EditState:
          case InterfaceState.NewState:
            break;
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData( e );
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
          Parent.Update();
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch ( GenericStateMachineEngine.ActionResult ex )
        {
          return ex;
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Update" );
        }
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        try
        {
          Parent.Create();
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch ( GenericStateMachineEngine.ActionResult ex )
        {
          return ex;
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Create" );
        }
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        try
        {
          Parent.Delete();
          return GenericStateMachineEngine.ActionResult.Success;
        }
        catch ( GenericStateMachineEngine.ActionResult ex )
        {
          return ex;
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Create" );
        }
      }
      protected override void ClearUserInterface()
      {
        Parent.ClearAssigned();
      }
      protected override void SetEnabled( GenericStateMachineEngine.ControlsSet _buttons )
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError( GenericStateMachineEngine.InterfaceEvent interfaceEvent )
      {
        ShowActionResult
          ( ActionResult.Exception( new ApplicationError( "SMError", CurrentMachineState.ToString(), "State maschine internal error", null ), "State maschine internal error" ) );
        CurrentMachineState = InterfaceState.ViewState;
      }
      protected override void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
      {
        Parent.ShowActionResult( _rslt );
        base.ShowActionResult( _rslt );
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
      {
        get
        {
          return Parent.m_ControlState.InterfaceState;
        }
        set
        {
          if ( Parent.m_ControlState.InterfaceState == value )
            return;
          Parent.m_ControlState.InterfaceState = value;
          EnterState();
        }
      }
      protected override void EnterState()
      {
        switch ( CurrentMachineState )
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
      internal void m_ClearButton_Click( object sender, EventArgs e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.ViewState:
            ActionResult _resu = Parent.ClearThroughCustom();
            switch ( _resu.LastActionResult )
            {
              case ActionResult.Result.Success:
                break;
              case ActionResult.Result.NotValidated:
                CurrentMachineState = InterfaceState.EditState;
                ShowActionResult( _resu );
                break;
              case ActionResult.Result.Exception:
              default:
                ShowActionResult( _resu );
                break;
            }
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            SMError( InterfaceEvent.NewClick );
            break;
        }
      }
      #endregion

      #region private
      private ClearenceWebPartUserControl Parent { get; set; }
      #endregion

    }
    #endregion

    #region SetInterconnectionData
    private void SetInterconnectionData( ClearenceInterconnectionnData e )
    {
      try
      {
        if ( m_ControlState.ClearanceID.CompareTo( e.ID ) == 0 )
          return;
        m_ControlState.IsModified = true;
        m_ControlState.ClearanceID = e.ID;
        m_ControlState.ClearanceTitle = e.Title;
        m_ClearenceTextBox.Text = e.Title;
        QueryAssigned();
      }
      catch ( Exception _ex )
      {
        ApplicationError _errr = new ApplicationError( "SetInterconnectionData", "ClearenceInterconnectionnData", _ex.Message, _ex );
        this.Controls.Add( _errr.CreateMessage( _errr.At, true ) );
      }
    }
    #endregion

    #region private

    #region business logic
    private void Update()
    {
      if ( CurrentClearence == null )
        throw GenericStateMachineEngine.ActionResult.Exception( null, "Internal error - ClearanceID is null or empty at Update" );
      CurrentClearence.ProcedureCode = SelectedClearenceProcedure.ToString();
      CurrentClearence.ClearenceProcedure = SelectedClearenceProcedure;
      switch ( SelectedGroup )
      {
        case Group.TobaccoNotAllocated:
          UpdateTobaccoNotAllocated();
          break;
        case Group.Tobacco:
        case Group.Dust:
        case Group.Waste:
        case Group.Cartons:
          UpdateDisposals();
          break;
      }
      m_DataContextManagement.DataContext.SubmitChanges();
    }
    private void Create()
    {
      CurrentClearence = Clearence.CreataClearence( m_DataContextManagement.DataContext, "", SelectedClearenceProcedure );
      Update();
    }
    internal void Delete()
    {
      foreach ( Disposal _dx in CurrentClearence.Disposal )
        _dx.Disposal2ClearenceIndex = null;
      m_DataContextManagement.DataContext.SubmitChanges();
      m_DataContextManagement.DataContext.Clearence.DeleteOnSubmit( CurrentClearence );
      ClearAssigned();
      m_DataContextManagement.DataContext.SubmitChanges();
    }
    private void SetEnabled( GenericStateMachineEngine.ControlsSet controlsSet )
    {
      GenericStateMachineEngine.ControlsSet _allowed = (GenericStateMachineEngine.ControlsSet)0xff;
      if ( m_ControlState.ClearanceID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.DeleteOn;
      controlsSet &= _allowed;
      //Buttons
      m_EditButton.Enabled = ( controlsSet & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_ClearButton.Enabled = m_EditButton.Enabled;
      m_CancelButton.Enabled = ( controlsSet & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Enabled = ( controlsSet & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Enabled = ( controlsSet & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteButton.Enabled = ( controlsSet & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
      switch ( m_StateMachineEngine.CurrentState )
      {
        case GenericStateMachineEngine.InterfaceState.ViewState:
          m_FiltersPanel.Enabled = true;
          m_AvailableGridView.Enabled = false;
          m_GridViewActionsPanel.Enabled = false;
          break;
        case GenericStateMachineEngine.InterfaceState.EditState:
          m_FiltersPanel.Enabled = false;
          m_GridViewActionsPanel.Enabled = true;
          AssignedGridViewAddCommandField();
          break;
        case GenericStateMachineEngine.InterfaceState.NewState:
          m_FiltersPanel.Enabled = false;
          m_GridViewActionsPanel.Enabled = true;
          AssignedGridViewAddCommandField();
          break;
      }
    }
    private void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
    {
      Controls.Add( ControlExtensions.CreateMessage( _rslt.ActionException.Message ) );
    }
    private GenericStateMachineEngine.ActionResult ClearThroughCustom()
    {
      try
      {
        string _masterDocumentName = XMLResources.FinishedGoodsExportFormFileName( _clearence.Identyfikator.Value );
        int _sadConsignmentIdentifier = default( int );
        switch ( SelectedGroup )
        {
          case Group.Tobacco:
          case Group.TobaccoNotAllocated:
            DocumentContent _newTobaccoDoc =
              DisposalsFormFactory.GetTobaccoFreeCirculationFormContent( _clearence.Disposal, _clearence.ProcedureCode, _masterDocumentName );
            _sadConsignmentIdentifier = Dokument.PrepareConsignment( SPContext.Current.Web, _newTobaccoDoc, _masterDocumentName, CompensatiionGood.Tobacco );
            break;
          case Group.Waste:
          case Group.Dust:
            DocumentContent _newDustWasteDoc =
              DisposalsFormFactory.GetDustWasteFormContent( _clearence.Disposal, _clearence.ProcedureCode, _masterDocumentName );
            CompensatiionGood _compensatiionGood = SelectedGroup == Group.Waste ?
              CompensatiionGood.Waste : CompensatiionGood.Dust;
            _sadConsignmentIdentifier = Dokument.PrepareConsignment( SPContext.Current.Web, _newDustWasteDoc, _masterDocumentName, _compensatiionGood );
            break;
          case Group.Cartons:
            DocumentContent _newBoxFormContent =
              DisposalsFormFactory.GetBoxFormContent( _clearence.Disposal, _clearence.ProcedureCode, _masterDocumentName );
            _sadConsignmentIdentifier = Dokument.PrepareConsignment( SPContext.Current.Web, _newBoxFormContent, _masterDocumentName, CompensatiionGood.Cartons );
            break;
        }
        SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>( m_DataContextManagement.DataContext.SADConsignment, _sadConsignmentIdentifier );
        _clearence.SADConsignmentLibraryIndex = _sadConsignment;
        m_DataContextManagement.DataContext.SubmitChanges();
        return GenericStateMachineEngine.ActionResult.Success;
      }
      catch ( GenericStateMachineEngine.ActionResult _ar )
      {
        return _ar;
      }
      catch ( Exception ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( ex, "ClearThroughCustom" );
      }
    }
    private void UpdateDisposals()
    {
      Entities edc = m_DataContextManagement.DataContext;
      //remove from clearance
      foreach ( Selection.SelectionTableRow _row in m_ControlState.AvailableItems.SelectionTable.OnlyAdded )
      {
        Linq.IPR.Disposal _dspsl = Element.GetAtIndex<Linq.IPR.Disposal>( edc.Disposal, _row.Identyfikator );
        _dspsl.Disposal2ClearenceIndex = null;
      }
      //add to clearance
      foreach ( Selection.SelectionTableRow _row in m_ControlState.AssignedItems.SelectionTable.OnlyAdded )
      {
        Linq.IPR.Disposal _dspsl = Element.GetAtIndex<Linq.IPR.Disposal>( edc.Disposal, _row.Identyfikator );
        _dspsl.Disposal2ClearenceIndex = CurrentClearence;
      }
    }
    private void UpdateTobaccoNotAllocated()
    {
      Entities _edc = m_DataContextManagement.DataContext;
      //remove for clearance
      foreach ( Selection.SelectionTableRow _row in m_ControlState.AvailableItems.SelectionTable.OnlyDisposals )
      {
        Linq.IPR.Disposal _dspsl = Element.GetAtIndex<Linq.IPR.Disposal>( _edc.Disposal, _row.Identyfikator );
        _dspsl.Disposal2ClearenceIndex = null;
        _dspsl.Disposal2IPRIndex.RevertWithdraw( _dspsl.SettledQuantity );
        _edc.Disposal.DeleteOnSubmit( _dspsl );
      }
      //add to clearance
      foreach ( Selection.SelectionTableRow _row in m_ControlState.AssignedItems.SelectionTable.OnlyAdded )
      {
        if ( _row.Disposal )
          throw SharePoint.Web.GenericStateMachineEngine.ActionResult.NotValidated( "Internal error - disposal is on the added to assigned list" );
        Linq.IPR.IPR _ipr = Element.GetAtIndex<Linq.IPR.IPR>( _edc.IPR, _row.Identyfikator );
        Disposal _nd = new Disposal()
        {
          Disposal2ClearenceIndex = CurrentClearence,
          ClearingType = ClearingType.PartialWindingUp,
          CustomsProcedure = CurrentClearence.ProcedureCode,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr,
          Disposal2MaterialIndex = null,
          PCNCompensationGood = _ipr.IPR2PCNPCN.Title(),
          DisposalStatus = DisposalStatus.Tobacco,
          //DutyAndVAT - in SetUpCalculatedColumns,
          //DutyPerSettledAmount - in SetUpCalculatedColumns,
          InvoiceNo = String.Empty.NotAvailable(),
          IPRDocumentNo = String.Empty.NotAvailable(),
          JSOXCustomsSummaryIndex = null,
          No = new Nullable<double>(),
          Disposal2PCNID = _ipr.IPR2PCNPCN,
          RemainingQuantity = new Nullable<double>(),
          SADDate = SharePoint.Extensions.DateTimeNull,
          SADDocumentNo = String.Empty.NotAvailable(),
          SettledQuantity = _row.Quantity,
          Title = "Creating",
          //VATPerSettledAmount - in SetUpCalculatedColumns,
          //TobaccoValue - in SetUpCalculatedColumns,
        };
        _nd.SetUpCalculatedColumns( ClearingType.PartialWindingUp );
        _edc.Disposal.InsertOnSubmit( _nd );
        _ipr.Withdraw( _row.Quantity );
      }
      _edc.SubmitChanges();
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      double _availableSum = m_ControlState.AvailableItems.SelectionTable.Count > 0 ? ( from _avrx in m_ControlState.AvailableItems.SelectionTable select _avrx ).Sum( x => x.Quantity ) : 0;
      double _assignedSum = m_ControlState.AssignedItems.SelectionTable.Count > 0 ? ( from _avrx in m_ControlState.AssignedItems.SelectionTable select _avrx ).Sum( x => x.Quantity ) : 0;
      m_AvailableGridViewQuntitySumLabel.Text = String.Format( "Quantity {0:F2}", _availableSum );
      m_AssignedGridViewQuantitySumLabel.Text = String.Format( "Quantity {0:F2}", _assignedSum );
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
      m_ControlState.ClearAssigned();
      if ( CurrentClearence == null )
        return;
      List<Selection.SelectionTableRowWraper> _dsposals = ( from _dsx in CurrentClearence.Disposal
                                                            select new Selection.SelectionTableRowWraper( _dsx ) ).ToList();
      m_ControlState.AssignedItems.SelectionTable.Add( _dsposals );
    }
    private void QueryAvailable()
    {
      DateTime? _start = m_AllDate.Checked || m_StartDateTimeControl.IsDateEmpty ? new Nullable<DateTime>() : m_StartDateTimeControl.SelectedDate.Date;
      DateTime? _finish = m_AllDate.Checked || m_EndTimeControl1.IsDateEmpty ? new Nullable<DateTime>() : m_EndTimeControl1.SelectedDate.Date;
      string _currency = m_SelectCurrencyRadioButtonList.SelectedValue.Contains( "All" ) ? String.Empty : m_SelectCurrencyRadioButtonList.SelectedValue;
      List<Selection.SelectionTableRowWraper> _available = null;
      switch ( SelectedGroup )
      {
        case Group.Tobacco:
          GetDisposals( new DisposalStatus[] { DisposalStatus.Overuse, DisposalStatus.SHMenthol }, false, _start, _finish, _currency );
          break;
        case Group.TobaccoNotAllocated:
          GetMaterial( m_ControlState.AvailableItems, _start, _finish, _currency );
          break;
        case Group.Dust:
          GetDisposals( new DisposalStatus[] { DisposalStatus.Dust }, true, _start, _finish, _currency );
          break;
        case Group.Waste:
          GetDisposals( new DisposalStatus[] { DisposalStatus.Waste }, true, _start, _finish, _currency );
          break;
        case Group.Cartons:
          GetDisposals( new DisposalStatus[] { DisposalStatus.Cartons }, false, _start, _finish, _currency );
          break;
        default:
          throw new SharePoint.ApplicationError( "SelectDataDS", "switch", "Internal error - wrong switch case.", null );
      };
      m_ControlState.AvailableItems.SelectionTable.Add( _available );
    }
    private List<Selection.SelectionTableRowWraper> GetMaterial( Selection data, DateTime? start, DateTime? finisch, string currency )
    {
      return ( from _iprx in m_DataContextManagement.DataContext.IPR
               let _batch = _iprx.Batch
               where ( !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0 ) &&
                     ( !start.HasValue || _iprx.CustomsDebtDate >= start.Value ) &&
                     ( !finisch.HasValue || _iprx.CustomsDebtDate <= finisch.Value ) &&
                     ( String.IsNullOrEmpty( currency ) || _iprx.Currency.Contains( currency ) )
               orderby _batch ascending
               select new Selection.SelectionTableRowWraper( _iprx ) ).ToList();
    }
    private List<Selection.SelectionTableRowWraper> GetDisposals( DisposalStatus[] status, bool creationDataFilter, DateTime? start, DateTime? finisch, string currency )
    {
      DisposalStatus _status0 = status[ 0 ];
      DisposalStatus _status1 = status.Length == 2 ? status[ 1 ] : DisposalStatus.Invalid;
      List<Selection.SelectionTableRowWraper> _ret = null;
      _ret = ( from _dspslx in m_DataContextManagement.DataContext.Disposal
               let _ogl = _dspslx.Disposal2IPRIndex.DocumentNo
               let _currency = _dspslx.Disposal2IPRIndex.Currency
               where _dspslx.Disposal2ClearenceIndex == null &&
                     _dspslx.CustomsStatus.Value == CustomsStatus.NotStarted &&
                     ( _dspslx.DisposalStatus.Value == _status0 || _dspslx.DisposalStatus.Value == _status1 ) &&
                     ( String.IsNullOrEmpty( currency ) || _currency.Contains( currency ) )
               orderby _ogl ascending
               select new Selection.SelectionTableRowWraper( _dspslx ) ).ToList();
      if ( creationDataFilter )
        _ret = ( from _itmx in _ret
                 where ( !start.HasValue || _itmx.Created >= start ) && ( !finisch.HasValue || _itmx.Created <= finisch )
                 orderby _itmx.Created ascending
                 select _itmx ).ToList();
      else
        _ret = ( from _itmx in _ret
                 where ( !start.HasValue || _itmx.DebtDate >= start ) && ( !finisch.HasValue || _itmx.DebtDate <= finisch )
                 orderby _itmx.DebtDate ascending
                 select _itmx ).ToList();
      return _ret;
    }
    private ClearenceProcedure SelectedClearenceProcedure
    {
      get
      {
        if ( m_ProcedureRadioButtonList.SelectedIndex < 0 )
          throw GenericStateMachineEngine.ActionResult.NotValidated( "Customs procedure must  be selected" );
        switch ( m_ProcedureRadioButtonList.SelectedValue )
        {
          case "4051":
            return ClearenceProcedure._4051;
          case "3151":
            return ClearenceProcedure._3151;
          default:
            throw GenericStateMachineEngine.ActionResult.Exception( null, "Internal error - wrong Customs procedure is is selected" );
        }
      }
    }
    private enum Group { Tobacco, TobaccoNotAllocated, Dust, Waste, Cartons }
    private Group SelectedGroup
    {
      get
      {
        switch ( m_SelectGroupRadioButtonList.SelectedValue )
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
            return Group.Tobacco;
          default:
            throw new SharePoint.ApplicationError( "SelectedGroup", "switch", "Internal error - wrong switch case.", null );
        };
      }
    }
    #endregion

    #region vars
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = new ControlState( null );
    private DataContextManagement<Entities> m_DataContextManagement = null;
    private const string m_IDItemLabel = "IDItemLabel";
    private const string m_IDEditLabel = "IDEditLabel";
    private const string m_QuantityNewValue = "QuantityNewValue";
    private ObjectDataSource m_AvailableGridViewDataSource;
    private ObjectDataSource m_AssignedGridViewDataSource;
    private Clearence @_clearence = default( Clearence );
    private Clearence CurrentClearence
    {
      get
      {
        if ( @_clearence != null )
          return @_clearence;
        if ( m_ControlState.ClearanceID.IsNullOrEmpty() )
          return null;
        @_clearence = Element.GetAtIndex<Clearence>( m_DataContextManagement.DataContext.Clearence, m_ControlState.ClearanceID );
        return @_clearence;
        ;
      }
      set
      {
        @_clearence = value;
        m_ControlState.ClearanceID = value.Identyfikator.Value.ToString();
        ;
        m_ControlState.ClearanceTitle = value.Title;
      }
    }
    #endregion

    #region Event handlers

    private static void GetRow( SPGridView sender, GridViewSelectEventArgs e, Selection.SelectionTableDataTable destination, Selection.SelectionTableDataTable source )
    {
      if ( sender == null )
        return;
      GridViewRow row = sender.Rows[ e.NewSelectedIndex ];
      string _id = ( (Label)row.FindControlRecursive( m_IDItemLabel ) ).Text;
      destination.GetRow( source, _id );
      e.NewSelectedIndex = -1;
      e.Cancel = true;
    }

    #region AvailableGridView event handlers
    /// <summary>
    /// Handles the RowEditing event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewEditEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowEditing( object sender, GridViewEditEventArgs e )
    {
      SPGridView _sender = sender as SPGridView;
      if ( _sender == null )
        return;
      Label _idLabel = (Label)_sender.Rows[ e.NewEditIndex ].FindControlRecursive( m_IDEditLabel );
      if ( Selection.SelectionTableRow.IsDisposal( _idLabel.Text ) )
        e.Cancel = true;
      else
        _sender.EditIndex = e.NewEditIndex;  //TODO GridView - Split - BUG
    }
    /// <summary>
    /// Handles the RowCancelingEdit event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewCancelEditEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowCancelingEdit( object sender, GridViewCancelEditEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      _sender.EditIndex = -1;
      e.Cancel = true;
    }
    /// <summary>
    /// Handles the RowUpdating event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewUpdateEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      //Update the values.
      GridViewRow row = _sender.Rows[ e.RowIndex ];
      Label _idLabel = (Label)row.FindControlRecursive( m_IDEditLabel );
      double _qtty = default( double );
      Selection.SelectionTableRow _slctdItem = m_ControlState.AvailableItems.SelectionTable.FindByID( _idLabel.Text );
      TextBox _quantityTB = (TextBox)row.FindControlRecursive( m_QuantityNewValue );
      if ( !double.TryParse( _quantityTB.Text, out _qtty ) || ( _slctdItem.Quantity < _qtty ) )
      {
        _quantityTB.Text = _slctdItem.Quantity.ToString( "F2" );
        _quantityTB.BorderColor = System.Drawing.Color.Red;
        _quantityTB.BackColor = System.Drawing.Color.Yellow;
        e.Cancel = true;
        return;
      }
      Selection.SelectionTableRow _assignedRow = m_ControlState.AssignedItems.SelectionTable.FindByID( _idLabel.Text );
      if ( _assignedRow == null )
      {
        m_ControlState.AssignedItems.SelectionTable.ImportRow( _slctdItem );
        _assignedRow = m_ControlState.AssignedItems.SelectionTable.FindByID( _idLabel.Text );
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
    protected void m_AvailableGridView_SelectedIndexChanging( object sender, GridViewSelectEventArgs e )
    {
      GetRow( sender as SPGridView, e, m_ControlState.AssignedItems.SelectionTable, m_ControlState.AvailableItems.SelectionTable );
    }
    #endregion

    #region AssignedGridView
    /// <summary>
    /// Handles the SelectedIndexChanging event of the m_AssignedGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewSelectEventArgs" /> instance containing the event data.</param>
    protected void m_AssignedGridView_SelectedIndexChanging( object sender, GridViewSelectEventArgs e )
    {
      GetRow( sender as SPGridView, e, m_ControlState.AvailableItems.SelectionTable, m_ControlState.AssignedItems.SelectionTable );
    }
    private void AssignedGridViewAddCommandField()
    {
      CommandField _cf = new CommandField()
      {
        HeaderText = "Manage",
        ShowEditButton = false,
        ShowSelectButton = true,
        SelectText = "Split",
      };
      _cf.ItemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
      m_AssignedGridView.Columns.Add( _cf );
    }
    #endregion

    #region m_GridViewActionsPanel
    private void m_GridViewRemoveDisplayed_Click( object sender, EventArgs e )
    {
      for ( int i = m_AssignedGridView.LowestDataItemIndex; i <= m_AssignedGridView.HighestDataItemIndex; i++ )
        m_ControlState.AvailableItems.SelectionTable.GetRow( m_ControlState.AssignedItems.SelectionTable[ i ] );
    }
    private void m_GridViewRemoveAll_Click( object sender, EventArgs e )
    {
      foreach ( Selection.SelectionTableRow _rw in m_ControlState.AssignedItems.SelectionTable )
        m_ControlState.AvailableItems.SelectionTable.GetRow( _rw );
    }
    private void m_GridViewAddDisplayed_Click( object sender, EventArgs e )
    {
      for ( int i = m_AvailableGridView.LowestDataItemIndex; i <= m_AvailableGridView.HighestDataItemIndex; i++ )
        m_ControlState.AssignedItems.SelectionTable.GetRow( m_ControlState.AvailableItems.SelectionTable[ i ] );
    }
    private void m_GridViewAddAll_Click( object sender, EventArgs e )
    {
      foreach ( Selection.SelectionTableRow _rw in m_ControlState.AvailableItems.SelectionTable )
        m_ControlState.AssignedItems.SelectionTable.GetRow( _rw );
    }
    #endregion

    /// <summary>
    /// Handles the ObjectCreating event of the m_GridViewDataSource control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ObjectDataSourceEventArgs" /> instance containing the event data.</param>
    protected void m_GridViewDataSource_ObjectCreating( object sender, ObjectDataSourceEventArgs e )
    {
      e.ObjectInstance = this;
    }

    #endregion

    #endregion
  }
}
