using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Warehouse
  {
    internal static Warehouse GetLookup(EntitiesDataContext edc, string index)
    {
      Warehouse newWarehouse = null;
      try
      {
        newWarehouse = (from Warehouse item in edc.Warehouse where item.Tytuł.Contains(index) select item).First<Warehouse>();
      }
      catch (Exception) { }
      return newWarehouse;
    }
    internal static void ImportData(ConfigurationWarehouseItem[] configuration, EntitiesDataContext edc)
    {
      List<Warehouse> list = new List<Warehouse>();
      foreach (ConfigurationWarehouseItem item in configuration)
      {
        Warehouse wrh = new Warehouse
        {
          External = item.External,
          ProductType = item.ProductType.ParseProductType(),
          Tytuł = item.Title
        };
        list.Add(wrh);
      };
      edc.Warehouse.InsertAllOnSubmit(list);
    }
  }
}
