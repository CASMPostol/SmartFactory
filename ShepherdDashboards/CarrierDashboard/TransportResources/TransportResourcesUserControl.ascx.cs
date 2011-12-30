using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources
{
  public partial class TransportResourcesUserControl : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    internal void GetData(System.Collections.Generic.Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary)
    {
      throw new NotImplementedException();
    }
  }
}
