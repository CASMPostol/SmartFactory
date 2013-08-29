//<summary>
//  Title   : class Anons
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  /// Adds a message to the Event log list.
  /// </summary>
  public partial class Anons
  {
    /// <summary>
    /// Creates the anons.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static Anons CreateAnons( string source, string message )
    {
      return new Anons()
      {
        Title = source,
        Body = message,
        Expires = DateTime.Now + new TimeSpan( 2, 0, 0, 0 )
      };
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public static void WriteEntry( EntitiesDataContext edc, string source, string message )
    {
      if ( edc == null )
      {
        EventLog.WriteEntry( "CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 40 );
        return;
      }
      Anons log = CreateAnons( source, message );
      edc.EventLogList.InsertOnSubmit( log );
      edc.SubmitChangesSilently( Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues );
    }
    /// <summary>
    /// Reports the exception.
    /// </summary>
    /// <param name="edc">The <see cref="EntitiesDataContext"/> object containing Linq Entities</param>
    /// <param name="source">The source location of the exception.</param>
    /// <param name="ex">The <see cref="Exception "/> to log.</param>
    public static void ReportException(EntitiesDataContext edc, string source, Exception ex )
    {
      try
      {
        Anons.WriteEntry( edc, source, ex.Message );
      }
      catch ( Exception ) { }
    }
  }
}
