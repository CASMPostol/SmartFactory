using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using CAS.SmartFactory.IPR.WebsiteModel;
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
        string _documentName = Settings.RequestForBalanceSheetDocumentName( _edc, jsoxLibItemId + 1 );
        _newFile = SPDocumentFactory.Prepare( web, _content, _documentName );
        _newFile.DocumentLibrary.Update();
        JSOXLibFactory _current = JSOXLibFactory.ConstructJSOXLibFActory( _edc, _newFile.Item.ID );
        bool _validated = _current.CreateJSOXReport( _edc, _old );
        _edc.SubmitChanges();
        _content = CreateContent( _current, _documentName, !_validated );
      }
      _content.UpdateDocument( _newFile );
      _newFile.DocumentLibrary.Update();
    }
    internal static void UpdateReport( SPListItem listItem, string WebUrl, int jsoxLibItemId )
    {
      BalanceSheetContent _content = null;
      using ( Entities _edc = new Entities( WebUrl ) )
      {
        JSOXLibFactory _jsoxLibFactory = JSOXLibFactory.ConstructJSOXLibFActory( _edc, jsoxLibItemId );
        if ( _jsoxLibFactory.JSOXLibraryReadOnly )
          throw new ApplicationException( "The record is read only and the report must not be updated." );
        bool _validated = _jsoxLibFactory.UpdateBalanceReport( _edc );
        string _documentName = Settings.RequestForBalanceSheetDocumentName( _edc, _jsoxLibFactory.Id );
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent( _jsoxLibFactory, _documentName, !_validated );
        _jsoxLibFactory.JSOXLibraryReadOnly = true;
        _edc.SubmitChanges();
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
    private static BalanceSheetContent CreateContent( JSOXLibFactory factory, string documentName, bool preliminary )
    {
      if ( preliminary )
        documentName += " " + "PRELIMINARY".GetLocalizedString();
      JSOXLib list = factory.JSOXList;
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.GetValueOrDefault(),
        BalanceBatch = GetBalanceBatchContent( list.BalanceBatch ),
        JSOX = GetJSOContent( factory ),
        SituationAtDate = list.SituationDate.GetValueOrDefault(),
        StartDate = list.PreviousMonthDate.GetValueOrDefault()
      };
      return _ret;
    }
    private static JSOContent GetJSOContent( JSOXLibFactory factory )
    {
      JSOXLib list = factory.JSOXList;
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.GetValueOrDefault(),
        BalanceQuantity = list.BalanceQuantity.Rount2DecimalOrDefault(),
        JSOXCustomsSummaryList = GetDisposalsList( factory.SummaryContentList ),
        IntroducingDateEnd = list.IntroducingDateEnd.GetValueOrDefault(),
        IntroducingDateStart = list.IntroducingDateStart.GetValueOrDefault(),
        IntroducingQuantity = list.IntroducingQuantity.Rount2DecimalOrDefault(),
        OutboundDateEnd = list.OutboundDateEnd.GetValueOrDefault(),
        OutboundDateStart = list.OutboundDateStart.GetValueOrDefault(),
        OutboundQuantity = list.OutboundQuantity.Rount2DecimalOrDefault(),
        PreviousMonthDate = list.PreviousMonthDate.GetValueOrDefault(),
        PreviousMonthQuantity = list.PreviousMonthQuantity.Rount2DecimalOrDefault(),
        ReassumeQuantity = list.ReassumeQuantity.Rount2DecimalOrDefault(),
        SituationDate = list.SituationDate.GetValueOrDefault(),
        SituationQuantity = list.SituationQuantity.Rount2DecimalOrDefault()
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContentList GetDisposalsList( List<JSOXCustomsSummaryContent> collection )
    {
      decimal _total = 0;
      JSOXCustomsSummaryContentOGLGroup[] _arrayOfDisposalRows = GetJSOXCustomsSummaryContentOGLGroupArray( collection, out _total );
      JSOXCustomsSummaryContentList _ret = new JSOXCustomsSummaryContentList()
      {
        JSOXCustomsSummaryOGLGroupArray = _arrayOfDisposalRows,
        SubtotalQuantity = _total
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContentOGLGroup[] GetJSOXCustomsSummaryContentOGLGroupArray( List<JSOXCustomsSummaryContent> collection, out decimal total )
    {
      List<JSOXCustomsSummaryContentOGLGroup> _ret = new List<JSOXCustomsSummaryContentOGLGroup>();
      IEnumerable<IGrouping<string, JSOXCustomsSummaryContent>> _customsGroup = from _grpx in collection group _grpx by _grpx.ExportOrFreeCirculationSAD;
      decimal _total = 0;
      foreach ( IGrouping<string, JSOXCustomsSummaryContent> _grpx in _customsGroup )
      {
        decimal _subTotal = 0;
        _ret.Add( GetJSOXCustomsSummaryContentOGLGroup( _grpx, out _subTotal ) );
        _total += _subTotal;
      }
      total = _total;
      return _ret.ToArray();
    }
    private static JSOXCustomsSummaryContentOGLGroup GetJSOXCustomsSummaryContentOGLGroup( IGrouping<string, JSOXCustomsSummaryContent> collection, out decimal total )
    {
      if ( collection == null )
        throw new ArgumentNullException( "collection", "GetDisposals cannot have collection null" );
      total = 0;
      JSOXCustomsSummaryContent[] _rows = GetDisposalRowArray( collection, out total );
      JSOXCustomsSummaryContentOGLGroup _ret = new JSOXCustomsSummaryContentOGLGroup()
      {
        JSOXCustomsSummaryArray = _rows,
        SubtotalQuantity = total
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContent[] GetDisposalRowArray( IGrouping<string, JSOXCustomsSummaryContent> collection, out decimal total )
    {
      total = 0;
      List<JSOXCustomsSummaryContent> _ret = new List<JSOXCustomsSummaryContent>();
      foreach ( JSOXCustomsSummaryContent _jx in collection )
      {
        _ret.Add( _jx );
        total += Convert.ToDecimal( _jx.Quantity );
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
              TotalBalance = _bsx.Balance.Rount2DecimalOrDefault(),
              TotalDustCSNotStarted = _bsx.DustCSNotStarted.Rount2DecimalOrDefault(),
              TotalIPRBook = _bsx.IPRBook.Rount2DecimalOrDefault(),
              TotalSHWasteOveruseCSNotStarted = _bsx.SHWasteOveruseCSNotStarted.Rount2DecimalOrDefault(),
              TotalTobaccoAvailable = _bsx.TobaccoAvailable.Rount2DecimalOrDefault(),
              TotalTobaccoInCigarettesProduction = _bsx.TobaccoInCigarettesProduction.Rount2DecimalOrDefault(),
              TotalTobaccoInCigarettesWarehouse = _bsx.TobaccoInCigarettesWarehouse.Rount2DecimalOrDefault(),
              TotalTobaccoInCutfillerWarehouse = _bsx.TobaccoInCutfillerWarehouse.Rount2DecimalOrDefault(),
              TotalTobaccoInWarehouse = _bsx.TobaccoInWarehouse.Rount2DecimalOrDefault(),
              TotalTobaccoStarted = _bsx.TobaccoInFGCSStarted.Rount2DecimalOrDefault()
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
          Batch = _item.Batch,
          DustCSNotStarted = _item.DustCSNotStarted.Rount2DecimalOrDefault(),
          EntryDocumentNo = _item.DocumentNo,
          IPRBook = _item.IPRBook.Rount2DecimalOrDefault(),
          SHWasteOveruseCSNotStarted = _item.SHWasteOveruseCSNotStarted.Rount2DecimalOrDefault(),
          SKU = _item.SKU,
          TobaccoAvailable = _item.TobaccoAvailable.Rount2DecimalOrDefault(),
          TobaccoStarted = _item.TobaccoInFGCSStarted.Rount2DecimalOrDefault()
        };
        _iprRows.Add( _new );
      }
      return _iprRows.ToArray<BalanceIPRContent>();
    }
    #endregion

  }
}
