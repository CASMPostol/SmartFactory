using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using CAS.SmartFactory.CW.Dashboards.WebPartPagesCW;

namespace CAS.SmartFactory.CW.Dashboards.Features.CWDashboards
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>

  [Guid("c7c620cf-4249-4bdb-95a1-6ae16c0c8f3b")]
  public class CWDashboardsEventReceiver : SPFeatureReceiver
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
        using (Entities _edc = new Entities(site.RootWeb.Url))
        {
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "FeatureActivated strating");
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceFeatureActivated, "Navigation setup starting");
          _cp = "SPNavigationNodeCollection";
          SPNavigationNodeCollection _topNav = site.RootWeb.Navigation.TopNavigationBar;
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCWBookTitle, ProjectElementManagement.URLCWBookDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCWBookClosedTitle, ProjectElementManagement.URLCWBookClosedDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCWBookCustomsOfficeTitle, ProjectElementManagement.URLCWBookCustomsOfficeDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCWBookClosedCustomsOfficeTitle, ProjectElementManagement.URLCWBookClosedCustomsOfficeDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuCheckListExitSheetTitle, ProjectElementManagement.URLCheckListExitSheetDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuSupplementCWBookTitle, ProjectElementManagement.URLSupplementCWBookDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuSupplementWZNoTitle, ProjectElementManagement.URLSupplementWZNoDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuDisposalRequestTitle, ProjectElementManagement.URLDisposalRequestDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuDisposalsViewTitle, ProjectElementManagement.URLDisposalsViewDashboard));
          _topNav.AddAsLast(new SPNavigationNode(ProjectElementManagement.MenuGenerateSadConsignmentTitle, ProjectElementManagement.URLGenerateSadConsignment));
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

    public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    {
      SPSite _site = properties.Feature.Parent as SPSite;
      if (_site == null)
        throw new ApplicationException("FeatureDeactivating cannot get access to the Web");
      using (Entities _edc = new Entities(_site.RootWeb.Url))
      {
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation starting.");
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Removing pages.");
        WebPartPagesCW.ProjectElementManagement.RemovePages(_edc, _site.RootWeb);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Removing Navigation Entries.");
        RemoveNavigationEntries(_site.RootWeb);
        ActivityLogCT.WriteEntry(_edc, "FeatureDeactivating", "Feature Deactivation finished.");
      }
    }

    #endregion

    #region private
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
    private const string m_SourceClass = "DashboardsEventReceiver.";
    private const string m_SourceFeatureActivated = "FeatureActivated";

    #endregion
  }
}
