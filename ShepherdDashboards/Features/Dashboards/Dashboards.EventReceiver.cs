using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using System.Collections.Generic;

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
    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      SPSite site = properties.Feature.Parent as SPSite;
      SPWeb _root = site.RootWeb;
      if (site == null)
        throw new ApplicationException("FeatureActivated cannot get access to the Web");
      using (Entities.EntitiesDataContext _edc = new Entities.EntitiesDataContext(_root.Url))
      {
        Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated strating");
        _root.Title = "Shepherd Home";
        _root.SiteLogoUrl = @"_layouts/images/ShepherdDashboards/Shepherd_50x50.png";
        _root.Update();
        // create dropdown menu for custom site pages
        Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "Navigation setup starting"); 
        SPNavigationNodeCollection _topNav = _root.Navigation.TopNavigationBar;
        SPNavigationNode _topMenu = _topNav[0].Children.AddAsLast(new SPNavigationNode(m_VendorMenyTitle, WebPartPages.ProjectElementManagement.CarrierDashboardURL));
        WebPartPages.ProjectElementManagement.SetupConnections(_edc, _root);
        Entities.Anons.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated finished");
      }
    }

    // Uncomment the method below to handle the event raised before a feature is deactivated.
    public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    {
      SPSite site = properties.Feature.Parent as SPSite;
      SPWeb _root = site.RootWeb;
      if (site == null)
        throw new ApplicationException("FeatureDeactivating cannot get access to the Web");
      using (Entities.EntitiesDataContext _edc = new Entities.EntitiesDataContext(_root.Url))
      {
        Entities.Anons.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation starting.");
        WebPartPages.ProjectElementManagement.RemovePages(_edc, _root);
        RemoveNavigationEntries(_root);
        DeleteWebParts(_edc, _root);
        Entities.Anons.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation finished.");
      }
    }
    private static void RemoveNavigationEntries(SPWeb _root)
    {
      try
      {
        SPNavigationNodeCollection topNav = _root.Navigation.TopNavigationBar;
        for (int i = topNav[0].Children.Count - 1; i >= 0; i--)
          if (topNav[0].Children[i].Title == m_VendorMenyTitle)
            topNav[0].Children[i].Delete();
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Cannot remove navigation entries: " + ex.Message);
      }
    }
    private const string m_VendorMenyTitle = "Vendor";
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
