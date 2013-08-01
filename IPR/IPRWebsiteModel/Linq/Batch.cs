using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Batch
  /// </summary>
  public partial class Batch
  {
    #region public
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> instance.</param>
    /// <param name="batch">The index of the entry we are lookin for.</param>
    /// <returns>
    /// The most recent <see cref="Batch" /> object.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindLookup( Entities edc, string batch )
    {
      return ( from _batchX in edc.Batch
               where _batchX.Batch0.Contains( batch ) && _batchX.BatchStatus != Linq.BatchStatus.Progress
               orderby _batchX.Id.Value
               select _batchX ).FirstOrDefault();
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
    public static Batch FindStockToBatchLookup( Entities edc, string batch )
    {
      return ( from _batchX in edc.Batch
               where _batchX.Batch0.Contains( batch )
               orderby _batchX.Id.Value descending
               select _batchX ).FirstOrDefault();
    }
    /// <summary>
    /// Batches the processing.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="status">The status.</param>
    /// <param name="contentInfo">The content info.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="progressChanged">The progress changed.</param>
    /// <param name="newBatch">if set to <c>true</c> it is new batch.</param>
    public void BatchProcessing( Entities edc, BatchStatus status, SummaryContentInfo contentInfo, BatchLib parent, ProgressChangedEventHandler progressChanged, bool newBatch )
    {
      BatchLibraryIndex = parent;
      BatchStatus = status;
      Batch0 = contentInfo.Product.Batch;
      SKU = contentInfo.Product.SKU;
      Title = String.Format( "{0} SKU: {1}; Batch: {2}", contentInfo.Product.ProductType, SKU, Batch0 );
      if ( newBatch )
        FGQuantityAvailable = contentInfo.Product.FGQuantity;
      else
      {
        double _diff = contentInfo.Product.FGQuantity.Value - FGQuantity.Value;
        double _available = FGQuantityAvailable.Value;
        if ( _diff + _available < 0 )
        {
          string _ptrn = "The previous batch {0} has quantity of finisched good greater then the new one - it looks like wrong messages sequence. Available={1}, Diff={2}";
          throw new InputDataValidationException( "wrong status of the input batch", "BatchProcessing", String.Format( _ptrn, contentInfo.Product.Batch, _available, _diff ), true );
        }
        FGQuantityAvailable = _diff + _available;
      }
      FGQuantity = contentInfo.Product.FGQuantity;
      contentInfo.AdjustMaterialQuantity( status == Linq.BatchStatus.Final );
      MaterialQuantity = Convert.ToDouble( contentInfo.TotalTobacco );
      ProductType = contentInfo.Product.ProductType;
      SKUIndex = contentInfo.SKULookup;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Dependences
      Dust DustIndex = Linq.Dust.GetLookup( ProductType.Value, edc );
      BatchDustCooeficiency = DustIndex.DustRatio;
      DustCooeficiencyVersion = DustIndex.Version;
      SHMenthol SHMentholIndex = Linq.SHMenthol.GetLookup( ProductType.Value, edc );
      BatchSHCooeficiency = SHMentholIndex.SHMentholRatio;
      SHCooeficiencyVersion = SHMentholIndex.Version;
      Waste WasteIndex = Linq.Waste.GetLookup( ProductType.Value, edc );
      BatchWasteCooeficiency = WasteIndex.WasteRatio;
      WasteCooeficiencyVersion = WasteIndex.Version;
      CutfillerCoefficient _cc = CutfillerCoefficient.GetLookup( edc );
      this.CFTProductivityNormMax = _cc.CFTProductivityNormMax;
      this.CFTProductivityNormMin = _cc.CFTProductivityNormMin;
      this.CFTProductivityRateMax = _cc.CFTProductivityRateMax;
      this.CFTProductivityRateMin = _cc.CFTProductivityRateMin;
      this.CFTProductivityVersion = _cc.Version;
      Usage _usage = Usage.GetLookup( SKUIndex.FormatIndex, edc );
      this.CTFUsageMax = _usage.CTFUsageMax;
      this.CTFUsageMin = _usage.CTFUsageMin;
      this.UsageMin = _usage.UsageMin;
      this.UsageMax = _usage.UsageMax;
      this.UsageVersion = _usage.Version;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      CalculatedOveruse = GetOverusage( MaterialQuantity.Value, FGQuantity.Value, UsageMax.Value, UsageMin.Value );
      MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( SKUIndex is SKUCigarette ) && ( (SKUCigarette)SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)SKUIndex ).MentholMaterial.Value ? SHMentholIndex.SHMentholRatio.Value : 0;
      Material.Ratios _mr = new Material.Ratios
      {
        dustRatio = DustIndex.DustRatio.ValueOrException<double>( "Batch", "Material.Ratios", "dustRatio" ),
        shMentholRatio = _shmcf,
        wasteRatio = WasteIndex.WasteRatio.ValueOrException<double>( "Batch", "Material.Ratios", "wasteRatio" )
      };
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      contentInfo.ProcessMaterials( edc, this, _mr, CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      Dust = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.Dust ] );
      SHMenthol = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.SHMenthol ] );
      Waste = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.Waste ] );
      Tobacco = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.TobaccoInCigaretess ] );
      Overuse = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.OverusageInKg ] );
      if ( this.BatchStatus.Value == Linq.BatchStatus.Progress )
        return;
      //TODO - adjust overuse 
      foreach ( InvoiceContent _ix in this.InvoiceContent )
        _ix.UpdateExportedDisposals( edc );
      contentInfo.UpdateNotStartedDisposals( edc, this, progressChanged );
    }
    internal string DanglingBatchWarningMessage
    {
      get
      {
        string _msg = "Stock mismatch; the batch {0} is not reported on the stock. Correct your stock and try again.";
        return String.Format( _msg, this.Title );
      }
    }
    //internal void AddProgressDisposals( Entities edc, Batch parent, ProgressChangedEventHandler progressChanged )
    //{
    //  if ( edc.ObjectTrackingEnabled )
    //    throw new ApplicationException( "At Batch.GetDisposals the ObjectTrackingEnabled is set." );
    //  if ( this.BatchStatus.Value != Linq.BatchStatus.Progress )
    //    throw new ApplicationException( "At Batch.GetDisposals the BatchStatus != Linq.BatchStatus.Progress" );
    //  progressChanged( this, new ProgressChangedEventArgs( 1, "UpdateDisposals" ) );
    //  foreach ( Material _materialX in this.Material )
    //    _materialX.UpdateDisposals( edc, parent, progressChanged );
    //}
    internal void GetInventory( StockDictionary balanceStock, StockDictionary.StockValueKey key, double quantityOnStock )
    {
      switch ( this.BatchStatus.Value )
      {
        case Linq.BatchStatus.Progress:
          foreach ( Material _mtx in Material )
            _mtx.GetInventory( balanceStock, key, quantityOnStock / this.FGQuantity.Value );
          break;
        case Linq.BatchStatus.Intermediate:
        case Linq.BatchStatus.Final:
          break;
      }
    }
    internal void CheckQuantity( List<string> _warnings, StockLib lib )
    {
      if ( this.ProductType.Value != Linq.ProductType.Cigarette )
        return;
      decimal _onStock = 0;
      foreach ( StockEntry _stock in this.StockEntry )
      {
        if ( _stock.StockLibraryIndex != lib )
          continue;
        _onStock += Convert.ToDecimal( _stock.Quantity );
      }
      if ( Convert.ToDecimal( this.FGQuantityAvailable ) != _onStock )
      {
        string _msg = string.Format( m_noMachingQuantityWarningMessage, Convert.ToDecimal( this.FGQuantityAvailable ), _onStock, this.ProductType, this.Batch0, this.SKU );
        _warnings.Add( _msg );
      }
    }
    #endregion

    #region private
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be uploaded.";
    private string m_noMachingQuantityWarningMessage = "Inconsistent quantity batch: {0} / stock: {1} of the product: {2} batch: {3}/sku: {4} on the stock.";
    /// <summary>
    /// Gets the overuse as the ratio of overused tobacco divided by totaly usage of tobacco.
    /// </summary>
    /// <param name="_materialQuantity">The _material quantity.</param>
    /// <param name="_fGQuantity">The finished goods quantity.</param>
    /// <param name="_usageMax">The cutfiller usage max.</param>
    /// <param name="_usageMin">The cutfiller usage min.</param>
    /// <returns></returns>
    private static double GetOverusage( double _materialQuantity, double _fGQuantity, double _usageMax, double _usageMin )
    {
      double _ret = ( _materialQuantity - _fGQuantity * _usageMax / 1000 );
      if ( _ret > 0 )
        return _ret / _materialQuantity; // Overusage
      _ret = ( _materialQuantity - _fGQuantity * _usageMin / 1000 );
      if ( _ret < 0 )
        return _ret / _materialQuantity; //Underusage
      return 0;
    }
    #endregion

  }
}

