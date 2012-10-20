using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ExportWebPart
{
  /// <summary>
  /// Export Web Part
  /// </summary>
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
    protected override void OnPreRender( EventArgs e )
    {
      m_Control.SetInterconnectionData( m_ProvidersDictionary );
      base.OnPreRender( e );
    }
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Batch list interconnection", "BatchInterconnection", AllowsMultipleConnections = false )]
    public void SetBatchProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.BatchInterconnection, _provider );
    }
    /// <summary>
    /// Sets the InvoiceContentInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "InvoiceContent list interconnection", "InvoiceContentInterconnection", AllowsMultipleConnections = false )]
    public void SetInvoiceContentProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.InvoiceContentInterconnection, _provider );
    }
    /// <summary>
    /// Sets the InvoiceContentInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Invoice Library interconnection", "InvoiceInterconnection", AllowsMultipleConnections = false )]
    public void SetInvoiceProvider( IWebPartRow _provider )
    {
      m_ProvidersDictionary.Add( ConnectionSelector.InvoiceInterconnection, _provider );
    }
    private Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary = new Dictionary<ConnectionSelector, IWebPartRow>();
    #endregion
  }
}
