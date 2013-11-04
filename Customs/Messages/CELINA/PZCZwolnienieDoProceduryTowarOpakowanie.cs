//<summary>
//  Title   : partial class PZCZwolnienieDoProceduryTowarOpakowanie
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
  /// partial class PZCZwolnienieDoProceduryTowarOpakowanie
  /// </summary>
  public partial class PZCZwolnienieDoProceduryTowarOpakowanie : PackageDescription
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
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public override string GetPackage()
    {
      return this.Rodzaj;
    }
  }
}
