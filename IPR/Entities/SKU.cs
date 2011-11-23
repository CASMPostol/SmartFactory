using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKU
  {
    internal static SKU GetLookup(EntitiesDataContext edc, string index)
    {
      SKU newSKU = null;
      try
      {
        newSKU = (from item in edc.SKU where item.SKU0.Contains(index) select item).First<SKU>();
      }
      catch (Exception)
      {
        newSKU = new SKU()
        {
          SKU0 = index,
          ProductType = Entities.ProductType.Invalid,
          SKUDescription = "Preliminary - to be replaced",
          SKULibraryLookup = null,
          Tytuł = index,
          FormatLookupTitle = "****", //TODO wrond column definition
          FormatLookupIdentyfikator = int.MaxValue //TODO ????? What is this.
        };
        edc.SKU.InsertOnSubmit(newSKU);
      }
      return newSKU;
    }
  }
}
