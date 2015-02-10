//<summary>
//  Title   : class BalanceSheetContentFactory
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

using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal static class BalanceSheetContentFactory
  {
    #region public
    internal static void CreateReport(SPWeb web, string webUrl, int jsoxLibItemId)
    {
      SPFile _newFile = default(SPFile);
      BalanceSheetContent _content = default(BalanceSheetContent);
      using (Entities _edc = new Entities(webUrl))
      {
        JSOXLib _old = Element.GetAtIndex<JSOXLib>(_edc.JSOXLibrary, jsoxLibItemId);
        if (_old.JSOXLibraryReadOnly.Value)
          throw new ApplicationException("The record is read only and new report must not be created.");
        _old.JSOXLibraryReadOnly = true;
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateEmptyContent();
        string _documentName = Settings.RequestForBalanceSheetDocumentName(_edc, jsoxLibItemId + 1);
        _newFile = SPDocumentFactory.Prepare(web, _content, _documentName);
        _newFile.DocumentLibrary.Update();
        JSOXLibFactory _current = JSOXLibFactory.ConstructJSOXLibFActory(_edc, _newFile.Item.ID);
        bool _validated = _current.CreateJSOXReport(_edc, _old);
        _edc.SubmitChanges();
        _content = CreateContent(_edc, _current, _documentName, !_validated);
      }
      _content.UpdateDocument(_newFile);
      _newFile.DocumentLibrary.Update();
    }
    internal static void UpdateReport(SPListItem listItem, string WebUrl, int jsoxLibItemId)
    {
      BalanceSheetContent _content = null;
      using (Entities _edc = new Entities(WebUrl))
      {
        JSOXLibFactory _jsoxLibFactory = JSOXLibFactory.ConstructJSOXLibFActory(_edc, jsoxLibItemId);
        if (_jsoxLibFactory.JSOXLibraryReadOnly)
          throw new ApplicationException("The record is read only and the report must not be updated.");
        bool _validated = _jsoxLibFactory.UpdateBalanceReport(_edc);
        string _documentName = Settings.RequestForBalanceSheetDocumentName(_edc, _jsoxLibFactory.Id);
        _content = DocumentsFactory.BalanceSheetContentFactory.CreateContent(_edc, _jsoxLibFactory, _documentName, !_validated);
        _jsoxLibFactory.JSOXLibraryReadOnly = true;
        _edc.SubmitChanges();
      }
      _content.UpdateDocument(listItem.File);
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
    private static BalanceSheetContent CreateContent(Entities edc, JSOXLibFactory factory, string documentName, bool preliminary)
    {
      if (preliminary)
        documentName += " " + "PRELIMINARY".GetLocalizedString();
      JSOXLib list = factory.JSOXList;
      List<BalanceCutfillerContent> _cr = CreateBalanceCutfillerContent(edc, factory);
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.GetValueOrDefault(),
        BalanceBatch = GetBalanceBatchContent(edc, list.BalanceBatch(edc)),
        JSOX = GetJSOContent(factory),
        SituationAtDate = list.SituationDate.GetValueOrDefault(),
        StartDate = list.PreviousMonthDate.GetValueOrDefault(),
        BalanceCutfiller = _cr.ToArray<BalanceCutfillerContent>()
      };
      return _ret;
    }
    private static List<BalanceCutfillerContent> CreateBalanceCutfillerContent(Entities edc, JSOXLibFactory factory)
    {
      List<BalanceCutfillerContent> _ret = new List<BalanceCutfillerContent>();
      foreach (StockEntry _StockEntryX in from _sex in factory.JSOXList.Stock(edc).StockEntry(edc) where _sex.IPRType.Value && _sex.ProductType == ProductType.Cutfiller orderby _sex.Batch select _sex)
      {
        double _cfc = _StockEntryX.Quantity.Value / _StockEntryX.BatchIndex.FGQuantity.Value;
        foreach (Material _MaterialX in from _mtrx in edc.Material
                                        let _mtrlId = _mtrx.Material2BatchIndex.Id.Value
                                        let _stoctId = _StockEntryX.BatchIndex.Id.Value
                                        where (_mtrlId == _stoctId && _mtrx.ProductType.Value == ProductType.IPRTobacco)
                                        select _mtrx)
        {
          BalanceCutfillerContent _bcc = new BalanceCutfillerContent()
          {
            CtfBatch = _StockEntryX.Batch,
            CtfBatchQuantity = _StockEntryX.BatchIndex.FGQuantity.Value.Round2Double(),
            CtfOnStock = _StockEntryX.Quantity.Value.Round2Double(),
            CtfSKU = _StockEntryX.SKU,
            TobaccoBatch = _MaterialX.Batch,
            TobaccoBatchQuantity = _MaterialX.TobaccoQuantity.Value.Round2Double(),
            TobaccoKg = (_MaterialX.TobaccoQuantity.Value * _cfc).Round2Double(),
            TobaccoSKU = _MaterialX.SKU
          };
          _ret.Add(_bcc);
        }
      }
      return _ret;
    }
    private static JSOContent GetJSOContent(JSOXLibFactory factory)
    {
      JSOXLib list = factory.JSOXList;
      JSOContent _ret = new JSOContent()
      {
        BalanceDate = list.BalanceDate.GetValueOrDefault(),
        BalanceQuantity = list.BalanceQuantity.Round2DecimalOrDefault(),
        JSOXCustomsSummaryList = GetDisposalsList(factory.SummaryContentList),
        IntroducingDateEnd = list.IntroducingDateEnd.GetValueOrDefault(),
        IntroducingDateStart = list.IntroducingDateStart.GetValueOrDefault(),
        IntroducingQuantity = list.IntroducingQuantity.Round2DecimalOrDefault(),
        OutboundDateEnd = list.OutboundDateEnd.GetValueOrDefault(),
        OutboundDateStart = list.OutboundDateStart.GetValueOrDefault(),
        OutboundQuantity = list.OutboundQuantity.Round2DecimalOrDefault(),
        PreviousMonthDate = list.PreviousMonthDate.GetValueOrDefault(),
        PreviousMonthQuantity = list.PreviousMonthQuantity.Round2DecimalOrDefault(),
        ReassumeQuantity = list.ReassumeQuantity.Round2DecimalOrDefault(),
        SituationDate = list.SituationDate.GetValueOrDefault(),
        SituationQuantity = list.SituationQuantity.Round2DecimalOrDefault()
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContentList GetDisposalsList(List<JSOXCustomsSummaryContent> collection)
    {
      decimal _total = 0;
      JSOXCustomsSummaryContentOGLGroup[] _arrayOfDisposalRows = GetJSOXCustomsSummaryContentOGLGroupArray(collection, out _total);
      JSOXCustomsSummaryContentList _ret = new JSOXCustomsSummaryContentList()
      {
        JSOXCustomsSummaryOGLGroupArray = _arrayOfDisposalRows,
        SubtotalQuantity = _total
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContentOGLGroup[] GetJSOXCustomsSummaryContentOGLGroupArray(List<JSOXCustomsSummaryContent> collection, out decimal total)
    {
      List<JSOXCustomsSummaryContentOGLGroup> _ret = new List<JSOXCustomsSummaryContentOGLGroup>();
      IEnumerable<IGrouping<string, JSOXCustomsSummaryContent>> _customsGroup = from _grpx in collection group _grpx by _grpx.ExportOrFreeCirculationSAD;
      decimal _total = 0;
      foreach (IGrouping<string, JSOXCustomsSummaryContent> _grpx in _customsGroup)
      {
        decimal _subTotal = 0;
        _ret.Add(GetJSOXCustomsSummaryContentOGLGroup(_grpx, out _subTotal));
        _total += _subTotal;
      }
      total = _total;
      return _ret.ToArray();
    }
    private static JSOXCustomsSummaryContentOGLGroup GetJSOXCustomsSummaryContentOGLGroup(IGrouping<string, JSOXCustomsSummaryContent> collection, out decimal total)
    {
      if (collection == null)
        throw new ArgumentNullException("collection", "GetDisposals cannot have collection null");
      total = 0;
      JSOXCustomsSummaryContent[] _rows = GetDisposalRowArray(collection, out total);
      JSOXCustomsSummaryContentOGLGroup _ret = new JSOXCustomsSummaryContentOGLGroup()
      {
        JSOXCustomsSummaryArray = _rows,
        SubtotalQuantity = total
      };
      return _ret;
    }
    private static JSOXCustomsSummaryContent[] GetDisposalRowArray(IGrouping<string, JSOXCustomsSummaryContent> collection, out decimal total)
    {
      total = 0;
      List<JSOXCustomsSummaryContent> _ret = new List<JSOXCustomsSummaryContent>();
      foreach (JSOXCustomsSummaryContent _jx in collection)
      {
        _ret.Add(_jx);
        total += Convert.ToDecimal(_jx.Quantity);
      }
      return _ret.ToArray<JSOXCustomsSummaryContent>();
    }
    private static BalanceBatchContent[] GetBalanceBatchContent(Entities edc, IEnumerable<BalanceBatch> collection)
    {
      List<BalanceBatchContent> _ret = new List<BalanceBatchContent>();
      if (collection != null)
        foreach (BalanceBatch _bsx in collection)
        {
          BalanceBatchContent _new = new BalanceBatchContent()
            {
              BalanceIPR = GetBalanceIPRContent(_bsx.BalanceIPR(edc)),
              TotalBalance = _bsx.Balance.Round2DecimalOrDefault(),
              TotalDustCSNotStarted = _bsx.DustCSNotStarted.Round2DecimalOrDefault(),
              TotalIPRBook = _bsx.IPRBook.Round2DecimalOrDefault(),
              TotalSHWasteOveruseCSNotStarted = _bsx.SHWasteOveruseCSNotStarted.Round2DecimalOrDefault(),
              TotalTobaccoAvailable = _bsx.TobaccoAvailable.Round2DecimalOrDefault(),
              TotalTobaccoInCigarettesProduction = _bsx.TobaccoInCigarettesProduction.Round2DecimalOrDefault(),
              TotalTobaccoInCigarettesWarehouse = _bsx.TobaccoInCigarettesWarehouse.Round2DecimalOrDefault(),
              TotalTobaccoInCutfillerWarehouse = _bsx.TobaccoInCutfillerWarehouse.Round2DecimalOrDefault(),
              TotalTobaccoInWarehouse = _bsx.TobaccoInWarehouse.Round2DecimalOrDefault(),
              TotalTobaccoStarted = _bsx.TobaccoStarted.Round2DecimalOrDefault()
            };
          _ret.Add(_new);
        }
      return _ret.ToArray<BalanceBatchContent>();
    }
    private static BalanceIPRContent[] GetBalanceIPRContent(IEnumerable<BalanceIPR> collection)
    {
      List<BalanceIPRContent> _iprRows = new List<BalanceIPRContent>();
      foreach (BalanceIPR _item in collection)
      {
        BalanceIPRContent _new = new BalanceIPRContent()
        {
          Batch = _item.Batch,
          DustCSNotStarted = _item.DustCSNotStarted.Round2DecimalOrDefault(),
          EntryDocumentNo = _item.DocumentNo,
          IPRBook = _item.IPRBook.Round2DecimalOrDefault(),
          SHWasteOveruseCSNotStarted = _item.SHWasteOveruseCSNotStarted.Round2DecimalOrDefault(),
          SKU = _item.SKU,
          TobaccoAvailable = _item.TobaccoAvailable.Round2DecimalOrDefault(),
          TobaccoStarted = _item.TobaccoStarted.Round2DecimalOrDefault()
        };
        _iprRows.Add(_new);
      }
      return _iprRows.ToArray<BalanceIPRContent>();
    }
    #endregion

  }
}
