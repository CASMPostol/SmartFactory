using System;
using EntitiesProductType = CAS.SmartFactory.IPR.Entities.ProductType;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Material
  {
    internal string MaterialGroup { get; set; } //TODO must be replaced by the ProductID: http://itrserver/Bugs/BugDetail.aspx?bid=2854
    internal void GetProductType(EntitiesDataContext edc)
    {
      SKUCommonPart sku = SKUCommonPart.GetLookup(edc, this.SKU);
      if (sku != null)
      {
        this.ProductType = sku.ProductType;
        return;
      }
      Warehouse wrh = Warehouse.GetLookup(edc, this.Location);
      if (wrh != null)
        this.ProductType = wrh.ProductType;
    }
    internal string GetKey()
    {
      return String.Format(keyForam, SKU, Batch, Location);
    }
    private const string keyForam = "{0}:{1}:{2}";
  }
}
