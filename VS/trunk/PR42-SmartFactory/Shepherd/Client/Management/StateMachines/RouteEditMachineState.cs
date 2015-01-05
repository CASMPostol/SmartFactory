//<summary>
//  Title   : RouteEditMachineState
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
using CAS.SmartFactory.Shepherd.Client.DataManagement;
using CAS.SmartFactory.Shepherd.Client.DataManagement.InputData;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  public abstract class RouteEditMachineState<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>, ILoggerFacade
    where ViewModelContextType : IViewModelContext
  {

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="RouteEditMachineState{ViewModelContextType}"/> class.
    /// </summary>
    internal RouteEditMachineState()
    {
      m_ButtonsTemplate = new CancelTemplate(Resources.UpdateRoutesButtonTitle, Resources.ImportXMLButtonTitle, Resources.SetupButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.Cancel();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent] = x => this.OnSetupButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => this.OnUpdateRoutesButton();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => this.OnReadXMLFileButton();
    }
    #endregion

    #region object
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
      return Infrastructure.ViewNames.RouteEditorStateName;
    }
    #endregion

    #region BackgroundWorkerMachine
    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public override void OnEnteringState()
    {
      this.Log(String.Format("OnEnteringState {0}", Infrastructure.ViewNames.RouteEditorStateName), Category.Debug, Priority.Low);
      base.OnEnteringState();
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      m_DoWorkEventHandler(sender, e);
    }
    protected override void RunWorkerCompleted(object result)
    {
      m_CompletedEventHandler(result);
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
      Log("Requested Cancel operation", Category.Debug, Priority.Low);
      base.Cancel();
    }
    #endregion

    #region private

    #region UpdateRoutes
    private void OnUpdateRoutesButton()
    {
      Log("Entering UpdateRoutes", Category.Debug, Priority.Low);
      if (!this.GetReadSiteContentConfirmation())
      {
        Log("Leaving UpdateRoutes - no confirmation.", Category.Debug, Priority.Low);
        return;
      }
      m_DoWorkEventHandler = DoWorkEventHandler_UpdateRoutes;
      m_CompletedEventHandler = RunWorkerCompletedEventHandler_UpdateRoutes;
      this.RunAsync(new UpdateRoutesArgument() { RoutesCatalog = this.RoutesCatalog, URL = this.URL, RoutePrefix = this.RoutePrefix });
    }
    private void DoWorkEventHandler_UpdateRoutes(object argument, DoWorkEventArgs e)
    {
      ReportProgress(this, new ProgressChangedEventArgs(1, "Starting UpdateRoutes background worker"));
      UpdateRoutesArgument _argument = (UpdateRoutesArgument)e.Argument;
      if (string.IsNullOrEmpty(_argument.URL))
        throw new ArgumentException("ReadSiteContent: URL cannot be empty or null");
      UpdateRotes.DoUpdate(_argument.URL, _argument.RoutesCatalog, _argument.RoutePrefix, x => ReportProgress(this, x));
    }
    private void RunWorkerCompletedEventHandler_UpdateRoutes(object result)
    {
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, true);
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "Operation UpdateRoutes finished"));
    }
    #endregion

    #region ReadXMLFile
    private void OnReadXMLFileButton()
    {
      string path = GetReadRouteFileNameConfirmation();
      if (String.IsNullOrEmpty(path))
        return;
      Context.ProgressChang(this, new ProgressChangedEventArgs(1, String.Format("Start reading the file containing routes {0}", path)));
      m_DoWorkEventHandler = DoWorkEventHandler_ReadXMLFile;
      m_CompletedEventHandler = RunWorkerCompletedEventHandler_ReadXMLFile;
      this.RunAsync(path);
    }
    private void DoWorkEventHandler_ReadXMLFile(object sender, DoWorkEventArgs e)
    {
      ReportProgress(this, new ProgressChangedEventArgs(0, "Starting read data from XML file"));
      string path = e.Argument as string;
      if (String.IsNullOrEmpty(path))
        throw new ArgumentException("DoWorkEventHandler ReadXMLFile", "argument");
      RoutesCatalog _catalog = CAS.Common.DocumentsFactory.XmlFile.ReadXmlFile<RoutesCatalog>(path);
      ReportProgress(this, new ProgressChangedEventArgs(0, "Read data from XML file finished"));
      e.Result = _catalog;
    }
    private void RunWorkerCompletedEventHandler_ReadXMLFile(object result)
    {
      this.RoutesCatalog = (RoutesCatalog)result;
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(true, true, true);
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, "Operation ReadXMLFile finished"));
    }
    #endregion

    private struct UpdateRoutesArgument
    {
      internal string URL;
      internal string RoutePrefix;
      internal RoutesCatalog RoutesCatalog;
    }
    private DoWorkEventHandler m_DoWorkEventHandler = null;
    private Action<object> m_CompletedEventHandler = null;
    private CancelTemplate m_ButtonsTemplate = null;
    private Action<object>[] m_StateMachineActionsArray;
    private void OnSetupButton()
    {
      Log("User requested navigation to setup dialog screen.", Category.Debug, Priority.Low);
      NavigationParameters _par = new NavigationParameters();
      _par.Add(Infrastructure.ViewNames.SetupStateName, String.Empty);
      Context.RequestNavigate(Infrastructure.ViewNames.SetupStateName, _par);
    }
    #endregion

    #region abstract
    protected abstract bool GetReadSiteContentConfirmation();
    protected abstract string GetReadRouteFileNameConfirmation();
    protected abstract RoutesCatalog RoutesCatalog { set; get; }
    protected abstract string RoutePrefix { get; }
    protected abstract string URL { get; }
    /// <summary>
    /// Write a new log entry with the specified category and priority.
    /// </summary>
    /// <param name="message">Message body to log.</param>
    /// <param name="category">Category of the entry.</param>
    /// <param name="priority">The priority of the entry.</param>
    public abstract void Log(string message, Category category, Priority priority);


    #endregion
  }
}
