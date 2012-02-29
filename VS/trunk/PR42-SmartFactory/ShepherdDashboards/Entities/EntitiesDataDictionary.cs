using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  using UpdateToolStripEvent = CAS.SmartFactory.Shepherd.Dashboards.GlobalDefinitions.UpdateToolStripEvent;
  using Microsoft.SharePoint.Linq;

  public class EntitiesDataDictionary : IDisposable
  {
    public EntitiesDataDictionary(string _url)
    {
      m_EDC = new EntitiesDataContext(_url);
    }
    public void Dispose()
    {
      m_EDC.SubmitChanges();
      m_EDC.Dispose(); ;
    }
    public void AddRoute(UpdateToolStripEvent _update, Schemas.PreliminaryDataRouteRoute _route, bool _testData)
    {

      try
      {
        ServiceType _service = GetService(_route, _update);
        Partner _prtnr = GetOrAddJTIPartner(_update, _service, _route.Vendor.Trim());
        FreightPayer _freightPayer = GetOrAdd<FreightPayer>(m_EDC.FreightPayer, _update, m_FreightPayer, _route.Freight_Payer__I_C__MainLeg);
        CityType _CityType = GetOrAddCity(_update, _route.Dest_City, _route.Dest_Country);
        Currency _Currency = GetOrAdd<Currency>(m_EDC.Currency, _update, m_Currency, _route.Currency);
        ShipmentTypeShipmentType _ShipmentType = GetOrAdd<ShipmentTypeShipmentType>(m_EDC.ShipmentType, _update, m_ShipmentType, ShipmentTypeParse(_route.Material_Master_Short_Text));
        CarrierCarrierType _CarrierCarrierType = GetOrAdd<CarrierCarrierType>(m_EDC.Carrier, _update, m_CarrierCarrierType, _route.Carrier);
        TransportUnitTypeTranspotUnit _TransportUnitTypeTranspotUnit = GetOrAdd<TransportUnitTypeTranspotUnit>(m_EDC.TransportUnitType, _update, m_TransportUnitTypeTranspotUnit, _route.Equipment_Type__UoM);
        SAPDestinationPlantSAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlantSAPDestinationPlant>(m_EDC.SAPDestinationPlant, _update, m_SAPDestinationPlant, _route.SAP_Dest_Plant);
        BusienssDescription _busnessDscrptn = GetOrAdd<BusienssDescription>(m_EDC.BusinessDescription, _update, m_BusinessDescription, _route.Business_description);
        string _sku = _route.Material_Master_Short_Text;
        string __title = String.Format("To: {0}, by: {1}, of: {2}", _CityType.Tytuł, _prtnr.Tytuł, _route.Commodity);
        switch (_service)
        {
          case ServiceType.Forwarder:
            Route _rt = new Route()
            {
              BusinessDescription = _busnessDscrptn,
              Carrier = _CarrierCarrierType,
              CityName = _CityType,
              CityOfDeparture = _route.Dept_City,
              Currency = _Currency,
              FreightPayer = _freightPayer,
              FreightPO = _route.PO_NUMBER,
              MaterialMaster = _sku,
              PortOfDeparture = _route.Port_of_Dept,
              RemarkMM = _route.Remarks,
              SAPDestinationPlant = _SAPDestinationPlant,
              ShipmentType = _ShipmentType,
              TransportCosts = _testData ? 4567.8 : _route.Total_Cost_per_UoM.String2Double(),
              TransportUnitType = _TransportUnitTypeTranspotUnit,
              VendorName = _prtnr
            };
            m_EDC.Route.InsertOnSubmit(_rt);
            break;
          case ServiceType.SecurityEscortProvider:
            SecurityEscortCatalog _sec = new SecurityEscortCatalog()
            {
              BusinessDescription = _busnessDscrptn,
              Currency = _Currency,
              EscortDestination = _route.Dest_City,
              FreightPayer = _freightPayer,
              MaterialMaster = _sku,
              RemarkMM = _route.Remarks,
              SecurityCost = _testData ? 345.6 : _route.Total_Cost_per_UoM.String2Double(),
              SecurityEscortPO = _route.PO_NUMBER,
              Tytuł = __title,
              VendorName = _prtnr
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
        Entities.Anons.WriteEntry(m_EDC, "AddRoute", String.Format(_format, _route.Material_Master_Short_Text, _route.Business_description, ex.Message));
      }
    }
    internal void AddMarket(UpdateToolStripEvent _update, Schemas.PreliminaryDataRouteMarket _market)
    {
      try
      {
        MarketMarket _mrkt = GetOrAdd<MarketMarket>(m_EDC.Market, _update, m_MarketMarket, "");
        CityType _CityType = GetOrAddCity(_update, _market.DestinationCity, _market.DestinationCountry);
        string _dstName = String.Format("{0} in {1}", _CityType.Tytuł, _mrkt.Tytuł);
        DestinationMarket _DestinationMarket = new DestinationMarket()
          {
            Tytuł = _dstName,
            CityName = _CityType,
            Market = _mrkt
          };
        m_EDC.DestinationMarket.InsertOnSubmit(_DestinationMarket);
        m_EDC.SubmitChanges();

      }
      catch (Exception ex)
      {
        string _format = "Cannot add market data DestinationCity={0} Market={1} because of import Error= {2}";
        Entities.Anons.WriteEntry(m_EDC, "AddRoute", String.Format(_format, _market.DestinationCity, _market.Market, ex.Message));
      }
    }
    public void AddWarehouse(UpdateToolStripEvent _update)
    {
      CommodityCommodity _cmmdty = GetOrAdd<CommodityCommodity>(m_EDC.Commodity, _update, m_CommodityCommodity, "");
      Warehouse _wrse = Create<Warehouse>(m_EDC.Warehouse, m_Warehouse, ""); //TODO
      _wrse.Commodity = _cmmdty;
    }
    public void AddShippingPoint(UpdateToolStripEvent _update, Direction _direction)
    {
      Warehouse _wrse = GetOrAdd<Warehouse>(m_EDC.Warehouse, _update, m_Warehouse, "");
      ShippingPoint _shippingPoint = Create<ShippingPoint>(m_EDC.ShippingPoint, m_ShippingPoint, "");
      _shippingPoint.Warehouse = _wrse;
      _shippingPoint.Direction = _direction;
    }
    #region private

    #region helpers
    private string ShipmentTypeParse(string _sku)
    {
      if (!_sku.IsNullOrEmpty())
      {
        string _skuTrmed = _sku.ToUpper().Trim();
        foreach (var item in m_ShipmentTypeLabelsCollection)
          if (_skuTrmed.Contains(item.Key))
            return item.Value;
      }
      return "VARI";
    }
    private ServiceType GetService(Schemas.PreliminaryDataRouteRoute _route, UpdateToolStripEvent _update)
    {
      if (_route.Equipment_Type__UoM.ToUpper().Contains("ESC"))
        return ServiceType.SecurityEscortProvider;
      else
        return ServiceType.Forwarder;
    }
    #endregion
    #region data management
    private static type Create<type>(EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key) where type : Element, new()
    {
      type _elmnt = new type() { Tytuł = _key };
      _dictionary.Add(_key, _elmnt);
      _EDC.InsertOnSubmit(_elmnt);
      return _elmnt;
    }
    private type GetOrAdd<type>(EntityList<type> _EDC, UpdateToolStripEvent _update, Dictionary<string, type> _dictionary, string _key)
      where type : Element, new()
    {
      if (_key.IsNullOrEmpty())
        _key = EmptyKey;
      if (_dictionary.ContainsKey(_key))
        return _dictionary[_key];
      else
        return Create<type>(_EDC, _dictionary, _key);
    }
    private Partner GetOrAddJTIPartner(UpdateToolStripEvent _update, ServiceType _st, string _partner)
    {
      if (m_Partner.ContainsKey(_partner))
        return m_Partner[_partner];
      else
      {
        Partner _prtnr = Create<Partner>(m_EDC.Partner, m_Partner, _partner);
        _prtnr.ServiceType = _st;
        return _prtnr;
      }
    }
    private CityType GetOrAddCity(UpdateToolStripEvent _update, string _city, string _country)
    {
      if (m_CityType.ContainsKey(_city))
        return m_CityType[_city];
      else
      {
        CountryClass _countryClass = GetOrAdd(m_EDC.Country, _update, m_CountryClass, _country);
        CityType _prtnr = Create<CityType>(m_EDC.City, m_CityType, _city);
        _prtnr.CountryName = _countryClass;
        return _prtnr;
      }
    }
    private EntitiesDataContext m_EDC;
    private short m_EmptyKeyIdx = 0;
    private string EmptyKey { get { return String.Format("EmptyKey{0}", m_EmptyKeyIdx++); } }
    #endregion

    #region Dictionaries
    private Dictionary<string, Partner> m_Partner = new Dictionary<string, Partner>();
    private Dictionary<string, FreightPayer> m_FreightPayer = new Dictionary<string, FreightPayer>();
    private Dictionary<string, CityType> m_CityType = new Dictionary<string, CityType>();
    private Dictionary<string, CountryClass> m_CountryClass = new Dictionary<string, CountryClass>();
    private Dictionary<string, Currency> m_Currency = new Dictionary<string, Currency>();
    private Dictionary<string, ShipmentTypeShipmentType> m_ShipmentType = new Dictionary<string, ShipmentTypeShipmentType>();
    private Dictionary<string, CarrierCarrierType> m_CarrierCarrierType = new Dictionary<string, CarrierCarrierType>();
    private Dictionary<string, TransportUnitTypeTranspotUnit> m_TransportUnitTypeTranspotUnit = new Dictionary<string, TransportUnitTypeTranspotUnit>();
    private Dictionary<string, SAPDestinationPlantSAPDestinationPlant> m_SAPDestinationPlant = new Dictionary<string, SAPDestinationPlantSAPDestinationPlant>();
    private Dictionary<string, MarketMarket> m_MarketMarket = new Dictionary<string, MarketMarket>();
    private Dictionary<string, Warehouse> m_Warehouse = new Dictionary<string, Warehouse>();
    private Dictionary<string, CommodityCommodity> m_CommodityCommodity = new Dictionary<string, CommodityCommodity>();
    private Dictionary<string, ShippingPoint> m_ShippingPoint = new Dictionary<string, ShippingPoint>();
    private Dictionary<string, BusienssDescription> m_BusinessDescription = new Dictionary<string, BusienssDescription>();
    private Dictionary<string, string> m_ShipmentTypeLabelsCollection = new Dictionary<string, string>() { { "SHIP", "OCEAN Freight" }, { "ROAD", "LAND Freight" }, { "FOBD", "FOBD" }, { "MMOD", "MMOD" } };
    #endregion

    #endregion

  }
}
