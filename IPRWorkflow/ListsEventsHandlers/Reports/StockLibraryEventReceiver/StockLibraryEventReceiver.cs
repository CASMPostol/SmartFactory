using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.Linq.IPR;
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
        properties.ListItem.File.ToString(),
        ( object obj, ProgressChangedEventArgs progres ) => { return; }
        );
      this.EventFiringEnabled = true;
      base.ItemAdded( properties );
    }
    public static void IportStockFromXML
      ( Stream stream, string url, int listIndex, string fileName, ProgressChangedEventHandler progressChanged )
    {
      Entities edc = null;
      try
      {
        edc = new Entities( url );
        String message = String.Format( "Import of the stock message {0} starting.", fileName );
        Anons.WriteEntry( edc, m_Title, message );
        edc.SubmitChanges();
        StockXml document = StockXml.ImportDocument( stream );
        Dokument entry = Element.GetAtIndex<Dokument>( edc.StockLibrary, listIndex );
        IportXml( document, edc, entry, progressChanged );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes" ) );
        Anons.WriteEntry( edc, m_Title, "Import of the stock message finished" );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        Anons.WriteEntry( edc, "Stock message import error", ex.Message );
      }
      finally
      {
        if ( edc != null )
        {
          edc.SubmitChangesSilently( RefreshMode.KeepCurrentValues );
          edc.Dispose();
        }
      }
    }
    private static void IportXml( StockXml document, Entities edc, Dokument entry, ProgressChangedEventHandler progressChanged )
    {
      Stock newStock = new Stock( entry, edc );
      edc.Stock.InsertOnSubmit( newStock );
      List<StockEntry> stockEntities = new List<StockEntry>();
      bool errors = false;
      foreach ( StockXmlRow item in document.Row )
      {
        try
        {
          StockEntry nse = CreateStockEntry( item, newStock );
          nse.ProcessEntry( edc );
          progressChanged( item, new ProgressChangedEventArgs( 1, item.Material ) );
          stockEntities.Add( nse );
        }
        catch ( Exception ex )
        {
          Anons.WriteEntry( edc, "Stock entry import error", ex.Message );
          errors = true;
        }
      }
      if ( errors )
        throw new IPRDataConsistencyException( m_Source, m_AbortMessage, null, "Stock import error" );
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
  }
}
