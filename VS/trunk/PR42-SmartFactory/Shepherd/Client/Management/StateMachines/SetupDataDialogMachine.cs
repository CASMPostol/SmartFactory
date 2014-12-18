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
using CAS.SmartFactory.Shepherd.Client.Management.Services;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
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
      m_ButtonsTemplate = new ConnectCancelTemplate();
      m_ButtonsTemplate.LeftButtonTitle = "Route edit";
      m_ButtonsTemplate.LeftButtonVisibility = System.Windows.Visibility.Visible;
      m_ButtonsTemplate.LeftMiddleButtonTitle = "Archive";
      m_ButtonsTemplate.LeftMiddleButtonVisibility = System.Windows.Visibility.Visible;
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[m_ButtonsTemplate.ConnectPosition] = x => this.OnConnectCommand();
      m_StateMachineActionsArray[m_ButtonsTemplate.CancelPosition] = x => this.OnCancellation();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => this.OnRouteEditCommand();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => this.OnArchiveCommand();
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
      ConnectionDescription _connectionDescription = this.GetConnectionData;
      string _connectionString = this.GetConnectionString(_connectionDescription);
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(_connectionString);
      SHRARCHIVE _entities = new SHRARCHIVE(_connection);
      ConnectionData _cd = ConnectionData.ThisInstance;
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
      _cd.SPConnected = NsSPLinq.Connectivity.TestConnection(_connectionDescription.SharePointServerURL, x => ReportProgress(this, x));
      e.Result = _cd;
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void RunWorkerCompleted(object result)
    {
      ConnectionData _cdResult = (ConnectionData)result;
      StateMachineEvents _events = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
      if (_cdResult.SPConnected)
        _events |=  StateMachineEvents.LeftMiddleButtonEvent;
      if (_cdResult.SPConnected)
        _events |= StateMachineEvents.LeftMiddleButtonEvent;
      Context.EnabledEvents = _events;
      DataContentState = _cdResult;
    }
    /// <summary>
    ///  Called when only cancel button must be active - after starting background worker.
    /// </summary>
    protected override void OnlyCancelActive()
    {
      m_ButtonsTemplate.OnlyCancel();
    }
    /// <summary>
    /// Gets the state of the buttons panel.
    /// </summary>
    /// <value>The state of the buttons panel.</value>
    protected override ButtonsPanelState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    #endregion

    #region abstract
    protected struct ConnectionDescription
    {
      /// <summary>
      /// Gets the URL of the SharePoint application website.
      /// </summary>
      /// <value>The URL.</value>
      internal string SharePointServerURL;
      /// <summary>
      /// Gets the name of the SQL database containing backup of SharePoint application data content.
      /// </summary>
      /// <value>The name of the database.</value>
      internal string DatabaseName;
      /// <summary>
      /// Gets the SQL server part of the connection string.
      /// </summary>
      /// <value>The SQL server address.</value>
      internal string SQLServer;
    }
    /// <summary>
    /// Gets the get connection data.
    /// </summary>
    /// <value>The get connection data.</value>
    protected abstract ConnectionDescription GetConnectionData { get; }
    /// <summary>
    /// Sets the state of the data content.
    /// </summary>
    /// <value>The state of the data content.</value>
    internal protected abstract Services.ConnectionData DataContentState { set; }
    #endregion

    #region private
    private void OnConnectCommand()
    {
      this.RunAsync();
    }
    private object OnArchiveCommand()
    {
      throw new NotImplementedException();
    }
    private object OnRouteEditCommand()
    {
      throw new NotImplementedException();
    }
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
