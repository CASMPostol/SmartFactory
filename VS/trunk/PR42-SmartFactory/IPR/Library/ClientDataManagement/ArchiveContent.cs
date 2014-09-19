﻿//<summary>
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
      /// <summary>
      /// Gets or sets the archival delay of reports.
      /// </summary>
      /// <value>
      /// The reports archival delay.
      /// </value>
      public int ReportsArchivalDelay { get; set; }
    }
    /// <summary>
    /// Performs Archive Content Task.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void Go(ArchiveSettings settings, ProgressChangedEventHandler progressChanged)
    {
      using (NSLinq2SQL.IPRDEV _sqledc = NSLinq2SQL.IPRDEV.Connect2SQL(settings.ConnectionString, progressChanged))
      {
        if (String.IsNullOrEmpty(settings.ConnectionString))
          throw new ArgumentNullException("Database connection string cannot be null or empty.");
        if (!_sqledc.DatabaseExists())
          throw new ArgumentOutOfRangeException(String.Format("The database at {0} does nor exist.", settings.ConnectionString));
        using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
        {
          GoIPR(_spedc, _sqledc, settings, progressChanged);
          GoReports(_spedc, _sqledc, settings, progressChanged);
          GoBatch(_spedc, _sqledc, settings, progressChanged);
        }
        Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Archiving, progressChanged);
      }
      progressChanged(null, new ProgressChangedEventArgs(1, "Finished archiving all lists"));
    }
    #endregion

    #region private
    private static void GoIPR(NSSPLinq.Entities spedc, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
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
          List<NSSPLinq.BalanceIPR> _toDeletedBalanceIPR = new List<NSSPLinq.BalanceIPR>();
          List<IArchival> _toBeMarkedArchival4BalanceIPR = new List<IArchival>();
          List<NSSPLinq.Material> _toDeletedMaterial = new List<NSSPLinq.Material>();
          List<IArchival> _toBeMarkedArchival4Material = new List<IArchival>();
          List<NSSPLinq.SADDocumentType> _toDeletedSADDocumentType = new List<NSSPLinq.SADDocumentType>();
          List<NSSPLinq.SADGood> _toDeletedSADGood = new List<NSSPLinq.SADGood>();
          List<NSSPLinq.SADRequiredDocuments> _toDeletedSADRequiredDocuments = new List<NSSPLinq.SADRequiredDocuments>();
          List<NSSPLinq.SADQuantity> _toDeletedSADQuantity = new List<NSSPLinq.SADQuantity>();
          List<NSSPLinq.SADPackage> _toDeletedSADPackage = new List<NSSPLinq.SADPackage>();
          List<NSSPLinq.SADDuties> _toDeletedSADDuties = new List<NSSPLinq.SADDuties>();
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
            if (_dspslx.Disposal2MaterialIndex != null && _dspslx.Disposal2MaterialIndex.Disposal.Count == 1)
            {
              _toBeMarkedArchival4Material.AddIfNotNull(_dspslx.Disposal2MaterialIndex.Material2BatchIndex);
              _toDeletedMaterial.Add(_dspslx.Disposal2MaterialIndex);
            }
            if (_dspslx.Disposal2InvoiceContentIndex != null && _dspslx.Disposal2InvoiceContentIndex.Disposal.Count == 1 )
            {
              NSSPLinq.InvoiceContent _invCnt = _dspslx.Disposal2InvoiceContentIndex;
              if (_invCnt.InvoiceIndex.InvoiceContent.Count == 1 )
              {
                NSSPLinq.InvoiceLib _invcLb = _invCnt.InvoiceIndex;
                bool _lstInvce = spedc.InvoiceLibrary.ToList<NSSPLinq.InvoiceLib>().Where<NSSPLinq.InvoiceLib>(x => x.ClearenceIndex == _invcLb.ClearenceIndex ).Count<NSSPLinq.InvoiceLib>() == 1;
                if (_lstInvce)
                {
                  NSSPLinq.Clearence _clrnc = _invcLb.ClearenceIndex;
                  if (_clrnc.Clearence2SadGoodID.Clearence.Count == 1 )
                  {
                    NSSPLinq.SADGood _SDGd = _clrnc.Clearence2SadGoodID;
                    if (_SDGd.Clearence.Count == 1)
                    {
                      _toDeletedSADDocumentType.Add(_SDGd.SADDocumentIndex);
                      _toDeletedSADDuties.AddRange(_SDGd.SADDuties);
                      _toDeletedSADPackage.AddRange(_SDGd.SADPackage);
                      _toDeletedSADQuantity.AddRange(_SDGd.SADQuantity);
                      _toDeletedSADGood.Add(_SDGd);
                    }
                  }
                }

              }
              
            }
          }
          foreach (NSSPLinq.BalanceIPR _biprx in _iprX.BalanceIPR)
          {
            //_toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.IPRIndex);
            //_toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.BalanceIPR2JSOXIndex);
            _toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.BalanceBatchIndex);
            _toDeletedBalanceIPR.Add(_biprx);
          }
          _toDeleteIPR.Add(_iprX);
          string _mtmp = "Selected {0} IPR account with {1} Disposal and {2} BalanceIPR entries to be deleted.";
          progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _iprX.Title, _toDeletedDisposal.Count, _toDeletedBalanceIPR.Count)));
          //delete BalanceIPR
          spedc.BalanceIPR.Delete<NSSPLinq.BalanceIPR, NSLinq2SQL.History>
            (_toDeletedBalanceIPR, _toBeMarkedArchival4BalanceIPR, x => sqledc.BalanceIPR.GetAt<NSLinq2SQL.BalanceIPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          //delete Disposal entries
          spedc.Disposal.Delete<NSSPLinq.Disposal, NSLinq2SQL.History>
            (_toDeletedDisposal, _toBeMarkedArchival4Disposal, x => sqledc.Disposal.GetAt<NSLinq2SQL.Disposal>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          //delete IPR
          spedc.IPR.Delete<NSSPLinq.IPR, NSLinq2SQL.History>
            (_toDeleteIPR, _toBeMarkedArchival4IPR, x => sqledc.IPR.GetAt<NSLinq2SQL.IPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
          _mtmp = "Selected {0} Material entries to be deleted.";
          progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _iprX.Title, _toDeletedDisposal.Count, _toDeletedBalanceIPR.Count)));
          spedc.Material.Delete<NSSPLinq.Material, NSLinq2SQL.History>
            (_toDeletedMaterial, _toBeMarkedArchival4Material, x => sqledc.Material.GetAt<NSLinq2SQL.Material>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
        }
      }
      //Update Activities Log
      progress(null, new ProgressChangedEventArgs(1, "Finished Archive IPR"));
    }
    private static void GoReports(NSSPLinq.Entities spedc, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting Reports archive - {0} delay. It could take several minutes", settings.ReportsArchivalDelay)));
      progress(null, new ProgressChangedEventArgs(1, "Buffering JSOXLib and IPR entries"));
      List<NSSPLinq.JSOXLib> _2DeleteJSOXLib = spedc.JSOXLibrary.ToList<NSSPLinq.JSOXLib>().Where<NSSPLinq.JSOXLib>(x => x.SituationDate.IsLatter(settings.ReportsArchivalDelay)).ToList<NSSPLinq.JSOXLib>();
      progress(null, new ProgressChangedEventArgs(1, String.Format("There are {0} JSOXLib to be archived", _2DeleteJSOXLib.Count)));
      foreach (NSSPLinq.JSOXLib _jsoxlx in _2DeleteJSOXLib)
      {
        List<NSSPLinq.BalanceIPR> _2deleteBalanceIPR = new List<NSSPLinq.BalanceIPR>();
        List<IArchival> _2ArchivalBIPR = new List<IArchival>();
        bool _any = false;
        foreach (NSSPLinq.JSOXCustomsSummary _jcsx in _jsoxlx.JSOXCustomsSummary)
          if (_jcsx.Disposal.Count != 0)
          {
            _any = true;
            break;
          }
        if (_any)
          continue;
        //Start deleting procedure of this branch of lists
        //delete BalanceIPR
        foreach (NSSPLinq.BalanceIPR _birx in _jsoxlx.BalanceIPR)
        {
          _2ArchivalBIPR.AddIfNotNull(_birx.IPRIndex);
          _2deleteBalanceIPR.Add(_birx);
        }
        spedc.BalanceIPR.Delete<NSSPLinq.BalanceIPR, NSLinq2SQL.History>
          (_2deleteBalanceIPR, null, x => sqledc.BalanceIPR.GetAt<NSLinq2SQL.BalanceIPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        progress(null, new ProgressChangedEventArgs(1, "Finished Archive BalanceIPR"));
        Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
        //delete BalanceBatch
        spedc.BalanceBatch.Delete<NSSPLinq.BalanceBatch, NSLinq2SQL.History>
          (_jsoxlx.BalanceBatch, null, x => sqledc.BalanceBatch.GetAt<NSLinq2SQL.BalanceBatch>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
        //delete StockEntry
        List<NSSPLinq.StockEntry> _2deleteStockEntry = new List<NSSPLinq.StockEntry>();
        List<IArchival> _2ArchivalStockEntry = new List<IArchival>();
        foreach (NSSPLinq.StockLib _slx in _jsoxlx.StockLib)
        {
          _slx.Archival = true;
          foreach (NSSPLinq.StockEntry _sex in _slx.StockEntry)
          {
            //TODO to be investigated - remove comment if needed _2ArchivalStockEntry.AddIfNotNull(_sex.BatchIndex); 
            _2deleteStockEntry.Add(_sex);
          }
        }
        spedc.StockEntry.Delete<NSSPLinq.StockEntry, NSLinq2SQL.History>
          (_2deleteStockEntry, _2ArchivalStockEntry, x => sqledc.StockEntry.GetAt<NSLinq2SQL.StockEntry>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
        progress(null, new ProgressChangedEventArgs(1, "Finished Archive Reports"));
      }
    }
    private static void GoBatch(NSSPLinq.Entities spedc, NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting Batch archive - {0} delay. It could take several minutes", settings.ArchiveBatchDelay)));
      progress(null, new ProgressChangedEventArgs(1, "Buffering Material entries"));
      List<NSSPLinq.Material> _mtrl = spedc.Material.ToList<NSSPLinq.Material>();
      int _batchArchived = 0;
      int _progressBatchArchived = 0;
      int _materialArchived = 0;
      IEnumerable<NSSPLinq.Batch> _allB = spedc.Batch.ToList<NSSPLinq.Batch>().Where<NSSPLinq.Batch>(x => x.ProductType.Value == NSSPLinq.ProductType.Cigarette);
      Dictionary<string, IGrouping<string, NSSPLinq.Batch>> _progressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
      List<NSSPLinq.Batch> _noProgressBatch = (from _fbx in _allB where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Final || _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Intermediate select _fbx).ToList<NSSPLinq.Batch>();
      string _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries", _progressBatch.Count, _noProgressBatch.Count);
      progress(null, new ProgressChangedEventArgs(1, _msg));
      List<NSSPLinq.Batch> _2BeDeletedBatch = new List<NSSPLinq.Batch>();
      List<NSSPLinq.Material> _2BeDeletedMaterial = new List<NSSPLinq.Material>();
      foreach (NSSPLinq.Batch _batchX in _noProgressBatch)
      {
        //Deleting progress batches
        if (_progressBatch.Keys.Contains(_batchX.Batch0))
        {
          Debug.Assert(_batchX.BatchStatus.Value != NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be != BatchStatus.Progress");
          foreach (NSSPLinq.Batch _bpx in _progressBatch[_batchX.Batch0])
          {
            Debug.Assert(_bpx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress, "Wrong BatchStatus should be == BatchStatus.Progress");
            _2BeDeletedBatch.Add(_bpx);
            _progressBatchArchived++;
            foreach (NSSPLinq.Material _mpx in _bpx.Material)
            {
              Debug.Assert(_mpx.Material2BatchIndex == _bpx, "Wrong Material to Batch link");
              _materialArchived++;
              _2BeDeletedMaterial.Add(_mpx);
            }
          }
        } //foreach (Batch _batchX ....
        if ((_batchX.FGQuantityAvailable > 0) || (_batchX.BatchStatus.Value == NSSPLinq.BatchStatus.Intermediate))
          continue;
        Debug.Assert(_batchX.ProductType.Value == NSSPLinq.ProductType.Cigarette);
        bool _2archive = true;
        foreach (NSSPLinq.Material _material in _batchX.Material)
        {
          if (_material.Disposal.Count > 0)
          {
            _2archive = false;
            break;
          }
        } // foreach (Material
        if (_2archive)
        {
          _materialArchived += _batchX.Material.Count;
          _2BeDeletedMaterial.AddRange(_batchX.Material);
          if (!_batchX.Modified.IsLatter(settings.ArchiveBatchDelay) || _batchX.InvoiceContent.Count > 0 || _batchX.StockEntry.Count > 0 || _batchX.Disposal.Count > 0)
          {
            _batchArchived++;
            _2BeDeletedBatch.Add(_batchX);
          }
        }
      }// foreach (Batch 
      progress(null, new ProgressChangedEventArgs(1, String.Format("Archiving {0} Batch final, Batch progress {1} and {2} Material entries.", _batchArchived, _progressBatchArchived, _materialArchived)));
      spedc.Material.Delete<NSSPLinq.Material, NSLinq2SQL.History>
        (_2BeDeletedMaterial, null, x => sqledc.Material.GetAt<NSLinq2SQL.Material>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
      spedc.Batch.Delete<NSSPLinq.Batch, NSLinq2SQL.History>
        (_2BeDeletedBatch, null, x => sqledc.Batch.GetAt<NSLinq2SQL.Batch>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
          settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
      Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
      progress(null, new ProgressChangedEventArgs(1, "Finished archival Batch and Material list"));
    }

    #endregion

  }
}
