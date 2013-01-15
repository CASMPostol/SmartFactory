using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using CAS.SharePoint;

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
        bool _validated = _current.CreateJSOXReport( _edc, _old );
        _edc.SubmitChanges();
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent( _current, _documentName, !_validated );
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
        if ( _current.JSOXLibraryReadOnly.Value )
          throw new ApplicationException( "The record is read only and the report must not be updated." );
        bool _validated = _current.UpdateBalanceReport( edc );
        string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( _current.Identyfikator.Value );
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent( _current, _documentName, !_validated );
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
        BalanceBatch = null,
        JSOX = null,
        SituationAtDate = _default,
        StartDate = _default
      };
      return _ret;
    }
    private static BalanceSheetContent CreateContent( JSOXLib list, string documentName, bool preliminary )
    {
      if ( preliminary )
        documentName += " " + "PRELIMINARY".GetLocalizedString();
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.GetValueOrDefault(),
        BalanceBatch = GetBalanceBatchContent( list.BalanceBatch ),
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
        JSOXCustomsSummaryList = GetDisposalsList( list.JSOXCustomsSummary ),
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
    private static JSOXCustomsSummaryContentList GetDisposalsList( EntitySet<JSOXCustomsSummary> entitySet )
    {
      decimal _total = 0;
      JSOXCustomsSummaryContentOGLGroup[] _arrayOfDisposalRows = GetJSOXCustomsSummaryContentOGLGroupArray( entitySet, out _total );
      JSOXCustomsSummaryContentList _ret = new JSOXCustomsSummaryContentList()
      {
        JSOXCustomsSummaryOGLGroupArray = _arrayOfDisposalRows,
        SubtotalQuantity = Convert.ToDouble( _total )
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContentOGLGroup[] GetJSOXCustomsSummaryContentOGLGroupArray( EntitySet<JSOXCustomsSummary> collection, out decimal total )
    {
      IQueryable<IGrouping<string, JSOXCustomsSummary>> _customsGroup = from _grpx in collection group _grpx by _grpx.ExportOrFreeCirculationSAD;
      List<JSOXCustomsSummaryContentOGLGroup> _ret = new List<JSOXCustomsSummaryContentOGLGroup>();
      decimal _total = 0;
      foreach ( IGrouping<string, JSOXCustomsSummary> _grpx in _customsGroup )
      {
        decimal _subTotal = 0;
        _ret.Add( GetJSOXCustomsSummaryContentOGLGroup( _grpx, out _subTotal ) );
        _total += _subTotal;
      }
      total = _total;
      return _ret.ToArray();
    }
    private static JSOXCustomsSummaryContentOGLGroup GetJSOXCustomsSummaryContentOGLGroup( IGrouping<string, JSOXCustomsSummary> collection, out decimal total )
    {
      if ( collection == null )
        throw new ArgumentNullException( "collection", "GetDisposals cannot have collection null" );
      total = 0;
      JSOXCustomsSummaryContent[] _rows = GetDisposalRowArray( collection, out total );
      JSOXCustomsSummaryContentOGLGroup _ret = new JSOXCustomsSummaryContentOGLGroup()
      {
        JSOXCustomsSummaryArray = _rows,
        SubtotalQuantity = Convert.ToDouble( total )
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContent[] GetDisposalRowArray( IGrouping<string, JSOXCustomsSummary> collection, out decimal _total )
    {
      _total = 0;
      List<JSOXCustomsSummaryContent> _ret = new List<JSOXCustomsSummaryContent>();
      foreach ( JSOXCustomsSummary _jx in collection )
      {
        JSOXCustomsSummaryContent _new = new JSOXCustomsSummaryContent()
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
      return _ret.ToArray<JSOXCustomsSummaryContent>();
    }
    private static BalanceBatchContent[] GetBalanceBatchContent( IQueryable<BalanceBatch> collection )
    {
      List<BalanceBatchContent> _ret = new List<BalanceBatchContent>();
      if ( collection != null )
        foreach ( BalanceBatch _bsx in collection )
        {
          BalanceBatchContent _new = new BalanceBatchContent()
            {
              BalanceIPR = GetBalanceIPRContent( _bsx.BalanceIPR ),
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
      return _ret.ToArray<BalanceBatchContent>();
    }
    private static BalanceIPRContent[] GetBalanceIPRContent( IQueryable<BalanceIPR> collection )
    {
      List<BalanceIPRContent> _iprRows = new List<BalanceIPRContent>();
      foreach ( BalanceIPR _item in collection )
      {
        BalanceIPRContent _new = new BalanceIPRContent()
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
      }
      return _iprRows.ToArray<BalanceIPRContent>();
    }
    #endregion

  }
}
