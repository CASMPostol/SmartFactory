using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using System.Collections.Generic;
using System;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class InvoiceContent
  {
    internal static string GetXmlContent(InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, Invoice parent)
    {
      string functionValue = String.Empty;
      List<InvoiceContent> itemsList = new List<InvoiceContent>();
      foreach (InvoiceItemXml item in invoiceEntries)
      {
        InvoiceContent ic = new InvoiceContent(edc, parent, item);
        itemsList.Add(ic);
        if (String.IsNullOrEmpty(functionValue))
          functionValue = item.Bill_doc.ToString();
      }
      if (itemsList.Count > 0)
        edc.InvoiceContent.InsertAllOnSubmit(itemsList);
      return functionValue;
    }
    private InvoiceContent(EntitiesDataContext edc, Invoice parent, InvoiceItemXml item) :
      this()
    {
      Batch = item.Batch;
      BatchLookup = Entities.Batch.GetLookup(edc, item.Batch);
      InvoiceLookup = parent;
      ItemNo = item.Item.ConvertToDouble();
      ProductType = SKUCommonPart.GetLookup(edc, item.Material).ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      SKU = item.Material;
      Tytuł = item.Description;
      Units = item.BUn;
    }
  }
}
