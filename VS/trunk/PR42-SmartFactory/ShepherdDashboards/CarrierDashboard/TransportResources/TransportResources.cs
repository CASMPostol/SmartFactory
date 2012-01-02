using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources
{
  [ToolboxItemAttribute(false)]
  public class TransportResources : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/TransportResources/TransportResourcesUserControl.ascx";
    protected override void CreateChildControls()
    {
      m_AssociatedUserControl = (TransportResourcesUserControl)Page.LoadControl(_ascxPath);
      Controls.Add(m_AssociatedUserControl);
    }
    private TransportResourcesUserControl m_AssociatedUserControl;
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    protected override void OnPreRender(EventArgs e)
    {
      m_AssociatedUserControl.GetData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    #endregion

    #region public
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
    /// Sets the shipping provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Shipping table interconnection", "ShippingInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.ShippingInterconnection, _provider);
    }
    #endregion
  }
}
