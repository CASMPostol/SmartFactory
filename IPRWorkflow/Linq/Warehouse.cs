using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class WarehouseExtension
  {
    internal static Warehouse Find(Entities edc, string index)
    {
      Warehouse newWarehouse = null;
      try
      {
        newWarehouse = (from Warehouse item in edc.Warehouse where item.Title.Contains(index) select item).First<Warehouse>();
      }
      catch (Exception) { }
      return newWarehouse;
    }
    internal static void ImportData(ConfigurationWarehouseItem[] configuration, Entities edc)
    {
      List<Warehouse> list = new List<Warehouse>();
      foreach (ConfigurationWarehouseItem item in configuration)
      {
        Warehouse wrh = new Warehouse
        {
          External = item.External,
          ProductType = item.ProductType.ParseProductType(),
          Title = item.Title
        };
        list.Add(wrh);
      };
      edc.Warehouse.InsertAllOnSubmit(list);
    }
  }
}
