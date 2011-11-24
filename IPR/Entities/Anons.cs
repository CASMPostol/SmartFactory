using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Anons
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public Anons(string source, string message)
    {
      Tytuł = source;
      Treść = "Import of a stock xml message failed because the file is empty.";
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    internal static void WriteEntry(EntitiesDataContext edc, string source, string message)
    {
      if (edc == null)
      {
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
        return;
      }
      Anons log = new Anons(source, message);
      edc.ActivityLog.InsertOnSubmit(log);
    }
  }
}
