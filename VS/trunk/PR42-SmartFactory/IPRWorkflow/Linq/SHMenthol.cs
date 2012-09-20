﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SHMenthol
  {
    internal static SHMenthol GetLookup(ProductType type, Entities edc)
    {
      try
      {
        return (from idx in edc.SHMenthol where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, m_Message, ex, "SHMenthol lookup error");
      }
    }
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
    #region private
    private const string m_Source = "SHMenthol";
    private const string m_Message = "I cannot find any SHMenthol coefficient";
    #endregion
  }
}
