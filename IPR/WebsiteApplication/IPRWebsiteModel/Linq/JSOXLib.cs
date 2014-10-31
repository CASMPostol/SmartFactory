//<summary>
//  Title   : class JSOXLib
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
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// JSOXLib
  /// </summary>
  sealed public partial class JSOXLib
  {

    #region API
    internal delegate decimal GetOutboundQuantity(Entities entities, JSOXLib parent, out DateTime start, out DateTime end);
    /// <summary>
    /// Updates the balance report.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> representing data model.</param>
    /// <param name="getOutboundQuantity">Delegate to get outbound quantity.</param>
    internal bool UpdateBalanceReport(Entities edc, GetOutboundQuantity getOutboundQuantity)
    {
      bool _validated = false;
      StockDictionary _balanceStock = new StockDictionary();
      Dictionary<string, IGrouping<string, IPR>> _accountGroups = Linq.IPR.GetAllOpen4JSOXGroups(edc).ToDictionary(x => x.Key);
      Linq.StockLib _stock = Stock(edc);
      if (_stock != null)
      {
        _validated = _stock.Validate(edc, _accountGroups, _stock);
        _stock.GetInventory(edc, _balanceStock);
        _stock.Stock2JSOXLibraryIndex = this;
      }
      else
        ActivityLogCT.WriteEntry(edc, "Balance report", "Cannot find stock report - only preliminary report will be created");
      List<string> _processed = new List<string>();
      List<BalanceBatch> _BalanceBatch = edc.BalanceBatch.WhereItem<BalanceBatch>(x => x.Balance2JSOXLibraryIndex == this).ToList<BalanceBatch>();
      foreach (BalanceBatch _bbx in _BalanceBatch)
      {
        if (_accountGroups.ContainsKey(_bbx.Batch))
          _bbx.Update(edc, _accountGroups[_bbx.Batch], _balanceStock.GetOrDefault(_bbx.Batch));
        else
          edc.BalanceBatch.DeleteOnSubmit(_bbx);
        _processed.Add(_bbx.Batch);
      }
      foreach (string _btchx in _processed)
        _accountGroups.Remove(_btchx);
      foreach (var _grpx in _accountGroups)
        Linq.BalanceBatch.Create(edc, _grpx.Value, this, _balanceStock.GetOrDefault(_grpx.Key));

      //Introducing
      DateTime _thisIntroducingDateStart = LinqIPRExtensions.DateTimeMaxValue;
      DateTime _thisIntroducingDateEnd = LinqIPRExtensions.DateTimeMinValue;
      decimal _introducingQuantity = Linq.IPR.GetIntroducingData(edc, this, out _thisIntroducingDateStart, out _thisIntroducingDateEnd);
      this.IntroducingDateStart = _thisIntroducingDateStart;
      this.IntroducingDateEnd = _thisIntroducingDateEnd;
      this.IntroducingQuantity = Convert.ToDouble(_introducingQuantity);

      //Outbound
      DateTime _thisOutboundDateEnd = LinqIPRExtensions.DateTimeMinValue;
      DateTime _thisOutboundDateStart = LinqIPRExtensions.DateTimeMaxValue;
      decimal _outQuantity = getOutboundQuantity(edc, this, out _thisOutboundDateStart, out _thisOutboundDateEnd);
      this.OutboundQuantity = _outQuantity.Convert2Double2Decimals();
      this.OutboundDateEnd = _thisOutboundDateEnd;
      this.OutboundDateStart = _thisOutboundDateStart;

      //Balance
      decimal _thisBalanceQuantity = Convert.ToDecimal(this.PreviousMonthQuantity) + _introducingQuantity - _outQuantity;
      this.BalanceQuantity = _thisBalanceQuantity.Convert2Double2Decimals();

      //Situation at
      decimal _thisSituationQuantity = _BalanceBatch.Sum<BalanceBatch>(x => x.IPRBookDecimal);
      this.SituationQuantity = Convert.ToDouble(_thisSituationQuantity);

      //Reassign
      this.ReassumeQuantity = (_thisBalanceQuantity - _thisSituationQuantity).Convert2Double2Decimals();
      return _validated;
    }
    internal void CreateJSOXReport(JSOXLib previous)
    {
      PreviousMonthDate = previous.BalanceDate.GetValueOrDefault(DateTime.Today.Date - TimeSpan.FromDays(30));
      PreviousMonthQuantity = previous.SituationQuantity.GetValueOrDefault(-1);
      this.BalanceDate = DateTime.Today.Date;
      this.SituationDate = this.BalanceDate;
      this.JSOXLibraryReadOnly = false;
    }
    /// <summary>
    /// Stock for this <see cref="JSOXLib"/>.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    public Linq.StockLib Stock(Entities edc)
    {
      if (!m_NoStock && b_Stock == null)
      {
        b_Stock = edc.StockLibrary.WhereItem<StockLib>(x => x.Stock2JSOXLibraryIndex == this).FirstOrDefault<Linq.StockLib>();
        if (b_Stock == null)
          b_Stock = Linq.StockLib.Find(edc);
        m_NoStock = b_Stock == null;
      }
      return b_Stock;
    }
    #endregion

    #region private
    private Linq.StockLib b_Stock;
    private bool m_NoStock = false;
    #endregion

  }
}
