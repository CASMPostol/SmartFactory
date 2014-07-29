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
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SetupDataDialogMachine : BackgroundWorkerMachine<MainWindowModel>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    public SetupDataDialogMachine() { }

    #region AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Success = false;
      SetEventMask(Events.Cancel);
      Context.ButtonNextTitle = " --- ";
      RunAsync();
    }
    public override void Next()
    {
      if (Success)
        Context.EnterState<SetupDataDialogMachine>(); //TODO for testing only
      //Context.Machine = new CleanupMachine(Context);
      else
        Context.EnterState<SetupDataDialogMachine>();
    }
    public override string ToString()
    {
      return "Setup";
    }
    public override void OnException(System.Exception exception)
    {
      Context.Exception(exception);
      SetEventMask(Events.Cancel | Events.Next);
      Context.ButtonNextTitle = "Connect";
      Context.SetStatus2Error();
      Success = false;
    }
    public override void OnExitingState()
    {
      base.OnExitingState();
      Context.SaveSettings();
    }
    protected override void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
      base.BackgroundWorker_RunWorkerCompleted(sender, e);
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      string _connectionString = String.Format(Properties.Settings.Default.ConnectionString, Properties.Settings.Default.SQLServer, Properties.Settings.Default.SQLDatabaseName);
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(_connectionString);
      IPRDEV _entities = new IPRDEV(_connection);
      if (_entities.DatabaseExists())
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database can be opened."));
      else
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      WorkerReturnData m_WorkerReturnData = new WorkerReturnData();
      GetLastOperation(_entities, ActivitiesLogs.CleanupOperationName, ref m_WorkerReturnData.CleanupLastRunBy, ref m_WorkerReturnData.CleanupLastRunDate);
      GetLastOperation(_entities, ActivitiesLogs.SynchronizationOperationName, ref m_WorkerReturnData.SyncLastRunBy, ref m_WorkerReturnData.SyncLastRunDate);
      GetLastOperation(_entities, ActivitiesLogs.ArchivingOperationName, ref m_WorkerReturnData.ArchivingLastRunBy, ref m_WorkerReturnData.ArchivingLastRunDate);
      e.Result = m_WorkerReturnData;
    }
    private static void GetLastOperation(IPRDEV _entities, string operationName, ref string RunBy, ref string RunDate)
    {
      ActivitiesLogs _recentActions = _entities.ActivitiesLogs.Where<ActivitiesLogs>(x => x.Operation.Contains(operationName)).OrderByDescending<ActivitiesLogs, DateTime>(x => x.Date).FirstOrDefault();
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
      SetEventMask(Events.Cancel | Events.Next);
      Context.ButtonNextTitle = "Run";
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "The data has been retrieved successfully."));
    }
    #endregion
    private class WorkerReturnData
    {
      public string CleanupLastRunBy = Properties.Settings.Default.RunByError;
      public string SyncLastRunBy = Properties.Settings.Default.RunByError;
      public string ArchivingLastRunBy = Properties.Settings.Default.RunByError;
      public string CleanupLastRunDate = Properties.Settings.Default.RunDateError;
      public string SyncLastRunDate = Properties.Settings.Default.RunDateError;
      public string ArchivingLastRunDate = Properties.Settings.Default.RunDateError;
    }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="SetupDataDialogMachine"/> finished the background operation successfully.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    public bool Success { get; set; }
  }
}
