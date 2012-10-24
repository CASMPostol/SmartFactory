using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Clearence
  /// </summary>
  public partial class Clearence
  {
    /// <summary>
    /// Fimds the clearence.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <param name="referenceNumber">The _reference number.</param>
    /// <returns>An object od <see cref=""/> that has <paramref name="referenceNumber"/></returns>
    //TODO rename to get
    public static Clearence FimdClearence( Entities _edc, string referenceNumber )
    {
      return ( from _cx in _edc.Clearence where referenceNumber.Contains( _cx.ReferenceNumber ) select _cx ).First<Clearence>();
    }
  }
}
