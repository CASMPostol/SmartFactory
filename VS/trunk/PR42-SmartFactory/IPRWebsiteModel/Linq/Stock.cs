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
    internal void Validate( Entities edc, Dictionary<string, System.Linq.IGrouping<string, IPR>> _accountGroups )
    {
      int _problems = 0;
      List<Batch> _batches = new List<Batch>();
      foreach ( StockEntry _sex in this.AllIPRFinishedGoods )
      {
        Batch _batchLookup = Batch.FindStockToBatchLookup( edc, _sex.Batch );
        if ( _batchLookup != null )
        {
          _batches.Add( _batchLookup );
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
      foreach ( Batch _senbx in _batches )
        _senbx.CheckQuantity( _warnings );
      foreach ( string _msg in _warnings )
      {
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _msg );
        _problems++;
      }
      foreach ( Batch _btx in DanglingBatches( edc ) )
      {
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _btx.DanglingBatchWarningMessage );
        _problems++;
      }
      string _mtmplt = "Stock inventory validation passed successfully.";
      if ( _problems > 0 )
        _mtmplt = "Stock inventory validation failed. There are problems reported that must be resolved to start calculation procedure. Details you can find on the application log.";
      ActivityLogCT.WriteEntry( m_ActivityLogEntryName, _mtmplt );
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
               where _sex.ProductType.Value == ProductType.Cigarette
               && _sex.IPRType.Value
               select _sex;
      }
    }
    public IEnumerable<StockEntry> AllIPRTobacco
    {
      get
      {
        return from _sex in this.StockEntry
               where ( _sex.ProductType.Value == ProductType.Tobacco )
               && _sex.IPRType.Value
               select _sex;
      }
    }
    private IEnumerable<Batch> DanglingBatches( Entities edc )
    {
      List<Batch> _list = ( from _btx in edc.Batch
                            where _btx.FGQuantityAvailable.Value > 0
                            select _btx ).ToList<Batch>();
      return from _btx in _list where _btx.StockEntry.Any() select _btx;
    }
    #endregion

  }
}
