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
using CAS.SharePoint.Client.Link2SQL;
using CAS.SharePoint.Client.Linq2SP;
using NSLinq2SQL = CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;

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
      using (Entities _spedc = new Entities(siteURL))
      {
        progress(null, new ProgressChangedEventArgs(1, String.Format("Starting CleanupContent. It could take several minutes")));
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockLib entries"));
        List<StockLib> _slList = _spedc.StockLibrary.ToList<StockLib>();
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockLib entries", _slList.Count)));
        progress(null, new ProgressChangedEventArgs(1, "Buffering StockEntry entries"));
        List<StockEntry> _seToBeDeleted = _spedc.StockEntry.ToList<StockEntry>().Where<StockEntry>(x => x.StockLibraryIndex == null).ToList<StockEntry>();
        //Delete not associated StockEntries
        progress(null, new ProgressChangedEventArgs(1, String.Format("There is {0} StockEntry to be deleted because they are not associated with StockLib.", _seToBeDeleted.Count)));
        _spedc.StockEntry.Delete<StockEntry>(_seToBeDeleted, null, x => _sqledc.StockEntry.GetAt<NSLinq2SQL.StockEntry>(x), (id, listName) => _sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()));
        Link2SQLExtensions.SubmitChanges(_spedc, _sqledc, progress);
        _seToBeDeleted.Clear();
        List<StockLib> _slToBeDeleted = new List<StockLib>();
        foreach (StockLib _selX in _slList)
        {
          if (_selX.Stock2JSOXLibraryIndex != null)
            continue;
          _slToBeDeleted.Add(_selX);
          _seToBeDeleted.AddRange(_selX.StockEntry);
        }
        progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} Stock entries.", _seToBeDeleted.Count)));
        _spedc.StockEntry.Delete<StockEntry>(_seToBeDeleted, null, x => _sqledc.StockEntry.GetAt<NSLinq2SQL.StockEntry>(x), (id, listName) => _sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()));
        progress(null, new ProgressChangedEventArgs(1, String.Format("To be deleted {0} StockLib entries.", _slToBeDeleted.Count)));
        _spedc.StockLibrary.Delete<StockLib>(_slToBeDeleted, null, x => _sqledc.StockLibrary.GetAt<NSLinq2SQL.StockLibrary>(x), (id, listName) => _sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()));
        Link2SQLExtensions.SubmitChanges(_spedc, _sqledc, progress);
        //Update Activities Log
        Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Cleanup, progress);
        progress(null, new ProgressChangedEventArgs(1, "Finished Cleanup Content Operation."));
      }
    }
  }
}
