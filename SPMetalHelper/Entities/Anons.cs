using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
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
      Treść = message;
      this.Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0);
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
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114);
        return;
      }
      Anons log = new Anons(source, message);
      edc.EventLogList.InsertOnSubmit(log);
      edc.SubmitChangesSilently(Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues);
    }
  }
}
