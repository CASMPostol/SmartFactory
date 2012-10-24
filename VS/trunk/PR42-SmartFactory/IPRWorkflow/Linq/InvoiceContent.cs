using System;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class InvoiceContentExtension
  {
    #region public
    internal static void GetXmlContent( InvoiceXml xml, Entities edc, InvoiceLib entry )
    {
      bool _ok = GetXmlContent( xml.Item, edc, entry );
      entry.ClearenceIndex = null;
      entry.InvoiceLibraryStatus = _ok;
      entry.InvoiceLibraryReadOnly = false;
      edc.SubmitChanges();
    }
    #endregion

    #region private
    private static bool GetXmlContent( InvoiceItemXml[] invoiceEntries, Entities edc, InvoiceLib parent )
    {
      bool _result = true;
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        InvoiceContent _ic = null;
        try
        {
          _ic = CreateInvoiceContent( edc, parent, item );
          _result &= _ic.InvoiceContentStatus.Value == InvoiceContentStatus.OK;
          if ( parent.BillDoc.IsNullOrEmpty() )
          {
            parent.BillDoc = item.Bill_doc.ToString();
            parent.InvoiceCreationDate = item.Created_on;
          }
          edc.InvoiceContent.InsertOnSubmit( _ic );
          edc.SubmitChanges();
          _ic.CreateTitle();
          edc.SubmitChanges();
        }
        catch ( Exception ex )
        {
          _result = false;
          string _msg = "Cannot create new entry for the invoice No={0}/{1}, SKU={2}, because of error: {3}";
          Anons.WriteEntry( edc, "Invoice import", String.Format( _msg, item.Bill_doc, item.Item, item.Description, ex.Message ) );
        }
      }
      return _result;
    }
    private static InvoiceContent CreateInvoiceContent( Entities edc, InvoiceLib parent, InvoiceItemXml item )
    {
      Batch _batch = Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceContentStatus _invoiceContentStatus = InvoiceContentStatus.OK;
      double? _Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      if ( _batch.BatchStatus.Value == BatchStatus.Preliminary )
        _invoiceContentStatus = InvoiceContentStatus.BatchNotFound;
      else if ( !_batch.Available( _Quantity.Value ) )
        _invoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
      return new InvoiceContent()
      {
        InvoiceContent2BatchIndex = _batch,
        InvoiceIndex = parent,
        SKUDescription = item.Description,
        ProductType = _batch.ProductType,
        Quantity = _Quantity,
        Units = item.BUn,
        Title = "Creating",
        InvoiceContentStatus = _invoiceContentStatus
      };
    }
    #endregion
  }
}
