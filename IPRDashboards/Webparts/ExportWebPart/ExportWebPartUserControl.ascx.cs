using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Linq.IPR;
using Microsoft.SharePoint;
using System.Globalization;

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
      GenericStateMachineEngine.ControlsSet _allowed = (GenericStateMachineEngine.ControlsSet)0xff;
      if ( m_ControlState.InvoiceID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.NewOn;
      if ( m_ControlState.InvoiceContentID.IsNullOrEmpty() )
        _allowed ^= GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.SaveOn | GenericStateMachineEngine.ControlsSet.DeleteOn;
      SetEnabled( m_ControlState.SetEnabled & _allowed );
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
      public string InvoiceContentID = String.Empty;
      public string InvoiceContentTitle = String.Empty;
      public string BatchID = String.Empty;
      public string BatchTitle = String.Empty;
      public double? InvoiceQuantity = new Nullable<double>();
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      public GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      public bool IsModified { get; set; }
      #endregion

      #region public
      internal void Clear()
      {
        InvoiceID = String.Empty;
        InvoiceTitle = String.Empty;
        InvoiceContentID = String.Empty;
        InvoiceContentTitle = String.Empty;
        BatchID = String.Empty;
        BatchTitle = String.Empty;
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
      [Flags]
      internal enum LocalControlsSet { EditBatchCheckBox, ExportButton };
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
            Parent.SetInterconnectionData( e );
            break;
          case InterfaceState.ViewState:
          case InterfaceState.NewState:
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
      protected override GenericStateMachineEngine.ActionResult Show()
      {
        return Parent.Show();
      }
      protected override GenericStateMachineEngine.ActionResult Update()
      {
        try
        {
          List<string> _errors = new List<string>();
          Batch _btch = Element.GetAtIndex<Batch>( Parent.m_DataContextManagement.DataContext.Batch, Parent.m_ControlState.BatchID );
          InvoiceContent _ic = Element.GetAtIndex<InvoiceContent>( Parent.m_DataContextManagement.DataContext.InvoiceContent, Parent.m_ControlState.InvoiceContentID );
          if ( _ic.BatchID.Identyfikator != _btch.Identyfikator )
          {
            _ic.BatchID = _btch;
            _ic.ProductType = _ic.BatchID.ProductType;
            _ic.Units = _ic.BatchID.ProductType.Value.Units();
            _ic.Tytuł = _ic.BatchID.Tytuł;
          }
          double? _nq = Parent.m_InvoiceQuantityTextBox.TextBox2Double( _errors );
          if ( _errors.Count == 0 )
            return ActionResult.NotValidated( _errors[ 0 ] ); //TODO [pr4-3510] Move ActionResult Twp the SharePoint project
          _ic.Quantity = 0;
          _ic.Status = Status.OK;
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
          Batch _btch = Element.GetAtIndex<Batch>( Parent.m_DataContextManagement.DataContext.Batch, Parent.m_ControlState.BatchID );
          InvoiceLib _invc = Element.GetAtIndex<InvoiceLib>( Parent.m_DataContextManagement.DataContext.InvoiceLibrary, Parent.m_ControlState.InvoiceID );
          string _units = String.Empty;
          InvoiceContent _nic = new InvoiceContent()
        {
          BatchID = _btch,
          InvoiceLookup = _invc,
          ItemNo = int.MaxValue,
          ProductType = _btch.ProductType,
          Quantity = Parent.m_InvoiceQuantityTextBox.TextBox2Double( _errors ),
          Status = Linq.IPR.Status.OK,
          Tytuł = _btch.SKULookup.Tytuł,
          Units = _btch.ProductType.Value.Units()
        };
          Parent.m_DataContextManagement.DataContext.InvoiceContent.InsertOnSubmit( _nic );
          //TODO add diagnostic 
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
          return GenericStateMachineEngine.ActionResult.Exception( new NotImplementedException(), "Delete" );
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Delete" );
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override void ClearUserInterface()
      {
        Parent.m_BatchTextBox.Text = String.Empty;
        Parent.m_EditBatchCheckBox.Checked = false;
        Parent.m_InvoiceContentTextBox.Text = String.Empty;
        Parent.m_InvoiceQuantityTextBox.Text = String.Empty;
        Parent.m_InvoiceTextBox.Text = String.Empty;
      }
      protected override void SetEnabled( GenericStateMachineEngine.ControlsSet _buttons )
      {
        // TODO throw new NotImplementedException();
      }
      protected override void SMError( GenericStateMachineEngine.InterfaceEvent interfaceEvent )
      {
        //TODO throw new NotImplementedException();
      }
      protected override void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
      {
        //TODO improve diagnostic
        Parent.Controls.Add( CAS.SharePoint.Web.ControlExtensions.CreateMessage( "Provided data is wrong" ) );
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
      {
        get
        {
          return Parent.m_ControlState.InterfaceState;
        }
        set
        {
          Parent.m_ControlState.InterfaceState = value;
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
        m_InvoiceTextBox.Text = e.Title;
        m_ControlState.InvoiceContentID = string.Empty;
        m_ControlState.InvoiceContentTitle = string.Empty;
        m_InvoiceContentTextBox.Text = string.Empty;
        m_ControlState.BatchID = string.Empty;
        m_ControlState.BatchTitle = string.Empty;
        m_BatchTextBox.Text = string.Empty;
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
        m_ControlState.InvoiceContentID = e.ID;
        m_ControlState.InvoiceContentTitle = e.Title;
        InvoiceContent _ic = Element.GetAtIndex<InvoiceContent>( m_DataContextManagement.DataContext.InvoiceContent, e.ID );
        m_ControlState.BatchID = _ic.BatchID != null ? _ic.BatchID.Identyfikator.IntToString() : String.Empty;
        m_ControlState.BatchTitle = _ic.BatchID != null ? _ic.BatchID.Tytuł : "N/A";
        m_ControlState.InvoiceQuantity = _ic.Quantity;
      }
      catch ( Exception _ex )
      {
        ApplicationError _errr = new ApplicationError( "SetInterconnectionData", "InvoiceContentInterconnectionnData", _ex.Message, _ex );
        this.Controls.Add( _errr.CreateMessage( _errr.At, true ) );
      }
    }
    #endregion

    #region Controls management
    private void SetVisible( GenericStateMachineEngine.ControlsSet set, LocalStateMachineEngine.LocalControlsSet lset )
    {
      SetVisible( set );
    }
    private void SetVisible( GenericStateMachineEngine.ControlsSet _set )
    {
      m_EditButton.Visible = ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_CancelButton.Visible = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Visible = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Visible = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteButton.Visible = ( _set & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
    }
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set, LocalStateMachineEngine.LocalControlsSet lset )
    {
      m_EditBatchCheckBox.Enabled = ( lset & LocalStateMachineEngine.LocalControlsSet.EditBatchCheckBox ) != 0;
      //TODO  [pr4-3540] ExportWebPart - add button Export and column Read Only http://itrserver/Bugs/BugDetail.aspx?bid=3540
      SetEnabled( _set );
    }
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set )
    {
      m_EditButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_CancelButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.DeleteOn ) != 0;
    }
    #endregion

    #region User interface
    internal GenericStateMachineEngine.ActionResult Show()
    {
      m_InvoiceTextBox.Text = m_ControlState.InvoiceTitle;
      m_InvoiceContentTextBox.Text = m_ControlState.InvoiceContentTitle;
      m_InvoiceQuantityTextBox.Text = m_ControlState.InvoiceQuantity.Value.ToString( CultureInfo.CurrentCulture );
      m_BatchTextBox.Text = m_ControlState.BatchTitle;
      return GenericStateMachineEngine.ActionResult.Success;
    }
    #endregion

    #region private variables
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = new ControlState( null );
    private DataContextManagement<EntitiesDataContext> m_DataContextManagement = null;
    #endregion
  }
}
