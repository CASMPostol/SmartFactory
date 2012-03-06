using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;

namespace CAS.SmartFactory.Shepherd.Dashboards.GuardWebPart
{
  [ToolboxItemAttribute(false)]
  public class GuardWebPart : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/GuardWebPart/GuardWebPartUserControl.ascx";
    private GuardWebPartUserControl m_Control;
    protected override void CreateChildControls()
    {
      m_Control = (GuardWebPartUserControl)Page.LoadControl(_ascxPath);
      Controls.Add(m_Control);
    }
    protected override void OnPreRender(EventArgs e)
    {
      m_Control.SetInterconnectionData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary =
      new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion
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
    #endregion
  }
}
