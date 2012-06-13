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
      try
      {
        if (!properties.List.Title.Contains("Batch Library"))
          throw new IPRDataConsistencyException(m_Title, "Wrong library name", null, "Wrong library name");
        this.EventFiringEnabled = false;
        ImportBatchFromXml(
          properties.ListItem.File.OpenBinaryStream(),
          properties.WebUrl,
          properties.ListItem.ID,
          properties.ListItem.File.ToString(),
          (object obj, ProgressChangedEventArgs progres) => { return; });
      }
      catch (Exception _ex)
      {
        using (Entities.EntitiesDataContext _edc = new EntitiesDataContext(properties.WebUrl))
        {
          Anons.WriteEntry(_edc, _ex.Message, _ex.Message);
        }
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded(properties);
    }
    /// <summary>
    /// Imports the batch from XML.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="url">The URL.</param>
    /// <param name="listIndex">Index of the list.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed delegate <see cref="ProgressChangedEventHandler"/>.</param>
    public static void ImportBatchFromXml(Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      try
      {
        progressChanged(null, new ProgressChangedEventArgs(1, "Importing XML"));
        edc = new EntitiesDataContext(url);
        Anons.WriteEntry(edc, m_Title, String.Format(m_Message, fileName));
        edc.SubmitChanges();
        BatchXml xml = BatchXml.ImportDocument(stream);
        progressChanged(null, new ProgressChangedEventArgs(1, "Finding lookup to the library"));
        Dokument entry = Element.GetAtIndex<Dokument>(edc.BatchLibrary, listIndex);
        progressChanged(null, new ProgressChangedEventArgs(1, "Getting Data"));
        Batch.GetXmlContent(xml, edc, entry, progressChanged);
        progressChanged(null, new ProgressChangedEventArgs(1, "Submiting Changes"));
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
