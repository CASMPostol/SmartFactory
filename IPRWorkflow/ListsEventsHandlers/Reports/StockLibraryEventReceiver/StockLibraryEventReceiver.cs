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
    public static void IportStockFromXML
      ( Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        using ( Entities _edc = new Entities( url ) )
        {
          String _message = String.Format( "Import of the stock message {0} starting.", fileName );
          ActivityLogCT.WriteEntry( _edc, m_Title, _message );
          _edc.SubmitChanges();
          StockXml document = StockXml.ImportDocument( stream );
          Dokument entry = Element.GetAtIndex<Dokument>( _edc.StockLibrary, listIndex );
          IportXml( document, _edc, entry, progressChanged );
          progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes" ) );
          _edc.SubmitChanges();
          _message = String.Format( "Import of the stock message {0} finished.", fileName );
          ActivityLogCT.WriteEntry( _edc, m_Title, _message );
        }
      }
      catch ( CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException _iove )
      {
        _iove.ReportActionResult( url, fileName );
      }
      catch ( Exception ex )
      {
        ActivityLogCT.WriteEntry( "Stock message import error", ex.Message, url );
      }
    }
    #endregion

    #region private
    private static void IportXml( StockXml document, Entities edc, Dokument entry, ProgressChangedEventHandler progressChanged )
    {
      Stock newStock = new Stock( entry, edc );
      edc.Stock.InsertOnSubmit( newStock );
      List<StockEntry> stockEntities = new List<StockEntry>();
      List<string> _warnings = new List<string>();
      foreach ( StockXmlRow _row in document.Row )
      {
        try
        {
          StockEntry nse = CreateStockEntry( _row, newStock );
          nse.ProcessEntry( edc, _warnings );
          progressChanged( _row, new ProgressChangedEventArgs( 1, _row.Material ) );
          stockEntities.Add( nse );
        }
        catch ( Exception ex )
        {
          _warnings.Add( String.Format( "Stock entry {1} fatal import error: {0}", ex.ToString(), _row.MaterialDescription ) );
        }
      }
      if ( _warnings.Count > 0 )
        throw new InputDataValidationException( "there are fatal errors in the XML message.", "GetBatchLookup", _warnings );
      if ( stockEntities.Count > 0 )
        edc.StockEntry.InsertAllOnSubmit( stockEntities );
    }
    private static StockEntry CreateStockEntry( StockXmlRow xml, Stock parent )
    {
      return new StockEntry()
      {
        StockIndex = parent,
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
    private const string m_Title = "Stock Message Import";
    #endregion

  }
}
