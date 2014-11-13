//<summary>
//  Title   : class BalanceBatch
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

using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// class BalanceBatch
  /// </summary>
  public partial class BalanceBatch
  {

    #region public
    internal static void Create(Entities edc, IGrouping<string, IPR> _grpx, JSOXLib parent, StockDictionary.BalanceStock balanceStock)
    {
      try
      {
        IPR _firsTIPR = _grpx.FirstOrDefault<IPR>();
        BalanceBatch _newBB = new BalanceBatch()
        {
          Balance2JSOXLibraryIndex = parent,
          Batch = _grpx.Key,
          Title = "creating",
          SKU = _firsTIPR == null ? "NA" : _firsTIPR.SKU,
        };
        edc.BalanceBatch.InsertOnSubmit(_newBB);
        _newBB.Update(edc, _grpx, balanceStock);
      }
      catch (Exception ex)
      {
        throw new SharePoint.ApplicationError("BalanceBatch.Create", "Body", ex.Message, ex);
      }
    }
    internal void Update(Entities edc, IGrouping<string, IPR> grouping, StockDictionary.BalanceStock balanceStock)
    {
      try
      {
        Dictionary<string, IPR> _iprDictionary = grouping.ToDictionary(x => x.DocumentNo);
        List<string> _processed = new List<string>();
        BalanceTotals _totals = new BalanceTotals();
        foreach (BalanceIPR _blncIPRx in this.BalanceIPR(edc))
        {
          if (_iprDictionary.ContainsKey(_blncIPRx.DocumentNo))
          {
            IPR.Balance _newBipr = _blncIPRx.Update(edc);
            _totals.Add(_newBipr);
          }
          else
            edc.BalanceIPR.DeleteOnSubmit(_blncIPRx);
          _processed.Add(_blncIPRx.DocumentNo);
        }
        foreach (string _dcn in _processed)
          _iprDictionary.Remove(_dcn);
        foreach (IPR _iprx in _iprDictionary.Values)
        {
          IPR.Balance _newBipr = Linq.BalanceIPR.Create(edc, _iprx, this, this.Balance2JSOXLibraryIndex);
          _totals.Add(_newBipr);
        }
        this.DustCSNotStarted = _totals[IPR.ValueKey.DustCSNotStarted];
        this.DustCSStarted = _totals[IPR.ValueKey.DustCSStarted];
        this.IPRBook = _totals[IPR.ValueKey.IPRBook];
        this.OveruseCSNotStarted = _totals[IPR.ValueKey.OveruseCSNotStarted];
        this.OveruseCSStarted = _totals[IPR.ValueKey.OveruseCSStarted];
        this.PureTobaccoCSNotStarted = _totals[IPR.ValueKey.PureTobaccoCSNotStarted];
        this.PureTobaccoCSStarted = _totals[IPR.ValueKey.PureTobaccoCSStarted];
        this.SHMentholCSNotStarted = _totals[IPR.ValueKey.SHMentholCSNotStarted];
        this.SHMentholCSStarted = _totals[IPR.ValueKey.SHMentholCSStarted];
        this.SHWasteOveruseCSNotStarted = _totals[IPR.ValueKey.SHWasteOveruseCSNotStarted];
        this.TobaccoAvailable = _totals[IPR.ValueKey.TobaccoAvailable];
        this.TobaccoCSFinished = _totals[IPR.ValueKey.TobaccoCSFinished];
        this.TobaccoEnteredIntoIPR = _totals[IPR.ValueKey.TobaccoEnteredIntoIPR];
        this.TobaccoInFGCSNotStarted = _totals[IPR.ValueKey.TobaccoInFGCSNotStarted];
        this.TobaccoInFGCSStarted = _totals[IPR.ValueKey.TobaccoInFGCSStarted];
        this.TobaccoToBeUsedInTheProduction = _totals[IPR.ValueKey.TobaccoToBeUsedInTheProduction];
        this.TobaccoUsedInTheProduction = _totals[IPR.ValueKey.TobaccoUsedInTheProduction];
        this.WasteCSNotStarted = _totals[IPR.ValueKey.WasteCSNotStarted];
        this.WasteCSStarted = _totals[IPR.ValueKey.WasteCSStarted];
        this.TobaccoStarted = _totals[IPR.ValueKey.TobaccoStarted];
        //
        balanceStock.CalculateBalance(_totals.Base[IPR.ValueKey.TobaccoInFGCSNotStarted], _totals.Base[IPR.ValueKey.TobaccoAvailable]);
        this.Balance = balanceStock[StockDictionary.StockValueKey.Balance];
        this.TobaccoInCigarettesProduction = balanceStock[StockDictionary.StockValueKey.TobaccoInCigarettesProduction];
        this.TobaccoInCigarettesWarehouse = balanceStock[StockDictionary.StockValueKey.TobaccoInCigarettesWarehouse];
        this.TobaccoInCutfillerWarehouse = balanceStock[StockDictionary.StockValueKey.TobaccoInCutfillerWarehouse];
        this.TobaccoInWarehouse = balanceStock[StockDictionary.StockValueKey.TobaccoInWarehouse];
      }
      catch (Exception ex)
      {
        throw new SharePoint.ApplicationError("BalanceBatch.Update", "Body", ex.Message, ex);
      }
    }
    internal decimal IPRBookDecimal { get { return this.IPRBook.Rount2DecimalOrDefault(); } }
    #endregion

    #region private
    private IEnumerable<BalanceIPR> BalanceIPR(Entities edc)
    {
      if (m_BalanceIPR == null)
        m_BalanceIPR = from _biprx in edc.BalanceIPR let _id = _biprx.BalanceBatchIndex.Id where _id == this.Id select _biprx;
      return m_BalanceIPR;
    }
    private IEnumerable<BalanceIPR> m_BalanceIPR = null;
    /// <summary>
    /// Balance Totals
    /// </summary>
    private class BalanceTotals : Dictionary<IPR.ValueKey, decimal>
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="BalanceTotals" /> class.
      /// </summary>
      internal BalanceTotals()
      {
        foreach (IPR.ValueKey _vkx in Enum.GetValues(typeof(IPR.ValueKey)))
          base[_vkx] = 0;
      }
      internal new double this[IPR.ValueKey index]
      {
        get { return Convert.ToDouble(base[index]); }
      }
      internal Dictionary<IPR.ValueKey, decimal> Base { get { return this; } }
      /// <summary>
      /// Adds the specified balance.
      /// </summary>
      /// <param name="balance">The balance.</param>
      internal void Add(IPR.Balance balance)
      {
        foreach (IPR.ValueKey _vkx in Enum.GetValues(typeof(IPR.ValueKey)))
          base[_vkx] += balance.Base[_vkx];
      }
    }
    /// <summary>
    /// Called when to notify clients that a property value has changed..
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected override void OnPropertyChanged(string propertyName)
    {
      string _template = "SKU: {0}/Batch: {1}.";
      Title = String.Format(_template, this.SKU, this.Batch);
      base.OnPropertyChanged(propertyName);
    }
    #endregion

  }
}
