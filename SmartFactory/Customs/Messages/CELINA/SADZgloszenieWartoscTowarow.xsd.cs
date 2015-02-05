//<summary>
//  Title   : partial class SADZgloszenieWartoscTowarow
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
  /// <summary>
  /// partial class SADZgloszenieWartoscTowarow
  /// </summary>
  public partial class SADZgloszenieWartoscTowarow
  {
    /// <summary>
    /// Creates new instance of the <see cref="SADZgloszenieWartoscTowarow"/> with default Waluta = "PLN", KursWaluty = 1.0m
    /// </summary>
    /// <param name="value">The value.</param>
    public static SADZgloszenieWartoscTowarow Create( decimal value )
    {
      return new SADZgloszenieWartoscTowarow()
      {
        KursWaluty = 1.0m,
        KursWalutySpecified = true,
        SzczegolyWartosci = string.Empty,
        Waluta = "PLN",
        Wartosc = value,
        WartoscSpecified = true,
      };
    }
  }
}
