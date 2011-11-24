using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class CutfillerCoefficient
  {
    internal static CutfillerCoefficient GetLookup(EntitiesDataContext edc)
    {
      CutfillerCoefficient value = null;
      try
      {
        value = (from idx in edc.CutfillerCoefficient select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
        return value;
      }
      catch (ArgumentNullException)
      {
        value = new CutfillerCoefficient()
        {
          CFTProductivityNormMax = 0,
          CFTProductivityNormMin = 0,
          CFTProductivityRateMax = 0,
          CFTProductivityRateMin = 0,
          Tytuł = "Preliminary"
        }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }
  }
}
