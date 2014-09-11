//<summary>
//  Title   : class SynchronizationContent
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

using CAS.SharePoint.Client.Link2SQL;
using CAS.SharePoint.Client.Linq2SP;
using CAS.SharePoint.Client.Reflection;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement
{
  /// <summary>
  /// Helpers to synchronize SQL content with the SharePoint website
  /// </summary>
  public static class SynchronizationContent
  {
    /// <summary>
    /// Represents synchronization settings
    /// </summary>
    public struct SynchronizationSettings
    {
      /// <summary>
      /// The site URL
      /// </summary>
      public string SiteURL;
      /// <summary>
      /// The connection string
      /// </summary>
      public string ConnectionString;
      /// <summary>
      /// the maximal number of items queried form the ShsrePoint.
      /// </summary>
      public int RowLimit;
      /// <summary>
      /// Indication if porting to new Rel 2.10 i required.
      /// </summary>
      public bool Port2Rel210;
    }
    /// <summary>
    /// Realize the synchronization operation..
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void Go(SynchronizationSettings settings, ProgressChangedEventHandler progressChanged)
    {
      IPRDEV _sqledc = IPRDEV.Connect2SQL(settings.ConnectionString, progressChanged);
      using (Entities _spedc = new Entities(settings.SiteURL))
      {
        _spedc.RowLimit = settings.RowLimit;
        Synchronize(_sqledc.JSOXLibrary, _spedc.JSOXLibrary, progressChanged, JSOXLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.BalanceBatch, _spedc.BalanceBatch, progressChanged, Linq.BalanceBatch.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADDocumentLibrary, _spedc.SADDocumentLibrary, progressChanged, Linq.SADDocumentLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADDocument, _spedc.SADDocument, progressChanged, Linq.SADDocumentType.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADGood, _spedc.SADGood, progressChanged, Linq.SADGood.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADConsignment, _spedc.SADConsignment, progressChanged, Linq.SADConsignment.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Clearence, _spedc.Clearence, progressChanged, Linq.Clearence.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Consent, _spedc.Consent, progressChanged, Linq.Consent.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.PCNCode, _spedc.PCNCode, progressChanged, Linq.PCNCode.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.IPRLibrary, _spedc.IPRLibrary, progressChanged, Linq.IPRLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.IPR, _spedc.IPR, progressChanged, Linq.IPR.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.BalanceIPR, _spedc.BalanceIPR, progressChanged, Linq.BalanceIPR.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.BatchLibrary, _spedc.BatchLibrary, progressChanged, Linq.BatchLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SPFormat, _spedc.Format, progressChanged, Linq.Format.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SKULibrary, _spedc.SKULibrary, progressChanged, Linq.Document.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SKU, _spedc.SKU, progressChanged, Linq.SKUCommonPart.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Batch, _spedc.Batch, progressChanged, Linq.Batch.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.CustomsUnion, _spedc.CustomsUnion, progressChanged, Linq.CustomsUnion.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.CutfillerCoefficient, _spedc.CutfillerCoefficient, progressChanged, Linq.CutfillerCoefficient.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.InvoiceLibrary, _spedc.InvoiceLibrary, progressChanged, Linq.InvoiceLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.InvoiceContent, _spedc.InvoiceContent, progressChanged, Linq.InvoiceContent.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Material, _spedc.Material, progressChanged, Linq.Material.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.JSOXCustomsSummary, _spedc.JSOXCustomsSummary, progressChanged, Linq.JSOXCustomsSummary.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Disposal, _spedc.Disposal, progressChanged, Linq.Disposal.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Dust, _spedc.Dust, progressChanged, Linq.Dust.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADDuties, _spedc.SADDuties, progressChanged, Linq.SADDuties.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADPackage, _spedc.SADPackage, progressChanged, Linq.SADPackage.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADQuantity, _spedc.SADQuantity, progressChanged, Linq.SADQuantity.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SADRequiredDocuments, _spedc.SADRequiredDocuments, progressChanged, Linq.SADRequiredDocuments.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Settings, _spedc.Settings, progressChanged, Linq.Settings.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.SHMenthol, _spedc.SHMenthol, progressChanged, Linq.SHMenthol.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.StockLibrary, _spedc.StockLibrary, progressChanged, Linq.StockLib.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.StockEntry, _spedc.StockEntry, progressChanged, Linq.StockEntry.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Usage, _spedc.Usage, progressChanged, Linq.Usage.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Warehouse, _spedc.Warehouse, progressChanged, Linq.Warehouse.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.Waste, _spedc.Waste, progressChanged, Linq.Waste.GetMappings(), settings.Port2Rel210);
        Synchronize(_sqledc.ActivityLog, _spedc.ActivityLog, progressChanged, Linq.ActivityLogCT.GetMappings(), settings.Port2Rel210);
      }
      if (settings.Port2Rel210)
        Linq.Settings.SaveCurrentContentVersion(settings.SiteURL, progressChanged);
      Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Synchronization, progressChanged);
      progressChanged(1, new ProgressChangedEventArgs(1, "SynchronizationContent has been finished"));
    }
    private static void Synchronize<TSQL, TSP>(Table<TSQL> sqlTable, EntityList<TSP> spList, ProgressChangedEventHandler progressChanged, Dictionary<string, string> mapping, bool port2210)
      where TSQL : class, SharePoint.Client.Link2SQL.IItem, INotifyPropertyChanged, new()
      where TSP : Linq.Item, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SharePoint website.", typeof(TSP).Name)));
      List<TSP> _scrList = spList.ToList<TSP>();
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SQL database.", typeof(TSQL).Name)));
      Dictionary<int, IItem> _dictinary = sqlTable.Where<TSQL>(x => !x.OnlySQL).ToDictionary<TSQL, int, IItem>(x => x.ID, y => (SharePoint.Client.Link2SQL.IItem)y);
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Synchronization {0} elements in the SharePoint source tables with the {1} element in the SQL table.", _scrList.Count, _dictinary.Count)));
      //create descriptors using reflection
      Dictionary<string, StorageItemsList> _spDscrpt = StorageItem.CreateStorageDescription(typeof(TSP));
      Dictionary<string, SQLStorageItem> _sqlDscrpt = SQLStorageItem.CreateStorageDescription(typeof(TSQL), mapping);
      int _counter = 0;
      int _itemChanges = 0;
      bool _itemChanged;
      Action<TSP> _port = x => { };
      List<string> _changes = new List<string>();
      if (port2210 && typeof(TSP) is IArchival)
      {
        _port = x => ((IArchival)x).Archival = false;
        progressChanged(1, new ProgressChangedEventArgs(1, "The table will be updated new software version"));
      }
      foreach (TSP _spItem in _scrList)
      {
        _port(_spItem);
        SharePoint.Client.Link2SQL.IItem _sqlItem = default(SharePoint.Client.Link2SQL.IItem);
        if (!_dictinary.TryGetValue(_spItem.Id.Value, out _sqlItem))
        {
          _sqlItem = new TSQL();
          _sqlItem.OnlySQL = false;
          _dictinary.Add(_spItem.Id.Value, _sqlItem);
          sqlTable.InsertOnSubmit((TSQL)_sqlItem);
        }
        Microsoft.SharePoint.Linq.ContentTypeAttribute _attr = _spItem.GetType().GetContentType();
        _itemChanged = false;
        Synchronize<TSQL, TSP>(
          (TSQL)_sqlItem,
          _spItem, _spDscrpt[_attr.Id],
          _sqlDscrpt, progressChanged,
          spList.DataContext,
          (x, y) =>
          {
            _counter++;
            _changes.Add(y.PropertyName);
            _itemChanged = true;
          });
        if (_itemChanged)
          _itemChanges++;
      }
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Submitting {0} changes on {1} list rows to SQL database", _counter, _itemChanges)));
      sqlTable.Context.SubmitChanges();
    }
    private static void Synchronize<TSQL, TSP>
      (
          TSQL sqlItem,
          TSP splItem,
          List<StorageItem> _spDscrpt,
          Dictionary<string, SQLStorageItem> _sqlDscrpt,
          ProgressChangedEventHandler progressChanged,
          Microsoft.SharePoint.Linq.DataContext dataContext,
          PropertyChangedEventHandler propertyChanged
      )
        where TSQL : class, SharePoint.Client.Link2SQL.IItem, INotifyPropertyChanged, new()
    {
      sqlItem.PropertyChanged += propertyChanged;
      foreach (StorageItem _si in _spDscrpt.Where<StorageItem>(x => !x.IsReverseLookup()))
        if (_sqlDscrpt.ContainsKey(_si.PropertyName))
          _si.GetValueFromEntity(splItem, x => _sqlDscrpt[_si.PropertyName].Assign(x, sqlItem), dataContext);
      sqlItem.PropertyChanged -= propertyChanged;
    }
  }
}
