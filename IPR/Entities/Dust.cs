using System;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Dust
  {
    internal static Dust GetLookup(ProductType type, EntitiesDataContext edc)
    {
      Dust value = null;
      value = (from idx in edc.Dust where idx.ProductType == type orderby idx.Wersja select idx).First();
      return value;
    }
    internal static void ImportData(ConfigurationDustItem[] configuration, EntitiesDataContext edc)
    {
      List<Dust> list = new List<Dust>();
      foreach (ConfigurationDustItem item in configuration)
      {
        Dust dst = new Dust
        {
          Batch = null,
          DustRatio = item.DustRatio,
          ProductType = item.ProductType.ParseProductType(),
          Tytuł = "Unawailable in the source data"
        };
        list.Add(dst);
      };
      edc.Dust.InsertAllOnSubmit(list);
    }
  }
}
