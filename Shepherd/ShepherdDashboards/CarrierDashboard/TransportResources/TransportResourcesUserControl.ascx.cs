using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources
{
  public partial class TransportResourcesUserControl: UserControl
  {
    #region public
    internal void GetData( Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary )
    {
      foreach ( var item in _ProvidesDictionary )
        switch ( item.Key )
        {
          case InterconnectionData.ConnectionSelector.ShippingInterconnection:
            new ShippingInterconnectionData().SetRowData( _ProvidesDictionary[ item.Key ], NewDataEventHandler );
            break;
          default:
            break;
        }
    }
    internal TransportResources.RolesSet Role { get; set; }
    #endregion

    #region private

    #region vars
    private DataContextManagementAutoDispose<EntitiesDataContext> myDataContextManagement = null;
    public EntitiesDataContext EDC
    {
      get
      {
        if ( myDataContextManagement == null )
          myDataContextManagement = DataContextManagementAutoDispose<EntitiesDataContext>.GetDataContextManagement( this );
        return myDataContextManagement.DataContext;
      }
    }
    #endregion

    #region state management
    [Serializable]
    private class MyControlState
    {
      public string ShippingIdx = String.Empty;
    }
    private MyControlState m_ControlState = new MyControlState();
    protected override void OnInit( EventArgs e )
    {
      Page.RegisterRequiresControlState( this );
      base.OnInit( e );
    }
    protected void Page_Load( object sender, EventArgs e )
    {
      if ( Role == TransportResources.RolesSet.SecurityEscort )
      {
        m_TrailerDropDown.Visible = false;
        m_TrailerHeaderLabel.Visible = false;
        m_TruckHeaderLabel.Text = "Cars".GetLocalizedString();
      }
      m_AddDriverButton.Click += new EventHandler( m_AddDriverButton_Click );
      m_RemoveDriverButton.Click += new EventHandler( m_RemoveDriverButton_Click );
      m_TrailerDropDown.SelectedIndexChanged += new EventHandler( m_TrailerDropDown_SelectedIndexChanged );
      m_TruckDropDown.SelectedIndexChanged += new EventHandler( m_TruckDropDown_SelectedIndexChanged );
    }
    protected override void LoadControlState( object _state )
    {
      if ( _state != null )
        m_ControlState = (MyControlState)_state;
    }
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    #endregion

    #region Connectivity
    private void NewDataEventHandler( object sender, ShippingInterconnectionData e )
    {
      m_ControlState.ShippingIdx = e.ID;
      try
      {
        UpdateUserInterface( EDC );
      }
      catch ( Exception ex )
      {
        string _frmt = "NewDataEventHandlerErrorMesage".GetLocalizedString();
        this.Controls.Add( new LiteralControl( String.Format( _frmt, m_ControlState.ShippingIdx, ex.Message ) ) );
      }
    }
    #endregion

    #region User Interface Actions
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void UpdateUserInterface( EntitiesDataContext edc )
    {
      ClearUserInterface();
      if ( m_ControlState.ShippingIdx.IsNullOrEmpty() )
        return;
      Shipping _Shipping = Element.GetAtIndex<Shipping>( edc.Shipping, m_ControlState.ShippingIdx );
      Partner _prtn = Role == TransportResources.RolesSet.Carrier ? _Shipping.PartnerTitle : _Shipping.Shipping2PartnerTitle;
      if ( _prtn == null )
        return;
      ;
      Dictionary<int, Driver> _drivers = _prtn.Driver.ToDictionary( x => x.Identyfikator.Value );
      foreach ( ShippingDriversTeam item in from idx in _Shipping.ShippingDriversTeam select idx )
      {
        Driver _driver = item.DriverTitle;
        if ( ( Role == TransportResources.RolesSet.SecurityEscort && _driver.Driver2PartnerTitle.ServiceType.Value != ServiceType.SecurityEscortProvider ) ||
           ( Role == TransportResources.RolesSet.Carrier && _driver.Driver2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider ) )
          continue;
        m_DriversTeamListBox.Items.Add( new ListItem( _driver.Tytuł, item.Identyfikator.Value.ToString() ) );
        _drivers.Remove( _driver.Identyfikator.Value );
      }
      foreach ( var item in _drivers )
        m_DriversListBox.Items.Add( new ListItem( item.Value.Tytuł, item.Key.ToString() ) );
      m_ShippingTextBox.Text = _Shipping.Tytuł;
      m_TruckDropDown.Items.Add( new ListItem() );
      foreach ( Truck _item in _prtn.Truck )
      {
        if ( ( Role == TransportResources.RolesSet.SecurityEscort && _item.Truck2PartnerTitle.ServiceType.Value != ServiceType.SecurityEscortProvider ) ||
          ( Role == TransportResources.RolesSet.Carrier && _item.Truck2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider ) )
          continue;
        ListItem _li = new ListItem( _item.Tytuł, _item.Identyfikator.Value.ToString() );
        m_TruckDropDown.Items.Add( _li );
        if ( Role == TransportResources.RolesSet.SecurityEscort )
        {
          if ( _Shipping.Shipping2TruckTitle == _item )
            _li.Selected = true;
        }
        else
          if ( _Shipping.TruckTitle == _item )
            _li.Selected = true;
      }
      m_TrailerDropDown.Items.Add( new ListItem() );
      foreach ( Trailer item in _prtn.Trailer )
      {
        ListItem _li = new ListItem( item.Tytuł, item.Identyfikator.Value.ToString() );
        m_TrailerDropDown.Items.Add( _li );
        if ( _Shipping.TrailerTitle == item )
          _li.Selected = true;
      }
      SetButtons( true );
    }
    private void ClearUserInterface()
    {
      m_ShippingTextBox.Text = String.Empty;
      m_DriversListBox.Items.Clear();
      m_DriversTeamListBox.Items.Clear();
      m_TrailerDropDown.Items.Clear();
      m_TruckDropDown.Items.Clear();
      SetButtons( false );
    }
    private void SetButtons( bool _enabled )
    {
      m_AddDriverButton.Enabled = _enabled && m_DriversListBox.Items.Count > 0;
      m_RemoveDriverButton.Enabled = _enabled && m_DriversTeamListBox.Items.Count > 0;
      m_TrailerDropDown.Enabled = _enabled;
      m_TruckDropDown.Enabled = _enabled;
      m_DriversListBox.Enabled = _enabled;
      m_DriversTeamListBox.Enabled = _enabled;
    }
    #endregion

    #region Evnt Handlers
    private void m_RemoveDriverButton_Click( object sender, EventArgs e )
    {
      try
      {
        ListItem _sel = m_DriversTeamListBox.SelectedItem;
        if ( _sel == null )
          return;
        ShippingDriversTeam _cd = Element.GetAtIndex<ShippingDriversTeam>( EDC.DriversTeam, _sel.Value );
        Shipping _sh = _cd.ShippingIndex;
        _cd.DriverTitle = null;
        _cd.ShippingIndex = null;
        EDC.DriversTeam.DeleteOnSubmit( _cd );
        EDC.SubmitChanges();
        _sh.CalculateState();
        EDC.SubmitChanges();
        UpdateUserInterface( EDC );
      }
      catch ( Exception ex )
      {
        SignalException( "TransportResourcesUserControl.m_RemoveDriverButton_Click", "RemoveDriverButtonErrorMessage".GetLocalizedString(), ex );
      }
    }
    private void m_AddDriverButton_Click( object sender, EventArgs e )
    {
      try
      {
        ListItem _sel = m_DriversListBox.SelectedItem;
        if ( _sel == null )
          return;
        ShippingDriversTeam _cd = new ShippingDriversTeam()
          {
            DriverTitle = Element.GetAtIndex<Driver>( EDC.Driver, _sel.Value ),
            ShippingIndex = Element.GetAtIndex( EDC.Shipping, m_ControlState.ShippingIdx )
          };
        EDC.DriversTeam.InsertOnSubmit( _cd );
        EDC.SubmitChanges();
        _cd.ShippingIndex.CalculateState();
        EDC.SubmitChanges();
        UpdateUserInterface( EDC );
      }
      catch ( Exception ex )
      {
        SignalException( "m_AddDriverButton_Click", "AddDriverButtonErrorMessage".GetLocalizedString(), ex );
      }
    }
    private void m_TruckDropDown_SelectedIndexChanged( object sender, EventArgs e )
    {
      ListItem _li = m_TruckDropDown.SelectedItem;
      if ( _li == null )
        return;
      try
      {
        Shipping _sh = Element.GetAtIndex<Shipping>( EDC.Shipping, m_ControlState.ShippingIdx );
        if ( String.IsNullOrEmpty( _li.Value ) )
          if ( Role == TransportResources.RolesSet.Carrier )
            _sh.TruckTitle = null;
          else
            _sh.Shipping2TruckTitle = null;
        else
        {
          Truck _ct = Element.GetAtIndex<Truck>( EDC.Truck, _li.Value );
          if ( Role == TransportResources.RolesSet.Carrier )
            _sh.TruckTitle = _ct;
          else
            _sh.Shipping2TruckTitle = _ct;
        }
        _sh.CalculateState();
        EDC.SubmitChanges();
      }
      catch ( Exception _ex )
      {
        SignalException( "m_TruckDropDown_SelectedIndexChanged", "TruckDropDownErrorMessage".GetLocalizedString(), _ex );
      }
    }
    private void m_TrailerDropDown_SelectedIndexChanged( object sender, EventArgs e )
    {
      ListItem _li = m_TrailerDropDown.SelectedItem;
      if ( _li == null )
        return;
      try
      {
        Shipping _sh = Element.GetAtIndex<Shipping>( EDC.Shipping, m_ControlState.ShippingIdx );
        if ( String.IsNullOrEmpty( _li.Value ) )
          _sh.TrailerTitle = null;
        else
          _sh.TrailerTitle = Element.GetAtIndex<Trailer>( EDC.Trailer, _li.Value );
        _sh.CalculateState();
        EDC.SubmitChanges();
      }
      catch ( Exception _ex )
      {
        SignalException( "m_TrailerDropDown_SelectedIndexChanged", "TrailerDropDownErrorMessage".GetLocalizedString(), _ex );
      }
    }
    private ListBox SortTextBox( ListBox _listBox )
    {
      var _ldSorted = from ListItem _rng in _listBox.Items orderby _rng.Value ascending select _rng;
      ListBox _ret = new ListBox();
      foreach ( var item in _ldSorted )
        _ret.Items.Add( new ListItem( item.Text, item.Value ) );
      return _ret;
    }
    private void SignalException( string _source, string _format, Exception _ex )
    {
      string _msg = String.Format( _format, _ex.Message );
      this.Controls.Add( new LiteralControl( _msg ) );
      Anons.WriteEntry( EDC, _source, _msg );
    }
    #endregion

    #endregion
  }
}
