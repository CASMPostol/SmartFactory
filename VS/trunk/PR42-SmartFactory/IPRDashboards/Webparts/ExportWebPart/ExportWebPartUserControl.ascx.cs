using System;
using System.Collections.Generic;
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
    public ExportWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine( this );
      m_DataContextManagement = new DataContextManagement<EntitiesDataContext>( this );
    }
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
      public string InvoiceQuantity = String.Empty;
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
      if ( !IsPostBack )
      {
        m_StateMachineEngine.InitMahine();
      }
      m_SaveButton.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
      m_NewButton.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
      m_CancelButton.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
      m_EditButton.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
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
      internal enum LocalControlsSet { };
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

      #region NewDataEventHandlers'
      internal void NewDataEventHandler( object sender, BatchInterconnectionData e )
      {
        switch ( CurrentMachineState )
        {
          case InterfaceState.ViewState:
            m_Parent.SetInterconnectionData( e );
            break;
          case InterfaceState.EditState:
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
            m_Parent.SetInterconnectionData( e );
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
            m_Parent.SetInterconnectionData( e );
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
        Parent.m_InvoiceTextBox.Text = Parent.m_ControlState.InvoiceTitle;
        Parent.m_InvoiceContentTextBox.Text = Parent.m_ControlState.InvoiceContentTitle;
        Parent.m_InvoiceQuantityTextBox.Text = Parent.m_ControlState.InvoiceQuantity;
        Parent.m_BatchTextBox.Text = Parent.m_ControlState.BatchTitle;
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Update()
      {
        try
        {
          Batch _btch = Element.GetAtIndex<Batch>( Parent.m_DataContextManagement.DataContext.Batch, Parent.m_ControlState.BatchID );
          //TODO [pr4-3465] Invoice Content list - add fields http://itrserver/Bugs/BugDetail.aspx?bid=3465
          InvoiceContent _ic = Element.GetAtIndex<InvoiceContent>( Parent.m_DataContextManagement.DataContext.InvoiceContent, Parent.m_ControlState.InvoiceContentID );
          _ic.BatchID = null;
          _ic.Quantity = Convert.ToDouble( m_Parent.m_InvoiceQuantityTextBox.Text );
          _ic.ProductType = _ic.BatchID.ProductType;
          _ic.Status = true;
          _ic.Tytuł = _ic.BatchID.Tytuł;
          //TODO _ic.Units = _ic.BatchID.SKULookup.u
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
          InvoiceContent _nic = new InvoiceContent()
          {

          };
        }
        catch ( Exception ex )
        {
          return GenericStateMachineEngine.ActionResult.Exception( ex, "Create" );
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        throw new NotImplementedException();
      }
      protected override void ClearUserInterface()
      {
        throw new NotImplementedException();
      }
      protected override void SetEnabled( GenericStateMachineEngine.ControlsSet _buttons )
      {
        throw new NotImplementedException();
      }
      protected override void SMError( GenericStateMachineEngine.InterfaceEvent interfaceEvent )
      {
        throw new NotImplementedException();
      }
      protected override void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
      {
        throw new NotImplementedException();
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
      {
        get
        {
          throw new NotImplementedException();
        }
        set
        {
          throw new NotImplementedException();
        }
      }
      #endregion

      #region private
      private ExportWebPartUserControl m_Parent = null;
      private ExportWebPartUserControl Parent { get; set; }
      #endregion

    }

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
        m_InvoiceContentTextBox.Text = e.Title;
        InvoiceContent _ic = Element.GetAtIndex<InvoiceContent>( m_DataContextManagement.DataContext.InvoiceContent, e.ID );
        m_ControlState.BatchID = _ic.BatchID != null ? _ic.BatchID.Identyfikator.IntToString() : String.Empty;
        m_BatchTextBox.Text = _ic.BatchID != null ? _ic.BatchID.Tytuł : "N/A";
        m_ControlState.BatchTitle = m_BatchTextBox.Text;
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
    }
    private void SetEnabled( GenericStateMachineEngine.ControlsSet _set )
    {
      m_EditButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.EditOn ) != 0;
      m_CancelButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_NewButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.NewOn ) != 0;
      m_SaveButton.Enabled = ( _set & GenericStateMachineEngine.ControlsSet.SaveOn ) != 0;
    }
    #endregion

    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = null;
    private DataContextManagement<EntitiesDataContext> m_DataContextManagement = null;
  }
}
