//<summary>
//  Title   : abstract class PackageDescription
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
  /// abstract class PackageDescription
  /// </summary>
  public abstract class PackageDescription
  {
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetItemNo();
    /// <summary>
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPackage();
  }
}
