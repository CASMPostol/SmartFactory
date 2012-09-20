using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class StockEntry
  {
    public StockEntry(StockXmlRow xml, Stock parent)
      : this()
    {
      StockIndex = parent;
      Batch = xml.Batch;
      Blocked = xml.Blocked;
      InQualityInsp = xml.InQualityInsp;
      IPRType = false;
      this.StorLoc = xml.SLoc;
      RestrictedUse = xml.RestrictedUse;
      SKU = xml.Material;
      Title = xml.MaterialDescription;
      Units = xml.BUn;
      Unrestricted = xml.Unrestricted;
      BatchIndex = null;
      ProductType = Linq.IPR.ProductType.Invalid;
      Quantity = xml.Blocked.GetValueOrDefault(0) + xml.InQualityInsp.GetValueOrDefault(0) + xml.RestrictedUse.GetValueOrDefault(0) + xml.Unrestricted.GetValueOrDefault(0);
    }
    internal void ProcessEntry(Entities edc)
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
      if (ProductType != Linq.IPR.ProductType.Cigarette && ProductType != Linq.IPR.ProductType.Cutfiller)
        return;
      if (!IPRType.GetValueOrDefault(false))
        return;
      this.BatchIndex = Linq.IPR.Batch.GetOrCreatePreliminary(edc, this.Batch);
    }
    private const string m_Source = "Stock Entry";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
  }
}
