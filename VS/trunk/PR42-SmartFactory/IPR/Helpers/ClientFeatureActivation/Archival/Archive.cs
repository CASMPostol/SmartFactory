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
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation.Archival
{
  /// <summary>
  /// Archive class contain collection of function supporting archival data management
  /// </summary>
  public static class Archive
  {
    #region public
    /// <summary>
    /// Goes the specified edc.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="ProgressChanged">The progress changed.</param>
    public static void Go(string siteURL, Func<object, EntitiesChangedEventArgs, bool> ProgressChanged)
    {
      using (Entities edc = new Entities(siteURL))
      {
        GoIPR(edc, ProgressChanged);
        GoBatch(edc, ProgressChanged);
      }
    }
    #endregion

    #region private
    private static void GoIPR(Entities edc, Func<object, EntitiesChangedEventArgs, bool> ProgressChanged)
    {
      ProgressChanged(null, new EntitiesChangedEventArgs(1, "Starting Archive GoIPR", edc));
      int _disposalsArchived = 0;
      int _iprArchived = 0;
      foreach (IPR.WebsiteModel.Linq.IPR _iprX in edc.IPR)
      {
        if (_iprX.AccountClosed.Value == true && _iprX.ClosingDate.IsLatter(Properties.Settings.Default.IPRAccountArchivalDelay))
        {
          _iprArchived++;
          _iprX.Archival = true;
          foreach (Disposal _dspx in _iprX.Disposal)
          {
            _dspx.Archival = true;
            _disposalsArchived++;
            ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
          }
        }
        else
          _iprX.Archival = false;
        ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
      }
      edc.SubmitChanges();
      ProgressChanged(null, new EntitiesChangedEventArgs(1, String.Format("Archived {0} IPR accounts and {1} disposals.", _iprArchived, _disposalsArchived), edc));
    }
    private static void GoBatch(Entities edc, Func<object, EntitiesChangedEventArgs, bool> ProgressChanged)
    {
      ProgressChanged(null, new EntitiesChangedEventArgs(1, "Starting Archive GoBatch", edc));
      //TODO progress cig exclude if final or intermidiate exist
      //TODO progres arch if there is neww 
      //TODO cuttfiler AD ??
      int _batchArchived = 0;
      int _progressBatchArchived = 0;
      int _materialArchived = 0;
      IEnumerable<Batch> _allB = edc.Batch.ToList<Batch>().Where<Batch>(x => x.ProductType.Value == ProductType.Cigarette);
      Dictionary<string, IGrouping<string, Batch>> _progressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
      List<Batch> _noProgressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == BatchStatus.Final || _fbx.BatchStatus.Value == BatchStatus.Intermediate select _fbx).ToList<Batch>();
      string _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries", _progressBatch.Count, _noProgressBatch.Count);
      ProgressChanged(null, new EntitiesChangedEventArgs(1, _msg, edc));
      foreach (Batch _batchX in _noProgressBatch)
      {
        if (_progressBatch.Keys.Contains(_batchX.Batch0))
        {
          Debug.Assert(_batchX.BatchStatus.Value != BatchStatus.Progress, "Wrong BatchStatus should be != BatchStatus.Progress");
          foreach (Batch _bpx in _progressBatch[_batchX.Batch0])
          {
            Debug.Assert(_bpx.BatchStatus.Value == BatchStatus.Progress, "Wrong BatchStatus should be == BatchStatus.Progress");
            ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
            _bpx.Archival = true;
            _progressBatchArchived++;
            foreach (Material _mpx in _bpx.Material)
            {
              Debug.Assert(_mpx.Material2BatchIndex == _bpx, "Wrong Material to Batch link");
              ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
              _materialArchived++;
              _bpx.Archival = true;
            }
          }
        }
        ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
        if (_batchX.ProductType.Value != ProductType.Cigarette || _batchX.FGQuantityAvailable > 0)
        {
          _batchX.Archival = false;
          continue;
        }
        bool _2archive = true;
        foreach (Material _material in _batchX.Material)
        {
          ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
          foreach (Disposal _disposalX in _material.Disposal)
          {
            if (_disposalX.CustomsStatus.Value != CustomsStatus.Finished || !_disposalX.SADDate.IsLatter(Properties.Settings.Default.BatchArchivalDelay))
            {
              _2archive = false;
              break;
            }
            ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
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
            ProgressChanged(null, new EntitiesChangedEventArgs(1, null, edc));
          }
        }
      }// foreach (Batch 
      edc.SubmitChanges();
      ProgressChanged(null, new EntitiesChangedEventArgs(1, String.Format("Archived {0} Batch final, Batch progress {1} and {2} Material entries.", _batchArchived, _progressBatchArchived, _materialArchived), edc));
    }
    #endregion

  }
}
