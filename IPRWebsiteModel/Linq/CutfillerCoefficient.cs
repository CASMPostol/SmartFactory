using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class CutfillerCoefficient
  {
    #region public
    internal static CutfillerCoefficient GetLookup(Entities edc)
    {
      try
      {
        return (from idx in edc.CutfillerCoefficient orderby idx.Wersja select idx).First();
      }
      catch (System.Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, m_Message, ex, "CutfillerCoefficient lookup error");
      }
    }
    #endregion

    #region private
    private const string m_Source = "Cutfiller Coefficient";
    private const string m_Message = "I cannot find any cutfiller coefficient"; 
    #endregion
  }
}
