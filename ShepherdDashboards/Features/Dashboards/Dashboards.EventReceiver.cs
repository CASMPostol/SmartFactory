using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using System.Collections.Generic;
using CAS.SmartFactory.Shepherd.Dashboards.WebPartPages;

namespace CAS.SmartFactory.Shepherd.Dashboards.Features.Dashboards
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>

  [Guid("79ed4ae0-8478-445d-996c-d84421cfe0d5")]
  public class DashboardsEventReceiver : SPFeatureReceiver
  {

    #region public
    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      string _cp = "Starting";
      try
      {
        SPSite site = (SPSite)properties.Feature.Parent;
        if (site == null)
          throw new ApplicationException("In FeatureActivated the Site is null");
        SPWeb _root = site.RootWeb;
        using (Entities.EntitiesDataContext _edc = new Entities.EntitiesDataContext(_root.Url))
        {
          Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated strating");
          _cp = "ReplaceMasterMage";
          ReplaceMasterMage(site);
          Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "The master page has been replaced.");
          _cp = "AddDocumentTemplates";
          AddDocumentTemplates(_root, GlobalDefinitions.FreightPurchaseOrderTemplate, GlobalDefinitions.FreightPOLibraryTitle);
          AddDocumentTemplates(_root, GlobalDefinitions.EscortPOLibraryTemplate, GlobalDefinitions.EscortPOLibraryTitle);
          AddDocumentTemplates(_root, GlobalDefinitions.SealProtocolLibraryTemplate, GlobalDefinitions.SealProtocolLibraryTitle);
          // create dropdown menu for custom site pages
          Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "Navigation setup starting");
          _cp = "SPNavigationNodeCollection";
          SPNavigationNodeCollection _topNav = _root.Navigation.TopNavigationBar;
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuVendorTitle, ProjectElementManagement.URLVendorDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuInboundOwnerTitle, ProjectElementManagement.URLInboundOwner));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuViewInboundsTitle, ProjectElementManagement.URLViewInbounds));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuViewOutboundsTitle, ProjectElementManagement.URLViewOutbounds));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuOutboundOwnerTitle, ProjectElementManagement.URLOutboundOwner));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuOutboundCoordinatorTitle, ProjectElementManagement.URLOutboundCoordinator));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuForwarderTitle, ProjectElementManagement.URLForwarderDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuSecurityEscortProviderTitle, ProjectElementManagement.URLSecurityEscortProviderDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuSecurityGateTitle, ProjectElementManagement.URLGateDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuOperatorTitle, ProjectElementManagement.URLOperator));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuSupervisorTitle, ProjectElementManagement.URLSupervisor));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuDriversTitle, ProjectElementManagement.URLDrivers));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCarsTitle, ProjectElementManagement.URLCars));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuTrailersTitle, ProjectElementManagement.URLTrailers));
          foreach (SPNavigationNode item in _topNav)
            item.Update();
          //WebPartPages.ProjectElementManagement.SetupConnections(_edc, _root);
          _cp = "Entities.Anons";
          Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated finished");
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
      SPWeb _root = _site.RootWeb;
      if (_site == null)
        throw new ApplicationException("FeatureDeactivating cannot get access to the Web");
      using (Entities.EntitiesDataContext _edc = new Entities.EntitiesDataContext(_root.Url))
      {
        Entities.Anons.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation starting.");
        WebPartPages.ProjectElementManagement.RemovePages(_edc, _root);
        RemoveNavigationEntries(_root);
        DeleteWebParts(_edc, _root);
        Entities.Anons.WriteEntry(_edc, "FeatureDeactivating", "Reverting to default master page.");
        RevertMasterPage(_site);
        Entities.Anons.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation finished.");
      }
    }
    #endregion

    #region private
    private void ReplaceMasterMage(SPSite _siteCollection)
    {
      try
      {
        SPWeb _rootWeb = _siteCollection.RootWeb;
        //_root.Update();
        // calculate relative path to site from Web Application root
        string _webAppRelativePath = _rootWeb.ServerRelativeUrl;
        if (!_webAppRelativePath.EndsWith("/"))
          _webAppRelativePath += "/";
        // enumerate through each site and apply branding
        foreach (SPWeb _webx in _siteCollection.AllWebs)
        {
          _webx.MasterUrl = _webAppRelativePath + "_catalogs/masterpage/" + GlobalDefinitions.MasterPage;
          _webx.CustomMasterUrl = _webAppRelativePath + "_catalogs/masterpage/" + GlobalDefinitions.MasterPage;
          //site.AlternateCssUrl = WebAppRelativePath + "Style%20Library/Branding101/Styles.css";
          //site.SiteLogoUrl = WebAppRelativePath + "Style%20Library/Branding101/Images/Logo.gif";
          _webx.Title = "Shepherd Home";
          _webx.SiteLogoUrl = @"_layouts/images/ShepherdDashboards/Shepherd_50x50.png";
          _webx.UIVersion = 4;
          _webx.Update();
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
        SPWeb topLevelSite = siteCollection.RootWeb;
        // calculate relative path of site from Web Application root
        string WebAppRelativePath = topLevelSite.ServerRelativeUrl;
        if (!WebAppRelativePath.EndsWith("/"))
        {
          WebAppRelativePath += "/";
        }
        // enumerate through each site and remove custom branding
        foreach (SPWeb site in siteCollection.AllWebs)
        {
          site.MasterUrl = WebAppRelativePath + "_catalogs/masterpage/v4.master";
          site.CustomMasterUrl = WebAppRelativePath + "_catalogs/masterpage/v4.master";
          //site.AlternateCssUrl = "";
          //site.SiteLogoUrl = "";
          site.Update();
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format("RevertMasterPage exception: {0}", ex.Message));
      }
    }
    private static void AddDocumentTemplates(SPWeb _root, string templateUrl, string _strListName)
    {
      SPDocumentLibrary _list = (SPDocumentLibrary)_root.Lists[_strListName];
      _list.DocumentTemplateUrl = templateUrl;
      _list.Update();
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
    private static void DeleteWebParts(Entities.EntitiesDataContext _edc, SPWeb _root)
    {
      Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts strating");
      try
      {
        SPList _wpl = _root.GetCatalog(SPListTemplateType.WebPartCatalog);
        List<SPFile> _filesToDelete = new List<SPFile>();
        // figure out which Web Part template files need to be deleted
        Entities.Anons.WriteEntry
        (
          _edc, m_SourceClass + m_SourceDeleteWebParts,
          String.Format("Processing of the WebPartCatalog containing {0} items starting ", _wpl.ItemCount)
        );
        foreach (SPListItem _li in _wpl.Items)
        {
          bool _delete = _li.File.Name.StartsWith("ShepherdDashboards");
          if (_delete)
            _filesToDelete.Add(_li.File);
          string _mess = String.Format("Title: {0}, Name: {1}, File name: {2}, deleted: {3}", _li.Title, _li.Name, _li.File.Name, _delete);
          Entities.Anons.WriteEntry(_edc, "Processing Web Part", _mess);
        }
        // delete Web Part template files
        foreach (SPFile file in _filesToDelete)
          file.Delete();
      }
      catch (Exception ex)
      {
        Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts finished with exception: " + ex.Message);
      }
      Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceDeleteWebParts, "Delete Web Parts finished");
    }
    private const string m_SourceClass = "DashboardsEventReceiver.";
    private const string m_SourceDeleteWebParts = "DeleteWebParts";
    private const string m_SourceFeatureActivated = "FeatureActivated";
    #endregion

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
