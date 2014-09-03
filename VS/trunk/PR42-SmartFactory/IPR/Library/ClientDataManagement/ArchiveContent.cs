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

using CAS.SharePoint.Client.Link2SQL;
using CAS.SharePoint.Client.Linq2SP;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
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
    }
    /// <summary>
    /// Performs Archive Content Task.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="ProgressChanged">The progress changed.</param>
    public static void Go(ArchiveSettings settings, Action<object, ProgressChangedEventArgs> ProgressChanged)
    {
      using (NSLinq2SQL.IPRDEV _sqledc = NSLinq2SQL.IPRDEV.Connect2SQL(settings.ConnectionString, ProgressChanged))
      {
        if (String.IsNullOrEmpty(settings.ConnectionString))
          throw new ArgumentNullException("Database connection string cannot be null or empty.");
        if (!_sqledc.DatabaseExists())
          throw new ArgumentOutOfRangeException(String.Format("The database at {0} does nor exist.", settings.ConnectionString));
        using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
        {
          GoIPR(_spedc, _sqledc, settings, ProgressChanged);
          GoBatch(_spedc, _sqledc, settings, ProgressChanged);
        }
      }
    }
    #endregion

    #region private
    private static void GoIPR(NSSPLinq.Entities spedc, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting IPR archive-{0} delay. It could take several minutes", settings.ArchiveIPRDelay)));
      //Select delete candidates.
      progress(null, new ProgressChangedEventArgs(1, "Buffering Disposal and IPR entries"));
      List<NSSPLinq.Disposal> _dspsls = spedc.Disposal.ToList<NSSPLinq.Disposal>();
      foreach (Linq.IPR _iprX in spedc.IPR)
      {
        if (_iprX.AccountClosed.Value == true && _iprX.ClosingDate.IsLatter(settings.ArchiveIPRDelay))
        {
          bool _any = false;
          foreach (NSSPLinq.Disposal _dspx in _iprX.Disposal)
            if (_dspx.Disposal2BatchIndex != null && (_dspx.Disposal2BatchIndex.FGQuantityAvailable.Value != 0 || _dspx.Disposal2BatchIndex.BatchStatus.Value != NSSPLinq.BatchStatus.Final))
            {
              _any = true;
              break;
            }
          if (_any)
            continue;
          List<NSSPLinq.IPR> _toDeleteIPR = new List<NSSPLinq.IPR>();
          List<IArchival> _toBeMarkedArchival4IPR = new List<IArchival>();
          List<Linq.Disposal> _toDeletedDisposal = new List<NSSPLinq.Disposal>();
          List<IArchival> _toBeMarkedArchival4Disposal = new List<IArchival>();
          //_toBeMarkedArchival.Add(_iprX.IPR2ConsentTitle);
          //_toBeMarkedArchival.Add(_iprX.IPR2JSOXIndex);
          //_toBeMarkedArchival.Add(_iprX.IPR2PCNPCN);
          _toBeMarkedArchival4IPR.AddIfNotNull(_iprX.ClearenceIndex);
          //_toBeMarkedArchival.Add(_iprX.IPRLibraryIndex);
          foreach (NSSPLinq.Disposal _dspslx in _iprX.Disposal)
          {
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2BatchIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2ClearenceIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2InvoiceContentIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2IPRIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2MaterialIndex);
            //_toBeMarkedArchival.Add(_dspslx.Disposal2PCNID);
            //_toBeMarkedArchival.Add(_dspslx.JSOXCustomsSummaryIndex);
            _toDeletedDisposal.AddIfNotNull(_dspslx);
          }
          _toDeleteIPR.Add(_iprX);
          progress(null, new ProgressChangedEventArgs(1, String.Format("Selected {0} IPR account with {1} disposal entries to be deleted.", _iprX.Title, _toDeletedDisposal.Count)));
          spedc.Disposal.Delete<NSSPLinq.Disposal, NSLinq2SQL.History>
            (_toDeletedDisposal, _toBeMarkedArchival4Disposal, x => sqledc.Disposal.GetAt<NSLinq2SQL.Disposal>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          spedc.IPR.Delete<NSSPLinq.IPR, NSLinq2SQL.History>
            (_toDeleteIPR, _toBeMarkedArchival4IPR, x => sqledc.IPR.GetAt<NSLinq2SQL.IPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
        }
      }
      //Update Activities Log
      Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Archiving, progress);
      progress(null, new ProgressChangedEventArgs(1, "Finished Archive IPR"));
    }
    private static void GoBatch(NSSPLinq.Entities _spedc, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting Batch archive - {0} delay. It could take several minutes", settings.ArchiveBatchDelay)));
      //TODO progress cig exclude if final or intermediate exist
      //TODO progress arch if there is new 
      //TODO cuttfiler AD ??
      progress(null, new ProgressChangedEventArgs(1, "Buffering Material entries"));
      List<NSSPLinq.Material> _mtrl = _spedc.Material.ToList<NSSPLinq.Material>();
      int _batchArchived = 0;
      int _progressBatchArchived = 0;
      int _materialArchived = 0;
      IEnumerable<NSSPLinq.Batch> _allB = _spedc.Batch.ToList<NSSPLinq.Batch>().Where<NSSPLinq.Batch>(x => x.ProductType.Value == NSSPLinq.ProductType.Cigarette);
      Dictionary<string, IGrouping<string, NSSPLinq.Batch>> _progressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
      List<NSSPLinq.Batch> _noProgressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Final || _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Intermediate select _fbx).ToList<NSSPLinq.Batch>();
      string _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries", _progressBatch.Count, _noProgressBatch.Count);
      progress(null, new ProgressChangedEventArgs(1, _msg));
      foreach (NSSPLinq.Batch _batchX in _noProgressBatch)
      {
        if (_progressBatch.Keys.Contains(_batchX.Batch0))
        {
          Debug.Assert(_batchX.BatchStatus.Value != NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be != BatchStatus.Progress");
          foreach (NSSPLinq.Batch _bpx in _progressBatch[_batchX.Batch0])
          {
            Debug.Assert(_bpx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be == BatchStatus.Progress");
            progress(null, new ProgressChangedEventArgs(1, null));
            _bpx.Archival = true;
            _progressBatchArchived++;
            Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
            foreach (NSSPLinq.Material _mpx in _bpx.Material)
            {
              Debug.Assert(_mpx.Material2BatchIndex == _bpx, "Wrong Material to Batch link");
              progress(null, new ProgressChangedEventArgs(1, null));
              _materialArchived++;
              _mpx.Archival = true;
            }
          }
        } //foreach (Batch _batchX
        progress(null, new ProgressChangedEventArgs(1, null));
        if (_batchX.ProductType.Value != NSSPLinq.ProductType.Cigarette || _batchX.FGQuantityAvailable > 0)
          continue;
        bool _2archive = true;
        foreach (NSSPLinq.Material _material in _batchX.Material)
        {
          progress(null, new EntitiesChangedEventArgs(1, null, _spedc));
          foreach (NSSPLinq.Disposal _disposalX in _material.Disposal)
          {
            if (_disposalX.CustomsStatus.Value != NSSPLinq.CustomsStatus.Finished || !_disposalX.SADDate.IsLatter(settings.ArchiveBatchDelay))
            {
              _2archive = false;
              break;
            }
            progress(null, new EntitiesChangedEventArgs(1, null, _spedc));
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
            progress(null, new EntitiesChangedEventArgs(1, null, _spedc));
          }
          Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
        }
      }// foreach (Batch 
      Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
      progress(null, new EntitiesChangedEventArgs(1, String.Format("Finished Archive.GoBatch; Archived {0} Batch final, Batch progress {1} and {2} Material entries.", _batchArchived, _progressBatchArchived, _materialArchived), _spedc));
    }
    #endregion

  }
}
