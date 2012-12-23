using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal class BalanceSheetContentFactory
  {
    internal static BalanceSheetContent CreateRequestContent( JSOXLib list, int documentNo, string documentName )
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
    private static JSOContent GetJSOX( JSOXLib list )
    {
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.GetValueOrDefault(),
        BalanceQuantity = list.BalanceQuantity.GetValueOrDefault(),
        Disposals = GetDisposals( null ),
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
    private static DisposalRow[] GetDisposals( IQueryable<JSOXCustomsSummary> collection )
    {
      if ( collection == null )
        return new DisposalRow[] { };
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
          TotalAmount = _jx.TotalAmount.GetValueOrDefault()
        };
        _ret.Add( _new );
      }
      return _ret.ToArray<DisposalRow>();
    }
    private static IPRStockContent[] GetIPRStock( IQueryable<BalanceSummary> collection )
    {
      List<IPRStockContent> _ret = new List<IPRStockContent>();
      if ( collection != null )
        foreach ( BalanceSummary _bsx in collection )
        {
          IPRStockContent _new = new IPRStockContent()
            {
              IPRList = GetIPRList( null ),
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
    private static IPRRow[] GetIPRList( IQueryable<JSOXSummary> collection )
    {
      List<IPRRow> _ret = new List<IPRRow>();
      foreach ( JSOXSummary _item in collection )
      {
        IPRRow _new = new IPRRow()
        {
          Balance = -1,
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
        _ret.Add( _new );
      }
      return _ret.ToArray<IPRRow>();
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
