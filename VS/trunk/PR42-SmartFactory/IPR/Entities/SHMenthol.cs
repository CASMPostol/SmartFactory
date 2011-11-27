using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SHMenthol
  {
    internal static SHMenthol GetLookup(ProductType type, EntitiesDataContext edc)
    {
      SHMenthol value = (from idx in edc.SHMenthol where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      return value;
    }
  }
}
