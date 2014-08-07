using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ClinetLinqSP = CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using ClinetLinqSQL = CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL;
using Linq2SP = CAS.SharePoint.Client.Linq2SP;
using Linq2SQL = CAS.SharePoint.Client.Link2SQL;

namespace CAS.SmartFactory.IPR.Client.DataManagementCAS.SmartFactory.IPR.Client.DataManagement
{

  [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
  public class CreateStorageTest
  {
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CreateStorageDescription4SP()
    {
      TestToDictionary<ClinetLinqSP.Disposal>(32);
      TestToDictionary<ClinetLinqSP.Dust>(8);
      TestToDictionary<ClinetLinqSP.SADDuties>(11);
      TestToDictionary<ClinetLinqSP.SADPackage>(11);
      TestToDictionary<ClinetLinqSP.SADQuantity>(12);
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CompareStorageContent()
    {
      ComareSelectedStoragesContent<ClinetLinqSP.Disposal, ClinetLinqSQL.Disposal>();
      ComareSelectedStoragesContent<ClinetLinqSP.Dust, ClinetLinqSQL.Dust>();
      ComareSelectedStoragesContent<ClinetLinqSP.SADDuties, ClinetLinqSQL.SADDuties>();
      ComareSelectedStoragesContent<ClinetLinqSP.SADPackage, ClinetLinqSQL.SADDuties>();
      ComareSelectedStoragesContent<ClinetLinqSP.SADQuantity, ClinetLinqSQL.SADDuties>();
    }

    private static void TestToDictionary<TEntity>(int expectedCount)
    {
      List<Linq2SP.StorageItem> _storageList = Linq2SP.StorageItem.CreateStorageDescription(typeof(TEntity));
      Dictionary<string, Linq2SP.StorageItem> _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.Description.Name);
      Assert.AreEqual(expectedCount, _storageDictionary.Count);
      _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      Assert.AreEqual(expectedCount, _storageDictionary.Count);
    }
    private static void ComareSelectedStoragesContent<TSP, TSQL>()
    {
      //Get SP stage info
      List<Linq2SP.StorageItem> _storageListSP = Linq2SP.StorageItem.CreateStorageDescription(typeof(TSP));
      Dictionary<string, Linq2SP.StorageItem> _storageSPDictionary = _storageListSP.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      //Get SQL stage info
      Dictionary<string, Linq2SQL.SQLStorageItem> _storageListSQLDictionary = new Dictionary<string, Linq2SQL.SQLStorageItem>();
      Linq2SQL.SQLStorageItem.FillUpStorageInfoDictionary(typeof(TSQL), ClinetLinqSP.Disposal.GetMappings(), _storageListSQLDictionary);
      //Assert.AreEqual<int>(_storageSPDictionary.Count, _storageListSQL.Count, String.Format("Storage length of {0} must be equal, if not loss of data may occur", typeof(TSP).Name));
      foreach (string _item in _storageListSQLDictionary.Keys)
      {
        if (!_storageSPDictionary.ContainsKey(_item))
          Assert.Fail(String.Format("Storage SP of {1} does not contain property {0}", _item, typeof(TSP).Name));
      }
      foreach (string _item in _storageSPDictionary.Keys)
      {
        if (!_storageListSQLDictionary.ContainsKey(_item))
          Assert.Fail(String.Format("Storage SQL of {1} does not contain property {0}", _item, typeof(TSP).Name));
      }
    }
  }
}
