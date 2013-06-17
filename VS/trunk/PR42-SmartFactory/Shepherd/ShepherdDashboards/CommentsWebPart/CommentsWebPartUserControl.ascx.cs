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
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CommentsWebPart
{
  /// <summary>
  /// Comments Web Part User Control
  /// </summary>
  public partial class CommentsWebPartUserControl: UserControl
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsWebPartUserControl"/> class.
    /// </summary>
    public CommentsWebPartUserControl()
    {
      m_ControlState = new ControlState();
    }
    #endregion

    #region public
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        switch ( value )
        {
          case GlobalDefinitions.Roles.InboundOwner:
          case GlobalDefinitions.Roles.OutboundOwner:
          case GlobalDefinitions.Roles.Coordinator:
          case GlobalDefinitions.Roles.Supervisor:
          case GlobalDefinitions.Roles.Operator:
            SetVisible( true );
            break;
          case GlobalDefinitions.Roles.Vendor:
          case GlobalDefinitions.Roles.Forwarder:
          case GlobalDefinitions.Roles.Escort:
          case GlobalDefinitions.Roles.Guard:
          case GlobalDefinitions.Roles.None:
            SetVisible( false );
            break;
          default:
            SetVisible( false );
            break;
        }
      }
    }
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
    protected void Page_Load( object sender, EventArgs e ) { }
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
      {
        m_ButtonAddNew.Enabled = false;
        m_TaskLabel.Text = "Not connected";
      }
      else
        m_TaskLabel.Text = m_ControlState.Title;
      base.OnPreRender( e );
    }
    #endregion

    #region SetInterconnectionData
    private void NewDataEventHandler( object sender, CommentsInterconnectionData e )
    {
      SetInterconnectionData( e );
    }
    private void SetInterconnectionData( CommentsInterconnectionData e )
    {
      if ( e.ID.IsNullOrEmpty() || m_ControlState.ShippingID.Contains( e.ID ) )
        return;
      m_ControlState.ShippingID = e.ID;
      m_ControlState.Title = e.Title;
    }
    #endregion

    #region private

    [Serializable]
    private class ControlState
    {

      #region state fields
      public string ShippingID = String.Empty;
      public string Title = String.Empty;
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
    private DataContextManagementAutoDispose<EntitiesDataContext> myDataContextManagement = null;
    private EntitiesDataContext EDC
    {
      get
      {
        if ( myDataContextManagement == null )
          myDataContextManagement = DataContextManagementAutoDispose<EntitiesDataContext>.GetDataContextManagement( this );
        return myDataContextManagement.DataContext;
      }
    }
    private ControlState m_ControlState = null;
    private void ShowActionResult( GenericStateMachineEngine.ActionResult _rslt )
    {
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Success )
        return;
      if ( _rslt.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Exception )
      {
#if DEBUG
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Exception at: {0}/{1} of : {2}." );
        this.Controls.Add( GlobalDefinitions.ErrorLiteralControl( String.Format( _format, _rslt.ActionException.Source, At, _rslt.ActionException.Message ) ) );
#endif
        Anons.WriteEntry( EDC, _rslt.ActionException.Source, _rslt.ActionException.Message );
      }
      else
      {
        string _format = CommonDefinitions.Convert2ErrorMessageFormat( "Validation error at: {0}/{1} of : {2}." );
        this.Controls.Add( GlobalDefinitions.ErrorLiteralControl( String.Format( _format, _rslt.ActionException.Source, At, _rslt.ActionException.Message ) ) );
      }
    }
    private void SetVisible(bool visible)
    {
        //m_ExternalCheckBox.Checked = false;
        //m_ExternalCheckBox.Visible = visible;
        //m_ExternalLabel.Visible = visible;
    }
    protected void m_ButtonAddNew_Click( object sender, EventArgs e )
    {
      try
      {
        Shipping _shpppng = CurrentShipping;
        if ( m_Shipping == null )
          return;
        //if ( m_ExternalCheckBox.Checked && CurrentShipping.PartnerTitle == null )
        //{
        //  m_ExternalCheckBox.Checked = false;
        //  //TODO must be localized http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3842
        //  Parent.Controls.Add( GlobalDefinitions.ErrorLiteralControl( "Partner not set only internal messages allowed." ) );
        //  return;
        //}
        ShippingComments _new = new ShippingComments()
        {
          Body = m_ShippingCommentsTextBox.Text,
          //ExternalComment = m_ExternalCheckBox.Checked && CurrentShipping.PartnerTitle != null,
          ShippingComments2PartnerTitle = CurrentShipping.PartnerTitle,
          ShippingComments2ShippingID = CurrentShipping,
        };
        EDC.Comments.InsertOnSubmit( _new );
        EDC.SubmitChanges();
        m_ShippingCommentsTextBox.Text = String.Empty;
      }
      catch ( Exception ex )
      {
        Parent.Controls.Add( GlobalDefinitions.ErrorLiteralControl( ex.Message ) );
        Anons.WriteEntry( EDC, "m_ShippingCommentsTextBox_TextChanged", ex.Message );
      }
    }
    #endregion

  }
}
