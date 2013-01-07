using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal class BalanceSheetContentFactory
  {
    internal static BalanceSheetContent CreateRequestContent( JSOXLib list, string documentName )
    {
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.GetValueOrDefault(),
        IPRStock = GetIPRStock( null ),
        JSOX = GetJSOX( list ),
        SituationAtDate = list.SituationDate.GetValueOrDefault(),
        StartDate = list.PreviousMonthDate.GetValueOrDefault()
      };
      return _ret;
    }
    internal static BalanceSheetContent CreateEmptyRequestContent()
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
    private static JSOContent GetJSOX( JSOXLib list )
    {
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.GetValueOrDefault(),
        BalanceQuantity = list.BalanceQuantity.GetValueOrDefault(),
        Disposals = GetDisposals( list.JSOXCustomsSummary ),
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
    private static ArrayOfDisposalRow GetDisposals( IQueryable<JSOXCustomsSummary> collection )
    {
      if ( collection == null )
        throw new ArgumentNullException( "collection", "GetDisposals cannot have collection null" );
      decimal _total = 0;
      DisposalRow[] _rows = GetDisposalRowArray( collection, out _total );
      ArrayOfDisposalRow _ret = new ArrayOfDisposalRow()
      {
        DisposalRow = _rows,
        TotalAmount = Convert.ToDouble( _total )
      };
      return _ret;
    }
    private static DisposalRow[] GetDisposalRowArray( IQueryable<JSOXCustomsSummary> collection, out decimal _total )
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
          Balance = -1, //TODO remaining quantity
          Procedure = "TBD"
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
          Balance = -1, //TODO
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
  }
  //TODO to be removed 
  internal static class BalanceSheetContentFactoryExtensions
  {
    private static double GetValueOrDefault( this double? value )
    {
      return value.HasValue ? value.GetValueOrDefault() : -999;
    }
  }
}
