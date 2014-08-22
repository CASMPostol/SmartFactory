//<summary>
//  Title   : public static class CleanupContent
//  System  : Microsoft VisulaStudio 2013 / C#
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
    /// <param name="connectionString">The connection string.</param>
    /// <param name="progress">Used to report the progress of the operation.</param>
    public static void Go(string siteURL, string connectionString, Action<object, ProgressChangedEventArgs> progress)
    {
      Linq2SQL.IPRDEV _sqledc = Linq2SQL.IPRDEV.Connect2SQL(connectionString, progress);
      using (Entities entities = new Entities(siteURL))
      {
        progress(null, new ProgressChangedEventArgs(1, String.Format("Starting CleanupContent. It could take several minutes")));
        int _StockEntryArchived = 0;
        int _StockLibArchived = 0;
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockLib entries"));
        List<StockLib> _slList = entities.StockLibrary.ToList<StockLib>();
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockLib entries", _slList.Count)));
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockEntry entries"));
        List<StockEntry> _seList = entities.StockEntry.ToList<StockEntry>();
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockEntry entries", _seList.Count)));
        foreach (StockEntry _sex in _seList)
        {
          if (_sex.StockLibraryIndex == null)
          {
            _StockEntryArchived++;
            entities.StockEntry.DeleteOnSubmit(_sex);
            Linq2SQL.StockEntry.MarkSQLOnly(_sqledc, _sex.Id.Value);
          }
        }
        progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} StockEntry entries.", _StockEntryArchived)));
        SPSubmitChanges(entities, _sqledc, progress);
        foreach (StockLib _selX in _slList)
        {
          if (_selX.Stock2JSOXLibraryIndex == null)
          {
            foreach (StockEntry _sex in _selX.StockEntry)
            {
              entities.StockEntry.DeleteOnSubmit(_sex);
              Linq2SQL.StockEntry.MarkSQLOnly(_sqledc, _sex.Id.Value);
            }
            progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} Stock entries.", _selX.StockEntry.Count)));
            _StockEntryArchived += _selX.StockEntry.Count;
            _StockLibArchived++;
            entities.StockLibrary.DeleteOnSubmit(_selX);
            Linq2SQL.StockLibrary.MarkSQLOnly(_sqledc, _selX.Id.Value);
          }
        }
        progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} StockLibrary entries.", _StockLibArchived)));
        SPSubmitChanges(entities, _sqledc, progress);
        Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Cleanup, progress);
        progress(null, new ProgressChangedEventArgs(1, String.Format("Finished CleanupContent; deleted {0} StockEntry entries and {1} StockEntry.", _StockLibArchived, _StockEntryArchived)));
      }
    }
    private static void SPSubmitChanges(Entities spEntities, Linq2SQL.IPRDEV sqlEntities, Action<object, ProgressChangedEventArgs> progress)
    {
      progress(null, new ProgressChangedEventArgs(1, "Submitting changes to SharePoint"));
      spEntities.SubmitChanges();
      progress(null, new ProgressChangedEventArgs(1, "Submitting changes to SQL"));
      sqlEntities.SubmitChanges();
    }
  }
}
