using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.Schemas
{
  public partial class PreliminaryDataRoute
  {
    static public PreliminaryDataRoute ImportDocument(Stream documetStream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PreliminaryDataRoute));
      return (PreliminaryDataRoute)serializer.Deserialize(documetStream);
    }
    public void ImportData(PreliminaryDataRoute cnfg, string _URL, GlobalDefinitions.UpdateToolStripEvent _update, bool _testData)
    {
      _update(this, new ProgressChangedEventArgs(1, "ImportData starting"));
      using (EntitiesDataDictionary _dictionary = new EntitiesDataDictionary(_URL.Trim()))
      {
        foreach (PreliminaryDataRouteCommodityRow _Commodity in this.CommodityTable)
        {
          _dictionary.AddCommodity(_update, _Commodity);
          _update(this, new ProgressChangedEventArgs(1, "AddCommodity " + _Commodity.Title));
        }
        foreach (PreliminaryDataRouteWarehouseRow _warehouse in this.WarehouseTable)
        {
          _dictionary.AddWarehouse(_update, _warehouse);
          _update(this, new ProgressChangedEventArgs(1, "AddWarehouse " + _warehouse.Title));
        }
        foreach (PreliminaryDataRouteShippingPointRow _shippingPoint in this.ShippingPointTable)
        {
          _dictionary.AddShippingPoint(_update, _shippingPoint);
          _update(this, new ProgressChangedEventArgs(1, "AddShippingPoint " + _shippingPoint.Title));
        }
        foreach (PreliminaryDataRoutePartnersRow _partner in this.PartnersTable)
        {
          _dictionary.AddPartner(_update, _partner, _testData);
          _update(this, new ProgressChangedEventArgs(1, "AddPartner " + _partner.Name));
        }
        foreach (PreliminaryDataRoutePayersRow _payer in this.PayersTable)
        {
          _dictionary.AddFreightPayer(_update, _payer, _testData);
          _update(this, new ProgressChangedEventArgs(1, "AddPartner " + _payer.Name));
        }
        foreach (PreliminaryDataRouteRoute _rt in this.GlobalPricelist)
        {
          _dictionary.AddRoute(_update, _rt, _testData);
          _update(this, new ProgressChangedEventArgs(1, "AddRoute " + _rt.Material_Master__Reference));
        }
        foreach (PreliminaryDataRouteMarket _market in this.MarketTable)
        {
          _dictionary.AddMarket(_update, _market);
          _update(this, new ProgressChangedEventArgs(1, "AddMarket " + _market.Market));
        }
      }
    }
  }
}
