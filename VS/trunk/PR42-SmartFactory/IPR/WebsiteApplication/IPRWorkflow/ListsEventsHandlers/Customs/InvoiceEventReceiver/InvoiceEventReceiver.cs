using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.Customs;
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
      if ( !properties.ListTitle.Contains( "Invoice Library" ) )
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
      using ( Stream _str = properties.ListItem.File.OpenBinaryStream() )
        IportInvoiceFromXml
        (
          _str,
          properties.WebUrl,
          properties.ListItem.ID,
          properties.ListItem.File.Name,
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
          ActivityLogCT.WriteEntry( edc, m_Title, String.Format( "Import of the invoice message {0} finished", fileName ) );
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
      try
      {
        entry.InvoiceLibraryStatus = GetXmlContent( xml.Item, edc, entry );
        edc.SubmitChanges();
      }
      catch ( Exception )
      {
        entry.BillDoc = String.Empty.NotAvailable();
        entry.InvoiceLibraryStatus = false;
        edc.SubmitChanges();
        throw;
      }
    }
    private static bool GetXmlContent( InvoiceItemXml[] invoiceEntries, Entities edc, InvoiceLib parent )
    {
      List<InvoiceContent> _invcs = new List<InvoiceContent>();
      ErrorsList _warnings = new ErrorsList();
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
          _warnings.Add( new Warnning( String.Format( _msg, item.Bill_doc, item.Item, item.Description, ex.Message ), true ) );
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
    private static InvoiceContent CreateInvoiceContent( Entities edc, InvoiceLib parent, InvoiceItemXml item, ErrorsList errors )
    {
      Batch _batch = Batch.FindLookup( edc, item.Batch );
      if ( _batch == null )
      {
        errors.Add( new Warnning( String.Format( "Cannot find batch {0} for stock record {1}.", item.Batch, item.Description ), true ) );
        return null;
      }
      InvoiceContentStatus _invoiceContentStatus = InvoiceContentStatus.OK;
      double? _Quantity = item.Bill_qty_in_SKU.ConvertToDouble();
      if ( _batch.FGQuantityAvailable.Value < _Quantity.Value )
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
