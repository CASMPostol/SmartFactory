using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class PCNCode
  {
    /// <summary>
    /// Adds the or get.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <param name="_code">The _code.</param>
    /// <returns></returns>
    public static PCNCode AddOrGet( Entities _edc, string _code )
    {
      PCNCode _pcncode = ( from _pcnx in _edc.PCNCode where _code.Trim().Contains( _pcnx.ProductCodeNumber ) select _pcnx ).FirstOrDefault();
      if ( _pcncode == null )
      {
        _pcncode = new PCNCode()
          {
            CompensationGood = Linq.CompensationGood.Tytoń,
            ProductCodeNumber = _code,
            Title = String.Format( "{0} {1}", Linq.CompensationGood.Tytoń, _code )
          };
        _edc.PCNCode.InsertOnSubmit( _pcncode );
      }
      return _pcncode;
    }
  }
}
