using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      warnings.Add( String.Format( "Cannot find batch:{0}/sku: {1} for stock record {2} for the stock location:{3}.", this.Batch, this.SKU, this.Title, this.StorLoc ), false );
    }
  }
}
