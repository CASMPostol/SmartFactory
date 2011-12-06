using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class StockEntry
  {
    public StockEntry(StockXmlRow xml, Stock parent)
      : this()
    {
      StockListLookup = parent;
      Batch = xml.Batch;
      Blocked = xml.Blocked;
      InQualityInsp = xml.InQualityInsp;
      IPRType = false;
      Location = xml.SLoc;
      RestrictedUse = xml.RestrictedUse;
      SKU = xml.Material;
      Tytuł = xml.MaterialDescription;
      Units = xml.BUn;
      Unrestricted = xml.Unrestricted;
      BatchLookup = null;
      ProductType = Entities.ProductType.Invalid;
      Quantity = xml.Blocked.GetValueOrDefault(0) + xml.InQualityInsp.GetValueOrDefault(0) + xml.RestrictedUse.GetValueOrDefault(0) + xml.Unrestricted.GetValueOrDefault(0);
    }
    internal void ProcessEntry(EntitiesDataContext edc)
    {
      GetProductType(edc);
      GetBatchLookup(edc);
    }
    private void GetProductType(EntitiesDataContext edc)
    {
      SKUCommonPart sku = SKUCommonPart.GetLookup(edc, SKU);
      if (sku != null)
      {
        this.ProductType = sku.ProductType;
        this.IPRType = sku.IPRMaterial;
        return;
      };
      Warehouse wrh = Warehouse.GetLookup(edc, this.Location);
      if (wrh != null)
      {
        this.ProductType = wrh.ProductType;
        this.IPRType = Entities.ProductType.IPRTobacco == wrh.ProductType.Value;
        return;
      }
      throw new IPRDataConsistencyException(m_Source, String.Format(m_WrongProductTypeMessage, this.SKU, this.Location), null);
    }
    private void GetBatchLookup(EntitiesDataContext edc)
    {
      if (ProductType != Entities.ProductType.Cigarette || ProductType != Entities.ProductType.Cutfiller)
        return;
      if (!IPRType.GetValueOrDefault(false))
        return;
      BatchLookup = Entities.Batch.GetCreateLookup(edc, this.Batch);
    }
    private const string m_Source = "Stock Entry";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
  }
}
