using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class CutfillerCoefficient
  {
    #region public
    internal static CutfillerCoefficient GetLookup(EntitiesDataContext edc)
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
    internal static void ImportData(ConfigurationCutfillerCoefficientItem[] configuration, EntitiesDataContext edc)
    {
      List<CutfillerCoefficient> list = new List<CutfillerCoefficient>();
      foreach (ConfigurationCutfillerCoefficientItem item in configuration)
      {
        CutfillerCoefficient cc = new CutfillerCoefficient
        {
          CFTProductivityRateMax = item.CFTProductivityRateMax,
          CFTProductivityRateMin = item.CFTProductivityRateMin
        };
        list.Add(cc);
      };
      edc.CutfillerCoefficient.InsertAllOnSubmit(list);
    } 
    #endregion

    #region private
    private const string m_Source = "Cutfiller Coefficient";
    private const string m_Message = "I cannot find any cutfiller coefficient"; 
    #endregion
  }
}
