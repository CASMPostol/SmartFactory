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
using System.Diagnostics;
using System.Linq;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Archival
{
  /// <summary>
  /// Archive class contain collection of function supporting archival data management
  /// </summary>
  public static class Archive
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
    }
    /// <summary>
    /// Goes the specified edc.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="ProgressChanged">The progress changed.</param>
    public static void Go(ArchiveSettings settings, Action<object, EntitiesChangedEventArgs> ProgressChanged)
    {
      using (Entities edc = new Entities(settings.SiteURL))
      {
        GoIPR(edc, settings, ProgressChanged);
        GoBatch(edc, settings, ProgressChanged);
      }
    }
    #endregion

    #region private
    private static void SubmitChanges(Entities entities, Action<object, EntitiesChangedEventArgs> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "SubmitChanges", entities));
      entities.SubmitChanges();
    }
    private static void GoIPR(Entities entities, ArchiveSettings settings, Action<object, EntitiesChangedEventArgs> progress)
    {
      if (!settings.DoArchiveIPR)
      {
        progress(null, new EntitiesChangedEventArgs(1, "IPR archive skipped", entities));
        return;
      }
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Starting IPR archive-{0} delay. It could take several minutes", settings.ArchiveIPRDelay), entities));
      int _disposalsArchived = 0;
      int _iprArchived = 0;
      progress(null, new EntitiesChangedEventArgs(1, "Buffering Disposal entries", entities));
      List<Disposal> _dspsls = entities.Disposal.ToList<Disposal>();
      foreach (IPR.Client.DataManagement.Linq.IPR _iprX in entities.IPR)
      {
        if (_iprX.AccountClosed.Value == true && _iprX.ClosingDate.IsLatter(settings.ArchiveIPRDelay))
        {
          _iprArchived++;
          _iprX.Archival = true;
          foreach (Disposal _dspx in _iprX.Disposal)
          {
            _dspx.Archival = true;
            _disposalsArchived++;
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
          }
        }
        else
          _iprX.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(entities, progress);
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Finished Archive GoIPR; Archived {0} IPR accounts and {1} disposals.", _iprArchived, _disposalsArchived), entities));
    }
    private static void GoBatch(Entities entities, ArchiveSettings settings, Action<object, EntitiesChangedEventArgs> progress)
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
      List<Material> _mtrl = entities.Material.ToList<Material>();
      int _batchArchived = 0;
      int _progressBatchArchived = 0;
      int _materialArchived = 0;
      IEnumerable<Batch> _allB = entities.Batch.ToList<Batch>().Where<Batch>(x => x.ProductType.Value == ProductType.Cigarette);
      Dictionary<string, IGrouping<string, Batch>> _progressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
      List<Batch> _noProgressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == BatchStatus.Final || _fbx.BatchStatus.Value == BatchStatus.Intermediate select _fbx).ToList<Batch>();
      string _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries", _progressBatch.Count, _noProgressBatch.Count);
      progress(null, new EntitiesChangedEventArgs(1, _msg, entities));
      foreach (Batch _batchX in _noProgressBatch)
      {
        if (_progressBatch.Keys.Contains(_batchX.Batch0))
        {
          Debug.Assert(_batchX.BatchStatus.Value != BatchStatus.Progress, "Wrong BatchStatus should be != BatchStatus.Progress");
          foreach (Batch _bpx in _progressBatch[_batchX.Batch0])
          {
            Debug.Assert(_bpx.BatchStatus.Value == BatchStatus.Progress, "Wrong BatchStatus should be == BatchStatus.Progress");
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
            _bpx.Archival = true;
            _progressBatchArchived++;
            SubmitChanges(entities, progress);
            foreach (Material _mpx in _bpx.Material)
            {
              Debug.Assert(_mpx.Material2BatchIndex == _bpx, "Wrong Material to Batch link");
              progress(null, new EntitiesChangedEventArgs(1, null, entities));
              _materialArchived++;
              _mpx.Archival = true;
            }
          }
        } //foreach (Batch _batchX
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
        if (_batchX.ProductType.Value != ProductType.Cigarette || _batchX.FGQuantityAvailable > 0)
          continue;
        bool _2archive = true;
        foreach (Material _material in _batchX.Material)
        {
          progress(null, new EntitiesChangedEventArgs(1, null, entities));
          foreach (Disposal _disposalX in _material.Disposal)
          {
            if (_disposalX.CustomsStatus.Value != CustomsStatus.Finished || !_disposalX.SADDate.IsLatter(settings.ArchiveBatchDelay))
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
          foreach (Material _material in _batchX.Material)
          {
            _material.Archival = true;
            progress(null, new EntitiesChangedEventArgs(1, null, entities));
          }
          SubmitChanges(entities, progress);
        }
      }// foreach (Batch 
      SubmitChanges(entities, progress);
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Finished Archive.GoBatch; Archived {0} Batch final, Batch progress {1} and {2} Material entries.", _batchArchived, _progressBatchArchived, _materialArchived), entities));
    }
    #endregion

  }
}
