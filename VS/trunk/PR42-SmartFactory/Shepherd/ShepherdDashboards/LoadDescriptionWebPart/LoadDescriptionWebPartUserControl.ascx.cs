﻿//<summary>
//  Title   : Load Description Web Part User Control
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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SharePoint.Linq;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart
{
  using System.Text;
  using InterfaceState = StateMachineEngine.InterfaceState;

  /// <summary>
  /// Load Description Web Part User Control
  /// </summary>
  public partial class LoadDescriptionWebPartUserControl: UserControl
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadDescriptionWebPartUserControl"/> class.
    /// </summary>
    public LoadDescriptionWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine( this );
    }
    internal void SetInterconnectionData( Dictionary<CarrierDashboard.InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary )
    {
      if ( _ProvidesDictionary.Keys.Contains( InboundInterconnectionData.ConnectionSelector.ShippingInterconnection ) )
        new ShippingInterconnectionData().SetRowData
          ( _ProvidesDictionary[ InboundInterconnectionData.ConnectionSelector.ShippingInterconnection ], m_StateMachineEngine.NewDataEventHandler );
    }
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        switch ( value )
        {
          case GlobalDefinitions.Roles.InboundOwner:
          case GlobalDefinitions.Roles.Vendor:
          case GlobalDefinitions.Roles.Forwarder:
          case GlobalDefinitions.Roles.Escort:
            m_OutboundControlsPanel.Visible = false;
            break;
          case GlobalDefinitions.Roles.OutboundOwner:
          case GlobalDefinitions.Roles.Coordinator:
          case GlobalDefinitions.Roles.Supervisor:
          case GlobalDefinitions.Roles.Operator:
          case GlobalDefinitions.Roles.Guard:
          case GlobalDefinitions.Roles.None:
            m_OutboundControlsPanel.Visible = true;
            break;
          default:
            break;
        };
      }
    }
    #endregion

    #region UserControl override
    [Serializable]
    private class ControlState
    {
      #region state fields
      public string ShippingID = String.Empty;
      public string LoadDescriptionID = String.Empty;
      internal StateMachineEngine.ControlsSet SetEnabled = 0;
      public InterfaceState InterfaceState = InterfaceState.ViewState;
      #endregion

      #region public
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
      try
      {
        if ( !IsPostBack )
        {
          m_LoadDescriptionGridView.AutoGenerateDeleteButton = false;
          m_LoadDescriptionGridView.AutoGenerateColumns = false;
          m_LoadDescriptionGridView.Caption = "Loads".GetLocalizedString();
          //m_LoadDescriptionGridView.Columns.Add(new BoundField() { DataField = "Title", Visible = true, HeaderText = "Title" });
          m_LoadDescriptionGridView.Columns.Add( m_DeliveryNumberBoundField );
          m_LoadDescriptionGridView.Columns.Add( new BoundField() { DataField = "PalletTypes", Visible = true, HeaderText = "LoadDescriptionGridViewPalletType".GetLocalizedString() } );
          m_LoadDescriptionGridView.Columns.Add( new BoundField() { DataField = "NumberOfPallets", Visible = true, HeaderText = "LoadDescriptionGridViewPalletsQty".GetLocalizedString() } );
          m_LoadDescriptionGridView.Columns.Add( new BoundField() { DataField = "Commodity", Visible = true, HeaderText = "LoadDescriptionGridViewCommodity".GetLocalizedString() } );
          m_LoadDescriptionGridView.Columns.Add( m_MarketTitleBoundField );
          m_LoadDescriptionGridView.Columns.Add( new BoundField() { DataField = "ID", Visible = false, HeaderText = "ID" } );
          m_LoadDescriptionGridView.DataKeyNames = new String[] { "ID" };
          //SetVisible(m_AllButtons);
          m_StateMachineEngine.InitMahine();
          if ( m_PalletTypesDropDown.Visible )
          {
            m_PalletTypesDropDown.Items.Add( new ListItem( "PalletTypeEuro".GetLocalizedString(), ( (int)PalletType.Euro ).ToString() ) );
            m_PalletTypesDropDown.Items.Add( new ListItem( "PalletTypeIndustrial".GetLocalizedString(), ( (int)PalletType.Industrial ).ToString() ) );
            m_PalletTypesDropDown.Items.Add( new ListItem( "PalletTypeOther".GetLocalizedString(), ( (int)PalletType.Other ).ToString() ) );
            m_PalletTypesDropDown.DataTextField = "Label";
            m_PalletTypesDropDown.DataValueField = "Index";
            m_PalletTypesDropDown.DataBind();
            m_PalletTypesDropDown.SelectedIndex = 0;
          }
          if ( m_CommodityDropDown.Visible )
          {
            m_CommodityDropDown.DataSource = from _idx in EDC.Commodity
                                             orderby _idx.Tytuł ascending
                                             select new { Label = _idx.Tytuł, Index = _idx.Identyfikator };
            m_CommodityDropDown.DataTextField = "Label";
            m_CommodityDropDown.DataValueField = "Index";
            m_CommodityDropDown.DataBind();
            m_CommodityDropDown.SelectedIndex = 0;
          }
        }
        m_SaveLoadDescriptionButton.Click += new EventHandler( m_StateMachineEngine.SaveButton_Click );
        m_NewLoadDescriptionButton.Click += new EventHandler( m_StateMachineEngine.NewButton_Click );
        m_CancelLoadDescriptionButton.Click += new EventHandler( m_StateMachineEngine.CancelButton_Click );
        m_EditLoadDescriptionButton.Click += new EventHandler( m_StateMachineEngine.EditButton_Click );
        m_DeleteLoadDescriptionButton.Click += new EventHandler( m_StateMachineEngine.DeleteButton_Click );
        m_LoadDescriptionGridView.SelectedIndexChanged += new EventHandler( m_StateMachineEngine.LoadDescriptionGridView_SelectedIndexChanged );

      }
      catch ( Exception _ex )
      {
        throw new ApplicationException( "Page_Load exception: " + _ex.Message, _ex );
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
      SetEnabled( m_ControlState.SetEnabled );
      if ( m_ControlState.LoadDescriptionID.IsNullOrEmpty() )
      {
        m_EditLoadDescriptionButton.Enabled = false;
        m_DeleteLoadDescriptionButton.Enabled = false;
      }
      if ( m_ControlState.ShippingID.IsNullOrEmpty() )
        m_NewLoadDescriptionButton.Enabled = false;
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

    #region State machine
    private class LocalStateMachineEngine: StateMachineEngine
    {
      #region ctor
      public LocalStateMachineEngine( LoadDescriptionWebPartUserControl _parent )
        : base()
      {
        Parent = _parent;
      }
      #endregion

      #region abstract implementation
      protected override ActionResult ShowShipping( ShippingInterconnectionData _shipping )
      {
        return Parent.ShowShipping( _shipping );
      }
      protected override ActionResult ShowShipping()
      {
        return Parent.ShowShipping();
      }
      protected override ActionResult ShowLoadDescription( DataKey dataKey )
      {
        try
        {
          Parent.ShowLoadDescription( dataKey );
          return StateMachineEngine.ActionResult.Success;
        }
        catch ( Exception ex )
        {
          return new ActionResult( ex );
        }
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
      protected override void SetEnabled( ControlsSet _buttons )
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError( StateMachineEngine.InterfaceEvent _interfaceEvent )
      {
        Parent.Controls.Add( GlobalDefinitions.ErrorLiteralControl( String.Format( "State machine error, in {0} the event {1} occured", Parent.m_ControlState.InterfaceState.ToString(), _interfaceEvent.ToString() ) ) );
      }
      protected override void ExceptionCatched( string _source, string _message )
      {
        Parent.ExceptionCatched( _source, _message );
      }
      protected override InterfaceState CurrentMachineState
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
      private LoadDescriptionWebPartUserControl Parent { get; set; }
      #endregion

      #region private
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

    }//LocalStateMachineEngine
    #endregion

    #region Variables
    private ControlState m_ControlState = new ControlState( null );
    private LocalStateMachineEngine m_StateMachineEngine = null;
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
    private StateMachineEngine.ControlsSet m_EditbilityACL = (StateMachineEngine.ControlsSet)short.MaxValue;
    private Shipping m_Shipping;
    private Shipping CurrentShipping
    {
      get
      {
        if ( m_Shipping != null )
          return m_Shipping;
        if ( m_ControlState.ShippingID.IsNullOrEmpty() )
          throw new ApplicationException("Not connected - no shipping associated.");
        m_Shipping = Element.GetAtIndex<Shipping>( EDC.Shipping, m_ControlState.ShippingID );
        return m_Shipping;
      }
    }
    private BoundField m_DeliveryNumberBoundField = new BoundField() { DataField = "DeliveryNumber", Visible = true, HeaderText = "DeliveryNumberBoundFieldHeaderText".GetLocalizedString() };
    private BoundField m_MarketTitleBoundField = new BoundField() { DataField = "MarketTitle", Visible = false, HeaderText = "MarketTitleBoundFieldHeaderText".GetLocalizedString() };
    private bool m_POModified = false;
    #endregion

    #region private methods
    private StateMachineEngine.ActionResult ShowShipping()
    {
      if ( m_ControlState.ShippingID.IsNullOrEmpty() )
      {
        ClearUserInterface( false );
        return StateMachineEngine.ActionResult.Success;
      }
      return ShowShipping( CurrentShipping );
    }
    private StateMachineEngine.ActionResult ShowShipping( ShippingInterconnectionData _interconnectionData )
    {
      if ( m_ControlState.ShippingID == _interconnectionData.ID )
        return StateMachineEngine.ActionResult.Success;
      m_ControlState.ShippingID = _interconnectionData.ID;
      m_Shipping = null;
      return ShowShipping( CurrentShipping );
    }
    private StateMachineEngine.ActionResult ShowShipping( Shipping _sppng )
    {
      try
      {
        ClearUserInterface( _sppng.IsOutbound.Value );
        m_ShippingLabel.Text = _sppng.Tytuł;
        if ( m_MarketDropDown.Visible && _sppng.Shipping2City != null )
        {
          m_MarketDropDown.DataSource = from DestinationMarket _idx in _sppng.Shipping2City.DestinationMarket
                                        let _mkr = _idx.MarketTitle
                                        orderby _mkr.Tytuł ascending
                                        select new { Label = _mkr.Tytuł, Index = _mkr.Identyfikator.Value };
          m_MarketDropDown.DataTextField = "Label";
          m_MarketDropDown.DataValueField = "Index";
          m_MarketDropDown.DataBind();
          m_MarketDropDown.SelectedIndex = -1;
        }
        InitLoadDescriptionGridView( _sppng );
        return StateMachineEngine.ActionResult.Success;
      }
      catch ( Exception ex )
      {
        return new StateMachineEngine.ActionResult( ex );
      }
    }
    private StateMachineEngine.ActionResult Create()
    {
      try
      {
        LoadDescription _ld = new LoadDescription();
        _ld.LoadDescription2ShippingIndex = CurrentShipping;
        List<string> _ve = new List<string>();
        StateMachineEngine.ActionResult _res = Update( _ld, _ve );
        if ( _res.ActionSucceeded )
        {
          //ReportAlert("LoadDescription created");
          _ld.LoadDescription2PartnerTitle = CurrentShipping.PartnerTitle;
          EDC.LoadDescription.InsertOnSubmit( _ld );
          if ( m_POModified )
            CurrentShipping.UpdatePOInfo();
          EDC.SubmitChanges();
          m_ControlState.LoadDescriptionID = _ld.Identyfikator.Value.ToString();
          InitLoadDescriptionGridView( CurrentShipping );
        }
        return _res;
      }
      catch ( Exception ex )
      {
        return new StateMachineEngine.ActionResult( ex );
      }
    }
    private StateMachineEngine.ActionResult Delete()
    {
      if ( m_ControlState.LoadDescriptionID.IsNullOrEmpty() )
        return StateMachineEngine.ActionResult.Success;
      try
      {
        LoadDescription _ld = Element.GetAtIndex<LoadDescription>( EDC.LoadDescription, m_ControlState.LoadDescriptionID );
        //string _msg = String.Format("The {0} load description deleted.", _ld.Tytuł);
        EDC.LoadDescription.RecycleOnSubmit( _ld );
        CurrentShipping.UpdatePOInfo();
        //ReportAlert(_msg);
        EDC.SubmitChanges();
        InitLoadDescriptionGridView( CurrentShipping );
        return StateMachineEngine.ActionResult.Success;
      }
      catch ( Exception ex )
      {
        return new StateMachineEngine.ActionResult( ex );
      }
    }
    private StateMachineEngine.ActionResult Update()
    {
      if ( m_ControlState.LoadDescriptionID.IsNullOrEmpty() )
        return StateMachineEngine.ActionResult.Success;
      try
      {
        LoadDescription _ld = Element.GetAtIndex<LoadDescription>( EDC.LoadDescription, m_ControlState.LoadDescriptionID );
        List<string> _ve = new List<string>();
        StateMachineEngine.ActionResult _res = Update( _ld, _ve );
        if ( _res.ActionSucceeded )
        {
          //ReportAlert("LoadDescription updated");
          if ( m_POModified )
            CurrentShipping.UpdatePOInfo();
          EDC.SubmitChanges();
          InitLoadDescriptionGridView( CurrentShipping );
        }
        return _res;
      }
      catch ( Exception ex )
      {
        return new StateMachineEngine.ActionResult( ex );
      }
    }
    private void ShowLoadDescription( DataKey _dataKey )
    {
      m_ControlState.LoadDescriptionID = _dataKey.Value.ToString();
      LoadDescription _ld = Element.GetAtIndex<LoadDescription>( EDC.LoadDescription, m_ControlState.LoadDescriptionID );
      m_CMRTextBox.Text = _ld.CMRNumber;
      m_CommodityDropDown.Select( _ld.LoadDescription2Commodity );
      m_LoadDescriptionNumberTextBox.Text = _ld.DeliveryNumber;
      m_GoodsQuantityTextBox.Text = _ld.GoodsQuantity.HasValue ? _ld.GoodsQuantity.ToString() : String.Empty;
      m_InvoiceTextBox.Text = _ld.InvoiceNumber;
      m_MarketDropDown.Select( _ld.MarketTitle );
      m_NumberOfPalletsTextBox.Text = _ld.NumberOfPallets.HasValue ? _ld.NumberOfPallets.Value.ToString() : String.Empty;
      m_PalletTypesDropDown.Select( (int)_ld.PalletType );
    }
    private StateMachineEngine.ActionResult Update( LoadDescription _ld, List<string> _ve )
    {
      try
      {
        _ld.CMRNumber = m_CMRTextBox.Text;
        _ld.LoadDescription2Commodity = Element.FindAtIndex<Commodity>( EDC.Commodity, m_CommodityDropDown.SelectedValue );
        _ld.DeliveryNumber = m_LoadDescriptionNumberTextBox.Text;
        _ld.GoodsQuantity = m_GoodsQuantityTextBox.TextBox2Double( _ve );
        _ld.InvoiceNumber = m_InvoiceTextBox.Text;
        _ld.MarketTitle = Element.FindAtIndex<Market>( EDC.Market, m_MarketDropDown.SelectedValue );
        _ld.NumberOfPallets = m_NumberOfPalletsTextBox.TextBox2Double( _ve );
        _ld.PalletType = (PalletType)m_PalletTypesDropDown.SelectedValue.String2Int().Value;
        _ld.Tytuł = _ld.DeliveryNumber;
      }
      catch ( Exception ex )
      {
        string _fm = "UpdateLoadingErrorMessage".GetLocalizedString();
        _ve.Add( String.Format( _fm, ex.Message ) );
      }
      return AddValidationMessages( _ve );
    }
    private void InitLoadDescriptionGridView( Shipping _sppng )
    {
      m_LoadDescriptionGridView.DataSource = from _ldidx in _sppng.LoadDescription
                                             orderby _ldidx.DeliveryNumber
                                             select new
                                             {
                                               Title = _ldidx.Tytuł,
                                               DeliveryNumber = _ldidx.DeliveryNumber,
                                               PalletTypes = _ldidx.PalletType.HasValue ? _ldidx.PalletType.Value : PalletType.Other,
                                               NumberOfPallets = _ldidx.NumberOfPallets,
                                               Commodity = _ldidx.LoadDescription2Commodity == null ? String.Empty : _ldidx.LoadDescription2Commodity.Tytuł,
                                               MarketTitle = _ldidx.MarketTitle == null ? String.Empty : _ldidx.MarketTitle.Tytuł,
                                               ID = _ldidx.Identyfikator.Value
                                             };
      m_LoadDescriptionGridView.DataBind();
      m_LoadDescriptionGridView.SelectedIndex = -1;
    }
    private void ClearUserInterface()
    {
      ClearUserInterface( CurrentShipping == null ? false : CurrentShipping.IsOutbound.Value );
    }
    private void ClearUserInterface( bool _isOutbound )
    {
      m_ControlState.LoadDescriptionID = String.Empty;
      m_LoadDescriptionNumberTextBox.Text = String.Empty;
      m_CMRTextBox.Text = string.Empty;
      m_GoodsQuantityTextBox.Text = string.Empty;
      m_InvoiceTextBox.Text = String.Empty;
      m_NumberOfPalletsTextBox.Text = String.Empty;
      m_MarketDropDown.SelectedIndex = -1;
      m_PalletTypesDropDown.SelectedIndex = -1;
      m_CommodityDropDown.SelectedIndex = -1;
      string _label = default( string );
      _label = _isOutbound ? "DeliveryNumberBoundFieldHeaderTextOut".GetLocalizedString() : "DeliveryNumberBoundFieldHeaderTextIn".GetLocalizedString();
      m_LoadDescriptionNumberLabel.Text = _label;
      m_DeliveryNumberBoundField.HeaderText = _label;
      m_MarketTitleBoundField.Visible = _isOutbound;
    }
    private void SetEnabled( StateMachineEngine.ControlsSet _set )
    {
      _set &= m_EditbilityACL;
      //TextBoxes
      m_CMRTextBox.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_CommodityDropDown.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_GoodsQuantityTextBox.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_InvoiceTextBox.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_LoadDescriptionNumberTextBox.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_MarketDropDown.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_NumberOfPalletsTextBox.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      m_PalletTypesDropDown.Enabled = ( _set & StateMachineEngine.ControlsSet.EditModeOn ) != 0;
      //Buttons
      m_SaveLoadDescriptionButton.Enabled = ( _set & StateMachineEngine.ControlsSet.SaveOn ) != 0;
      m_DeleteLoadDescriptionButton.Enabled = ( _set & StateMachineEngine.ControlsSet.DeleteOn ) != 0;
      m_CancelLoadDescriptionButton.Enabled = ( _set & StateMachineEngine.ControlsSet.CancelOn ) != 0;
      m_EditLoadDescriptionButton.Enabled = ( ( _set & StateMachineEngine.ControlsSet.EditOn ) != 0 );
      m_NewLoadDescriptionButton.Enabled = ( _set & StateMachineEngine.ControlsSet.NewOn ) != 0;
    }
    private void ReportAlert( string _msg )
    {
      AlarmsAndEvents _ae = new AlarmsAndEvents()
      {
        AlarmsAndEventsList2Shipping = CurrentShipping,
        AlarmsAndEventsList2PartnerTitle = CurrentShipping.PartnerTitle,
        Tytuł = _msg,
      };
      EDC.AlarmsAndEvents.InsertOnSubmit( _ae );
      EDC.SubmitChanges();
    }
    private StateMachineEngine.ActionResult AddValidationMessages( List<string> _ve )
    {
      if ( _ve.Count == 0 )
        return StateMachineEngine.ActionResult.Success;
      foreach ( string item in _ve )
        this.Controls.Add( GlobalDefinitions.ErrorLiteralControl( item ) );
      return StateMachineEngine.ActionResult.NotValidated;
    }
    private void ExceptionCatched( string source, string message )
    {
      Anons.WriteEntry( EDC, source, message );
      this.Controls.Add( GlobalDefinitions.ErrorLiteralControl( message ) );
    }

    #region events hanlers
    protected void m_LoadDescriptionNumberTextBox_TextChanged( object sender, EventArgs e )
    {
      m_POModified = true;
    }
    #endregion

    #endregion

  }

}
