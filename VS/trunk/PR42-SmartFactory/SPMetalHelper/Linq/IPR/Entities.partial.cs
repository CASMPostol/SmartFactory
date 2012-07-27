using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.SharePoint.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  //TODO  [pr4-3510] Move ActionResult Twp the SharePoint project http://itrserver/Bugs/BugDetail.aspx?bid=3510
  internal class ActionResult: List<string>
  {
    #region public
    internal bool Valid { get { return this.Count == 0; } }
    internal void AddException( string _src, Exception _excptn )
    {
      string _msg = String.Format( "The operation interrupted at {0} by the error: {1}.", _src, _excptn.Message );
      base.Add( _msg );
    }
    internal void ReportActionResult( string _url )
    {
      if ( this.Count == 0 )
        return;
      try
      {
        using ( EntitiesDataContext EDC = new EntitiesDataContext( _url ) )
        {
          foreach ( string _msg in this )
          {
            Anons _entry = new Anons() { Tytuł = "ReportActionResult", Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
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
  /// Extends the EntitiesDataContext class
  /// </summary>
  public partial class EntitiesDataContext
  {
    public EntitiesDataContext() : base( SPContext.Current.Web.Url ) { }
    /// <summary>
    /// Persists to the content database changes made by the current user to one or more lists using the specified failure mode;
    /// or, if a concurrency conflict is found, populates the <see cref="P:Microsoft.SharePoint.Linq.DataContext.ChangeConflicts"/> property.
    /// </summary>
    /// <param name="mode">Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it 
    /// finds that a list item has been changed by another process since it was retrieved.
    /// </param>
    public void SubmitChangesSilently( RefreshMode mode )
    {
      try
      {
        SubmitChanges();
      }
      catch ( ChangeConflictException )
      {
        foreach ( ObjectChangeConflict changedListItem in this.ChangeConflicts )
        {
          changedListItem.Resolve( mode );
        }
        this.SubmitChanges();
      }
    }
    internal void ResolveChangeConflicts( ActionResult _rsult )
    {
      string _cp = "Starting";
      try
      {
        foreach ( ObjectChangeConflict _itx in this.ChangeConflicts )
        {
          _cp = "ObjectChangeConflict";
          string _tmp = String.Format( "Object: {0}", _itx.Object == null ? "null" : _itx.Object.ToString() );
          if ( _itx.MemberConflicts != null )
          {
            string _ft = ", Conflicts: Member.Name={0}; CurrentValue={1}; DatabaseValue={2}; OriginalValue={3}";
            String _chnges = String.Empty;
            foreach ( MemberChangeConflict _mid in _itx.MemberConflicts )
            {
              _chnges += String.Format( _ft,
                _mid.Member == null ? "null" : _mid.Member.Name,
                _mid.CurrentValue == null ? "null" : _mid.CurrentValue.ToString(),
                _mid.DatabaseValue == null ? "null" : _mid.DatabaseValue.ToString(),
                _mid.OriginalValue == null ? "null" : _mid.OriginalValue.ToString() );
            }
            _tmp += _chnges;
          }
          else
            _tmp += "; No member details";
          _rsult.AddMessage( "ResolveChangeConflicts at: " + _cp, _tmp );
          _cp = "AddMessage";
          _itx.Resolve( RefreshMode.KeepCurrentValues );
        } //foreach (ObjectChangeConflict
      }
      catch ( Exception ex )
      {
        string _frmt = "The current operation has been interrupted in ResolveChangeConflicts at {0} by error {1}.";
        throw new ApplicationException( String.Format( _frmt, _cp, ex.Message ) );
      }
    }
  } //EntitiesDataContext
  /// <summary>
  /// Extednds the Element class
  /// </summary>
  public partial class Element
  {
    internal const string IDColunmName = "ID";
    internal const string TitleColunmName = "Title";
    internal const string IDPropertyName = "Identyfikator";
    internal const string TitlePropertyName = "Tytuł";
    /// <summary>
    /// Try to get at index. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index or null if <paramref name="_ID"/> is null or empty.</returns>
    internal static t TryGetAtIndex<t>( EntityList<t> _list, string _ID )
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
    internal static t GetAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        throw new ApplicationException( typeof( t ).Name + " index is null" );
      ;
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
    internal static t FindAtIndex<t>( EntityList<t> _list, string _ID )
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
  /// Adds a message to the Event log list.
  /// </summary>
  public partial class Anons
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public Anons( string source, string message )
    {
      Tytuł = source;
      Treść = message;
      this.Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    internal static void WriteEntry( EntitiesDataContext edc, string source, string message )
    {
      if ( edc == null )
      {
        EventLog.WriteEntry( "CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114 );
        return;
      }
      Anons log = new Anons( source, message );
      edc.ActivityLog.InsertOnSubmit( log );
      edc.SubmitChangesSilently( Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues );
    }
  }
  public static class LinqIPRExtensions
  {
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
      if ( invoice.BatchID != null )
      {
        _batch = invoice.BatchID.Batch0;
        if ( invoice.BatchID.SKULookup != null )
          _sku = invoice.BatchID.SKULookup.SKU;
      }
      invoice.Tytuł = String.Format( _tmplt, invoice.Identyfikator.Value, invoice.InvoiceLookup.BillDoc, _sku, _batch );
    }
    internal static bool Available( this Batch batch, double _nq )
    {
      return batch.FGQuantityAvailable >= _nq;
    }
  }
}
