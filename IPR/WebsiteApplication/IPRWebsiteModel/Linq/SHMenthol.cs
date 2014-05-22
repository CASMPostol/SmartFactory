using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class SHMenthol
  {
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    /// <exception cref="IPRDataConsistencyException">SHMenthol lookup error</exception>
    public static SHMenthol GetLookup( ProductType type, Entities edc )
    {
      try
      {
        return (from idx in edc.SHMenthol where idx.ProductType == type orderby idx.Version descending select idx).First();
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
