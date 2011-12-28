using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Warehouse
  {
    internal class WarehouseItem
    {
      public string Title { get; set; }
      public string ID { get; set; }
    }
    internal static IQueryable<Warehouse> GatAll(EntitiesDataContext edc)
    {
      return from _idx in edc.Warehouse
             orderby _idx.Tytuł ascending
             select _idx;//new WarehouseItem() { ID = _idx.Identyfikator.Value.ToString(), Title = _idx.Tytuł };
    }
  }
}
