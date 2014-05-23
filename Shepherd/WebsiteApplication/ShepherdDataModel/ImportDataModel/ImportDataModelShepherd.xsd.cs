using System.IO;
using System.Xml.Serialization;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.DataModel.ImportDataModel
{
  public partial class PreliminaryDataRoute
  {
    /// <summary>
    /// Imports the document.
    /// </summary>
    /// <param name="documetStream">The documet stream.</param>
    /// <returns></returns>
    static public PreliminaryDataRoute ImportDocument(Stream documetStream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PreliminaryDataRoute));
      return (PreliminaryDataRoute)serializer.Deserialize(documetStream);
    }
    /// <summary>
    /// Imports the data.
    /// </summary>
    /// <param name="_URL">The _ URL.</param>
    public void ImportData(string _URL)
    {
      using (EntitiesDataDictionary _dictionary = new EntitiesDataDictionary(_URL))
      {
        foreach (PreliminaryDataRouteCommodityRow _Commodity in this.CommodityTable)
        {
          _dictionary.AddCommodity(_Commodity);
        }
        foreach (PreliminaryDataRouteWarehouseRow _warehouse in this.WarehouseTable)
        {
          _dictionary.AddWarehouse(_warehouse);
        }
        foreach (PreliminaryDataRouteShippingPointRow _shippingPoint in this.ShippingPointTable)
        {
          _dictionary.AddShippingPoint(_shippingPoint);
        }
        foreach (PreliminaryDataRoutePartnersRow _partner in this.PartnersTable)
        {
          _dictionary.AddPartner(_partner, TestingData);
        }
        foreach (PreliminaryDataRoutePayersRow _payer in this.PayersTable)
        {
          _dictionary.AddFreightPayer(_payer, TestingData);
        }
        foreach (PreliminaryDataRouteRoute _rt in this.GlobalPricelist)
        {
          _dictionary.AddRoute(_rt, TestingData);
        }
        foreach (PreliminaryDataRouteMarket _market in this.MarketTable)
        {
          _dictionary.AddMarket(_market);
        }
        foreach (PreliminaryDataRouteRole _role in this.DistributionList)
        {
          _dictionary.AddRole(_role, TestingData);
        }
      }
    }
  }
}
