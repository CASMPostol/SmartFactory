using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class BatchEventReceiver : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if (!properties.List.Title.Contains("Batch"))
        return;
      this.EventFiringEnabled = false;
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of a batch xml message failed because the file is empty.");
      //  return;
      //}
      ImportBatchFromXml(
        properties.ListItem.File.OpenBinaryStream(),
        properties.WebUrl,
        properties.ListItem.ID,
        properties.ListItem.File.ToString(),
        (object obj, ProgressChangedEventArgs progres) => { return; });
      this.EventFiringEnabled = true;
      base.ItemAdded(properties);
    }
    public static void ImportBatchFromXml(Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      try
      {
        edc = new EntitiesDataContext(url);
        Anons.WriteEntry(edc, m_Title, String.Format(m_Message, fileName));
        edc.SubmitChanges();
        BatchXml xml = BatchXml.ImportDocument(stream);
        Dokument entry = Dokument.GetEntity(listIndex, edc.BatchLibrary);
        Batch.GetXmlContent(xml, edc, entry);
        Anons.WriteEntry(edc, m_Title, "Import of the batch message finished");
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        Anons.WriteEntry(edc, "Batch message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
          edc.Dispose();
        }
      }
    }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the batch message {0} starting.";
  }
}
