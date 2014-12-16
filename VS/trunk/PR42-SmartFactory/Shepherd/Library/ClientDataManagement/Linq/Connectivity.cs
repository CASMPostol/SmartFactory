//<summary>
//  Title   : Connectivity
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
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq
{
  /// <summary>
  /// Class Connectivity - helper class to test connectivity with SharePoint website and SQL data base.
  /// </summary>
  public static class Connectivity
  {

    /// <summary>
    /// Tests the connection with the SharePoint website.
    /// </summary>
    /// <param name="sharePointServerURL">The SharePoint server URL.</param>
    /// <param name="reportProgress">The report progress.</param>
    /// <returns><c>true</c> if the connection succeeded, <c>false</c> otherwise.</returns>
    /// <exception cref="System.ArgumentException">The currency list is empty. It must be added at least one element before any further operations.</exception>
    public static bool TestConnection(string sharePointServerURL, Action<ProgressChangedEventArgs> reportProgress)
    {
      try
      {
        using (Entities _edc = new Entities(sharePointServerURL))
        {
          reportProgress(new ProgressChangedEventArgs(1, String.Format("Trying to get access to SharePoint content at {0}.", sharePointServerURL)));
          if (!_edc.Currency.ToList<Currency>().Any())
            throw new ArgumentException("The currency list is empty. It must be added at least one element before any further operations.");
          return true;
        }
      }
      catch (Exception ex)
      {
        reportProgress(new ProgressChangedEventArgs(1, String.Format("Failed to get access to SharePoint content because: {0}.", ex.Message)));
      }
      return false;
    }

  }
}
