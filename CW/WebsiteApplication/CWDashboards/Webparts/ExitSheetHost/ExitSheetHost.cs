//<summary>
//  Title   : Exit Sheet SilverLight Host
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.CW.Dashboards.Silverlight;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost
{
  /// <summary>
  /// class ExitSheetHost as WebPart
  /// </summary>
  [ToolboxItemAttribute(false)]
  public class ExitSheetHost : WebPart
  {

    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="provider">The provider interface.</param>
    [ConnectionConsumer("Disposal list interconnection", "DisposalInterconnection", AllowsMultipleConnections = false)]
    public void SetBatchProvider(IWebPartRow provider)
    {
      m_ProvidersDictionary = provider;
    }
    #endregion

    #region private
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      try
      {
        Controls.Add(m_SelectedItemTitle);
        Controls.Add(m_HiddenFieldData);
        m_slwc = new SilverlightWebControl() { Source = CommonDefinition.SilverlightExitSheetWebPartPath };
        Controls.Add(m_slwc);
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
      m_slwc.AddInitParams(new InitParam(CommonDefinition.HiddenFieldDataParameterName, m_HiddenFieldData.ClientID));
      base.OnPreRender(e);
    }
    private void SetInterconnectionData(IWebPartRow webPartRow)
    {
      if (webPartRow == null)
      {
        m_SelectedItemTitle.Text = "No item selected";
        return;
      }
      new DisposalInterconnectionData().SetRowData(webPartRow, NewDataEventHandler);
    }
    private void NewDataEventHandler(object sender, DisposalInterconnectionData e)
    {
      if (string.IsNullOrEmpty(e.ID))
        return;
      EnsureChildControls();
      m_SelectedItemTitle.Text = e.Title;
      using (Entities _edx = new Entities())
      {
        CustomsWarehouseDisposal _cwd = Element.GetAtIndex<CustomsWarehouseDisposal>(_edx.CustomsWarehouseDisposal, e.ID);
        ExitSheeDataContract _dc = ExitSheeDataContract.GetExitSheeDataContract(_edx, _cwd);
        m_HiddenFieldData.Value = _dc.Serialize();
      }
    }
    private IWebPartRow m_ProvidersDictionary = null;
    private LiteralControl m_SelectedItemTitle = new LiteralControl();
    private HiddenField m_HiddenFieldData = new HiddenField();
    private SilverlightWebControl m_slwc = null;
    #endregion

  }
}