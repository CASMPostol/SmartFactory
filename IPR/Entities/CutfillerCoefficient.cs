using System;
using System.Linq;

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
  }
}
