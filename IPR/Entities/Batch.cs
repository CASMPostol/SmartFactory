﻿using System;
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
    /// <param name="entry">The entry.</param>
    internal static void GetXmlContent(BatchXml xml, EntitiesDataContext edc, Dokument entry)
    {
      Batch newBatch = new Batch(xml, entry);
      Material.SummaryContentInfo fg = Material.GetXmlContent(xml.Material, edc, newBatch);
      Batch oldBatch = null;
      try
      {
        oldBatch =
          (from batch in edc.Batch where batch.Batch0.Contains(fg.Product.Batch) && batch.BatchStatus.Value == Entities.BatchStatus.Preliminary select batch).First();
      }
      catch (Exception) { }
      newBatch.BatchProcessing(xml, edc, fg);
      edc.Batch.InsertOnSubmit(newBatch);
      if (oldBatch == null)
        edc.Batch.DeleteOnSubmit(oldBatch);
    }
    /// <summary>
    /// Gets or creates lookup.
    /// </summary>
    /// <param name="edc">The <see cref="EntitiesDataContext"/></param>
    /// <param name="index">The indexob batch.</param>
    /// <returns></returns>
    internal static Batch GetOrCreateLookup(EntitiesDataContext edc, string index)
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
          Tytuł = index
        };
        Anons.WriteEntry(edc, m_Source, String.Format(m_BatchLookupFiledMessage, index));
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
      Batch newBatch = (from batch in edc.Batch where batch.Batch0.Contains(index) orderby batch.Identyfikator descending select batch).First();
      return newBatch;
    }
    internal Disposal[] GetDisposals(EntitiesDataContext edc)
    {
      var disposals =
          from idx in edc.Disposal
          where this.Identyfikator == idx.BatchLookup.Identyfikator
          select idx;
      return disposals.ToArray<Disposal>();
    } 
    #endregion

    #region private
    private Batch(BatchXml document, Dokument entry)
      : this()
    {
      this.BatchLibraryLookup = entry;
      this.BatchStatus = GetBatchStatus(document.Status);
    }
	private void BatchProcessing(BatchXml document, EntitiesDataContext edc, Material.SummaryContentInfo fg)
    {
      Batch0 = fg.Product.Batch;
      SKU = fg.Product.SKU;
      Tytuł = String.Format("SKU: {0}; Batch: {1}", SKU, Batch0);
      FGQuantity = fg.Product.FGQuantity;
      MaterialQuantity = fg.Product.TobaccoQuantity;
      ProductType = fg.Product.ProductType;
      //interconnect 
      SKULookup = SKUCommonPart.GetLookup(edc, fg.Product.SKU);
      CutfillerCoefficientLookup = CutfillerCoefficient.GetLookup(edc);
      DustLookup = Entities.Dust.GetLookup(ProductType.Value, edc);
      SHMentholLookup = SHMenthol.GetLookup(ProductType.Value, edc);
      UsageLookup = Usage.GetLookup(SKULookup.FormatLookup, edc);
      WasteLookup = Entities.Waste.GetLookup(ProductType.Value, edc);
      CalculatedOveruse = fg.ProcessDisposals();
    }
    private static BatchStatus? GetBatchStatus(xml.erp.BatchStatus batchStatus)
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
    private const string m_BatchLookupFiledMessage = "I cannot recognize batch (0) - added preliminary entry do the list that must be edited.";
	#endregion  
  }
}
