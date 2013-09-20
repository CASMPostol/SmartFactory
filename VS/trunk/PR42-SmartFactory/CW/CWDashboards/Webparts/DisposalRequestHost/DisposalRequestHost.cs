//<summary>
//  Title   : Disposal Request SilverLight Host
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.CW.Dashboards.SharePointLib;
using CAS.SmartFactory.CW.Dashboards.Silverlight;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.DisposalRequestHost
{
  /// <summary>
  /// Disposal Request SilverLight <see cref="WebPart"/> Host
  /// </summary>
  [ToolboxItemAttribute(false)]
  public class DisposalRequestHost : WebPart
  {
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Batch list interconnection", "BatchInterconnection", AllowsMultipleConnections = false)]
    public void SetBatchProvider(IWebPartRow _provider)
    {
      m_ProvidersDictionary = _provider;
    }
    #endregion

    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      try
      {
        SilverlightWebControl _slwc = new SilverlightWebControl()
                                          {
                                            Source = "SiteAssets/TestDisposalRequest/DisposalRequestWebPart/DisposalRequestWebPart.xap",
                                          };
        Controls.Add(m_SelectedItemTitle);
        Controls.Add(_slwc);
      }
      catch (Exception ex)
      {
        this.Controls.Add(new ExceptionMessage(ex));
      }
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      SetInterconnectionData(m_ProvidersDictionary);
      base.OnPreRender(e);
    }
    private void SetInterconnectionData(IWebPartRow webPartRow)
    {
      if (webPartRow == null)
      {
        m_SelectedItemTitle.Text = "No item selected";
        return;
      }
      new DisposalRequestInterconnectionData().SetRowData(webPartRow, NewDataEventHandler);
    }
    private void NewDataEventHandler(object sender, DisposalRequestInterconnectionData e)
    {
      m_DisposalRequestId = e.ID;
      m_DisposalRequestTitle = e.Title;
      m_SelectedItemTitle.Text = e.Title;
    }
    private string m_DisposalRequestId = String.Empty;
    private IWebPartRow m_ProvidersDictionary = null;
    private string m_DisposalRequestTitle = String.Empty;
    private LiteralControl m_SelectedItemTitle = new LiteralControl();
  }
}
