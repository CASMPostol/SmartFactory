﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TruckManager
{
  [ToolboxItemAttribute(false)]
  public class TruckManager : WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard/TruckManager/TruckManagerUserControl.ascx";
    private TruckManagerUserControl m_AssociatedUserControl;
    private Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> m_ProvidesDictionary = new Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow>();
    #endregion

    #region WebPart override
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      m_AssociatedUserControl = (TruckManagerUserControl)Page.LoadControl(_ascxPath);
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

    #region  Interconnections Providers
    /// <summary>
    /// Sets the Truck list provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Truck list interconnection", "TruckInterconnection", AllowsMultipleConnections = false)]
    public void SetShippingProvider(IWebPartRow _provider)
    {
      m_ProvidesDictionary.Add(InboundInterconnectionData.ConnectionSelector.TruckInterconnection, _provider);
    }
    #endregion  

  }
}
