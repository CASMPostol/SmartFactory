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
    //ShippingInterconnection
    [ConnectionConsumer("Shipping table interconnection", "ShippingInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.ShippingInterconnection, _provider);
    }
    //TimeSlotInterconnection
    [ConnectionConsumer("Time Slots calendar interconnection", "TimeSlotInterconnection", AllowsMultipleConnections = false)]
    public void SetTimeSlotsProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.TimeSlotInterconnection, _provider);
    }
    //PartnerInterconnection
    [ConnectionConsumer("Current user table interconnection", "PartnerInterconnection", AllowsMultipleConnections = false)]
    public void SetWarehouseProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.PartnerInterconnection, _provider);
    }
    #endregion

    #endregion

  }
}
