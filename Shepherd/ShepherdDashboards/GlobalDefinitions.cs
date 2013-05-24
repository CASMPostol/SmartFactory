using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public class GlobalDefinitions
  {
    public enum Roles
    {
      InboundOwner,
      OutboundOwner,
      Coordinator,
      Supervisor,
      Operator,
      Vendor,
      Forwarder,
      Escort,
      Guard,
      None,
    }
    internal const string NumberOfTimeSLotsFormat = "<b><font size=\"1\" color=\"#0072bc\"> [{0}]</font></b>";
    internal const string FreightPurchaseOrderTemplate = @"Lists/FreightPOLibrary/Forms/FreightPurchaseOrderTemplate.dotx";
    internal const string EscortPOLibraryTemplate = @"Lists/EscortPOLibrary/Forms/SecurityEscortPurchaseOrderTemplate.dotx";
    internal const string SealProtocolLibraryTemplate = @"Lists/SealProtocolLibrary/Forms/SecuritySealProtocolTemplate.dotx";
    internal const string FreightPOLibraryTitle = "Freight PO Library";
    internal const string EscortPOLibraryTitle = "Escort PO Library";
    internal const string SealProtocolLibraryTitle = "Seal Protocol Library";
    internal const string MasterPage = "cas.master";
    internal const string RootResourceFileName = "CASSmartFactoryShepherdCode";
    public delegate void UpdateToolStripEvent( object obj, ProgressChangedEventArgs progres );
    internal const string CarrierDashboardWebPart = "CarrierDashboardWebPart";
    internal const string DriversManager = "DriversManager";
    internal const string TrailerManager = "TrailerManager";
    internal const string TransportResources = "TransportResources";
    internal const string TruckManager = "TruckManager";
    internal const string CurrentUserWebPart = "CurrentUserWebPart";
    internal const string GuardWebPart = "GuardWebPart";
    internal const string LoadDescriptionWebPart = "LoadDescriptionWebPart";
    internal const string TimeSlotWebPart = "TimeSlotWebPart";
    internal static string ErrorMessage( string error )
    {
      return String.Format( GlobalDefinitions.ErrorMessageFormat, error ) ;
    }
    internal static LiteralControl ErrorLiteralControl( string error )
    {
      return new LiteralControl( String.Format( GlobalDefinitions.ErrorMessageFormat, error ) );
    }
    private const string ErrorMessageFormat = "<font color=red>{0}</font><br/>";

  }
}

