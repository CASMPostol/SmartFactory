//<summary>
//  Title   : SetupDataDialogMachine
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

using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using CAS.SmartFactory.Shepherd.Client.Management.Services;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using NsSPLinq = CAS.SmartFactory.Shepherd.Client.DataManagement.Linq;

/// <summary>
/// The StateMachines namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  /// <summary>
  /// Class SetupDataDialogMachine.
  /// </summary>
  public abstract class SetupDataDialogMachine<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>
    where ViewModelContextType : IViewModelContext
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    public SetupDataDialogMachine()
    {
      m_ButtonsTemplate = new ConnectCancelTemplate(Resources.RouteEditButtonTitle, Resources.ArchiveButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.ConnectPosition] = x => this.OnConnectCommand();
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.Cancel();
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.LeftButtonPosition] = x => this.OnRouteEditCommand();
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.LeftMiddleButtonPosition] = x => this.OnArchiveCommand();
    }
    #endregion

    #region BackgroundWorkerMachine
    /// <summary>
    /// Gets the state machine actions array.
    /// </summary>
    /// <value>The current state machine actions array.</value>
    public override Action<object>[] StateMachineActionsArray
    {
      get { return m_StateMachineActionsArray; }
    }
    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Context.EnabledEvents = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    /// <summary>
    /// Called when exception has occurred. Make context aware about exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public override void OnException(Exception exception)
    {
      base.OnException(exception);
      //TODO Improve usage of teh mask bits. 
      Context.EnabledEvents = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Properties.Resources.SetupDataDialogMachineName;
    }
    /// <summary>
    /// Handles the DoWork event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = null;
      ConnectionData _cd = ConnectionData.ThisInstance;
      _cd.ConnectionDescription = this.GetConnectionData;
      string _connectionString = this.GetConnectionString(_cd.ConnectionDescription);
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(_connectionString);
      SHRARCHIVE _entities = new SHRARCHIVE(_connection);
      if (_entities.DatabaseExists())
      {
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database exists."));
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Cleanup, x => _cd.CleanupLastRunBy = x, x => _cd.CleanupLastRunDate = x);
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Synchronization, x => _cd.SyncLastRunBy = x, x => _cd.SyncLastRunDate = x);
        GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Archiving, x => _cd.ArchivingLastRunBy = x, x => _cd.ArchivingLastRunDate);
        _cd.SQLConnected = true;
      }
      else
        ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      _cd.SPConnected = NsSPLinq.Connectivity.TestConnection(_cd.ConnectionDescription.SharePointServerURL, x => ReportProgress(this, x));
      e.Result = _cd;
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void RunWorkerCompleted(object result)
    {
      m_ConnectionData = (ConnectionData)result;
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(m_ConnectionData.SQLConnected, m_ConnectionData.SPConnected);
      if (m_ConnectionData.SPConnected)
        this.PublishSPURL();
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "Operation Connect finished."));
    }
    /// <summary>
    ///  Called when only cancel button must be active - after starting background worker.
    /// </summary>
    protected override void OnlyCancelActive()
    {
      Context.EnabledEvents = m_ButtonsTemplate.OnlyCancel();
    }
    /// <summary>
    /// Gets the state of the buttons panel.
    /// </summary>
    /// <value>The state of the buttons panel.</value>
    protected override ButtonsSetState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    public override void Cancel()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(1, "Cancellation pending"));
      base.Cancel();
    }
    #endregion

    #region abstract
    /// <summary>
    /// Gets the get connection data.
    /// </summary>
    /// <value>The get connection data.</value>
    protected abstract ConnectionDescription GetConnectionData { get; }
    /// <summary>
    /// Publishes the SharePoint site URL.
    /// </summary>
    /// <param name="URL">The URL of the website.</param>
    internal protected abstract void PublishSPURL();
    #endregion

    #region private
    private void OnConnectCommand()
    {
      this.RunAsync(null);
    }
    private void OnArchiveCommand()
    {
      RequestNavigate(Infrastructure.ViewNames.ArchivalStateName);
    }
    private void OnRouteEditCommand()
    {
      RequestNavigate(Infrastructure.ViewNames.RouteEditorStateName);
    }

    private void RequestNavigate(string targetUri)
    {
      NavigationParameters _par = new NavigationParameters();
      Debug.Assert(m_ConnectionData.ConnectionDescription != null, "In OnRouteEditCommand the ConnectionDescription is null");
      _par.Add(targetUri, m_ConnectionData.ConnectionDescription);
      Context.RequestNavigate(targetUri, _par);
    }
    private Services.ConnectionData m_ConnectionData = null;
    private static void GetLastOperation(SHRARCHIVE entities, ArchivingOperationLogs.OperationName operationName, Func<string, string> RunBy, Func<string, string> RunDate)
    {
      ArchivingOperationLogs _recentActions = ArchivingOperationLogs.GetRecentActions(entities, operationName);
      if (_recentActions != null)
      {
        RunBy(_recentActions.UserName);
        RunDate(_recentActions.Date.ToString("G"));
      }
      else
      {
        RunBy(Properties.Settings.Default.RunByUnknown);
        RunDate(Properties.Settings.Default.RunDateUnknown);
      }
    }
    private ConnectCancelTemplate m_ButtonsTemplate = null;
    private Action<object>[] m_StateMachineActionsArray;
    private string GetConnectionString(ConnectionDescription data)
    {
      return String.Format(Properties.Settings.Default.ConnectionString, data.SQLServer, data.DatabaseName);
    }
    #endregion

  }
}
