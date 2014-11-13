//<summary>
//  Title   : Summary Batch Content Info
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.Customs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Summary Batch Content Info
  /// </summary>
  public abstract class SummaryContentInfo : SortedList<string, Material>
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SummaryContentInfo" /> class.
    /// </summary>
    public SummaryContentInfo(BatchStatus batchStatus)
    {
      this.BatchStatus = batchStatus;
      AccumulatedDisposalsAnalisis = new DisposalsAnalisis();
    }
    #endregion

    #region public
    /// <summary>
    /// Gets the batch status.
    /// </summary>
    /// <value>
    /// The batch status <see cref="BatchStatus"/>.
    /// </value>
    public BatchStatus BatchStatus { get; private set; }
    /// <summary>
    /// Gets the product.
    /// </summary>
    /// <value>
    /// The product.
    /// </value>
    public Material Product { get; private set; }
    /// <summary>
    /// Validates this instance.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="disposals">The list of disposals created already.</param>
    /// <exception cref="InputDataValidationException">Batch content validate failed;XML content validation</exception>
    public void Validate(Entities edc, IEnumerable<Disposal> disposals)
    {
      ErrorsList _validationErrors = new ErrorsList();
      if (Product == null)
        _validationErrors.Add(new Warnning("Unrecognized finished good", true));
      SKULookup = SKUCommonPart.Find(edc, Product.SKU);
      if (SKULookup == null)
      {
        string _msg = "Cannot find finished good SKU={0} in the SKU dictionary - dictionary update is required";
        _validationErrors.Add(new Warnning(String.Format(_msg, Product.SKU), true));
      }
      if (Product.ProductType != ProductType.Cigarette)
        return;
      Dictionary<string, IGrouping<string, Disposal>> _disposalGroups = (from _mx in disposals.ToList<Disposal>()
                                                                         group _mx by _mx.Disposal2IPRIndex.Batch).ToDictionary<IGrouping<string, Disposal>, string>(k => k.Key);
      Dictionary<string, decimal> _materials = new Dictionary<string, decimal>();
      foreach (IGrouping<string, Disposal> _groupX in _disposalGroups.Values)
        _materials.Add(_groupX.Key, _groupX.Sum<Disposal>(x => x.SettledQuantityDec));
      foreach (Material _materialX in this.Values)
      {
        if (_materialX.ProductType.Value != Linq.ProductType.IPRTobacco)
          continue;
        decimal _disposed = 0;
        if (_materials.ContainsKey(_materialX.Batch))
          _disposed = _materials[_materialX.Batch];
        decimal _diff = _materialX.Accounts2Dispose.Sum<IPR>(a => a.TobaccoNotAllocatedDec) + _disposed - _materialX.TobaccoQuantityDec;
        if (_diff < -Settings.MinimalOveruse)
        {
          string _mssg = "Cannot find any IPR account to dispose the quantity {3}: Tobacco batch: {0}, fg batch: {1}, quantity to dispose: {2} kg";
          _validationErrors.Add(new Warnning(String.Format(_mssg, _materialX.Batch, Product.Batch, _materialX.TobaccoQuantityDec, -_diff), true));
        }
      }
      if (_validationErrors.Count > 0)
        throw new InputDataValidationException("Batch content validation failed", "XML content validation", _validationErrors);
    }
    /// <summary>
    /// Gets the SKU lookup.
    /// </summary>
    /// <value>
    /// The SKU lookup.
    /// </value>
    internal SKUCommonPart SKULookup { get; private set; }
    /// <summary>
    /// Gets the total quantity of the tobacco.
    /// </summary>
    /// <value>
    /// The total tobacco.
    /// </value>
    internal double TotalTobacco
    {
      get { return Convert.ToDouble(myTotalTobacco); }
    }
    internal double CalculatedOveruse { get; private set; }
    internal void Analyze(Entities edc, Batch parent, ProgressChangedEventHandler progressChanged, Material.Ratios materialRatios)
    {
      progressChanged(this, new ProgressChangedEventArgs(1, "Analyze: ProcessMaterials"));
      this.ReplaceMaterials(edc, parent, materialRatios, progressChanged);
      progressChanged(this, new ProgressChangedEventArgs(1, "Analyze: AdjustMaterialQuantity"));
      List<Material> _tobacco = this.Values.Where<Material>(x => x.ProductType.Value == ProductType.IPRTobacco || x.ProductType.Value == ProductType.Tobacco).ToList<Material>();
      List<Material> _IPRtobacco = _tobacco.Where<Material>(x => x.ProductType.Value == ProductType.IPRTobacco).ToList<Material>();
      if (this.Product.ProductType.Value == ProductType.Cigarette && this.BatchStatus == Linq.BatchStatus.Final)
        foreach (Material _mx in _IPRtobacco)
          _mx.AdjustTobaccoQuantity(ref myTotalTobacco, progressChanged);
      progressChanged(this, new ProgressChangedEventArgs(1, "Analyze: GetOverusage"));
      this.GetOverusage(parent.UsageMax.Value, parent.UsageMin.Value);
      if (this.BatchStatus == Linq.BatchStatus.Progress)
        return;
      progressChanged(this, new ProgressChangedEventArgs(1, "Analyze: CalculateOveruse"));
      foreach (Material _mx in _IPRtobacco)
        _mx.CalculateOveruse(edc, materialRatios, CalculatedOveruse);
      this.AdjustOveruse(materialRatios, _IPRtobacco);
      progressChanged(this, new ProgressChangedEventArgs(1, "Analyze: AccumulatedDisposalsAnalisis"));
      foreach (Material _mx in _tobacco)
      {
        _mx.CalculateCompensationComponents(materialRatios);
        AccumulatedDisposalsAnalisis.Accumutate(_mx);
      }
      foreach (InvoiceContent _ix in parent.InvoiceContent(edc))
        _ix.UpdateExportedDisposals(edc);
      this.UpdateNotStartedDisposals(edc, progressChanged);
    }
    internal double this[DisposalEnum index]
    {
      get { return Convert.ToDouble(AccumulatedDisposalsAnalisis[index]); }
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the accumulated disposals analysis.
    /// </summary>
    /// <value>
    /// The accumulated disposals analysis.
    /// </value>
    private DisposalsAnalisis AccumulatedDisposalsAnalisis { get; set; }
    private void ReplaceMaterials(Entities entities, Batch parent, Material.Ratios materialRatios, ProgressChangedEventHandler progressChanged)
    {
      if (Product == null)
        throw new IPRDataConsistencyException("SummaryContentInfo.ProcessMaterials", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized.");
      try
      {
        List<Material> _newMaterialList = new List<Material>();
        List<Material> _oldMaterialList = new List<Material>();
        List<Material> _copyThis = new List<Material>();
        _copyThis.AddRange(this.Values);
        Dictionary<string, Material> _parentsMaterials = parent.Material(entities).ToDictionary<Material, string>(x => x.GetKey());
        progressChanged(this, new ProgressChangedEventArgs(1, "ProcessMaterials: ReplaceByExistingOne"));
        foreach (Material _materialX in _copyThis)
          _materialX.ReplaceByExistingOne(_oldMaterialList, _newMaterialList, _parentsMaterials, parent);
        progressChanged(this, new ProgressChangedEventArgs(1, "ProcessMaterials: InsertAllOnSubmit"));
        if (_newMaterialList.Count > 0)
          entities.Material.InsertAllOnSubmit(_newMaterialList);
        foreach (Material _omx in _oldMaterialList)
        {
          this.Remove(_omx.GetKey());
          this.Add(_omx.GetKey(), _omx);
        }
      }
      catch (Exception _ex)
      {
        throw new IPRDataConsistencyException("SummaryContentInfo.ProcessMaterials", _ex.Message, _ex, "Disposal processing error");
      }
    }
    private void UpdateNotStartedDisposals(Entities edc, ProgressChangedEventHandler progressChanged)
    {
      progressChanged(this, new ProgressChangedEventArgs(1, "UpdateDisposals"));
      foreach (Material _materialX in this.Values)
        _materialX.UpdateDisposals(edc, progressChanged);
    }
    private void AdjustOveruse(Material.Ratios materialRatios, List<Material> _IPRTobacco)
    {
      if (CalculatedOveruse <= 0)
        return;
      decimal _2remove = 0;
      List<Material> _2Add = new List<Material>();
      foreach (Material _mx in _IPRTobacco)
        _2remove += _mx.RemoveOveruseIfPossible(_2Add, materialRatios);
      if (_2Add.Count == 0)
        _2Add.Add(_IPRTobacco.Max<Material, Material>(x => x));
      decimal _AddingCff = _2remove / _2Add.Sum<Material>(x => x.TobaccoQuantityDec);
      foreach (Material _mx in _2Add)
        _mx.IncreaseOveruse(_AddingCff, materialRatios);
    }
    private void GetOverusage(double usageMax, double usageMin)
    {
      double _fGQuantity = Product.FGQuantity.Value;
      double _ret = (this.TotalTobacco - _fGQuantity * usageMax / 1000);
      if (_ret > 0)
      {
        CalculatedOveruse = _ret / this.TotalTobacco; // Over-usage
        return;
      }
      _ret = (this.TotalTobacco - _fGQuantity * usageMin / 1000);
      if (_ret < 0)
      {
        CalculatedOveruse = _ret / this.TotalTobacco; //Under-usage
        return;
      }
      CalculatedOveruse = 0;
    }
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location.
    /// </summary>
    private class DisposalsAnalisis : SortedList<DisposalEnum, decimal>
    {
      internal DisposalsAnalisis()
      {
        foreach (Linq.DisposalEnum _item in Enum.GetValues(typeof(WebsiteModel.Linq.DisposalEnum)))
          this.Add(_item, 0);
      }
      internal void Accumutate(Material material)
      {
        if (material.ProductType.Value != ProductType.IPRTobacco)
          return;
        foreach (Linq.DisposalEnum _item in Enum.GetValues(typeof(WebsiteModel.Linq.DisposalEnum)))
        {
          if (_item == DisposalEnum.Tobacco || _item == DisposalEnum.Cartons)
            continue;
          this[_item] += material[_item];
        }
      }
    } //DisposalsAnalisis
    private decimal myTotalTobacco = 0;
    /// <summary>
    /// Adds an element with the specified key and value.
    /// </summary>
    /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
    /// <exception cref="System.ArgumentNullException">key is null</exception>
    /// <exception cref="System.ArgumentException">An element with the same key as the <paramref name="value" /> that already exists in the collection.</exception>
    protected void Add(Material value)
    {
      Material ce = null;
      if (value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco)
        myTotalTobacco += value.TobaccoQuantityDec;
      if (this.TryGetValue(value.GetKey(), out ce))
      {
        ce.FGQuantity += value.FGQuantity;
        ce.TobaccoQuantity += value.TobaccoQuantity;
      }
      else
      {
        if (value.ProductType == ProductType.Cigarette)
          Product = value;
        else if (Product == null && value.ProductType == ProductType.Cutfiller)
          Product = value;
        base.Add(value.GetKey(), value);
      }
    }
    private void Subtract(Material value, List<string> _warnings)
    {
      if (value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco)
        myTotalTobacco -= value.TobaccoQuantityDec;
      if (this.TryGetValue(value.GetKey(), out value))
      {
        value.FGQuantity -= value.FGQuantity;
        value.TobaccoQuantity -= value.TobaccoQuantity;
      }
      else
        _warnings.Add(String.Format("Cannot find material {0} to subtract", value.ToString()));
    }
    private void InsertAllOnSubmit(Entities entities, Batch parent)
    {
      foreach (Material item in Values)
        item.Material2BatchIndex = parent;
      entities.Material.InsertAllOnSubmit(this.Values);
    }
    #endregion

  }//SummaryContentInfo
}
