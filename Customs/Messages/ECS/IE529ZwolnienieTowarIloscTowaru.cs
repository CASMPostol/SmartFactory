//<summary>
//  Title   : partial class IE529ZwolnienieTowarIloscTowaru
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

namespace CAS.SmartFactory.Customs.Messages.ECS
{
  /// <summary>
  /// partial class IE529ZwolnienieTowarIloscTowaru
  /// </summary>
  public partial class IE529ZwolnienieTowarIloscTowaru : QuantityDescription
  {
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public override double? GetItemNo()
    {
      return null;
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
