//<summary>
//  Title   : class MainViewmodel
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

using CAS.Common.ViewModel;
using  CAS.SmartFactory.Shepherd.Client.Management.InputData;
using  CAS.SmartFactory.Shepherd.Client.Management.UpdateData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace  CAS.SmartFactory.Shepherd.Client.Management
{
  internal class MainViewmodel : ViewModelBackgroundWorker
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewmodel"/> class.
    /// </summary>
    public MainViewmodel()
    {
      RoutesCatalog = null;
      Log = new ObservableCollection<string>();
      URL = Properties.Settings.Default.SiteURL;
      Connected = false;
      Prefix = DateTime.Today.Year.ToString();
      RoutesCatalog = null;
    }
    #endregion

    #region properties
    private string b_URL;
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
    private bool b_Connected;
    public bool Connected
    {
      get
      {
        return b_Connected;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_Connected, "Connected", this);
      }
    }
    private string b_Prefix;
    public string Prefix
    {
      get
      {
        return b_Prefix;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Prefix, "Prefix", this);
      }
    }
    private ObservableCollection<string> b_Log;
    public ObservableCollection<string> Log
    {
      get
      {
        return b_Log;
      }
      set
      {
        RaiseHandler<ObservableCollection<string>>(value, ref b_Log, "Log", this);
      }
    }
    private RoutesCatalog b_Routes;
    public RoutesCatalog RoutesCatalog
    {
      get
      {
        return b_Routes;
      }
      set
      {
        if (value == null)
        {
          this.Commodity = null;
          this.Market = null;
          this.Partners = null;
          this.Route = null;
        }
        else
        {
          this.Commodity = value.CommodityTable != null ? new ObservableCollection<RoutesCatalogCommodityRow>(value.CommodityTable) : null;
          this.Market = value.MarketTable != null ? new ObservableCollection<RoutesCatalogMarket>(value.MarketTable) : null;
          this.Partners = value.PartnersTable != null ? new ObservableCollection<RoutesCatalogPartnersRow>(value.PartnersTable) : null;
          this.Route = value.GlobalPricelist != null ? new ObservableCollection<RoutesCatalogRoute>(value.GlobalPricelist) : null;
        }
        RaiseHandler<RoutesCatalog>(value, ref b_Routes, "RoutesCatalog", this);
      }
    }
    private ObservableCollection<RoutesCatalogCommodityRow> b_Commodity;
    public ObservableCollection<RoutesCatalogCommodityRow> Commodity
    {
      get
      {
        return b_Commodity;
      }
      set
      {
        RaiseHandler<ObservableCollection<RoutesCatalogCommodityRow>>(value, ref b_Commodity, "Commodity", this);
      }
    }
    private ObservableCollection<RoutesCatalogMarket> b_Market;
    public ObservableCollection<RoutesCatalogMarket> Market
    {
      get
      {
        return b_Market;
      }
      set
      {
        RaiseHandler<ObservableCollection<RoutesCatalogMarket>>(value, ref b_Market, "Market", this);
      }
    }
    private ObservableCollection<RoutesCatalogPartnersRow> b_Partners;
    public ObservableCollection<RoutesCatalogPartnersRow> Partners
    {
      get
      {
        return b_Partners;
      }
      set
      {
        RaiseHandler<ObservableCollection<RoutesCatalogPartnersRow>>(value, ref b_Partners, "Partners", this);
      }
    }
    private ObservableCollection<RoutesCatalogRoute> b_Route;
    public ObservableCollection<RoutesCatalogRoute> Route
    {
      get
      {
        return b_Route;
      }
      set
      {
        RaiseHandler<ObservableCollection<RoutesCatalogRoute>>(value, ref b_Route, "Route", this);
      }
    }
    #endregion

    #region API

    #region ReadSiteContent
    internal void ReadSiteContent()
    {
      Uri _uri = default(Uri);
      try
      {
        _uri = new Uri(URL, UriKind.Absolute);
        m_DoWorkEventHandler = DoWorkEventHandler_ReadSiteContent;
        m_ProgressChangedEventHandler = ProgressChangedEventHandler_Logger;
        m_CompletedEventHandler = RunWorkerCompletedEventHandler_ReadSiteContent;
        this.StartBackgroundWorker(_uri);
      }
      catch (Exception _ex)
      {
        ExceptionMessageBox(_ex);
      }
    }
    private Object DoWorkEventHandler_ReadSiteContent(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      string _url = ((Uri)argument).AbsoluteUri;
      progress(new ProgressChangedEventArgs(0, String.Format("Trying to establish connection with the site {0}.", _url)));
      EntitiesDataDictionary _ret = new EntitiesDataDictionary(_url);
      if (_ret == null)
        throw new ArgumentException("DoWorkEventHandler UpdateRoutes", "argument");
      progress(new ProgressChangedEventArgs(0, "Starting read current data from selected site."));
      int _prc = 0;
      _ret.ReadSiteContent(x => progress(new ProgressChangedEventArgs(_prc++, x)));
      progress(new ProgressChangedEventArgs(100, "Finished read current data from selected site."));
      return _ret;
    }
    private void RunWorkerCompletedEventHandler_ReadSiteContent(object sender, RunWorkerCompletedEventArgs e)
    {
      Connected = false;
      DisposeEntitiesDataDictionary();
      if (e.Error != null)
      {
        ExceptionMessageBox(e.Error);
        Log.Add(String.Format("Operation ReadSiteContent terminated by exception {0}.", e.Error.Message));
        return;
      }
      if (e.Cancelled)
        Log.Add("Operation ReadSiteContent canceled by the user");
      else
      {
        m_EntitiesDataDictionary = e.Result as EntitiesDataDictionary;
        Connected = true;
        Log.Add("Operation ReadSiteContent finished");
      }
    }
    #endregion

    #region UpdateRoutes
    internal void UpdateRoutes()
    {
      try
      {
        if (!Connected)
          throw new ApplicationException("Before updating changes you must establish connection.");
        m_DoWorkEventHandler = DoWorkEventHandler_UpdateRoutes;
        m_ProgressChangedEventHandler = ProgressChangedEventHandler_Logger;
        m_CompletedEventHandler = RunWorkerCompletedEventHandler_Logger;
        this.StartBackgroundWorker(m_EntitiesDataDictionary);
      }
      catch (Exception _ex)
      {
        ExceptionMessageBox(_ex);
      }
    }
    private Object DoWorkEventHandler_UpdateRoutes(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      EntitiesDataDictionary _edc = argument as EntitiesDataDictionary;
      if (_edc == null)
        throw new ArgumentException("DoWorkEventHandler UpdateRoutes", "argument");
      progress(new ProgressChangedEventArgs(100, "Start updating the site data."));
      _edc.ImportTable(RoutesCatalog.CommodityTable, progress);
      progress(new ProgressChangedEventArgs(0, "Commodity updated."));
      _edc.ImportTable(RoutesCatalog.PartnersTable, false, progress);
      progress(new ProgressChangedEventArgs(0, "Partners updated."));
      _edc.ImportTable(RoutesCatalog.MarketTable, progress);
      progress(new ProgressChangedEventArgs(0, "Market updated."));
      _edc.ImportTable(RoutesCatalog.GlobalPricelist, false, progress);
      progress(new ProgressChangedEventArgs(0, "GlobalPricelist updated."));
      progress(new ProgressChangedEventArgs(0, "Data from current site has been read"));
      _edc.SubmitChages();
      progress(new ProgressChangedEventArgs(0, "Submitted changes."));
      return null;
    }
    #endregion

    #region ReadXMLFile
    internal void ReadXMLFile(string path)
    {
      m_DoWorkEventHandler = DoWorkEventHandler_ReadXMLFile;
      m_ProgressChangedEventHandler = ProgressChangedEventHandler_Logger;
      m_CompletedEventHandler = RunWorkerCompletedEventHandler_ReadXMLFile;
      this.StartBackgroundWorker(path);
    }
    private Object DoWorkEventHandler_ReadXMLFile(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "Starting read data from XML file"));
      string path = argument as string;
      if (String.IsNullOrEmpty(path))
        throw new ArgumentException("DoWorkEventHandler ReadXMLFile", "argument");
      RoutesCatalog _catalog = CAS.Common.DocumentsFactory.XmlFile.ReadXmlFile<RoutesCatalog>(path);
      progress(new ProgressChangedEventArgs(0, "Read data from XML file finished"));
      return _catalog;
    }
    private void RunWorkerCompletedEventHandler_ReadXMLFile(object sender, RunWorkerCompletedEventArgs e)
    {
      this.RoutesCatalog = null;
      if (e.Error != null)
        ExceptionMessageBox(e.Error);
      if (e.Cancelled)
        Log.Add("Operation canceled by the user");
      else
      {
        RoutesCatalog = e.Result as RoutesCatalog;
        Log.Add("Operation ReadXMLFile finished");
      }
    }
    #endregion

    #endregion

    #region ViewModelBackgroundWorker implementation
    private ViewModelBackgroundWorker.DoWorkEventHandler m_DoWorkEventHandler = null;
    private System.ComponentModel.RunWorkerCompletedEventHandler m_CompletedEventHandler = null;
    private System.ComponentModel.ProgressChangedEventHandler m_ProgressChangedEventHandler = null;
    protected override ViewModelBackgroundWorker.DoWorkEventHandler GetDoWorkEventHandler
    {
      get { return m_DoWorkEventHandler; }
    }
    protected override System.ComponentModel.RunWorkerCompletedEventHandler CompletedEventHandler
    {
      get { return m_CompletedEventHandler; }
    }
    protected override System.ComponentModel.ProgressChangedEventHandler ProgressChangedEventHandler
    {
      get { return m_ProgressChangedEventHandler; }
    }
    /// <summary>
    /// Called when the NotBusy has been changed.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void OnNotBusyChanged()
    {
      //http://casas:11227/sites/awt/Lists/TaskList/_cts/Tasks/displayifs.aspx?List=72c511b5%2D8b63%2D4dfa%2Dad34%2D133a97eba469&ID=4430&ContentTypeId=0x01005D39260836CE498D8E0D443AD5CAD3AC00456AB372ACF9DA41B8AE870CD1954927
      throw new NotImplementedException();
    }
    #endregion

    #region Dispose
    protected override void Dispose(bool disposing)
    {
      Properties.Settings.Default.SiteURL = URL;
      Properties.Settings.Default.Save();
      if (disposing)
        DisposeEntitiesDataDictionary();
      base.Dispose(disposing);
    }
    #endregion

    #region private
    private EntitiesDataDictionary m_EntitiesDataDictionary = null;
    private void ExceptionMessageBox(Exception ex)
    {
      MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
      Log.Add(String.Format("Caught Exception {0}.", ex.Message));
    }
    private void ProgressChangedEventHandler_Logger(object sender, ProgressChangedEventArgs e)
    {
      Log.Add(String.Format("Position: {0:d3}; {1}", e.ProgressPercentage, e.UserState as string));
    }
    private void RunWorkerCompletedEventHandler_Logger(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        ExceptionMessageBox(e.Error);
      if (e.Cancelled)
        Log.Add("Operation canceled by the user");
      else
        Log.Add("Operation finished");
    }
    private void DisposeEntitiesDataDictionary()
    {
      if (m_EntitiesDataDictionary == null)
        return;
      m_EntitiesDataDictionary.Dispose();
      m_EntitiesDataDictionary = null;
    }
    #endregion
  }
}
