using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Material
  {
    #region public
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location. <see cref="GetKey"/>
    /// </summary>
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
        if (value.ProductType == Entities.ProductType.IPRTobacco || value.ProductType == Entities.ProductType.Tobacco)
          TotalTobacco += value.TobaccoQuantity.GetValueOrDefault(0);
        if (this.TryGetValue(value.GetKey(), out ce))
        {
          ce.FGQuantity += value.FGQuantity;
          ce.TobaccoQuantity += value.TobaccoQuantity;
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
      internal void ProcessDisposals(EntitiesDataContext _edc, Batch _parent, double _overusageCoefficient )
      {
         if ( Product == null)
        throw new IPRDataConsistencyException("Material.ProcessDisposals", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized.");
        InsertAllOnSubmit(_edc, _parent);
        foreach (Material _midx in this.Values)
        {
          List<IPR> _accounts = (from IPR _iprx in _edc.IPR orderby _iprx.Identyfikator descending select _iprx).ToList<IPR>();
        }
        //TODO to be implemented: http://itrserver/Bugs/BugDetail.aspx?bid=2869
      }
      internal IEnumerable<Material> GeContentEnumerator()
      {
        return this.Values;
      }
      internal void CheckConsistence()
      {
        if (Product == null)
          throw new IPRDataConsistencyException("Processing disposals", "Unrecognized finisched good", null, "CheckConsistence error");
      }
      private void InsertAllOnSubmit(EntitiesDataContext edc, Batch parent)
      {
        foreach (var item in Values)
          item.BatchLookup = parent;
        edc.Material.InsertAllOnSubmit(GeContentEnumerator());
      }
    } //SummaryContentInfo
    internal static SummaryContentInfo GetXmlContent(BatchMaterialXml[] xml, EntitiesDataContext edc, ProgressChangedEventHandler progressChanged)
    {
      SummaryContentInfo itemsList = new SummaryContentInfo();
      foreach (BatchMaterialXml item in xml)
      {
        Material newMaterial = new Material(item);
        newMaterial.GetProductType(edc);
        progressChanged(null, new ProgressChangedEventArgs(1, String.Format("SKU={0}", newMaterial.SKU)));
        itemsList.Add(newMaterial);
      }
      itemsList.CheckConsistence();
      return itemsList;
    }
    internal SKUCommonPart SKULookup { get; set; }
    internal bool IPRMaterial { get; set; }
    #endregion

    #region private
    private Material(BatchMaterialXml item)
      : this()
    {
      Batch = item.Batch;
      BatchLookup = null;
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
      return String.Format(m_keyForam, SKU, Batch, Location);
    }
    private void GetProductType(EntitiesDataContext edc)
    {
      EntitiesDataContext.ProductDescription product = edc.GetProductType(this.SKU, this.Location);
      this.ProductType = product.productType;
      this.SKULookup = product.skuLookup;
      this.IPRMaterial = product.IPRMaterial;
    }
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion
  }
}
