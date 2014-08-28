//<summary>
//  Title   : Archive class contain collection of function supporting archival data management
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NSLinq2SQL = CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using NSSPLinq = CAS.SmartFactory.IPR.Client.DataManagement.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement
{
  /// <summary>
  /// Archive class contain collection of function supporting archival data management
  /// </summary>
  public static class ArchiveContent
  {

    #region public
    /// <summary>
    /// Archive settings structure
    /// </summary>
    public struct ArchiveSettings
    {
      /// <summary>
      /// if <c>true</c> do archive of ipr entries 
      /// </summary>
      public bool DoArchiveIPR;
      /// <summary>
      /// if <c>true</c> do archive of Batch entries 
      /// </summary>
      public bool DoArchiveBatch;
      /// <summary>
      /// The archive batch delay
      /// </summary>
      public int ArchiveBatchDelay;
      /// <summary>
      /// The archive ipr delay
      /// </summary>
      public int ArchiveIPRDelay;
      /// <summary>
      /// The site URL
      /// </summary>
      public string SiteURL;
      /// <summary>
      /// The connection string
      /// </summary>
      public string ConnectionString;
      /// <summary>
      /// The user name
      /// </summary>
      public string UserName;
    }
    /// <summary>
    /// Performs Archive Content Task.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="ProgressChanged">The progress changed.</param>
    public static void Go(ArchiveSettings settings, Action<object, ProgressChangedEventArgs> ProgressChanged)
    {
      NSLinq2SQL.IPRDEV _sqledc = NSLinq2SQL.IPRDEV.Connect2SQL(settings.ConnectionString, ProgressChanged);
      if (String.IsNullOrEmpty(settings.ConnectionString))
        throw new ArgumentNullException("Database connection string cannot be null or empty.");
      if (!_sqledc.DatabaseExists())
        throw new ArgumentOutOfRangeException(String.Format("The database at {0} does nor exist.", settings.ConnectionString));
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        GoIPR(_spedc, _sqledc, settings, ProgressChanged);
        //GoBatch(edc, settings, ProgressChanged);
      }
    }
    #endregion

    #region private
    private static void GoIPR(NSSPLinq.Entities spEntities, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progress)
    {
      if (!settings.DoArchiveIPR)
      {
        progress(null, new ProgressChangedEventArgs(1, "IPR archive skipped"));
        return;
      }
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting IPR archive-{0} delay. It could take several minutes", settings.ArchiveIPRDelay)));
      int _disposalsArchived = 0;
      int _iprArchived = 0;
      progress(null, new ProgressChangedEventArgs(1, "Buffering Disposal and IPR entries"));
      List<NSSPLinq.Disposal> _dspsls = spEntities.Disposal.ToList<NSSPLinq.Disposal>();
      //Select delete candidates.
      List<NSSPLinq.IPR> _toDeleteIPR = new List<NSSPLinq.IPR>();
      List<Linq.Disposal> _toDeletedDisposal = new List<NSSPLinq.Disposal>();
      List<NSSPLinq.IArchival> _toBeMarkedArchival = new List<NSSPLinq.IArchival>();
      foreach (Linq.IPR _iprX in spEntities.IPR)
      {
        if (_iprX.AccountClosed.Value == true && _iprX.ClosingDate.IsLatter(settings.ArchiveIPRDelay))
        {
          _iprArchived++;
          bool _any = false;
          foreach (NSSPLinq.Disposal _dspx in _iprX.Disposal)
            if (_dspx.Disposal2BatchIndex == null || _dspx.Disposal2BatchIndex.FGQuantity.Value != 0 || _dspx.Disposal2BatchIndex.BatchStatus.Value != NSSPLinq.BatchStatus.Final)
            {
              _any = true;
              break;
            }
          if (_any)
            continue;
          _toDeletedDisposal.AddRange(_iprX.Disposal);
          _toDeleteIPR.Add(_iprX);
        }
      }
      progress(null, new ProgressChangedEventArgs(1, String.Format("Selected {0} IPR accounts and {1} disposals entries to be deleted.", _toDeleteIPR.Count, _toDeletedDisposal.Count)));
      NSSPLinq.Item.Delete<NSSPLinq.Disposal>(spEntities.Disposal, _toDeletedDisposal, _toBeMarkedArchival, x => sqledc.Disposal.GetAt<NSLinq2SQL.Disposal>(x), (id, listName) => AddLog(sqledc.ArchivingLogs, id, listName, settings.UserName));
      SubmitChanges(spEntities, sqledc, progress);
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Finished Archive GoIPR; Archived {0} IPR accounts and {1} disposals.", _iprArchived, _disposalsArchived), spEntities));
    }
    private static void AddLog(System.Data.Linq.Table<NSLinq2SQL.ArchivingLogs> log, int itemID, string listName, string userName)
    {
      NSLinq2SQL.ArchivingLogs _newItem = new NSLinq2SQL.ArchivingLogs()
      {
        Date = DateTime.UtcNow,
        ItemID = itemID,
        ListName = listName,
        UserName = userName
      };
      log.InsertOnSubmit(_newItem);
    }
    private static void GoBatch(NSSPLinq.Entities entities, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progress)
    {
      if (!settings.DoArchiveBatch)
      {
        progress(null, new EntitiesChangedEventArgs(1, "Batch archive skipped", entities));
        return;
      }
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Starting Batch archive - {0} delay. It could take several minutes", settings.ArchiveBatchDelay), entities));
      //TODO progress cig exclude if final or intermediate exist
      //TODO progress arch if there is new 
      //TODO cuttfiler AD ??
      progress(null, new EntitiesChangedEventArgs(1, "Buffering Material entries", entities));
      List<NSSPLinq.Material> _mtrl = entities.Material.ToList<NSSPLinq.Material>();
      int _batchArchived = 0;
      int _progressBatchArchived = 0;
      int _materialArchived = 0;
      IEnumerable<NSSPLinq.Batch> _allB = entities.Batch.ToList<NSSPLinq.Batch>().Where<NSSPLinq.Batch>(x => x.ProductType.Value == NSSPLinq.ProductType.Cigarette);
      Dictionary<string, IGrouping<string, NSSPLinq.Batch>> _progressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
      List<NSSPLinq.Batch> _noProgressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Final || _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Intermediate select _fbx).ToList<NSSPLinq.Batch>();
      string _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries", _progressBatch.Count, _noProgressBatch.Count);
      progress(null, new EntitiesChangedEventArgs(1, _msg, entities));
      foreach (NSSPLinq.Batch _batchX in _noProgressBatch)
      {
        if (_progressBatch.Keys.Contains(_batchX.Batch0))
        {
          Debug.Assert(_batchX.BatchStatus.Value != NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be != BatchStatus.Progress");
          foreach (NSSPLinq.Batch _bpx in _progressBatch[_batchX.Batch0])
          {
            Debug.Assert(_bpx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be == BatchStatus.Progress");
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
            _bpx.Archival = true;
            _progressBatchArchived++;
            SubmitChanges(entities, sqledc, progress);
            foreach (NSSPLinq.Material _mpx in _bpx.Material)
            {
              Debug.Assert(_mpx.Material2BatchIndex == _bpx, "Wrong Material to Batch link");
              progress(null, new EntitiesChangedEventArgs(1, null, entities));
              _materialArchived++;
              _mpx.Archival = true;
            }
          }
        } //foreach (Batch _batchX
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
        if (_batchX.ProductType.Value != NSSPLinq.ProductType.Cigarette || _batchX.FGQuantityAvailable > 0)
          continue;
        bool _2archive = true;
        foreach (NSSPLinq.Material _material in _batchX.Material)
        {
          progress(null, new EntitiesChangedEventArgs(1, null, entities));
          foreach (NSSPLinq.Disposal _disposalX in _material.Disposal)
          {
            if (_disposalX.CustomsStatus.Value != NSSPLinq.CustomsStatus.Finished || !_disposalX.SADDate.IsLatter(settings.ArchiveBatchDelay))
            {
              _2archive = false;
              break;
            }
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
          }
          if (!_2archive)
            break;
        } // foreach (Material
        if (_2archive)
        {
          _batchX.Archival = true;
          _batchArchived++;
          foreach (NSSPLinq.Material _material in _batchX.Material)
          {
            _material.Archival = true;
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
          }
          SubmitChanges(entities, sqledc, progress);
        }
      }// foreach (Batch 
      SubmitChanges(entities, sqledc, progress);
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Finished Archive.GoBatch; Archived {0} Batch final, Batch progress {1} and {2} Material entries.", _batchArchived, _progressBatchArchived, _materialArchived), entities));
    }
    private static void SubmitChanges(NSSPLinq.Entities entities, Linq2SQL.IPRDEV sqledc, Action<object, ProgressChangedEventArgs> progress)
    {
      progress(null, new ProgressChangedEventArgs(1, "SubmitChanges"));
      entities.SubmitChanges();
      sqledc.SubmitChanges();
    }
    #endregion

  }
}
