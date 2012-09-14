using System;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// Batch
  /// </summary>
  public partial class Batch
  {
    #region public
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The entry.</param>
    internal static void GetXmlContent( BatchXml xml, EntitiesDataContext edc, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      Material.SummaryContentInfo xmlBatchContent = new Material.SummaryContentInfo( xml.Material, edc, progressChanged );
      Batch batch =
          ( from idx in edc.Batch where idx.Batch0.Contains( xmlBatchContent.Product.Batch ) && idx.BatchStatus.Value == Linq.IPR.BatchStatus.Preliminary select idx ).FirstOrDefault();
      if ( batch == null )
      {
        batch = new Batch();
        edc.Batch.InsertOnSubmit( batch );
      }
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: BatchProcessing" ) );
      batch.BatchProcessing( edc, GetBatchStatus( xml.Status ), xmlBatchContent, parent, progressChanged );
    }
    /// <summary>
    /// Gets or creates lookup.
    /// </summary>
    /// <param name="edc">The <see cref="EntitiesDataContext"/></param>
    /// <param name="index">The indexob batch.</param>
    /// <returns></returns>
    internal static Batch GetOrCreatePreliminary( EntitiesDataContext edc, string batch )
    {
      Batch newBatch = FindLookup( edc, batch );
      if ( newBatch == null )
      {
        newBatch = new Batch()
        {
          Batch0 = batch,
          BatchStatus = Linq.IPR.BatchStatus.Preliminary,
          Tytuł = "Preliminary batch: " + batch,
          ProductType = Linq.IPR.ProductType.Invalid,
          FGQuantityAvailable = 0,
          FGQuantityBlocked = 0,
          FGQuantityKUKg = 0,
          FGQuantityPrevious = 0
        };
        edc.Batch.InsertOnSubmit( newBatch );
        Anons.WriteEntry( edc, m_Source, String.Format( m_LookupFailedAndAddedMessage, batch ) );
      }
      return newBatch;
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="EntitiesDataContext"/> instance.</param>
    /// <param name="index">The index of the entry we are lookin for.</param>
    /// <returns>The most recent <see cref="Batch"/> object.</returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    internal static Batch FindLookup( EntitiesDataContext edc, string index )
    {
      try
      {
        return ( from batch in edc.Batch where batch.Batch0.Contains( index ) orderby batch.Identyfikator descending select batch ).FirstOrDefault();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( m_Source, String.Format( m_LookupFailedMessage, index ), ex, "Cannot find batch" );
      }
    }
    #endregion

    #region private
    private void BatchProcessing( EntitiesDataContext edc, BatchStatus status, Material.SummaryContentInfo content, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      this.BatchLibraryLookup = parent;
      this.BatchStatus = status;
      Batch0 = content.Product.Batch;
      SKU = content.Product.SKU;
      Tytuł = String.Format( "{0} SKU: {1}; Batch: {2}", content.Product.ProductType, SKU, Batch0 );
      FGQuantityKUKg = content.Product.FGQuantityKUKg;
      MaterialQuantity = content.TotalTobacco;
      ProductType = content.Product.ProductType;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: interconnect" ) );
      //interconnect 
      SKULookup = SKUCommonPart.GetLookup( edc, content.Product.SKU );
      CutfillerCoefficientLookup = CutfillerCoefficient.GetLookup( edc );
      UsageLookup = Usage.GetLookup( SKULookup.FormatLookup, edc );
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Coefficients
      DustLookup = Linq.IPR.Dust.GetLookup( ProductType.Value, edc );
      DustCooeficiency = DustLookup.DustRatio;
      DustCoeficiencyVersion = DustLookup.Wersja;
      SHMentholLookup = Linq.IPR.SHMenthol.GetLookup( ProductType.Value, edc );
      SHCooeficiency = SHMentholLookup.SHMentholRatio;
      SHCoeficiencyVersion = SHMentholLookup.Wersja;
      WasteLookup = Linq.IPR.Waste.GetLookup( ProductType.Value, edc );
      WasteCooeficiency = WasteLookup.WasteRatio;
      WasteCoeficiencyVersion = WasteLookup.Wersja;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      this.CalculatedOveruse = GetOverusage( MaterialQuantity.Value, FGQuantityKUKg.Value, UsageLookup.UsageMax.Value, UsageLookup.UsageMin.Value );
      this.FGQuantityAvailable = FGQuantityKUKg;
      this.FGQuantityBlocked = 0;
      this.FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      this.MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( (SKULookup is SKUCigarette) && ((SKUCigarette)SKULookup ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)SKULookup ).MentholMaterial.Value ? SHMentholLookup.SHMentholRatio.Value : 0;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      content.ProcessDisposals( edc, this, DustLookup.DustRatio.Value, _shmcf, WasteLookup.WasteRatio.Value, CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      this.DustKg = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Dust ];
      this.SHMentholKg = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.SHMenthol ];
      this.WasteKg = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Waste ];
      this.TobaccoKg = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Tobacco ];
      this.OveruseKg = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.OverusageInKg ];
      foreach ( var _invoice in this.InvoiceContent )
      {
        _invoice.CreateTitle();
        if ( this.Available( _invoice.Quantity.Value ) )
          _invoice.Status = Status.OK;
        else
          _invoice.Status = Status.NotEnoughQnt;
      }
    }
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
    private static BatchStatus GetBatchStatus( xml.erp.BatchStatus batchStatus )
    {
      switch ( batchStatus )
      {
        case CAS.SmartFactory.xml.erp.BatchStatus.Final:
          return Linq.IPR.BatchStatus.Final;
        case CAS.SmartFactory.xml.erp.BatchStatus.Intermediate:
          return Linq.IPR.BatchStatus.Intermediate;
        case CAS.SmartFactory.xml.erp.BatchStatus.Progress:
          return Linq.IPR.BatchStatus.Progress;
        default:
          return Linq.IPR.BatchStatus.Preliminary;
      }
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be uploaded.";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    #endregion
  }
}
