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
      string _mtmplt = "Stock inventory validation passed successfully.";
      if ( !Validate( edc ) )
        _mtmplt = "Stock inventory validation failed. There are problems reported that must be resolved to start calculation procedure. Details you can find on the application log.";
      ActivityLogCT.WriteEntry( m_ActivityLogEntryName, _mtmplt );
      foreach ( StockEntry _sex in StockEntry )
        _sex.GetInventory( balanceStock );
    }
    #endregion

    #region private
    private bool Validate( Entities edc )
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
        ActivityLogCT.WriteEntry( edc, m_ActivityLogEntryName, _sex.NoMachingBatcgWarningMessage );
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
      return _problems == 0;
    }
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
               where ( ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) )
               && _sex.IPRType.Value
               select _sex;
      }
    }
    /// <summary>
    /// List of all IPR finished goods not balanced with the batch.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    private IQueryable<StockEntry> FinishedGoodsNotBalanced( Entities edc )
    {
      return from _sex in this.StockEntry
             where ( ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) ) &&
                   _sex.IPRType.Value &&
                   _sex.BatchIndex != null &&
                   _sex.Quantity.Value != _sex.BatchIndex.FGQuantityAvailable.Value
             select _sex;
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
