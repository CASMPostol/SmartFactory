using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  [ToolboxItemAttribute( false )]
  public class ClearenceWebPart: WebPart
  {
    #region WebPart override
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.IPR.Dashboards.Webparts/ClearenceWebPart/ClearenceWebPartUserControl.ascx";
    private ClearenceWebPartUserControl m_control;
    protected override void CreateChildControls()
    {
      m_control = Page.LoadControl( _ascxPath ) as ClearenceWebPartUserControl;
      Controls.Add( m_control );
    }
    protected override void OnPreRender( EventArgs e )
    {
      m_control.SetInterconnectionData( m_ProvidersDictionary );
      base.OnPreRender( e );
    }
    #endregion

    #region Interconnections Providers
    /// <summary>
    /// Sets the ClearanceInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Clearance list interconnection", "ClearanceInterconnection", AllowsMultipleConnections = false )]
    public void SetBatchProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.ClearenceInterconnection, _provider );
    }
    private Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary = new Dictionary<ConnectionSelector, IWebPartRow>();
    #endregion

  }
}
