using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.Dashboards.WebPartPages;
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
    // Uncomment the method below to handle the event raised after a feature has been activated.

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
    private const string m_SourceClass = "DashboardsEventReceiver.";
    private const string m_SourceFeatureActivated = "FeatureActivated";

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
            if ( _webx != null )
              _webx.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format("ReplaceMasterMage exception: {0}", ex.Message));
      }
    }

    #endregion

    // Uncomment the method below to handle the event raised before a feature is deactivated.

    //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    //{
    //}


    // Uncomment the method below to handle the event raised after a feature has been installed.

    //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
    //{
    //}


    // Uncomment the method below to handle the event raised before a feature is uninstalled.

    //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
    //{
    //}

    // Uncomment the method below to handle the event raised when a feature is upgrading.

    //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
    //{
    //}
  }
}
