//<summary>
//  Title   : SynchronizationContent
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
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class SynchronizationContent provides service to synchronize SQL backup database and SharePoint content.
  /// </summary>
  public static class SynchronizationContent
  {
    /// <summary>
    /// Runs the synchronization of the SharePoint and SQL.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="sqlConnectionString">The SQL connection string.</param>
    /// <param name="reportProgress">An action to trace progress in a log stream.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    public static void DoSynchronizationContent(string URL, string sqlConnectionString, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      bool _breakingIssueEncountered = false;
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoSynchronizationContent URL: {0}, connection string: {1}", URL, sqlConnectionString)));
      trace("Establishing connection with the SP site and SQL database.");
      using (Linq.Entities _edc = new Linq.Entities(trace, URL))
      {
        using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
        {
          if (false)
            _breakingIssueEncountered = true;
        }
      }
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoSynchronizationContent"));
      if (_breakingIssueEncountered)
        throw new ApplicationException("DoSynchronizationContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.");
    }
  }
}
