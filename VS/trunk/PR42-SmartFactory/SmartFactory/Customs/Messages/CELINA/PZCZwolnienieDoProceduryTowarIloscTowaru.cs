//<summary>
//  Title   : public partial class PZCZwolnienieDoProceduryTowarIloscTowaru
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

namespace CAS.SmartFactory.Customs.Messages.CELINA.PZC
{
  /// <summary>
  /// partial class PZCZwolnienieDoProceduryTowarIloscTowaru
  /// </summary>
  public partial class PZCZwolnienieDoProceduryTowarIloscTowaru : QuantityDescription
  {
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
