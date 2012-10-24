using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Dictionaries
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SKUEventHandlers : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if ( !properties.List.Title.Contains( "SKU Library" ) )
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        base.ItemAdded(properties);
        return;
      }
      //TODo remove or implement
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of a SKU xml message failed because the file is empty.");
      //  return;
      //}
      this.EventFiringEnabled = false;
      SKUEvetReceiher(
        properties.ListItem.File.OpenBinaryStream(),
        properties.WebUrl,
        properties.ListItem.ID,
        properties.ListItem.File.ToString(),
        (object obj, ProgressChangedEventArgs progres) => { return; });
      this.EventFiringEnabled = true;
      base.ItemAdded(properties);
    }
    public static void SKUEvetReceiher
      (Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged)
    {
      Entities edc = null;
      try
      {
        edc = new Entities(url);
        String message = String.Format("Import of the SKU message {0} starting.", listIndex);
        Anons.WriteEntry(edc, m_Title, message);
        SKUXml xml = SKUXml.ImportDocument(stream);
        Dokument entry = Element.GetAtIndex<Dokument>(edc.SKULibrary, listIndex);
        SKUGetFromXML.GetXmlContent(xml, edc, entry, progressChanged);
        progressChanged(null, new ProgressChangedEventArgs(1, "Submiting Changes"));
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(edc, "SKU message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of the message finished");
          edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
          edc.Dispose();
        }
      }
    }
    private const string m_Title = "SKU Message Import";
  }
}



