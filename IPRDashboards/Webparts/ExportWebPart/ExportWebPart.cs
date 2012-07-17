using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ExportWebPart
{
  /// <summary>
  /// Export Web Part
  /// </summary>
  //TODO [pr4-3492] Create Export Finisched Goods workflow http://itrserver/Bugs/BugDetail.aspx?bid=3492
  [ToolboxItemAttribute( false )]
  public class ExportWebPart: WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.IPR.Dashboards.Webparts/ExportWebPart/ExportWebPartUserControl.ascx";
    private ExportWebPartUserControl m_Control;

    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl( _ascxPath ) as ExportWebPartUserControl;
      Controls.Add( m_Control );
    }
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Batch table interconnection", "BatchInterconnection", AllowsMultipleConnections = false )]
    public void SetBatchProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.BatchInterconnection, _provider );
    }
    /// <summary>
    /// Sets the SecurityEscortCatalog provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Batch table interconnection", "BatchInterconnection", AllowsMultipleConnections = false )]
    public void SetInvoiceProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.BatchInterconnection, _provider );
    }
    private Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary = new Dictionary<ConnectionSelector, IWebPartRow>();
    #endregion
  }
}
