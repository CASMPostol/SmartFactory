using System;
using System.Collections.Generic;
using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceContent
  {
    internal static void GetXmlContent( InvoiceXml xml, EntitiesDataContext edc, InvoiceLib entry )
    {
      string number = String.Empty;
      DateTime date = CAS.SharePoint.Extensions.DateTimeNull;
      InvoiceContent.GetXmlContent( xml.Item, edc, entry, out number, out date );
      entry.BillDoc = number;
      //TODO [pr4-3465] Invoice Content list - add fields http://itrserver/Bugs/BugDetail.aspx?bid=3465
      //entry.CreationDate = date;
      edc.SubmitChanges();
    }
    internal static void GetXmlContent( InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, InvoiceLib parent, out string number, out DateTime date )
    {
      number = String.Empty;
      date = CAS.SharePoint.Extensions.DateTimeNull;
      string functionValue = String.Empty;
      List<InvoiceContent> itemsList = new List<InvoiceContent>();
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        InvoiceContent ic = new InvoiceContent( edc, parent, item );
        itemsList.Add( ic );
        if ( String.IsNullOrEmpty( functionValue ) )
        {
          number = item.Bill_doc.ToString();
          date = item.Created_on;
        }
      }
      if ( itemsList.Count > 0 )
        edc.InvoiceContent.InsertAllOnSubmit( itemsList );
      edc.SubmitChanges();
    }
    private InvoiceContent( EntitiesDataContext edc, InvoiceLib parent, InvoiceItemXml item ) :
      this()
    {
      this.BatchID = Linq.IPR.Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceLookup = parent;
      ItemNo = item.Item.ConvertToDouble();
      ProductType = this.BatchID.ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      this.BatchID.FGQuantityAvailable -= Quantity;
      Tytuł = item.Description;
      Units = item.BUn;
      //TODO [pr4-3465] Invoice Content list - add fields http://itrserver/Bugs/BugDetail.aspx?bid=3465
      if ( this.BatchID.BatchStatus.Value == BatchStatus.Preliminary )
      {
        this.Status = false;
        this.Error = InvoiceState.BatchNotFount;
      }
      else if ( this.BatchID.FGQuantityAvailable < 0 )
      {
        this.Status = false;
        this.Error = InvoiceState.NotEnouchQuantity;
      }
    }
    //TODO [pr4-3465] Invoice Content list - add fields http://itrserver/Bugs/BugDetail.aspx?bid=3465
    private enum InvoiceState { OK, BatchNotFount, NotEnouchQuantity };
    private InvoiceState Error = InvoiceState.OK;
  }
}
