using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.Linq.IPR;
using Microsoft.SharePoint;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class BatchEventReceiver: SPItemEventReceiver
  {
    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="_properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded( SPItemEventProperties _properties )
    {
      base.ItemAdded( _properties );
      try
      {
        if ( !_properties.List.Title.Contains( "Batch Library" ) )
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          base.ItemAdded( _properties );
          return;
          //throw new IPRDataConsistencyException(m_Title, "Wrong library name", null, "Wrong library name");
        }
        this.EventFiringEnabled = false;
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          At = "ImportBatchFromXml";
          ImportBatchFromXml
            (
              _edc,
              _properties.ListItem.File.OpenBinaryStream(),
              _entry,
              _properties.ListItem.File.ToString(),
              ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; }
            );
          At = "ListItem assign";
          _entry.BatchLibraryOK = true;
          _entry.BatchLibraryComments = "Batch message import succeeded.";
          At = "SubmitChanges";
          _edc.SubmitChanges();
        }
      }
      catch ( IPRDataConsistencyException _ex )
      {
        _ex.Source += " at " + At;
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          _ex.Add2Log( _edc );
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          _entry.BatchLibraryOK = false;
          _entry.BatchLibraryComments = _ex.Comments;
          _edc.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          Anons.WriteEntry( _edc, "BatchEventReceiver.ItemAdded" + " at " + At, _ex.Message );
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          _entry.BatchLibraryComments = "Batch message import error";
          _entry.BatchLibraryOK = false;
          _edc.SubmitChanges();
        }
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
    public static void ImportBatchFromXml( Entities _edc, Stream stream, BatchLib _entry, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        progressChanged( null, new ProgressChangedEventArgs( 1, "Importing XML" ) );
        Anons.WriteEntry( _edc, m_Title, String.Format( m_Message, fileName ) );
        _edc.SubmitChanges();
        BatchXml _xml = BatchXml.ImportDocument( stream );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Getting Data" ) );
        BatchExtension.GetXmlContent( _xml, _edc, _entry, progressChanged );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes" ) );
        Anons.WriteEntry( _edc, m_Title, "Import of the batch message finished" );
        _edc.SubmitChanges();
      }
      catch ( IPRDataConsistencyException _ex )
      {
        throw _ex;
      }
      catch ( Exception ex )
      {
        string _src = "BatchEventReceiver.ImportBatchFromXml";
        string _Comments = "Batch message import error";
        throw new IPRDataConsistencyException( _src, ex.Message, ex, _Comments );
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
