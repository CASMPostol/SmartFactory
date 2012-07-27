using System;
using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceContent
  {
    #region public
    internal static void GetXmlContent( InvoiceXml xml, EntitiesDataContext edc, InvoiceLib entry )
    {
      string number = String.Empty;
      DateTime date = CAS.SharePoint.Extensions.DateTimeNull;
      bool _ok = InvoiceContent.GetXmlContent( xml.Item, edc, entry, out number, out date );
      entry.ClearenceListLookup = null;
      entry.BillDoc = number;
      entry.CreationDate = date;
      entry.OK = _ok;
      edc.SubmitChanges();
    }
    #endregion

    #region private
    private static bool GetXmlContent( InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, InvoiceLib parent, out string number, out DateTime date )
    {
      number = String.Empty;
      date = CAS.SharePoint.Extensions.DateTimeNull;
      bool _result = true;
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        InvoiceContent _ic = null;
        try
        {
          _ic = new InvoiceContent( edc, parent, item );
          _result &= _ic.Status.Value == Linq.IPR.Status.OK;
        }
        catch ( Exception ex )
        {
          string _msg = "Cannot create new entry for the invoice No={0}/{1}, SKU={2}, because of error: {3}";
          Anons.WriteEntry( edc, "Invoice import", String.Format( _msg, item.Bill_doc, item.Item, item.Description, ex.Message ) );
        }
        if ( String.IsNullOrEmpty( number ) )
        {
          number = item.Bill_doc.ToString();
          date = item.Created_on;
        }
        edc.InvoiceContent.InsertOnSubmit( _ic );
        edc.SubmitChanges();
      }
      return _result;
    }
    private InvoiceContent( EntitiesDataContext edc, InvoiceLib parent, InvoiceItemXml item ) :
      this()
    {
      this.BatchID = Linq.IPR.Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceLookup = parent;
      ItemNo = item.Item.ConvertToDouble();
      ProductType = this.BatchID.ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      this.CreateTitle();
      //TODO  SKUTitle = item.Description;
      Units = item.BUn;
      this.Status = Linq.IPR.Status.OK;
      if ( this.BatchID.BatchStatus.Value == BatchStatus.Preliminary )
        this.Status = Linq.IPR.Status.BatchNotFound;
      else if ( this.BatchID.Available(Quantity.Value))
        this.Status = Linq.IPR.Status.NotEnoughQnt;
    }
    #endregion
  }
}
