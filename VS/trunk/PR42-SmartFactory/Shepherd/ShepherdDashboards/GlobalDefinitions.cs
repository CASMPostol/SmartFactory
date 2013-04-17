using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
    internal const string ErrorMessageFormat = "<font color=red>{0}</font><br/>";
    internal const string FreightPurchaseOrderTemplate = @"Lists/FreightPOLibrary/Forms/FreightPurchaseOrderTemplate.dotx";
    internal const string EscortPOLibraryTemplate = @"Lists/EscortPOLibrary/Forms/SecurityEscortPurchaseOrderTemplate.dotx";
    internal const string SealProtocolLibraryTemplate = @"Lists/SealProtocolLibrary/Forms/SecuritySealProtocolTemplate.dotx";
    internal const string FreightPOLibraryTitle = "Freight PO Library";
    internal const string EscortPOLibraryTitle = "Escort PO Library";
    internal const string SealProtocolLibraryTitle = "Seal Protocol Library";
    internal const string MasterPage = "cas.master";
    internal const string RootResourceFileName = "CASSmartFactoryShepherd";
    public delegate void UpdateToolStripEvent(object obj, ProgressChangedEventArgs progres);
  }
}

