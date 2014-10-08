//<summary>
//  Title   : App
//  System  : Microsoft VisulaStudio 2013 / C#
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
using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace CAS.SmartFactory.IPR.Client.UserInterface
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      this.Startup += App_Startup;
    }
    private void App_Startup(object sender, StartupEventArgs e)
    {
      if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun || Environment.CommandLine.ToLower().Contains("installsample"))
      {
        CreateEventSourceSample1();
      }
    }
    private void CreateEventSourceSample1()
    {
      try
      {
        string _sourceName = CAS.SharePoint.Client.Properties.Settings.Default.EvenlLogSource;
        // Create the event source if it does not exist.
        if (!EventLog.SourceExists(_sourceName))
        {
          EventSourceCreationData mySourceData = new EventSourceCreationData(_sourceName, String.Empty);
          EventLog.CreateEventSource(mySourceData);
        }
        AssemblyName _name = Assembly.GetEntryAssembly().GetName();
        string _msg = String.Format("Installed CAS application {0} Version {1}", _name.Name, _name.Version);
        EventLog.WriteEntry(CAS.SharePoint.Client.Properties.Settings.Default.EvenlLogSource, _msg, System.Diagnostics.EventLogEntryType.SuccessAudit, 224, 0);
      }
      catch (Exception ex)
      {
        string _mt = String.Format("Problem with registration of the event log source name for application because of exception {0}", ex.Message);
        MessageBox.Show(_mt, "Software Installation", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}

