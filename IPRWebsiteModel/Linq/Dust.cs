using System;
using System.Linq;
using System.Collections.Generic;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Dust
  {
    public static Dust GetLookup( ProductType type, Entities edc )
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
    #region private
    private const string m_Source = "Dust";
    private const string m_Message = "I cannot find any dust coefficient";
    #endregion
  }
}
