using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockLib
  {

    #region internal
    internal static StockLib Find( Entities edc )
    {
      return ( from _stcx in edc.StockLibrary
               where _stcx.Stock2JSOXLibraryIndex == null
               orderby _stcx.Identyfikator.Value descending
               select _stcx ).FirstOrDefault<StockLib>();
    }
    internal void GetInventory( Entities edc, Balance.StockDictionary balanceStock )
    {
      if ( !Validate( edc ) )
      {
        string _mtmplt = "Data validation failed. There are problems reported that must be resolved to start calculation procedure. Details you can find on the application log.";
        ActivityLogCT.WriteEntry( "GetInventory Validatoion", _mtmplt );
      }
      foreach ( StockEntry _sex in StockEntry )
        _sex.GetInventory( balanceStock );
    }
    #endregion

    #region private
    private bool Validate( Entities edc )
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
    #endregion

  }
}
