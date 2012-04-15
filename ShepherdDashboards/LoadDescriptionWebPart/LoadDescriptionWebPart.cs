using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;

namespace CAS.SmartFactory.Shepherd.Dashboards.LoadDescriptionWebPart
{
  [ToolboxItemAttribute(false)]
  public class LoadDescriptionWebPart : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/LoadDescriptionWebPart/LoadDescriptionWebPartUserControl.ascx";
    private LoadDescriptionWebPartUserControl m_AssociatedUserControl;
    protected override void CreateChildControls()
    {
      string _phase = "Starting";
      try
      {
        m_AssociatedUserControl = (LoadDescriptionWebPartUserControl)Page.LoadControl(_ascxPath);
        _phase = "m_AssociatedUserControl";
        m_AssociatedUserControl.Role = Role;
        _phase = "m_AssociatedUserControl";
        Controls.Add(m_AssociatedUserControl);
      }
      catch (Exception _ex)
      {
        string _frmt = "Cannot lod the user control at: {0} because : {1}";
        Controls.Add(new LiteralControl(String.Format(_frmt, _phase, _ex.Message)));
      }
    }
    protected override void OnPreRender(EventArgs e)
    {
      m_AssociatedUserControl.SetInterconnectionData(m_ProvidesDictionary);
      base.OnPreRender(e);
    }
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region public
    public LoadDescriptionWebPart()
    {
      Role = GlobalDefinitions.Roles.None;
    }

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
    public GlobalDefinitions.Roles Role { get; set; }
    #endregion

    #region  Interconnections Providers
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

    #endregion
  }
}
