using System.IO;
using System.Xml.Serialization;
using CAS.SmartFactory.Shepherd.RouteEditor.UpdateData;

namespace CAS.SmartFactory.Shepherd.RouteEditor.InputData
{
  public partial class RoutesCatalog
  {
    /// <summary>
    /// Imports the document.
    /// </summary>
    /// <param name="documetStream">The documet stream.</param>
    /// <returns></returns>
    static public RoutesCatalog ImportDocument( Stream documetStream )
    {
      XmlSerializer serializer = new XmlSerializer( typeof( RoutesCatalog ) );
      return (RoutesCatalog)serializer.Deserialize( documetStream );
    }
    /// <summary>
    /// Imports the data.
    /// </summary>
    /// <param name="_URL">The _ URL.</param>
    public void ImportData( string _URL, bool TestingData )
    {
      using ( EntitiesDataDictionary _dictionary = new EntitiesDataDictionary( _URL ) )
      {
        if ( this.CommodityTable != null )
          foreach ( RoutesCatalogCommodityRow _Commodity in this.CommodityTable )
            _dictionary.AddCommodity( _Commodity );
        if ( this.PartnersTable != null )
          foreach ( RoutesCatalogPartnersRow _partner in this.PartnersTable )
          _dictionary.AddPartner( _partner, TestingData );
        if ( this.GlobalPricelist != null )
          foreach ( RoutesCatalogRoute _rt in this.GlobalPricelist )
          _dictionary.AddRoute( _rt, TestingData );
        if ( this.MarketTable != null )
          foreach ( RoutesCatalogMarket _market in this.MarketTable )
          _dictionary.AddMarket( _market );
      }
    }
  }
}
