using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        EndDate = list.SituationDate.Value,
        IPRStock = GetIPRStock(),
        JSOX = GetJSOX( list ),
        SituationAtDate = list.SituationDate.Value,
        StartDate = list.PreviousMonthDate.Value
      };
      return _ret;
    }
    private static JSOContent GetJSOX( JSOXLib list )
    {
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.Value,
        BalanceQuantity = list.BalanceQuantity.GetValueOrDefault( -1 ),
        Disposals = GetDisposals( null ),
        IntroducingDateEnd = list.IntroducingDateEnd.Value,
        IntroducingDateStart = list.IntroducingDateStart.Value,
        IntroducingQuantity = list.IntroducingQuantity.Value,
        OutboundDateEnd = list.OutboundDateEnd.Value,
        OutboundDateStart = list.OutboundDateStart.Value,
        OutboundQuantity = list.OutboundQuantity.Value,
        PreviousMonthDate = list.PreviousMonthDate.Value,
        PreviousMonthQuantity = list.PreviousMonthQuantity.Value,
        ReassumeQuantity = list.ReassumeQuantity.Value,
        SituationDate = list.SituationDate.Value,
        SituationQuantity = list.SituationQuantity.Value
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
          SADDate = _jx.SADDate.Value,
          TotalAmount = _jx.TotalAmount.Value
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
              TotalBalance = _bsx.Balance.GetValueOrDefault( -1 ),
              TotalDustCSNotStarted = _bsx.DustCSNotStarted.GetValueOrDefault( -1 ),
              TotalIPRBook = _bsx.IPRBook.GetValueOrDefault( -1 ),
              TotalSHWasteOveruseCSNotStarted = _bsx.SHWasteOveruseCSNotStarted.GetValueOrDefault( -1 ),
              TotalTobaccoAvailable = _bsx.TobaccoAvailable.GetValueOrDefault( -1 ),
              TotalTobaccoInCigarettesProduction = _bsx.TobaccoInCigarettesProduction.GetValueOrDefault( -1 ),
              TotalTobaccoInCigarettesWarehouse = _bsx.TobaccoInCigarettesWarehouse.GetValueOrDefault( -1 ),
              TotalTobaccoInCutfillerWarehouse = _bsx.TobaccoInCutfillerWarehouse.GetValueOrDefault( -1 ),
              TotalTobaccoInWarehouse = _bsx.TobaccoInWarehouse.GetValueOrDefault( -1 )
            };
          _ret.Add( _new );
        }
      return _ret.ToArray<IPRStockContent>();
    }

    private static IPRRow[] GetIPRList( IQueryable<JSOXSummary> collection )
    {
      throw new NotImplementedException();
    }

  }
}
