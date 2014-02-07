//<summary>
//  Title   : class GlobalDefinitions
//  System  : Microsoft Visual C# .NET 2012
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
      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  /// <summary>
  /// the class GlobalDefinitions contains common definitions for CAS.SmartFactory.Shepherd.Dashboards
  /// </summary>
  public partial class GlobalDefinitions
  {
    internal const string MasterPage = "cas.master";
    internal const string ShepherdResourceFileName = "CASSmartFactoryShepherdCode";
    /// <summary>
    /// delegate UpdateToolStripEvent
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="progres">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
    public delegate void UpdateToolStripEvent(object obj, ProgressChangedEventArgs progres);
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

