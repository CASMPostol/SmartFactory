using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Navigation;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using SPWebPartConnection = Microsoft.SharePoint.WebPartPages.SPWebPartConnection;
using System.Collections.Generic;
using System.Text;

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
        _root.SiteLogoUrl = @"_layouts/images/ShepherdDashboards/Shepherd_50x50.png";
        _root.Update();
      }
      // create dropdown menu for custom site pages
      SPNavigationNodeCollection _topNav = _root.Navigation.TopNavigationBar;
      SPNavigationNode _topMenu = _topNav[0].Children.AddAsLast(new SPNavigationNode("Vendor", "WebPartPages/CarrierDashboard.aspx"));

      //SetupConnections(_root);
    }

    private static void SetupConnections(SPWeb _root)
    {
      SPFile file = _root.GetFile("WebPartPages/CarrierDashboard.aspx");
      System.Collections.Generic.Dictionary<string, WebPart> _dict = new System.Collections.Generic.Dictionary<string, WebPart>();
      string _phase = "starting";
      using (Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager wpMgr = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
      {
        try
        {
          _dict = wpMgr.WebParts.Cast<WebPart>().ToDictionary(key => key.ID); //.ForEach(wp => wpMgr.DeleteWebPart(wp));
          _phase = "After wpMgr.WebParts.Cast";
          SPWebPartConnection _CarrierDashboardWebPartPartner = new SPWebPartConnection
          {
            ConsumerID = "CarrierDashboardWebPart",
            ProviderID = "CurrentUserWebPart",
            ConsumerConnectionPointID = "PartnerInterconnection",
            ProviderConnectionPointID = "CurrentUserProviderPoint",
            ID = "CarrierDashboardWebPartPartner"
          };
          _phase = "After new SPWebPartConnection";
          wpMgr.SPWebPartConnections.Add(_CarrierDashboardWebPartPartner);
          _phase = "After Add(_CarrierDashboardWebPartPartner)";
          wpMgr.SaveChanges(_dict["CarrierDashboardWebPart"]);
          _phase = "after SaveChanges(_dict['CarrierDashboardWebPart'])";
          wpMgr.SaveChanges(_dict["CurrentUserWebPart"]);
          _phase = "Finished";
        }
        catch (Exception ex)
        {
          StringBuilder _names = new StringBuilder();
          _dict.Keys.ToList<string>().ForEach(name => _names.Append(name + ", "));
          throw new ApplicationException(String.Format("Phase={0}, Count={1}, First={2}, Ex={3}", _phase, _dict.Count, _names.ToString(), ex.Message));
        }
        //SPWebPartConnection connection = new SPWebPartConnection
        //{
        //  ConsumerID = "CarrierDashboardWebPart",
        //  ProviderID = "CurrentUserWebPart",
        //  ConsumerConnectionPointID = "PartnerInterconnection",
        //  ProviderConnectionPointID = "CurrentUserProviderPoint",
        //  ID = "CarrierDashboardWebPartPartner"
        //};

        ////RssWebPart rssWebPart = new RssWebPart
        //{
        //  ID = "RssWebPart",
        //  Title = "RSS Reader",
        //  ItemCount = 5
        //};
        //FeedInputWebPart feedInput = new FeedInputWebPart
        //{
        //  ID = "FeedInput",
        //  Title = "Feed input"
        //};
        //wpMgr.AddWebPart(feedInput, "Main", 0);
        //wpMgr.AddWebPart(rssWebPart, "Main", 1);
        //SPWebPartConnection connection = new SPWebPartConnection
        //{
        //  ConsumerID = "RssWebPart",
        //  ProviderID = "FeedInput",
        //  ConsumerConnectionPointID = "feed",
        //  ProviderConnectionPointID = "",
        //  ID = "connection"
        //};
        //wpMgr.SPWebPartConnections.Add(connection);
        //wpMgr.SaveChanges(rssWebPart);
        //wpMgr.SaveChanges(feedInput);
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
