using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class StockLib
  {
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
    /// <summary>
    /// List of all IPR finished goods that have not batch associated.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    public IQueryable<StockEntry> FinishedGoodsNotProcessed( Entities edc )
    {
      return from _sex in this.StockEntry
             where ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) && _sex.IPRType.Value && _sex.BatchIndex == null
             select _sex;
    }
    /// <summary>
    /// List of all IPR finished goods not balanced with the batch.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    public IQueryable<StockEntry> FinishedGoodsNotBalanced( Entities edc )
    {
      return from _sex in this.StockEntry
             where ( ( _sex.ProductType.Value == ProductType.Cigarette ) || ( _sex.ProductType.Value == ProductType.Cutfiller ) ) &&
                   _sex.IPRType.Value &&
                   _sex.BatchIndex != null &&
                   _sex.Quantity.Value != _sex.BatchIndex.FGQuantityAvailable.Value
             select _sex;
    }
  }
}
