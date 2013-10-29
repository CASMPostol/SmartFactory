using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  public partial class SADZgloszenie
  {
    public static SADZgloszenie Create()
    {
      SADZgloszenie _new = new SADZgloszenie()
      {
        NrWlasny = "13SXX0000",
        P1a = "XX",
        P1b = "X",
        LiczbaPozycji = 1,
        LiczbaOpakowan = 1,
        KrajWysylki = "XX",
        KrajPrzeznaczenia = "XX",
        Kontenery = false,
        RodzajTransakcji = "11",
        MasaBrutto = 205,
        Rodzaj = new SADZgloszenieRodzaj() { Typ = "H", Podtyp = "A", Powiadomienie = false },
        UC = new SADZgloszenieUC()
          {
            UCZgloszenia = "PL362010",
            UCGraniczny = "PL362010",
            Lokalizacja = new SADZgloszenieUCLokalizacja() { Miejsce = "PL360000SC0002" },
            SkladCelny = new SADZgloszenieUCSkladCelny() { Typ = "C", Miejsce = "PL360000SC0002", Kraj = "PL" }
          },
        Nadawca = new SADZgloszenieNadawca[] { new SADZgloszenieNadawca() { PozId = 1, Nazwa = "JT INTERNATIONAL SA A MEMBER OF THE", UlicaNumer = "1,RUE DE LA GABELLE", KodPocztowy = "1211", Miejscowosc = "GENEVA", Kraj = "CH" } },
        Odbiorca = new SADZgloszenieOdbiorca[] { new SADZgloszenieOdbiorca() { PozId = 1, Nazwa = "JTI  POLSKA  SP. Z O.O.", UlicaNumer = "GOSTKOW STARY 42", KodPocztowy = "99-220", Miejscowosc = "WARTKOWICE", Kraj = "PL", TIN = "PL8280001819", Regon = "00130199100000", EORI = "PL828000181900000" } },
        TransportNaGranicy = null,
        WarunkiDostawy = new SADZgloszenieWarunkiDostawy() { Kod = "XXX", Miejsce = "XXX", MiejsceKod = "X" },

      };
      return _new;
    }

  }
}
