using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.Shepherd.ImportDataModel;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.RouteEditor.UpdateData
{

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
    internal void AddCommodity(PreliminaryDataRouteCommodityRow _CommodityRow)
    {
      Commodity _c = Create<Commodity>(m_EDC.Commodity, m_CommodityCommodity, _CommodityRow.Title, false);
    }
    internal void AddWarehouse(PreliminaryDataRouteWarehouseRow _warehouse)
    {
      Commodity _commodity = GetOrAdd<Commodity>(m_EDC.Commodity, m_CommodityCommodity, _warehouse.Commodity, false);
      Warehouse _wh = Create<Warehouse>(m_EDC.Warehouse, m_Warehouse, _warehouse.Title, false);
      _wh.CommodityTitle = _commodity;
    }
    internal void AddShippingPoint(PreliminaryDataRouteShippingPointRow _shippingPoint)
    {
      Warehouse _wh = GetOrAdd<Warehouse>(m_EDC.Warehouse, m_Warehouse, _shippingPoint.Warehouse, false);
      ShippingPoint _sp = Create<ShippingPoint>(m_EDC.ShippingPoint, m_ShippingPoint, _shippingPoint.Title, false);
      _sp.ShippingPointDescription = _shippingPoint.Description;
      _sp.Direction = ParseDirection(_shippingPoint.Direction);
      _sp.WarehouseTitle = _wh;
    }
    internal void AddPartner(PreliminaryDataRoutePartnersRow _partner, bool _testData)
    {
      Partner _prtnr = Create<Partner>(m_EDC.Partner, m_Partner, _partner.Name, _testData);
      _prtnr.EmailAddress = DummyEmail(_partner.E_Mail, "AdresEMail", _testData);
      _prtnr.CellPhone = DummyName(_partner.Mobile, "Mobile", _testData); ;
      _prtnr.ServiceType = ParseServiceType(_partner.ServiceType);
      _prtnr.WorkPhone = DummyName(_partner.BusinessPhone, "BusinessPhone", _testData); ;
      _prtnr.VendorNumber = DummyName(_partner.NumberFromSAP, "NumberFromSAP", _testData); ;
      _prtnr.Partner2WarehouseTitle = GetOrAdd<Warehouse>(m_EDC.Warehouse, m_Warehouse, _partner.Warehouse, false);
    }
    internal void AddFreightPayer(PreliminaryDataRoutePayersRow item, bool _testData)
    {
      FreightPayer _fp = Create<FreightPayer>(m_EDC.FreightPayer, m_FreightPayer, item.Freight_Payer__I_C__MainLeg, _testData);
      _fp.CompanyAddress = DummyName(item.Address, "Address", _testData);
      _fp.PayerName = DummyName(item.Name, "Company", _testData);
      _fp.WorkZip = item.ZIP_Postal_Code;
      _fp.WorkCountry = item.Country_Region;
      _fp.WorkCity = item.City;
      _fp.NIP = DummyName(item.NIP___VAT_No, "NIPVATNo", _testData);
      string _sitf = "{0}\n{1}\n{2} {3}\n{4}";
      _fp.SendInvoiceToMultiline = DummyName(String.Format(_sitf, item.Name2, item.Country_Region6, item.ZIP_Postal_Code4, item.City5, item.Address3), "SendInvoiceTo", _testData);
    }
    internal void AddRoute(PreliminaryDataRouteRoute _route, bool _testData)
    {

      try
      {
        ServiceType _service = GetService(_route);
        Partner _prtnr = GetOrAddJTIPartner(_service, _route.Vendor.Trim(), _testData);
        FreightPayer _freightPayer = GetOrAdd<FreightPayer>(m_EDC.FreightPayer, m_FreightPayer, _route.Freight_Payer__I_C__MainLeg, _testData);
        CityType _CityType = GetOrAddCity(_route.Dest_City, _route.Dest_Country, null);
        Currency _Currency = GetOrAdd<Currency>(m_EDC.Currency, m_Currency, _route.Currency, false);
        ShipmentType _ShipmentType = GetOrAdd<ShipmentType>(m_EDC.ShipmentType, m_ShipmentType, _route.ShipmentType, false);
        CarrierType _CarrierCarrierType = GetOrAdd<CarrierType>(m_EDC.Carrier, m_CarrierCarrierType, _route.Carrier, false);
        TranspotUnit _TranspotUnit = GetOrAdd<TranspotUnit>(m_EDC.TransportUnitType, m_TranspotUnit, _route.Equipment_Type__UoM, false);
        SAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlant>(m_EDC.SAPDestinationPlant, m_SAPDestinationPlant, _route.SAP_Dest_Plant, false);
        BusienssDescription _busnessDscrptn = GetOrAdd<BusienssDescription>(m_EDC.BusinessDescription, m_BusinessDescription, _route.Business_description, false);
        Commodity _cmdty = GetOrAdd<Commodity>(m_EDC.Commodity, m_CommodityCommodity, _route.Commodity, false);
        string _sku = _route.Material_Master__Reference;
        string _title = String.Format("To: {0}, by: {1}, of: {2}", _CityType.Tytuł, _prtnr.Tytuł, _route.Commodity);
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
              TransportCosts = _testData ? 4567.8 : _route.Total_Cost_per_UoM.String2Double(),
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
              SecurityCost = _testData ? 345.6 : _route.Total_Cost_per_UoM.String2Double(),
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
        throw new ApplicationException(String.Format(_format, _route.Material_Master_Short_Text, _route.Business_description, ex.Message));
      }
    }
    internal void AddMarket(PreliminaryDataRouteMarket _market)
    {
      try
      {
        Market _mrkt = GetOrAdd<Market>(m_EDC.Market, m_MarketMarket, _market.Market, false);
        CityType _CityType = GetOrAddCity(_market.DestinationCity, _market.DestinationCountry, _market.Area);
        string _dstName = String.Format("{0} in {1}", _CityType.Tytuł, _mrkt.Tytuł);
        DestinationMarket _DestinationMarket = new DestinationMarket()
          {
            //Tytuł = _dstName,
            DestinationMarket2CityTitle = _CityType,
            MarketTitle = _mrkt
          };
        m_EDC.DestinationMarket.InsertOnSubmit(_DestinationMarket);
        m_EDC.SubmitChanges();
      }
      catch (Exception ex)
      {
        string _format = "Cannot add market data DestinationCity={0} Market={1} because of import Error= {2}";
        throw new ApplicationException(String.Format(_format, _market.DestinationCity, _market.Market, ex.Message));
      }
    }
    internal void AddRole(PreliminaryDataRouteRole _role, bool _testData)
    {
      DistributionList _dl = Create<DistributionList>(m_EDC.DistributionList, m_DistributionList, _role.Title, false);
      _dl.EmailAddress = DummyEmail(_role.E_mail, "AddressEmail", _testData);
      try
      {
        _dl.ShepherdRole = (ShepherdRole)Enum.Parse(typeof(ShepherdRole), _role.Shepherd_Role, true);
      }
      catch (Exception)
      {
        _dl.ShepherdRole = ShepherdRole.Invalid;
      }
    }
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
    private ServiceType GetService(PreliminaryDataRouteRoute _route)
    {
      if (_route.Equipment_Type__UoM.ToUpper().Contains("ESC"))
        return ServiceType.SecurityEscortProvider;
      else
        return ServiceType.Forwarder;
    }
    #endregion

    #region data management
    private type Create<type>(EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key, bool _testData)
      where type : Element, new()
    {
      type _elmnt = new type() { Tytuł = _testData ? EmptyKey : _key };
      if (_dictionary.Keys.Contains(_key))
        _key = String.Format("Duplicated name: {0} [{1}]", _key, EmptyKey);
      _dictionary.Add(_key, _elmnt);
      _EDC.InsertOnSubmit(_elmnt);
      return _elmnt;
    }
    private type GetOrAdd<type>(EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key, bool _testData)
      where type : Element, new()
    {
      if (_key.IsNullOrEmpty())
        _key = EmptyKey;
      if (_dictionary.ContainsKey(_key))
        return _dictionary[_key];
      else
        return Create<type>(_EDC, _dictionary, _key, _testData);
    }
    private Partner GetOrAddJTIPartner(ServiceType _st, string _partner, bool _testData)
    {
      if (m_Partner.ContainsKey(_partner))
        return m_Partner[_partner];
      else
      {
        Partner _prtnr = Create<Partner>(m_EDC.Partner, m_Partner, _partner, _testData);
        _prtnr.ServiceType = _st;
        return _prtnr;
      }
    }
    private CityType GetOrAddCity(string _city, string _country, string _area)
    {
      CountryType _countryClass = GetOrAdd(m_EDC.Country, m_CountryClass, _country, false);
      if (_countryClass.CountryGroup.IsNullOrEmpty() && !_area.IsNullOrEmpty())
        _countryClass.CountryGroup = _area;
      if (m_CityType.ContainsKey(_city))
        return m_CityType[_city];
      CityType _prtnr = Create<CityType>(m_EDC.City, m_CityType, _city, false);
      _prtnr.CountryTitle = _countryClass;
      return _prtnr;
    }
    private EntitiesDataContext m_EDC;
    private short m_EmptyKeyIdx = 0;
    private string EmptyKey { get { return String.Format("EmptyKey{0}", m_EmptyKeyIdx++); } }
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
    private Dictionary<string, CityType> m_CityType = new Dictionary<string, CityType>();
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
    #endregion

    #endregion

  }
}
