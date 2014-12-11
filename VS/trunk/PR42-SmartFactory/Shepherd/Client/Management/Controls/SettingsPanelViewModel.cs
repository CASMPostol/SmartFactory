﻿//<summary>
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
  public class SettingsPanelViewModel : PropertyChangedBase, IViewModelContext
  {
    public SettingsPanelViewModel()
    {
      RestoreSettings();
    }
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
    /// <summary>
    /// Sets the master shell view model. It activates the state <see cref="SetupDataDialogMachine"/>
    /// </summary>
    /// <value>The master shell view model <see cref="ShellViewModel"/>.</value>
    [Import]
    public ShellViewModel MasterShellViewModel
    {
      set
      {
        SetupDataDialogMachineLocal _myState = value.EnterState<SetupDataDialogMachineLocal>(this);
      }
    }
    //vars
    private string b_URL = string.Empty;
    private string b_DatabaseName = string.Empty;
    private string b_SQLServer = string.Empty;
    private class SetupDataDialogMachineLocal : SetupDataDialogMachine<SettingsPanelViewModel>
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
  }
}
