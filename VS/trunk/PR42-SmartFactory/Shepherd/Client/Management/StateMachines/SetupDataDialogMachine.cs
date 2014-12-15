﻿//<summary>
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
using System;
using System.ComponentModel;
using System.Data.SqlClient;

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

    #region IAbstractMachineState
    /// <summary>
    /// Gets the state machine actions array.
    /// </summary>
    /// <value>The state machine actions array.</value>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Action<object>[] StateMachineActionsArray
    {
      get { return m_StateMachineActionsArray; }
    }
    #endregion

    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Context.EnabledEvents = StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    public override void OnCancellation()
    {
      base.OnCancellation();
    }
    public override void OnException(Exception exception)
    {
      base.OnException(exception);
      m_ButtonsTemplate.OnlyTemplate();
    }
    public override void OnExitingState()
    {
      base.OnExitingState();
      //Context.SaveSettings();
    }
    #region BackgroundWorkerMachine
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
      //string _connectionString = this.GetConnectionString();
      //ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _connectionString)));
      //System.Data.IDbConnection _connection = new SqlConnection(_connectionString);
      //IPRDEV _entities = new IPRDEV(_connection);
      //WorkerReturnData m_WorkerReturnData = new WorkerReturnData();
      //if (_entities.DatabaseExists())
      //{
      //  ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database exists."));
      //  GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Cleanup, ref m_WorkerReturnData.CleanupLastRunBy, ref m_WorkerReturnData.CleanupLastRunDate);
      //  GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Synchronization, ref m_WorkerReturnData.SyncLastRunBy, ref m_WorkerReturnData.SyncLastRunDate);
      //  GetLastOperation(_entities, ArchivingOperationLogs.OperationName.Archiving, ref m_WorkerReturnData.ArchivingLastRunBy, ref m_WorkerReturnData.ArchivingLastRunDate);
      //}
      //else
      //  ReportProgress(this, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      //NsSPLinq.Settings.GetCurrentContentVersion(Properties.Settings.Default.SiteURL, ref m_WorkerReturnData.CurrentContentVersion, ReportProgress);
      //ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("The current version of the site content is: {0}", m_WorkerReturnData.CurrentContentVersion.ToString())));
      //e.Result = m_WorkerReturnData;
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void RunWorkerCompleted(object result)
    {
      Context.EnabledEvents = StateMachineEvents.LeftButtonEvent | StateMachineEvents.LeftMiddleButtonEvent | StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent;
    }
    /// <summary>
    ///  Called when only cancel button must be active - after starting background worker.
    /// </summary>
    protected override void OnlyCancelActive()
    {
      m_ButtonsTemplate.OnlyCancel();
    }
    protected override ButtonsPanelState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    #endregion

    #region abstract
    /// <summary>
    /// Gets the URL of the SharePoint application website.
    /// </summary>
    /// <value>The URL.</value>
    protected abstract string URL { get; }
    /// <summary>
    /// Gets the name of the SQL database containing backup of SharePoint application data content.
    /// </summary>
    /// <value>The name of the database.</value>
    protected abstract string DatabaseName { get; }
    /// <summary>
    /// Gets the SQL server part of the connection string.
    /// </summary>
    /// <value>The SQL server address.</value>
    protected abstract string SQLServer { get; }
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

    private ConnectCancelTemplate m_ButtonsTemplate = null;
    private Action<object>[] m_StateMachineActionsArray;
    private string GetConnectionString()
    {
      return String.Format(Properties.Settings.Default.ConnectionString, SQLServer, DatabaseName);
    }
    #endregion

  }
}
