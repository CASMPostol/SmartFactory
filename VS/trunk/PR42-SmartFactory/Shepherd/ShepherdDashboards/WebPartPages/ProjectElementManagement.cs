using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Entities;
using Microsoft.SharePoint;


namespace CAS.SmartFactory.Shepherd.Dashboards.WebPartPages
{
  using SPLimitedWebPartManager = Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager;

  internal class ProjectElementManagement
  {
    //Menu entries
    internal static string MenuOutboundOwnerTitle = "OutboundOwner".GetLocalizationExpresion();
    internal static string MenuInboundOwnerTitle = "InboundOwner".GetLocalizationExpresion();
    internal static string MenuVendorTitle = "Vendor".GetLocalizationExpresion();
    internal static string MenuForwarderTitle = "Forwarder".GetLocalizationExpresion();
    internal static string MenuSecurityEscortProviderTitle = "Escort".GetLocalizationExpresion();
    internal static string MenuSecurityGateTitle = "Gate".GetLocalizationExpresion();
    internal static string MenuOutboundCoordinatorTitle = "OutboundCoordinator".GetLocalizationExpresion();
    internal static string MenuOperatorTitle = "Operator".GetLocalizationExpresion();
    internal static string MenuSupervisorTitle = "Supervisor".GetLocalizationExpresion();
    internal static string MenuDriversTitle = "Drivers".GetLocalizationExpresion();
    internal static string MenuTrucksTitle = "Trucks".GetLocalizationExpresion();
    internal static string MenuTrailersTitle = "Trailers".GetLocalizationExpresion();
    internal static string MenuViewInboundsTitle = "ViewInbounds".GetLocalizationExpresion();
    internal static string MenuViewOutboundsTitle = "ViewOutbounds".GetLocalizationExpresion();

    //Webpages
    internal const string WebPartPagesFolder = "WebPartPages";
    internal const string URLVendorDashboard = WebPartPagesFolder + "/VendorDashboard.aspx";
    internal const string URLSecurityEscortProviderDashboard = WebPartPagesFolder + "/SecurityEscortProviderDashboard.aspx";
    internal const string URLForwarderDashboard = WebPartPagesFolder + "/ForwarderDashboard.aspx";
    internal const string URLGateDashboard = WebPartPagesFolder + "/GateDashboard.aspx";
    internal const string URLOutboundOwner = WebPartPagesFolder + "/OutboundOwnerDashboard.aspx";
    internal const string URLInboundOwner = WebPartPagesFolder + "/InboundOwnerDashboard.aspx";
    internal const string URLOutboundCoordinator = WebPartPagesFolder + "/OutboundCoordinatorDashboard.aspx";
    internal const string URLOperator = WebPartPagesFolder + "/OperatorDashboard.aspx";
    internal const string URLSupervisor = WebPartPagesFolder + "/SupervisorDashboard.aspx";
    internal const string URLDrivers = WebPartPagesFolder + "/ManageDriversDashboard.aspx";
    internal const string URLTrucks = WebPartPagesFolder + "/ManageCarsDashboard.aspx";
    internal const string URLTrailers = WebPartPagesFolder + "/ManageTrailersDashboard.aspx";
    internal const string URLViewInbounds = WebPartPagesFolder + "/ViewInboundsDashboard.aspx";
    internal const string URLViewOutbounds = WebPartPagesFolder + "/ViewOutboundsDashboard.aspx";

    internal const string IDCurrentUser = "CDCurrentUser";
    internal const string IDTimeSlots = "CDTimeSlots";
    internal const string IDCDShipping = "CDShipping";
    internal const string IDLoadDescription = "CDLoadDescription";
    internal const string IDCarrierDashboardWebPart = "CDCarrierDashboardWebPart";
    internal const string IDTransportResources = "CDTransportResources";
    internal const string IDAlarmsAndEevents = "CDAlarmsAndEevents";
    internal static void SetupConnections(EntitiesDataContext _edc, SPWeb _root)
    {
      SPFile file = _root.GetFile(URLVendorDashboard);
      System.Collections.Generic.Dictionary<string, WebPart> _dict = new System.Collections.Generic.Dictionary<string, WebPart>();
      Anons.WriteEntry(_edc, m_SourceClass + m_SourceSetupConnections, "Setup connections starting");
      string _phase = "starting";
      using (SPLimitedWebPartManager _pageMgr = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
        try
        {
          _dict = _pageMgr.WebParts.Cast<WebPart>().ToDictionary(key => key.ID); //.ForEach(wp => wpMgr.DeleteWebPart(wp));
          _phase = "After wpMgr.WebParts.Cast";
          ConnectWebParts
          (
            _pageMgr,
            _dict[IDCurrentUser],
            _dict[IDCarrierDashboardWebPart],
            CurrentUserWebPart.CurrentUserWebPart.CurrentUserProviderPoint,
            CarrierDashboard.CarrierDashboardWebPart.CarrierDashboardWebPart.ConsumertIDPartnerInterconnection
          );
        }
        catch (Exception ex)
        {
          StringBuilder _names = new StringBuilder();
          _dict.Keys.ToList<string>().ForEach(name => _names.Append(name + ", "));
          string _msg = String.Format("Setup connections failed in Phase={0}, Count={1}, First={2}, Ex={3}", _phase, _dict.Count, _names.ToString(), ex.Message);
          Anons.WriteEntry(_edc, m_SourceClass + m_SourceSetupConnections, _msg);
          //throw new ApplicationException(_msg);
        }
      Anons.WriteEntry(_edc, m_SourceClass + m_SourceSetupConnections, "Setup connections finished");
    }
    internal static void RemovePages(EntitiesDataContext _edc, SPWeb _root)
    {
      Anons.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove Pages starting");
      try
      {
        SPFolder WebPartPagesFolder = _root.GetFolder(ProjectElementManagement.WebPartPagesFolder);
        if (WebPartPagesFolder.Exists)
          WebPartPagesFolder.Delete();
        else
          Anons.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Failed, the folder " + WebPartPagesFolder + "dies not exist.");
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove pages failed with exception: " + ex.Message);
      }
      Anons.WriteEntry(_edc, m_SourceClass + m_SourceRemovePages, "Remove Pages finished");
    }
    private const string m_SourceClass = "WebPartPages.ProjectElementManagement.";
    private const string m_SourceRemovePages = "RemovePages";
    private const string m_SourceSetupConnections = "SetupConnections";
    private static void ConnectWebParts(SPLimitedWebPartManager _pageMgr, WebPart _prvdr, WebPart _cnsmr, string _providerConnectionPoints, string _consumerConnectionPoints)
    {
      _pageMgr.SPConnectWebParts(
        _prvdr,
        _pageMgr.GetProviderConnectionPoints(_prvdr)[_providerConnectionPoints],
        _cnsmr,
        _pageMgr.GetConsumerConnectionPoints(_cnsmr)[_consumerConnectionPoints]);
    }
  }
}

