using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Waste
  {
    internal static Waste GetLookup(ProductType type, EntitiesDataContext edc)
    {
      Waste value = null;
      value = (from idx in edc.Waste where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      return value;
    }
    internal static void ImportData(ConfigurationWasteItem[] configuration, EntitiesDataContext edc)
    {
      List<Waste> list = new List<Waste>();
      foreach (ConfigurationWasteItem item in configuration)
      {
        Waste wst = new Waste
        {
          Batch = null,
          ProductType = item.ProductType.ParseProductType(),
          Tytuł = "Not in the source data", //TODO to be removed
          WasteRatio = item.WasteRatio
        };
        list.Add(wst);
      };
      edc.Waste.InsertAllOnSubmit(list);
    }
  }
}
