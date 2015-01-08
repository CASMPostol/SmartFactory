﻿//<summary>
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
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement
{
  /// <summary>
  /// Class ArchivingContent - provides service to archive the Shepherd website to SQL backup database.
  /// </summary>
  public static class ArchivingContent
  {
    /// <summary>
    /// Does the archiving of the Shepherd content.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="sqlConnectionString">The SQL connection string.</param>
    /// <param name="archivalDelay">The archival delay.</param>
    /// <param name="rowLimit">The row limit.</param>
    /// <param name="reportProgress"> action to trace progress in a log stream.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    /// <exception cref="System.ApplicationException">DoSynchronizationContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.</exception>
    public static void DoArchivingContent(string URL, string sqlConnectionString, int archivalDelay, int rowLimit, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      bool _breakingIssueEncountered = false;
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoArchivingContent URL: {0}, connection string: {1}, ArchivalDelay: {2}, RowLimit: {3}", URL, sqlConnectionString, archivalDelay, rowLimit)));
      trace("Establishing connection with the SP site and SQL database.");
      using (Linq.Entities _edc = new Linq.Entities(trace, URL))
      {
        using (Linq2SQL.SHRARCHIVE _sqledc = Linq2SQL.SHRARCHIVE.Connect2SQL(sqlConnectionString, y => trace(y)))
        {
          if (false)
            _breakingIssueEncountered = true;
        }
      }
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoArchivingContent"));
      if (_breakingIssueEncountered)
        throw new ApplicationException("DoArchivingContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.");
    }
  }
}
