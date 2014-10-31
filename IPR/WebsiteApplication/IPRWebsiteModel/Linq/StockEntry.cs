//<summary>
//  Title   : StockEntry
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.SmartFactory.Customs;
using System;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockEntry
  {
    #region internal
    /// <summary>
    /// Processes the entry.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> item.</param>
    /// <param name="warnings">The warnings.</param>
    public void ProcessEntry(Entities edc, ErrorsList warnings)
    {
      GetProductType(edc);
      GetBatchLookup(edc, warnings);
    }
    /// <summary>
    /// Gets the no matching batch warning message.
    /// </summary>
    /// <value>
    /// The no matching batch warning message.
    /// </value>
    public string NoMachingBatchWarningMessage
    {
      get
      {
        return String.Format("Cannot find batch:{0}/sku: {1} for stock record {2} on the stock location:{3}.", this.Batch, this.SKU, this.Title, this.StorLoc);
      }
    }
    /// <summary>
    /// Gets the no matching tobacco warning message.
    /// </summary>
    /// <value>
    /// The no matching tobacco warning message.
    /// </value>
    public string NoMachingTobaccoWarningMessage
    {
      get
      {
        return String.Format("Cannot find open IPR account for tobacco batch:{0}/sku: {1} for stock record {2} on the stock location: {3}.", this.Batch, this.SKU, this.Title, this.StorLoc);
      }
    }
    internal void GetInventory(Entities edc, Balance.StockDictionary balanceStock)
    {
      switch (ProductType.Value)
      {
        case Linq.ProductType.Cutfiller:
          if (IPRType.Value && BatchIndex != null)
            BatchIndex.GetInventory(edc, balanceStock, Balance.StockDictionary.StockValueKey.TobaccoInCutfillerWarehouse, this.Quantity.Value);
          break;
        case Linq.ProductType.Cigarette:
          if (IPRType.Value && BatchIndex != null)
            BatchIndex.GetInventory(edc, balanceStock, Balance.StockDictionary.StockValueKey.TobaccoInCigarettesProduction, this.Quantity.Value);
          break;
        case Linq.ProductType.IPRTobacco:
          balanceStock.Sum(this.Quantity.Value, this.Batch, Balance.StockDictionary.StockValueKey.TobaccoInWarehouse);
          break;
        case Linq.ProductType.Tobacco:
        case Linq.ProductType.Other:
          break;
      }
    }
    #endregion

    #region private
    private void GetProductType(Entities edc)
    {
      Entities.ProductDescription product = edc.GetProductType(this.SKU, this.StorLoc);
      this.ProductType = product.productType;
      this.IPRType = product.IPRMaterial;
    }
    private void GetBatchLookup(Entities edc, ErrorsList warnings)
    {
      if (ProductType != Linq.ProductType.Cigarette && ProductType != Linq.ProductType.Cutfiller)
        return;
      if (!IPRType.GetValueOrDefault(false))
        return;
      this.BatchIndex = Linq.Batch.FindStockToBatchLookup(edc, this.Batch);
      if (this.BatchIndex != null)
        return;
      warnings.Add(new Warnning(NoMachingBatchWarningMessage, false));
    }
    #endregion

  }
}
