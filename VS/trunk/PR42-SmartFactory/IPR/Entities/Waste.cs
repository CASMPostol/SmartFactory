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
      value = (from idx in edc.Waste where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      return value;
    }
  }
}
