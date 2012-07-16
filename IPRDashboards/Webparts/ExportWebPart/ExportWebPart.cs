using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace IPRDashboards.Webparts.ExportWebPart
{
  [ToolboxItemAttribute( false )]
  public class ExportWebPart: WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/IPRDashboards.Webparts/ExportWebPart/ExportWebPartUserControl.ascx";
    private ExportWebPartUserControl m_Control;

    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl( _ascxPath ) as ExportWebPartUserControl;
      Controls.Add( m_Control );
    }
    #region Interconnections Providers
    /// <summary>
    /// Sets the SecurityEscortCatalog provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Security Escort Catalog table interconnection", "SecurityEscortCatalogInterconnection", AllowsMultipleConnections = false )]
    public void SetSecurityEscortCatalogProvider( IWebPartRow _provider )
    {
      //TODO [pr4-3492] Create Export Finisched Goods workflow http://itrserver/Bugs/BugDetail.aspx?bid=3492
      //m_ProvidesDictionary.Add( InboundInterconnectionData.ConnectionSelector.SecurityEscortCatalogInterconnection, _provider );
    }
    #endregion
  }
}
