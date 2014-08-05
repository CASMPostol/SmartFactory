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
        //BalanceBatch();
        //SADDocumentLibrary();
        //SADDocument();
        //SADGood();
        //SADConsignment();
        //Clearence();
        //Consent();
        //PCNCode();
        //IPRLibrary();
        //IPR();
        //BalanceIPR();
        //BatchLibrary();
        //SPFormat();
        //SKULibrary();
        //SKU();
        //Batch();
        //CustomsUnion();
        //CutfillerCoefficient();
        //InvoiceLibrary();
        //InvoiceContent();
        //Material();
        //JSOXCustomsSummary();
        //Disposal();
        //Dust();
        //SADDuties();
        //SADPackage();
        //SADQuantity();
        //SADRequiredDocuments();
        //Settings();
        //SHMenthol();
        //StockLibrary();
        //StockEntry();
        //Usage();
        //Warehouse();
        //Waste();
        //History();
        //ArchivingLogs();
        //ActivitiesLogs();
        UpdateActivitiesLogs(_sqledc, progressChanged);
        progressChanged(1, new ProgressChangedEventArgs(1, "SynchronizationContent has been finished"));
      }
    }

    private static void UpdateActivitiesLogs(IPRDEV sqlEntities, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      Linq2SQL.ActivitiesLogs _logs = new ActivitiesLogs()
      {
        Date = DateTime.Now,
        Operation = Linq2SQL.ActivitiesLogs.SynchronizationOperationName,
        UserName = String.Format(Properties.Resources.ActivitiesLogsUserNamePattern, Environment.UserName, Environment.MachineName)
      };
      sqlEntities.ActivitiesLogs.InsertOnSubmit(_logs);
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
      List<SharePoint.Client.Linq2SP.StorageItem> _spDscrpt = new List<SharePoint.Client.Linq2SP.StorageItem>();
      SharePoint.Client.Linq2SP.StorageItem.CreateStorageDescription(typeof(TSP), _spDscrpt);
      Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem> _sqlDscrpt = new Dictionary<string, SharePoint.Client.Link2SQL.SQLStorageItem>();
      SharePoint.Client.Link2SQL.SQLStorageItem.CreateStorageDescription(typeof(TSQL), mapping, _sqlDscrpt);
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
        else
          progressChanged(_si, new ProgressChangedEventArgs(1, String.Format("Cannot find the {0} argument in the SQL entity {1}.", _si.PropertyName, typeof(TSP).Name)));
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
