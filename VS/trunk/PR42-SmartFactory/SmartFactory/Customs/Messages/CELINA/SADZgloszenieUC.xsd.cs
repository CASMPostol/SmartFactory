//<summary>
//  Title   : public partial class SADZgloszenieUC
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

using CAS.SmartFactory.Customs.Messages.Serialization;
using System.Collections.Generic;

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  public partial class SADZgloszenieUC
  {
    /// <summary>
    /// Creates the instance of <see cref="SADZgloszenieUC" />.
    /// </summary>
    /// <param name="customsOfficeJson">The customs office.</param>
    /// <returns>Returns new object of <see cref="SADZgloszenieUC"/>.</returns>
    public static SADZgloszenieUC Create( string customsOfficeJson )
    {
      CustomsOffice _co = CustomsOffice.Deserialize( customsOfficeJson );
      SADZgloszenieUCUCTranzytowy[] _uctArray = null;
      if ( _co.UCTranzytowy != null )
      {
        List<SADZgloszenieUCUCTranzytowy> _uct = new List<SADZgloszenieUCUCTranzytowy>();
        foreach ( var _couct in _co.UCTranzytowy )
          _uct.Add( new SADZgloszenieUCUCTranzytowy() { PozId = _couct.PozId, UCTranzytowy = _couct.UCTranzytowy } );
        _uctArray = _uct.ToArray();
      }
      return new SADZgloszenieUC()
      {
        UCZgloszenia = _co.UCZgloszenia,
        UCGraniczny = _co.UCGraniczny,
        Lokalizacja = _co.Lokalizacja == null ? null : new SADZgloszenieUCLokalizacja() { Miejsce = _co.Lokalizacja.Miejsce, Opis = _co.Lokalizacja.Opis, UC = _co.Lokalizacja.UC },
        SkladCelny = new SADZgloszenieUCSkladCelny() { Typ = _co.SkladCelny.Typ, Miejsce = _co.SkladCelny.Miejsce, Kraj = _co.SkladCelny.Kraj },
        UCKontrolny = _co.UCKontrolny == null ? null : new SADZgloszenieUCUCKontrolny()
                                                              {
                                                                KodPocztowy = _co.UCKontrolny.KodPocztowy,
                                                                Kraj = _co.UCKontrolny.Kraj,
                                                                Miejscowosc = _co.UCKontrolny.Miejscowosc,
                                                                Nazwa = _co.UCKontrolny.Nazwa,
                                                                UCKontrolny = _co.UCKontrolny.UCKontrolny,
                                                                UlicaNumer = _co.UCKontrolny.UlicaNumer
                                                              },
        UCPrzeznaczenia = _co.UCPrzeznaczenia,
        UCTranzytowy = _uctArray,
      };
    }
  }
}
