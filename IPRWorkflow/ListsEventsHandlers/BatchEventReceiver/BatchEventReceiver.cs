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
    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="_properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded(SPItemEventProperties _properties)
    {
      base.ItemAdded(_properties);
      try
      {
        if (!_properties.List.Title.Contains("Batch Library"))
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          base.ItemAdded(_properties);
          return;
          //throw new IPRDataConsistencyException(m_Title, "Wrong library name", null, "Wrong library name");
        }
        this.EventFiringEnabled = false;
        ImportBatchFromXml(
          _properties.ListItem.File.OpenBinaryStream(),
          _properties.WebUrl,
          _properties.ListItemId,
          _properties.ListItem.File.ToString(),
          (object obj, ProgressChangedEventArgs progres) => { At = (string)progres.UserState; });
        _properties.ListItem[_batchLibraryOK] = true;
        _properties.ListItem[_batchLibraryComments] = "Batch message import succeeded.";
        _properties.ListItem.UpdateOverwriteVersion();
      }
      catch (IPRDataConsistencyException _ex)
      {
        _ex.Source += " at " + At;
        using (Entities.EntitiesDataContext _edc = new EntitiesDataContext(_properties.WebUrl))
          _ex.Add2Log(_edc);
        _properties.ListItem[_batchLibraryOK] = false;
        _properties.ListItem[_batchLibraryComments] = _ex.Comments;
        _properties.ListItem.UpdateOverwriteVersion();
      }
      catch (Exception _ex)
      {
        using (Entities.EntitiesDataContext _edc = new EntitiesDataContext(_properties.WebUrl))
          Anons.WriteEntry(_edc, _ex.Source + " at " + At, _ex.Message);
        string _comments = "Batch message import error";
        _properties.ListItem[_batchLibraryOK] = false;
        _properties.ListItem[_batchLibraryComments] = _comments;
        _properties.ListItem.UpdateOverwriteVersion();
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
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
      EntitiesDataContext _edc = null;
      BatchLib _entry = null;
      try
      {
        progressChanged(null, new ProgressChangedEventArgs(1, "Importing XML"));
        _edc = new EntitiesDataContext(url);
        Anons.WriteEntry(_edc, m_Title, String.Format(m_Message, fileName));
        _edc.SubmitChanges();
        BatchXml _xml = BatchXml.ImportDocument(stream);
        progressChanged(null, new ProgressChangedEventArgs(1, "Finding lookup to the library"));
        _entry = Element.GetAtIndex<BatchLib>(_edc.BatchLibrary, listIndex);
        progressChanged(null, new ProgressChangedEventArgs(1, "Getting Data"));
        Batch.GetXmlContent(_xml, _edc, _entry, progressChanged);
        progressChanged(null, new ProgressChangedEventArgs(1, "Submiting Changes"));
        Anons.WriteEntry(_edc, m_Title, "Import of the batch message finished");
        _edc.SubmitChanges();
      }
      catch (IPRDataConsistencyException _ex)
      {
        throw _ex;
      }
      catch (Exception ex)
      {
        string _src = "BatchEventReceiver.ImportBatchFromXml";
        string _Comments = "Batch message import error";
        throw new IPRDataConsistencyException(_src, ex.Message, ex, _Comments);
      }
      finally
      {
        if (_edc != null)
        {
          progressChanged(null, new ProgressChangedEventArgs(1, "BatchEventReceiver.ImportBatchFromXml.SubmitChangesSilently"));
          _edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
          progressChanged(null, new ProgressChangedEventArgs(1, "BatchEventReceiver.ImportBatchFromXml.Dispose"));
          _edc.Dispose();
        }
      }
    }
    #endregion
    
    #region private
    private const string _batchLibraryOK = "BatchLibraryOK";
    private const string _batchLibraryComments = "BatchLibraryComments";
    private string At { get; set; }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the batch message {0} starting.";
    #endregion
  }
}
