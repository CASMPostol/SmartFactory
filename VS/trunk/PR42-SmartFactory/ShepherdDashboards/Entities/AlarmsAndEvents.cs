using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  /// <summary>
  /// Alarms And Events list entry
  /// </summary>
  public partial class AlarmsAndEvents
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="_title">The evrnt title.</param>
    /// <param name="_partner">The partner associated with the event.</param>
    /// <param name="_shippingIndex">Index of the shipping.</param>
    public AlarmsAndEvents(string _title, Partner _partner, Shipping _shippingIndex)
    {
      Tytuł = _title;
      this.VendorName = _partner;
      this.ShippingIndex = _shippingIndex;
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for,
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="_title">The evrnt title.</param>
    /// <param name="_partner">The partner associated with the event.</param>
    /// <param name="_shippingIndex">Index of the shipping.</param>
    internal static void WriteEntry(EntitiesDataContext edc, string _title, Partner _partner, Shipping _shippingIndex)
    {
      if (edc == null)
      {
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114);
        return;
      }
      AlarmsAndEvents _log = new AlarmsAndEvents(_title, _partner, _shippingIndex);
      edc.AlarmsAndEvents.InsertOnSubmit(_log);
      edc.SubmitChanges(Microsoft.SharePoint.Linq.ConflictMode.ContinueOnConflict);
    }
  }
}
