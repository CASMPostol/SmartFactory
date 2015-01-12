//<summary>
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.Link2SQL;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL
{
  public partial class AlarmsAndEvents : IItem { }
  public partial class ArchivingLogs : IArchivingLogs, IId { }
  public partial class ArchivingOperationLogs : IId { }
  public partial class BusinessDescription : IItem { }
  public partial class Carrier : IItem { }
  public partial class CarrierPerformanceReport : IItem { }
  public partial class City : IItem { }
  public partial class Commodity : IItem { }
  public partial class Country : IItem { }
  public partial class Currency : IItem { }
  public partial class DestinationMarket : IItem { }
  public partial class Driver : IItem { }
  public partial class DriversTeam : IItem { }
  public partial class EscortPOLibrary : IItem { }
  public partial class FreightPayer : IItem { }
  public partial class FreightPOLibrary : IItem { }
  public partial class LoadDescription : IItem { }
  public partial class Market : IItem { }
  public partial class Partner : IItem { }
  public partial class Route : IItem { }
  public partial class SAPDestinationPlant : IItem { }
  public partial class ScheduleTemplate : IItem { }
  public partial class SealProtocolLibrary : IItem { }
  public partial class SecurityEscortRoute : IItem { }
  public partial class ShipmentType : IItem { }
  public partial class Shipping : IItem { }
  public partial class ShippingPoint : IItem { }
  public partial class TimeSlot : IItem { }
  public partial class TimeSlotsTemplate : IItem { }
  public partial class Trailer : IItem { }
  public partial class TransportUnitType : IItem { }
  public partial class Truck : IItem { }
  public partial class Warehouse : IItem { }
  public partial class History : IHistory, IId { }

}
