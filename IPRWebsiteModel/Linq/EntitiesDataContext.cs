using System;
using Microsoft.SharePoint.Linq;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Entities
  {
    internal class ProductDescription
    {
      internal ProductType productType;
      internal bool IPRMaterial;
      internal Linq.IPR.SKUCommonPart skuLookup;
      internal ProductDescription(ProductType type, bool ipr, SKUCommonPart lookup)
      {
        productType = type;
        IPRMaterial = ipr;
        skuLookup = lookup;
      }
      internal ProductDescription(ProductType type, bool ipr)
        : this(type, ipr, null)
      { }
    }
    internal ProductDescription GetProductType(string sku, string location)
    {
      SKUCommonPart entity = SKUCommonPart.Find(this, sku);
      if (entity != null)
        return new ProductDescription(entity.ProductType.GetValueOrDefault(ProductType.Other), entity.IPRMaterial.GetValueOrDefault(false), entity);
      Warehouse wrh = Linq.IPR.Warehouse.Find( this, location );
      if (wrh != null)
      {
        switch (wrh.ProductType)
        {
          case ProductType.Tobacco:
            return new ProductDescription(ProductType.Tobacco, false);
          case ProductType.IPRTobacco:
            return new ProductDescription(ProductType.IPRTobacco, true);
          case ProductType.Invalid:
          case ProductType.Cutfiller:
          case ProductType.Cigarette:
          case ProductType.Other:
          default:
            break;
        }
      }
      return new ProductDescription(ProductType.Other, false);
      //throw new IPRDataConsistencyException(m_Source, String.Format(m_WrongProductTypeMessage, entity, location), null);
    }
    private const string m_Source = "Data Context";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
  }
}

