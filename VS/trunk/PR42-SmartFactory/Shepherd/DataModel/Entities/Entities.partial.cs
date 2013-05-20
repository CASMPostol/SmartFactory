
//<summary>
//  Title   : Entities helper classes.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using System.Globalization;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  /// Extends the EntitiesDataContext class
  /// </summary>
  public partial class EntitiesDataContext
  {
    public EntitiesDataContext()
      : this( SPContext.Current.Web.Url )
    { }
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
    /// <summary>
    /// Resolves the change conflicts.
    /// </summary>
    /// <param name="rsult">The rsult.</param>
    /// <exception cref="System.ApplicationException"></exception>
    public void ResolveChangeConflicts( ActionResult rsult )
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
          rsult.AddMessage( "ResolveChangeConflicts at: " + _cp, _tmp );
          _cp = "AddMessage";
          _itx.Resolve( RefreshMode.OverwriteCurrentValues );
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
    /// <summary>
    /// The ID colunm name
    /// </summary>
    public const string IDColunmName = "ID";
    /// <summary>
    /// The title colunm name
    /// </summary>
    public const string TitleColunmName = "Title";
    /// <summary>
    /// The ID property name
    /// </summary>
    public const string IDPropertyName = "Identyfikator";
    /// <summary>
    /// The title property name
    /// </summary>
    internal const string TitlePropertyName = "Tytuł";
    /// <summary>
    /// Try to get at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The _list.</param>
    /// <param name="id">The id.</param>
    /// <returns>
    /// An instance of the <typeparamref name="t"/> for the selected index or null if <paramref name="id" /> is null or empty.
    /// </returns>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    public static t TryGetAtIndex<t>( EntityList<t> list, string id )
      where t: Element
    {
      if ( id.IsNullOrEmpty() )
        return null;
      return GetAtIndex<t>( list, id );
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="id">The id.</param>
    /// <returns>
    /// An instance of the <typeparamref name="t"/> for the selected index.
    /// </returns>
    /// <exception cref="System.ApplicationException">
    /// </exception>
    /// <exception cref="ApplicationException">id is nuul or element cannot be found.</exception>
    public static t GetAtIndex<t>( EntityList<t> list, string id )
      where t: Element
    {
      int? _index = id.String2Int();
      if ( !_index.HasValue )
        throw new ApplicationException( typeof( t ).Name + " index is null" );
      ;
      try
      {
        return (
              from idx in list
              where idx.Identyfikator == _index.Value
              select idx ).First();
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} cannot be found at specified index{1}", typeof( t ).Name, _index.Value ) );
      }
    }
    /// <summary>
    /// Finds at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public static t FindAtIndex<t>( EntityList<t> list, string id )
      where t: Element
    {
      int? _index = id.String2Int();
      if ( !_index.HasValue )
        return null;
      try
      {
        return (
              from idx in list
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
  /// Alarms And Events list entry
  /// </summary>
  public partial class AlarmsAndEvents
  {
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for,
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="title">The evrnt title.</param>
    /// <param name="partner">The partner associated with the event.</param>
    /// <param name="shippingIndex">Index of the shipping.</param>
    internal static void WriteEntry( EntitiesDataContext edc, string title, Partner partner, Shipping shippingIndex )
    {
      if ( edc == null )
      {
        EventLog.WriteEntry( "CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114 );
        return;
      }
      AlarmsAndEvents _log = new AlarmsAndEvents()
        {
          Tytuł = title,
          AlarmsAndEventsList2PartnerTitle = partner,
          AlarmsAndEventsList2Shipping = shippingIndex
        };
      edc.AlarmsAndEvents.InsertOnSubmit( _log );
      edc.SubmitChanges( Microsoft.SharePoint.Linq.ConflictMode.ContinueOnConflict );
    }
  } //AlarmsAndEvents
  /// <summary>
  /// Extend the DistributionList autogenerated class 
  /// </summary>
  public partial class DistributionList
  {
    /// <summary>
    /// Gets the email.
    /// </summary>
    /// <param name="ccRole">The cc role.</param>
    /// <param name="EDC">The EDC.</param>
    /// <returns></returns>
    public static string GetEmail( ShepherdRole ccRole, EntitiesDataContext EDC )
    {
      var _ccdl = ( from _ccx in EDC.DistributionList
                    where _ccx.ShepherdRole.GetValueOrDefault( Entities.ShepherdRole.Invalid ) == ccRole
                    select new { Email = _ccx.EmailAddress } ).FirstOrDefault();
      if ( _ccdl == null || String.IsNullOrEmpty( _ccdl.Email ) )
        _ccdl = ( from _ccx in EDC.DistributionList
                  where _ccx.ShepherdRole.GetValueOrDefault( Entities.ShepherdRole.Invalid ) == Entities.ShepherdRole.Administrator
                  select new { Email = _ccx.EmailAddress } ).FirstOrDefault();
      return ( _ccdl == null ? String.Empty : _ccdl.Email ).UnknownIfEmpty();
    }
  }
  /// <summary>
  /// Extend the CityType autogenerated class 
  /// </summary>
  public partial class CityType
  {
    /// <summary>
    /// Creates the cities.
    /// </summary>
    /// <param name="EDC">The EDC.</param>
    public static void CreateCities( EntitiesDataContext EDC )
    {
      for ( int i = 0; i < 10; i++ )
      {
        CityType _cmm = new CityType() { Tytuł = String.Format( "City {0}", i ) };
        EDC.City.InsertOnSubmit( _cmm );
        EDC.SubmitChanges();
      }
    }
  }
  /// <summary>
  /// Extend the Partner autogenerated class 
  /// </summary>
  public partial class Partner
  {
    /// <summary>
    /// Finds for user.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="user">The user.</param>
    /// <returns></returns>
    public static Partner FindForUser( EntitiesDataContext edc, SPUser user )
    {
      if ( edc.Partner == null )
        return null;
      else
        return edc.Partner.FirstOrDefault( idx => idx.ShepherdUserTitle.IsNullOrEmpty() ? false : idx.ShepherdUserTitle.Contains( user.Name ) );
    }
  }
  /// <summary>
  /// Extensions
  /// </summary>
  public static class Extensions
  {
    #region public
    /// <summary>
    /// Convert int to string.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string IntToString( this int? _val )
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents <see cref="DateTime"/>.
    /// </summary>
    /// <param name="_val">The value to convert</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString( this DateTime? _val )
    {
      return _val.HasValue ? _val.Value.ToString( CultureInfo.CurrentUICulture ) : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <param name="_format">The _format.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString( this DateTime? _val, string _format )
    {
      return _val.HasValue ? string.Format( _format, _val.Value.ToString( CultureInfo.CurrentUICulture ) ) : String.Empty;
    }
    internal const string UnknownEmail = "unknown@comapny.com";
    /// <summary>
    /// Unknowns if empty.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string UnknownIfEmpty( this String val )
    {
      return String.IsNullOrEmpty( val ) ? UnknownEmail : val;
    }
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static int? String2Int( this string _val )
    {
      int _ret;
      if ( int.TryParse( _val, out _ret ) )
      {
        return _ret;
      }
      return new Nullable<int>();
    }
    /// <summary>
    /// String2s the double.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static double? String2Double( this string _val )
    {
      double _res = 0;
      if ( double.TryParse( _val, out _res ) )
        return _res;
      else
        return null;
    }
    /// <summary>
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty( this string _val )
    {
      return String.IsNullOrEmpty( _val );
    }
    #endregion
  }
}
