using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart
{
  public partial class LoadDescriptionWebPartUserControl : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    internal void SetInterconnectionData(System.Collections.Generic.Dictionary<CarrierDashboard.InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary)
    {
      throw new NotImplementedException();
    }
    internal GlobalDefinitions.Roles Role { get; set; }
  }
}
