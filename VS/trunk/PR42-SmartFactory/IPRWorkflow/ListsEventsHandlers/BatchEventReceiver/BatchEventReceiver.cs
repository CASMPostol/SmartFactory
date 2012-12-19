using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;
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
          BatchXml _xml = default( BatchXml );
          using ( Stream _stream = _properties.ListItem.File.OpenBinaryStream() )
            _xml = ImportBatchFromXml( _edc, _stream, _properties.ListItem.File.Name,
                                     ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; } );
          At = "Getting Data";
          GetXmlContent( _xml, _edc, _entry, ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; } );
          At = "ListItem assign";
          _entry.BatchLibraryOK = true;
          _entry.BatchLibraryComments = "Batch message import succeeded.";
          At = "SubmitChanges";
          _edc.SubmitChanges();
          ActivityLogCT.WriteEntry( _edc, m_Title, String.Format( "Import of the batch {0} message finished", _properties.ListItem.File.Name ) );
        }
      }
      catch ( InputDataValidationException _idve )
      {
        _idve.ReportActionResult( _properties.WebUrl, _properties.ListItem.File.Name );
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
          ActivityLogCT.WriteEntry( _edc, "BatchEventReceiver.ItemAdded" + " at " + At, _ex.Message );
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
    /// <param name="edc">The _edc.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="entry">The _entry.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed delegate <see cref="ProgressChangedEventHandler" />.</param>
    /// <exception cref="IPRDataConsistencyException"></exception>
    public static BatchXml ImportBatchFromXml( Entities edc, Stream stream, string fileName, ProgressChangedEventHandler progressChanged )
    {
      progressChanged( null, new ProgressChangedEventArgs( 1, "ImportBatchFromXml.starting" ) );
      string _Message = String.Format( m_Message, fileName );
      ActivityLogCT.WriteEntry( edc, m_Title, _Message );
      BatchXml _xml = BatchXml.ImportDocument( stream );
      progressChanged( null, new ProgressChangedEventArgs( 1, "ImportBatchFromXml.Validate" ) );
      ErrorsList _validationErrors = new ErrorsList();
      _xml.Validate( Settings.GetParameter( edc, SettingsEntry.BatchNumberPattern ), _validationErrors );
      if ( _validationErrors.Count > 0 )
        throw new InputDataValidationException( "Batch XML message validation failed", "XML sysntax validation", _validationErrors );
      return _xml;
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The entry.</param>
    /// <param name="progressChanged">The progress changed.</param>
    private static void GetXmlContent( BatchXml xml, Entities edc, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: starting" ) );
      Content _contentInfo = new Content( xml.Material, edc, progressChanged );
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: FindLookup" ) );
      Batch _batch = Batch.FindLookup( edc, _contentInfo.Product.Batch );
      List<string> _warnings = new List<string>();
      BatchStatus _newBtachStatus = GetBatchStatus( xml.Status );
      bool _newBatch = false;
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: switch" ) );
      switch ( _newBtachStatus )
      {
        case BatchStatus.Progress:
          if ( _batch != null )
            throw new InputDataValidationException( "wrong status of the input batch", "Get Xml Content", "The status of Progress is not allowed if any batch has been imported previouly", true );
          _batch = new Batch();
          _newBatch = true;
          break;
        case BatchStatus.Intermediate:
        case BatchStatus.Final:
          if ( _batch != null )
          {
            if ( _batch.BatchStatus.Value != BatchStatus.Intermediate )
            {
              string _ptrn = "The final batch {0} has been analyzed already.";
              throw new InputDataValidationException( "wrong status of the input batch", "Get Xml Content", String.Format( _ptrn, _contentInfo.Product.Batch ), true );
            }
          }
          else
          {
            _batch = new Batch();
            _newBatch = true;
          }
          break;
      }
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: Validate" ) );
      _contentInfo.Validate( edc, _batch.Disposal );
      if ( _newBatch )
        edc.Batch.InsertOnSubmit( _batch );
      _batch.BatchProcessing( edc, _newBtachStatus, _contentInfo, parent, progressChanged, _newBatch );
    }
    private static BatchStatus GetBatchStatus( xml.erp.BatchStatus batchStatus )
    {
      BatchStatus _status = default( BatchStatus );
      switch ( batchStatus )
      {
        case xml.erp.BatchStatus.Final:
          _status = BatchStatus.Final;
          break;
        case xml.erp.BatchStatus.Intermediate:
          _status = BatchStatus.Intermediate;
          break;
        case xml.erp.BatchStatus.Progress:
          _status = BatchStatus.Progress;
          break;
      }
      return _status;
    }
    private class Content: SummaryContentInfo
    {
      internal Content( BatchMaterialXml[] xml, Entities edc, ProgressChangedEventHandler progressChanged )
        : base()
      {
        foreach ( BatchMaterialXml item in xml )
        {
          Entities.ProductDescription product = edc.GetProductType( item.Material, item.Stor__Loc, item.IsFinishedGood );
          Material _newMaterial = new Material( edc, product, item.Batch, item.Material, item.Stor__Loc, item.Material_description, item.Unit, item.Quantity, item.Quantity_calculated, item.material_group );
          progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "SKU={0}", _newMaterial.SKU ) ) );
          Add( _newMaterial );
        }
        if ( Product == null )
          throw new InputDataValidationException( "Unrecognized finished good", "Product", "Wrong Batch XML message", true );
      }
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private string At { get; set; }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the batch message {0} starting.";
    #endregion
  }
}
