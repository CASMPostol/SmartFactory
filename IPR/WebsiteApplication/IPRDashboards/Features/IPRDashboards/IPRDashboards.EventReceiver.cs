using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.Dashboards.WebPartPages;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

namespace CAS.SmartFactory.IPR.Dashboards.Features.IPRDashboards
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>

  [Guid("12ecc775-53b3-433a-a67e-47caaf7f8e82")]
  public class IPRDashboardsEventReceiver : SPFeatureReceiver
  {
    #region public
    /// <summary>
    /// Occurs after a Feature is activated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
    /// <exception cref="System.ApplicationException">
    /// In FeatureActivated the Site is null
    /// or
    /// </exception>
    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      string _cp = "Starting";
      try
      {
        SPSite site = (SPSite)properties.Feature.Parent;
        if (site == null)
          throw new ApplicationException("In FeatureActivated the Site is null");
        using (Entities _edc = new Entities(site.RootWeb.Url))
        {
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated strating");
          _cp = "ReplaceMasterMage";
          ReplaceMasterPage(site);
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "Navigation setup starting");
          _cp = "SPNavigationNodeCollection";
          SPNavigationNodeCollection _topNav = site.RootWeb.Navigation.TopNavigationBar;
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuIPRBookTitle, ProjectElementManagement.URLIPRBookDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuIPRClosedBookTitle, ProjectElementManagement.URLIPRBookClosedDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuIPRBookCustomsOfficeTitle, ProjectElementManagement.URLIPRBookCustomsOfficeDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuIPRClosedBookCustomsOfficeTitle, ProjectElementManagement.URLIPRBookClosedCustomsOfficeDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuClearanceTitle, ProjectElementManagement.URLClearenceDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuClearanceViewTitle, ProjectElementManagement.URLClearenceViewDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuExportTitle, ProjectElementManagement.URLExportDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuBatchTitle, ProjectElementManagement.URLBatchDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuStocksTitle, ProjectElementManagement.URLStocksDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuReportsTitle, ProjectElementManagement.URLReportsDashboard));
          foreach (SPNavigationNode item in _topNav)
            item.Update();
          //WebPartPages.ProjectElementManagement.SetupConnections(_edc, _root);
          _cp = "Entities.Anons";
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated finished");
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format("FeatureActivated exception at {0}: {1}", _cp, ex.Message));
      }
    }
    /// <summary>
    /// Occurs when a Feature is deactivated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"/> object that represents the properties of the event.</param>
    public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    {
      SPSite _site = properties.Feature.Parent as SPSite;
      if (_site == null)
        throw new ApplicationException("FeatureDeactivating cannot get access to the Web");
      using (Entities _edc = new Entities(_site.RootWeb.Url))
      {
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation starting.");
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Removing pages.");
        WebPartPages.ProjectElementManagement.RemovePages(_edc, _site.RootWeb);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Removing Navigation Entries.");
        RemoveNavigationEntries(_site.RootWeb);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Starting deletion of web parts.");
        DeleteWebParts(_edc, _site.RootWeb);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Reverting to default master page.");
        RevertMasterPage(_site);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation finished.");
      }
    }
    #endregion

    #region private
    private void ReplaceMasterPage(SPSite _siteCollection)
    {
      try
      {
        //_root.Update();
        // calculate relative path to site from Web Application root
        string _webAppRelativePath = _siteCollection.RootWeb.ServerRelativeUrl;
        if (!_webAppRelativePath.EndsWith("/"))
          _webAppRelativePath += "/";
        // enumerate through each site and apply branding
        foreach (SPWeb _webx in _siteCollection.AllWebs)
        {
          //This best practice addresses the issue identified by the SharePoint Dispose Checker Tool as SPDisposeCheckID_130.
          try
          {
            _webx.MasterUrl = _webAppRelativePath + "_catalogs/masterpage/" + GlobalDefinitions.MasterPage;
            _webx.CustomMasterUrl = _webAppRelativePath + "_catalogs/masterpage/" + GlobalDefinitions.MasterPage;
            _webx.Title = "IPR Home";
            _webx.SiteLogoUrl = @"_layouts/images/IPRDashboards/ipr_logo_60x60.png";
            _webx.UIVersion = 4;
            _webx.Update();
          }
          finally
          {
            if (_webx != null)
              _webx.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format("ReplaceMasterMage exception: {0}", ex.Message));
      }
    }
    private void RevertMasterPage(SPSite siteCollection)
    {
      try
      {
        // calculate relative path of site from Web Application root
        string WebAppRelativePath = siteCollection.RootWeb.ServerRelativeUrl;
        if (!WebAppRelativePath.EndsWith("/"))
        {
          WebAppRelativePath += "/";
        }
        // enumerate through each site and remove custom branding
        foreach (SPWeb site in siteCollection.AllWebs)
        {
          //This best practice addresses the issue identified by the SharePoint Dispose Checker Tool as SPDisposeCheckID_130.
          try
          {
            site.MasterUrl = WebAppRelativePath + "_catalogs/masterpage/v4.master";
            site.CustomMasterUrl = WebAppRelativePath + "_catalogs/masterpage/v4.master";
            //site.AlternateCssUrl = "";
            //site.SiteLogoUrl = "";
            site.Update();
          }
          finally
          {
            if (site != null)
              site.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format("RevertMasterPage exception: {0}", ex.Message));
      }
    }
    private static void RemoveNavigationEntries(SPWeb _root)
    {
      try
      {
        SPNavigationNodeCollection topNav = _root.Navigation.TopNavigationBar;
        for (int i = topNav.Count - 1; i >= 0; i--)
          topNav[i].Delete();
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Cannot remove navigation entries: " + ex.Message);
      }
    }
    private static void DeleteWebParts(Entities edc, SPWeb root)
    {
      ActivityLogCT.WriteEntry(edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts starting");
      try
      {
        SPList _wpl = root.GetCatalog(SPListTemplateType.WebPartCatalog);
        List<SPFile> _filesToDelete = new List<SPFile>();
        // figure out which Web Part template files need to be deleted
        ActivityLogCT.WriteEntry
        (
          edc, m_SourceClass + m_SourceDeleteWebParts,
          String.Format("Processing of the WebPartCatalog containing {0} items starting ", _wpl.ItemCount)
        );
        foreach (SPListItem _li in _wpl.Items)
        {
          bool _delete = _li.File.Name.StartsWith("IPRDashboards");
          if (_delete)
            _filesToDelete.Add(_li.File);
          string _mess = String.Format("Title: {0}, Name: {1}, File name: {2}, deleted: {3}", _li.Title, _li.Name, _li.File.Name, _delete);
          ActivityLogCT.WriteEntry(edc, "Processing Web Part", _mess);
        }
        // delete Web Part template files
        foreach (SPFile file in _filesToDelete)
          file.Delete();
      }
      catch (Exception ex)
      {
        ActivityLogCT.WriteEntry(edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts finished with exception: " + ex.Message);
      }
      ActivityLogCT.WriteEntry(edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts finished");
    }
    private const string m_SourceClass = "DashboardsEventReceiver.";
    private const string m_SourceDeleteWebParts = "DeleteWebParts";
    private const string m_SourceFeatureActivated = "FeatureActivated";
    #endregion

  }
}
