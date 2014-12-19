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
using CAS.Common.ViewModel;
using CAS.Common.ViewModel.Wizard;
using CAS.SmartFactory.Shepherd.Client.Management.InputData;
using CAS.SmartFactory.Shepherd.Client.Management.UpdateData;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Forms;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class RouteEditViewModel - the ViewModel for RouteEdit
  /// </summary>
  [Export]
  public sealed class RouteEditViewModel : ViewModelBase<RouteEditViewModel.RouteEditMachineStateLocal>
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="RouteEditViewModel"/> class.
    /// </summary>
    public RouteEditViewModel()
    {
      Connected = false;
      Prefix = DateTime.Today.Year.ToString();
      RoutesCatalog = null;
      ReadSiteContentConfirmation = new InteractionRequest<IConfirmation>();
      ReadRouteFileNameConfirmation = new InteractionRequest<IConfirmation>();
    }
    /// <summary>
    /// Class RouteEditMachineStateLocal - ViewModel extension of the machine state <see cref="RouteEditMachineState"/>.
    /// </summary>
    public class RouteEditMachineStateLocal : StateMachines.RouteEditMachineState<RouteEditViewModel>
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
        this.ViewModelContext.ReadSiteContentConfirmation.Raise(
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
        get { throw new NotImplementedException(); }
      }
    }
    #endregion

    #region properties
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
    /// <summary>
    /// Gets the read site content confirmation.
    /// </summary>
    /// <value>The read site content confirmation.</value>
    public InteractionRequest<IConfirmation> ReadSiteContentConfirmation { get; private set; }
    public InteractionRequest<IConfirmation> ReadRouteFileNameConfirmation { get; private set; }
    #endregion

  }
}
