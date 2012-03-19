using System;
using Microsoft.SharePoint.Linq;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.SendNotification.Entities
{
  public partial class Element
  {
    internal const string IDColunmName = "ID";
    internal const string TitleColunmName = "Title";
    internal const string IDPropertyName = "Identyfikator";
    internal const string TitlePropertyName = "Tytuł";
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    internal static t GetAtIndex<t>(EntityList<t> _list, int _ID)
      where t: Element
    {
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _ID
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index{1}", typeof(t).Name, _ID));
      }
    }
  }
}
