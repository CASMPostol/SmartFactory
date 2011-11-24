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
      SHMenthol value = null;
      try
      {
        value = (from idx in edc.SHMenthol where idx.ProductType == type select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
        return value;
      }
      catch (ArgumentNullException)
      {
        value = new SHMenthol() { SHMentholRatio = 0, ProductType = type, Tytuł = type.ToString() }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }

  }
}
