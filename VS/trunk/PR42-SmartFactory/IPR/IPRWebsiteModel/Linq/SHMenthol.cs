using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class SHMenthol
  {
    public static SHMenthol GetLookup( ProductType type, Entities edc )
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
    #region private
    private const string m_Source = "SHMenthol";
    private const string m_Message = "I cannot find any SHMenthol coefficient";
    #endregion
  }
}
