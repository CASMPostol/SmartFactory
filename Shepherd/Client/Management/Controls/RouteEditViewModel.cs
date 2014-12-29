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
using CAS.SmartFactory.Shepherd.Client.Management.InputData;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Win32;
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
    public RouteEditViewModel(ILoggerFacade loggingService)
    {
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
          this.Commodity = value.CommodityTable != null ? new ObservableCollection<RoutesCatalogCommodityRow>(value.CommodityTable) : null;
          m_LoggingService.Log(String.Format("Entered {0} items to Commodity table", this.Commodity == null ? 0 : this.Commodity.Count), Category.Info, Priority.Medium);
          this.Market = value.MarketTable != null ? new ObservableCollection<RoutesCatalogMarket>(value.MarketTable) : null;
          m_LoggingService.Log(String.Format("Entered {0} items to Market table", this.Market == null ? 0 : this.Market.Count), Category.Info, Priority.Medium);
          this.Partners = value.PartnersTable != null ? new ObservableCollection<RoutesCatalogPartnersRow>(value.PartnersTable) : null;
          m_LoggingService.Log(String.Format("Entered {0} items to Partners table", this.Partners == null ? 0 : this.Partners.Count), Category.Info, Priority.Medium);
          this.Route = value.GlobalPricelist != null ? new ObservableCollection<RoutesCatalogRoute>(value.GlobalPricelist) : null;
          m_LoggingService.Log(String.Format("Entered {0} items to Route table", this.Route == null ? 0 : this.Route.Count), Category.Info, Priority.Medium);
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
    public class RouteEditMachineStateLocal : RouteEditMachineState<RouteEditViewModel>
    {
      protected override bool GetReadSiteContentConfirmation()
      {
        bool _confirmed = false;
        this.ViewModelContext.ReadSiteContentConfirmation.Raise(
          new Confirmation() { Title = "Read Site Content.", Content = "You are about to read the website content. All changes will be lost.. Are you sure?", Confirmed = true },
          c => _confirmed = c.Confirmed);
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
      protected override RoutesCatalog SetRoutesCatalog
      {
        set { ViewModelContext.RoutesCatalog = value; }
        get { return ViewModelContext.RoutesCatalog; }
      }
      protected override string URL
      {
        get
        {
          Debug.Assert(ViewModelContext.m_ConnectionData != null, "There is no connection data available.");
          return ViewModelContext.m_ConnectionData.SharePointServerURL;
        }
      }
    }
    #endregion

    #region private
    private string b_Prefix;
    private RoutesCatalog b_Routes;
    private ObservableCollection<RoutesCatalogCommodityRow> b_Commodity;
    private ObservableCollection<RoutesCatalogMarket> b_Market;
    private ObservableCollection<RoutesCatalogPartnersRow> b_Partners;
    private ObservableCollection<RoutesCatalogRoute> b_Route;
    private Services.ConnectionDescription m_ConnectionData;
    private ILoggerFacade m_LoggingService;
    #endregion


    #region INavigationAware
    /// <summary>
    /// Called when the implementer has been navigated to.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      m_ConnectionData = (Services.ConnectionDescription)navigationContext.Parameters[Infrastructure.ViewNames.RouteEditorStateName];
      string _msg = String.Format
        ("Created view model {0} for SharePoint: {1}.", typeof(RouteEditViewModel).Name, m_ConnectionData.SharePointServerURL);
      m_LoggingService.Log(_msg, Category.Debug, Priority.Low);
    }
    /// <summary>
    /// Called to determine if this instance can handle the navigation request.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    /// <returns><see langword="true" /> if this instance accepts the navigation request; otherwise, <see langword="false" />.</returns>
    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return false;
    }
    /// <summary>
    /// Called when the implementer is being navigated away from.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    public void OnNavigatedFrom(NavigationContext navigationContext)
    { }
    #endregion

  }
}
