using System;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// Batch
  /// </summary>
  internal static class BatchExtension
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
      MaterialExtension.SummaryContentInfo xmlBatchContent = new MaterialExtension.SummaryContentInfo( xml.Material, edc, progressChanged );
      Batch batch =
          ( from idx in edc.Batch where idx.Batch0.Contains( xmlBatchContent.Product.Batch ) && idx.BatchStatus.Value == BatchStatus.Preliminary select idx ).FirstOrDefault();
      if ( batch == null )
      {
        batch = new Batch();
        edc.Batch.InsertOnSubmit( batch );
      }
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: BatchProcessing" ) );
      BatchProcessing(batch, edc, GetBatchStatus( xml.Status ), xmlBatchContent, parent, progressChanged );
    }
    private static void BatchProcessing( Batch _this, Entities edc, BatchStatus status, MaterialExtension.SummaryContentInfo content, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      _this.BatchLibraryIndex = parent;
      _this.BatchStatus = status;
      _this.Batch0 = content.Product.Batch;
      _this.SKU = content.Product.SKU;
      _this.Title = String.Format( "{0} SKU: {1}; Batch: {2}", content.Product.ProductType, _this.SKU, _this.Batch0 );
      _this.FGQuantity = content.Product.FGQuantity;
      _this.MaterialQuantity = content.TotalTobacco;
      _this.ProductType = content.Product.ProductType;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: interconnect" ) );
      //interconnect 
      _this.SKUIndex = SKUCommonPart.GetLookup( edc, content.Product.SKU );
      _this.CutfillerCoefficientIndex = CutfillerCoefficient.GetLookup( edc );
      _this.UsageIndex = Usage.GetLookup( _this.SKUIndex.FormatIndex, edc );
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Coefficients
      _this.DustIndex = Dust.GetLookup( _this.ProductType.Value, edc );
      _this.BatchDustCooeficiency = _this.DustIndex.DustRatio;
      _this.DustCooeficiencyVersion = _this.DustIndex.Wersja;
      _this.SHMentholIndex = SHMenthol.GetLookup( _this.ProductType.Value, edc );
      _this.BatchSHCooeficiency = _this.SHMentholIndex.SHMentholRatio;
      _this.SHCooeficiencyVersion = _this.SHMentholIndex.Wersja;
      _this.WasteIndex = Waste.GetLookup( _this.ProductType.Value, edc );
      _this.BatchWasteCooeficiency = _this.WasteIndex.WasteRatio;
      _this.WasteCooeficiencyVersion = _this.WasteIndex.Wersja;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      _this.CalculatedOveruse = GetOverusage( _this.MaterialQuantity.Value, _this.FGQuantity.Value, _this.UsageIndex.UsageMax.Value, _this.UsageIndex.UsageMin.Value );
      _this.FGQuantityAvailable = _this.FGQuantity;
      _this.FGQuantityBlocked = 0;
      _this.FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      _this.MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( _this.SKUIndex is SKUCigarette ) && ( (SKUCigarette)_this.SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)_this.SKUIndex ).MentholMaterial.Value ? _this.SHMentholIndex.SHMentholRatio.Value : 0;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      content.ProcessDisposals( edc, _this, _this.DustIndex.DustRatio.Value, _shmcf, _this.WasteIndex.WasteRatio.Value, _this.CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      _this.Dust = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Dust ];
      _this.SHMenthol = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol ];
      _this.Waste = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Waste ];
      _this.Tobacco = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Tobacco ];
      _this.Overuse = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.OverusageInKg ];
      foreach ( var _invoice in _this.InvoiceContent )
      {
        _invoice.CreateTitle();
        if ( _this.Available( _invoice.Quantity.Value ) )
          _invoice.InvoiceContentStatus = InvoiceContentStatus.OK;
        else
          _invoice.InvoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
      }
    }

    /// <summary>
    /// Gets or creates lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/></param>
    /// <param name="index">The indexob batch.</param>
    /// <returns></returns>
     #endregion

    #region private
    private static BatchStatus GetBatchStatus( xml.erp.BatchStatus batchStatus )
    {
      switch ( batchStatus )
      {
        case CAS.SmartFactory.xml.erp.BatchStatus.Final:
          return BatchStatus.Final;
        case CAS.SmartFactory.xml.erp.BatchStatus.Intermediate:
          return BatchStatus.Intermediate;
        case CAS.SmartFactory.xml.erp.BatchStatus.Progress:
          return BatchStatus.Progress;
        default:
          return BatchStatus.Preliminary;
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
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    #endregion
  }
}
