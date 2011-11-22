using System;
using System.Linq;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint;
using StockXml = CAS.SmartFactory.xml.IPR.Stock;
using StockXmlRow = CAS.SmartFactory.xml.IPR.StockRow;
using System.Diagnostics;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Reports
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class StockLibraryEventReceiver : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      try
      {
        this.EventFiringEnabled = false;
        using (EntitiesDataContext edc = new EntitiesDataContext(properties.WebUrl))
        {
          if (properties.ListItem.File == null)
          {
            Anons log = new Anons()
            {
              Tytuł = m_Title,
              Treść = "Import of a stock xml message failed because the file is empty."
            };
            edc.ActivityLog.InsertOnSubmit(log);
            edc.SubmitChanges();
            return;
          }
          Anons mess = new Anons()
          {
            Tytuł = m_Title,
            Treść = String.Format("Import of the stock message {0} starting.", properties.ListItem.File.ToString())
          };
          edc.ActivityLog.InsertOnSubmit(mess);
          edc.SubmitChanges();
          StockXml document = StockXml.ImportDocument(properties.ListItem.File.OpenBinaryStream());
          Dokument entry =
            (from enr in edc.StockLibrary
             where enr.Identyfikator == properties.ListItem.ID
             select enr).First<Dokument>();
          GetStock(document, edc, entry);
          edc.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        SPWeb web = properties.Web;
        SPList log = web.Lists.TryGetList("Activity Log");
        if (log == null)
        {
          EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
          return;
        }
        SPListItem item = log.AddItem();
        item["Title"] = "Stock message import error";
        item["Body"] = ex.Message;
        item.UpdateOverwriteVersion();
        properties.ListItem["Name"] = properties.ListItem["Name"] + ": Import Error !!";
        properties.ListItem.UpdateOverwriteVersion();
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded(properties);
    }
    private void GetStock(StockXml document, EntitiesDataContext edc, Dokument entry)
    {
      Stock newStock = new Stock
      {
        StockLibraryLookup = entry
      };
      edc.Stock.InsertOnSubmit(newStock);
      GetStock(document.Row, edc, newStock);
    }
    private void GetStock(StockXmlRow[] rows, EntitiesDataContext edc, Stock entry)
    {
      List<StockEntry> stockEntities = new List<StockEntry>();
      List<Batch> batchEntries = new List<Batch>();
      foreach (StockXmlRow item in rows)
      {
        StockEntry nse = new StockEntry()
        {
          StockListLookup = entry, 
          Batch = item.Batch.Trim(),
          Blocked = item.Blocked,
          DocumentNo = "To be removed",
          InQualityInsp = item.InQualityInsp,
          IPRType = false,
          Location = item.SLoc,
          RestrictedUse = item.RestrictedUse,
          SKU = item.Material.Trim(),
          SKUDescription = "To be removed",
          Tytuł = item.MaterialDescription.Trim(),
          Units = item.BUn,
          Unrestricted = item.Unrestricted,
          Quantity = 0,
          BatchLookup = null
        };
        nse.Quantity = item.Blocked.GetValueOrDefault(0) +
          item.InQualityInsp.GetValueOrDefault(0) +
          item.RestrictedUse.GetValueOrDefault(0) +
          item.Unrestricted.GetValueOrDefault(0);
        var cb =
          from batch in edc.Batch where batch.Batch0.Contains(nse.Batch) select batch;
        if (cb.Count<Batch>() == 0)
        {
          Batch newBatch = new Batch()
          {
            Batch0 = nse.Batch,
            BatchStatus = "Preliminary",
            Tytuł = nse.Batch
          };
          batchEntries.Add(newBatch);
        }
        stockEntities.Add(nse);
      }
      if (stockEntities.Count > 0)
        edc.StockEntry.InsertAllOnSubmit(stockEntities);
      if (batchEntries.Count > 0)
        edc.Batch.InsertAllOnSubmit(batchEntries);
    }
    private const string m_Title = "Stock Message Import";
  }
}
