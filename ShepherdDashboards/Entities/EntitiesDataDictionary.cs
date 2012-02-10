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
    public void AddRoute(UpdateToolStripEvent _update, ServiceType _service)
    {
      Partner _prtnr = GetOrAddJTIPartner(_update, "", _service);
      FreightPayer _freightPayer = GetOrAdd<FreightPayer>(m_EDC.FreightPayer, _update, m_FreightPayer, "");
      CityType _CityType = GetOrAddCity(_update, "", "");
      Currency _Currency = GetOrAdd<Currency>(m_EDC.Currency, _update, m_Currency, "");
      ShipmentTypeShipmentType _ShipmentType = GetOrAdd<ShipmentTypeShipmentType>(m_EDC.ShipmentType, _update, m_ShipmentType, "");
      CarrierCarrierType _CarrierCarrierType = GetOrAdd<CarrierCarrierType>(m_EDC.Carrier, _update, m_CarrierCarrierType, "");
      TransportUnitTypeTranspotUnit _TransportUnitTypeTranspotUnit = GetOrAdd<TransportUnitTypeTranspotUnit>(m_EDC.TransportUnitType, _update, m_TransportUnitTypeTranspotUnit, "");
      SAPDestinationPlantSAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlantSAPDestinationPlant>(m_EDC.SAPDestinationPlant, _update, m_SAPDestinationPlant, "");
      string _sku = "";
      Route _rt = Create<Route>(m_EDC.Route, m_Route, _sku);
      _rt.Carrier = _CarrierCarrierType;
      _rt.CityName = _CityType;
      _rt.Currency = _Currency;
      _rt.FreightPayer = _freightPayer;
      _rt.FreightPO = ""; //TODO
      _rt.MaterialMaster = ""; //TODO
      _rt.RemarkMM = ""; //TODO
      _rt.SAPDestinationPlant = _SAPDestinationPlant;
      _rt.ShipmentType = _ShipmentType;
      _rt.TransportUnitType = _TransportUnitTypeTranspotUnit;
      _rt.VendorName = _prtnr;
      m_EDC.SubmitChanges();
    }
    public void AddMarket(UpdateToolStripEvent _update)
    {
      MarketMarket _mrkt = GetOrAdd<MarketMarket>(m_EDC.Market, _update, m_MarketMarket, "");
      CityType _CityType = GetOrAddCity(_update, "", "");
      string _dstName = String.Format("{0} in {1}", _CityType.Tytuł, _mrkt.Tytuł);
      DestinationMarket _DestinationMarket = Create<DestinationMarket>(m_EDC.DestinationMarket, m_DestinationMarket, _dstName);
      _DestinationMarket.CityName = _CityType;
      _DestinationMarket.Market = _mrkt;
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
      ShippingPoint _shippingPoint = Create<ShippingPoint>(m_EDC.ShippingPoint, m_ShippingPoint, ""); //TODO
      _shippingPoint.Warehouse = _wrse;
      _shippingPoint.Direction = _direction;
    }

    #region private
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
      if (_dictionary.ContainsKey(_key))
        return _dictionary[_key];
      else
        return Create<type>(_EDC, _dictionary, _key);
    }
    private Partner GetOrAddJTIPartner(UpdateToolStripEvent _update, string _partner, ServiceType _st)
    {
      if (m_Partner.ContainsKey(_partner))
        return m_Partner[_partner];
      else
      {
        Partner _prtnr = Create<Partner>(m_EDC.JTIPartner, m_Partner, _partner);
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
    private Dictionary<string, DestinationMarket> m_DestinationMarket = new Dictionary<string, DestinationMarket>();
    private Dictionary<string, Warehouse> m_Warehouse = new Dictionary<string, Warehouse>();
    private Dictionary<string, CommodityCommodity> m_CommodityCommodity = new Dictionary<string, CommodityCommodity>();
    private Dictionary<string, ShippingPoint> m_ShippingPoint = new Dictionary<string, ShippingPoint>();
    private Dictionary<string, Route> m_Route = new Dictionary<string, Route>();
    #endregion
    #endregion

  }
}
