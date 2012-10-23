using System;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Clearence
  {
    private static Clearence FimdClearence( Entities _edc, string _referenceNumber )
    {
      return ( from _cx in _edc.Clearence where _referenceNumber.Contains( _cx.ReferenceNumber ) select _cx ).First<Clearence>();
    }
  }
}
