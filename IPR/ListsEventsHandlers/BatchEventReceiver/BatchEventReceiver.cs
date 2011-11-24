using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using BatchMaterialXml = CAS.SmartFactory.xml.IPR.BatchMaterial;
using BatchXml = CAS.SmartFactory.xml.IPR.Batch;

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
      EntitiesDataContext edc = null;
      try
      {
        this.EventFiringEnabled = false;
        edc = new EntitiesDataContext(properties.WebUrl);
        if (properties.ListItem.File == null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of a batch xml message failed because the file is empty.");
          return;
        }
        String message = String.Format("Import of the batch message {0} starting.", properties.ListItem.File.ToString());
        Anons.WriteEntry(edc, m_Title, message);
        BatchXml document = BatchXml.ImportDocument(properties.ListItem.File.OpenBinaryStream());
        Dokument entry = Dokument.GetEntity(properties.ListItem.ID, edc.BatchLibrary);
        GetXmlContent(document, edc, entry);
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(edc, "Invoice message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          edc.SubmitChangesSilently(RefreshMode.KeepCurrentValues);
          edc.Dispose();
        }
        this.EventFiringEnabled = true;
        base.ItemAdded(properties);
      }
    }
    private void GetXmlContent(BatchXml document, EntitiesDataContext edc, Dokument entry)
    {
      Batch newBatch = new Batch()
      {
        BatchLibraryLookup = entry,
        BatchStatus = document.Status.ToString(),
      };
      SummaryContentInfo fg = GetXmlContent(document.Material, edc, newBatch);
      newBatch.Batch0 = fg.Product.Batch;
      newBatch.SKU = fg.Product.SKU;
      newBatch.Tytuł = String.Format("SKU: {}; Batch: {1}", newBatch.SKU, newBatch.Batch0);
      newBatch.FGQuantity = fg.Product.FGQuantity;
      newBatch.MaterialQuantity = fg.Product.TobaccoQuantity;
      newBatch.ProductType = fg.Product.MaterialType;
      //interconnect 
      newBatch.SKULookup = SKU.GetLookup(edc, fg.Product.SKU);
      newBatch.CutfillerCoefficientLookup = CutfillerCoefficient.GetLookup(edc);
      newBatch.DustLookup = Dust.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.SHMentholLookup = SHMenthol.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.UsageLookup = Usage.GetLookup(newBatch.SKULookup.FormatLookupTitle, edc);
      newBatch.WasteLookup = Waste.GetLookup(newBatch.ProductType.Value, edc);
      newBatch.CalculatedOveruse = fg.ProcessDisposals();
      edc.Batch.InsertOnSubmit(newBatch);
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
          if (value.MaterialType == ProductType.None) //TODO replace with tobacco
            TotalTobacco = TotalTobacco + value.TobaccoQuantity.GetValueOrDefault(0);
        }
        else
        {
          if (value.MaterialType == ProductType.Cigarette)
            Product = value;
          else if (Product == null && value.MaterialType == ProductType.Cutfiller)
            Product = value;
          base.Add(value.GetKey(), value);
        }
      }
      internal double ProcessDisposals()
      {
        //TODO to be implemented
        return 0;
      }
      internal IEnumerable<Material> GeContentEnumerator()
      {
        return this.Values;
      }
    }
    private SummaryContentInfo GetXmlContent(BatchMaterialXml[] batchEntries, EntitiesDataContext edc, Batch parent)
    {
      SummaryContentInfo itemsList = new SummaryContentInfo();
      foreach (BatchMaterialXml item in batchEntries)
      {
        Material newMaterial = new Material()
        {
          Batch = item.Batch.Trim(),
          BatchLookup = parent,
          SKU = item.Material.Trim(),
          Location = item.Stor__Loc,
          SKUDescription = item.Material_description,
          Tytuł = item.Material_description,
          Units = item.Unit,
          FGQuantity = Convert.ToDouble(item.Quantity),
          TobaccoQuantity = Convert.ToDouble(item.Quantity_calculated),
          MaterialType = Material.GetMaterialType(item.material_group)
        };
        itemsList.Add(newMaterial.SKU, newMaterial);
      }
      edc.Material.InsertAllOnSubmit(itemsList.GeContentEnumerator());
      return itemsList;
    }
    private const string m_Title = "Batch Message Import";
  }
}
