//<summary>
//  Title   : public abstract class QuantityDescription
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

namespace CAS.SmartFactory.Customs.Messages
{
  /// <summary>
  /// abstract class QuantityDescription
  /// </summary>
  public abstract class QuantityDescription
  {
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetItemNo();
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetNetMass();
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public abstract string GetUnits();
  }
}
