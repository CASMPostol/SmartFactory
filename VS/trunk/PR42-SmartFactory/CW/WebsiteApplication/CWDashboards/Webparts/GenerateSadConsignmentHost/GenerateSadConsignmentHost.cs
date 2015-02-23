using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using CAS.SmartFactory.CW.Dashboards.SharePointLib;
using CAS.SmartFactory.CW.Dashboards.Silverlight;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.GenerateSadConsignmentHost
{
  /// <summary>
  /// Class GenerateSadConsignmentHost.
  /// </summary>
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
      using (Entities _entities = new Entities())
      {
        DisposalRequestLib _drl = Element.GetAtIndex<DisposalRequestLib>(_entities.DisposalRequestLibrary, e.ID);
        List<SAD> _cns = new List<SAD>();
        SPWeb _wb = SPContext.Current.Web;
        foreach (CustomsWarehouseDisposal _cwd in _drl.CustomsWarehouseDisposal(_entities, false))
        {
          if (_cwd.CWL_CWDisposal2ClearanceID == null)
            continue;
          if (_cwd.CWL_CWDisposal2ClearanceID.SADConsignmentLibraryIndex == null)
            continue;
          SPDocumentLibrary _lib = (SPDocumentLibrary)_wb.Lists[SADConsignment.IPRSADConsignmentLibraryTitle];
          SAD _sad = CAS.SharePoint.DocumentsFactory.File.ReadXmlFile<SAD>(_lib, _cwd.CWL_CWDisposal2ClearanceID.SADConsignmentLibraryIndex.Id.Value);
          _cns.Add(_sad);
        }
        SADCollection _sc = new SADCollection() { ListOfSAD = _cns.ToArray() };
        m_HiddenFieldData.Value = System.Web.HttpUtility.HtmlEncode(CAS.SharePoint.Serialization.XmlSerializer.Serialize<SADCollection>(_sc, Settings.SADCollectionStylesheetName));
      }
    }
    private IWebPartRow m_ProvidersDictionary = null;
    private LiteralControl m_SelectedItemTitle = new LiteralControl();
    private HiddenField m_HiddenFieldData = new HiddenField();
    private SilverlightWebControl m_SilverlightWebControl = null;
    #endregion
  }
}
