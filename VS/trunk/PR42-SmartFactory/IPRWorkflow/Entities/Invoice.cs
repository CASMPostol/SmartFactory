using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml;

namespace CAS.SmartFactory.IPR.Entities
{
  /// <summary>
  /// Invoice class
  /// </summary>
  public partial class Invoice
  {
    internal static void GetXmlContent(InvoiceXml xml, EntitiesDataContext edc, InvoiceLibraryInvoiceLib entry)
    {
      Invoice newInvoice = new Invoice(entry);
      newInvoice.BillDoc = Entities.InvoiceContent.GetXmlContent(xml.Item, edc, newInvoice);
      edc.Invoice.InsertOnSubmit(newInvoice);
    }
    private Invoice(InvoiceLibraryInvoiceLib entry)
      : this()
    {
      InvoiceLibraryLookup = entry;
      this.BillDoc = String.Empty;
      //this.InvoiceContent - backward
      this.Tytuł = String.Empty;
    }
  }
}
