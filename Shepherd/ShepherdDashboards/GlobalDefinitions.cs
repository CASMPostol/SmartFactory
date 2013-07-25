using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public partial class GlobalDefinitions
  {
    internal const string MasterPage = "cas.master";
    internal const string ShepherdResourceFileName = "CASSmartFactoryShepherdCode";
    internal delegate void UpdateToolStripEvent( object obj, ProgressChangedEventArgs progres );
    internal const string CarrierDashboardWebPart = "CarrierDashboardWebPart";
    internal const string DriversManager = "DriversManager";
    internal const string TrailerManager = "TrailerManager";
    internal const string TransportResources = "TransportResources";
    internal const string TruckManager = "TruckManager";
    internal const string CurrentUserWebPart = "CurrentUserWebPart";
    internal const string GuardWebPart = "GuardWebPart";
    internal const string LoadDescriptionWebPart = "LoadDescriptionWebPart";
    internal const string TimeSlotWebPart = "TimeSlotWebPart"; 
  }
}

