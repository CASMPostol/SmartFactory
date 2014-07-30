using CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement
{
  /// <summary>
  /// Helpers to synchronize SQL content with the SharePoint website
  /// </summary>
  public static class SynchronizationContent
  {
    public struct ArchiveSettings
    {
      public string SiteURL;
      public string ConnectionString;
    }
    public static void Go(ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      IPRDEV _sqledc = Connect2SQL(settings, progressChanged);
      using (Entities _spedc = new Entities(settings.SiteURL))
      {
        Dictionary<int, Linq2SQL.JSOXLibrary> _JSOXLibrary = JSOXLibrary(_sqledc.JSOXLibrary, _spedc.JSOXLibrary, progressChanged);
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

    private static Dictionary<int, Linq2SQL.JSOXLibrary> JSOXLibrary<TSP>

      (Table<Linq2SQL.JSOXLibrary> table, EntityList<TSP> entityList, Action<object, ProgressChangedEventArgs> progressChanged)
      where TSP : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new() 
    {
      progressChanged(1, new ProgressChangedEventArgs(1, "Reading the table JSOXLibrary from SQL database."));
      Dictionary<int, Linq2SQL.JSOXLibrary> _dictinary = table.ToDictionary<Linq2SQL.JSOXLibrary, int>(x => x.ID);
      Synchronize<Linq2SQL.JSOXLibrary, TSP>(entityList, _dictinary, progressChanged);
      return _dictinary;
    }
    private static void Synchronize<TSQL, TSP>(EntityList<TSP> entityList, Dictionary<int, TSQL> _dictinary, Action<object, ProgressChangedEventArgs> progressChanged)
      where TSP : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
      where TSQL : new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, "Reading the table JSOXLibrary from SharePoint website."));
      List<TSP> _scrList = entityList.ToList<TSP>();

    }
    private static IPRDEV Connect2SQL(ArchiveSettings settings, Action<object, ProgressChangedEventArgs> progressChanged)
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
