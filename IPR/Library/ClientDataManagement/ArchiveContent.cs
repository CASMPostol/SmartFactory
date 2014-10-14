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
        GoIPR(_sqledc, settings, progressChanged);
        GoReports(_sqledc, settings, progressChanged);
        GoBatch(_sqledc, settings, progressChanged);
        Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Archiving, progressChanged);
      }
      progressChanged(null, new ProgressChangedEventArgs(1, "Finished archiving all lists"));
    }
    #endregion

    #region private
    private static void GoIPR(NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting IPR archive-{0} delay. It could take several minutes", settings.ArchiveIPRDelay)));
      //Select delete candidates.
      List<int> _invcLib2BeChecked = new List<int>();
      List<int> _clearance2BeChecked = new List<int>();
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        progress(null, new ProgressChangedEventArgs(1, "Buffering IPR entries"));
        foreach (Linq.IPR _iprX in _spedc.IPR.ToList<Linq.IPR>().Where<Linq.IPR>(_iprX => _iprX.AccountClosed.Value == true && _iprX.ClosingDate.IsLatter(settings.ArchiveIPRDelay)))
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
          _toBeMarkedArchival4IPR.AddIfNotNull(_iprX.ClearenceIndex);
          foreach (NSSPLinq.Disposal _dspslx in _iprX.Disposal)
          {
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2BatchIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2ClearenceIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2InvoiceContentIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2IPRIndex);
            _toBeMarkedArchival4Disposal.AddIfNotNull(_dspslx.Disposal2MaterialIndex);
            //_toBeMarkedArchival.Add(_dspslx.Disposal2PCNID);
            //_toBeMarkedArchival.Add(_dspslx.JSOXCustomsSummaryIndex);
            _toDeletedDisposal.Add(_dspslx);
            if (_dspslx.Disposal2InvoiceContentIndex != null)
              _invcLib2BeChecked.AddIfNew(_dspslx.Disposal2InvoiceContentIndex.InvoiceIndex.Id.Value);
            if (_dspslx.Disposal2ClearenceIndex != null)
              _clearance2BeChecked.AddIfNew(_dspslx.Disposal2ClearenceIndex.Id.Value);
          }
          foreach (NSSPLinq.BalanceIPR _biprx in _iprX.BalanceIPR)
          {
            //_toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.IPRIndex);
            //_toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.BalanceIPR2JSOXIndex);
            _toBeMarkedArchival4BalanceIPR.AddIfNotNull(_biprx.BalanceBatchIndex);
            _toDeletedBalanceIPR.Add(_biprx);
          }
          _toDeleteIPR.Add(_iprX);
          string _mtmp = "The selected {0} IPR account with {1} Disposal and {2} BalanceIPR entries are to be deleted.";
          progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _iprX.Title, _toDeletedDisposal.Count, _toDeletedBalanceIPR.Count)));
          //delete BalanceIPR
          _spedc.BalanceIPR.Delete<NSSPLinq.BalanceIPR, NSLinq2SQL.History>
            (_toDeletedBalanceIPR, _toBeMarkedArchival4BalanceIPR, x => sqledc.BalanceIPR.GetAt<NSLinq2SQL.BalanceIPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          //delete Disposal entries
          _spedc.Disposal.Delete<NSSPLinq.Disposal, NSLinq2SQL.History>
            (_toDeletedDisposal, _toBeMarkedArchival4Disposal, x => sqledc.Disposal.GetAt<NSLinq2SQL.Disposal>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          //delete IPR
          _spedc.IPR.Delete<NSSPLinq.IPR, NSLinq2SQL.History>
            (_toDeleteIPR, _toBeMarkedArchival4IPR, x => sqledc.IPR.GetAt<NSLinq2SQL.IPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
        }
      }
      //delete InvoiceLib and InvoiceContent
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.InvoiceContent> _toDeletedInvoiceContent = new List<NSSPLinq.InvoiceContent>();
        List<NSSPLinq.InvoiceLib> _toDeletedInvoiceLib = new List<NSSPLinq.InvoiceLib>();
        Dictionary<int, NSSPLinq.InvoiceLib> _allInvcLib = _spedc.InvoiceLibrary.ToDictionary<NSSPLinq.InvoiceLib, int>(x => x.Id.Value);
        int _brokenCleranceLookup = 0;
        foreach (int _ilx in _invcLib2BeChecked)
        {
          NSSPLinq.InvoiceLib _invcLb = _allInvcLib[_ilx];
          bool _any = false;
          foreach (NSSPLinq.InvoiceContent _icx in _invcLb.InvoiceContent)
          {
            _any = _icx.Disposal.Count > 0;
            if (_any)
              break;
          }
          if (_any)
            continue;
          _toDeletedInvoiceLib.Add(_invcLb);
          _toDeletedInvoiceContent.AddRange(_invcLb.InvoiceContent);
          if (_invcLb.ClearenceIndex == null)
            _brokenCleranceLookup++;
          else
            _clearance2BeChecked.AddIfNew(_invcLb.ClearenceIndex.Id.Value);
        }
        if (_brokenCleranceLookup != 0)
          progress(null, new ProgressChangedEventArgs(1, String.Format("Warning: there are {0} InvoiceLib entries with broken lookup on Clearance", _brokenCleranceLookup)));
        string _mtmp = "The selected: {0} InvoiceLibrary, and {1} InvoiceContent entries are to be deleted.";
        progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _toDeletedInvoiceLib.Count, _toDeletedInvoiceContent.Count)));
        _spedc.InvoiceContent.Delete<NSSPLinq.InvoiceContent, NSLinq2SQL.History>
          (_toDeletedInvoiceContent, null, x => sqledc.InvoiceContent.GetAt<NSLinq2SQL.InvoiceContent>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _spedc.InvoiceLibrary.Delete<NSSPLinq.InvoiceLib, NSLinq2SQL.History>
          (_toDeletedInvoiceLib, null, x => sqledc.InvoiceLibrary.GetAt<NSLinq2SQL.InvoiceLibrary>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _invcLib2BeChecked = null;
        Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
      }
      //Clearance
      List<int> _sad2BeChecked = new List<int>();
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.Clearence> _toDeletedClearence = new List<NSSPLinq.Clearence>();
        Dictionary<int, NSSPLinq.Clearence> _allClearences = _spedc.Clearence.ToDictionary<NSSPLinq.Clearence, int>(x => x.Id.Value);
        List<IArchival> _toBeMarkedArchival4Clearence = new List<IArchival>();
        List<NSSPLinq.InvoiceLib> _allInvcs = _spedc.InvoiceLibrary.ToList<NSSPLinq.InvoiceLib>();
        List<NSSPLinq.IPR> _allIPR = _spedc.IPR.ToList<NSSPLinq.IPR>();
        foreach (int _clrncIdx in _clearance2BeChecked)
        {
          NSSPLinq.Clearence _clrnc = _allClearences[_clrncIdx];
          if (_clrnc.Disposal.Count == 0 &&
              !_allInvcs.Where<NSSPLinq.InvoiceLib>(x => x.ClearenceIndex == _clrnc).Any<NSSPLinq.InvoiceLib>() &&
              !_allIPR.Where<NSSPLinq.IPR>(x => x.ClearenceIndex == _clrnc).Any<NSSPLinq.IPR>())
            _toDeletedClearence.Add(_clrnc);
          //Debug.Assert(_clrnc.Clearence2SadGoodID != null); - Disposal with 0 belongs may be assigned to closed account but not cleared through custom. 
          if (_clrnc.Clearence2SadGoodID == null)
            continue;
          _toBeMarkedArchival4Clearence.AddIfNotNull(_clrnc.Clearence2SadGoodID);
          _sad2BeChecked.AddIfNew(_clrnc.Clearence2SadGoodID.Id.Value);
        }
        _clearance2BeChecked = null;
        string _mtmp = "The selected {0} Clearance entries are to be deleted.";
        progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _toDeletedClearence.Count)));
        _spedc.Clearence.Delete<NSSPLinq.Clearence, NSLinq2SQL.History>
          (_toDeletedClearence, _toBeMarkedArchival4Clearence, x => sqledc.Clearence.GetAt<NSLinq2SQL.Clearence>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
      }
      List<int> _sadDocument2BeChecked = new List<int>();
      //SADDocument
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.SADGood> _toDeletedSADGood = new List<NSSPLinq.SADGood>();
        List<NSSPLinq.SADRequiredDocuments> _toDeletedSADRequiredDocuments = new List<NSSPLinq.SADRequiredDocuments>();
        List<NSSPLinq.SADQuantity> _toDeletedSADQuantity = new List<NSSPLinq.SADQuantity>();
        List<NSSPLinq.SADPackage> _toDeletedSADPackage = new List<NSSPLinq.SADPackage>();
        List<NSSPLinq.SADDuties> _toDeletedSADDuties = new List<NSSPLinq.SADDuties>();
        Dictionary<int, NSSPLinq.SADGood> _allSDGd = _spedc.SADGood.ToDictionary<NSSPLinq.SADGood, int>(x => x.Id.Value);
        foreach (int _sgid in _sad2BeChecked)
        {
          NSSPLinq.SADGood _SDGd = _allSDGd[_sgid];
          if (_SDGd.Clearence.Count > 0)
            continue;
          _sadDocument2BeChecked.AddIfNew(_SDGd.SADDocumentIndex.Id.Value);
          _toDeletedSADGood.Add(_SDGd);
          _toDeletedSADRequiredDocuments.AddRange(_SDGd.SADRequiredDocuments);
          _toDeletedSADDuties.AddRange(_SDGd.SADDuties);
          _toDeletedSADPackage.AddRange(_SDGd.SADPackage);
          _toDeletedSADQuantity.AddRange(_SDGd.SADQuantity);
        }
        string _mtmp = "The selected: {0} SADGood, {1} SADRequiredDocuments, {2} SADQuantity, {3} SADPackage, and {4} SADDuties entries are to be deleted.";
        progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _toDeletedSADGood.Count, _toDeletedSADRequiredDocuments.Count, _toDeletedSADQuantity.Count,
                                                    _toDeletedSADPackage.Count, _toDeletedSADDuties.Count)));
        _spedc.SADDuties.Delete<NSSPLinq.SADDuties, NSLinq2SQL.History>
          (_toDeletedSADDuties, null, x => sqledc.SADDuties.GetAt<NSLinq2SQL.SADDuties>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
           settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _spedc.SADPackage.Delete<NSSPLinq.SADPackage, NSLinq2SQL.History>
          (_toDeletedSADPackage, null, x => sqledc.SADPackage.GetAt<NSLinq2SQL.SADPackage>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
           settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _spedc.SADQuantity.Delete<NSSPLinq.SADQuantity, NSLinq2SQL.History>
          (_toDeletedSADQuantity, null, x => sqledc.SADQuantity.GetAt<NSLinq2SQL.SADQuantity>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
           settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _spedc.SADRequiredDocuments.Delete<NSSPLinq.SADRequiredDocuments, NSLinq2SQL.History>
          (_toDeletedSADRequiredDocuments, null, x => sqledc.SADRequiredDocuments.GetAt<NSLinq2SQL.SADRequiredDocuments>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
           settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        _spedc.SADGood.Delete<NSSPLinq.SADGood, NSLinq2SQL.History>
          (_toDeletedSADGood, null, x => sqledc.SADGood.GetAt<NSLinq2SQL.SADGood>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
           settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
        _sad2BeChecked = null;
      }
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.SADDocumentType> _toDeletedSADDocumentType = new List<NSSPLinq.SADDocumentType>();
        Dictionary<int, NSSPLinq.SADDocumentType> _allSDGd = _spedc.SADDocument.ToDictionary<NSSPLinq.SADDocumentType, int>(x => x.Id.Value);
        foreach (int _sdid in _sadDocument2BeChecked)
        {
          NSSPLinq.SADDocumentType _SDcumnt = _allSDGd[_sdid];
          if (_SDcumnt.SADGood.Count > 0)
            continue;
          _toDeletedSADDocumentType.AddIfNotNull<NSSPLinq.SADDocumentType>(_SDcumnt);
        }
        string _mtmp = "The selected: {0} SADDocument entries are to be deleted.";
        progress(null, new ProgressChangedEventArgs(1, String.Format(_mtmp, _toDeletedSADDocumentType.Count)));
        _spedc.SADDocument.Delete<NSSPLinq.SADDocumentType, NSLinq2SQL.History>
          (_toDeletedSADDocumentType, null, x => sqledc.SADDocument.GetAt<NSLinq2SQL.SADDocument>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
      }
      //Update Activities Log
      progress(null, new ProgressChangedEventArgs(1, "Finished archive operation of IPR related lists"));
    }
    private static void GoReports(NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting Reports archive - {0} delay. It could take several minutes", settings.ReportsArchivalDelay)));
      progress(null, new ProgressChangedEventArgs(1, "Buffering JSOXLib and IPR entries"));
      using (NSSPLinq.Entities _spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.JSOXLib> _2DeleteJSOXLib = _spedc.JSOXLibrary.ToList<NSSPLinq.JSOXLib>().Where<NSSPLinq.JSOXLib>(x => x.SituationDate.IsLatter(settings.ReportsArchivalDelay)).ToList<NSSPLinq.JSOXLib>();
        foreach (NSSPLinq.JSOXLib _jsoxlx in _2DeleteJSOXLib)
        {
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
          //delete StockEntry
          List<NSSPLinq.StockEntry> _2deleteStockEntry = new List<NSSPLinq.StockEntry>();
          foreach (NSSPLinq.StockLib _slx in _jsoxlx.StockLib)
            _2deleteStockEntry.AddRange(_slx.StockEntry);
          if (_jsoxlx.BalanceIPR.Count + _jsoxlx.BalanceBatch.Count + _jsoxlx.JSOXCustomsSummary.Count + _2deleteStockEntry.Count == 0)
            continue;
          String _mstTmp = "The selected {0} JSOXLib report related list are to be deleted: {1} BalanceIPR, {2} BalanceBatch, {3} JSOXCustomsSummary, and {4} StockEntry  entries are to be deleted.";
          progress(null, new ProgressChangedEventArgs(1, String.Format(_mstTmp, _jsoxlx.Title, _jsoxlx.BalanceIPR.Count, _jsoxlx.BalanceBatch.Count, _jsoxlx.JSOXCustomsSummary.Count, _2deleteStockEntry.Count)));
          _spedc.BalanceIPR.Delete<NSSPLinq.BalanceIPR, NSLinq2SQL.History>
            (_jsoxlx.BalanceIPR, null, x => sqledc.BalanceIPR.GetAt<NSLinq2SQL.BalanceIPR>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          _spedc.BalanceBatch.Delete<NSSPLinq.BalanceBatch, NSLinq2SQL.History>
            (_jsoxlx.BalanceBatch, null, x => sqledc.BalanceBatch.GetAt<NSLinq2SQL.BalanceBatch>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          _spedc.JSOXCustomsSummary.Delete<NSSPLinq.JSOXCustomsSummary, NSLinq2SQL.History>
            (_jsoxlx.JSOXCustomsSummary, null, x => sqledc.JSOXCustomsSummary.GetAt<NSLinq2SQL.JSOXCustomsSummary>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          _spedc.StockEntry.Delete<NSSPLinq.StockEntry, NSLinq2SQL.History>
            (_2deleteStockEntry, null, x => sqledc.StockEntry.GetAt<NSLinq2SQL.StockEntry>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
              settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
          Link2SQLExtensions.SubmitChanges(_spedc, sqledc, progress);
        }
      }
      progress(null, new ProgressChangedEventArgs(1, "Finished Archive Reports"));
    }
    private static void GoBatch(NSLinq2SQL.IPRDEV sqledc, ArchiveSettings settings, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, String.Format("Starting Batch archive - {0} delay. It could take several minutes", settings.ArchiveBatchDelay)));
      progress(null, new ProgressChangedEventArgs(1, "Buffering Material entries"));
      using (NSSPLinq.Entities spedc = new NSSPLinq.Entities(settings.SiteURL))
      {
        List<NSSPLinq.Material> _mtrl = spedc.Material.ToList<NSSPLinq.Material>();
        int _batchArchived = 0;
        int _progressBatchArchived = 0;
        int _materialArchived = 0;
        List<NSSPLinq.Batch> _allBtchs = spedc.Batch.ToList<NSSPLinq.Batch>();
        List<NSSPLinq.Batch> _2BeDeletedBatch = new List<NSSPLinq.Batch>();
        List<NSSPLinq.Material> _2BeDeletedMaterial = new List<NSSPLinq.Material>();
        foreach (NSSPLinq.Batch _noCig in _allBtchs.Where<NSSPLinq.Batch>(x => x.ProductType.Value != NSSPLinq.ProductType.Cigarette))
        {
          if (_noCig.StockEntry.Count > 0)
            continue;
          _2BeDeletedBatch.Add(_noCig);
          _2BeDeletedMaterial.AddRange(_noCig.Material);
        }
        string _msg = String.Format("There are {0} Batch and {1} Material entries for no cigarette to be deleted.", _2BeDeletedBatch.Count, _2BeDeletedMaterial.Count);
        progress(null, new ProgressChangedEventArgs(1, _msg));
        IEnumerable<NSSPLinq.Batch> _allBtchCig = _allBtchs.Where<NSSPLinq.Batch>(x => x.ProductType.Value == NSSPLinq.ProductType.Cigarette);
        Dictionary<string, IGrouping<string, NSSPLinq.Batch>> _progressBatch = (from _fbx in _allBtchCig where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Progress group _fbx by _fbx.Batch0).ToDictionary(x => x.Key);
        List<NSSPLinq.Batch> _noProgressBatch = (from _fbx in _allBtchCig where _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Final || _fbx.BatchStatus.Value == NSSPLinq.BatchStatus.Intermediate select _fbx).ToList<NSSPLinq.Batch>();
        _msg = String.Format("There are {0} Progress and {1} not Progress Batch entries for cigarette", _progressBatch.Count, _noProgressBatch.Count);
        progress(null, new ProgressChangedEventArgs(1, _msg));
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
            if (_batchX.Modified.IsLatter(settings.ArchiveBatchDelay) && _batchX.InvoiceContent.Count == 0 && _batchX.StockEntry.Count == 0 && _batchX.Disposal.Count == 0)
            {
              _batchArchived++;
              _2BeDeletedBatch.Add(_batchX);
            }
          }
        }// foreach (Batch 
        progress(null, new ProgressChangedEventArgs(1, String.Format("The selected {0} Batch final, {1} Batch progress and {2} Material entries are to be deleted.", _batchArchived, _progressBatchArchived, _materialArchived)));
        spedc.Material.Delete<NSSPLinq.Material, NSLinq2SQL.History>
          (_2BeDeletedMaterial, null, x => sqledc.Material.GetAt<NSLinq2SQL.Material>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        spedc.Batch.Delete<NSSPLinq.Batch, NSLinq2SQL.History>
          (_2BeDeletedBatch, null, x => sqledc.Batch.GetAt<NSLinq2SQL.Batch>(x), (id, listName) => sqledc.ArchivingLogs.AddLog(id, listName, Extensions.UserName()),
            settings.SiteURL, x => sqledc.History.AddHistoryEntry(x));
        Link2SQLExtensions.SubmitChanges(spedc, sqledc, progress);
      }
      progress(null, new ProgressChangedEventArgs(1, "Finished archival of the Batch and Material lists"));
    }
    #endregion

  }
}
