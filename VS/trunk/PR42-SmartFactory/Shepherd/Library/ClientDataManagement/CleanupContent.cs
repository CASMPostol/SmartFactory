//<summary>
//  Title   : CleanupContent
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
  /// Class CleanupContent - provides service to cleanup the website content.
  /// </summary>
  public static class CleanupContent
  {
    /// <summary>
    /// Does the content cleanup.
    /// </summary>
    /// <param name="URL">The requested URL.</param>
    /// <param name="reportProgress">An action to report the progress.</param>
    /// <param name="trace">An action to trace progress in a log stream.</param>
    /// <exception cref="System.ApplicationException">DoCleanupContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.</exception>
    public static void DoCleanupContent(string URL, Action<ProgressChangedEventArgs> reportProgress, Action<String> trace)
    {
      bool _breakingIssueEncountered = false;
      reportProgress(new ProgressChangedEventArgs(1, String.Format("Starting DoCleanupContent and establishing connection with the site {0}.", URL)));
      using (Linq.Entities _edc = new Linq.Entities(trace, URL))
      {
        if (false)
          _breakingIssueEncountered = true;
      }
      reportProgress(new ProgressChangedEventArgs(1, "Finished DoCleanupContent"));
      if (_breakingIssueEncountered)
        throw new ApplicationException("DoCleanupContent has encountered breaking inconsistency - review the log and remove problems to pass to next phase.");

    }

  }
}
