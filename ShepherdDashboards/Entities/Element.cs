
using System;
using Microsoft.SharePoint.Linq;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
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
    internal static t TryGetAtIndex<t>(EntityList<t> _list, string _ID)
      where t : Element
    {
      if (_ID.IsNullOrEmpty())
        return null;
      return GetAtIndex<t>(_list, _ID);
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    internal static t GetAtIndex<t>(EntityList<t> _list, string _ID)
      where t: Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        throw new ApplicationException(typeof(t).Name + " index is null"); ;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index{1}", typeof(t).Name, _index.Value));
      }
    }
    internal static t FindAtIndex<t>(EntityList<t> _list, string _ID)
      where t : Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx).FirstOrDefault();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
