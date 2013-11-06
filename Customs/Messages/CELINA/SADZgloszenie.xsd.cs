//<summary>
//  Title   : public partial class SADZgloszenie
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
    public static SADZgloszenie Create( SADZgloszenieTowar[] goods, SADZgloszenieUC customsOffice, string recipientOrganizationJson, string senderOrganizationJson )
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
        Nadawca = new SADZgloszenieNadawca[] { CreateSADZgloszenieNadawca( senderOrganizationJson ) },
        Odbiorca = new SADZgloszenieOdbiorca[] { CreateSADZgloszenieOdbiorca( recipientOrganizationJson ) },
        TransportNaGranicy = null,
        WarunkiDostawy = new SADZgloszenieWarunkiDostawy() { Kod = "XXX", Miejsce = "XXX", MiejsceKod = "X" },
        WartoscTowarow = _valueTotal,
        Towar = goods,
        MiejsceData = null
      };
      return _new;
    }
    public static SADZgloszenieNadawca CreateSADZgloszenieNadawca( string stringJson )
    {
      Organization _copy = Organization.Deserialize( stringJson );
      return new SADZgloszenieNadawca()
      {
        CRP = string.Empty,
        EORI = _copy.EORI,
        KodPocztowy = _copy.Kod,
        Kraj = _copy.Kraj,
        Miejscowosc = _copy.Miejscowosc,
        Nazwa = _copy.Nazwa,
        Pesel = _copy.Nazwa,
        PozId = _copy.Id,
        Regon = _copy.Regon,
        TIN = _copy.TIN,
        UlicaNumer = _copy.UlicaNr
      };
    }
    public static SADZgloszenieOdbiorca CreateSADZgloszenieOdbiorca( string stringJson )
    {
      Organization _copy = Organization.Deserialize( stringJson );
      return new SADZgloszenieOdbiorca()
      {
        EORI = _copy.EORI,
        KodPocztowy = _copy.Kod,
        Kraj = _copy.Kraj,
        Miejscowosc = _copy.Miejscowosc,
        Nazwa = _copy.Nazwa,
        Pesel = _copy.Nazwa,
        PozId = _copy.Id,
        Regon = _copy.Regon,
        TIN = _copy.TIN,
        UlicaNumer = _copy.UlicaNr
      };
    }
  }
}
