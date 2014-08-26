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
    }
    /// <summary>
    /// Realize the synchronization operation..
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void Go(SynchronizationSettings settings, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      IPRDEV _sqledc = IPRDEV.Connect2SQL(settings.ConnectionString, progressChanged);
      using (Entities _spedc = new Entities(settings.SiteURL))
      {
        Synchronize(_sqledc.JSOXLibrary, _spedc.JSOXLibrary, progressChanged, JSOXLib.GetMappings());
        Synchronize(_sqledc.BalanceBatch, _spedc.BalanceBatch, progressChanged, Linq.BalanceBatch.GetMappings());
        Synchronize(_sqledc.SADDocumentLibrary, _spedc.SADDocumentLibrary, progressChanged, Linq.SADDocumentLib.GetMappings());
        Synchronize(_sqledc.SADDocument, _spedc.SADDocument, progressChanged, Linq.SADDocumentType.GetMappings());
        Synchronize(_sqledc.SADGood, _spedc.SADGood, progressChanged, Linq.SADGood.GetMappings());
        Synchronize(_sqledc.SADConsignment, _spedc.SADConsignment, progressChanged, Linq.SADConsignment.GetMappings());
        Synchronize(_sqledc.Clearence, _spedc.Clearence, progressChanged, Linq.Clearence.GetMappings());
        Synchronize(_sqledc.Consent, _spedc.Consent, progressChanged, Linq.Consent.GetMappings());
        Synchronize(_sqledc.PCNCode, _spedc.PCNCode, progressChanged, Linq.PCNCode.GetMappings());
        Synchronize(_sqledc.IPRLibrary, _spedc.IPRLibrary, progressChanged, Linq.IPRLib.GetMappings());
        Synchronize(_sqledc.IPR, _spedc.IPR, progressChanged, Linq.IPR.GetMappings());
        Synchronize(_sqledc.BalanceIPR, _spedc.BalanceIPR, progressChanged, Linq.BalanceIPR.GetMappings());
        Synchronize(_sqledc.BatchLibrary, _spedc.BatchLibrary, progressChanged, Linq.BatchLib.GetMappings());
        Synchronize(_sqledc.SPFormat, _spedc.Format, progressChanged, Linq.Format.GetMappings());
        Synchronize(_sqledc.SKULibrary, _spedc.SKULibrary, progressChanged, Linq.Document.GetMappings());
        Synchronize(_sqledc.SKU, _spedc.SKU, progressChanged, Linq.SKUCommonPart.GetMappings());
        Synchronize(_sqledc.Batch, _spedc.Batch, progressChanged, Linq.Batch.GetMappings());
        Synchronize(_sqledc.CustomsUnion, _spedc.CustomsUnion, progressChanged, Linq.CustomsUnion.GetMappings());
        Synchronize(_sqledc.CutfillerCoefficient, _spedc.CutfillerCoefficient, progressChanged, Linq.CutfillerCoefficient.GetMappings());
        Synchronize(_sqledc.InvoiceLibrary, _spedc.InvoiceLibrary, progressChanged, Linq.InvoiceLib.GetMappings());
        Synchronize(_sqledc.InvoiceContent, _spedc.InvoiceContent, progressChanged, Linq.InvoiceContent.GetMappings());
        Synchronize(_sqledc.Material, _spedc.Material, progressChanged, Linq.Material.GetMappings());
        Synchronize(_sqledc.JSOXCustomsSummary, _spedc.JSOXCustomsSummary, progressChanged, Linq.JSOXCustomsSummary.GetMappings());
        Synchronize(_sqledc.Disposal, _spedc.Disposal, progressChanged, Linq.Disposal.GetMappings());
        Synchronize(_sqledc.Dust, _spedc.Dust, progressChanged, Linq.Dust.GetMappings());
        Synchronize(_sqledc.SADDuties, _spedc.SADDuties, progressChanged, Linq.SADDuties.GetMappings());
        Synchronize(_sqledc.SADPackage, _spedc.SADPackage, progressChanged, Linq.SADPackage.GetMappings());
        Synchronize(_sqledc.SADQuantity, _spedc.SADQuantity, progressChanged, Linq.SADQuantity.GetMappings());
        Synchronize(_sqledc.SADRequiredDocuments, _spedc.SADRequiredDocuments, progressChanged, Linq.SADRequiredDocuments.GetMappings());
        Synchronize(_sqledc.Settings, _spedc.Settings, progressChanged, Linq.Settings.GetMappings());
        Synchronize(_sqledc.SHMenthol, _spedc.SHMenthol, progressChanged, Linq.SHMenthol.GetMappings());
        Synchronize(_sqledc.StockLibrary, _spedc.StockLibrary, progressChanged, Linq.StockLib.GetMappings());
        Synchronize(_sqledc.StockEntry, _spedc.StockEntry, progressChanged, Linq.StockEntry.GetMappings());
        Synchronize(_sqledc.Usage, _spedc.Usage, progressChanged, Linq.Usage.GetMappings());
        Synchronize(_sqledc.Warehouse, _spedc.Warehouse, progressChanged, Linq.Warehouse.GetMappings());
        Synchronize(_sqledc.Waste, _spedc.Waste, progressChanged, Linq.Waste.GetMappings());
        Synchronize(_sqledc.ActivityLog, _spedc.ActivityLog, progressChanged, Linq.ActivityLogCT.GetMappings());
        //History();
        //ArchivingLogs();
        Linq2SQL.ArchivingOperationLogs.UpdateActivitiesLogs(_sqledc, Linq2SQL.ArchivingOperationLogs.OperationName.Synchronization, progressChanged);
        progressChanged(1, new ProgressChangedEventArgs(1, "SynchronizationContent has been finished"));
      }
    }
    private static void Synchronize<TSQL, TSP>(Table<TSQL> sqlTable, EntityList<TSP> spList, Action<object, ProgressChangedEventArgs> progressChanged, Dictionary<string, string> mapping)
      where TSQL : class, SharePoint.Client.Link2SQL.IItem, new()
      where TSP : Linq.Item, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SharePoint website.", typeof(TSP).Name)));
      List<TSP> _scrList = spList.ToList<TSP>();
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SQL database.", typeof(TSQL).Name)));
      Dictionary<int, IItem> _dictinary = sqlTable.Where<TSQL>(x => !x.OnlySQL).ToDictionary<TSQL, int, IItem>(x => x.ID, y => (SharePoint.Client.Link2SQL.IItem)y);
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Synchronization {0} elements in the SharePoint source tables with the {1} element in the SQL table.", _scrList.Count, _dictinary.Count)));
      //create descriptors using reflection
      Dictionary<string, List<StorageItem>> _spDscrpt = StorageItem.CreateStorageDescription(typeof(TSP));
      Dictionary<string, SQLStorageItem> _sqlDscrpt = SQLStorageItem.CreateStorageDescription(typeof(TSQL), mapping);
      foreach (TSP _spItem in _scrList)
      {
        SharePoint.Client.Link2SQL.IItem _sqlItem = default(SharePoint.Client.Link2SQL.IItem);
        if (!_dictinary.TryGetValue(_spItem.Id.Value, out _sqlItem))
        {
          _sqlItem = new TSQL();
          _sqlItem.OnlySQL = false;
          _dictinary.Add(_spItem.Id.Value, _sqlItem);
          sqlTable.InsertOnSubmit((TSQL)_sqlItem);
        }
        Microsoft.SharePoint.Linq.ContentTypeAttribute _attr =  _spItem.GetType().GetContentType();
        Synchronize<TSQL, TSP>((TSQL)_sqlItem, _spItem, _spDscrpt[_attr.Id], _sqlDscrpt, progressChanged, spList.DataContext);
      }
      progressChanged(1, new ProgressChangedEventArgs(1, "Submitting Changes to SQL database"));
      sqlTable.Context.SubmitChanges();
    }
    private static void Synchronize<TSQL, TSP>(TSQL sqlItem,
      TSP splItem,
      List<SharePoint.Client.Linq2SP.StorageItem> _spDscrpt,
      Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem> _sqlDscrpt,
      Action<object, ProgressChangedEventArgs> progressChanged,
      Microsoft.SharePoint.Linq.DataContext dataContext)
    {
      foreach (StorageItem _si in _spDscrpt.Where<StorageItem>(x => x.IsNotReverseLookup()))
        if (_sqlDscrpt.ContainsKey(_si.PropertyName))
          _si.GetValueFromEntity(splItem, x => _sqlDscrpt[_si.PropertyName].Assign(x, sqlItem), dataContext);
    }
  }
}
