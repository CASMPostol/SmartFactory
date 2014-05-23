//<summary>
//  Title   : Vendor custom partial entity class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Linq;
using CAS.SmartFactory.Customs.Messages.Serialization;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Vendor custom partial entity class
  /// </summary>
  public partial class Vendor
  {

    internal static Vendor FirstOrDefault( Entities entities )
    {
      return ( from _vx in entities.Vendor orderby _vx.Id ascending select _vx ).FirstOrDefault<Vendor>();
    }
    public static Organization SenderOrganization( Entities entities )
    {
      Vendor _Def = Element.GetAtIndex<Vendor>( entities.Vendor, 1 );
      Organization _ret = new Organization()
      {
        EORI = string.Empty,
        Id = 1,
        Kod = _Def.WorkZip,
        Kraj = _Def.WorkCountry,
        Miejscowosc = _Def.WorkCity,
        Nazwa = _Def.Title,
        Pesel = string.Empty,
        Regon = string.Empty,
        TIN = string.Empty,
        UlicaNr = _Def.WorkAddress
      };
      return _ret;
    }

  }
}
