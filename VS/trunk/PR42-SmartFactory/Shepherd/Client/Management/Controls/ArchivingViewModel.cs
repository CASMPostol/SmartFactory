//<summary>
//  Title   : ArchivingViewModel
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

using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class ArchivingViewModel - it is view model for the <see cref="ArchivingMachineState{ArchivingViewModel}"/>
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class ArchivingViewModel : ViewModelStateMachineBase<ArchivingViewModel.ArchivingMachineStateLocal>
  {

    /// <summary>
    /// Importing constructor that initializes a new instance of the <see cref="ArchivingViewModel"/> class. 
    /// The constructor is to be used by the composition infrastructure.
    /// </summary>
    /// <param name="loggingService">The logging service.</param>
    [ImportingConstructor]
    public ArchivingViewModel(ILoggerFacade loggingService)
      : base(loggingService)
    {
      loggingService.Log("Created ArchivingViewModel.", Category.Debug, Priority.Low);
      m_loggingService = loggingService;
    }

    #region UI API
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
    public int ArchivalDelay
    {
      get
      {
        return b_ArchivalDelay;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ArchivalDelay, "ArchivalDelay", this);
      }
    }
    public int RowLimit
    {
      get
      {
        return b_RowLimit;
      }
      set
      {
        RaiseHandler<int>(value, ref b_RowLimit, "RowLimit", this);
      }
    }

    #endregion

    #region ViewModelStateMachineBase
    public class ArchivingMachineStateLocal : ArchivingMachineState<ArchivingViewModel>
    {
      /// <summary>
      /// Logs the specified message.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="category">The category.</param>
      /// <param name="priority">The priority.</param>
      public override void Log(string message, Category category, Priority priority)
      {
        ViewModelContext.Log(message, category, priority);
      }
      protected override ArchivingMachineState<ArchivingViewModel>.BackgroundProcessArgument GetArgument
      {
        get
        {
          Phases _phase = ViewModelContext.DoCleanupIsChecked ? Phases.CleanupContent : (Phases)0;
          if (ViewModelContext.DoSynchronizationIsChecked)
            _phase = Phases.CleanupContent | Phases.SynchronizationContent;
          else if (ViewModelContext.DoArchivingIsChecked)
            _phase = Phases.ArchivingContent | Phases.CleanupContent | Phases.SynchronizationContent;
          return new BackgroundProcessArgument()
          {
            Phases = _phase,
            SQLConnectionString = ViewModelContext.SQLServer,
            URL = ViewModelContext.URL,
            ArchivalDelay = ViewModelContext.ArchivalDelay,
            RowLimit = ViewModelContext.RowLimit
          };
        }
      }
    }
    //INavigationAware
    /// <summary>
    /// Called when the implementer has been navigated to.
    /// </summary>
    /// <param name="navigationContext">The navigation context <see cref="NavigationContext"/>.</param>
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
      base.OnNavigatedTo(navigationContext);
      RestoreSettings();
      ISPContentState _spContentState = (ISPContentState)navigationContext.Parameters[typeof(ISPContentState).Name];
      Debug.Assert(_spContentState != null, string.Format("{0} parameter cannot be null while navigating to ArchivingViewModel.", typeof(ISPContentState).Name));
      Debug.Assert(_spContentState.SPConnected == true, "SP has to be connected before navigating to ArchivingViewModel.");
      Debug.Assert(!String.IsNullOrEmpty(_spContentState.SharePointWebsiteURL), "SharePoint Website URL cannot be empty.");
      URL = _spContentState.SharePointWebsiteURL;
      ISQLContentState _sqlContentState = (ISQLContentState)navigationContext.Parameters[typeof(ISQLContentState).Name];
      Debug.Assert(_sqlContentState != null, string.Format("{0} parameter cannot be null while navigating to ArchivingViewModel.", typeof(ISPContentState).Name));
      Debug.Assert(_sqlContentState.SQLConnected == true, "SP has to be connected before navigating to ArchivingViewModel.");
      Debug.Assert(!String.IsNullOrEmpty(_sqlContentState.SQLConnectionString), "Connection string cannot be empty.");
      SQLServer = _sqlContentState.SQLConnectionString;
      SyncLastRunDate = _sqlContentState.SyncLastRunDate.LocalizedString();
      SyncLastRunBy = _sqlContentState.SyncLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown);
      CleanupLastRunDate = _sqlContentState.CleanupLastRunDate.LocalizedString();
      CleanupLastRunBy = _sqlContentState.CleanupLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown);
      ArchivingLastRunDate = _sqlContentState.ArchivingLastRunDate.LocalizedString();
      ArchivingLastRunBy = _sqlContentState.ArchivingLastRunBy.GetValueOrDefault(Settings.Default.RunByUnknown);
      //Log the current state.
      string _msg = String.Format("OnNavigatedTo - created view model {0} for SharePoint: {1} and database {2}.", typeof(ArchivingViewModel).Name, URL, SQLServer);
      m_loggingService.Log(_msg, Category.Debug, Priority.Low);
    }
    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
      base.OnNavigatedFrom(navigationContext);
      SaveSettings();
    }
    #endregion

    #region private

    #region backing fields
    public string b_SQLServer = String.Empty;
    private string b_URL = String.Empty;
    private string b_SyncLastRunDate = Settings.Default.RunDateUnknown;
    private string b_SyncLastRunBy = Settings.Default.RunByUnknown;
    private string b_CleanupLastRunDate = Settings.Default.RunDateUnknown;
    private string b_CleanupLastRunBy = Settings.Default.RunByUnknown;
    private string b_ArchivingLastRunDate = Settings.Default.RunDateUnknown;
    private string b_ArchivingLastRunBy = Settings.Default.RunByUnknown;
    private bool b_DoCleanupIsChecked = false;
    private bool b_DoSynchronizationIsChecked = false;
    private bool b_DoArchivingIsChecked = false;
    private ILoggerFacade m_loggingService = null;
    private int b_ArchivalDelay;
    private int b_RowLimit;
    #endregion
    //methods
    private void RestoreSettings()
    {
      this.Log("Restoring user settings from configuration file.", Category.Debug, Priority.None);
      //User
      this.ArchivalDelay = Properties.Settings.Default.ArchivalDelay;
      this.RowLimit = Properties.Settings.Default.RowLimit;
    }
    private void SaveSettings()
    {
      this.Log("Saving user settings to configuration file.", Category.Debug, Priority.None);
      Properties.Settings.Default.ArchivalDelay = this.ArchivalDelay;
      Properties.Settings.Default.RowLimit = this.RowLimit;
      Settings.Default.Save();
    }
    #endregion

  }
}
