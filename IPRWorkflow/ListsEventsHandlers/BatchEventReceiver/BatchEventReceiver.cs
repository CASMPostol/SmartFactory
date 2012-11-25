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
          BatchXml _xml = ImportBatchFromXml
               (
                 _edc,
                 _properties.ListItem.File.OpenBinaryStream(),
                 _properties.ListItem.File.ToString(),
                 ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; }
               );
          At = "Getting Data";
          GetXmlContent( _xml, _edc, _entry, ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; } );
          At = "Submiting Changes";
          ActivityLogCT.WriteEntry( _edc, m_Title, "Import of the batch message finished" );
          At = "ListItem assign";
          _entry.BatchLibraryOK = true;
          _entry.BatchLibraryComments = "Batch message import succeeded.";
          At = "SubmitChanges";
          _edc.SubmitChanges();
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
      try
      {
        string _Message = String.Format( m_Message, fileName );
        progressChanged( null, new ProgressChangedEventArgs( 1, _Message ) );
        ActivityLogCT.WriteEntry( edc, m_Title, _Message );
        BatchXml _xml = BatchXml.ImportDocument( stream );
        progressChanged( null, new ProgressChangedEventArgs( 1, "XML sysntax validation" ) );
        List<string> _validationErrors = new List<string>();
        _xml.Validate( Settings.GetParameter( edc, SettingsEntry.BatchNumberPattern ), _validationErrors );
        if ( _validationErrors.Count > 0 )
          throw new InputDataValidationException( "Batch content validate failed", "XML sysntax validation", _validationErrors );
        return _xml;
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
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: contentInfo.Validate" ) );
      List<string> _validationErrors = new List<string>();
      _contentInfo.Validate( edc, _validationErrors );
      if ( _validationErrors.Count > 0 )
        throw new InputDataValidationException( "Batch content validate failed", "XML content validation", _validationErrors );
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: batch" ) );
      IQueryable<Batch> _batches = ( from idx in edc.Batch where idx.Batch0.Contains( _contentInfo.Product.Batch ) select idx );
      Batch batch = null;
      BatchStatus _newBtachStatus = GetBatchStatus( xml.Status );
      List<string> _warnings = new List<string>();
      switch ( _newBtachStatus )
      {
        case BatchStatus.Progress:
          throw new InputDataValidationException( "Wrong status of the input batch", "Get Xml Content", "The status of Progress is not implemented yet" );
        case BatchStatus.Intermediate:
          throw new InputDataValidationException( "Wrong status of the input batch", "Get Xml Content", "The status of Intermediate is not implemented yet" );
        case BatchStatus.Final:
          if ( _batches.Count<Batch>() == 0 )
          {
            batch = new Batch();
            edc.Batch.InsertOnSubmit( batch );
          }
          else if ( _batches.Where<Batch>( prdc => prdc.BatchStatus.Value == BatchStatus.Final ).Any<Batch>() )
          {
            string _ptrn = "The batch {0} has been analyzed already.";
            throw new InputDataValidationException( "Wrong status of the input batch", "Get Xml Content", String.Format( _ptrn, _contentInfo.Product.Batch ) );
          }
          else if ( _batches.Count<Batch>() == 1 )
          {
            batch = _batches.FirstOrDefault<Batch>();
            if ( batch.BatchStatus.Value != BatchStatus.Preliminary )
              throw new IPRDataConsistencyException( "GetXmlContent", "Wrong batch status - internal error", null, "InternalBufferOverflowException error" );
            progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: BatchProcessing" ) );
            batch.BatchProcessing( edc, _newBtachStatus, _contentInfo, parent, progressChanged );
          }
          else
            throw new InputDataValidationException( "Wrong status of the input batch", "Get Xml Content", "The association of many batches is not implemented yet." );
          break;
      }
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
