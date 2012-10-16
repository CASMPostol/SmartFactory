using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;

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
    }
    #endregion

    #region public
    internal void SetInterconnectionData( Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary )
    {
      foreach ( var _item in m_ProvidersDictionary )
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
          at = "AddColumn";
          //m_AvailableGridView.DataBound += m_AssignedGridView_DataBound;
          //m_AvailableGridView.AllowFiltering = false;
          at = "DataTable";
          Selection _data = new Selection() { };
          var _dataQery = ( from _dspslx in m_DataContextManagement.DataContext.Disposal
                            let _ogl = _dspslx.Disposal2IPRIndex.DocumentNo
                            where _dspslx.CustomsStatus.Value == CustomsStatus.NotStarted && _dspslx.DisposalStatus.Value == DisposalStatus.Dust && _dspslx.ClearenceIndex == null
                            orderby _ogl ascending
                            select new
                            {
                              DocumentNo = _dspslx.Disposal2IPRIndex.DocumentNo,
                              DebtDate = _dspslx.Disposal2IPRIndex.CustomsDebtDate,
                              ValidTo = _dspslx.Disposal2IPRIndex.ValidToDate,
                              SKU = _dspslx.Disposal2IPRIndex.SKU,
                              Batch = _dspslx.Disposal2IPRIndex.Batch,
                              UnitPrice = _dspslx.Disposal2IPRIndex.IPRUnitPrice,
                              Currency = _dspslx.Disposal2IPRIndex.Currency,
                              Quantity = _dspslx.SettledQuantity,
                              Status = _dspslx.DisposalStatus,
                              Created = SharePoint.Extensions.SPMinimum,
                              ID = _dspslx.Identyfikator.Value
                            }
                           );
          at = "foreach";
          foreach ( var _rowx in _dataQery )
          {
            Selection.SelectionTableRow _nr = _data.SelectionTable.NewSelectionTableRow();
            _nr.Batch = _rowx.Batch;
            _nr.Created = _rowx.Created;
            _nr.Currency = _rowx.Currency;
            _nr.DocumentNo = _rowx.DocumentNo;
            _nr.DebtDate = _rowx.DebtDate.GetValueOrDefault( SharePoint.Extensions.SPMinimum );
            _nr.ID = _rowx.ID;
            _nr.SKU = _rowx.SKU;
            _nr.Quantity = _rowx.Quantity.Value;
            _nr.Status = _rowx.Status.ToString();
            _nr.UnitPrice = _rowx.UnitPrice.Value;
            _nr.ValidTo = _rowx.ValidTo.GetValueOrDefault( SharePoint.Extensions.SPMinimum );
            _data.SelectionTable.AddSelectionTableRow( _nr );
          }
          //Persist the table in the ControlState object.
          m_ControlState.AvailableItems = _data;
          at = "DataSource";
          //ObjectDataSource _availableDataSource = GetObjectDataSource();
          //Bind the GridView control to the data source.
          m_AvailableGridViewBindData();
        }
        at = "Event handlers";
        m_SaveButton.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
        m_NewButton.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
        m_CancelButton.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
        //TODO 
        //m_EditButton.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
        m_DeleteButton.Click += new EventHandler( m_StateMachineEngine.DeleteButton_Click );
        m_ClearButton.Click += new EventHandler( m_StateMachineEngine.m_ClearButton_Click );
      }
      catch ( Exception ex )
      {
        ApplicationError _ae = new ApplicationError( "Page_Load", at, ex.Message, ex );
        this.Controls.Add( _ae.CreateMessage( at, true ) );
      }
    }



    private void m_AvailableGridViewBindData()
    {
      m_AvailableGridView.DataSource = m_ControlState.AvailableItems.SelectionTable.DefaultView;
      m_AvailableGridView.DataBind();
    }
    private void AddColumn( DataControlField _column )
    {
      m_AvailableGridView.Columns.Add( _column );
      //m_AssignedGridView.Columns.Add( _column );
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
      public Selection AvailableItems = default( Selection );
      public string SortDirection = "ASC";
      public string SortExpression = String.Empty;
      #endregion

      #region public
      internal string GetSortDirection( string column )
      {
        if ( SortExpression == column )
          if ( SortDirection == "ASC" )
            SortDirection = "DESC";
          else
            SortDirection = "ASC";
        else
        {
          SortExpression = column;
          SortDirection = "DESC";
        }
        return SortDirection;
      }
      internal void UpdateControlState( InvoiceContent _ic )
      {
      }
      internal void Clear()
      {
      }
      public ControlState( ControlState _old )
      {
        if ( _old == null )
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion


      internal void ClearClearance()
      {
        throw new NotImplementedException();
      }
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
      #endregion

      #region NewDataEventHandlers
      internal void NewDataEventHandler( object sender, ClearenceInterconnectionnData e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.EditState:
          case InterfaceState.NewState:
            Parent.SetInterconnectionData( e );
            break;
          case InterfaceState.ViewState:
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
          List<string> _errors = new List<string>();
          if ( _errors.Count > 0 )
            return ActionResult.NotValidated( _errors[ 0 ] );
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Update" );
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        try
        {
          List<string> _errors = new List<string>();
          if ( _errors.Count > 0 )
            return ActionResult.NotValidated( _errors[ 0 ] );
          Clearence _newClearance = new Clearence()
          {
          };
          Parent.m_DataContextManagement.DataContext.Clearence.InsertOnSubmit( _newClearance );
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Create" );
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        try
        {
          InvoiceContent _invc = Element.GetAtIndex<InvoiceContent>( Parent.m_DataContextManagement.DataContext.InvoiceContent, Parent.m_ControlState.ClearanceID );
          Parent.m_ControlState.ClearClearance();
          Parent.m_DataContextManagement.DataContext.InvoiceContent.DeleteOnSubmit( _invc );
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Delete" );
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override void ClearUserInterface()
      {
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
        Parent.Controls.Add( ControlExtensions.CreateMessage( _rslt.ActionException.Message ) );
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
          case InterfaceState.EditState:
            break;
          case InterfaceState.NewState:
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
      }
      catch ( Exception _ex )
      {
        ApplicationError _errr = new ApplicationError( "SetInterconnectionData", "ClearenceInterconnectionnData", _ex.Message, _ex );
        this.Controls.Add( _errr.CreateMessage( _errr.At, true ) );
      }
    }
    #endregion

    #region private
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = new ControlState( null );
    private DataContextManagement<Entities> m_DataContextManagement = null;
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set )
    {
      GenericStateMachineEngine.ControlsSet _allowed = (GenericStateMachineEngine.ControlsSet)0xff;
      if ( m_ControlState.ClearanceID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.DeleteOn;
      _set &= _allowed;
      //Buttons
      //m_EditButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_CancelButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
      //Lodcal controls
      //TODO setup other controls.
    }
    private GenericStateMachineEngine.ActionResult ClearThroughCustom()
    {
      try
      {
        if ( m_ProcedureRadioButtonList.SelectedIndex < 0 )
          return GenericStateMachineEngine.ActionResult.NotValidated( "Customs procedure must  be selected" );
        switch ( m_ProcedureRadioButtonList.SelectedValue )
        {
          case "4051":
            return GenericStateMachineEngine.ActionResult.Exception( new NotImplementedException( "Revert  to free circulation is not implemented yet" ), "ClearThroughCustom" );
          case "3151":
            return Export( SPContext.Current.Web );
        }
      }
      catch ( Exception ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( ex, "ClearThroughCustom" );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Export( SPWeb site )
    {
      //InvoiceLib _invoice = Element.GetAtIndex<InvoiceLib>( m_DataContextManagement.DataContext.InvoiceLibrary, m_ControlState.InvoiceID );
      //foreach ( InvoiceContent item in _invoice.InvoiceContent )
      //{
      //  ActionResult _checkResult = item.InvoiceContent2BatchIndex.ExportPossible( item.Quantity );
      //  if ( _checkResult.Valid )
      //    continue;
      //  foreach ( var _msg in _checkResult )
      //    Controls.Add( ControlExtensions.CreateMessage( _msg ) );
      //  m_ControlState.UpdateControlState( item );
      //  string _frmt = "Cannot proceed with export because the invoice item contains eroors {0}.";
      //  return GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _frmt, item.Title ) );
      //}
      //_invoice.InvoiceLibraryStatus = true;
      //List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm> _consignment = new List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm>();
      //string _customsProcedureCode = Resources.CustomsProcedure3151.GetLocalizedString();
      //Clearence _newClearance = Clearence.CreataClearence( m_DataContextManagement.DataContext, _customsProcedureCode, ClearenceProcedure._3151 );
      //string _masterDocumentName = XMLResources.FinishedGoodsExportFormFileName( _newClearance.Identyfikator.Value );
      //int _position = 1;
      //foreach ( InvoiceContent item in _invoice.InvoiceContent )
      //  item.InvoiceContent2BatchIndex.Export( m_DataContextManagement.DataContext, item, _consignment, _invoice.BillDoc, _customsProcedureCode, _newClearance, _masterDocumentName, ref _position );
      //_invoice.InvoiceLibraryReadOnly = true;
      //int _sadConsignmentIdentifier = Dokument.PrepareConsignment( site, _consignment, _masterDocumentName, _invoice.BillDoc );
      //SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>( m_DataContextManagement.DataContext.SADConsignment, _sadConsignmentIdentifier );
      //_newClearance.SADConsignmentLibraryIndex = _sadConsignment;
      //m_DataContextManagement.DataContext.SubmitChanges();
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      //m_InvoiceTextBox.Text = m_ControlState.InvoiceTitle;
      //m_InvoiceContentTextBox.Text = m_ControlState.InvoiceContentTitle;
      //m_InvoiceQuantityTextBox.Text = m_ControlState.InvoiceQuantity.ToString( CultureInfo.CurrentCulture );
      //m_BatchTextBox.Text = m_ControlState.BatchTitle;
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private CheckBox m_AssignedCheckAll { get; set; }

    #region AvailableGridView event handlers
    protected void m_AvailableGridView_RowEditing( object sender, GridViewEditEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      _sender.EditIndex = e.NewEditIndex;
      m_AvailableGridViewBindData();
    }
    protected void m_AvailableGridView_RowCancelingEdit( object sender, GridViewCancelEditEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      _sender.EditIndex = -1;
      m_AvailableGridViewBindData();
    }
    protected void m_AvailableGridView_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      //Update the values.
      GridViewRow row = _sender.Rows[ e.RowIndex ];
      List<Control> _controls = new List<Control>();
      int _id = ( (Label)FindControlRecursive( row, "IDEditLabel", _controls ) ).Text.String2Int().Value;
      double _qtty = double.Parse( ( (TextBox)FindControlRecursive( row, "QuantityNewValue", _controls ) ).Text );
      Selection.SelectionTableRow _slctdItem = m_ControlState.AvailableItems.SelectionTable.FindByID( _id );
      if ( _slctdItem.Quantity < _qtty )
        SharePoint.Web.GenericStateMachineEngine.ActionResult.NotValidated( "You cannot withdraw more than there is on the stock." );
      _slctdItem.Quantity -= _qtty;
      _sender.EditIndex = -1;
      m_AvailableGridViewBindData();
    }
    protected Control FindControlRecursive( Control rootControl, string controlID, List<Control> _ctrls )
    {

      _ctrls.Add( rootControl );
      if ( rootControl.ID == controlID )
        return rootControl;
      foreach ( Control controlToSearch in rootControl.Controls )
      {
        Control controlToReturn = FindControlRecursive( controlToSearch, controlID, _ctrls );
        if ( controlToReturn != null )
          return controlToReturn;
      }
      return null;
    }
    protected void m_AvailableGridView_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      if ( e.Keys[ "ID" ] == null )
        return;
      int _key = (int)e.Keys[ "ID" ];
      Selection.SelectionTableRow _row = m_ControlState.AvailableItems.SelectionTable.FindByID( _key );
      double Quantity = (double)e.NewValues[ "Quantity" ];
      if ( Quantity > _row.Quantity )
        throw SharePoint.Web.GenericStateMachineEngine.ActionResult.NotValidated( "You cannot withdraw more than there is on the stock." );
      _row.Quantity -= Quantity;
      _sender.EditIndex = -1;
      m_AvailableGridViewBindData();
    }
    protected void m_AvailableGridView_Sorting( object sender, GridViewSortEventArgs e )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      //Retrieve the table from the session object.
      if ( m_ControlState.AvailableItems == null )
        return;
      //Sort the data.
      m_ControlState.AvailableItems.SelectionTable.DefaultView.Sort = e.SortExpression + " " + m_ControlState.GetSortDirection( e.SortExpression );
      m_AvailableGridViewBindData();
    }
    protected void m_AssignedGridView_DataBound( object sender, EventArgs e )
    {
      string at = "starting";
      try
      {
        at = "GridViewRow";
        GridViewRow _heade = m_AvailableGridView.HeaderRow;
        at = "Cells[ 0 ].Controls.Clear";
        _heade.Cells[ 0 ].Controls.Clear();
        at = "new CheckBox";
        m_AssignedCheckAll = new CheckBox() { Text = "All", Checked = false };
        _heade.Cells[ 0 ].Controls.Add( m_AssignedCheckAll );
      }
      catch ( Exception ex )
      {
        ApplicationError _ae = new ApplicationError( "Page_Load", at, ex.Message, ex );
        this.Controls.Add( _ae.CreateMessage( at, true ) );
      }
    }
    /// <summary>
    /// Handles the PageIndexChanging event of the m_AvailableGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="GridViewPageEventArgs" /> instance containing the event data.</param>
    protected void m_AvailableGridView_PageIndexChanging( object sender, GridViewPageEventArgs args )
    {
      GridView _sender = sender as GridView;
      if ( _sender == null )
        return;
      _sender.PageIndex = args.NewPageIndex;
      m_AvailableGridViewBindData();
    }
    #endregion
    
    #region AssignedGridView
    protected void m_AssignedGridView_Sorting( object sender, GridViewSortEventArgs e )
    {

    }
    protected void m_AssignedGridView_RowCancelingEdit( object sender, GridViewCancelEditEventArgs e )
    {

    }
    protected void m_AssignedGridView_RowEditing( object sender, GridViewEditEventArgs e )
    {

    }
    protected void m_AssignedGridView_RowUpdated( object sender, GridViewUpdatedEventArgs e )
    {

    }
    protected void m_AssignedGridView_RowUpdating( object sender, GridViewUpdateEventArgs e )
    {

    }
    #endregion    

    #endregion

  }
}
