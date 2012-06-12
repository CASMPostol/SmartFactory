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
      try
      {
        return (from idx in edc.Dust where idx.ProductType == type orderby idx.Wersja select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, m_Message, ex, "Cannot find Dust");
      }
    }
    internal static void ImportData(ConfigurationDustItem[] configuration, EntitiesDataContext edc)
    {
      List<Dust> list = new List<Dust>();
      foreach (ConfigurationDustItem item in configuration)
      {
        Dust dst = new Dust
        {
          DustRatio = item.DustRatio,
          ProductType = item.ProductType.ParseProductType(),
        };
        list.Add(dst);
      };
      edc.Dust.InsertAllOnSubmit(list);
    }
    #region private
    private const string m_Source = "Dust";
    private const string m_Message = "I cannot find any dust coefficient";
    #endregion
  }
}
