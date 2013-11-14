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

using System;
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
    public static SADZgloszenie Create( SADZgloszenieTowar[] goods, SADZgloszenieUC customsOffice, string recipientOrganizationJson, Serialization.Organization senderOrganizationJson )
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
        MiejsceData = new SADZgloszenieMiejsceData() { Data = DateTime.Today, DataSpecified = true, Miejsce = "LODZ" }
      };
      return _new;
    }
    /// <summary>
    /// Creates the sad zgloszenie nadawca.
    /// </summary>
    /// <param name="stringJson">The string json.</param>
    /// <returns></returns>
    public static SADZgloszenieNadawca CreateSADZgloszenieNadawca( Organization stringJson )
    {
      return new SADZgloszenieNadawca()
      {
        CRP = string.Empty,
        EORI = stringJson.EORI,
        KodPocztowy = stringJson.Kod,
        Kraj = stringJson.Kraj,
        Miejscowosc = stringJson.Miejscowosc,
        Nazwa = stringJson.Nazwa,
        Pesel = stringJson.Nazwa,
        PozId = stringJson.Id,
        Regon = stringJson.Regon,
        TIN = stringJson.TIN,
        UlicaNumer = stringJson.UlicaNr
      };
    }
    /// <summary>
    /// Creates the sad zgloszenie odbiorca.
    /// </summary>
    /// <param name="stringJson">The string json.</param>
    /// <returns></returns>
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
