using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

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
      m_AssociatedUserControl.Role = Role;
      Controls.Add(m_AssociatedUserControl);
    }
    private TransportResourcesUserControl m_AssociatedUserControl;
    private Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = new Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow>();
    protected override void OnPreRender(EventArgs e)
    {
      m_AssociatedUserControl.GetData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    #endregion

    #region public
    public enum RolesSet { Carrier, SecurityEscort }

    #region Personalization properties
    /// <summary>
    /// Gets or sets the role of the hosting dashboard.
    /// </summary>
    /// <value>
    /// The role.
    /// </value>
    [WebBrowsable(true)]
    [Personalizable(PersonalizationScope.Shared)]
    [WebDisplayName("The webpart role")]
    [WebDescription("The role of this webpart. Depending on the role the dashboard customizes" +
      " the functionality provided to the user.")]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName("CAS Custom Properties")]
    public RolesSet Role { get; set; }
    #endregion

    /// <summary>
    /// Sets the shipping provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Shipping table interconnection", "ShippingInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add( InterconnectionData.ConnectionSelector.ShippingInterconnection, _provider );
    }
    #endregion
  }
}
