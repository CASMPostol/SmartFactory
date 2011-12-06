using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;
using System.IO;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class BatchEventReceiver : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if (!properties.List.Title.Contains("Batch"))
        return;
      this.EventFiringEnabled = false;
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of a batch xml message failed because the file is empty.");
      //  return;
      //}
      ImportBatchFromXml(
        properties.ListItem.File.OpenBinaryStream(),
        properties.WebUrl,
        properties.ListItem.ID,
        properties.ListItem.File.ToString(),
        (object obj, ProgressChangedEventArgs progres) => { return; });
      this.EventFiringEnabled = true;
      base.ItemAdded(properties);
    }
    public static void ImportBatchFromXml
      (Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      try
      {
        edc = new EntitiesDataContext(url);
        String message = String.Format("Import of the batch message {0} starting.", fileName);
        Anons.WriteEntry(edc, m_Title, message);
        edc.SubmitChanges();
        BatchXml document = BatchXml.ImportDocument(stream);
        Dokument entry = Dokument.GetEntity(listIndex, edc.BatchLibrary);
        GetXmlContent(document, edc, entry);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(edc, "Batch message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of the stock message finished");
          edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
          edc.Dispose();
        }
      }
    }
    private static void GetXmlContent(BatchXml document, EntitiesDataContext edc, Dokument entry)
    {
      Batch newBatch = new Batch()
      {
        BatchLibraryLookup = entry,
        BatchStatus = GetBatchStatus(document.Status)
      };
      SummaryContentInfo fg = GetXmlContent(document.Material, edc, newBatch);
      newBatch.Batch0 = fg.Product.Batch;
      newBatch.SKU = fg.Product.SKU;
      newBatch.Tytuł = String.Format("SKU: {}; Batch: {1}", newBatch.SKU, newBatch.Batch0);
      newBatch.FGQuantity = fg.Product.FGQuantity;
      newBatch.MaterialQuantity = fg.Product.TobaccoQuantity;
      newBatch.ProductType = fg.Product.ProductType;
      //interconnect 
      newBatch.SKULookup = SKUCommonPart.GetLookup(edc, fg.Product.SKU);
      newBatch.CutfillerCoefficientLookup = CutfillerCoefficient.GetLookup(edc);
      newBatch.DustLookup = Dust.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.SHMentholLookup = SHMenthol.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.UsageLookup = Usage.GetLookup(newBatch.SKULookup.FormatLookup, edc);
      newBatch.WasteLookup = Waste.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.CalculatedOveruse = fg.ProcessDisposals();
      edc.Batch.InsertOnSubmit(newBatch);
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
    private class SummaryContentInfo : SortedList<string, Material>
    {
      public SummaryContentInfo() { }
      public Material Product { get; private set; }
      public double TotalTobacco { get; private set; }
      /// <summary>
      /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList<TKey,TValue>.
      /// </summary>
      /// <param name="key">The key of the element to add.</param>
      /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
      /// <exception cref="System.ArgumentNullException">key is null</exception>
      /// <exception cref="System.ArgumentException">An element with the same key already exists in the <paramref name="System.Collections.Generic.SortedList<TKey,TValue>"/>.</exception>
      public void Add(Material value)
      {
        Material ce = null;
        if (this.TryGetValue(value.GetKey(), out ce))
        {
          ce.FGQuantity = ce.FGQuantity + value.FGQuantity;
          ce.TobaccoQuantity = ce.TobaccoQuantity + value.TobaccoQuantity;
          if (value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco)
            TotalTobacco = TotalTobacco + value.TobaccoQuantity.GetValueOrDefault(0);
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
      internal double ProcessDisposals()
      {
        //TODO to be implemented: http://itrserver/Bugs/BugDetail.aspx?bid=2869
        return 0;
      }
      internal IEnumerable<Material> GeContentEnumerator()
      {
        return this.Values;
      }
    }
    private static SummaryContentInfo GetXmlContent(BatchMaterialXml[] batchEntries, EntitiesDataContext edc, Batch parent)
    {
      SummaryContentInfo itemsList = new SummaryContentInfo();
      foreach (BatchMaterialXml item in batchEntries)
      {
        Material newMaterial = new Material()
        {
          Batch = item.Batch,
          BatchLookup = parent,
          SKU = item.Material,
          Location = item.Stor__Loc,
          SKUDescription = item.Material_description,
          Tytuł = item.Material_description,
          Units = item.Unit,
          FGQuantity = Convert.ToDouble(item.Quantity),
          TobaccoQuantity = Convert.ToDouble(item.Quantity_calculated),
          ProductType = ProductType.Invalid,
          ProductID = item.material_group
        };
        newMaterial.GetProductType(edc);
        itemsList.Add(newMaterial.SKU, newMaterial);
      }
      edc.Material.InsertAllOnSubmit(itemsList.GeContentEnumerator());
      return itemsList;
    }
    private const string m_Title = "Batch Message Import";
  }
}
