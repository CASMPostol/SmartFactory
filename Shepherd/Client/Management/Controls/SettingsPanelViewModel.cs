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
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export]
  public class SettingsPanelViewModel : ViewModelBase<SettingsPanelViewModel.SetupDataDialogMachineLocal>
  {
    #region creator
    public SettingsPanelViewModel()
    {
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
    public class SetupDataDialogMachineLocal : SetupDataDialogMachine<SettingsPanelViewModel>
    {
      protected override string URL { get { return this.ViewModelContext.URL; } }
      protected override string DatabaseName { get { return this.ViewModelContext.DatabaseName; } }
      protected override string SQLServer { get { return this.ViewModelContext.SQLServer; } }

      protected override void OnlyCancelActive()
      {
        m_ButtonsTemplate.OnlyCancel();
      }

      protected override ButtonsPanelState ButtonsPanelState
      {
        get { return m_ButtonsTemplate; }
      }

      public override System.Action<object>[] StateMachineActionsArray
      {
        get { throw new System.NotImplementedException(); }
      }
      private ConnectCancelTemplate m_ButtonsTemplate = new ConnectCancelTemplate();
    }
    //methods
    private void RestoreSettings()
    {
      //User
      URL = Properties.Settings.Default.SiteURL;
      DatabaseName = Properties.Settings.Default.SQLDatabaseName;
      SQLServer = Properties.Settings.Default.SQLServer;
    }
    #endregion

  }
}
