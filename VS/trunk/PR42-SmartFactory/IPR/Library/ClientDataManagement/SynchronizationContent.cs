//<summary>
//  Title   : class SynchronizationContent
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
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
        Dictionary<int, Linq2SQL.JSOXLibrary> _JSOXLibrary = Synchronize(_sqledc.JSOXLibrary, _spedc.JSOXLibrary, progressChanged);
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
      }
    }
    private static Dictionary<int, TSQL> Synchronize<TSQL, TSP>(Table<TSQL> table, EntityList<TSP> entityList, Action<object, ProgressChangedEventArgs> progressChanged)
      where TSQL : class, IItem, new()
      where TSP : Linq.Item, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SQL database.", typeof(TSQL).Name)));
      Dictionary<int, TSQL> _dictinary = table.ToDictionary<TSQL, int>(x => x.ID);
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SharePoint website.", typeof(TSP).Name)));
      List<TSP> _scrList = entityList.ToList<TSP>();
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Synchronization {0} elements in the SharePoint source tables with the {1} element in the SQL table.", _scrList.Count, _dictinary.Count)));
      foreach (TSP _spItem in _scrList)
      {
        TSQL _sqlItem = default(TSQL);
        if (!_dictinary.TryGetValue(_spItem.Id.Value, out _sqlItem))
        {
          _sqlItem = new TSQL();
          _dictinary.Add(_spItem.Id.Value, _sqlItem);
          table.InsertOnSubmit(_sqlItem);
        }
        Synchronize<TSQL, TSP>(_sqlItem, _spItem);
      }
      progressChanged(1, new ProgressChangedEventArgs(1, "Submitting Changes to SQL database"));
      //table.Context.SubmitChanges();
      return _dictinary;
    }
    private static void Synchronize<TSQL, TSP>(TSQL _spItem, TSP _sqlItem)
    {
      List<SharePoint.Client.Linq.StorageItem> _dscrpt = new List<SharePoint.Client.Linq.StorageItem>();
      SharePoint.Client.Linq.StorageItem.CreateStorageDescription(typeof(TSP), _dscrpt);
      foreach (SharePoint.Client.Linq.StorageItem _si in _dscrpt.Where < SharePoint.Client.Linq.StorageItem>( x => x.IsNotReverseLookup()))
      {
        _si.GetValueFromEntity(_sqlItem, x => Assign(x, _sqlItem));
      }
    }
    private static void Assign(SharePoint.Client.Linq.StorageItem.Descriptor descriptor, object sqlItem)
    {
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
