using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal static class BalanceSheetContentFactory
  {
    #region public
    internal static void CreateReport( SPWeb web, string webUrl, int jsoxLibItemId )
    {
      SPFile _newFile = default( SPFile );
      BalanceSheetContent _content = default( BalanceSheetContent );
      using ( Entities _edc = new Entities( webUrl ) )
      {
        JSOXLib _old = Element.GetAtIndex<JSOXLib>( _edc.JSOXLibrary, jsoxLibItemId );
        if ( _old.JSOXLibraryReadOnly.Value )
          throw new ApplicationException( "The record is read only and new report must not be created." );
        _old.JSOXLibraryReadOnly = true;
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateEmptyContent();
        string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( jsoxLibItemId + 1 );
        _newFile = SPDocumentFactory.Prepare( web, _content, _documentName );
        _newFile.DocumentLibrary.Update();
        JSOXLib _current = Element.GetAtIndex<JSOXLib>( _edc.JSOXLibrary, _newFile.Item.ID );
        _current.CreateJSOXReport( _edc, _old );
        _edc.SubmitChanges();
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent( _current, _documentName );
      }
      _content.UpdateDocument( _newFile );
      _newFile.DocumentLibrary.Update();
    }
    internal static void UpdateReport( SPListItem listItem, string WebUrl, int jsoxLibItemId )
    {
      BalanceSheetContent _content = null;
      using ( Entities edc = new Entities( WebUrl ) )
      {
        JSOXLib _current = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, jsoxLibItemId );
        if (_current.JSOXLibraryReadOnly.Value)
          throw new ApplicationException( "The record is read only and the report must not be updated." );
        _current.UpdateBalanceReport( edc );
        string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( _current.Identyfikator.Value );
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent( _current, _documentName );
        edc.SubmitChanges();
      }
      _content.UpdateDocument( listItem.File );
      listItem.ParentList.Update();
    }
    #endregion

    #region private
    private static BalanceSheetContent CreateEmptyContent()
    {
      DateTime _default = DateTime.Today.Date;
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = _default,
        DocumentNo = "Temporal report.",
        EndDate = _default,
        IPRStock = null,
        JSOX = null,
        SituationAtDate = _default,
        StartDate = _default
      };
      return _ret;
    }
    private static BalanceSheetContent CreateContent( JSOXLib list, string documentName )
    {
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.GetValueOrDefault(),
        IPRStock = GetIPRStock( list.BalanceBatch ),
        JSOX = GetJSOX( list ),
        SituationAtDate = list.SituationDate.GetValueOrDefault(),
        StartDate = list.PreviousMonthDate.GetValueOrDefault()
      };
      return _ret;
    }
    private static JSOContent GetJSOX( JSOXLib list )
    {
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.GetValueOrDefault(),
        BalanceQuantity = list.BalanceQuantity.GetValueOrDefault(),
        Disposals = GetDisposalsList( list.JSOXCustomsSummary ),
        IntroducingDateEnd = list.IntroducingDateEnd.GetValueOrDefault(),
        IntroducingDateStart = list.IntroducingDateStart.GetValueOrDefault(),
        IntroducingQuantity = list.IntroducingQuantity.GetValueOrDefault(),
        OutboundDateEnd = list.OutboundDateEnd.GetValueOrDefault(),
        OutboundDateStart = list.OutboundDateStart.GetValueOrDefault(),
        OutboundQuantity = list.OutboundQuantity.GetValueOrDefault(),
        PreviousMonthDate = list.PreviousMonthDate.GetValueOrDefault(),
        PreviousMonthQuantity = list.PreviousMonthQuantity.GetValueOrDefault(),
        ReassumeQuantity = list.ReassumeQuantity.GetValueOrDefault(),
        SituationDate = list.SituationDate.GetValueOrDefault(),
        SituationQuantity = list.SituationQuantity.GetValueOrDefault()
      };
      return _ret;
    }
    private static ArrayOfDisposalList GetDisposalsList( EntitySet<JSOXCustomsSummary> entitySet )
    {
      decimal _total = 0;
      ArrayOfDisposalRows[] _arrayOfDisposalRows = GetArrayOfDisposalRows( entitySet, out _total );
      ArrayOfDisposalList _ret = new ArrayOfDisposalList()
      {
        DisposalRows = _arrayOfDisposalRows,
        TotalAmount = Convert.ToDouble( _total )
      };
      return _ret;
    }
    private static ArrayOfDisposalRows[] GetArrayOfDisposalRows( EntitySet<JSOXCustomsSummary> collection, out decimal total )
    {
      IQueryable<IGrouping<string, JSOXCustomsSummary>> _customsGroup = from _grpx in collection group _grpx by _grpx.ExportOrFreeCirculationSAD;
      List<ArrayOfDisposalRows> _ret = new List<ArrayOfDisposalRows>();
      decimal _total = 0;
      foreach ( IGrouping<string, JSOXCustomsSummary> _grpx in _customsGroup )
      {
        decimal _subTotal = 0;
        _ret.Add( GetDisposals( _grpx, out _subTotal ) );
        _total += _subTotal;
      }
      total = _total;
      return _ret.ToArray();
    }
    private static ArrayOfDisposalRows GetDisposals( IGrouping<string, JSOXCustomsSummary> collection, out decimal total )
    {
      if ( collection == null )
        throw new ArgumentNullException( "collection", "GetDisposals cannot have collection null" );
      total = 0;
      DisposalRow[] _rows = GetDisposalRowArray( collection, out total );
      ArrayOfDisposalRows _ret = new ArrayOfDisposalRows()
      {
        DisposalRow = _rows,
        SubtotalQuantity = Convert.ToDouble( total )
      };
      return _ret;
    }
    private static DisposalRow[] GetDisposalRowArray( IGrouping<string, JSOXCustomsSummary> collection, out decimal _total )
    {
      _total = 0;
      List<DisposalRow> _ret = new List<DisposalRow>();
      foreach ( JSOXCustomsSummary _jx in collection )
      {
        DisposalRow _new = new DisposalRow()
        {
          CompensationGood = _jx.CompensationGood,
          EntryDocumentNo = _jx.IntroducingSADNo,
          ExportOrFreeCirculationSAD = _jx.ExportOrFreeCirculationSAD,
          InvoiceNo = _jx.InvoiceNo,
          SADDate = _jx.SADDate.GetValueOrDefault(),
          Quantity = _jx.TotalAmount.GetValueOrDefault(),
          Balance = _jx.RemainingQuantity.GetValueOrDefault(),
          Procedure = _jx.CustomsProcedure
        };
        _ret.Add( _new );
        _total += Convert.ToDecimal( _new.Quantity );
      }
      return _ret.ToArray<DisposalRow>();
    }
    private static IPRStockContent[] GetIPRStock( IQueryable<BalanceBatch> collection )
    {
      List<IPRStockContent> _ret = new List<IPRStockContent>();
      if ( collection != null )
        foreach ( BalanceBatch _bsx in collection )
        {
          IPRStockContent _new = new IPRStockContent()
            {
              IPRList = GetIPRList( _bsx.BalanceIPR ),
              TotalBalance = _bsx.Balance.GetValueOrDefault(),
              TotalDustCSNotStarted = _bsx.DustCSNotStarted.GetValueOrDefault(),
              TotalIPRBook = _bsx.IPRBook.GetValueOrDefault(),
              TotalSHWasteOveruseCSNotStarted = _bsx.SHWasteOveruseCSNotStarted.GetValueOrDefault(),
              TotalTobaccoAvailable = _bsx.TobaccoAvailable.GetValueOrDefault(),
              TotalTobaccoInCigarettesProduction = _bsx.TobaccoInCigarettesProduction.GetValueOrDefault(),
              TotalTobaccoInCigarettesWarehouse = _bsx.TobaccoInCigarettesWarehouse.GetValueOrDefault(),
              TotalTobaccoInCutfillerWarehouse = _bsx.TobaccoInCutfillerWarehouse.GetValueOrDefault(),
              TotalTobaccoInWarehouse = _bsx.TobaccoInWarehouse.GetValueOrDefault()
            };
          _ret.Add( _new );
        }
      return _ret.ToArray<IPRStockContent>();
    }
    private static ArrayOfIPRRow GetIPRList( IQueryable<BalanceIPR> collection )
    {
      ArrayOfIPRRow _ArrayOfIPRRow = new ArrayOfIPRRow();
      List<IPRRow> _iprRows = new List<IPRRow>();
      foreach ( BalanceIPR _item in collection )
      {
        IPRRow _new = new IPRRow()
        {
          Balance = _item.Balance.GetValueOrDefault(),
          Batch = _item.Batch,
          DustCSNotStarted = _item.DustCSNotStarted.GetValueOrDefault(),
          EntryDocumentNo = _item.DocumentNo,
          IPRBook = _item.IPRBook.GetValueOrDefault(),
          SHWasteOveruseCSNotStarted = _item.SHWasteOveruseCSNotStarted.GetValueOrDefault(),
          SKU = _item.SKU,
          TobaccoAvailable = _item.TobaccoAvailable.GetValueOrDefault(),
          TobaccoInCigarettesProduction = _item.TobaccoUsedInTheProduction.GetValueOrDefault(),
          TobaccoInCigarettesWarehouse = _item.TobaccoInFGCSNotStarted.GetValueOrDefault(),
          TobaccoInCutfillerWarehouse = -1,
          TobaccoInWarehouse = _item.TobaccoAvailable.GetValueOrDefault()
        };
        _iprRows.Add( _new );
        _ArrayOfIPRRow.SubtotalBalance += _new.Balance;
        _ArrayOfIPRRow.SubtotalDustCSNotStarted += _new.DustCSNotStarted;
        _ArrayOfIPRRow.SubtotalIPRBook += _new.IPRBook;
        _ArrayOfIPRRow.SubtotalSHWasteOveruseCSNotStarted += _new.SHWasteOveruseCSNotStarted;
        _ArrayOfIPRRow.SubtotalTobaccoAvailable += _new.TobaccoAvailable;
        _ArrayOfIPRRow.SubtotalTobaccoInCigarettesProduction += _new.TobaccoInCigarettesProduction;
        _ArrayOfIPRRow.SubtotalTobaccoInCigarettesWarehouse += _new.TobaccoInCutfillerWarehouse;
        _ArrayOfIPRRow.SubtotalTobaccoInCutfillerWarehouse += _new.TobaccoInCutfillerWarehouse;
        _ArrayOfIPRRow.SubtotalTobaccoInWarehouse += _new.TobaccoInWarehouse;
      }
      _ArrayOfIPRRow.IPRRow = _iprRows.ToArray<IPRRow>();
      return _ArrayOfIPRRow;
    }
    #endregion

  }
}
