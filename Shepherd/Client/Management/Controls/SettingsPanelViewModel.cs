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

using CAS.Common.ComponentModel;
using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
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
  public class SettingsPanelViewModel : ViewModelBase<SettingsPanelViewModel.SetupDataDialogMachineLocal>
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPanelViewModel"/> class.
    /// </summary>
    [ImportingConstructor]
    public SettingsPanelViewModel(ILoggerFacade loggingService, IEventAggregator eventAggregator)
    {
      m_EventAggregator = eventAggregator;
      m_ILoggerFacade = loggingService;
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
    /// Sets the master shell view model. It activates the state <see cref="SetupDataDialogMachine" />
    /// </summary>
    /// <value>The master shell view model <see cref="ShellViewModel" />.</value>
    [Import]
    public ShellViewModel MasterShellViewModel
    {
      set
      {
        base.EnterState(value);
      }
    }
    #endregion

    #region private
    //vars
    private string b_URL = string.Empty;
    private string b_DatabaseName = string.Empty;
    private string b_SQLServer = string.Empty;
    private IEventAggregator m_EventAggregator;
    private ILoggerFacade m_ILoggerFacade;
    //types
    public class SetupDataDialogMachineLocal : SetupDataDialogMachine<SettingsPanelViewModel>
    {
      protected override SetupDataDialogMachine<SettingsPanelViewModel>.ConnectionDescription GetConnectionData
      {
        get
        {
          return new ConnectionDescription()
            {
              SharePointServerURL = this.ViewModelContext.URL,
              DatabaseName = this.ViewModelContext.DatabaseName,
              SQLServer = this.ViewModelContext.SQLServer,
            };
        }
      }
      protected internal override Services.ConnectionData DataContentState
      {
        set
        {
          if (value.SPConnected)
            this.ViewModelContext.m_EventAggregator.GetEvent<Infrastructure.SharePointWebsiteEvent>().Publish(new Infrastructure.SharePointWebsiteData(this.ViewModelContext.URL, null));
          if (value.SPConnected)
            this.ViewModelContext.m_EventAggregator.GetEvent<Infrastructure.SharePointWebsiteEvent>().Publish(new Infrastructure.SharePointWebsiteData(" ERR ", null));
        }
      }
    }
    //methods
    private void RestoreSettings()
    {
      m_ILoggerFacade.Log("Restoring user settings", Category.Debug, Priority.None);
      //User
      URL = Properties.Settings.Default.SiteURL;
      DatabaseName = Properties.Settings.Default.SQLDatabaseName;
      SQLServer = Properties.Settings.Default.SQLServer;
    }
    #endregion

  }
}
