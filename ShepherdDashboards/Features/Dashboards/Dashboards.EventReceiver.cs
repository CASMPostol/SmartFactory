using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

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
    // Uncomment the method below to handle the event raised after a feature has been activated.

    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      SPSite site = properties.Feature.Parent as SPSite;
      SPWeb _root = site.RootWeb;
      if (site != null)
      {
        _root.Title = "Shepherd Home";
        _root.SiteLogoUrl = @"_layouts/images/ShepherdDashboards/SiteLogo49x48.gif";
        _root.Update();
      }
    }


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
