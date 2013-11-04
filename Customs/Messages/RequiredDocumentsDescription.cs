//<summary>
//  Title   : abstract class RequiredDocumentsDescription
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
  /// abstract class RequiredDocumentsDescription
  /// </summary>
  public abstract class RequiredDocumentsDescription
  {
    /// <summary>
    /// Gets the code.
    /// </summary>
    /// <returns></returns>
    public abstract string GetCode();
    /// <summary>
    /// Gets the number.
    /// </summary>
    /// <returns></returns>
    public abstract string GetNumber();
  }
}
