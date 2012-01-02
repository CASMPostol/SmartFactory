using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  [ToolboxItemAttribute(false)]
  public class CarrierDashboardWebPart : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/CarrierDashboardWebPart/CarrierDashboardWebPartUserControl.ascx";
    private CarrierDashboardWebPartUserControl m_Control;
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl(_ascxPath) as CarrierDashboardWebPartUserControl;
      Controls.Add(m_Control);
    }
    protected override void OnPreRender(EventArgs e)
    {
      m_Control.GetData(m_ProvidesDictionary );
      base.OnPreRender(e);
    }
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = 
      new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region public

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
    #endregion

    #endregion
  }
}
