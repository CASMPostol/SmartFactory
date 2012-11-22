using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Consent
  /// </summary>
  public partial class Consent
  {
    /// <summary>
    /// Finds the specified edc.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="consentNo">The consent no.</param>
    /// <returns></returns>
    public static Consent Find( Entities edc, string consentNo )
    {
      return ( from _cidx in edc.Consent where _cidx.Title.Trim().Equals( consentNo.Trim() ) orderby _cidx.Wersja descending select _cidx ).FirstOrDefault();
    }
  }
}
