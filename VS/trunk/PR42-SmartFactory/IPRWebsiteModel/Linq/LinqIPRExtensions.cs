using System;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// LinqIPRExtensions
  /// </summary>
  public static class LinqIPRExtensions
  {
    /// <summary>
    /// Unitses for the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns></returns>
    public static string Units( this ProductType product )
    {
      switch ( product )
      {
        case ProductType.IPRTobacco:
        case ProductType.Cutfiller:
        case ProductType.Tobacco:
          return "kg";
        case ProductType.Cigarette:
          return "kU";
        case ProductType.None:
        case ProductType.Invalid:
        case ProductType.Other:
        default:
          return "N/A";
      }
    }
    public static void CreateTitle( this InvoiceContent invoice )
    {
      string _tmplt = "{0}/{1} SKU:{2}/Batch:{3}";
      string _sku = "N/A";
      string _batch = "N/A";
      if ( invoice.InvoiceContent2BatchIndex != null )
      {
        _batch = invoice.InvoiceContent2BatchIndex.Batch0;
        if ( invoice.InvoiceContent2BatchIndex.SKUIndex != null )
          _sku = invoice.InvoiceContent2BatchIndex.SKUIndex.SKU;
      }
      invoice.Title = String.Format( _tmplt, invoice.Identyfikator.Value, invoice.InvoiceIndex.BillDoc, _sku, _batch );
    }
    public static bool Available( this Batch batch, double _nq )
    {
      return batch.FGQuantityAvailable >= _nq;
    }
    public static double AvailableQuantity( this Batch _batch )
    {
      return _batch.FGQuantityAvailable.Value;
    }
    public static string Title( this Element _val )
    {
      return _val == null ? "NotApplicable".GetLocalizedString() : _val.Title.NotAvailable();
    }
  }
}
