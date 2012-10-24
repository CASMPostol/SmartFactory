using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class CustomsUnionExtension
  {
    internal static void ImportData(ConfigurationCustomsUnionItem[] configuration, Entities edc)
    {
      List<CustomsUnion> list = new List<CustomsUnion>();
      foreach (ConfigurationCustomsUnionItem item in configuration)
      {
        CustomsUnion csu = new CustomsUnion
        {
          EUPrimeMarket = item.EUPrimeMarket,
          Title = item.Title
        };
        list.Add(csu);
      };
      edc.CustomsUnion.InsertAllOnSubmit(list);
    }
  }
}
