using System.Collections.Generic;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SHMentholExtension
  {
    internal static void ImportData(ConfigurationSHMentholItem[] configuration, Entities edc)
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
