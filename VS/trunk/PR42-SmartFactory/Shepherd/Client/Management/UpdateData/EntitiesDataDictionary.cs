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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using  CAS.SmartFactory.Shepherd.Client.Management.InputData;
using Microsoft.SharePoint.Linq;

namespace  CAS.SmartFactory.Shepherd.Client.Management.UpdateData
{
  internal class EntitiesDataDictionary : IDisposable
  {

    #region internal
    internal EntitiesDataDictionary(string _url)
    {
      m_EDC = new EntitiesDataContext(_url);
    }
    internal void ImportTable(RoutesCatalogCommodityRow[] routesCatalogCommodityRow, Action<ProgressChangedEventArgs> progress)
    {
      if (routesCatalogCommodityRow == null)
        return;
      int _poz = 0;
      foreach (RoutesCatalogCommodityRow _CommodityRow in routesCatalogCommodityRow)
        try
        {
          _poz++;
          GetOrAdd<Commodity>(m_EDC.Commodity, m_CommodityCommodity, _CommodityRow.Title, false);
        }
        catch (Exception _ex)
        {
          string _format = "Cannot add RoutesCatalogCommodityRow data Name={0} because of import Error= {1}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(_poz, String.Format(_format, _CommodityRow.Title, _ex.Message)));
        }
    }
    internal void ImportTable(RoutesCatalogPartnersRow[] routesCatalogPartnersRow, bool _testData, Action<ProgressChangedEventArgs> progress)
    {
      if (routesCatalogPartnersRow == null)
        return;
      int _poz = 0;
      foreach (RoutesCatalogPartnersRow _partner in routesCatalogPartnersRow)
      {
        try
        {
          _poz++;
          if (_partner.Name.IsNullOrEmpty())
            throw new ArgumentNullException("RoutesCatalogPartnersRow.Name", String.Format("Cannot add empty key to the dictionary {0}", typeof(Partner).Name));
          if (m_Partner.ContainsKey(_partner.Name))
            return;
          Partner _prtnr = Create<Partner>(m_EDC.Partner, m_Partner, _partner.Name, _testData);
          _prtnr.EmailAddress = DummyEmail(_partner.E_Mail, "AdresEMail", _testData);
          _prtnr.CellPhone = DummyName(_partner.Mobile, "Mobile", _testData);
          _prtnr.ServiceType = ParseServiceType(_partner.ServiceType);
          _prtnr.WorkPhone = DummyName(_partner.BusinessPhone, "BusinessPhone", _testData);
          _prtnr.VendorNumber = DummyName(_partner.NumberFromSAP, "NumberFromSAP", _testData);
          _prtnr.Partner2WarehouseTitle = GetOrAdd<Warehouse>(m_EDC.Warehouse, m_Warehouse, _partner.Warehouse, false);
        }
        catch (Exception _ex)
        {
          string _format = "Cannot add RoutesCatalogPartnersRow data Name={0} NumberFromSAP={1} because of import Error= {2}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(_poz, String.Format(_format, _partner.Name, _partner.NumberFromSAP, _ex.Message)));
        }
      }
    }
    internal void ImportTable(RoutesCatalogRoute[] routesCatalogRoute, bool testData, Action<ProgressChangedEventArgs> progress)
    {
      if (routesCatalogRoute == null)
        return;
      int _poz = 0;
      foreach (RoutesCatalogRoute _route in routesCatalogRoute)
      {
        try
        {
          _poz++;
          ServiceType _service = GetService(_route);
          Partner _prtnr = GetOrAddJTIPartner(_service, _route.Vendor.Trim(), testData);
          FreightPayer _freightPayer = GetOrAdd<FreightPayer>(m_EDC.FreightPayer, m_FreightPayer, _route.Freight_Payer__I_C__MainLeg.ToString(), testData);
          CityType _CityType = GetOrAddCity(_route.Dest_City.Trim(), _route.Dest_Country.Trim(), "Unknown");
          Currency _Currency = GetOrAdd<Currency>(m_EDC.Currency, m_Currency, _route.Currency, false);
          ShipmentType _ShipmentType = GetOrAdd<ShipmentType>(m_EDC.ShipmentType, m_ShipmentType, _route.ShipmentType, false);
          CarrierType _CarrierCarrierType = GetOrAdd<CarrierType>(m_EDC.Carrier, m_CarrierCarrierType, _route.Carrier, false);
          TranspotUnit _TranspotUnit = GetOrAdd<TranspotUnit>(m_EDC.TransportUnitType, m_TranspotUnit, _route.Equipment_Type__UoM, false);
          SAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlant>(m_EDC.SAPDestinationPlant, m_SAPDestinationPlant, _route.SAP_Dest_Plant, false);
          BusienssDescription _busnessDscrptn = GetOrAdd<BusienssDescription>(m_EDC.BusinessDescription, m_BusinessDescription, _route.Business_description, false);
          Commodity _cmdty = GetOrAdd<Commodity>(m_EDC.Commodity, m_CommodityCommodity, _route.Commodity, false);
          string _sku = _route.Material_Master__Reference;
          string _title = String.Format("2014 To: {0}, by: {1}, of: {2}", _CityType.Tytuł, _prtnr.Tytuł, _route.Commodity);
          switch (_service)
          {
            case ServiceType.Forwarder:
              Route _rt = new Route()
              {
                Route2BusinessDescriptionTitle = _busnessDscrptn,
                CarrierTitle = _CarrierCarrierType,
                Route2CityTitle = _CityType,
                DepartureCity = _route.Dept_City,
                CurrencyTitle = _Currency,
                FreightPayerTitle = _freightPayer,
                GoodsHandlingPO = _route.PO_NUMBER,
                MaterialMaster = _sku,
                DeparturePort = _route.Port_of_Dept,
                RemarkMM = _route.Remarks,
                SAPDestinationPlantTitle = _SAPDestinationPlant,
                ShipmentTypeTitle = _ShipmentType,
                Tytuł = _title,
                TransportCosts = testData ? 4567.8 : _route.Total_Cost_per_UoM.String2Double(),
                TransportUnitTypeTitle = _TranspotUnit,
                PartnerTitle = _prtnr,
                Route2Commodity = _cmdty,
                Incoterm = _route.Selling_Incoterm
              };
              m_EDC.Route.InsertOnSubmit(_rt);
              break;
            case ServiceType.SecurityEscortProvider:
              SecurityEscortCatalog _sec = new SecurityEscortCatalog()
              {
                SecurityEscortCatalog2BusinessDescriptionTitle = _busnessDscrptn,
                CurrencyTitle = _Currency,
                EscortDestination = _route.Dest_City,
                FreightPayerTitle = _freightPayer,
                MaterialMaster = _sku,
                RemarkMM = _route.Remarks,
                SecurityCost = testData ? 345.6 : _route.Total_Cost_per_UoM.String2Double(),
                SecurityEscrotPO = _route.PO_NUMBER,
                Tytuł = _title,
                PartnerTitle = _prtnr
              };
              m_EDC.SecurityEscortRoute.InsertOnSubmit(_sec);
              break;
            case ServiceType.VendorAndForwarder:
            case ServiceType.None:
            case ServiceType.Invalid:
            case ServiceType.Vendor:
            default:
              break;
          }
          m_EDC.SubmitChanges();
        }
        catch (Exception ex)
        {
          string _format = "Cannot add route data SKU={0} Description={1} because of import Error= {2}";
          progress(new ProgressChangedEventArgs(_poz, String.Format(_format, _route.Material_Master_Short_Text, _route.Business_description, ex.Message)));
        }
      } //foreach (RoutesCatalogRoute 
    }
    internal void ImportTable(RoutesCatalogMarket[] routesCatalogMarket, Action<ProgressChangedEventArgs> progress)
    {
      if (routesCatalogMarket == null)
        return;
      int _poz = 0;
      foreach (RoutesCatalogMarket _market in routesCatalogMarket)
      {
        try
        {
          _poz++;
          Market _mrkt = GetOrAdd<Market>(m_EDC.Market, m_MarketMarket, _market.Market, false);
          CityType _CityType = GetOrAddCity(_market.DestinationCity, _market.DestinationCountry, _market.Area);
          string _dstName = DestinationMarketKey(_mrkt, _CityType);
          if (m_DestinationMarket.ContainsKey(_dstName))
            continue;
          DestinationMarket _DestinationMarket = new DestinationMarket()
            {
              DestinationMarket2CityTitle = _CityType,
              MarketTitle = _mrkt
            };
          m_DestinationMarket.Add(_dstName, _DestinationMarket);
          m_EDC.DestinationMarket.InsertOnSubmit(_DestinationMarket);
          m_EDC.SubmitChanges();
        }
        catch (Exception ex)
        {
          string _format = "Cannot add market data DestinationCity={0} Market={1} because of import Error= {2}. The entry is skipped.";
          progress(new ProgressChangedEventArgs(_poz, String.Format(_format, _market.DestinationCity, _market.Market, ex.Message)));
        }
      }
    }
    internal void ReadSiteContent(Action<string> progress)
    {
      progress("Reading Commodity");
      foreach (Commodity _cmdty in m_EDC.Commodity)
        Add<string, Commodity>(m_CommodityCommodity, _cmdty.Tytuł, _cmdty, false);
      progress("Reading Warehouse");
      foreach (Warehouse _wrse in m_EDC.Warehouse)
        Add<string, Warehouse>(m_Warehouse, _wrse.Tytuł, _wrse, false);
      progress("Reading Partner");
      foreach (Partner _prtnr in m_EDC.Partner)
        Add<string, Partner>(m_Partner, _prtnr.Tytuł, _prtnr, false);
      progress("Reading CountryType");
      foreach (CountryType _cntry in m_EDC.Country)
        Add<string, CountryType>(m_CountryClass, _cntry.Tytuł, _cntry, false);
      progress("Reading CityType");
      foreach (CityType _cty in m_EDC.City)
        Add<string, CityType>(m_CityDictionary, _cty.Tytuł, _cty, false);
      progress("Reading FreightPayer");
      foreach (FreightPayer _frpyr in m_EDC.FreightPayer)
        Add<string, FreightPayer>(m_FreightPayer, _frpyr.Tytuł, _frpyr, false);
      progress("Reading Currency");
      foreach (Currency _curr in m_EDC.Currency)
        Add<string, Currency>(m_Currency, _curr.Tytuł, _curr, false);
      progress("Reading BusienssDescription");
      foreach (BusienssDescription _bdsc in m_EDC.BusinessDescription)
        Add<string, BusienssDescription>(m_BusinessDescription, _bdsc.Tytuł, _bdsc, false);
      progress("Reading ShipmentType");
      foreach (ShipmentType _shpmnt in m_EDC.ShipmentType)
        Add<string, ShipmentType>(m_ShipmentType, _shpmnt.Tytuł, _shpmnt, false);
      progress("Reading CarrierType");
      foreach (CarrierType _crr in m_EDC.Carrier)
        Add<string, CarrierType>(m_CarrierCarrierType, _crr.Tytuł, _crr, false);
      progress("Reading TranspotUnit");
      foreach (TranspotUnit _tu in m_EDC.TransportUnitType)
        Add<string, TranspotUnit>(m_TranspotUnit, _tu.Tytuł, _tu, false);
      progress("Reading SAPDestinationPlant");
      foreach (SAPDestinationPlant _sdp in m_EDC.SAPDestinationPlant)
        Add<string, SAPDestinationPlant>(m_SAPDestinationPlant, _sdp.Tytuł, _sdp, false);
      progress("Reading Market");
      foreach (Market _mrkt in m_EDC.Market)
        Add<string, Market>(m_MarketMarket, _mrkt.Tytuł, _mrkt, false);
      progress("Reading DestinationMarket");
      foreach (DestinationMarket _dstm in m_EDC.DestinationMarket)
      {
        string _key = DestinationMarketKey(_dstm.MarketTitle, _dstm.DestinationMarket2CityTitle);
        if (m_DestinationMarket.ContainsKey(_key))
          continue;
        m_DestinationMarket.Add(_key, _dstm);
      }
    }
    internal void SubmitChages()
    {
      m_EDC.SubmitChanges();
    }
    #endregion

