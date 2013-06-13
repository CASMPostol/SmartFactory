using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.DriversManager
{
  [ToolboxItemAttribute(false)]
  public class DriversManager : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/DriversManager/DriversManagerUserControl.ascx";
    private DriversManagerUserControl m_AssociatedUserControl;
    private Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = new Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region WebPart override
    protected override void CreateChildControls()
    {
      m_AssociatedUserControl = (DriversManagerUserControl)Page.LoadControl(_ascxPath);
      Controls.Add(m_AssociatedUserControl);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      m_AssociatedUserControl.SetInterconnectionData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    #endregion

    #region Interconnections Providers
    /// <summary>
    /// Sets the shipping provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Drivers list interconnection", "DriverInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add( InterconnectionData.ConnectionSelector.DriverInterconnection, _provider );
    }
    #endregion
  }
}
