//<summary>
//  Title   : class StockLib
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
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// StockLib Entity
  /// </summary>
  public partial class StockLib
  {

    #region API
    internal static StockLib Find(Entities edc)
    {
      return (from _stcx in edc.StockLibrary
              let _notAssociated = _stcx.Stock2JSOXLibraryIndex == null
              where _notAssociated
              orderby _stcx.Id.Value descending
              select _stcx).FirstOrDefault<StockLib>();
    }
    internal void GetInventory(Entities edc, Balance.StockDictionary balanceStock)
    {
      foreach (StockEntry _sex in StockEntriesList(edc))
        _sex.GetInventory(edc, balanceStock);
    }
    internal bool Validate(Entities edc, Dictionary<string, IGrouping<string, IPR>> _accountGroups, StockLib library)
    {
      ActivityLogCT.WriteEntry(m_ActivityLogEntryName, "Starting stock inventory validation.");
      int _problems = 0;
      Dictionary<string, Batch> _batches = new Dictionary<string, Batch>();
      foreach (StockEntry _sex in this.AllIPRFinishedGoods(edc))
      {
        Batch _batchLookup = Batch.FindStockToBatchLookup(edc, _sex.Batch);
        if (_batchLookup != null)
        {
          if (!_batches.ContainsKey(_batchLookup.Batch0))
            _batches.Add(_batchLookup.Batch0, _batchLookup);
          _sex.BatchIndex = _batchLookup;
          continue;
        }
        ActivityLogCT.WriteEntry(edc, m_ActivityLogEntryName, _sex.NoMachingBatchWarningMessage);
        _problems++;
      }
      foreach (StockEntry _sex in this.AllIPRTobacco(edc))
      {
        if (_accountGroups.ContainsKey(_sex.Batch))
          continue;
        ActivityLogCT.WriteEntry(edc, m_ActivityLogEntryName, _sex.NoMachingTobaccoWarningMessage);
        _problems++;
      }
      List<string> _warnings = new List<string>();
      foreach (Batch _senbx in _batches.Values)
        _senbx.CheckQuantity(edc, _warnings, library);
      foreach (string _msg in _warnings)
      {
        ActivityLogCT.WriteEntry(edc, m_ActivityLogEntryName, _msg);
        _problems++;
      }
      foreach (Batch _btx in DanglingBatches(edc, _batches, library))
      {
        ActivityLogCT.WriteEntry(edc, m_ActivityLogEntryName, _btx.DanglingBatchWarningMessage);
        _problems++;
      }
      string _mtmplt = "Stock inventory validation passed successfully.";
      if (_problems > 0)
        _mtmplt = "Stock inventory validation failed. There are problems reported that must be resolved to start calculation procedure.";
      ActivityLogCT.WriteEntry(m_ActivityLogEntryName, _mtmplt);
      return _problems == 0;
    }
    /// <summary>
    /// Return of list of stock entries..
    /// </summary>
    /// <returns> list of stock entries associated with this <see cref="StockLib"/> entry.</returns>
    public List<StockEntry> StockEntriesList(Entities edc)
    {
      if (m_Entries == null)
        m_Entries = edc.StockEntry.WhereItem<StockEntry>(x => x.StockLibraryIndex == this).ToList<StockEntry>();
      return m_Entries;
    }
    #endregion

    #region private
    private string m_ActivityLogEntryName = "Stock Validation";
    /// <summary>
    /// List of all IPR finished goods that have not batch associated.
    /// </summary>
    /// <returns>The collection of <see cref="StockEntry" /></returns>
    private IEnumerable<StockEntry> AllIPRFinishedGoods(Entities edc)
    {
      return from _sex in this.StockEntriesList(edc)
             where ((_sex.ProductType.Value == ProductType.Cigarette) || (_sex.ProductType.Value == ProductType.Cutfiller)) && _sex.IPRType.Value
             select _sex;
    }
    private IEnumerable<StockEntry> AllIPRTobacco(Entities edc)
    {
      return from _sex in this.StockEntriesList(edc)
             where _sex.ProductType.Value == ProductType.IPRTobacco
             select _sex;
    }
    private IEnumerable<Batch> DanglingBatches(Entities edc, Dictionary<string, Batch> _batches, StockLib library)
    {
      Dictionary<string, Batch> _ret = new Dictionary<string, Batch>();
      Dictionary<int, Batch> _batchDictionary = (from _btx in edc.Batch
                                                 where _btx.FGQuantityAvailable.Value > 0
                                                 orderby _btx.Id.Value descending
                                                 select _btx).ToDictionary<Batch, int>(x => x.Id.Value);
      foreach (StockEntry _sex in edc.StockEntry.WhereItem<StockEntry>(x => x.StockLibraryIndex == library))
        if (_sex.BatchIndex != null && _batchDictionary.ContainsKey(_sex.BatchIndex.Id.Value))
          _batchDictionary.Remove(_sex.BatchIndex.Id.Value);
      foreach (Batch _bidx in _batchDictionary.Values)
      {
        if (_bidx.BatchStatus.Value == BatchStatus.Progress)
          continue;
        if (_batches.ContainsKey(_bidx.Batch0))
          continue;
        if (!_ret.ContainsKey(_bidx.Batch0))
          _ret.Add(_bidx.Batch0, _bidx);
      }
      return _ret.Values;
    }
    private List<StockEntry> m_Entries = null;
    #endregion

  }
}
