//<summary>
//  Title   : class EntitiesDataDictionary
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

using CAS.SmartFactory.Shepherd.Client.DataManagement.InputData;
using CAS.SmartFactory.Shepherd.Client.DataManagement.Linq;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class UpdateRotes - <see cref="UpdateRotes.DoUpdate"/> it is a service updating routs using already imported data.
  /// </summary>
  public static class UpdateRotes
  {

    #region public
    /// <summary>
    /// Does the update.
    /// </summary>
    /// <param name="URL">The URL of the SharePoint website.</param>
    /// <param name="routes">The routes catalog.</param>
    /// <param name="routePrefix">The route title prefix.</param>
    /// <param name="reportProgress">The report progress.</param>
    /// <param name="trace">The trace is used to write and log messages to an external trace.</param>
    public static void DoUpdate(string URL, RoutesCatalog routes, string routePrefix, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Establishing connection with the site {0}.", URL)));
      using (Linq.Entities _edc = new Linq.Entities(trace, URL))
      {
        Dictionaries _dictionary = new Dictionaries();
        reportProgress(new ProgressChangedEventArgs(1, "Starting read current data from the selected site."));
        _dictionary.ReadSiteContent(_edc, x => reportProgress(new ProgressChangedEventArgs(1, x)));
        reportProgress(new ProgressChangedEventArgs(1, "Start updating the site data."));
        ImportTable(_edc, routes.CommodityTable, _dictionary, x => reportProgress(x));
        ImportTable(_edc, routes.PartnersTable, _dictionary, false, x => reportProgress(x));
        ImportTable(_edc, routes.MarketTable, _dictionary, x => reportProgress(x));
        reportProgress(new ProgressChangedEventArgs(1, "Market updated."));
        ImportTable(_edc, routes.GlobalPricelist, routePrefix, _dictionary, false, x => reportProgress(x));
        reportProgress(new ProgressChangedEventArgs(1, "Global Price List updated."));
        reportProgress(new ProgressChangedEventArgs(1, "Data from current site has been read"));
        //TODO _edc.SubmitChanges();
        reportProgress(new ProgressChangedEventArgs(1, "Submitted changes."));
      }
    }
    #endregion

    #region private

    #region services
    private static void ImportTable(Entities edc, RoutesCatalogCommodityRow[] routesCatalogCommodityRow, Dictionaries dictionaries, Action<ProgressChangedEventArgs> progress)
    {
      progress(new ProgressChangedEventArgs(1, "ImportTable: Importing the RoutesCatalogCommodityRow table."));
      if (routesCatalogCommodityRow == null)
      {
        progress(new ProgressChangedEventArgs(1, "Finished the import because the parameter is empty"));
        return;
      }
      int _poz = 0;
      int _newCounter = 0;
      foreach (RoutesCatalogCommodityRow _CommodityRow in routesCatalogCommodityRow)
      {
        try
        {
          _poz++;
          GetOrAdd<Commodity>(edc.Commodity, dictionaries.m_CommodityCommodity, _CommodityRow.Title, false, x => _newCounter++);
        }
        catch (Exception _ex)
        {
          string _format = "Cannot add RoutesCatalogCommodityRow data Name={0} at position {1} because of import Error= {2}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(_poz, String.Format(_format, _CommodityRow.Title, _poz, _ex.Message)));
        }
        progress(new ProgressChangedEventArgs(1, null));
      }
      string _msg = String.Format("Importing finished, the {0} items have been reviewed, and {1} added.", _poz, _newCounter);
      progress(new ProgressChangedEventArgs(1, _msg));
    }
    private static void ImportTable(Entities edc, RoutesCatalogPartnersRow[] routesCatalogPartnersRow, Dictionaries dictionaries, bool _testData, Action<ProgressChangedEventArgs> progress)
    {
      progress(new ProgressChangedEventArgs(1, "ImportTable: Importing the RoutesCatalogPartnersRow table."));
      if (routesCatalogPartnersRow == null)
      {
        progress(new ProgressChangedEventArgs(1, "Finished the import because the parameter is empty"));
        return;
      }
      int _poz = 0;
      int _newCounter = 0;
      string _template = "New warechouse: {0} created for thye partner: {1}";
      foreach (RoutesCatalogPartnersRow _partner in routesCatalogPartnersRow)
      {
        try
        {
          _poz++;
          if (_partner.Name.IsNullOrEmpty())
            throw new ArgumentNullException("RoutesCatalogPartnersRow.Name", String.Format("Cannot add empty key to the dictionary {0}", typeof(Partner).Name));
          if (dictionaries.m_Partner.ContainsKey(_partner.Name))
            continue;
          Partner _newPartner = Create<Partner>(edc.Partner, dictionaries.m_Partner, _partner.Name, _testData);
          _newPartner.EmailAddress = DummyEmail(_partner.E_Mail, "AdresEmail", _testData);
          _newPartner.CellPhone = DummyName(_partner.Mobile, "Mobile", _testData);
          _newPartner.ServiceType = ParseServiceType(_partner.ServiceType);
          _newPartner.WorkPhone = DummyName(_partner.BusinessPhone, "BusinessPhone", _testData);
          _newPartner.VendorNumber = DummyName(_partner.NumberFromSAP, "NumberFromSAP", _testData);
          _newPartner.Partner2WarehouseTitle = GetOrAdd<Warehouse>(edc.Warehouse, dictionaries.m_Warehouse, _partner.Warehouse, false, x => progress(new ProgressChangedEventArgs(1, String.Format(_template, x.Title, _partner.Name))));
          _newCounter++;
        }
        catch (Exception _ex)
        {
          string _format = "Cannot add RoutesCatalogPartnersRow data Name={0} NumberFromSAP={1} at position {2} because of import Error= {3}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(1, String.Format(_format, _partner.Name, _partner.NumberFromSAP, _ex.Message)));
        }
        progress(new ProgressChangedEventArgs(1, null));
      }
      string _msg = String.Format("Importing RoutesCatalogPartnersRow table finished, the {0} items have been reviewed and {1} added.", _poz, _newCounter);
      progress(new ProgressChangedEventArgs(1, _msg));
    }
    private static void ImportTable(Entities m_EDC, RoutesCatalogMarket[] routesCatalogMarket, Dictionaries dic, Action<ProgressChangedEventArgs> progress)
    {
      progress(new ProgressChangedEventArgs(1, "ImportTable: Importing the RoutesCatalogMarket table."));
      if (routesCatalogMarket == null)
      {
        progress(new ProgressChangedEventArgs(1, "Finished the import because the parameter is empty"));
        return;
      }
      int _poz = 0;
      int _newCounter = 0;
      foreach (RoutesCatalogMarket _market in routesCatalogMarket)
      {
        _poz++;
        try
        {
          dic.ImportRow(m_EDC, _market, x => { _newCounter++; progress(x); });
          progress(new ProgressChangedEventArgs(1, null));
        }
        catch (Exception ex)
        {
          string _format = "Cannot add market data DestinationCity={0} Market={1}  at position {2} because of import Error= {3}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(1, String.Format(_format, _market.DestinationCity, _market.Market, _poz, ex.Message)));
        }
      }
        string _msg = String.Format("Importing RoutesCatalogMarket table finished, the {0} items have been reviewed and {1} added.", _poz, _newCounter);
        progress(new ProgressChangedEventArgs(1, _msg));
    }
    private static void ImportTable(Entities edc, RoutesCatalogRoute[] routesCatalogRoute, string routePrefix, Dictionaries dic, bool testData, Action<ProgressChangedEventArgs> progress)
    {
      progress(new ProgressChangedEventArgs(1, String.Format("ImportTable: Importing the RoutesCatalogRoute table with the prefix {0}.", routePrefix)));
      if (routesCatalogRoute == null)
      {
        progress(new ProgressChangedEventArgs(1, "Finished the import because the parameter is empty"));
        return;
      }
      int _poz = 0;
      foreach (RoutesCatalogRoute _route in routesCatalogRoute)
      {
        try
        {
          _poz++;
          dic.ImportRow(edc, _route, routePrefix, testData, progress);
          progress(new ProgressChangedEventArgs(1, null));
        }
        catch (Exception ex)
        {
          string _format = "Cannot add route data SKU={0} Description={1} at position {2} because of import Error= {3}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(1, String.Format(_format, _route.Material_Master_Short_Text, _route.Business_description, _poz, ex.Message)));
        }
      } //foreach (RoutesCatalogRoute 
      string _msg = String.Format("Importing RoutesCatalogRoute table finished, the {0} items have been reviewed and added to the application dictionaries.", _poz);
      progress(new ProgressChangedEventArgs(1, _msg));
    }
    #endregion

    // data management
    private static type Create<type>(EntityList<type> _EDC, Dictionary<string, type> _dictionary, string key, bool _testData)
      where type : Item, new()
    {
      type _elmnt = new type() { Title = _testData ? EmptyKey : key };
      if (_dictionary.Keys.Contains(key))
        key = String.Format("Duplicated name: {0} [{1}]", key, EmptyKey);
      _dictionary.Add(key, _elmnt);
      _EDC.InsertOnSubmit(_elmnt);
      return _elmnt;
    }
    private static type GetOrAdd<type>(EntityList<type> _EDC, Dictionary<string, type> dictionary, string key, bool testData, Action<type> initializeNew)
      where type : Item, new()
    {
      if (key.IsNullOrEmpty())
        throw new ArgumentNullException("key", String.Format("Cannot add empty key to the dictionary {0}", typeof(type).Name));
      if (dictionary.ContainsKey(key))
        return dictionary[key];
      else
      {
        type _newOne = Create<type>(_EDC, dictionary, key, testData);
        initializeNew(_newOne);
        return _newOne;
      }
    }
    // helpers
    private static Direction? ParseDirection(string p)
    {
      p = p.ToUpper();
      if (p.Contains("OUTBOUND"))
        return Direction.Outbound;
      if (p.Contains("INBOUND"))
        return Direction.Inbound;
      if (p.Contains("BOTH"))
        return Direction.BothDirections;
      return null;
    }
    private static ServiceType? ParseServiceType(string p)
    {
      p = p.ToUpper();
      string _vs = "VENDOR";
      string _fs = "FORWARDER";
      string _es = "ESCORT";
      if (p.Contains(_vs))
        if (p.Contains(_fs))
          return ServiceType.VendorAndForwarder;
        else
          return ServiceType.Vendor;
      if (p.Contains(_fs))
        return ServiceType.Forwarder;
      if (p.Contains(_es))
        return ServiceType.SecurityEscortProvider;
      return null;
    }
    private static ServiceType GetService(RoutesCatalogRoute _route)
    {
      if (_route.Equipment_Type__UoM.ToUpper().Contains("ESC"))
        return ServiceType.SecurityEscortProvider;
      else
        return ServiceType.Forwarder;
    }
    private static short m_EmptyKeyIdx = 0;
    private static string EmptyKey { get { return String.Format("EmptyKey{0}", m_EmptyKeyIdx++); } }
    private static string DestinationMarketKey(Market mrkt, CityType city)
    {
      string _dstName = String.Format("{0} in {1}", EntityEmptyKey(city), EntityEmptyKey(mrkt));
      return _dstName;
    }
    private static string EntityEmptyKey(Item entity)
    {
      return entity == null ? EmptyKey : entity.Title;
    }
    private static string DummyName(string _text, string _replacement, bool _testData)
    {
      return _testData ? String.Format("{0} {1}", _replacement, m_EmptyKeyIdx++) : _text;
    }
    private static string DummyEmail(string _text, string _replacement, bool _testData)
    {
      return _testData ? "oferty@cas.eu" : _text;
    }
    #endregion

    #region Dictionaries
    private class Dictionaries
    {
      internal void ReadSiteContent(Entities m_EDC, Action<string> progress)
      {
        progress("Reading Commodity");
        foreach (Commodity _cmdty in m_EDC.Commodity)
          Add<string, Commodity>(m_CommodityCommodity, _cmdty.Title, _cmdty, false);
        progress("Reading Warehouse");
        foreach (Warehouse _wrse in m_EDC.Warehouse)
          Add<string, Warehouse>(m_Warehouse, _wrse.Title, _wrse, false);
        progress("Reading Partner");
        foreach (Partner _prtnr in m_EDC.Partner)
          Add<string, Partner>(m_Partner, _prtnr.Title, _prtnr, false);
        progress("Reading CountryType");
        foreach (CountryType _cntry in m_EDC.Country)
          Add<string, CountryType>(m_CountryClass, _cntry.Title, _cntry, false);
        progress("Reading CityType");
        foreach (CityType _cty in m_EDC.City)
          Add<string, CityType>(m_CityDictionary, _cty.Title, _cty, false);
        progress("Reading FreightPayer");
        foreach (FreightPayer _frpyr in m_EDC.FreightPayer)
          Add<string, FreightPayer>(m_FreightPayer, _frpyr.Title, _frpyr, false);
        progress("Reading Currency");
        foreach (Currency _curr in m_EDC.Currency)
          Add<string, Currency>(m_Currency, _curr.Title, _curr, false);
        progress("Reading BusinessDescription");
        foreach (BusienssDescription _bdsc in m_EDC.BusinessDescription)
          Add<string, BusienssDescription>(m_BusinessDescription, _bdsc.Title, _bdsc, false);
        progress("Reading ShipmentType");
        foreach (ShipmentType _shpmnt in m_EDC.ShipmentType)
          Add<string, ShipmentType>(m_ShipmentType, _shpmnt.Title, _shpmnt, false);
        progress("Reading CarrierType");
        foreach (CarrierType _crr in m_EDC.Carrier)
          Add<string, CarrierType>(m_CarrierCarrierType, _crr.Title, _crr, false);
        progress("Reading TransportUnit");
        foreach (TranspotUnit _tu in m_EDC.TransportUnitType)
          Add<string, TranspotUnit>(m_TransportUnit, _tu.Title, _tu, false);
        progress("Reading SAPDestinationPlant");
        foreach (SAPDestinationPlant _sdp in m_EDC.SAPDestinationPlant)
          Add<string, SAPDestinationPlant>(m_SAPDestinationPlant, _sdp.Title, _sdp, false);
        progress("Reading Market");
        foreach (Market _mrkt in m_EDC.Market)
          Add<string, Market>(m_MarketMarket, _mrkt.Title, _mrkt, false);
        progress("Reading DestinationMarket");
        foreach (DestinationMarket _dstm in m_EDC.DestinationMarket)
        {
          string _key = DestinationMarketKey(_dstm.MarketTitle, _dstm.DestinationMarket2CityTitle);
          if (m_DestinationMarket.ContainsKey(_key))
            continue;
          m_DestinationMarket.Add(_key, _dstm);
        }
      }
      internal void ImportRow(Entities edc, RoutesCatalogMarket market, Action<ProgressChangedEventArgs> progress)
      {
        Market _market = GetOrAdd<Market>(edc.Market, m_MarketMarket, market.Market, false, x => NewItemCreateNotification(x, progress));
        CityType _CityType = GetOrAddCity(edc, market.DestinationCity, market.DestinationCountry, market.Area, progress);
        string _dstName = DestinationMarketKey(_market, _CityType);
        if (m_DestinationMarket.ContainsKey(_dstName))
          return;
        DestinationMarket _DestinationMarket = new DestinationMarket()
        {
          DestinationMarket2CityTitle = _CityType,
          MarketTitle = _market
        };
        m_DestinationMarket.Add(_dstName, _DestinationMarket);
        edc.DestinationMarket.InsertOnSubmit(_DestinationMarket);
        //TODO edc.SubmitChanges();
        string _msg = "New {0} destination market has been created for city {1}, country: {2}.";
        progress(new ProgressChangedEventArgs(1, String.Format(_msg, _market.Title, _CityType.Title, _CityType.CountryTitle.Title)));
      }
      internal void ImportRow(Entities edc, RoutesCatalogRoute route, string routePrefix, bool testData, Action<ProgressChangedEventArgs> progress)
      {
        ServiceType _service = GetService(route);
        Partner _Partner = GetOrAddJTIPartner(edc, _service, route.Vendor.Trim(), testData);
        FreightPayer _freightPayer = GetOrAdd<FreightPayer>(edc.FreightPayer, m_FreightPayer, route.Freight_Payer__I_C__MainLeg.ToString(), testData, x => NewItemCreateNotification(x, progress));
        CityType _CityType = GetOrAddCity(edc, route.Dest_City.Trim(), route.Dest_Country.Trim(), "Unknown", progress);
        Currency _Currency = GetOrAdd<Currency>(edc.Currency, m_Currency, route.Currency, false, x => NewItemCreateNotification(x, progress));
        ShipmentType _ShipmentType = GetOrAdd<ShipmentType>(edc.ShipmentType, m_ShipmentType, route.ShipmentType, false, x => NewItemCreateNotification(x, progress));
        CarrierType _CarrierCarrierType = GetOrAdd<CarrierType>(edc.Carrier, m_CarrierCarrierType, route.Carrier, false, x => NewItemCreateNotification(x, progress));
        TranspotUnit _TransportUnit = GetOrAdd<TranspotUnit>(edc.TransportUnitType, m_TransportUnit, route.Equipment_Type__UoM, false, x => NewItemCreateNotification(x, progress));
        SAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlant>(edc.SAPDestinationPlant, m_SAPDestinationPlant, route.SAP_Dest_Plant, false, x => NewItemCreateNotification(x, progress));
        BusienssDescription _businessDescription = GetOrAdd<BusienssDescription>(edc.BusinessDescription, m_BusinessDescription, route.Business_description, false, x => NewItemCreateNotification(x, progress));
        Commodity _commodity = GetOrAdd<Commodity>(edc.Commodity, m_CommodityCommodity, route.Commodity, false, x => NewItemCreateNotification(x, progress));
        string _sku = route.Material_Master__Reference;
        string _title = String.Format("{3} To: {0}, by: {1}, of: {2}", _CityType.Title, _Partner.Title, route.Commodity, routePrefix);
        switch (_service)
        {
          case ServiceType.Forwarder:
            Route _rt = new Route()
            {
              Route2BusinessDescriptionTitle = _businessDescription,
              CarrierTitle = _CarrierCarrierType,
              Route2CityTitle = _CityType,
              DepartureCity = route.Dept_City,
              CurrencyTitle = _Currency,
              FreightPayerTitle = _freightPayer,
              GoodsHandlingPO = route.PO_NUMBER,
              MaterialMaster = _sku,
              DeparturePort = route.Port_of_Dept,
              RemarkMM = route.Remarks,
              SAPDestinationPlantTitle = _SAPDestinationPlant,
              ShipmentTypeTitle = _ShipmentType,
              Title = _title,
              TransportCosts = testData ? 4567.8 : route.Total_Cost_per_UoM.String2Double(),
              TransportUnitTypeTitle = _TransportUnit,
              PartnerTitle = _Partner,
              Route2Commodity = _commodity,
              Incoterm = route.Selling_Incoterm
            };
            edc.Route.InsertOnSubmit(_rt);
            break;
          case ServiceType.SecurityEscortProvider:
            SecurityEscortCatalog _sec = new SecurityEscortCatalog()
            {
              SecurityEscortCatalog2BusinessDescriptionTitle = _businessDescription,
              CurrencyTitle = _Currency,
              EscortDestination = route.Dest_City,
              FreightPayerTitle = _freightPayer,
              MaterialMaster = _sku,
              RemarkMM = route.Remarks,
              SecurityCost = testData ? 345.6 : route.Total_Cost_per_UoM.String2Double(),
              SecurityEscrotPO = route.PO_NUMBER,
              Title = _title,
              PartnerTitle = _Partner
            };
            edc.SecurityEscortRoute.InsertOnSubmit(_sec);
            break;
          case ServiceType.VendorAndForwarder:
          case ServiceType.None:
          case ServiceType.Invalid:
          case ServiceType.Vendor:
          default:
            break;
        }
        //TODO edc.SubmitChanges();
      }
      private Partner GetOrAddJTIPartner(Entities edc, ServiceType service, string partner, bool testData)
      {
        if (partner.IsNullOrEmpty())
          throw new ArgumentNullException("partner", String.Format("Cannot add empty key to the partner dictionary for country service {0}.", service));
        if (m_Partner.ContainsKey(partner))
          return m_Partner[partner];
        else
        {
          Partner _prtnr = Create<Partner>(edc.Partner, m_Partner, partner, testData);
          _prtnr.ServiceType = service;
          return _prtnr;
        }
      }
      private CityType GetOrAddCity(Entities edc, string city, string country, string area, Action<ProgressChangedEventArgs> progress)
      {
        if (city.IsNullOrEmpty())
          throw new ArgumentNullException("city", String.Format("Cannot add empty key to the city dictionary for country {0}/area {1}.", country, area));
        CityType _city = default(CityType);
        if (m_CityDictionary.ContainsKey(city))
          _city = m_CityDictionary[city];
        else
          _city = Create<CityType>(edc.City, m_CityDictionary, city, false);
        if (_city.CountryTitle != null)
          return _city;
        if (country.IsNullOrEmpty())
          country = "Country-" + EmptyKey;
        CountryType _countryClass = GetOrAdd(edc.Country, m_CountryClass, country, false, x => NewItemCreateNotification(x, progress));
        if (_countryClass.CountryGroup.IsNullOrEmpty() && !area.IsNullOrEmpty())
          _countryClass.CountryGroup = area;
        _city.CountryTitle = _countryClass;
        return _city;
      }
      //dictionaries
      internal Dictionary<string, Partner> m_Partner = new Dictionary<string, Partner>();
      private Dictionary<string, FreightPayer> m_FreightPayer = new Dictionary<string, FreightPayer>();
      private Dictionary<string, CityType> m_CityDictionary = new Dictionary<string, CityType>();
      private Dictionary<string, CountryType> m_CountryClass = new Dictionary<string, CountryType>();
      private Dictionary<string, Currency> m_Currency = new Dictionary<string, Currency>();
      private Dictionary<string, ShipmentType> m_ShipmentType = new Dictionary<string, ShipmentType>();
      private Dictionary<string, CarrierType> m_CarrierCarrierType = new Dictionary<string, CarrierType>();
      private Dictionary<string, TranspotUnit> m_TransportUnit = new Dictionary<string, TranspotUnit>();
      private Dictionary<string, SAPDestinationPlant> m_SAPDestinationPlant = new Dictionary<string, SAPDestinationPlant>();
      private Dictionary<string, Market> m_MarketMarket = new Dictionary<string, Market>();
      internal Dictionary<string, Warehouse> m_Warehouse = new Dictionary<string, Warehouse>();
      internal Dictionary<string, Commodity> m_CommodityCommodity = new Dictionary<string, Commodity>();
      private Dictionary<string, ShippingPoint> m_ShippingPoint = new Dictionary<string, ShippingPoint>();
      private Dictionary<string, BusienssDescription> m_BusinessDescription = new Dictionary<string, BusienssDescription>();
      private Dictionary<string, DistributionList> m_DistributionList = new Dictionary<string, DistributionList>();
      private Dictionary<string, DestinationMarket> m_DestinationMarket = new Dictionary<string, DestinationMarket>();
      // static methods
      private static void NewItemCreateNotification(Linq.Item x, Action<ProgressChangedEventArgs> progress)
      {
        string _template = "New stub entry for the '{0}' item of title '{1}' has been created and must be manually updated.";
        progress(new ProgressChangedEventArgs(1, String.Format(_template, x.GetType().Name, x.Title)));
      }
      private static void Add<TKey, TEntity>(Dictionary<TKey, TEntity> _dictionary, TKey _key, TEntity entity, bool _testData)
      {
        if (_dictionary.Keys.Contains(_key))
          return;
        _dictionary.Add(_key, entity);
      }

    }
    #endregion

  }
}
