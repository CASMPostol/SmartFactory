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
      bool _ok = InvoiceContent.GetXmlContent( xml.Item, edc, entry );
      entry.ClearenceListLookup = null;
      entry.OK = _ok;
      entry.ReadOnly = false;
      edc.SubmitChanges();
    }
    #endregion

    #region private
    private static bool GetXmlContent( InvoiceItemXml[] invoiceEntries, EntitiesDataContext edc, InvoiceLib parent )
    {
      bool _result = true;
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        InvoiceContent _ic = null;
        try
        {
          _ic = new InvoiceContent( edc, parent, item );
          _result &= _ic.Status.Value == Linq.IPR.Status.OK;
          if ( parent.BillDoc.IsNullOrEmpty() )
          {
            parent.BillDoc = item.Bill_doc.ToString();
            parent.CreationDate = item.Created_on;
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
    private InvoiceContent( EntitiesDataContext edc, InvoiceLib parent, InvoiceItemXml item ) :
      this()
    {
      this.BatchID = Linq.IPR.Batch.GetOrCreatePreliminary( edc, item.Batch );
      InvoiceLookup = parent;
      ItemNo = item.Item.ConvertToDouble();
      ProductType = this.BatchID.ProductType;
      Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      //TODO  SKUTitle = item.Description;
      Units = item.BUn;
      Tytuł = "Creating";
      this.Status = Linq.IPR.Status.OK;
      if ( this.BatchID.BatchStatus.Value == BatchStatus.Preliminary )
        this.Status = Linq.IPR.Status.BatchNotFound;
      else if ( this.BatchID.Available( Quantity.Value ) )
        this.Status = Linq.IPR.Status.NotEnoughQnt;
    }
    #endregion
  }
}
