using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint;
using InvoiceItemXml = CAS.SmartFactory.xml.IPR.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.IPR.Invoice;
using CAS.SmartFactory.xml;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class InvoiceEventReceiver : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      EntitiesDataContext edc = null;
      try
      {
        this.EventFiringEnabled = false;
        edc = new EntitiesDataContext(properties.WebUrl);
        if (properties.ListItem.File == null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of an invoice xml message failed because the file is empty.");
          return;
        }
        String message = String.Format("Import of the invoice message {0} starting.", properties.ListItem.File.ToString());
        Anons.WriteEntry(edc, m_Title, message);
        InvoiceXml document = InvoiceXml.ImportDocument(properties.ListItem.File.OpenBinaryStream());
        Dokument entry =
          (from enr in edc.InvoiceLibrary
           where enr.Identyfikator == properties.ListItem.ID
           select enr).First<Dokument>();
        GetXmlContent(document, edc, entry);
      }
      catch (Exception ex)
      {
        if (edc == null)
        {
          EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
          return;
        }
        Anons.WriteEntry(edc, "Invoice message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          edc.SubmitChanges();
          edc.Dispose();
        }
        this.EventFiringEnabled = true;
        base.ItemAdded(properties);
      }
    }
    private void GetXmlContent(InvoiceXml document, EntitiesDataContext edc, Dokument entry)
    {
      Invoice newInvoice = new Invoice
      {
        InvoiceLibraryLookup = entry
      };
      edc.Invoice.InsertOnSubmit(newInvoice);
      newInvoice.BillDoc = GetXmlContent(document.Item, edc, newInvoice);
    }
    private string GetXmlContent(InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, Invoice parent)
    {
      string functionValue = String.Empty;
      List<InvoiceContent> itemsList = new List<InvoiceContent>();
      foreach (InvoiceItemXml item in invoiceEntries)
      {
        functionValue = String.IsNullOrEmpty(functionValue) ? item.Bill_doc.ToString() : functionValue + "; " + item.Bill_doc.ToString();
        InvoiceContent newInvoiceContent = new InvoiceContent()
        {
          Batch = item.Batch.Trim(),
          BatchLookup = Batch.GetLookup(edc, item.Batch.Trim()),
          InvoiceLookup = parent,
          ItemNo = item.Item.ConvertToDouble(),
          ProductType = SKU.GetLookup(edc, item.Material.Trim()).ProductType,
          Quantity = item.Bill_qty_in_SKU.ConvertToDouble(),
          SKU = item.Material.Trim(),
          SKUDescription = "To be removed",
          Tytuł = item.Description,
          Units = item.BUn
        };
      }
      return functionValue;
    }
    private const string m_Title = "Stock Message Import";
  }
}
