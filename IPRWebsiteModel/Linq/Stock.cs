using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockLib
  {
    public bool Validate( Entities edc )
    {
      int _problems = 0;
      foreach ( StockEntry _sex in this.FinishedGoodsNotProcessed( edc ) )
      {
        Batch _batchLookup = Batch.FindStockToBatchLookup( edc, _sex.Batch );
        if ( _batchLookup != null )
        {
          _sex.BatchIndex = _batchLookup;
          continue;
        }
        ActivityLogCT.WriteEntry( edc, "Validate", _sex.NoMachingBatcgWarningMessage );
        _problems++;
      }
      foreach ( StockEntry _senbx in this.FinishedGoodsNotBalanced( edc ) )
      {
        ActivityLogCT.WriteEntry( edc, "Validate", _senbx.NoMachingQuantityWarningMessage );
        _problems++;
      }
      foreach ( Batch _btx in DanglingBatches( edc ) )
      {
        ActivityLogCT.WriteEntry( edc, "Validate", _btx.DanglingBatchWarningMessage );
        _problems++;
      }
      return _problems > 0;
    }
    /// <summary>
    /// List of all IPR finished goods.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    public IQueryable<StockEntry> FinishedGoods( Entities edc )
    {
      return from _sex in this.StockEntry
             where ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) && _sex.IPRType.Value
             select _sex;
    }
    internal void GetInventory( Balance.StockDictionary balanceStock )
    {
      foreach ( StockEntry _sex in StockEntry )
        _sex.GetInventory( balanceStock );
    }
    /// <summary>
    /// List of all IPR finished goods that have not batch associated.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    private IQueryable<StockEntry> FinishedGoodsNotProcessed( Entities edc )
    {
      return from _sex in this.StockEntry
             where ( ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) )
             && _sex.IPRType.Value
             && _sex.BatchIndex == null
             select _sex;
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
    private IQueryable<Batch> DanglingBatches( Entities edc )
    {
      return from _btx in edc.Batch where _btx.FGQuantityAvailable.Value > 0 && _btx.StockEntry.Count<StockEntry>() == 0 select _btx;
    }
  }
}
