using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Dashboards.WebPartPagesCW
{
  using SPLimitedWebPartManager = Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager;

   internal class ProjectElementManagement
  {
      // TopNavigation Elements Title
     internal static string MenuCWBookTitle = "CWBookTitle".GetCWLocalizationExpression();
     internal static string MenuCWBookCustomsOfficeTitle = "CWBookCustomsOfficeTitle".GetCWLocalizationExpression();
     internal static string MenuCWBookClosedTitle = "CWBookClosedTitle".GetCWLocalizationExpression();
     internal static string MenuCWBookClosedCustomsOfficeTitle = "CWBookClosedCustomsOfficeTitle".GetCWLocalizationExpression();
     internal static string MenuSupplementCWBookTitle = "SupplementCWBookTitle".GetCWLocalizationExpression();
     internal static string MenuSupplementWZNoTitle = "SupplementWZNoTitle".GetCWLocalizationExpression();
     internal static string MenuCheckListExitSheetTitle = "CheckListExitSheetTitle".GetCWLocalizationExpression();
     internal static string MenuDisposalRequestTitle = "DisposalRequestTitle".GetCWLocalizationExpression();
     internal static string MenuDisposalsViewTitle = "DisposalsViewTitle".GetCWLocalizationExpression();
     internal static string MenuGenerateSadConsignmentTitle = "GenerateSadConsignmentTitle".GetCWLocalizationExpression();

     // DashboardsURL
     internal const string WebPartPagesFolder = "WebPartPagesCW";
     internal const string URLCWBookDashboard = WebPartPagesFolder + "/CWBookDashboard.aspx";
     internal const string URLCWBookCustomsOfficeDashboard = WebPartPagesFolder + "/CWBookCustomsOfficeDashboard.aspx";
     internal const string URLCWBookClosedDashboard = WebPartPagesFolder + "/CWBookClosedDashboard.aspx";
     internal const string URLCWBookClosedCustomsOfficeDashboard = WebPartPagesFolder + "/CWBookClosedCustomsOfficeDashboard.aspx";
     internal const string URLSupplementCWBookDashboard = WebPartPagesFolder + "/SupplementCWBookDashboard.aspx";
     internal const string URLSupplementWZNoDashboard = WebPartPagesFolder + "/SupplementWZNoDashboard.aspx";
     internal const string URLCheckListExitSheetDashboard = WebPartPagesFolder + "/CheckListExitSheetDashboard.aspx";
     internal const string URLDisposalRequestDashboard = WebPartPagesFolder + "/DisposalRequestDashboard.aspx";
     internal const string URLDisposalsViewDashboard = WebPartPagesFolder + "/DisposalsViewDashboard.aspx";
     internal const string URLGenerateSadConsignmentDashboard = WebPartPagesFolder + "/GenerateSadConsignmentDashboard.aspx";

     internal static void RemovePages(Entities _edc, SPWeb _root)
     {
       ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove Pages starting");
       try
       {
         SPFolder WebPartPagesFolder = _root.GetFolder(ProjectElementManagement.WebPartPagesFolder);
         if (WebPartPagesFolder.Exists)
           WebPartPagesFolder.Delete();
         else
           ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Failed, the folder " + WebPartPagesFolder + "does not exist.");
       }
       catch (Exception ex)
       {
         ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove pages failed with exception: " + ex.Message);
       }
       ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove Pages finished");
     }
     private const string m_SourceClass = "WebPartPages.ProjectElementManagement.";
     private const string m_SourceRemovePages = "RemovePages";
  }
}
