//<summary>
//  Title   : public static class CleanupContent
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement
{
  /// <summary>
  /// Helper class providing all functionality to cleanup content - remove useless entries.
  /// </summary>
  public static class CleanupContent
  {
    /// <summary>
    /// Runs synchronously the cleanup content operation.
    /// </summary>
    /// <param name="siteURL">The site URL.</param>
    /// <param name="progress">Used to report the progress of the operation.</param>
    public static void Go(string siteURL, Func<object, ProgressChangedEventArgs, bool> progress)
    {
      using (Entities entities = new Entities(siteURL))
      {
        progress(null, new ProgressChangedEventArgs(1, String.Format("Starting CleanupContent. It could take several minutes")));
        int _StockEntryArchived = 0;
        int _StockLibArchived = 0;
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockEntry entries"));
        List<StockEntry> _seList = entities.StockEntry.ToList<StockEntry>();
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockEntry entries", _seList.Count)));
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockLib entries"));
        List<StockLib> _slList = entities.StockLibrary.ToList<StockLib>();
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockLib entries", _slList.Count)));
        foreach (StockLib _selX in _slList)
        {
          if (_selX.Stock2JSOXLibraryIndex == null)
          {
            _StockLibArchived++;
            //entities.StockLibrary.DeleteOnSubmit(_selX);
          }
        }
        progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} StockLibrary entries.", _StockLibArchived)));
        SubmitChanges(entities, progress);
        foreach (StockEntry _seix in _seList)
        {
          if (_seix.StockLibraryIndex == null)
          {
            _StockEntryArchived++;
            //entities.StockEntry.DeleteOnSubmit(_seix);
          }
          progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} StockEntry entries.", _StockEntryArchived)));
        }
        SubmitChanges(entities, progress);
        progress(null, new ProgressChangedEventArgs(1, String.Format("Finished CleanupContent; deleted {0} StockEntry entries and {1} StockEntry.", _StockLibArchived, _StockEntryArchived)));
      }
    }
    private static void SubmitChanges(Entities entities, Func<object, ProgressChangedEventArgs, bool> progress)
    {
      progress(null, new ProgressChangedEventArgs(1, "SubmitChanges"));
      entities.SubmitChanges();
    }
  }
}
