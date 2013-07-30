using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class CutfillerCoefficient
  {
    #region public
    /// <summary>
    /// Gets the lookup to the <see cref="CutfillerCoefficient"/>.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    /// <exception cref="IPRDataConsistencyException">CutfillerCoefficient lookup error</exception>
    public static CutfillerCoefficient GetLookup( Entities edc )
    {
      try
      {
        return (from idx in edc.CutfillerCoefficient orderby idx.Version select idx).First();
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
