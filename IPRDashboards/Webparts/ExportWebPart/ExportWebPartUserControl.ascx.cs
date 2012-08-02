﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Linq.IPR;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ExportWebPart
{
  public partial class ExportWebPartUserControl: UserControl
  {
    #region ctor
    public ExportWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine( this );
      m_DataContextManagement = new DataContextManagement<EntitiesDataContext>( this );
    }
    #endregion

    #region public
    internal void SetInterconnectionData( Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary )
    {
      foreach ( var _item in m_ProvidersDictionary )
      {
        switch ( _item.Key )
        {
          case ConnectionSelector.BatchInterconnection:
            new BatchInterconnectionData().SetRowData( _item.Value, m_StateMachineEngine.NewDataEventHandler );
            break;
          case ConnectionSelector.InvoiceInterconnection:
            new InvoiceInterconnectionData().SetRowData( _item.Value, m_StateMachineEngine.NewDataEventHandler );
            break;
          case ConnectionSelector.InvoiceContentInterconnection:
            new InvoiceContentInterconnectionnData().SetRowData( _item.Value, m_StateMachineEngine.NewDataEventHandler );
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
        }
        at = "Event handlers";
        m_SaveButton.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
        m_NewButton.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
        m_CancelButton.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
        m_EditButton.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
        m_DeleteButton.Click += new EventHandler( m_StateMachineEngine.DeleteButton_Click );
        m_ExportButton.Click += new EventHandler( m_StateMachineEngine.m_ExportButton_Click );
      }
      catch ( Exception ex )
      {
        ApplicationError _ae = new ApplicationError( "Page_Load", "", ex.Message, ex );
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
      public string InvoiceID = String.Empty;
      public string InvoiceTitle = String.Empty;
      public bool ReadOnly;
      public string InvoiceContentID = String.Empty;
      public string InvoiceContentTitle = String.Empty;
      public string BatchID = String.Empty;
      public string BatchTitle = String.Empty;
      public double InvoiceQuantity = 0;
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      public GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      public bool IsModified { get; set; }
      #endregion

      #region public
      internal void UpdateControlState( InvoiceContent _ic )
      {
        InvoiceContentID = _ic.Identyfikator.IntToString();
        InvoiceContentTitle = _ic.Tytuł;
        BatchID = _ic.BatchID != null ? _ic.BatchID.Identyfikator.IntToString() : String.Empty;
        BatchTitle = _ic.BatchID != null ? _ic.BatchID.Tytuł : "N/A";
        InvoiceQuantity = _ic.Quantity.Value;
      }
      internal void Clear()
      {
        InvoiceID = String.Empty;
        InvoiceTitle = String.Empty;
        ReadOnly = false;
        ClearInvoiceContent();
      }
      internal void ClearInvoiceContent()
      {
        InvoiceContentID = String.Empty;
        InvoiceContentTitle = String.Empty;
        BatchID = String.Empty;
        BatchTitle = String.Empty;
        InvoiceQuantity = 0;
      }
      public ControlState( ControlState _old )
      {
        if ( _old == null )
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion

    }
    private class LocalStateMachineEngine: GenericStateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine( ExportWebPartUserControl parent )
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
      internal void NewDataEventHandler( object sender, BatchInterconnectionData e )
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
      internal void NewDataEventHandler( object sender, InvoiceInterconnectionData e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData( e );
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            break;
        }
      }
      internal void NewDataEventHandler( object sender, InvoiceContentInterconnectionnData e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData( e );
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
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
          double? _nq = Parent.m_InvoiceQuantityTextBox.TextBox2Double( _errors );
          if ( _errors.Count > 0 )
            return ActionResult.NotValidated( _errors[ 0 ] );
          if ( _nq.HasValue && _nq.Value < 0 )
            return ActionResult.NotValidated
              ( String.Format( Resources.NegativeValueNotAllowed.GetLocalizedString( GlobalDefinitions.RootResourceFileName ), Parent.m_InvoiceQuantityLabel.Text ) );
          Batch _batch = Element.GetAtIndex<Batch>( Parent.m_DataContextManagement.DataContext.Batch, Parent.m_ControlState.BatchID );
          if ( !_batch.Available( _nq.Value ) )
          {
            string _tmplt = Resources.NeBatchQuantityIsUnavailable.GetLocalizedString( GlobalDefinitions.RootResourceFileName );
            return ActionResult.NotValidated( String.Format( CultureInfo.CurrentCulture, _tmplt, _batch.AvailableQuantity() ) );
          }
          InvoiceContent _ic = Element.GetAtIndex<InvoiceContent>( Parent.m_DataContextManagement.DataContext.InvoiceContent, Parent.m_ControlState.InvoiceContentID );
          _ic.Quantity = _nq;
          _ic.Status = Status.OK;
          if ( _ic.BatchID.Identyfikator != _batch.Identyfikator )
          {
            _ic.BatchID = _batch;
            _ic.ProductType = _ic.BatchID.ProductType;
            _ic.Units = _ic.BatchID.ProductType.Value.Units();
            _ic.Quantity = _nq;
            _ic.CreateTitle();
          }
          Parent.m_ControlState.InvoiceQuantity = _ic.Quantity.Value;
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
          double? _nq = Parent.m_InvoiceQuantityTextBox.TextBox2Double( _errors );
          if ( _errors.Count > 0 )
            return ActionResult.NotValidated( _errors[ 0 ] );
          if ( _nq.HasValue && _nq.Value < 0 )
            return ActionResult.NotValidated
              ( String.Format( Resources.NegativeValueNotAllowed.GetLocalizedString( GlobalDefinitions.RootResourceFileName ), Parent.m_InvoiceQuantityLabel.Text ) );
          Batch _batch = Element.GetAtIndex<Batch>( Parent.m_DataContextManagement.DataContext.Batch, Parent.m_ControlState.BatchID );
          InvoiceLib _invc = Element.GetAtIndex<InvoiceLib>( Parent.m_DataContextManagement.DataContext.InvoiceLibrary, Parent.m_ControlState.InvoiceID );
          if ( !_batch.Available( _nq.Value ) )
          {
            string _tmplt = Resources.QuantityIsUnavailable.GetLocalizedString( GlobalDefinitions.RootResourceFileName );
            return ActionResult.NotValidated( String.Format( CultureInfo.CurrentCulture, _tmplt, _batch.AvailableQuantity() ) );
          }
          InvoiceContent _nic = new InvoiceContent()
          {
            BatchID = _batch,
            InvoiceLookup = _invc,
            SKUDescription = _batch.SKULookup.Tytuł,
            ProductType = _batch.ProductType,
            Quantity = _nq.Value,
            Status = Linq.IPR.Status.OK,
            Tytuł = _batch.SKULookup.Tytuł,
            Units = _batch.ProductType.Value.Units()
          };
          Parent.m_ControlState.InvoiceQuantity = _nq.Value;
          Parent.m_DataContextManagement.DataContext.InvoiceContent.InsertOnSubmit( _nic );
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
          _nic.CreateTitle();
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
          InvoiceContent _invc = Element.GetAtIndex<InvoiceContent>( Parent.m_DataContextManagement.DataContext.InvoiceContent, Parent.m_ControlState.InvoiceContentID );
          Parent.m_ControlState.ClearInvoiceContent();
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
        Parent.m_ControlState.ClearInvoiceContent();
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
            Parent.m_EditBatchCheckBox.Checked = false;
            break;
          case InterfaceState.NewState:
            Parent.m_EditBatchCheckBox.Checked = true;
            break;
        }
        base.EnterState();
      }
      #endregion

      #region Event handlers
      internal void m_ExportButton_Click( object sender, EventArgs e )
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
      private ExportWebPartUserControl Parent { get; set; }
      #endregion

    }
    #endregion

    #region SetInterconnectionData
    private void SetInterconnectionData( BatchInterconnectionData e )
    {
      if ( m_EditBatchCheckBox.Checked && m_ControlState.BatchID.CompareTo( e.ID ) != 0 )
      {
        m_ControlState.IsModified = true;
        m_ControlState.BatchID = e.ID;
        m_ControlState.BatchTitle = e.Title;
        m_BatchTextBox.Text = e.Title;
      }
    }
    private void SetInterconnectionData( InvoiceInterconnectionData e )
    {
      try
      {
        if ( m_ControlState.InvoiceID.CompareTo( e.ID ) == 0 )
          return;
        m_ControlState.InvoiceID = e.ID;
        m_ControlState.InvoiceTitle = e.Title;
        m_ControlState.ReadOnly = e.ReadOnly;
        m_ControlState.ClearInvoiceContent();
      }
      catch ( Exception _ex )
      {
        ApplicationError _errr = new ApplicationError( "SetInterconnectionData", "InvoiceInterconnectionData", _ex.Message, _ex );
        this.Controls.Add( _errr.CreateMessage( _errr.At, true ) );
      }
    }
    private void SetInterconnectionData( InvoiceContentInterconnectionnData e )
    {
      try
      {
        if ( m_ControlState.InvoiceContentID.CompareTo( e.ID ) == 0 )
          return;
        InvoiceContent _ic = Element.FindAtIndex<InvoiceContent>( m_DataContextManagement.DataContext.InvoiceContent, e.ID );
        if ( _ic == null )
        {
          m_ControlState.ClearInvoiceContent();
          return;
        }
        m_ControlState.UpdateControlState( _ic );
      }
      catch ( Exception _ex )
      {
        ApplicationError _errr = new ApplicationError( "SetInterconnectionData", "InvoiceContentInterconnectionnData", _ex.Message, _ex );
        this.Controls.Add( _errr.CreateMessage( _errr.At, true ) );
      }
    }
    #endregion

    #region private
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = new ControlState( null );
    private DataContextManagement<EntitiesDataContext> m_DataContextManagement = null;
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set )
    {
      GenericStateMachineEngine.ControlsSet _allowed = m_ControlState.ReadOnly ? 0 : (GenericStateMachineEngine.ControlsSet)0xff;
      if ( m_ControlState.InvoiceID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.NewOn;
      if ( m_ControlState.InvoiceContentID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.DeleteOn;
      _set &= _allowed;
      //Buttons
      m_EditButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_CancelButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
      //Lodcal controls
      m_EditBatchCheckBox.Enabled = m_CancelButton.Enabled;
      m_InvoiceQuantityTextBox.Enabled = m_CancelButton.Enabled;
      m_ExportButton.Enabled = m_NewButton.Enabled && !m_ControlState.InvoiceID.IsNullOrEmpty() && !m_ControlState.ReadOnly;
    }
    private GenericStateMachineEngine.ActionResult ClearThroughCustom()
    {
      try
      {
        if ( m_ExportProcedureRadioButtonList.SelectedIndex < 0 )
          return GenericStateMachineEngine.ActionResult.NotValidated( "Customs procedure must  be selected" );
        switch ( m_ExportProcedureRadioButtonList.SelectedValue )
        {
          case "Export":
            return Export();
          case "Revert":
            return GenericStateMachineEngine.ActionResult.Exception( new NotImplementedException( "Revert FG to free circulation is not implemented yet" ), "ClearThroughCustom" );
        }
      }
      catch ( Exception ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( ex, "ClearThroughCustom" );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Export()
    {
      InvoiceLib _invoice = Element.GetAtIndex<InvoiceLib>( m_DataContextManagement.DataContext.InvoiceLibrary, m_ControlState.InvoiceID );
      foreach ( InvoiceContent item in _invoice.InvoiceContent )
      {
        ActionResult _checkResult = item.BatchID.ExportPossible( item.Quantity );
        if ( _checkResult.Valid )
          continue;
        foreach ( var _msg in _checkResult )
          Controls.Add( ControlExtensions.CreateMessage( _msg ) );
        m_ControlState.UpdateControlState( item );
        string _frmt = "Cannot proceed with export because the invoice item contains eroors {0}.";
        return GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _frmt, item.Tytuł ) );
      }
      _invoice.OK = true;
      List<ExportConsignment> _consignment = new List<ExportConsignment>();
      string _customsProcedureCode = Resources.CustomsProcedure3151.GetLocalizedString( GlobalDefinitions.RootResourceFileName );
      string _title = Resources.FinishedGoodsExportFormFileName;
      Clearence _newClearance = Clearence.CreataClearence( m_DataContextManagement.DataContext, _title, _customsProcedureCode, Procedure._3151 );
      foreach ( InvoiceContent item in _invoice.InvoiceContent )
        item.BatchID.Export( m_DataContextManagement.DataContext, item, _consignment, _invoice.BillDoc, _customsProcedureCode, _newClearance );
      _invoice.ReadOnly = true;
      m_DataContextManagement.DataContext.SubmitChanges();
      return InvoiceLib.PrepareConsignment( m_DataContextManagement.DataContext, _consignment );
    }
    private GenericStateMachineEngine.ActionResult Show()
    {
      m_InvoiceTextBox.Text = m_ControlState.InvoiceTitle;
      m_InvoiceContentTextBox.Text = m_ControlState.InvoiceContentTitle;
      m_InvoiceQuantityTextBox.Text = m_ControlState.InvoiceQuantity.ToString( CultureInfo.CurrentCulture );
      m_BatchTextBox.Text = m_ControlState.BatchTitle;
      return GenericStateMachineEngine.ActionResult.Success;
    }
    #endregion

  }
}
