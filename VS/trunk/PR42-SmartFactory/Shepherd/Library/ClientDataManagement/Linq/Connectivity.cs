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
  public static class Connectivity
  {

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
