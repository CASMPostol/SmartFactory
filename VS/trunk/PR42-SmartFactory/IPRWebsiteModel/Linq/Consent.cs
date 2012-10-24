using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Consent
  {
    public static Consent Lookup( Entities _edc, string _consentNo )
    {
      return ( from _cidx in _edc.Consent where _cidx.Title.Trim().Equals( _consentNo.Trim() ) orderby _cidx.Wersja descending select _cidx ).First();
    }
  }
}
