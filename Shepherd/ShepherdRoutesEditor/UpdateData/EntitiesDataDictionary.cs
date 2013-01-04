using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.Shepherd.RouteEditor.InputData;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.RouteEditor.UpdateData
{

  public class EntitiesDataDictionary: IDisposable
  {
    public EntitiesDataDictionary( string _url )
    {
      m_EDC = new EntitiesDataContext( _url );
    }
    public void Dispose()
    {
      m_EDC.SubmitChanges();
      m_EDC.Dispose();
    }
    internal void AddCommodity( RoutesCatalogCommodityRow _CommodityRow )
    {
      GetOrAdd<Commodity>( m_EDC.Commodity, m_CommodityCommodity, _CommodityRow.Title, false );
    }
    internal void AddPartner( RoutesCatalogPartnersRow _partner, bool _testData )
    {
      if ( m_Partner.ContainsKey( _partner.Name ) )
        return;
      Partner _prtnr = Create<Partner>( m_EDC.Partner, m_Partner, _partner.Name, _testData );
      _prtnr.EmailAddress = DummyEmail( _partner.E_Mail, "AdresEMail", _testData );
      _prtnr.CellPhone = DummyName( _partner.Mobile, "Mobile", _testData );
      _prtnr.ServiceType = ParseServiceType( _partner.ServiceType );
      _prtnr.WorkPhone = DummyName( _partner.BusinessPhone, "BusinessPhone", _testData );
      _prtnr.VendorNumber = DummyName( _partner.NumberFromSAP, "NumberFromSAP", _testData );
      _prtnr.Partner2WarehouseTitle = GetOrAdd<Warehouse>( m_EDC.Warehouse, m_Warehouse, _partner.Warehouse, false );
    }
    internal void AddRoute( RoutesCatalogRoute _route, bool _testData )
    {
      try
      {
        ServiceType _service = GetService( _route );
        Partner _prtnr = GetOrAddJTIPartner( _service, _route.Vendor.Trim(), _testData );
        FreightPayer _freightPayer = GetOrAdd<FreightPayer>( m_EDC.FreightPayer, m_FreightPayer, _route.Freight_Payer__I_C__MainLeg.ToString(), _testData );
        CityType _CityType = GetOrAddCity( _route.Dest_City.Trim(), _route.Dest_Country.Trim(), "Unknown" );
        Currency _Currency = GetOrAdd<Currency>( m_EDC.Currency, m_Currency, _route.Currency, false );
        ShipmentType _ShipmentType = GetOrAdd<ShipmentType>( m_EDC.ShipmentType, m_ShipmentType, _route.ShipmentType, false );
        CarrierType _CarrierCarrierType = GetOrAdd<CarrierType>( m_EDC.Carrier, m_CarrierCarrierType, _route.Carrier, false );
        TranspotUnit _TranspotUnit = GetOrAdd<TranspotUnit>( m_EDC.TransportUnitType, m_TranspotUnit, _route.Equipment_Type__UoM, false );
        SAPDestinationPlant _SAPDestinationPlant = GetOrAdd<SAPDestinationPlant>( m_EDC.SAPDestinationPlant, m_SAPDestinationPlant, _route.SAP_Dest_Plant, false );
        BusienssDescription _busnessDscrptn = GetOrAdd<BusienssDescription>( m_EDC.BusinessDescription, m_BusinessDescription, _route.Business_description, false );
        Commodity _cmdty = GetOrAdd<Commodity>( m_EDC.Commodity, m_CommodityCommodity, _route.Commodity, false );
        string _sku = _route.Material_Master__Reference;
        string _title = String.Format( "2013 To: {0}, by: {1}, of: {2}", _CityType.Tytuł, _prtnr.Tytuł, _route.Commodity );
        switch ( _service )
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
            m_EDC.Route.InsertOnSubmit( _rt );
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
            m_EDC.SecurityEscortRoute.InsertOnSubmit( _sec );
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
      catch ( Exception ex )
      {
        string _format = "Cannot add route data SKU={0} Description={1} because of import Error= {2}";
        throw new ApplicationException( String.Format( _format, _route.Material_Master_Short_Text, _route.Business_description, ex.Message ) );
      }
    }
    internal void AddMarket( RoutesCatalogMarket _market )
    {
      try
      {
        Market _mrkt = GetOrAdd<Market>( m_EDC.Market, m_MarketMarket, _market.Market, false );
        CityType _CityType = GetOrAddCity( _market.DestinationCity, _market.DestinationCountry, _market.Area );
        string _dstName = DestinationMarketKey( _mrkt, _CityType );
        if ( m_DestinationMarket.ContainsKey( _dstName ) )
          return;
        DestinationMarket _DestinationMarket = new DestinationMarket()
          {
            //Tytuł = _dstName,
            DestinationMarket2CityTitle = _CityType,
            MarketTitle = _mrkt
          };
        m_DestinationMarket.Add( _dstName, _DestinationMarket );
        m_EDC.DestinationMarket.InsertOnSubmit( _DestinationMarket );
        m_EDC.SubmitChanges();
      }
      catch ( Exception ex )
      {
        string _format = "Cannot add market data DestinationCity={0} Market={1} because of import Error= {2}";
        throw new ApplicationException( String.Format( _format, _market.DestinationCity, _market.Market, ex.Message ) );
      }
    }
    internal void GetDictionaries()
    {
      foreach ( Commodity _cmdty in m_EDC.Commodity )
        Add<string, Commodity>( m_CommodityCommodity, _cmdty.Tytuł, _cmdty, false );
      foreach ( Warehouse _wrse in m_EDC.Warehouse )
        Add<string, Warehouse>( m_Warehouse, _wrse.Tytuł, _wrse, false );
      foreach ( Partner _prtnr in m_EDC.Partner )
        Add<string, Partner>( m_Partner, _prtnr.Tytuł, _prtnr, false );
      foreach ( var _cntry in m_EDC.Country )
        Add<string, CountryType>( m_CountryClass, _cntry.Tytuł, _cntry, false );
      foreach ( CityType _cty in m_EDC.City )
        Add<string, CityType>( m_CityDictionary, _cty.Tytuł, _cty, false );
      foreach ( FreightPayer _frpyr in m_EDC.FreightPayer )
        Add<string, FreightPayer>( m_FreightPayer, _frpyr.Tytuł, _frpyr, false );
      foreach ( Currency _curr in m_EDC.Currency )
        Add<string, Currency>( m_Currency, _curr.Tytuł, _curr, false );
      foreach ( var _bdsc in m_EDC.BusinessDescription )
        Add<string, BusienssDescription>( m_BusinessDescription, _bdsc.Tytuł, _bdsc, false );
      foreach ( ShipmentType _shpmnt in m_EDC.ShipmentType )
        Add<string, ShipmentType>( m_ShipmentType, _shpmnt.Tytuł, _shpmnt, false );
      foreach ( CarrierType _crr in m_EDC.Carrier )
        Add<string, CarrierType>( m_CarrierCarrierType, _crr.Tytuł, _crr, false );
      foreach ( TranspotUnit _tu in m_EDC.TransportUnitType )
        Add<string, TranspotUnit>( m_TranspotUnit, _tu.Tytuł, _tu, false );
      foreach ( SAPDestinationPlant _sdp in m_EDC.SAPDestinationPlant )
        Add<string, SAPDestinationPlant>( m_SAPDestinationPlant, _sdp.Tytuł, _sdp, false );
      foreach ( Market _mrkt in m_EDC.Market )
        Add<string, Market>( m_MarketMarket, _mrkt.Tytuł, _mrkt, false );
      foreach ( DestinationMarket _dstm in m_EDC.DestinationMarket )
      {
        string _key = DestinationMarketKey( _dstm.MarketTitle, _dstm.DestinationMarket2CityTitle );
        if ( m_DestinationMarket.ContainsKey( _key ) )
          continue;
        m_DestinationMarket.Add( _key, _dstm );
      }
    }

    #region private

    #region helpers
    private Direction? ParseDirection( string p )
    {
      p = p.ToUpper();
      if ( p.Contains( "OUTBOUND" ) )
        return Direction.Outbound;
      if ( p.Contains( "INBOUND" ) )
        return Direction.Inbound;
      if ( p.Contains( "BOTH" ) )
        return Direction.BothDirections;
      return null;
    }
    private ServiceType? ParseServiceType( string p )
    {
      p = p.ToUpper();
      string _vs = "VENDOR";
      string _fs = "FORWARDER";
      string _es = "ESCORT";
      if ( p.Contains( _vs ) )
        if ( p.Contains( _fs ) )
          return ServiceType.VendorAndForwarder;
        else
          return ServiceType.Vendor;
      if ( p.Contains( _fs ) )
        return ServiceType.Forwarder;
      if ( p.Contains( _es ) )
        return ServiceType.SecurityEscortProvider;
      return null;
    }
    private ServiceType GetService( RoutesCatalogRoute _route )
    {
      if ( _route.Equipment_Type__UoM.ToUpper().Contains( "ESC" ) )
        return ServiceType.SecurityEscortProvider;
      else
        return ServiceType.Forwarder;
    }
    #endregion

    #region data management
    private type Create<type>( EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key, bool _testData )
      where type: Element, new()
    {
      type _elmnt = new type() { Tytuł = _testData ? EmptyKey : _key };
      if ( _dictionary.Keys.Contains( _key ) )
        _key = String.Format( "Duplicated name: {0} [{1}]", _key, EmptyKey );
      _dictionary.Add( _key, _elmnt );
      _EDC.InsertOnSubmit( _elmnt );
      return _elmnt;
    }
    private void Add<TKey, TEntity>( Dictionary<TKey, TEntity> _dictionary, TKey _key, TEntity entity, bool _testData )
    {
      if ( _dictionary.Keys.Contains( _key ) )
        return;
      _dictionary.Add( _key, entity );
    }
    private type GetOrAdd<type>( EntityList<type> _EDC, Dictionary<string, type> _dictionary, string _key, bool _testData )
      where type: Element, new()
    {
      //TODO if ( _key.IsNullOrEmpty() )
      //  _key = EmptyKey;
      if ( _dictionary.ContainsKey( _key ) )
        return _dictionary[ _key ];
      else
        return Create<type>( _EDC, _dictionary, _key, _testData );
    }
    private Partner GetOrAddJTIPartner( ServiceType _st, string _partner, bool _testData )
    {
      if ( m_Partner.ContainsKey( _partner ) )
        return m_Partner[ _partner ];
      else
      {
        Partner _prtnr = Create<Partner>( m_EDC.Partner, m_Partner, _partner, _testData );
        _prtnr.ServiceType = _st;
        return _prtnr;
      }
    }
    private CityType GetOrAddCity( string _city, string _country, string _area )
    {
      CityType _prtnr = default( CityType );
      if ( m_CityDictionary.ContainsKey( _city ) )
        _prtnr = m_CityDictionary[ _city ];
      else
        _prtnr = Create<CityType>( m_EDC.City, m_CityDictionary, _city, false );
      if ( _prtnr.CountryTitle != null )
        return _prtnr;
      if ( _country.IsNullOrEmpty() )
        _country = "Country-" + EmptyKey;
      CountryType _countryClass = GetOrAdd( m_EDC.Country, m_CountryClass, _country, false );
      if ( _countryClass.CountryGroup.IsNullOrEmpty() && !_area.IsNullOrEmpty() )
        _countryClass.CountryGroup = _area;
      _prtnr.CountryTitle = _countryClass;
      return _prtnr;
    }
    private EntitiesDataContext m_EDC;
    private static short m_EmptyKeyIdx = 0;
    private static string EmptyKey { get { return String.Format( "EmptyKey{0}", m_EmptyKeyIdx++ ); } }
    private static string DestinationMarketKey( Market mrkt, CityType city )
    {
      string _dstName = String.Format( "{0} in {1}", EntityEmptyKey( city ), EntityEmptyKey( mrkt ) );
      return _dstName;
    }
    private static string EntityEmptyKey( Element entity )
    {
      return entity == null ? EmptyKey : entity.Tytuł;
    }
    private string DummyName( string _text, string _replacement, bool _testData )
    {
      return _testData ? String.Format( "{0} {1}", _replacement, m_EmptyKeyIdx++ ) : _text;
    }
    private string DummyEmail( string _text, string _replacement, bool _testData )
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

  }
}
