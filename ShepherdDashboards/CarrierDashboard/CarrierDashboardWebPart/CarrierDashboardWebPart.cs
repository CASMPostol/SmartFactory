﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  [ToolboxItemAttribute(false)]
  public class CarrierDashboardWebPart : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/CarrierDashboardWebPart/CarrierDashboardWebPartUserControl.ascx";
    private CarrierDashboardWebPartUserControl m_Control;
    private InterconnectionDataTable<ShippingOperationInbound> m_SelectedTimeSlot = null;
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl(_ascxPath) as CarrierDashboardWebPartUserControl;
      m_Control.Role = Role;
      Controls.Add(m_Control);
    }
    protected override void OnPreRender(EventArgs e)
    {
      m_Control.GetData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary =
      new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region public
    [WebBrowsable(true)]
    [Personalizable(PersonalizationScope.Shared)]
    [WebDisplayName("The dashboard role")]
    [WebDescription("The role of the dashboard this web part is located on. Depending on the role the dashboard customizes" +
      " the functionality provided to the user.")]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName("CAS Custom Properties")]
    public GlobalDefinitions.Roles Role { get; set; }
    public CarrierDashboardWebPart()
    {
      Role = GlobalDefinitions.Roles.None;
    }
    #region Interconnections Providers
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
    /// Gets the connection interface allowing to get selected entry of <see cref="ShippingOperationInbound"/>.
    /// </summary>
    /// <returns>Returns an instance of the <see cref="IWebPartRow"/> representing <see cref="ShippingOperationInbound"/>.</returns>
    [ConnectionProvider("Shipping Operation", "SelectedShippingOperationProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      if (m_Control == null)
        return new InterconnectionDataTable<ShippingOperationInbound>();
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
