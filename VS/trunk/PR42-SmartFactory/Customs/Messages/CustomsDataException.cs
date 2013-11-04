//<summary>
//  Title   : public class CustomsDataException
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

namespace CAS.SmartFactory.Customs.Messages
{
  /// <summary>
  /// public class CustomsDataException
  /// </summary>
  public class CustomsDataException : ApplicationException
  {
    /// <summary>
    /// Initializes a new instance of the System.ApplicationException class with a specified error message.
    /// </summary>
    /// <param name="source">The name of the application or the object that causes the error.</param>
    /// <param name="message">A message that describes the error.</param>
    public CustomsDataException(string source, string message)
      : base(message)
    {
      Source = source;
    }
  }
}
