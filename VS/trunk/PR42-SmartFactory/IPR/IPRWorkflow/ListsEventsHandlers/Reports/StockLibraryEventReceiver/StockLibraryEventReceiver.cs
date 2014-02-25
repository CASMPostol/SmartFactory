//<summary>
//  Title   : class StockLibraryEventReceiver
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CAS.SharePoint;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
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
      if ( !properties.ListTitle.Contains( "Stock Library" ) )
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        base.ItemAdded( properties );
        return;
      }
      this.EventFiringEnabled = false;
      try
      {
        properties.List.Update();
        //if (properties.ListItem.File == null)
        //{
        //  Anons.WriteEntry(edc, m_Title, "Import of a stock xml message failed because the file is empty.");
        //  edc.SubmitChanges();
        //  return;
        //}
        using ( Stream _str = properties.ListItem.File.OpenBinaryStream() )
          IportStockFromXML
            (
              _str,
              properties.WebUrl,
              properties.ListItem.ID,
              properties.ListItem.File.Name,
              ( object obj, ProgressChangedEventArgs progres ) => { return; }
            );
        properties.List.Update();
        properties.Cancel = true;
        properties.Status = SPEventReceiverStatus.CancelNoError;
      }
      catch ( Exception ex )
      {
        ActivityLogCT.WriteEntry( "Stock xml message SPItemEventReceiver fatal error.", ex.Message, properties.WebUrl );
      }
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
          StockLib _entry = Element.GetAtIndex<StockLib>( _edc.StockLibrary, listIndex );
          _entry.Stock2JSOXLibraryIndex = null;
          ErrorsList _warnings = new ErrorsList();
          IportXml( document, _edc, _entry, _warnings, progressChanged );
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
          progressChanged( null, new ProgressChangedEventArgs( 1, "Submitting Changes - Warnings" ) );
          _edc.SubmitChanges();
        }
      }
      catch ( WebsiteModel.InputDataValidationException _iove )
      {
        _iove.ReportActionResult( url, fileName );
        ActivityLogCT.WriteEntry( m_Source, String.Format( "Import of the stock message {0} failed.", fileName ), url );
      }
      catch ( Exception ex )
      {
        ActivityLogCT.WriteEntry( "Stock XML message import fatal error", ex.Message, url );
      }
      ActivityLogCT.WriteEntry( m_Source, String.Format( "Import of the stock message {0} finished.", fileName ), url );
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
          _warnings.Add( new Warnning( String.Format( "Stock entry {1} fatal import error: {0}", ex.ToString(), _row.MaterialDescription ), true ) );
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
        Batch = xml.Batch.NotAvailable(),
        Blocked = xml.Blocked,
        InQualityInsp = xml.InQualityInsp,
        IPRType = false,
        StorLoc = xml.SLoc.NotAvailable(),
        RestrictedUse = xml.RestrictedUse,
        SKU = xml.Material.NotAvailable(),
        Title = xml.MaterialDescription.NotAvailable(),
        Units = xml.BUn.NotAvailable(),
        Unrestricted = xml.Unrestricted,
        BatchIndex = null,
        ProductType = ProductType.Invalid,
        Quantity = xml.Blocked + xml.InQualityInsp + xml.RestrictedUse + xml.Unrestricted,
      };
    }
    private const string m_Source = "Stock processing";
    private const string m_AbortMessage = "There are errors while importing the stock message";
    #endregion

  }
}
