//<summary>
//  Title   : Batch entity partial class
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

using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Batch entity partial class
  /// </summary>
  public partial class Batch
  {
    #region public
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> instance.</param>
    /// <param name="batch">The index of the entry we are looking for.</param>
    /// <returns>
    /// The most recent <see cref="Batch" /> object.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindLookup(Entities edc, string batch)
    {
      return (from _batchX in edc.Batch
              where _batchX.Batch0.Contains(batch) && _batchX.BatchStatus != Linq.BatchStatus.Progress
              orderby _batchX.Id.Value
              select _batchX).FirstOrDefault();
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> instance.</param>
    /// <param name="batch">The batch.</param>
    /// <returns>
    /// The most recent <see cref="Batch" /> object.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindStockToBatchLookup(Entities edc, string batch)
    {
      return (from _batchX in edc.Batch
              where _batchX.Batch0.Contains(batch)
              orderby _batchX.Id.Value descending
              select _batchX).FirstOrDefault();
    }
    /// <summary>
    /// Batches the processing.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="contentInfo">The content info.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="progressChanged">The progress changed.</param>
    /// <param name="newBatch">if set to <c>true</c> it is new batch.</param>
    public void BatchProcessing(Entities edc, SummaryContentInfo contentInfo, BatchLib parent, ProgressChangedEventHandler progressChanged, bool newBatch)
    {
      BatchLibraryIndex = parent;
      progressChanged(this, new ProgressChangedEventArgs(1, "BatchProcessing: GetDependences"));
      Material.Ratios _mr = GetDependences(edc, contentInfo);
      contentInfo.Analyze(edc, this, progressChanged, _mr);
      AssignContentInfo(contentInfo, newBatch);
    }
    internal string DanglingBatchWarningMessage
    {
      get
      {
        string _msg = "Stock mismatch; the batch {0} is not reported on the stock. Correct your stock and try again.";
        return String.Format(_msg, this.Title);
      }
    }
    internal void GetInventory(Entities edc, StockDictionary balanceStock, StockDictionary.StockValueKey key, double quantityOnStock)
    {
      switch (this.BatchStatus.Value)
      {
        case Linq.BatchStatus.Progress:
          double _portion = quantityOnStock / this.FGQuantity.Value;
          foreach (Material _mtx in edc.Material.Where<Material>(x => x.Material2BatchIndex == this))
            _mtx.GetInventory(balanceStock, key, _portion);
          break;
        case Linq.BatchStatus.Intermediate:
        case Linq.BatchStatus.Final:
          break;
      }
    }
    internal void CheckQuantity(Entities edc, List<string> _warnings, StockLib lib)
    {
      if (this.ProductType.Value != Linq.ProductType.Cigarette)
        return;
      decimal _onStock = 0;
      foreach (StockEntry _stock in edc.StockEntry.Where<StockEntry>(x => x.BatchIndex == this))
      {
        if (_stock.StockLibraryIndex != lib)
          continue;
        _onStock += Convert.ToDecimal(_stock.Quantity);
      }
      if (Convert.ToDecimal(this.FGQuantityAvailable) != _onStock)
      {
        string _msg = string.Format(m_noMachingQuantityWarningMessage, Convert.ToDecimal(this.FGQuantityAvailable), _onStock, this.ProductType, this.Batch0, this.SKU);
        _warnings.Add(_msg);
      }
    }
    /// <summary>
    /// Reverse lookup to <see cref="Material"/>.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public IEnumerable<Material> Material(Entities entities)
    {
      return entities.Material.Where<Material>(x => x.Material2BatchIndex == this);
    }
    #endregion

    #region private
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be uploaded.";
    private string m_noMachingQuantityWarningMessage = "Inconsistent quantity batch: {0} / stock: {1} of the product: {2} batch: {3}/sku: {4} on the stock.";
    private Material.Ratios GetDependences(Entities edc, SummaryContentInfo contentInfo)
    {
      Linq.ProductType _producttype = contentInfo.Product.ProductType.Value;
      //Dependences
      Dust DustIndex = Linq.Dust.GetLookup(_producttype, edc);
      BatchDustCooeficiency = DustIndex.DustRatio;
      DustCooeficiencyVersion = DustIndex.Version;
      SHMenthol SHMentholIndex = Linq.SHMenthol.GetLookup(_producttype, edc);
      BatchSHCooeficiency = SHMentholIndex.SHMentholRatio;
      SHCooeficiencyVersion = SHMentholIndex.Version;
      Waste WasteIndex = Linq.Waste.GetLookup(_producttype, edc);
      BatchWasteCooeficiency = WasteIndex.WasteRatio;
      WasteCooeficiencyVersion = WasteIndex.Version;
      CutfillerCoefficient _cc = CutfillerCoefficient.GetLookup(edc);
      this.CFTProductivityNormMax = _cc.CFTProductivityNormMax;
      this.CFTProductivityNormMin = _cc.CFTProductivityNormMin;
      this.CFTProductivityRateMax = _cc.CFTProductivityRateMax;
      this.CFTProductivityRateMin = _cc.CFTProductivityRateMin;
      this.CFTProductivityVersion = _cc.Version;
      Usage _usage = Usage.GetLookup(contentInfo.SKULookup.FormatIndex, edc);
      this.CTFUsageMax = _usage.CTFUsageMax;
      this.CTFUsageMin = _usage.CTFUsageMin;
      this.UsageMin = _usage.UsageMin;
      this.UsageMax = _usage.UsageMax;
      this.UsageVersion = _usage.Version;
      double _shmcf = 0;
      if ((contentInfo.SKULookup is SKUCigarette) && ((SKUCigarette)contentInfo.SKULookup).MentholMaterial.Value)
        _shmcf = ((SKUCigarette)contentInfo.SKULookup).MentholMaterial.Value ? SHMentholIndex.SHMentholRatio.Value : 0;
      return new Material.Ratios
      {
        dustRatio = DustIndex.DustRatio.ValueOrException<double>("Batch", "Material.Ratios", "dustRatio"),
        shMentholRatio = _shmcf,
        wasteRatio = WasteIndex.WasteRatio.ValueOrException<double>("Batch", "Material.Ratios", "wasteRatio")
      };
    }
    private void AssignContentInfo(SummaryContentInfo contentInfo, bool newBatch)
    {
      if (newBatch)
        FGQuantityAvailable = contentInfo.Product.FGQuantity;
      else
      {
        double _diff = contentInfo.Product.FGQuantity.Value - FGQuantity.Value;
        double _available = FGQuantityAvailable.Value;
        if (_diff + _available < 0)
        {
          string _ptrn = "The previous batch {0} has quantity of finisched good greater then the new one - it looks like wrong messages sequence. Available={1}, Diff={2}";
          throw new InputDataValidationException("wrong status of the input batch", "BatchProcessing", String.Format(_ptrn, contentInfo.Product.Batch, _available, _diff), true);
        }
        FGQuantityAvailable = _diff + _available;
      }
      BatchStatus = contentInfo.BatchStatus;
      Batch0 = contentInfo.Product.Batch;
      SKU = contentInfo.Product.SKU;
      Title = String.Format("{0} SKU: {1}; Batch: {2}", contentInfo.Product.ProductType, SKU, Batch0);
      CalculatedOveruse = contentInfo.CalculatedOveruse;
      MaterialQuantity = contentInfo.TotalTobacco;
      FGQuantity = contentInfo.Product.FGQuantity;
      MaterialQuantityPrevious = 0;
      SKUIndex = contentInfo.SKULookup;
      ProductType = contentInfo.Product.ProductType;
      Dust = contentInfo[Linq.DisposalEnum.Dust];
      SHMenthol = contentInfo[Linq.DisposalEnum.SHMenthol];
      Waste = contentInfo[Linq.DisposalEnum.Waste];
      Tobacco = contentInfo[Linq.DisposalEnum.TobaccoInCigaretess];
      Overuse = contentInfo[Linq.DisposalEnum.OverusageInKg];
    }
    #endregion

  }
}

