using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Action Result
  /// </summary>
  //TODOD  ActionResult must be moved to the SharePoint project. 
  public class ActionResult: List<string>
  {
    #region public
    public bool Valid { get { return this.Count == 0; } }
    public void AddException( string _src, Exception _excptn )
    {
      string _msg = String.Format( "The operation interrupted at {0} by the error: {1}.", _src, _excptn.Message );
      base.Add( _msg );
    }
    public void ReportActionResult( string _url )
    {
      if ( this.Count == 0 )
        return;
      try
      {
        using ( Entities EDC = new Entities( _url ) )
        {
          foreach ( string _msg in this )
          {
            ActivityLogCT _entry = new ActivityLogCT() { Title = "ReportActionResult", Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
            EDC.ActivityLog.InsertOnSubmit( _entry );
          }
          EDC.SubmitChanges();
        }
      }
      catch ( Exception ) { }
    }
    public void AddMessage( string _src, string _message )
    {
      string _msg = String.Format( "The operation reports at {0} the problem: {1}.", _src, _message );
      base.Add( _message );
    }
    #endregion
  }
  /// <summary>
  /// Extednds the Element class
  /// </summary>
  public partial class Element
  {
    public const string IDColunmName = "ID";
    public const string TitleColunmName = "Title";
    public const string IDPropertyName = "Identyfikator";
    public const string TitlePropertyName = "Title";
    /// <summary>
    /// Try to get at index. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index or null if <paramref name="_ID"/> is null or empty.</returns>
    public static t TryGetAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      if ( _ID.IsNullOrEmpty() )
        return null;
      return GetAtIndex<t>( _list, _ID );
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    public static t GetAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        throw new ApplicationException( typeof( t ).Name + " index is null" );
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx ).First();
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} cannot be found at specified index{1}", typeof( t ).Name, _index.Value ) );
      }
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The _list.</param>
    /// <param name="index">The _index.</param>
    /// <returns>
    /// An instance of the <see cref="t"/> for the selected index.
    /// </returns>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    public static t GetAtIndex<t>( EntityList<t> list, int index )
      where t: Element
    {
      try
      {
        return ( from idx in list where idx.Identyfikator == index select idx ).First();
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} cannot be found at specified index{1}", typeof( t ).Name, index ) );
      }
    }
    public static t FindAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        return null;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx ).FirstOrDefault();
      }
      catch ( Exception )
      {
        return null;
      }
    }
  } //Element
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
