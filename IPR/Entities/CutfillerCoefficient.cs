using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class CutfillerCoefficient
  {
    internal static CutfillerCoefficient GetLookup(EntitiesDataContext edc)
    {
      CutfillerCoefficient value = null;
      value = (from idx in edc.CutfillerCoefficient orderby idx.Wersja select idx).First();
      return value;
    }
    internal static void ImportData(ConfigurationCutfillerCoefficientItem[] configuration, EntitiesDataContext edc)
    {
      List<CutfillerCoefficient> list = new List<CutfillerCoefficient>();
      foreach (ConfigurationCutfillerCoefficientItem item in configuration)
      {
        CutfillerCoefficient cc = new CutfillerCoefficient
        {
          Batch = null,
          CFTProductivityRateMax = item.CFTProductivityRateMax,
          CFTProductivityRateMin = item.CFTProductivityRateMin,
           Tytuł = item.Title
        };
        list.Add(cc);
      };
      edc.CutfillerCoefficient.InsertAllOnSubmit(list);
    }
  }
}
