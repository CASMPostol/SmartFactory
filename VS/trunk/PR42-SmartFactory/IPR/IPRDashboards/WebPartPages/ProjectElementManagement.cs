using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Dashboards.WebPartPages
{
  using SPLimitedWebPartManager = Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager;

  internal class ProjectElementManagement
  {
    // TopNavigation Elements Title
    internal static string MenuIPRBookTitle = "IprBookTitle".GetIPRLocalizationExpresion();
    internal static string MenuIPRClosedBookTitle = "IprClosedBookTitle".GetIPRLocalizationExpresion();
    internal static string MenuIPRBookCustomsOfficeTitle = "IPRBookCustomsOfficeTitle".GetIPRLocalizationExpresion();
    internal static string MenuIPRClosedBookCustomsOfficeTitle = "IPRClosedBookCustomsOfficeTitle".GetIPRLocalizationExpresion();
    internal static string MenuClearanceTitle = "ClearanceTitle".GetIPRLocalizationExpresion();
    internal static string MenuClearanceViewTitle = "ClearanceViewTitle".GetIPRLocalizationExpresion();
    internal static string MenuExportTitle = "ExportTitle".GetIPRLocalizationExpresion();
    internal static string MenuBatchTitle = "BatchTitle".GetIPRLocalizationExpresion();
    internal static string MenuStocksTitle = "StocksTitle".GetIPRLocalizationExpresion();
    internal static string MenuReportsTitle = "ReportsTitle".GetIPRLocalizationExpresion();


    // DashboardsURL
    internal const string WebPartPagesFolder = "WebPartPages";
    internal const string URLIPRBookDashboard = WebPartPagesFolder + "/IPRBookDashboard.aspx";
    internal const string URLIPRBookClosedDashboard = WebPartPagesFolder + "/IPRBookClosedDashboard.aspx";
    internal const string URLIPRBookCustomsOfficeDashboard = WebPartPagesFolder + "/IPRBookCustomsOfficeDashboard.aspx";
    internal const string URLIPRBookClosedCustomsOfficeDashboard = WebPartPagesFolder + "/IPRBookClosedCustomsOfficeDashboard.aspx";
    internal const string URLClearenceDashboard = WebPartPagesFolder + "/ClearenceDashboard.aspx";
    internal const string URLClearenceViewDashboard = WebPartPagesFolder + "/ClearenceViewDashboard.aspx";
    internal const string URLExportDashboard = WebPartPagesFolder + "/ExportDashboard.aspx";
    internal const string URLBatchDashboard = WebPartPagesFolder + "/BatchDashboard.aspx";
    internal const string URLStocksDashboard = WebPartPagesFolder + "/StocksDashboard.aspx";
    internal const string URLReportsDashboard = WebPartPagesFolder + "/ReportsDashboard.aspx";

    internal static void RemovePages(Entities _edc, SPWeb _root)
    {
      ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove Pages starting");
      try
      {
        SPFolder WebPartPagesFolder = _root.GetFolder(ProjectElementManagement.WebPartPagesFolder);
        if (WebPartPagesFolder.Exists)
          WebPartPagesFolder.Delete();
        else
          ActivityLogCT.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Failed, the folder " + WebPartPagesFolder + "dies not exist.");
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
