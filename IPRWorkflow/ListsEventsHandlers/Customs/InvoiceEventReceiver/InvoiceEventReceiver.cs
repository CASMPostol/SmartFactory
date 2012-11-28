using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel;
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
      catch ( CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException _iove )
      {
        _iove.ReportActionResult( url, fileName );
        ActivityLogCT.WriteEntry( m_Title, "Import of the invoice message failed", url );
      }
      catch ( Exception ex )
      {
        ActivityLogCT.WriteEntry( "Aborted Invoice message import because of the error", ex.Message, url );
        ActivityLogCT.WriteEntry( m_Title, "Import of the invoice message failed", url );
      }
    }
    #endregion

    #region private
    internal static void GetXmlContent( InvoiceXml xml, Entities edc, InvoiceLib entry )
    {
      entry.ClearenceIndex = null;
      entry.InvoiceLibraryReadOnly = false;
      entry.BillDoc = String.Empty.NotAvailable();
      try
      {
        entry.InvoiceLibraryStatus = GetXmlContent( xml.Item, edc, entry );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        entry.InvoiceLibraryStatus = false;
        edc.SubmitChanges();
        throw ex;
      }
    }
    private static bool GetXmlContent( InvoiceItemXml[] invoiceEntries, Entities edc, InvoiceLib parent )
    {
      List<InvoiceContent> _invcs = new List<InvoiceContent>();
      List<string> _warnings = new List<string>();
      bool _result = true;
      foreach ( InvoiceItemXml item in invoiceEntries )
      {
        try
        {
          InvoiceContent _ic = CreateInvoiceContent( edc, parent, item, _warnings );
          if ( _ic == null )
            continue;
          _invcs.Add( _ic );
          _result &= _ic.InvoiceContentStatus.Value == InvoiceContentStatus.OK;
          if ( parent.BillDoc.IsNullOrEmpty() )
          {
            parent.BillDoc = item.Bill_doc.ToString();
            parent.InvoiceCreationDate = item.Created_on;
          }
        }
        catch ( Exception ex )
        {
          string _msg = "Cannot create new entry for the invoice No={0}/{1}, SKU={2}, because of error: {3}";
          _warnings.Add( String.Format( _msg, item.Bill_doc, item.Item, item.Description, ex.Message ) );
        }
      }
      if ( _warnings.Count > 0 )
        throw new InputDataValidationException( "there are fatal errors in the XML message.", "GetBatchLookup", _warnings );
      edc.InvoiceContent.InsertAllOnSubmit( _invcs );
      edc.SubmitChanges();
      foreach ( InvoiceContent _ic in _invcs )
        _ic.CreateTitle();
      edc.SubmitChanges();
      return _result;
    }
    private static InvoiceContent CreateInvoiceContent( Entities edc, InvoiceLib parent, InvoiceItemXml item, List<string> errors )
    {
      IQueryable<Batch> _batches = Batch.FindAll( edc, item.Batch );
      if ( _batches.Count<Batch>() == 0 )
      {
        errors.Add( String.Format( "Cannot find batch {0} for stock record {1}.", item.Batch, item.Description ) );
        return null;
      }
      InvoiceContentStatus _invoiceContentStatus = InvoiceContentStatus.OK;
      double? _Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      if ( !( _batches.Sum<Batch>( x => x.FGQuantityAvailable.Value ) < _Quantity.Value ) )
        _invoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
      Batch _oldestBatch = _batches.First<Batch>();
      return new InvoiceContent()
      {
        InvoiceContent2BatchIndex = _oldestBatch,
        InvoiceIndex = parent,
        SKUDescription = item.Description,
        ProductType = _oldestBatch.ProductType,
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
