//<summary>
//  Title   : Comments Web Part User Control
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CommentsWebPart
{
  /// <summary>
  /// Task Management User Control
  /// </summary>
  [CLSCompliant( false )]
  public partial class CommentsWebPartUserControl: UserControl
  {
    #region creator
    public CommentsWebPartUserControl()
    {
      try
      {
        At = "DataContextManagement";
        m_ControlState = new ControlState();
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "WorkloadManagementUserControl" ) );
      }
    }
    #endregion

    #region public
    internal void SetInterconnectionData( IWebPartRow provider )
    {
      try
      {
        new CommentsInterconnectionData().SetRowData( provider, NewDataEventHandler );
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "SetInterconnectionData" ) );
      }
    }
    private void NewDataEventHandler( object sender, CommentsInterconnectionData e )
    {
      SetInterconnectionData( e );
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
    /// Page Load method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load( object sender, EventArgs e )
    {
      try
      {
        if ( !this.IsPostBack )
        {
          At = "InitMahine";
        }
        At = "Events handlers";
        m_ButtonAddNew.Click += m_ButtonAddNew_Click;
      }
      catch ( ApplicationError _ax )
      {
        this.Controls.Add( _ax.CreateMessage( At, true ) );
      }
      catch ( Exception _ex )
      {
        ShowActionResult( GenericStateMachineEngine.ActionResult.Exception( _ex, "Page_Load" ) );
      }
    }
    private void m_ButtonAddNew_Click( object sender, EventArgs e )
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="savedState">The state.</param>
    protected override void LoadControlState( object savedState )
    {
      if ( savedState != null )
        m_ControlState = (ControlState)savedState;
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
      if ( m_ControlState.ShippingID.IsNullOrEmpty() )
        m_ButtonAddNew.Enabled = false;
      base.OnPreRender( e );
    }
    #endregion

    #region SetInterconnectionData
    private void SetInterconnectionData( CommentsInterconnectionData e )
    {
      if ( e.ID.IsNullOrEmpty() || m_ControlState.ShippingID.Contains( e.ID ) )
        return;
      m_ControlState.ShippingID = e.ID;
    }
    #endregion

    #region private

    #region State machine
    private GenericStateMachineEngine.ActionResult CreateTask()
    {
      if ( !Page.IsValid )
        return GenericStateMachineEngine.ActionResult.NotValidated( "Required information must be provided." );
      try
      {
        if ( m_ControlState.ShippingID.IsNullOrEmpty() )
          return GenericStateMachineEngine.ActionResult.NotValidated( "Project is not selected." );
        At = "InsertOnSubmit";
        //TODO EDC.Comme.InsertOnSubmit( _newTask );
        At = "SubmitChanges #1";
        EDC.SubmitChanges();
      }
      catch ( GenericStateMachineEngine.ActionResult _ar )
      {
        return _ar;
      }
      catch ( ApplicationError _ax )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ax, "CreateNewWokload at: " + At );
      }
      catch ( Exception _ex )
      {
        return GenericStateMachineEngine.ActionResult.Exception( _ex, "CreateNewWokload at: " + At );
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    #endregion

    #region vars
    [Serializable]
    private class ControlState
    {

      #region state fields
      public string ShippingID = String.Empty;
      #endregion

    }
    private string At { get; set; }
    private Shipping m_Shipping;
    private Shipping CurrentShipping
    {
      get
      {
        if ( m_Shipping != null )
          return m_Shipping;
        if ( m_ControlState.ShippingID.IsNullOrEmpty() )
          return null;
        m_Shipping = Element.GetAtIndex<Shipping>( EDC.Shipping, m_ControlState.ShippingID );
        return m_Shipping;
      }
    }
    private DataContextManagement<EntitiesDataContext> myDataContextManagement = null;
    public EntitiesDataContext EDC
    {
      get
      {
        if ( myDataContextManagement == null )
          myDataContextManagement = new DataContextManagement<EntitiesDataContext>( this );
        return myDataContextManagement.DataContext;
      }
    }
    private ControlState m_ControlState = null;
    #endregion

    #region events handling
    private void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
    {
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Success )
        return;
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Exception )
      {
#if DEBUG
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Exception at: {0}/{1} of : {2}." );
        this.Controls.Add( new Literal() { Text = String.Format( _format, _rslt.ActionException.Source, At, _rslt.ActionException.Message ) } );
#endif
        Anons.WriteEntry( EDC, _rslt.ActionException.Source, _rslt.ActionException.Message );
      }
      else
      {
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Validation error at: {0}/{1} of : {2}." );
        this.Controls.Add( new Literal() { Text = String.Format( _format, _rslt.ActionException.Source, At, _rslt.ActionException.Message ) } );
      }
    }
    protected void m_TaskCommentsTextBox_TextChanged( object sender, EventArgs e )
    {

    }
    #endregion

    #endregion

  }
}
