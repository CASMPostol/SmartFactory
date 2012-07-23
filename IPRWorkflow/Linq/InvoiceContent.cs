using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;
using System.Collections.Generic;
using System;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceContent
  {
    internal static void GetXmlContent( InvoiceXml xml, EntitiesDataContext edc, InvoiceLib entry )
    {
      entry.BillDoc = InvoiceContent.GetXmlContent( xml.Item, edc, entry );
      edc.SubmitChanges();
    }
    internal static string GetXmlContent( InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, InvoiceLib parent )
    {
      string functionValue = String.Empty;
      List<InvoiceContent> itemsList = new List<InvoiceContent>();
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        InvoiceContent ic = new InvoiceContent( edc, parent, item );
        itemsList.Add( ic );
        if ( String.IsNullOrEmpty( functionValue ) )
          functionValue = item.Bill_doc.ToString();
      }
      if ( itemsList.Count > 0 )
        edc.InvoiceContent.InsertAllOnSubmit( itemsList );
      return functionValue;
    }
    private InvoiceContent( EntitiesDataContext edc, InvoiceLib parent, InvoiceItemXml item ) :
      this()
    {
      //TODO [pr4-3465] Invoice Content list - add fields http://itrserver/Bugs/BugDetail.aspx?bid=3465
      this.BatchID = Linq.IPR.Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceLookup = parent;
      ItemNo = item.Item.ConvertToDouble();
      ProductType = this.BatchID.ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      this.BatchID.FGQuantityAvailable -= Quantity;
      this.Status = this.BatchID.FGQuantityAvailable >= 0;
      Tytuł = item.Description;
      Units = item.BUn;
    }
  }
}
