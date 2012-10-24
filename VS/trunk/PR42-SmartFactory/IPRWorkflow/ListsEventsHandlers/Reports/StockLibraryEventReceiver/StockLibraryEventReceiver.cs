using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.Linq.IPR;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using StockXml = CAS.SmartFactory.xml.erp.Stock;

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
        StockExtension.IportXml( document, edc, entry, progressChanged );
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
    private const string m_Title = "Stock Message Import";
  }
}
