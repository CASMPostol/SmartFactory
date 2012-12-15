using System;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockEntry
  {
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
    public string NoMachingBatcgWarningMessage
    {
      get
      {
        return String.Format( "Cannot find batch:{0}/sku: {1} for stock record {2} on the stock location:{3}.", this.Batch, this.SKU, this.Title, this.StorLoc );
      }
    }
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
      this.BatchIndex = Linq.Batch.FindLookup( edc, this.Batch );
      if ( this.BatchIndex != null )
        return;
      warnings.Add( NoMachingBatcgWarningMessage, false );
    }
  }
}
