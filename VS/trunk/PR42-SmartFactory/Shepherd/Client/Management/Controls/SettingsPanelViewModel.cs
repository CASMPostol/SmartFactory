//<summary>
//  Title   : SettingsPanelViewModel
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

using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

/// <summary>
/// The Controls namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class SettingsPanelViewModel provide ViewModel for Setting state
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class SettingsPanelViewModel : ViewModelStateMachineBase<SettingsPanelViewModel.SetupDataDialogMachineLocal>, INavigationAware
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPanelViewModel"/> class.
    /// </summary>
    [ImportingConstructor]
    public SettingsPanelViewModel(ILoggerFacade loggingService, IEventAggregator eventAggregator)
      : base(loggingService)
    {
      loggingService.Log("Creating SettingsPanelViewModel", Category.Debug, Priority.Low);
      m_EventAggregator = eventAggregator;
      RestoreSettings();
    }
    #endregion

    #region public UI API
    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    public string URL
    {
      get
      {
        return b_URL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_URL, "URL", this);
      }
    }
    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    /// <value>The name of the database.</value>
    public string DatabaseName
    {
      get
      {
        return b_DatabaseName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_DatabaseName, "DatabaseName", this);
      }
    }
    /// <summary>
    /// Gets or sets the SQL server.
    /// </summary>
    /// <value>The SQL server.</value>
    public string SQLServer
    {
      get
      {
        return b_SQLServer;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SQLServer, "SQLServer", this);
      }
    }
    #endregion

    #region public
    /// <summary>
    /// Class SetupDataDialogMachineLocal local implementation of the <see cref="SetupDataDialogMachine{SettingsPanelViewModel}"/>
    /// </summary>
    public sealed class SetupDataDialogMachineLocal : SetupDataDialogMachine
    {

      protected override ConnectionDescription GetConnectionDescription
      {
        get
        {
          return new ConnectionDescription(this.ViewModelContext.URL, this.ViewModelContext.DatabaseName, this.ViewModelContext.SQLServer);
        }
      }
      internal protected override void PublishSPURL()
      {
        this.ViewModelContext.m_EventAggregator.GetEvent<Infrastructure.SharePointWebsiteEvent>().Publish(new SharePointWebsiteData(ViewModelContext.URL, null));
      }
    }
    #endregion

    #region private
    //vars
    private string b_URL = string.Empty;
    private string b_DatabaseName = string.Empty;
    private string b_SQLServer = string.Empty;
    private IEventAggregator m_EventAggregator;
    //methods
    private void RestoreSettings()
    {
      this.Log("Restoring user settings from configuration file.", Category.Debug, Priority.None);
      //User
      URL = Settings.Default.SiteURL;
      DatabaseName = Settings.Default.SQLDatabaseName;
      SQLServer = Settings.Default.SQLServer;
    }
    internal void SaveSettings()
    {
      this.Log("Saving user settings to configuration file.", Category.Debug, Priority.None);
      Settings.Default.SiteURL = URL;
      Settings.Default.SQLDatabaseName = DatabaseName;
      Settings.Default.SQLServer = SQLServer;
      Settings.Default.Save();
    }
    #endregion

    #region INavigationAware
    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
      base.OnNavigatedFrom(navigationContext);
      SaveSettings();
    }
    #endregion

  }
}
