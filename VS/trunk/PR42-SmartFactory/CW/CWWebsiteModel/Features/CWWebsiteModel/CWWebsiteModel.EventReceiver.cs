//<summary>
//  Title   : This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Runtime.InteropServices;
using CAS.SharePoint.Common.Logging;
using CAS.SharePoint.Common.ServiceLocation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.CW.WebsiteModel.Features.CWWebsiteModel
{
  /// <summary>
  /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
  /// </summary>
  /// <remarks>
  /// The GUID attached to this class may be used during packaging and should not be modified.
  /// </remarks>
  [Guid( "acaa7339-77af-4a13-9a56-6c09b05da2f4" )]
  public class CWWebsiteModelEventReceiver: SPFeatureReceiver
  {
    //public override void FeatureActivated(SPFeatureReceiverProperties properties)
    //{
    //}
    //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
    //{
    //}

    /// <summary>
    /// Occurs after a Feature is installed.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
    public override void FeatureInstalled( SPFeatureReceiverProperties properties )
    {
      try
      {
        if ( !System.Diagnostics.EventLog.SourceExists( SourceName ) )
          System.Diagnostics.EventLog.CreateEventSource( new System.Diagnostics.EventSourceCreationData( SourceName, "Application" ) );
        IServiceLocator _serviceLocator = SharePointServiceLocator.GetCurrent();
        ILogger logger = _serviceLocator.GetInstance<ILogger>();
        logger.TraceToDeveloper( String.Format( "CAS.SmartFactory.CW.WebsiteModel FeatureInstalled: {0}", properties.Definition.DisplayName ), TraceSeverity.High );
        IServiceLocatorConfig _typeMappings = _serviceLocator.GetInstance<IServiceLocatorConfig>();
        _typeMappings.RegisterTypeMapping<CAS.SmartFactory.Customs.Account.ICWAccountFactory, CAS.SmartFactory.CW.WebsiteModel.Linq.Account.CWAccountData>();
        logger.TraceToDeveloper( "CAS.SmartFactory.CW.WebsiteModel TypeMapping registered", TraceSeverity.High );
      }
      catch ( Exception  _ex)
      {
        System.Diagnostics.EventLog.WriteEvent( SourceName, new System.Diagnostics.EventInstance( 63, 0 ) { EntryType = System.Diagnostics.EventLogEntryType.Error }, _ex );
        throw;
      }
    }
    /// <summary>
    /// Occurs when a Feature is uninstalled.
    /// </summary>
    /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
    public override void FeatureUninstalling( SPFeatureReceiverProperties properties )
    {
      try
      {
        // Get the ServiceLocatorConfig service from the service locator.
        IServiceLocator _serviceLocator = SharePointServiceLocator.GetCurrent();
        ILogger logger = _serviceLocator.GetInstance<ILogger>();
        logger.TraceToDeveloper( String.Format( "CAS.SmartFactory.CW.WebsiteModel FeatureUninstalling: {0}", properties.Definition.DisplayName ), TraceSeverity.Monitorable );
        IServiceLocatorConfig _typeMappings = _serviceLocator.GetInstance<IServiceLocatorConfig>();
        _typeMappings.RemoveTypeMapping<CAS.SmartFactory.Customs.Account.ICWAccountFactory>( null );
        logger.TraceToDeveloper( "CAS.SmartFactory.CW.WebsiteModel TypeMapping removed: ICWAccountFactory", TraceSeverity.High );
      }
      catch ( Exception _ex)
      {
        System.Diagnostics.EventLog.WriteEvent( SourceName, new System.Diagnostics.EventInstance( 63, 0 ) { EntryType = System.Diagnostics.EventLogEntryType.Error }, _ex );
        throw;
      }
    }
    //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
    //{
    //}
    internal static string SourceName = "CAS.SmartFactory.CW.WebsiteModel";
  }
}
