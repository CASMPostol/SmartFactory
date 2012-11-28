using System;
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
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="index">The index of the entry we are lookin for.</param>
    /// <returns>The most recent <see cref="Batch"/> object.</returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindLookup( Entities edc, string index )
    {
      return ( from batch in edc.Batch where batch.Batch0.Contains( index ) orderby batch.Identyfikator descending select batch ).FirstOrDefault();
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="index">The index of the entry we are lookin for.</param>
    /// <returns>The most recent <see cref="Batch"/> object.</returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static IQueryable<Batch> FindAll( Entities edc, string index )
    {
      return from batch in edc.Batch where batch.Batch0.Contains( index ) && batch.BatchStatus != Linq.BatchStatus.Progress orderby batch.Identyfikator descending select batch;
    }
    public static double CumulatedAvailable( IQueryable<Batch> batches )
    {
      return batches.Sum<Batch>( x => x.FGQuantityAvailable.Value );
    }
    /// <summary>
    /// Batches the processing.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="status">The status.</param>
    /// <param name="contentInfo">The content info.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public void BatchProcessing( Entities edc, BatchStatus status, SummaryContentInfo contentInfo, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      BatchLibraryIndex = parent;
      BatchStatus = status;
      Batch0 = contentInfo.Product.Batch;
      SKU = contentInfo.Product.SKU;
      Title = String.Format( "{0} SKU: {1}; Batch: {2}", contentInfo.Product.ProductType, SKU, Batch0 );
      FGQuantity = contentInfo.Product.FGQuantity;
      MaterialQuantity = Convert.ToDouble( contentInfo.TotalTobacco );
      ProductType = contentInfo.Product.ProductType;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: interconnect" ) );
      //interconnect 
      SKUIndex = contentInfo.SKULookup;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Coefficients
      DustIndex = Linq.Dust.GetLookup( ProductType.Value, edc );
      BatchDustCooeficiency = DustIndex.DustRatio;
      DustCooeficiencyVersion = DustIndex.Wersja;
      SHMentholIndex = Linq.SHMenthol.GetLookup( ProductType.Value, edc );
      BatchSHCooeficiency = SHMentholIndex.SHMentholRatio;
      SHCooeficiencyVersion = SHMentholIndex.Wersja;
      WasteIndex = Linq.Waste.GetLookup( ProductType.Value, edc );
      BatchWasteCooeficiency = WasteIndex.WasteRatio;
      WasteCooeficiencyVersion = WasteIndex.Wersja;
      CutfillerCoefficient _cc = CutfillerCoefficient.GetLookup( edc );
      CutfillerCoefficientIndex = _cc;
      this.CFTProductivityNormMax = _cc.CFTProductivityNormMax;
      this.CFTProductivityNormMin = _cc.CFTProductivityNormMin;
      this.CFTProductivityRateMax = _cc.CFTProductivityRateMax;
      this.CFTProductivityRateMin = _cc.CFTProductivityRateMin;
      Usage _usage = Usage.GetLookup( SKUIndex.FormatIndex, edc );
      UsageIndex = _usage;
      this.CTFUsageMax = _usage.CTFUsageMax;
      this.CTFUsageMin = _usage.CTFUsageMin;
      this.UsageMin = _usage.UsageMin;
      this.UsageMax = _usage.UsageMax;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      CalculatedOveruse = GetOverusage( MaterialQuantity.Value, FGQuantity.Value, UsageMax.Value, UsageMin.Value );
      FGQuantityAvailable = FGQuantity;
      FGQuantityBlocked = 0;
      FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( SKUIndex is SKUCigarette ) && ( (SKUCigarette)SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)SKUIndex ).MentholMaterial.Value ? SHMentholIndex.SHMentholRatio.Value : 0;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      contentInfo.ProcessDisposals( edc, this, DustIndex.DustRatio.Value, _shmcf, WasteIndex.WasteRatio.Value, CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      Dust = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ WebsiteModel.Linq.Material.DisposalsEnum.Dust ] );
      SHMenthol = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ WebsiteModel.Linq.Material.DisposalsEnum.SHMenthol ] );
      Waste = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ WebsiteModel.Linq.Material.DisposalsEnum.Waste ] );
      Tobacco = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ WebsiteModel.Linq.Material.DisposalsEnum.Tobacco ] );
      Overuse = Convert.ToDouble( contentInfo.AccumulatedDisposalsAnalisis[ WebsiteModel.Linq.Material.DisposalsEnum.OverusageInKg ] );
      foreach ( var _invoice in InvoiceContent )
      {
        _invoice.CreateTitle();
        if ( this.Available( _invoice.Quantity.Value ) )
          _invoice.InvoiceContentStatus = InvoiceContentStatus.OK;
        else
          _invoice.InvoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
      }
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

