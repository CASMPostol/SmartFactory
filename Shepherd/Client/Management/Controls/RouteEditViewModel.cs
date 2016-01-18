//<summary>
//  Title   : class RouteEditViewModel
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

using CAS.Common.Interactivity.InteractionRequest;
using CAS.SmartFactory.Shepherd.Client.DataManagement.InputData;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Win32;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{

  /// <summary>
  /// Class RouteEditViewModel - the ViewModel for RouteEdit
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public sealed class RouteEditViewModel : ViewModelStateMachineBase<RouteEditViewModel.RouteEditMachineStateLocal>, INavigationAware
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="RouteEditViewModel"/> class.
    /// </summary>
    [ImportingConstructor]
    public RouteEditViewModel(IEventAggregator eventAggregator, ILoggerFacade loggingService)
      : base(loggingService)
    {
      loggingService.Log("Created RouteEditViewModel", Category.Debug, Priority.Low);
      m_EventAggregator = eventAggregator;
      m_LoggingService = loggingService;
      Prefix = DateTime.Today.Year.ToString();
      RoutesCatalog = null;
      ReadSiteContentConfirmation = new InteractionRequest<IConfirmation>();
      ReadRouteFileNameConfirmation = new InteractionRequest<IConfirmation>();
    }
    #endregion

    #region public UI
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
          ProgressChangeEvent _agr = m_EventAggregator.GetEvent<ProgressChangeEvent>();
          this.Commodity = value.CommodityTable != null ? new ObservableCollection<RoutesCatalogCommodityRow>(value.CommodityTable) : null;
          _agr.Publish(new System.ComponentModel.ProgressChangedEventArgs(1, String.Format("Entered {0} items to Commodity table", this.Commodity == null ? 0 : this.Commodity.Count)));
          this.Market = value.MarketTable != null ? new ObservableCollection<RoutesCatalogMarket>(value.MarketTable) : null;
          _agr.Publish(new System.ComponentModel.ProgressChangedEventArgs(1, String.Format("Entered {0} items to Market table", this.Market == null ? 0 : this.Market.Count)));
          this.Partners = value.PartnersTable != null ? new ObservableCollection<RoutesCatalogPartnersRow>(value.PartnersTable) : null;
          _agr.Publish(new System.ComponentModel.ProgressChangedEventArgs(1, String.Format("Entered {0} items to Partners table", this.Partners == null ? 0 : this.Partners.Count)));
          this.Route = value.GlobalPricelist != null ? new ObservableCollection<RoutesCatalogRoute>(value.GlobalPricelist) : null;
          _agr.Publish(new System.ComponentModel.ProgressChangedEventArgs(1, String.Format("Entered {0} items to Route table", this.Route == null ? 0 : this.Route.Count)));
        }
        RaiseHandler<RoutesCatalog>(value, ref b_Routes, "RoutesCatalog", this);
      }
    }
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
    /// <summary>
    /// Gets the read site content confirmation.
    /// </summary>
    /// <value>The read site content confirmation.</value>
    public InteractionRequest<IConfirmation> ReadSiteContentConfirmation { get; private set; }
    public InteractionRequest<IConfirmation> ReadRouteFileNameConfirmation { get; private set; }
    #endregion

    #region RouteEditMachineState
    /// <summary>
    /// Class RouteEditMachineStateLocal - ViewModel extension of the machine state <see cref="RouteEditMachineState"/>.
    /// </summary>
    public sealed class RouteEditMachineStateLocal : RouteEditMachineState<RouteEditViewModel>
    {
      #region RouteEditMachineState<RouteEditViewModel>
      protected override bool GetReadSiteContentConfirmation()
      {
        bool _confirmed = false;
        this.ViewModelContext.ReadSiteContentConfirmation.Raise(
          new Confirmation() { Title = "Import routes.", Content = "You are about to update the website content. Are you sure ?", Confirmed = true }, c => _confirmed = c.Confirmed);
        return _confirmed;
      }
      protected override string GetReadRouteFileNameConfirmation()
      {
        OpenFileDialog _ofd = new OpenFileDialog()
        {
          Title = "Read Routes",
          CheckFileExists = true,
          CheckPathExists = true,
          Filter = "XML Documents|*.XML|XML Documents|*.xml|All files |*.*",
          DefaultExt = ".xml",
          AddExtension = true,
        };
        bool _confirmed = false;
        this.ViewModelContext.ReadRouteFileNameConfirmation.Raise(
          new Confirmation() { Title = "Read Routes.", Content = _ofd, Confirmed = true },
          c => _confirmed = c.Confirmed);
        return _confirmed ? _ofd.FileName : String.Empty;
      }
      protected override RoutesCatalog RoutesCatalog
      {
        set { ViewModelContext.RoutesCatalog = value; }
        get { return ViewModelContext.RoutesCatalog; }
      }
      protected override string URL
      {
        get
        {
          Debug.Assert(ViewModelContext.m_ConnectionData != null, "There is no connection data available.");
          return ViewModelContext.m_ConnectionData.SharePointWebsiteURL;
        }
      }
      protected override string RoutePrefix
      {
        get { return ViewModelContext.Prefix; }
      }
      #endregion

      #region ILoggerFacade
      /// <summary>
      /// Write a new log entry with the specified category and priority.
      /// </summary>
      /// <param name="message">Message body to log.</param>
      /// <param name="category">Category of the entry.</param>
      /// <param name="priority">The priority of the entry.</param>
      public override void Log(string message, Category category, Priority priority)
      {
        ViewModelContext.m_LoggingService.Log(message, category, priority);
      }
      #endregion


    }
    /// <summary>
    /// Called when the implementer has been navigated to.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
      base.OnNavigatedTo(navigationContext);
      m_ConnectionData = (ISPContentState)navigationContext.Parameters[typeof(ISPContentState).Name];
      Debug.Assert(m_ConnectionData != null, "{} parameter cannot be null while navigating to RouteEditViewModel.");
      Debug.Assert(m_ConnectionData.SPConnected == true, "SP has to be connected before navigating to RouteEditViewModel.");
      string _msg = String.Format
        ("OnNavigatedTo - created view model {0} for SharePoint: {1}.", typeof(RouteEditViewModel).Name, m_ConnectionData.SharePointWebsiteURL);
      m_LoggingService.Log(_msg, Category.Debug, Priority.Low);
    }
    #endregion

    #region private
    private string b_Prefix;
    private RoutesCatalog b_Routes;
    private ObservableCollection<RoutesCatalogCommodityRow> b_Commodity;
    private ObservableCollection<RoutesCatalogMarket> b_Market;
    private ObservableCollection<RoutesCatalogPartnersRow> b_Partners;
    private ObservableCollection<RoutesCatalogRoute> b_Route;
    private ISPContentState m_ConnectionData;
    private ILoggerFacade m_LoggingService;
    private IEventAggregator m_EventAggregator;
    #endregion

  }

}
