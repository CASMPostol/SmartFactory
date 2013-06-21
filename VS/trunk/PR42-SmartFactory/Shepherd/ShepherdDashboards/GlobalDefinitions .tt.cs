using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public partial class GlobalDefinitions
  {
    /// <summary>
    /// Enumeration of Roles
    /// </summary>
    public enum Roles
    {
      /// <summary>
      /// The inbound owner
      /// </summary>
      InboundOwner,
      /// <summary>
      /// The outbound owner
      /// </summary>
      OutboundOwner,
      /// <summary>
      /// The coordinator
      /// </summary>
      Coordinator,
      /// <summary>
      /// The supervisor
      /// </summary>
      Supervisor,
      /// <summary>
      /// The operator
      /// </summary>
      Operator,
      /// <summary>
      /// The vendor
      /// </summary>
      Vendor,
      /// <summary>
      /// The forwarder
      /// </summary>
      Forwarder,
      /// <summary>
      /// The escort
      /// </summary>
      Escort,
      /// <summary>
      /// The guard
      /// </summary>
      Guard,
      /// <summary>
      /// The none
      /// </summary>
      None,
    }
    internal const string NumberOfTimeSLotsFormat = "<b><font size=\"1\" color=\"#0072bc\"> [{0}]</font></b>";
    internal const string FreightPurchaseOrderTemplate = @"Lists/FreightPOLibrary/Forms/FreightPurchaseOrderTemplate.dotx";
    internal const string EscortPOLibraryTemplate = @"Lists/EscortPOLibrary/Forms/SecurityEscortPurchaseOrderTemplate.dotx";
    internal const string SealProtocolLibraryTemplate = @"Lists/SealProtocolLibrary/Forms/SecuritySealProtocolTemplate.dotx";
    internal const string FreightPOLibraryTitle = "Freight PO Library";
    internal const string EscortPOLibraryTitle = "Escort PO Library";
    internal const string SealProtocolLibraryTitle = "Seal Protocol Library";
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

