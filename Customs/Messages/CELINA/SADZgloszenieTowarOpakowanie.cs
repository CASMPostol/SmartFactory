//<summary>
//  Title   : partial class SADZgloszenieTowarOpakowanie
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
  /// partial class SADZgloszenieTowarOpakowanie
  /// </summary>
  public partial class SADZgloszenieTowarOpakowanie: PackageDescription
  {
    /// <summary>
    /// Calculates number of packages.
    /// </summary>
    /// <param name="sADZgloszenieTowarOpakowanie">The arguments ad zgloszenie towar opakowanie.</param>
    internal static decimal Packages( SADZgloszenieTowarOpakowanie[] sADZgloszenieTowarOpakowanie )
    {
      decimal _ret = 0;
      foreach ( SADZgloszenieTowarOpakowanie _pckIx in sADZgloszenieTowarOpakowanie )
        _ret += _pckIx.LiczbaOpakowanSpecified ? _pckIx.LiczbaOpakowan : 0;
      return _ret;
    }
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    /// <summary>
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public override string GetPackage()
    {
      return this.Rodzaj;
    }
  }
}
