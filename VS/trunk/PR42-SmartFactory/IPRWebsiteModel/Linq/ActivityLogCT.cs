using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Adds a message to the Event log list.
  /// </summary>
  public partial class ActivityLogCT
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public ActivityLogCT( string source, string message )
      : base()
    {
      Title = source;
      Treść = message;
      this.Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 );
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
      using ( Entities edc = new Entities(url) )
        ActivityLogCT.WriteEntry( edc, source, message );
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public static void WriteEntry( Entities edc, string source, string message )
    {
      if ( edc == null )
      {
        EventLog.WriteEntry( "CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114 );
        return;
      }
      ActivityLogCT log = new ActivityLogCT( source, message );
      edc.ActivityLog.InsertOnSubmit( log );
      edc.SubmitChangesSilently( Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues );
    }
    /// <summary>
    /// Checks for a condition and displays a message if the condition is false.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> object.</param>
    /// <param name="condition"> true to prevent a message being displayed; otherwise, false.</c> [condition].</param>
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

