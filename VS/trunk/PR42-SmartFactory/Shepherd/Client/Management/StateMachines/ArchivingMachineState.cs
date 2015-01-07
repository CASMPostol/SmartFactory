//<summary>
//  Title   : ArchivingMachineState
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
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  public abstract class ArchivingMachineState<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>, ILoggerFacade
    where ViewModelContextType : IViewModelContext
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchivingMachineState{ViewModelContextType}"/> class.
    /// </summary>
    internal ArchivingMachineState()
    {
      m_ButtonsTemplate = new CancelTemplate(String.Empty, Resources.ArchiveButtonTitle, Resources.SetupButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.Cancel();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent] = x => this.OnSetupButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => this.OnArchiveButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => { };
    }

    #region object
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Infrastructure.ViewNames.ArchivalStateName;
    }
    #endregion

    #region BackgroundWorkerMachine
    public override void OnEnteringState()
    {
      this.Log(String.Format("OnEnteringState {0}", Infrastructure.ViewNames.ArchivalStateName), Category.Debug, Priority.Low);
      base.OnEnteringState();
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      ReportProgress(this, new System.ComponentModel.ProgressChangedEventArgs(1, "Starting archiving background process."));
      BackgroundProcessArgument _argument = (BackgroundProcessArgument)e.Argument;
      DataManagement.CleanupContent.DoCleanupContent(_argument.URL, x => ReportProgress(this, x), y => this.Log(y, Category.Debug, Priority.None));
      ReportProgress(this, new System.ComponentModel.ProgressChangedEventArgs(1, "Finished archiving background process."));
    }
    protected override void RunWorkerCompleted(object result)
    {
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
    }
    protected override void OnlyCancelActive()
    {
      Context.EnabledEvents = m_ButtonsTemplate.OnlyCancel();
    }
    protected override ButtonsSetState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    public override Action<object>[] StateMachineActionsArray
    {
      get { return m_StateMachineActionsArray; }
    }
    public override void Cancel()
    {
      Log("In ArchivingMachineState requested Cancel operation", Category.Debug, Priority.Low);
      base.Cancel();
    }
    public override void OnException(Exception exception)
    {
      base.OnException(exception);
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, false, true);
    }
    #endregion

    #region private
    private struct BackgroundProcessArgument
    {
      internal string URL;
    }
    private void OnSetupButton()
    {
      Log("User requested navigation to setup dialog screen.", Category.Debug, Priority.Low);
      NavigationParameters _par = new NavigationParameters();
      _par.Add(Infrastructure.ViewNames.SetupStateName, String.Empty);
      Context.RequestNavigate(Infrastructure.ViewNames.SetupStateName, _par);
    }
    private void OnArchiveButton()
    {
      Context.ProgressChang(this, new System.ComponentModel.ProgressChangedEventArgs(1, "Starting archival process - it could take several minutes."));
      RunAsync(new BackgroundProcessArgument() { URL = this.URL });
    }
    private CancelTemplate m_ButtonsTemplate;
    private Action<object>[] m_StateMachineActionsArray;
    #endregion

    #region abstract
    protected abstract string URL { get; }
    //ILoggerFacade
    public abstract void Log(string message, Category category, Priority priority);
    #endregion

  }
}
