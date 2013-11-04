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

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  public partial class SADZgloszenieUC
  {
    /// <summary>
    /// Creates the instance of <see cref="SADZgloszenieUC" />.
    /// </summary>
    /// <param name="customsOffice">The customs office.</param>
    /// <param name="localization">The localization.</param>
    /// <returns></returns>
    public static SADZgloszenieUC Create( string customsOffice, string localization )
    {
      return new SADZgloszenieUC()
      {
        UCZgloszenia = customsOffice,
        UCGraniczny = customsOffice,
        Lokalizacja = new SADZgloszenieUCLokalizacja() { Miejsce = localization },
        SkladCelny = new SADZgloszenieUCSkladCelny() { Typ = "C", Miejsce = localization, Kraj = "PL" }
      };
    }
  }
}
