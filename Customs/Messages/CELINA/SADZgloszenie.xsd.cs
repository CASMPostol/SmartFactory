//<summary>
//  Title   : public partial class SADZgloszenie
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  /// <summary>
  /// public partial class SADZgloszenie
  /// </summary>
  public partial class SADZgloszenie
  {

    /// <summary>
    /// Creates the instance of <see cref="SADZgloszenie"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="goods">The goods.</param>
    /// <param name="customsOffice">The .</param>
    /// <returns></returns>
    public static SADZgloszenie Create(  SADZgloszenieTowar[] goods, SADZgloszenieUC customsOffice )
    {
      decimal _grossMas = 0;
      decimal _pckgs = 0;
      decimal _value = 0;
      foreach ( SADZgloszenieTowar _gdsIx in goods )
      {
        _grossMas += _gdsIx.MasaBruttoSpecified ? _gdsIx.MasaBrutto : 0;
        _pckgs += SADZgloszenieTowarOpakowanie.Packages( _gdsIx.Opakowanie );
        _value += _gdsIx.WartoscTowaru.WartoscPozycji;
      }
      SADZgloszenieWartoscTowarow _valueTotal = SADZgloszenieWartoscTowarow.Create( _value );
      SADZgloszenie _new = new SADZgloszenie()
      {
        NrWlasny = "13SXX0000",
        P1a = "XX",
        P1b = "X",
        LiczbaPozycji = goods.Length,
        LiczbaOpakowan = _pckgs,
        KrajWysylki = "XX",
        KrajPrzeznaczenia = "XX",
        Kontenery = false,
        RodzajTransakcji = "11",
        MasaBrutto = _grossMas,
        Rodzaj = new SADZgloszenieRodzaj() { Typ = "H", Podtyp = "A", Powiadomienie = false },
        UC = customsOffice,
        //TODO using Vendor associated to CW
        Nadawca = new SADZgloszenieNadawca[] { new SADZgloszenieNadawca() { PozId = 1, Nazwa = "JT INTERNATIONAL SA A MEMBER OF THE", UlicaNumer = "1,RUE DE LA GABELLE", KodPocztowy = "1211", Miejscowosc = "GENEVA", Kraj = "CH" } },
        Odbiorca = new SADZgloszenieOdbiorca[] { new SADZgloszenieOdbiorca() { PozId = 1, Nazwa = "JTI  POLSKA  SP. Z O.O.", UlicaNumer = "GOSTKOW STARY 42", KodPocztowy = "99-220", Miejscowosc = "WARTKOWICE", Kraj = "PL", TIN = "PL8280001819", Regon = "00130199100000", EORI = "PL828000181900000" } },
        TransportNaGranicy = null,
        WarunkiDostawy = new SADZgloszenieWarunkiDostawy() { Kod = "XXX", Miejsce = "XXX", MiejsceKod = "X" },
        WartoscTowarow = _valueTotal,
        Towar = goods,
        MiejsceData = null
      };
      return _new;
    }

  }
}
