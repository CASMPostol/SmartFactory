using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Dust
  {
    internal static Dust GetLookup(ProductType type, EntitiesDataContext edc)
    {
      Dust value = null;
      try
      {
        value = (from idx in edc.Dust where idx.ProductType == type select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
        return value;
      }
      catch (ArgumentNullException)
      {
        value = new Dust() { DustRatio = 0, ProductType = type, Tytuł = type.ToString() }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }
  }
}
