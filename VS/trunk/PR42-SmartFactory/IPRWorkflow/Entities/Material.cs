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
      #region public
      internal Material Product { get; private set; }
      internal double TotalTobacco { get; private set; }
      /// <summary>
      /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList<TKey,TValue>.
      /// </summary>
      /// <param name="key">The key of the element to add.</param>
      /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
      /// <exception cref="System.ArgumentNullException">key is null</exception>
      /// <exception cref="System.ArgumentException">An element with the same key already exists in the <paramref name="System.Collections.Generic.SortedList<TKey,TValue>"/>.</exception>
      internal void Add(Material value)
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
      internal void ProcessDisposals(EntitiesDataContext _edc, Batch _parent, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusageCoefficient)
      {
        if (Product == null)
          throw new IPRDataConsistencyException("Material.ProcessDisposals", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized.");
        foreach (Material _midx in this.Values)
        {
          if (_midx.ProductType.Value != Entities.ProductType.IPRTobacco)
            continue;
          DisposalsAnalisis _dspsls = new DisposalsAnalisis(_midx.TobaccoQuantity.Value, _dustRatio, _shMentholRatio, _wasteRatio, _overusageCoefficient);
          foreach (var _item in _dspsls)
          {
            IPR _account = IPR.FindIPRAccount(_edc, _midx.Batch, _item.Value);
            _account.AddDisposal(_edc, _item, _parent);
          }
          _edc.SubmitChanges();
        }
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
      internal SummaryContentInfo(BatchMaterialXml[] xml, EntitiesDataContext edc, ProgressChangedEventHandler progressChanged)
      {
        foreach (BatchMaterialXml item in xml)
        {
          Material newMaterial = new Material(item);
          newMaterial.GetProductType(edc);
          progressChanged(null, new ProgressChangedEventArgs(1, String.Format("SKU={0}", newMaterial.SKU)));
          Add(newMaterial);
        }
        CheckConsistence();
      }
      #endregion

      #region private
      private void InsertAllOnSubmit(EntitiesDataContext edc, Batch parent)
      {
        foreach (var item in Values)
          item.BatchLookup = parent;
        edc.Material.InsertAllOnSubmit(GeContentEnumerator());
      }
      #endregion
    } //SummaryContentInfo
    internal class DisposalsAnalisis : SortedList<IPR.DisposalEnum, double>
    {
      internal DisposalsAnalisis(double _material, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusage)
      {
        this[IPR.DisposalEnum.Dust] = _material * _dustRatio;
        this[IPR.DisposalEnum.SHMenthol] = _material * _shMentholRatio;
        this[IPR.DisposalEnum.Waste] = _material * _wasteRatio;
        this[IPR.DisposalEnum.OverusageInKg] = _overusage > 0 ? _material * _overusage : 0;
        this[IPR.DisposalEnum.Tobacco] = _material - this[IPR.DisposalEnum.SHMenthol] - this[IPR.DisposalEnum.Waste] - this[IPR.DisposalEnum.Dust] - this[IPR.DisposalEnum.OverusageInKg];
      }
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
