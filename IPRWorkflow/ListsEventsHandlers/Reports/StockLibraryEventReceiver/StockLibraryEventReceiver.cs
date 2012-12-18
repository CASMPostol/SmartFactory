using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using StockXml = CAS.SmartFactory.xml.erp.Stock;
using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Reports
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class StockLibraryEventReceiver: SPItemEventReceiver
  {
    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded( SPItemEventProperties properties )
    {
      if ( !properties.List.Title.Contains( "Stock Library" ) )
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        base.ItemAdded( properties );
        return;
      }
      this.EventFiringEnabled = false;
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of a stock xml message failed because the file is empty.");
      //  edc.SubmitChanges();
      //  return;
      //}
      IportStockFromXML
        (
          properties.ListItem.File.OpenBinaryStream(),
          properties.WebUrl,
          properties.ListItem.ID,
          properties.ListItem.File.Name,
          ( object obj, ProgressChangedEventArgs progres ) => { return; }
        );
      this.EventFiringEnabled = true;
      base.ItemAdded( properties );
    }
    /// <summary>
    /// Iports the stock from XML.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="url">The URL.</param>
    /// <param name="listIndex">Index of the list.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void IportStockFromXML( Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        using ( Entities _edc = new Entities( url ) )
        {
          String _message = String.Format( "Import of the stock message {0} starting.", fileName );
          ActivityLogCT.WriteEntry( _edc, m_Source, _message );
          StockXml document = StockXml.ImportDocument( stream );
          StockLib entry = Element.GetAtIndex<StockLib>( _edc.StockLibrary, listIndex );
          ErrorsList _warnings = new ErrorsList();
          IportXml( document, _edc, entry, _warnings, progressChanged );
          InputDataValidationException _exc = new InputDataValidationException( "there are errors in the stockm XML message.", "GetBatchLookup", _warnings );
          switch ( _exc.Valid )
          {
            case InputDataValidationException.Result.Success:
              break;
            case InputDataValidationException.Result.Warnings:
              _exc.ReportActionResult( url, fileName );
              break;
            case InputDataValidationException.Result.FatalErrors:
              throw _exc;
          }
          progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes - Warnings" ) );
          _edc.SubmitChanges();
          _message = String.Format( "Import of the stock message {0} finished.", fileName );
          ActivityLogCT.WriteEntry( _edc, m_Source, _message );
        }
      }
      catch ( WebsiteModel.InputDataValidationException _iove )
      {
        _iove.ReportActionResult( url, fileName );
        ActivityLogCT.WriteEntry( String.Format( m_Source, "Import of the stock message {0} failed.", fileName ), url );
      }
      catch ( Exception ex )
      {
        ActivityLogCT.WriteEntry( "Stock message import fatal error", ex.Message, url );
      }
    }
    #endregion

    #region private
    private static void IportXml( StockXml document, Entities edc, StockLib parent, ErrorsList _warnings, ProgressChangedEventHandler progressChanged )
    {
      List<StockEntry> stockEntities = new List<StockEntry>();
      foreach ( StockXmlRow _row in document.Row )
      {
        try
        {
          StockEntry nse = CreateStockEntry( _row, parent );
          nse.ProcessEntry( edc, _warnings );
          progressChanged( _row, new ProgressChangedEventArgs( 1, _row.Material ) );
          stockEntities.Add( nse );
        }
        catch ( Exception ex )
        {
          _warnings.Add( String.Format( "Stock entry {1} fatal import error: {0}", ex.ToString(), _row.MaterialDescription ), true );
        }
      }
      if ( stockEntities.Count > 0 )
        edc.StockEntry.InsertAllOnSubmit( stockEntities );
    }
    private static StockEntry CreateStockEntry( StockXmlRow xml, StockLib parent )
    {
      return new StockEntry()
      {
        StockLibraryIndex = parent,
        Batch = xml.Batch,
        Blocked = xml.Blocked,
        InQualityInsp = xml.InQualityInsp,
        IPRType = false,
        StorLoc = xml.SLoc,
        RestrictedUse = xml.RestrictedUse,
        SKU = xml.Material,
        Title = xml.MaterialDescription,
        Units = xml.BUn,
        Unrestricted = xml.Unrestricted,
        BatchIndex = null,
        ProductType = ProductType.Invalid,
        Quantity = xml.Blocked.GetValueOrDefault( 0 ) + xml.InQualityInsp.GetValueOrDefault( 0 ) + xml.RestrictedUse.GetValueOrDefault( 0 ) + xml.Unrestricted.GetValueOrDefault( 0 ),
      };
    }
    private const string m_Source = "Stock processing";
    private const string m_AbortMessage = "There are errors while importing the stock message";
    #endregion

  }
}
