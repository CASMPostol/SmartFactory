using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SHMenthol
  {
    internal static SHMenthol GetLookup(ProductType type, EntitiesDataContext edc)
    {
      SHMenthol value = (from idx in edc.SHMenthol where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      return value;
    }
    internal static void ImportData(ConfigurationSHMentholItem[] configuration, EntitiesDataContext edc)
    {
      List<SHMenthol> list = new List<SHMenthol>();
      foreach (ConfigurationSHMentholItem item in configuration)
      {
        SHMenthol shm = new SHMenthol
        {
          ProductType = item.ProductType.ParseProductType(),
          SHMentholRatio = item.SHMentholRatio,
        };
        list.Add(shm);
      };
      edc.SHMenthol.InsertAllOnSubmit(list);
    }
  }
}
