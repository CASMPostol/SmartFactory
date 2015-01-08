//<summary>
//  Title   : Entities
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class Entities - provides additional constructor to implement tracing using the Entities.Log property.
  /// </summary>
  public partial class Entities
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Entities"/> class. 
    /// </summary>
    /// <param name="trace">The trace is used to write and log message to an external trace.</param>
    /// <param name="requestUrl">The URL of a Windows SharePoint Services "14" Web site that provides client site access and change tracking for the specified Web site.</param>
    public Entities(Action<String> trace, string requestUrl)
      : this(requestUrl)
    {
      this.Log = new TraceLogWriter(trace);
    }
  }
}
