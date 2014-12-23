//<summary>
//  Title   : RouteEditMachineState
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using CAS.SmartFactory.Shepherd.Client.Management.InputData;
using CAS.SmartFactory.Shepherd.Client.Management.Properties;
using CAS.SmartFactory.Shepherd.Client.Management.UpdateData;
using System;
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  public abstract class RouteEditMachineState<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>
    where ViewModelContextType : IViewModelContext
  {
    
    public RouteEditMachineState()
    {
      m_ButtonsTemplate = new CancelTemplate(Resources.UpdateRoutesButtonTitle, Resources.ImportXMLButtonTitle, Resources.ReadSPContentButtonTitle);
      m_StateMachineActionsArray = new Action<object>[4];
      m_StateMachineActionsArray[(int)m_ButtonsTemplate.CancelPosition] = x => this.OnCancellation();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent] = x => this.ReadSiteContent();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => this.UpdateRoutes();
      m_StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => this.ReadXMLFile();
    }

    #region BackgroundWorkerMachine
    private DoWorkEventHandler m_DoWorkEventHandler = null;
    private Action<object> m_CompletedEventHandler = null;
    public override void OnEnteringState()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, String.Format("On entering the state {0}", Infrastructure.ViewNames.RouteEditorStateName)));
      base.OnEnteringState();
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, false, true);
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
    #endregion

    #region API
    public override string ToString()
    {
      return Infrastructure.ViewNames.RouteEditorStateName;
    }
    #endregion

    #region private
    #region ReadSiteContent
    private void ReadSiteContent()
    {
      if (!this.GetReadSiteContentConfirmation())
        return;
      try
      {
        m_DoWorkEventHandler = DoWorkEventHandler_ReadSiteContent;
        m_CompletedEventHandler = RunWorkerCompletedEventHandler_ReadSiteContent;
        this.RunAsync(URL);
      }
      catch (Exception _ex)
      {
        this.Context.Exception(_ex);
      }
    }
    private void DoWorkEventHandler_ReadSiteContent(object sender, DoWorkEventArgs e)
    {
      ReportProgress(this, new ProgressChangedEventArgs(0, String.Format("Trying to establish connection with the site {0}.", URL)));
      string _url = (string)e.Argument;
      if (string.IsNullOrEmpty(_url))
        throw new ArgumentException("URL cannot be empty or null");
      EntitiesDataDictionary _ret = new EntitiesDataDictionary((string)e.Argument);
      ReportProgress(this, new ProgressChangedEventArgs(0, "Starting read current data from the selected site."));
      _ret.ReadSiteContent(x => ReportProgress(this, new ProgressChangedEventArgs(1, x)));
      ReportProgress(this, new ProgressChangedEventArgs(100, "Finished read current data from selected site."));
      e.Result = _ret;
    }
    private void RunWorkerCompletedEventHandler_ReadSiteContent(object result)
    {
      Connected = false;
      DisposeEntitiesDataDictionary();
      m_EntitiesDataDictionary = result as EntitiesDataDictionary;
      Connected = true;
      this.Context.ProgressChang(this, new ProgressChangedEventArgs(1, "Operation ReadSiteContent finished"));
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(false, true, false);
    }
    #endregion

    #region UpdateRoutes
    private void UpdateRoutes()
    {
      if (!Connected)
        throw new ApplicationException("Before updating changes you must establish connection.");
      m_DoWorkEventHandler = DoWorkEventHandler_UpdateRoutes;
      m_CompletedEventHandler = RunWorkerCompletedEventHandler_UpdateRoutes;
      this.RunAsync(m_EntitiesDataDictionary);
    }
    private void DoWorkEventHandler_UpdateRoutes(object argument, DoWorkEventArgs e)
    {
      EntitiesDataDictionary _edc = e.Argument as EntitiesDataDictionary;
      if (_edc == null)
        throw new ArgumentException("DoWorkEventHandler UpdateRoutes", "argument");
      ReportProgress(this, new ProgressChangedEventArgs(100, "Start updating the site data."));
      _edc.ImportTable(this.SetRoutesCatalog.CommodityTable, x => ReportProgress(this, x));
      ReportProgress(this, new ProgressChangedEventArgs(0, "Commodity updated."));
      _edc.ImportTable(this.SetRoutesCatalog.PartnersTable, false, x => ReportProgress(this, x));
      ReportProgress(this, new ProgressChangedEventArgs(0, "Partners updated."));
      _edc.ImportTable(this.SetRoutesCatalog.MarketTable, x => ReportProgress(this, x));
      ReportProgress(this, new ProgressChangedEventArgs(0, "Market updated."));
      _edc.ImportTable(this.SetRoutesCatalog.GlobalPricelist, false, x => ReportProgress(this, x));
      ReportProgress(this, new ProgressChangedEventArgs(0, "Global Price List updated."));
      ReportProgress(this, new ProgressChangedEventArgs(0, "Data from current site has been read"));
      _edc.SubmitChages();
      ReportProgress(this, new ProgressChangedEventArgs(0, "Submitted changes."));
    }
    private void RunWorkerCompletedEventHandler_UpdateRoutes(object result)
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(100, "Operation UpdateRoutes finished"));
      Context.EnabledEvents = m_ButtonsTemplate.SetEventsMask(true, false, false);
    }
    #endregion

    #region ReadXMLFile
    private void ReadXMLFile()
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
      this.SetRoutesCatalog = (RoutesCatalog)result;
      Context.ProgressChang(this, new ProgressChangedEventArgs(100, "Operation ReadXMLFile finished"));
    }
    #endregion


    private EntitiesDataDictionary m_EntitiesDataDictionary = null;
    private bool Connected;
    private CancelTemplate m_ButtonsTemplate;
    private Action<object>[] m_StateMachineActionsArray;
    private void DisposeEntitiesDataDictionary()
    {
      if (m_EntitiesDataDictionary == null)
        return;
      m_EntitiesDataDictionary.Dispose();
      m_EntitiesDataDictionary = null;
    }
    #endregion

    #region abstract
    protected abstract bool GetReadSiteContentConfirmation();
    protected abstract string GetReadRouteFileNameConfirmation();
    protected abstract RoutesCatalog SetRoutesCatalog { set; get; }
    protected abstract string URL { get; }
    #endregion

  }
}
