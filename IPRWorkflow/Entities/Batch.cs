using System;
using System.ComponentModel;
using System.Linq;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.IPR.Entities
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
    internal static void GetXmlContent(BatchXml xml, EntitiesDataContext edc, BatchLib parent, ProgressChangedEventHandler progressChanged)
    {
      Material.SummaryContentInfo fg = new Material.SummaryContentInfo(xml.Material, edc, progressChanged);
      Batch batch =
          (from idx in edc.Batch where idx.Batch0.Contains(fg.Product.Batch) && idx.BatchStatus.Value == Entities.BatchStatus.Preliminary select idx).FirstOrDefault();
      if (batch == null)
      {
        batch = new Batch();
        edc.Batch.InsertOnSubmit(batch);
      }
      batch.BatchProcessing(edc, GetBatchStatus(xml.Status), fg, parent);
    }
    /// <summary>
    /// Gets or creates lookup.
    /// </summary>
    /// <param name="edc">The <see cref="EntitiesDataContext"/></param>
    /// <param name="index">The indexob batch.</param>
    /// <returns></returns>
    internal static Batch GetOrCreatePreliminary(EntitiesDataContext edc, string index)
    {
      Batch newBatch;
      try
      {
        newBatch = GetLookup(edc, index);
      }
      catch (Exception)
      {
        newBatch = new Batch()
        {
          Batch0 = index,
          BatchStatus = Entities.BatchStatus.Preliminary,
          Tytuł = index,
          ProductType = Entities.ProductType.Invalid
        };
        Anons.WriteEntry(edc, m_Source, String.Format(m_LookupFailedAndAddedMessage, index));
        edc.Batch.InsertOnSubmit(newBatch);
        edc.SubmitChanges();
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
    internal static Batch GetLookup(EntitiesDataContext edc, string index)
    {
      try
      {
        return (from batch in edc.Batch where batch.Batch0.Contains(index) orderby batch.Identyfikator descending select batch).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_LookupFailedMessage, index), ex, "Cannot find batch");
      }
    }
    //TODO to be removed and replace by reverse lookup column
    //internal DisposalDisposal[] GetDisposals(EntitiesDataContext edc)
    //{
    //  var disposals =
    //      from idx in edc.Disposal
    //      where this.Identyfikator == idx.BatchLookup.Identyfikator
    //      select idx;
    //  return disposals.ToArray<DisposalDisposal>();
    //}
    #endregion

    #region private
    private void BatchProcessing(EntitiesDataContext edc, BatchStatus _status, Material.SummaryContentInfo fg, BatchLib parent)
    {
      this.BatchLibraryLookup = parent; 
      this.BatchStatus = _status;
      Batch0 = fg.Product.Batch;
      SKU = fg.Product.SKU;
      Tytuł = String.Format("SKU: {0}; Batch: {1}", SKU, Batch0);
      FGQuantity = fg.Product.FGQuantity;
      MaterialQuantity = fg.TotalTobacco;
      ProductType = fg.Product.ProductType;
      //interconnect 
      SKULookup = SKUCommonPart.GetLookup(edc, fg.Product.SKU);
      CutfillerCoefficientLookup = CutfillerCoefficient.GetLookup(edc);
      DustLookup = Entities.Dust.GetLookup(ProductType.Value, edc);
      SHMentholLookup = Entities.SHMenthol.GetLookup(ProductType.Value, edc);
      UsageLookup = Usage.GetLookup(SKULookup.FormatLookup, edc);
      WasteLookup = Entities.Waste.GetLookup(ProductType.Value, edc);
      //processing
      this.CalculatedOveruse = GetOverusage(MaterialQuantity, FGQuantity, UsageLookup.UsageMax, UsageLookup.UsageMin);
      this.FGQuantityAvailable = FGQuantity;
      this.FGQuantityBlocked = 0;
      this.FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      this.MaterialQuantityPrevious = 0;
      double _shmcf = SKULookup.IPRMaterial.Value ? SHMentholLookup.SHMentholRatio.Value : 0;
      fg.ProcessDisposals(edc, this, DustLookup.DustRatio.Value, _shmcf, WasteLookup.WasteRatio.Value, CalculatedOveruse.GetValueOrDefault(0));
      this.Dust = fg.AccumulatedDisposalsAnalisis[IPR.DisposalEnum.Dust];
      this.SHMenthol = fg.AccumulatedDisposalsAnalisis[IPR.DisposalEnum.SHMenthol];
      this.Waste = fg.AccumulatedDisposalsAnalisis[IPR.DisposalEnum.Waste];
      this.Tobacco = fg.AccumulatedDisposalsAnalisis[IPR.DisposalEnum.Tobacco];
      this.Overuse = fg.AccumulatedDisposalsAnalisis[IPR.DisposalEnum.OverusageInKg];
    }
    /// <summary>
    /// Gets the overuse as the ratio of overused tobacco divided by totaly usage of tobacco.
    /// </summary>
    /// <param name="_materialQuantity">The _material quantity.</param>
    /// <param name="_fGQuantity">The finished goods quantity.</param>
    /// <param name="_usageMax">The cutfiller usage max.</param>
    /// <param name="_usageMin">The cutfiller usage min.</param>
    /// <returns></returns>
    private static double GetOverusage(double? _materialQuantity, double? _fGQuantity, double? _usageMax, double? _usageMin)
    {
      double _ret = (_materialQuantity - _fGQuantity * _usageMax / 1000).GetValueOrDefault(0);
      if (_ret > 0)
        return _ret / _materialQuantity.Value; // Overusage
      _ret = (_materialQuantity - _fGQuantity * _usageMin / 1000).GetValueOrDefault(0);
      if (_ret < 0)
        return _ret / _materialQuantity.Value; //Underusage
      return 0;
    }
    private static BatchStatus GetBatchStatus(xml.erp.BatchStatus batchStatus)
    {
      switch (batchStatus)
      {
        case CAS.SmartFactory.xml.erp.BatchStatus.Final:
          return Entities.BatchStatus.Final;
        case CAS.SmartFactory.xml.erp.BatchStatus.Intermediate:
          return Entities.BatchStatus.Intermediate;
        case CAS.SmartFactory.xml.erp.BatchStatus.Progress:
          return Entities.BatchStatus.Progress;
        default:
          return Entities.BatchStatus.Preliminary;
      }
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be edited.";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    #endregion
  }
}
