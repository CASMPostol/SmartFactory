using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKUCommonPart
  {
    internal static SKUCommonPart GetLookup(EntitiesDataContext edc, string index)
    {
      SKUCommonPart newSKU = null;
      try
      {
        newSKU = (from item in edc.SKU where item.SKU.Contains(index) select item).First<SKUCommonPart>();
      }
      catch (Exception) { }
      return newSKU;
    }
  }
}
