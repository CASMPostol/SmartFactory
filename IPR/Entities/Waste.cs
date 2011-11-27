using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Waste
  {
    internal static Waste GetLookup(ProductType type, EntitiesDataContext edc)
    {
      Waste value = null;
      try
      {
        value = (from idx in edc.Waste where idx.ProductType == type select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
      }
      catch (ArgumentNullException)
      {
        value = new Waste() {  WasteRatio = 0, ProductType = type, Tytuł = type.ToString() }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }
  }
}
