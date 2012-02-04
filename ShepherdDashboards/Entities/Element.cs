
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
  }
}
