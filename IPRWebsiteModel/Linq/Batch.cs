using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
               orderby _batchX.Identyfikator.Value
               select _batchX ).FirstOrDefault();
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> instance.</param>
    /// <param name="index">The index of the entry we are lookin for.</param>
    /// <returns>
    /// The most recent <see cref="Batch" /> object.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindStockToBatchLookup( Entities edc, string batch )
    {
      return ( from _batchX in edc.Batch
               where _batchX.Batch0.Contains( batch )
               orderby _batchX.Identyfikator.Value
               ascending
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
      FGQuantityAvailable = newBatch ? contentInfo.Product.FGQuantity : contentInfo.Product.FGQuantity - FGQuantity + FGQuantityAvailable;
      if ( FGQuantityAvailable < 0 )
        throw new ArgumentException( "FGQuantityAvailable", "FGQuantityAvailable cannot be less then 0" );
      FGQuantity = contentInfo.Product.FGQuantity;
      MaterialQuantity = Convert.ToDouble( contentInfo.TotalTobacco );
      ProductType = contentInfo.Product.ProductType;
      SKUIndex = contentInfo.SKULookup;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Dependences
      Dust DustIndex = Linq.Dust.GetLookup( ProductType.Value, edc );
      BatchDustCooeficiency = DustIndex.DustRatio;
      DustCooeficiencyVersion = DustIndex.Wersja;
      SHMenthol SHMentholIndex = Linq.SHMenthol.GetLookup( ProductType.Value, edc );
      BatchSHCooeficiency = SHMentholIndex.SHMentholRatio;
      SHCooeficiencyVersion = SHMentholIndex.Wersja;
      Waste WasteIndex = Linq.Waste.GetLookup( ProductType.Value, edc );
      BatchWasteCooeficiency = WasteIndex.WasteRatio;
      WasteCooeficiencyVersion = WasteIndex.Wersja;
      CutfillerCoefficient _cc = CutfillerCoefficient.GetLookup( edc );
      this.CFTProductivityNormMax = _cc.CFTProductivityNormMax;
      this.CFTProductivityNormMin = _cc.CFTProductivityNormMin;
      this.CFTProductivityRateMax = _cc.CFTProductivityRateMax;
      this.CFTProductivityRateMin = _cc.CFTProductivityRateMin;
      this.CFTProductivityVersion = _cc.Wersja;
      Usage _usage = Usage.GetLookup( SKUIndex.FormatIndex, edc );
      this.CTFUsageMax = _usage.CTFUsageMax;
      this.CTFUsageMin = _usage.CTFUsageMin;
      this.UsageMin = _usage.UsageMin;
      this.UsageMax = _usage.UsageMax;
      this.UsageVersion = _usage.Wersja;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      CalculatedOveruse = GetOverusage( MaterialQuantity.Value, FGQuantity.Value, UsageMax.Value, UsageMin.Value );
      MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( SKUIndex is SKUCigarette ) && ( (SKUCigarette)SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)SKUIndex ).MentholMaterial.Value ? SHMentholIndex.SHMentholRatio.Value : 0;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      contentInfo.ProcessMaterials( edc, this, DustIndex.DustRatio.Value, _shmcf, WasteIndex.WasteRatio.Value, CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      Dust = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.Dust ] );
      SHMenthol = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.SHMenthol ] );
      Waste = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.Waste ] );
      Tobacco = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.TobaccoInCigaretess ] );
      Overuse = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ Linq.DisposalEnum.OverusageInKg ] );
      if ( this.BatchStatus.Value == Linq.BatchStatus.Progress )
        return;
      foreach ( InvoiceContent _ix in this.InvoiceContent )
        _ix.UpdateExportedDisposals( edc );
      contentInfo.UpdateNotStartedDisposals( edc, this, progressChanged );
    }
    internal void GetDisposals( Entities edc, Batch parent, ProgressChangedEventHandler progressChanged )
    {
      if (edc.ObjectTrackingEnabled)
        throw new ApplicationException( "At Batch.GetDisposals the ObjectTrackingEnabled is set." );
      if ( this.BatchStatus.Value != Linq.BatchStatus.Progress )
        throw new ApplicationException( "At Batch.GetDisposals the BatchStatus != Linq.BatchStatus.Progress" );
      progressChanged( this, new ProgressChangedEventArgs( 1, "UpdateDisposals" ) );
      foreach ( Material _materialX in this.Material )
        _materialX.UpdateDisposals( edc, parent, progressChanged );
    }
    #endregion

    #region private
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be uploaded.";
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

