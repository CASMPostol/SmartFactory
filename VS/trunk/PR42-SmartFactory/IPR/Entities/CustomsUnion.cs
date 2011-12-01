using System;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class CustomsUnion
  {
    internal static bool CheckIfUnion(string primeMarket, EntitiesDataContext edc)
    {
      return (from item in edc.CustomsUnion where item.EUPrimeMarket.Contains(primeMarket) select item).Any();
    }
    internal static void ImportData(ConfigurationCustomsUnionItem[] configuration, EntitiesDataContext edc)
    {
      List<CustomsUnion> list = new List<CustomsUnion>();
      foreach (ConfigurationCustomsUnionItem item in configuration)
      {
        CustomsUnion csu = new CustomsUnion
        {
          EUPrimeMarket = item.EUPrimeMarket,
          Tytuł = item.Title
        };
        list.Add(csu);
      };
      edc.CustomsUnion.InsertAllOnSubmit(list);
    }
  }
}
