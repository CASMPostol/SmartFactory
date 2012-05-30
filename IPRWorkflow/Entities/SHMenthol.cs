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
      try
      {
        return (from idx in edc.SHMenthol where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, m_Message, ex);
      }
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
    #region private
    private const string m_Source = "SHMenthol";
    private const string m_Message = "I cannot find any SHMenthol coefficient";
    #endregion
  }
}
