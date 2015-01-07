//<summary>
//  Title   : Name of Application
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  internal class TraceLogWriter: TextWriter
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TraceLogWriter"/> class that defines delegate <paramref name="trace"/> to be used to write string.
    /// </summary>
    /// <param name="trace">The trace.</param>
    internal TraceLogWriter(Action<String> trace)
    {
      m_Trace = trace;
    }
    /// <summary>
    /// Writes a string using defined delegate.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public override void Write(string value)
    {
      m_Trace(value);
    }
    /// <summary>
    /// Writes a string using defined delegate. The line terminator is omitted. 
    /// </summary>
    /// <param name="value">The string to write. If <paramref name="value" /> is null, nothing is written.</param>
    public override void WriteLine(string value)
    {
      Write(value);
    }
    /// <summary>
    /// It is not implemented.
    /// </summary>
    /// <value>The encoding.</value>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Encoding Encoding
    {
      get { throw new NotImplementedException(); }
    }

    private Action<String> m_Trace;
  }
}
