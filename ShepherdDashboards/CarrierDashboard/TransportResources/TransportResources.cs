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
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary =
  new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    protected override void OnPreRender(EventArgs e)
    {
      m_AssociatedUserControl.GetData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }

    #endregion
    #region public
    //TrailerInterconnection
    [ConnectionConsumer("Trailer table interconnection", "TrailerInterconnection", AllowsMultipleConnections = false)]
    public void SetTrailerProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.TrailerInterconnection, _provider);
    }
    //TruckInterconnection
    [ConnectionConsumer("Truck table interconnection", "TruckInterconnection", AllowsMultipleConnections = false)]
    public void SetTruckProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.TruckInterconnection, _provider);
    }
    #endregion
  }
}
