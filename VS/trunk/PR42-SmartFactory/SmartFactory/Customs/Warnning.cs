//<summary>
//  Title   : Name of Application
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

namespace CAS.SmartFactory.Customs
{
  /// <summary>
  /// Warnning class - provides message and severity information
  /// </summary>
  public class Warnning
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Warnning"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="fatal">if set to <c>true</c> the severity of the problem is fatal.</param>
    public Warnning(string message, bool fatal)
    {
      Message = message;
      Fatal = fatal;
    }
    /// <summary>
    /// The message
    /// </summary>
    public string Message;
    /// <summary>
    /// The severity information
    /// </summary>
    public bool Fatal;
  }

}
