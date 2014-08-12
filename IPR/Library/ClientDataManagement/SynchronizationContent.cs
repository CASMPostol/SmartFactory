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

using CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.SqlClient;
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
      IPRDEV _sqledc = Connect2SQL(settings, progressChanged);
      using (Entities _spedc = new Entities(settings.SiteURL))
      {
        SharePoint.Client.Link2SQL.RepositoryDataSet.ClearContent();
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
        //History();
        //ArchivingLogs();
        //ActivitiesLogs();
        UpdateActivitiesLogs(_sqledc, progressChanged);
        progressChanged(1, new ProgressChangedEventArgs(1, "SynchronizationContent has been finished"));
      }
    }

    private static void UpdateActivitiesLogs(IPRDEV sqlEntities, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      Linq2SQL.ArchivingOperationLogs _logs = new ArchivingOperationLogs()
      {
        Date = DateTime.Now,
        Operation = Linq2SQL.ArchivingOperationLogs.SynchronizationOperationName,
        UserName = String.Format(Properties.Resources.ActivitiesLogsUserNamePattern, Environment.UserName, Environment.MachineName)
      };
      sqlEntities.ArchivingOperationLogs.InsertOnSubmit(_logs);
      sqlEntities.SubmitChanges();
      progressChanged(1, new ProgressChangedEventArgs(1, "Updated ActivitiesLogs"));
    }
    private static void Synchronize<TSQL, TSP>(Table<TSQL> table, EntityList<TSP> entityList, Action<object, ProgressChangedEventArgs> progressChanged, Dictionary<string, string> mapping)
      where TSQL : class, SharePoint.Client.Link2SQL.IItem, new()
      where TSP : Linq.Item, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SharePoint website.", typeof(TSP).Name)));
      List<TSP> _scrList = entityList.ToList<TSP>();
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SQL database.", typeof(TSQL).Name)));
      Dictionary<int, SharePoint.Client.Link2SQL.IItem> _dictinary = table.ToDictionary<TSQL, int, SharePoint.Client.Link2SQL.IItem>(x => x.ID, y => (SharePoint.Client.Link2SQL.IItem)y);
      SharePoint.Client.Link2SQL.RepositoryDataSet.Repository.Add(entityList.Name, _dictinary);
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Synchronization {0} elements in the SharePoint source tables with the {1} element in the SQL table.", _scrList.Count, _dictinary.Count)));
      List<SharePoint.Client.Linq2SP.StorageItem> _spDscrpt = SharePoint.Client.Linq2SP.StorageItem.CreateStorageDescription(typeof(TSP), false);
      Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem> _sqlDscrpt = new Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem>();
      SharePoint.Client.Link2SQL.SQLStorageItem.FillUpStorageInfoDictionary(typeof(TSQL), mapping, _sqlDscrpt);
      foreach (TSP _spItem in _scrList)
      {
        SharePoint.Client.Link2SQL.IItem _sqlItem = default(SharePoint.Client.Link2SQL.IItem);
        if (!_dictinary.TryGetValue(_spItem.Id.Value, out _sqlItem))
        {
          _sqlItem = new TSQL();
          _dictinary.Add(_spItem.Id.Value, _sqlItem);
          table.InsertOnSubmit((TSQL)_sqlItem);
        }
        Synchronize<TSQL, TSP>((TSQL)_sqlItem, _spItem, _spDscrpt, _sqlDscrpt, progressChanged);
      }
      progressChanged(1, new ProgressChangedEventArgs(1, "Submitting Changes to SQL database"));
      table.Context.SubmitChanges();
    }
    private static void Synchronize<TSQL, TSP>(TSQL sqlItem,
      TSP splItem,
      List<SharePoint.Client.Linq2SP.StorageItem> _spDscrpt,
      Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem> _sqlDscrpt,
      Action<object, ProgressChangedEventArgs> progressChanged)
    {
      foreach (SharePoint.Client.Linq2SP.StorageItem _si in _spDscrpt.Where<SharePoint.Client.Linq2SP.StorageItem>(x => x.IsNotReverseLookup()))
        if (_sqlDscrpt.ContainsKey(_si.PropertyName))
          _si.GetValueFromEntity(splItem, x => _sqlDscrpt[_si.PropertyName].Assign(x, sqlItem));
      //else
      //  progressChanged(_si, new ProgressChangedEventArgs(1, String.Format("Cannot find the {0} argument in the SQL entity {1}.", _si.PropertyName, typeof(TSP).Name)));
    }
    private static IPRDEV Connect2SQL(SynchronizationSettings settings, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      progressChanged(settings, new ProgressChangedEventArgs(1, String.Format("Attempt to connect to SQL at: {0}", settings.ConnectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(settings.ConnectionString);
      IPRDEV _entities = new IPRDEV(_connection);
      if (_entities.DatabaseExists())
        progressChanged(settings, new ProgressChangedEventArgs(1, "The specified database can be opened."));
      else
        progressChanged(settings, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      return _entities;
    }

  }
}
