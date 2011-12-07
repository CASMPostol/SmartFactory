using System;
using System.Collections.Generic;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Material
  {
    #region public
    internal class SummaryContentInfo : SortedList<string, Material>
    {
      public SummaryContentInfo() { }
      public Material Product { get; private set; }
      public double TotalTobacco { get; private set; }
      /// <summary>
      /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList<TKey,TValue>.
      /// </summary>
      /// <param name="key">The key of the element to add.</param>
      /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
      /// <exception cref="System.ArgumentNullException">key is null</exception>
      /// <exception cref="System.ArgumentException">An element with the same key already exists in the <paramref name="System.Collections.Generic.SortedList<TKey,TValue>"/>.</exception>
      public void Add(Material value)
      {
        Material ce = null;
        if (this.TryGetValue(value.GetKey(), out ce))
        {
          ce.FGQuantity = ce.FGQuantity + value.FGQuantity;
          ce.TobaccoQuantity = ce.TobaccoQuantity + value.TobaccoQuantity;
          if (value.ProductType == Entities.ProductType.IPRTobacco || value.ProductType == Entities.ProductType.Tobacco)
            TotalTobacco = TotalTobacco + value.TobaccoQuantity.GetValueOrDefault(0);
        }
        else
        {
          if (value.ProductType == Entities.ProductType.Cigarette)
            Product = value;
          else if (Product == null && value.ProductType == Entities.ProductType.Cutfiller)
            Product = value;
          base.Add(value.GetKey(), value);
        }
      }
      internal double ProcessDisposals()
      {
        //TODO to be implemented: http://itrserver/Bugs/BugDetail.aspx?bid=2869
        return 0;
      }
      internal IEnumerable<Material> GeContentEnumerator()
      {
        return this.Values;
      }
    }
    internal static SummaryContentInfo GetXmlContent(BatchMaterialXml[] xml, EntitiesDataContext edc, Batch parent)
    {
      SummaryContentInfo itemsList = new SummaryContentInfo();
      foreach (BatchMaterialXml item in xml)
      {
        Material newMaterial = new Material(parent, item);
        newMaterial.GetProductType(edc);
        itemsList.Add(newMaterial);
      }
      edc.Material.InsertAllOnSubmit(itemsList.GeContentEnumerator());
      return itemsList;
    }
    internal SKUCommonPart SKULookup { get; set; }
    internal bool IPRMaterial { get; set; }
    #endregion

    #region private
    private Material(Batch parent, BatchMaterialXml item)
      : this()
    {
      Batch = item.Batch;
      BatchLookup = parent;
      SKU = item.Material;
      Location = item.Stor__Loc;
      SKUDescription = item.Material_description;
      Tytuł = item.Material_description;
      Units = item.Unit;
      FGQuantity = Convert.ToDouble(item.Quantity);
      TobaccoQuantity = Convert.ToDouble(item.Quantity_calculated);
      ProductType = Entities.ProductType.Invalid;
      ProductID = item.material_group;
    }
    private string GetKey()
    {
      return String.Format(keyForam, SKU, Batch, Location);
    }
    private void GetProductType(EntitiesDataContext edc)
    {
      EntitiesDataContext.ProductDescription product = edc.GetProductType(this.SKU, this.Location);
      this.ProductType = product.productType;
      this.SKULookup = product.skuLookup;
      this.IPRMaterial = product.IPRMaterial;
    }
    private const string keyForam = "{0}:{1}:{2}";
    #endregion
  }
}
