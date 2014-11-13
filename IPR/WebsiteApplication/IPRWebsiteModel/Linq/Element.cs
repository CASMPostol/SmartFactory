using System;
using System.Linq;
using CAS.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
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
    public const string IDPropertyName = "Id";
    /// <summary>
    /// The title property name
    /// </summary>
    public const string TitlePropertyName = "Title";
    /// <summary>
    /// Try to get at index. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index or null if <paramref name="_ID"/> is null or empty.</returns>
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
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index.</returns>
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
              where idx.Id == _index.Value
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
    /// An instance of the <typeparamref name="t"/> for the selected index.
    /// </returns>
    /// <exception cref="ApplicationException">element cannot be found at specified index.</exception>
    public static t GetAtIndex<t>( EntityList<t> list, int index )
      where t: Element
    {
      try
      {
        return ( from idx in list where idx.Id == index select idx ).First();
      }
      catch ( Exception )
      {
        throw new ApplicationException( String.Format( "{0} cannot be found at specified index {1}", typeof( t ).Name, index ) );
      }
    }
    /// <summary>
    /// Finds at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <returns></returns>
    public static t FindAtIndex<t>( EntityList<t> _list, string _ID )
      where t: Element
    {
      int? _index = _ID.String2Int();
      if ( !_index.HasValue )
        return null;
      return ( from idx in _list
               where idx.Id == _index.Value
               select idx ).FirstOrDefault();
    }
  } //Element
}
