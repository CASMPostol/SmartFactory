using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockEntry
  {
    public void ProcessEntry( Entities edc )
    {
      GetProductType(edc);
      GetBatchLookup(edc);
    }
    private void GetProductType(Entities edc)
    {
      Entities.ProductDescription product = edc.GetProductType(this.SKU, this.StorLoc);
      this.ProductType = product.productType;
      this.IPRType = product.IPRMaterial;
    }
    private void GetBatchLookup(Entities edc)
    {
      if (ProductType != Linq.ProductType.Cigarette && ProductType != Linq.ProductType.Cutfiller)
        return;
      if (!IPRType.GetValueOrDefault(false))
        return;
      this.BatchIndex = Linq.Batch.GetOrCreatePreliminary(edc, this.Batch);
    }
  }
}
