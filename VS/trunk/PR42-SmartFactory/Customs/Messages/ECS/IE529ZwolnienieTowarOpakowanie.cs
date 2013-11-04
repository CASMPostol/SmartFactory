//<summary>
//  Title   : partial class IE529ZwolnienieTowarOpakowanie
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


namespace CAS.SmartFactory.Customs.Messages.ECS
{

  /// <summary>
  /// partial class IE529ZwolnienieTowarOpakowanie
  /// </summary>
  public partial class IE529ZwolnienieTowarOpakowanie: PackageDescription
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
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public override string GetPackage()
    {
      return this.Rodzaj;
    }

  }
}
