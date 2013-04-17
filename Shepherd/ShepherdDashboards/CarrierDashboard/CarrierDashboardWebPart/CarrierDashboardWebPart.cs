using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  /// <summary>
  /// Carrier Dashboard Manager WebPart 
  /// </summary>
  [ToolboxItemAttribute(false)]
  public class CarrierDashboardWebPart : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/CarrierDashboardWebPart/CarrierDashboardWebPartUserControl.ascx";
    private CarrierDashboardWebPartUserControl m_Control;
    private InterconnectionDataTable<Shipping> m_SelectedTimeSlot = null;
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based 
    /// implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl(_ascxPath) as CarrierDashboardWebPartUserControl;
      m_Control.Role = Role;
      Controls.Add(m_Control);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      m_Control.SetInterconnectionData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary =
      new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region public
    #region Creator
    /// <summary>
    /// Initializes a new instance of the <see cref="CarrierDashboardWebPart"/> class.
    /// </summary>
    public CarrierDashboardWebPart()
    {
      Role = GlobalDefinitions.Roles.None;
    } 
    #endregion

    #region Personalization properties
    /// <summary>
    /// Gets or sets the role of the hosting dashboard.
    /// </summary>
    /// <value>
    /// The role.
    /// </value>
    [WebBrowsable(true)]
    [Personalizable(PersonalizationScope.Shared)]
    [WebDisplayName("The dashboard role")]
    [WebDescription("The role of the dashboard this web part is located on. Depending on the role the dashboard customizes" +
      " the functionality provided to the user.")]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName("CAS Custom Properties")]
    public GlobalDefinitions.Roles Role { get; set; }
    #endregion

    #region Interconnections Providers
    /// <summary>
    /// Sets the SecurityEscortCatalog provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Security Escort Catalog table interconnection", "SecurityEscortCatalogInterconnection", AllowsMultipleConnections = false)]
    public void SetSecurityEscortCatalogProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.SecurityEscortCatalogInterconnection, _provider);
    }
    /// <summary>
    /// "Route table interconnection.
    /// </summary>
    /// <param name="_provider">The provider interface..</param>
    [ConnectionConsumer("Route table interconnection", "RouteInterconnection", AllowsMultipleConnections = false)]
    public void SetRouteProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.RouteInterconnection, _provider);
    }
    /// <summary>
    /// Sets the shipping provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Shipping table interconnection", "ShippingInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.ShippingInterconnection, _provider);
    }
    /// <summary>
    /// Sets the time slots provider.
    /// </summary>
    /// <param name="_provider">The provider.</param>
    [ConnectionConsumer("Time Slots calendar interconnection", "TimeSlotInterconnection", AllowsMultipleConnections = false)]
    public void SetTimeSlotsProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.TimeSlotInterconnection, _provider);
    }
    /// <summary>
    /// Sets the current user provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Current user table interconnection", "PartnerInterconnection", AllowsMultipleConnections = false)]
    public void SetCurrentUserProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.PartnerInterconnection, _provider);
    }
    /// <summary>
    /// City table interconnection.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("City table interconnection", "CityInterconnection", AllowsMultipleConnections = false)]
    public void SetCityProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.CityInterconnection, _provider);
    }    /// <summary>
    /// Gets the connection interface allowing to get selected entry of <see cref="ShippingOperationInbound"/>.
    /// </summary>
    /// <returns>Returns an instance of the <see cref="IWebPartRow"/> representing <see cref="ShippingOperationInbound"/>.</returns>
    [ConnectionProvider("Shipping Operation", "SelectedShippingOperationProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      if (m_Control == null)
        return new InterconnectionDataTable<Shipping>();
      if (m_SelectedTimeSlot == null)
        m_SelectedTimeSlot = m_Control.GetSelectedShippingOperationInboundInterconnectionData();
      return m_SelectedTimeSlot;
    }
    internal static string ConsumertIDPartnerInterconnection = "PartnerInterconnection";
    internal static string ConsumertIDTimeSlotInterconnection = "TimeSlotInterconnection";
    internal static string ConsumertIDShippingInterconnection = "ShippingInterconnection";
    internal static string ProviderIDSelectedShippingOperationProviderPoint = "SelectedShippingOperationProviderPoint";
    #endregion

    #endregion

  }
}
