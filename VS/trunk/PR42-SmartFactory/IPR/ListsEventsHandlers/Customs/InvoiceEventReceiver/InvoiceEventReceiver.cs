using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

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
        Dokument entry = Dokument.GetEntity(properties.ListItem.ID, edc.InvoiceLibrary);
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
        if (String.IsNullOrEmpty(functionValue))
          functionValue = item.Bill_doc.ToString();
        InvoiceContent newInvoiceContent = new InvoiceContent()
        {
          Batch = item.Batch.Trim(),
          BatchLookup = Batch.GetLookup(edc, item.Batch.Trim()),
          InvoiceLookup = parent,
          ItemNo = item.Item.ConvertToDouble(),
          ProductType = SKUCommonPart.GetLookup(edc, item.Material.Trim()).ProductType,
          Quantity = item.Bill_qty_in_SKU.ConvertToDouble(),
          SKU = item.Material.Trim(),
          SKUDescription = item.Description.Trim(), //TODO to be removed: http://itrserver/Bugs/BugDetail.aspx?bid=2877
          Tytuł = item.Description.Trim(),
          Units = item.BUn
        };
      }
      if (itemsList.Count != 0)
        edc.InvoiceContent.InsertAllOnSubmit(itemsList);
      return functionValue;
    }
    private const string m_Title = "Stock Message Import";
  }
}