    #region private

    #region helpers
    private Direction? ParseDirection(string p)
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
    private ServiceType? ParseServiceType(string p)
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
    private ServiceType GetService(RoutesCatalogRoute _route)
    {
      if (_route.Equipment_Type__UoM.ToUpper().Contains("ESC"))
        return ServiceType.SecurityEscortProvider;
      else
        return ServiceType.Forwarder;
    }
    #endregion

    #region data management
    private type Create<type>(EntityList<type> _EDC, Dictionary<string, type> _dictionary, string key, bool _testData)
      where type : Element, new()
    {
      type _elmnt = new type() { Tytuł = _testData ? EmptyKey : key };
      if (_dictionary.Keys.Contains(key))
        key = String.Format("Duplicated name: {0} [{1}]", key, EmptyKey);
      _dictionary.Add(key, _elmnt);
      _EDC.InsertOnSubmit(_elmnt);
      return _elmnt;
    }
    private void Add<TKey, TEntity>(Dictionary<TKey, TEntity> _dictionary, TKey _key, TEntity entity, bool _testData)
    {
      if (_dictionary.Keys.Contains(_key))
        return;
      _dictionary.Add(_key, entity);
    }
    private type GetOrAdd<type>(EntityList<type> _EDC, Dictionary<string, type> dictionary, string key, bool testData)
      where type : Element, new()
    {
      if (key.IsNullOrEmpty())
        throw new ArgumentNullException("key", String.Format("Cannot add empty key to the dictionary {0}", typeof(type).Name));
      if (dictionary.ContainsKey(key))
        return dictionary[key];
      else
        return Create<type>(_EDC, dictionary, key, testData);
    }
    private Partner GetOrAddJTIPartner(ServiceType service, string partner, bool _testData)
    {
      if (partner.IsNullOrEmpty())
        throw new ArgumentNullException("partner", String.Format("Cannot add empty key to the partner dictionary for country service {0}.", service));
      if (m_Partner.ContainsKey(partner))
        return m_Partner[partner];
      else
      {
        Partner _prtnr = Create<Partner>(m_EDC.Partner, m_Partner, partner, _testData);
        _prtnr.ServiceType = service;
        return _prtnr;
      }
    }
    private CityType GetOrAddCity(string city, string country, string area)
    {
      if (city.IsNullOrEmpty())
        throw new ArgumentNullException("city", String.Format("Cannot add empty key to the city dictionary for country {0}/area {1}.", country, area));
      CityType _city = default(CityType);
      if (m_CityDictionary.ContainsKey(city))
        _city = m_CityDictionary[city];
      else
        _city = Create<CityType>(m_EDC.City, m_CityDictionary, city, false);
      if (_city.CountryTitle != null)
        return _city;
      if (country.IsNullOrEmpty())
        country = "Country-" + EmptyKey;
      CountryType _countryClass = GetOrAdd(m_EDC.Country, m_CountryClass, country, false);
      if (_countryClass.CountryGroup.IsNullOrEmpty() && !area.IsNullOrEmpty())
        _countryClass.CountryGroup = area;
      _city.CountryTitle = _countryClass;
      return _city;
    }
    private EntitiesDataContext m_EDC;
    private static short m_EmptyKeyIdx = 0;
    private static string EmptyKey { get { return String.Format("EmptyKey{0}", m_EmptyKeyIdx++); } }
    private static string DestinationMarketKey(Market mrkt, CityType city)
    {
      string _dstName = String.Format("{0} in {1}", EntityEmptyKey(city), EntityEmptyKey(mrkt));
      return _dstName;
    }
    private static string EntityEmptyKey(Element entity)
    {
      return entity == null ? EmptyKey : entity.Tytuł;
    }
    private string DummyName(string _text, string _replacement, bool _testData)
    {
      return _testData ? String.Format("{0} {1}", _replacement, m_EmptyKeyIdx++) : _text;
    }
    private string DummyEmail(string _text, string _replacement, bool _testData)
    {
      return _testData ? "oferty@cas.eu" : _text;
    }
    #endregion

