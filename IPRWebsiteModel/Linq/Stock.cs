using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockLib
  {

    #region internal
    internal static StockLib Find( Entities edc )
    {
      return ( from _stcx in edc.StockLibrary
               let _notAssociated = _stcx.Stock2JSOXLibraryIndex == null
               where _notAssociated
               orderby _stcx.Identyfikator.Value descending
               select _stcx ).FirstOrDefault<StockLib>();
    }
    internal void GetInventory( Entities edc, Balance.StockDictionary balanceStock )
    {
      foreach ( StockEntry _sex in StockEntry )
        _sex.GetInventory( balanceStock );
    }
    internal bool Validate( Entities edc, Dictionary<string, IGrouping<string, IPR>> _accountGroups, StockLib library )
    {
      ActivityLogCT.WriteEntry( m_ActivityLogEntryName, "Starting stock inventory validation." );
      int _problems = 0;
      Dictionary<string, Batch> _batches = new Dictionary<string, Batch>();
      foreach ( StockEntry _sex in this.AllIPRFinishedGoods )
      {
        Batch _batchLookup = Batch.FindStockToBatchLookup( edc, _sex.Batch );
        if ( _batchLookup != null )
        {
          if ( !_batches.ContainsKey( _batchLookup.Batch0 ) )
            _batches.Add( _batchLookup.Batch0, _batchLookup );
          _sex.BatchIndex = _batchLookup;
          continue;
        }
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _sex.NoMachingBatchWarningMessage );
        _problems++;
      }
      foreach ( StockEntry _sex in this.AllIPRTobacco )
      {
        if ( _accountGroups.ContainsKey( _sex.Batch ) )
          continue;
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _sex.NoMachingTobaccoWarningMessage );
        _problems++;
      }
      List<string> _warnings = new List<string>();
      foreach ( Batch _senbx in _batches.Values )
        _senbx.CheckQuantity( _warnings, library );
      foreach ( string _msg in _warnings )
      {
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _msg );
        _problems++;
      }
      foreach ( Batch _btx in DanglingBatches( edc, _batches, library ) )
      {
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _btx.DanglingBatchWarningMessage );
        _problems++;
      }
      string _mtmplt = "Stock inventory validation passed successfully.";
      if ( _problems > 0 )
        _mtmplt = "Stock inventory validation failed. There are problems reported that must be resolved to start calculation procedure.";
      ActivityLogCT.WriteEntry( m_ActivityLogEntryName, _mtmplt );
      return _problems == 0;
    }
    #endregion

    #region private
    private string m_ActivityLogEntryName = "Stock Validation";
    /// <summary>
    /// List of all IPR finished goods that have not batch associated.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    private IQueryable<StockEntry> AllIPRFinishedGoods
    {
      get
      {
        return from _sex in this.StockEntry
               where ( ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) ) && _sex.IPRType.Value
               select _sex;
      }
    }
    private IEnumerable<StockEntry> AllIPRTobacco
    {
      get
      {
        return from _sex in this.StockEntry
               where _sex.ProductType.Value == ProductType.IPRTobacco
               select _sex;
      }
    }
    private IEnumerable<Batch> DanglingBatches( Entities edc, Dictionary<string, Batch> _batches, StockLib library )
    {
      Dictionary<string, Batch> _ret = new Dictionary<string, Batch>();
      List<Batch> _list = ( from _btx in edc.Batch
                            where _btx.FGQuantityAvailable.Value > 0
                            orderby _btx.Identyfikator.Value descending
                            select _btx ).ToList<Batch>();
      foreach ( Batch _bidx in from _btx in _list where !_btx.StockEntry.Any( x => x.StockLibraryIndex == library ) select _btx )
      {
        if ( _batches.ContainsKey( _bidx.Batch0 ) )
          continue;
        if ( !_ret.ContainsKey( _bidx.Batch0 ) )
          _ret.Add( _bidx.Batch0, _bidx );
      }
      return _ret.Values;
    }
    #endregion

  }
}
