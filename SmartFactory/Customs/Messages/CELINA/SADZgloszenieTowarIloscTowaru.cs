//<summary>
//  Title   : partial class SADZgloszenieTowarIloscTowaru
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

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  /// <summary>
  /// partial class SADZgloszenieTowarIloscTowaru
  /// </summary>
  public partial class SADZgloszenieTowarIloscTowaru : QuantityDescription
  {
    /// <summary>
    /// Creates the an object of <see cref="SADZgloszenieTowarIloscTowaru"/>.
    /// </summary>
    /// <param name="pozId">The poz unique identifier.</param>
    /// <param name="netMass">The net mass.</param>
    /// <param name="grossMas">The gross mas.</param>
    /// <returns></returns>
    public static SADZgloszenieTowarIloscTowaru Create(ref decimal pozId, decimal netMass, decimal grossMas)
    {
      return new SADZgloszenieTowarIloscTowaru()
      {
        PozId = pozId,
        Jm = "KGM",
        Ilosc = netMass,
        GrossMas = grossMas
      };
    }
    /// <summary>
    /// Gets the gross mas.
    /// </summary>
    /// <value>
    /// The gross mas.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public decimal GrossMas { get; private set; }
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetNetMass()
    {
      return Convert.ToDouble(this.Ilosc);
    }
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public override string GetUnits()
    {
      return this.Jm;
    }
  }
}
