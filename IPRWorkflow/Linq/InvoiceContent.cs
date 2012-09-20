using System;
using CAS.SmartFactory.xml;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceContent
  {
    #region public
    internal static void GetXmlContent( InvoiceXml xml, Entities edc, InvoiceLib entry )
    {
      bool _ok = InvoiceContent.GetXmlContent( xml.Item, edc, entry );
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
          _ic = new InvoiceContent( edc, parent, item );
          _result &= _ic.InvoiceContentStatus.Value == Linq.IPR.InvoiceContentStatus.OK;
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
    private InvoiceContent( Entities edc, InvoiceLib parent, InvoiceItemXml item ) :
      this()
    {
      this.InvoiceContent2BatchIndex = Linq.IPR.Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceIndex = parent;
      this.SKUDescription = item.Description;
      ProductType = this.InvoiceContent2BatchIndex.ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      Units = item.BUn;
      Title = "Creating";
      this.InvoiceContentStatus = Linq.IPR.InvoiceContentStatus.OK;
      if ( this.InvoiceContent2BatchIndex.BatchStatus.Value == BatchStatus.Preliminary )
        this.InvoiceContentStatus = Linq.IPR.InvoiceContentStatus.BatchNotFound;
      else if ( !this.InvoiceContent2BatchIndex.Available( Quantity.Value ) )
        this.InvoiceContentStatus = Linq.IPR.InvoiceContentStatus.NotEnoughQnt;
    }
    #endregion
  }
}
