//_______________________________________________________________
//  Title   : CASEventReceiver
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint.Common.ServiceLocation;
using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Runtime.InteropServices;

namespace CAS.SmartFactory.IPR.WebsiteModel.Features
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>
  [Guid("2bd3db7c-a653-4419-b846-842832900973")]
  public class CASEventReceiver : SPFeatureReceiver
  {
    /// <summary>
    /// Occurs after a Feature is activated.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
    public override void FeatureActivated(SPFeatureReceiverProperties properties)
    {
      try
      {
        if (!System.Diagnostics.EventLog.SourceExists(SourceName))
          System.Diagnostics.EventLog.CreateEventSource(new System.Diagnostics.EventSourceCreationData(SourceName, "Application"));
        WebsiteModelExtensions.RegisterLoggerSource();
        WebsiteModelExtensions.TraceEvent
          (String.Format("CAS.SmartFactory.IPR.WebsiteModel FeatureInstalled: {0}", properties.Definition.DisplayName), 44, TraceSeverity.High, WebsiteModelExtensions.LoggingCategories.FeatureActivation);
      }
      catch (Exception _ex)
      {
        System.Diagnostics.EventLog.WriteEvent(SourceName, new System.Diagnostics.EventInstance(63, 0) { EntryType = System.Diagnostics.EventLogEntryType.Error }, _ex);
        throw;
      }
    }
    // Uncomment the method below to handle the event raised after a feature has been installed.
    //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
    //{
    //}
    // Uncomment the method below to handle the event raised before a feature is uninstalled.
    /// <summary>
    /// Occurs when a Feature is uninstalled.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
    public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
    {
      try
      {
        // Get the ServiceLocatorConfig service from the service locator.
        WebsiteModelExtensions.TraceEvent
          (String.Format("CAS.SmartFactory.IPR.WebsiteModel FeatureUninstalling: {0}", properties.Definition.DisplayName), 63, TraceSeverity.High, WebsiteModelExtensions.LoggingCategories.FeatureActivation);
        WebsiteModelExtensions.UnregisterLoggerSource();
      }
      catch (Exception _ex)
      {
        System.Diagnostics.EventLog.WriteEvent(SourceName, new System.Diagnostics.EventInstance(63, 0) { EntryType = System.Diagnostics.EventLogEntryType.Error }, _ex);
        throw;
      }
    }
    // Uncomment the method below to handle the event raised when a feature is upgrading.
    //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
    //{
    //}
    private static string SourceName = "CAS.SmartFactory.IPR";
  }
}
