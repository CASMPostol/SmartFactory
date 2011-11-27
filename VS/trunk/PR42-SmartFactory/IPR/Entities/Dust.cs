using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Dust
  {
    internal static Dust GetLookup(ProductType type, EntitiesDataContext edc)
    {
      Dust value = null;
      value = (from idx in edc.Dust where idx.ProductType == type orderby idx.Wersja select idx).First();
      return value;
    }
  }
}
