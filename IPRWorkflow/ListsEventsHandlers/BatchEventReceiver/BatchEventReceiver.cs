using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CAS.SharePoint;
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
    /// <param name="_edc">The _edc.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="_entry">The _entry.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed delegate <see cref="ProgressChangedEventHandler" />.</param>
    /// <exception cref="IPRDataConsistencyException"></exception>
    public static void ImportBatchFromXml( Entities _edc, Stream stream, BatchLib _entry, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        progressChanged( null, new ProgressChangedEventArgs( 1, "Importing XML" ) );
        Anons.WriteEntry( _edc, m_Title, String.Format( m_Message, fileName ) );
        _edc.SubmitChanges();
        BatchXml _xml = BatchXml.ImportDocument( stream );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Getting Data" ) );
        GetXmlContent( _xml, _edc, _entry, progressChanged );
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
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The entry.</param>
    private static void GetXmlContent( BatchXml xml, Entities edc, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      SummaryContentInfo contentInfo = new Content( xml.Material, edc, progressChanged );
      Batch batch =
          ( from idx in edc.Batch where idx.Batch0.Contains( contentInfo.Product.Batch ) && idx.BatchStatus.Value == BatchStatus.Preliminary select idx ).FirstOrDefault();
      if ( batch == null )
      {
        batch = new Batch();
        edc.Batch.InsertOnSubmit( batch );
      }
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: BatchProcessing" ) );
      batch.BatchProcessing( edc, GetBatchStatus( xml.Status ), contentInfo, parent, progressChanged );
    }

    private static BatchStatus GetBatchStatus( xml.erp.BatchStatus batchStatus )
    {
      switch ( batchStatus )
      {
        case CAS.SmartFactory.xml.erp.BatchStatus.Final:
          return BatchStatus.Final;
        case CAS.SmartFactory.xml.erp.BatchStatus.Intermediate:
          return BatchStatus.Intermediate;
        case CAS.SmartFactory.xml.erp.BatchStatus.Progress:
          return BatchStatus.Progress;
        default:
          return BatchStatus.Preliminary;
      }
    }
    private class Content: SummaryContentInfo
    {
      internal Content( BatchMaterialXml[] xml, Entities edc, ProgressChangedEventHandler progressChanged ): base()
      {
        foreach ( BatchMaterialXml item in xml )
        {
          Material _newMaterial = new Material( edc, item.Batch, item.Material, item.Stor__Loc, item.Material_description, item.Unit, item.Quantity, item.Quantity_calculated, item.material_group );
          progressChanged( null, new ProgressChangedEventArgs( 1, String.Format( "SKU={0}", _newMaterial.SKU ) ) );
          Add( _newMaterial );
        }
        progressChanged( null, new ProgressChangedEventArgs( 1, "CheckConsistence" ) );
        CheckConsistence();
        progressChanged( this, new ProgressChangedEventArgs( 1, "SummaryContentInfo created" ) );
      }
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";

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
