//_______________________________________________________________
//  Title   : GenerateSadConsignmentHost
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using CAS.SmartFactory.CW.Dashboards.Silverlight;
using CAS.SmartFactory.CW.Dashboards.SharePointLib;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.GenerateSadConsignmentHost
{
  [ToolboxItemAttribute(false)]
  public class GenerateSadConsignmentHost : WebPart
  {
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer("Disposal Request list interconnection", "DisposalRequestConsumer", AllowsMultipleConnections = false)]
    public void SetBatchProvider(IWebPartRow _provider)
    {
      m_ProvidersDictionary = _provider;
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
        m_SilverlightWebControl = new SilverlightWebControl() { Source = CommonDefinition.SilverlightGenerateSadConsignmentHost };
        Controls.Add(m_SilverlightWebControl);
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
      m_SilverlightWebControl.AddInitParams(new InitParam(CommonDefinition.HiddenFieldDataParameterName, m_HiddenFieldData.ClientID));
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
        DisposalRequestLib _drl = Element.GetAtIndex<DisposalRequestLib>(_edx.DisposalRequestLibrary, e.ID);
        CheckListWebPartDataContract _dc = CheckListWebPartDataContract.GetCheckListWebPartDataContract(_edx, _drl);
        m_HiddenFieldData.Value = _dc.Serialize();
      }
    }
    private IWebPartRow m_ProvidersDictionary = null;
    private LiteralControl m_SelectedItemTitle = new LiteralControl();
    private HiddenField m_HiddenFieldData = new HiddenField();
    private SilverlightWebControl m_SilverlightWebControl = null;
    #endregion
  }
}
