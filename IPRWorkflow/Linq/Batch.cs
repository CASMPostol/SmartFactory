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
    internal static void GetXmlContent( BatchXml xml, Entities edc, BatchLib parent, ProgressChangedEventHandler progressChanged )
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
    /// <param name="edc">The <see cref="Entities"/></param>
    /// <param name="index">The indexob batch.</param>
    /// <returns></returns>
     #endregion

    #region private
    private void BatchProcessing( Entities edc, BatchStatus status, Material.SummaryContentInfo content, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      this.BatchLibraryIndex = parent;
      this.BatchStatus = status;
      Batch0 = content.Product.Batch;
      SKU = content.Product.SKU;
      Title = String.Format( "{0} SKU: {1}; Batch: {2}", content.Product.ProductType, SKU, Batch0 );
      FGQuantity = content.Product.FGQuantity;
      MaterialQuantity = content.TotalTobacco;
      ProductType = content.Product.ProductType;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: interconnect" ) );
      //interconnect 
      SKUIndex = SKUCommonPart.GetLookup( edc, content.Product.SKU );
      CutfillerCoefficientIndex = CutfillerCoefficient.GetLookup( edc );
      UsageIndex = Usage.GetLookup( SKUIndex.FormatIndex, edc );
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Coefficients
      DustIndex = Linq.IPR.Dust.GetLookup( ProductType.Value, edc );
      this.BatchDustCooeficiency = DustIndex.DustRatio;
      DustCooeficiencyVersion = DustIndex.Wersja;
      SHMentholIndex = Linq.IPR.SHMenthol.GetLookup( ProductType.Value, edc );
      this.BatchSHCooeficiency = SHMentholIndex.SHMentholRatio;
      SHCooeficiencyVersion = SHMentholIndex.Wersja;
      WasteIndex = Linq.IPR.Waste.GetLookup( ProductType.Value, edc );
      this.BatchWasteCooeficiency = WasteIndex.WasteRatio;
      WasteCooeficiencyVersion = WasteIndex.Wersja;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      this.CalculatedOveruse = GetOverusage( MaterialQuantity.Value, FGQuantity.Value, UsageIndex.UsageMax.Value, UsageIndex.UsageMin.Value );
      this.FGQuantityAvailable = FGQuantity;
      this.FGQuantityBlocked = 0;
      this.FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      this.MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( SKUIndex is SKUCigarette ) && ( (SKUCigarette)SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)SKUIndex ).MentholMaterial.Value ? SHMentholIndex.SHMentholRatio.Value : 0;
      progressChanged( this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      content.ProcessDisposals( edc, this, DustIndex.DustRatio.Value, _shmcf, WasteIndex.WasteRatio.Value, CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      this.Dust = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Dust ];
      this.SHMenthol = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.SHMenthol ];
      this.Waste = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Waste ];
      this.Tobacco = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.Tobacco ];
      this.Overuse = content.AccumulatedDisposalsAnalisis[ IPR.DisposalEnum.OverusageInKg ];
      foreach ( var _invoice in this.InvoiceContent )
      {
        _invoice.CreateTitle();
        if ( this.Available( _invoice.Quantity.Value ) )
          _invoice.InvoiceContentStatus = InvoiceContentStatus.OK;
        else
          _invoice.InvoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
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
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    #endregion
  }
}
