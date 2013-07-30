using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Warehouse
  {
    /// <summary>
    /// Finds the specified edc.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The index.</param>
    /// <returns>An object of <see cref="Warehouse"/></returns>
    public static Warehouse Find( Entities edc, string index )
    {
      if ( index == null )
        index = string.Empty;
      return ( from Warehouse item in edc.Warehouse where item.Title.Contains( index ) select item ).FirstOrDefault<Warehouse>();
    }
  }
}
