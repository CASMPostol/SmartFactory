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

using CAS.SharePoint.Logging;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using Microsoft.SharePoint.Administration;
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
    internal static BalanceBatchWrapper Create(Entities edc, IGrouping<string, IPR> iprGroups, JSOXLib parent, StockDictionary.BalanceStock balanceStock, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering BalanceBatch.Create", 34, TraceSeverity.Verbose);
      BalanceBatchWrapper _ret;
      try
      {
        IPR _firsTIPR = iprGroups.FirstOrDefault<IPR>();
        BalanceBatch _newBB = new BalanceBatch()
        {
          Archival = false,
          Balance2JSOXLibraryIndex = parent,
          Batch = iprGroups.Key,
          Title = "creating",
          SKU = _firsTIPR == null ? "NA" : _firsTIPR.SKU,
        };
        edc.BalanceBatch.InsertOnSubmit(_newBB);
        List<BalanceIPR> _is = new List<BalanceIPR>();
        _newBB.Update(edc, iprGroups, balanceStock, _is, trace);
        _ret = new BalanceBatchWrapper() { iprCollection = _is.ToArray<BalanceIPR>(), batch = _newBB };
      }
      catch (CAS.SharePoint.ApplicationError)
      {
        throw;
      }
      catch (Exception ex)
      {
        trace("ApplicationError at BalanceBatch.Create", 58, TraceSeverity.High);
        throw new SharePoint.ApplicationError("BalanceBatch.Create", "Body", ex.Message, ex);
      }
      trace("Finished BalanceBatch.Create", 61, TraceSeverity.Verbose);
      return _ret;
    }
    internal void Update(Entities edc, IGrouping<string, IPR> grouping, StockDictionary.BalanceStock balanceStock, List<BalanceIPR> iprCollection, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering BalanceBatch.Update", 66, TraceSeverity.Verbose);
      if (grouping == null)
        throw new ArgumentNullException("grouping", "grouping at BalanceBatch.Update is null.");
      if (balanceStock == null)
        throw new ArgumentNullException("balanceStock", "balanceStock at BalanceBatch.Update is null.");
      try
      {
        Dictionary<string, IPR> _iprDictionary = grouping.ToDictionary(x => x.DocumentNo);
        List<string> _processed = new List<string>();
        BalanceTotals _totals = new BalanceTotals();
        trace("BalanceBatch.Update at BalanceIPR", 70, TraceSeverity.Verbose);
        foreach (BalanceIPR _balanceIPRx in this.BalanceIPR(edc))
        {
          if (_iprDictionary.ContainsKey(_balanceIPRx.DocumentNo))
          {
            IPR.Balance _new = _balanceIPRx.Update(edc, trace);
            _totals.Add(_new);
            iprCollection.Add(_balanceIPRx);
          }
          else
            edc.BalanceIPR.DeleteOnSubmit(_balanceIPRx);
          _processed.Add(_balanceIPRx.DocumentNo);
        }
        foreach (string _dcn in _processed)
          _iprDictionary.Remove(_dcn);
        trace("BalanceBatch.Update at BalanceIPR.Create", 55, TraceSeverity.Verbose);
        foreach (IPR _iprx in _iprDictionary.Values)
        {
          IPR.Balance _newBipr = Linq.BalanceIPR.Create(edc, _iprx, this, this.Balance2JSOXLibraryIndex, iprCollection, trace);
          _totals.Add(_newBipr);
        }
        trace("BalanceBatch.Update at update this.", 90, TraceSeverity.Verbose);
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
        trace("Exception at BalanceBatch.Update: " + ex.Message, 128, TraceSeverity.High);
        throw new SharePoint.ApplicationError("BalanceBatch.Update", "Body", ex.Message, ex);
      }
      trace("Finished BalanceBatch.Update", 131, TraceSeverity.Verbose);
    }
    internal decimal IPRBookDecimal { get { return this.IPRBook.Round2DecimalOrDefault(); } }
    /// <summary>
    /// Reverse lookup for <see cref="BalanceIPR" />
    /// </summary>
    /// <param name="edc">The entities context.</param>
    /// <returns>Collection of <see cref="BalanceIPR" /></returns>
    public IEnumerable<BalanceIPR> BalanceIPR(Entities edc)
    {
      if (!this.Id.HasValue)
        return new BalanceIPR[] { };
      return from _biprx in edc.BalanceIPR let _id = _biprx.BalanceBatchIndex.Id where _id == this.Id select _biprx;
    }
    #endregion

    #region private
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
