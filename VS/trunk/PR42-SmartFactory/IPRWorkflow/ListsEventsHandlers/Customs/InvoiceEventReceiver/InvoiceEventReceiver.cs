using System;
using System.ComponentModel;
using System.IO;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class InvoiceEventReceiver: SPItemEventReceiver
  {

    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded( SPItemEventProperties properties )
    {
      if ( !properties.List.Title.Contains( "Invoice Library" ) )
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        base.ItemAdded( properties );
        return;
      }
      this.EventFiringEnabled = false;
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of an invoice xml message failed because the file is empty.");
      //  return;
      //}
      IportInvoiceFromXml
      (
        properties.ListItem.File.OpenBinaryStream(),
        properties.WebUrl,
        properties.ListItem.ID,
        properties.ListItem.File.ToString(),
        ( object obj, ProgressChangedEventArgs progres ) => { return; }
      );
      this.EventFiringEnabled = true;
      base.ItemAdded( properties );
    }
    /// <summary>
    /// Iports the invoice from XML.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="url">The URL.</param>
    /// <param name="listIndex">Index of the list.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void IportInvoiceFromXml( Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        using ( Entities edc = new Entities( url ) )
        {
          String message = String.Format( "Import of the invoice message {0} starting.", fileName );
          ActivityLogCT.WriteEntry( edc, m_Title, message );
          InvoiceXml document = InvoiceXml.ImportDocument( stream );
          InvoiceLib entry = Element.GetAtIndex<InvoiceLib>( edc.InvoiceLibrary, listIndex );
          GetXmlContent( document, edc, entry );
          ActivityLogCT.WriteEntry( edc, m_Title, "Import of the invoice message finished" );
        }
      }
      catch ( Exception ex )
      {
        using ( Entities edc = new Entities( url ) )
          ActivityLogCT.WriteEntry( edc, "Aborted Invoice message import because of the error", ex.Message );
      }
    }
    #endregion

    #region private
    internal static void GetXmlContent( InvoiceXml xml, Entities edc, InvoiceLib entry )
    {
      bool _ok = GetXmlContent( xml.Item, edc, entry );
      entry.ClearenceIndex = null;
      entry.InvoiceLibraryStatus = _ok;
      entry.InvoiceLibraryReadOnly = false;
      edc.SubmitChanges();
    }
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
          ActivityLogCT.WriteEntry( edc, "Invoice import", String.Format( _msg, item.Bill_doc, item.Item, item.Description, ex.Message ) );
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
    private const string m_Title = "Invoice Message Import";
    #endregion
  }
}
