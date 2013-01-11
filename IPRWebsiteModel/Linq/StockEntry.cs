using System;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockEntry
  {
    #region internal
    /// <summary>
    /// Processes the entry.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="warnings">The warnings.</param>
    public void ProcessEntry( Entities edc, ErrorsList warnings )
    {
      GetProductType( edc );
      GetBatchLookup( edc, warnings );
    }
    /// <summary>
    /// Gets the no maching batcg warning message.
    /// </summary>
    /// <value>
    /// The no maching batcg warning message.
    /// </value>
    public string NoMachingBatcgWarningMessage
    {
      get
      {
        return String.Format( "Cannot find batch:{0}/sku: {1} for stock record {2} on the stock location:{3}.", this.Batch, this.SKU, this.Title, this.StorLoc );
      }
    }
    /// <summary>
    /// Gets the no maching quantity warning message.
    /// </summary>
    /// <value>
    /// The no maching quantity warning message.
    /// </value>
    /// <exception cref="System.ArgumentNullException">BatchIndex</exception>
    public string NoMachingQuantityWarningMessage
    {
      get
      {
        if ( this.BatchIndex == null )
          throw new ArgumentNullException( "BatchIndex" );
        string _mtmp = "Inconsistent quantity for the: batch {0}/sku: {1} for stock record {2}; quantity stock: {3}/batch: {4}.";
        return String.Format( _mtmp, this.Batch, this.SKU, this.Title, this.Quantity.Value, this.BatchIndex.FGQuantityAvailable.Value );
      }
    }
    internal void GetInventory( Balance.StockDictionary balanceStock )
    {
      switch ( ProductType.Value )
      {
        case Linq.ProductType.Cutfiller:
          if ( IPRType.Value && BatchIndex != null )
            BatchIndex.GetInventory( balanceStock, Balance.StockDictionary.StockValueKey.TobaccoInCutfillerWarehouse );
          break;
        case Linq.ProductType.Cigarette:
          if ( IPRType.Value && BatchIndex != null )
            BatchIndex.GetInventory( balanceStock, Balance.StockDictionary.StockValueKey.TobaccoInCigarettesProduction );
          break;
        case Linq.ProductType.IPRTobacco:
          balanceStock.Sum( this.Quantity.Value, this.Batch, Balance.StockDictionary.StockValueKey.TobaccoInWarehouse );
          break;
        case Linq.ProductType.Tobacco:
        case Linq.ProductType.Other:
          break;
      }
    }
    #endregion

    #region private
    private void GetProductType( Entities edc )
    {
      Entities.ProductDescription product = edc.GetProductType( this.SKU, this.StorLoc );
      this.ProductType = product.productType;
      this.IPRType = product.IPRMaterial;
    }
    private void GetBatchLookup( Entities edc, ErrorsList warnings )
    {
      if ( ProductType != Linq.ProductType.Cigarette && ProductType != Linq.ProductType.Cutfiller )
        return;
      if ( !IPRType.GetValueOrDefault( false ) )
        return;
      this.BatchIndex = Linq.Batch.FindStockToBatchLookup( edc, this.Batch );
      if ( this.BatchIndex != null )
        return;
      warnings.Add( NoMachingBatcgWarningMessage, false );
    }
    #endregion

  }
}
