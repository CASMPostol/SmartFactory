
//<summary>
//  Title   : Adds a message to the Event log list.
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
using System.Diagnostics;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Adds a message to the Event log list.
  /// </summary>
  public sealed partial class ActivityLogCT
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="message">The string to write to the event log.</param>
    /// <param name="level">Specifies what messages to output for the.</param>
    /// <param name="source">The source denominator of the message.</param>
    public ActivityLogCT( string title, string message, TraceLevel level, string source  )
      : this()
    {
      Title = title;
      Body = message;
      this.ActivityPriority = level.ToString();
      this.ActivitySource = source;
      this.Expires = DateTime.Now + new TimeSpan( 2, 0, 0, 0 );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public static void WriteEntry( string source, string message )
    {
      using ( Entities edc = new Entities() )
        ActivityLogCT.WriteEntry( edc, source, message );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    /// <param name="url">The URL.</param>
    public static void WriteEntry( string source, string message, string url )
    {
      using ( Entities edc = new Entities( url ) )
        ActivityLogCT.WriteEntry( edc, source, message );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="entities">Provides LINQ (Language Integrated Query) access to, and change tracking for,
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public static void WriteEntry( Entities entities, string source, string message )
    {
      if ( entities == null )
      {
        EventLog.WriteEntry( "CAS.SmartFactory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114 );
        return;
      }
      ActivityLogCT log = new ActivityLogCT( source, message, TraceLevel.Error, "ActivityLogCT" );
      entities.ActivityLog.InsertOnSubmit( log );
      entities.SubmitChangesSilently( Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues );
    }
    /// <summary>
    /// Checks for a condition and displays a message if the condition is false.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> object.</param>
    /// <param name="condition"> <c>true</c> to prevent a message being displayed; otherwise, false.</param>
    /// <param name="source">The source of the assertion.</param>
    /// <param name="message">The message to log.</param>
    public static void Assert( Entities edc, bool condition, string source, string message )
    {
      if ( condition )
        return;
      ActivityLogCT.WriteEntry( edc, source, message );
    }
  }
}

