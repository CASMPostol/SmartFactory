//<summary>
//  Title   : class SetupDataDialogMachine
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
using CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using CAS.SmartFactory.IPR.Client.UserInterface.ViewModel;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using NsSPLinq = CAS.SmartFactory.IPR.Client.DataManagement.Linq;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SetupDataDialogMachine : BackgroundWorkerMachine<MainWindowModel>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine" /> class.
    /// </summary>
    public SetupDataDialogMachine() { }

    #region AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
      Context.ButtonNextTitle = Properties.Resources.ButtonInactive;
      Context.ButtonGoBackwardTitle = Properties.Resources.ButtonInactive;
      RunAsync();
    }
    public override void Next()
    {
      if (!Success)
        Context.EnterState<SetupDataDialogMachine>();
      else
        Context.EnterState<CleanupMachine>();
    }
    public override void Previous()
    {
      base.Previous();
      Context.EnterState<SetupDataDialogMachine>();
    }
    public override string ToString()
    {
      return "Setup";
    }
    public override void OnException(System.Exception exception)
    {
      Context.Exception(exception);
      SetEventMask(Events.Cancel | Events.Previous);
      Context.ButtonNextTitle = Properties.Resources.ButtonInactive;
      Context.ButtonGoBackwardTitle = Properties.Resources.ButtonConnect;
      Context.SetStatus2Error();
    }
    public override void OnExitingState()
    {
      base.OnExitingState();
      Context.SaveSettings();
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = null;
      string _connectionString = ViewModel.MainWindowModel.GetConnectionString();
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(_connectionString);
      IPRDEV _entities = new IPRDEV(_connection);
      WorkerReturnData m_WorkerReturnData = new WorkerReturnData();
      if (_entities.DatabaseExists())
      {
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database exists."));
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Cleanup, ref m_WorkerReturnData.CleanupLastRunBy, ref m_WorkerReturnData.CleanupLastRunDate);
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Synchronization, ref m_WorkerReturnData.SyncLastRunBy, ref m_WorkerReturnData.SyncLastRunDate);
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Archiving, ref m_WorkerReturnData.ArchivingLastRunBy, ref m_WorkerReturnData.ArchivingLastRunDate);
      }
      else
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      NsSPLinq.Settings.GetCurrentContentVersion(Properties.Settings.Default.SiteURL, ref m_WorkerReturnData.CurrentContentVersion, ReportProgress);
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("The current version of the site content is: {0}", m_WorkerReturnData.CurrentContentVersion.ToString())));
      e.Result = m_WorkerReturnData;
    }
    /// <summary>
    /// Runs the worker completed.
    /// </summary>
    /// <param name="result">The result.</param>
    protected override void RunWorkerCompleted(object result)
    {
      WorkerReturnData m_WorkerReturnData = (WorkerReturnData)result;
      Context.SyncLastRunBy = m_WorkerReturnData.SyncLastRunBy;
      Context.SyncLastRunDate = m_WorkerReturnData.SyncLastRunDate;
      Context.CleanupLastRunBy = m_WorkerReturnData.CleanupLastRunBy;
      Context.CleanupLastRunDate = m_WorkerReturnData.CleanupLastRunDate;
      Context.ArchivingLastRunBy = m_WorkerReturnData.ArchivingLastRunBy;
      Context.ArchivingLastRunDate = m_WorkerReturnData.ArchivingLastRunDate;
      Context.CurrentContentVersion = m_WorkerReturnData.CurrentContentVersion;
      SetEventMask(Events.Cancel | Events.Next | Events.Previous);
      Context.ButtonNextTitle = Properties.Resources.ButtonRun;
      Context.ButtonGoBackwardTitle = Properties.Resources.ButtonConnect;
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "Connected, the data has been retrieved successfully."));
    }
    #endregion

    #region private
    private static void GetLastOperation(IPRDEV _entities, ArchivingOperationLogs.OperationName operationName, ref string RunBy, ref string RunDate)
    {
      ArchivingOperationLogs _recentActions = ArchivingOperationLogs.GetRecentActions(_entities, operationName);
      if (_recentActions != null)
      {
        RunBy = _recentActions.UserName;
        RunDate = _recentActions.Date.ToString("G");
      }
      else
      {
        RunBy = Properties.Settings.Default.RunByUnknown;
        RunDate = Properties.Settings.Default.RunDateUnknown;
      }
    }
    private class WorkerReturnData
    {
      public string CleanupLastRunBy = Properties.Settings.Default.RunByError;
      public string SyncLastRunBy = Properties.Settings.Default.RunByError;
      public string ArchivingLastRunBy = Properties.Settings.Default.RunByError;
      public string CleanupLastRunDate = Properties.Settings.Default.RunDateError;
      public string SyncLastRunDate = Properties.Settings.Default.RunDateError;
      public string ArchivingLastRunDate = Properties.Settings.Default.RunDateError;
      public Version CurrentContentVersion = new Version();
    }
    #endregion

  }
}