    #region Dictionaries
    private Dictionary<string, Partner> m_Partner = new Dictionary<string, Partner>();
    private Dictionary<string, FreightPayer> m_FreightPayer = new Dictionary<string, FreightPayer>();
    private Dictionary<string, CityType> m_CityDictionary = new Dictionary<string, CityType>();
    private Dictionary<string, CountryType> m_CountryClass = new Dictionary<string, CountryType>();
    private Dictionary<string, Currency> m_Currency = new Dictionary<string, Currency>();
    private Dictionary<string, ShipmentType> m_ShipmentType = new Dictionary<string, ShipmentType>();
    private Dictionary<string, CarrierType> m_CarrierCarrierType = new Dictionary<string, CarrierType>();
    private Dictionary<string, TranspotUnit> m_TranspotUnit = new Dictionary<string, TranspotUnit>();
    private Dictionary<string, SAPDestinationPlant> m_SAPDestinationPlant = new Dictionary<string, SAPDestinationPlant>();
    private Dictionary<string, Market> m_MarketMarket = new Dictionary<string, Market>();
    private Dictionary<string, Warehouse> m_Warehouse = new Dictionary<string, Warehouse>();
    private Dictionary<string, Commodity> m_CommodityCommodity = new Dictionary<string, Commodity>();
    private Dictionary<string, ShippingPoint> m_ShippingPoint = new Dictionary<string, ShippingPoint>();
    private Dictionary<string, BusienssDescription> m_BusinessDescription = new Dictionary<string, BusienssDescription>();
    private Dictionary<string, DistributionList> m_DistributionList = new Dictionary<string, DistributionList>();
    private Dictionary<string, DestinationMarket> m_DestinationMarket = new Dictionary<string, DestinationMarket>();
    #endregion

    #endregion

    #region IDisposable
    public void Dispose()
    {
      m_EDC.Dispose();
    }
    #endregion

  }
}
