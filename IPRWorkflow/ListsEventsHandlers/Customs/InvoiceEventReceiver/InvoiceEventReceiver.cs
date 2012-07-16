using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using InvoiceItemXml = CAS.SmartFactory.xml.erp.InvoiceItem;
using InvoiceXml = CAS.SmartFactory.xml.erp.Invoice;
using System.IO;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class InvoiceEventReceiver : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if (!properties.List.Title.Contains("Invoice"))
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        base.ItemAdded(properties);
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
        (object obj, ProgressChangedEventArgs progres) => { return; }
      );
      this.EventFiringEnabled = true;
      base.ItemAdded(properties);
    }
    public static void IportInvoiceFromXml(Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      try
      {
        edc = new EntitiesDataContext(url);
        String message = String.Format("Import of the invoice message {0} starting.", fileName);
        Anons.WriteEntry(edc, m_Title, message);
        edc.SubmitChanges();
        InvoiceXml document = InvoiceXml.ImportDocument(stream);
        InvoiceLib entry = Element.GetAtIndex<InvoiceLib>(edc.InvoiceLibrary, listIndex);
        InvoiceContent.GetXmlContent(document, edc, entry);
        Anons.WriteEntry(edc, m_Title, "Import of the stock message finished");
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(edc, "Aborted Invoice message import because of the error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          edc.SubmitChanges();
          edc.Dispose();
        }
      }
    }
    private const string m_Title = "Stock Message Import";
  }
}
