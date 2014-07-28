//<summary>
//  Title   : class MainWindowModel
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

using CAS.SharePoint.ViewModel.Wizard;
using CAS.SmartFactory.IPR.Client.UserInterface.StateMachine;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CAS.SmartFactory.IPR.Client.UserInterface.ViewModel
{
  internal class MainWindowModel : StateMachineContext
  {

    #region public
    //creator
    public MainWindowModel()
    {
      //InitializeUI();
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString(4);
      ProgressList = new ObservableCollection<string>();
      DoArchivingIsChecked = false;
      DoSynchronizationIsChecked = false;
      DoCleanupIsChecked = true;
      RestoreSettings();
      //create state machine
      this.EnterState<SetupDataDialogMachine>();
      //SetupDataDialogMachine _enteringState = new SetupDataDialogMachine(this);
      //Components.Add(new ActivationMachine(this));
      //Components.Add(new ArchivingMachine(this));
      //new FinishedMachine(this);
      //this.OpenEntryState(_enteringState);
    }
    //UI API
    public string Title
    {
      get
      {
        return b_Title;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Title, "Title", this);
      }
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
    public int IPRAccountArchivalDelay
    {
      get
      {
        return b_IPRAccountArchivalDelay;
      }
      set
      {
        RaiseHandler<int>(value, ref b_IPRAccountArchivalDelay, "IPRAccountArchivalDelay", this);
      }
    }
    public ObservableCollection<string> ProgressList
    {
      get
      {
        return b_ProgressList;
      }
      set
      {
        RaiseHandler<System.Collections.ObjectModel.ObservableCollection<string>>(value, ref b_ProgressList, "ProgressList", this);
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
    public string SyncLastRunDate
    {
      get
      {
        return b_SyncLastRunDate;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SyncLastRunDate, "SyncLastRunDate", this);
      }
    }
    public string SyncLastRunBy
    {
      get
      {
        return b_SyncLastRunBy;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SyncLastRunBy, "SyncLastRunBy", this);
      }
    }
    public string CleanupLastRunDate
    {
      get
      {
        return b_CleanupLastRunDate;
      }
      set
      {
        RaiseHandler<string>(value, ref b_CleanupLastRunDate, "CleanupLastRunDate", this);
      }
    }
    public string CleanupLastRunBy
    {
      get
      {
        return b_CleanupLastRunBy;
      }
      set
      {
        RaiseHandler<string>(value, ref b_CleanupLastRunBy, "CleanupLastRunBy", this);
      }
    }
    public string ArchivingLastRunDate
    {
      get
      {
        return b_ArchivingLastRunDate;
      }
      set
      {
        RaiseHandler<string>(value, ref b_ArchivingLastRunDate, "ArchivingLastRunDate", this);
      }
    }
    public string ArchivingLastRunBy
    {
      get
      {
        return b_ArchivingLastRunBy;
      }
      set
      {
        RaiseHandler<string>(value, ref b_ArchivingLastRunBy, "ArchivingLastRunBy", this);
      }
    }
    public bool DoCleanupIsChecked
    {
      get
      {
        return b_DoCleanupIsChecked;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_DoCleanupIsChecked, "DoCleanupIsChecked", this);
      }
    }
    public bool DoSynchronizationIsChecked
    {
      get
      {
        return b_DoSynchronizationIsChecked;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_DoSynchronizationIsChecked, "DoSynchronizationIsChecked", this);
      }
    }
    public bool DoArchivingIsChecked
    {
      get
      {
        return b_DoArchivingIsChecked;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_DoArchivingIsChecked, "DoArchivingIsChecked", this);
      }
    }
    public int ReportsArchivalDelay
    {
      get
      {
        return b_ReportsArchivalDelay;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ReportsArchivalDelay, "ReportsArchivalDelay", this);
      }
    }
    public int BatchArchivalDelay
    {
      get
      {
        return b_BatchArchivalDelay;
      }
      set
      {
        RaiseHandler<int>(value, ref b_BatchArchivalDelay, "BatchArchivalDelay", this);
      }
    }
    #endregion

    #region StateMachineContext
    public override void ProgressChang(IAbstractMachine activationMachine, System.ComponentModel.ProgressChangedEventArgs entitiesState)
    {
      base.ProgressChang(activationMachine, entitiesState);
      if (entitiesState.UserState is string)
        ProgressList.Add((String)entitiesState.UserState);
    }
    internal void SaveSettings()
    {
      Properties.Settings.Default.SiteURL = URL;
      Properties.Settings.Default.SQLDatabaseName = DatabaseName;
      Properties.Settings.Default.SQLServer = SQLServer;
      Properties.Settings.Default.ArchiveIPRDelay = IPRAccountArchivalDelay;
      Properties.Settings.Default.ArchiveBatchDelay = BatchArchivalDelay;
      Properties.Settings.Default.ReportsArchivalDelay = ReportsArchivalDelay;
      Properties.Settings.Default.Save();
    }
    #endregion

    #region private
    //backing fields
    private string b_Title;
    private int b_IPRAccountArchivalDelay;
    private int b_ReportsArchivalDelay;
    private int b_BatchArchivalDelay;
    private string b_URL;
    private string b_DatabaseName;
    private ObservableCollection<string> b_ProgressList;
    private string b_SQLServer;
    private string b_SyncLastRunDate;
    private string b_SyncLastRunBy;
    private string b_CleanupLastRunDate;
    private string b_CleanupLastRunBy;
    private string b_ArchivingLastRunDate;
    private string b_ArchivingLastRunBy;
    private bool b_DoCleanupIsChecked;
    private bool b_DoSynchronizationIsChecked;
    private bool b_DoArchivingIsChecked;
    //methods
    private void RestoreSettings()
    {
      //User
      URL = Properties.Settings.Default.SiteURL;
      DatabaseName = Properties.Settings.Default.SQLDatabaseName;
      SQLServer = Properties.Settings.Default.SQLServer;
      IPRAccountArchivalDelay = Properties.Settings.Default.ArchiveIPRDelay;
      BatchArchivalDelay = Properties.Settings.Default.ArchiveBatchDelay;
      ReportsArchivalDelay = Properties.Settings.Default.ReportsArchivalDelay;
      //Application
      CleanupLastRunBy = Properties.Settings.Default.RunByUnknown;
      SyncLastRunBy = Properties.Settings.Default.RunByUnknown;
      ArchivingLastRunBy = Properties.Settings.Default.RunByUnknown;
      CleanupLastRunDate = Properties.Settings.Default.RunDateUnknown;
      SyncLastRunDate = Properties.Settings.Default.RunDateUnknown;
      ArchivingLastRunDate = Properties.Settings.Default.RunDateUnknown;
    }
    #endregion

  }
}
