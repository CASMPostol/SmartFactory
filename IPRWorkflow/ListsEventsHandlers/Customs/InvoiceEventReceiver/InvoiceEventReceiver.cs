using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.Linq.IPR;
using Microsoft.SharePoint;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class InvoiceEventReceiver: SPItemEventReceiver
  {
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
    public static void IportInvoiceFromXml( Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        using ( Entities edc = new Entities( url ) )
        {
          String message = String.Format( "Import of the invoice message {0} starting.", fileName );
          Anons.WriteEntry( edc, m_Title, message );
          InvoiceXml document = InvoiceXml.ImportDocument( stream );
          InvoiceLib entry = Element.GetAtIndex<InvoiceLib>( edc.InvoiceLibrary, listIndex );
          InvoiceContentExtension.GetXmlContent( document, edc, entry );
          Anons.WriteEntry( edc, m_Title, "Import of the invoice message finished" );
        }
      }
      catch ( Exception ex )
      {
        using ( Entities edc = new Entities( url ) )
          Anons.WriteEntry( edc, "Aborted Invoice message import because of the error", ex.Message );
      }
    }
    private const string m_Title = "Invoice Message Import";
  }
}
