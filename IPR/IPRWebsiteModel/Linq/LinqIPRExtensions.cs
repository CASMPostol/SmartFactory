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
    /// <summary>
    /// Creates the title.
    /// </summary>
    /// <param name="invoice">The invoice.</param>
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
      invoice.Title = String.Format( _tmplt, invoice.Id.Value, invoice.InvoiceIndex.BillDoc, _sku, _batch );
    }
    /// <summary>
    /// Check if <paramref name="quantity" /> is availables in the specified batch.
    /// </summary>
    /// <param name="batch">The batch.</param>
    /// <param name="quantity">The requested quantity.</param>
    /// <returns></returns>
    public static bool Available( this Batch batch, double quantity )
    {
      return batch.FGQuantityAvailable >= quantity;
    }
    /// <summary>
    /// Returns available quantity.
    /// </summary>
    /// <param name="batch">The batch.</param>
    /// <returns></returns>
    public static double AvailableQuantity( this Batch batch )
    {
      return batch.FGQuantityAvailable.Value;
    }
    /// <summary>
    /// Titles the specified _val.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string Title( this Element _val )
    {
      return _val == null ? "NotApplicable".GetLocalizedString() : _val.Title.NotAvailable();
    }
    /// <summary>
    /// Mins the specified date time1.
    /// </summary>
    /// <param name="dateTime1">The date time1.</param>
    /// <param name="dateTime2">The date time2.</param>
    /// <returns></returns>
    public static DateTime Min( DateTime dateTime1, DateTime dateTime2 )
    {
      return dateTime1 < dateTime2 ? dateTime1 : dateTime2;
    }
    /// <summary>
    /// Maxes the specified date time1.
    /// </summary>
    /// <param name="dateTime1">The date time 1.</param>
    /// <param name="dateTime2">The date time 2.</param>
    /// <returns></returns>
    public static DateTime Max( DateTime dateTime1, DateTime dateTime2 )
    {
      return dateTime1 > dateTime2 ? dateTime1 : dateTime2;
    }
    internal static DateTime DateTimeMaxValue = DateTime.Today + TimeSpan.FromDays( 3000 );
    internal static DateTime DateTimeMinValue = DateTime.Today - TimeSpan.FromDays( 3000 );

  }
}
