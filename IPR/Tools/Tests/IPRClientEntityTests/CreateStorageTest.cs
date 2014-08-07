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
      List<Linq2SP.StorageItem> _storageList = new List<Linq2SP.StorageItem>();
      Linq2SP.StorageItem.CreateStorageDescription(typeof(ClinetLinqSP.Disposal), _storageList);
      Dictionary<string, Linq2SP.StorageItem> _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.Description.Name);
      Assert.AreEqual(32, _storageDictionary.Count);
      _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      Assert.AreEqual(32, _storageDictionary.Count);
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CompareStorageContent()
    {
      //Get SP stage info
      List<Linq2SP.StorageItem> _storageListSP = new List<Linq2SP.StorageItem>();
      Linq2SP.StorageItem.CreateStorageDescription(typeof(ClinetLinqSP.Disposal), _storageListSP);
      Dictionary<string, Linq2SP.StorageItem> _storageSPDictionary = _storageListSP.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      //Get SQL stage info
      Dictionary<string, Linq2SQL.SQLStorageItem> _storageListSQL = new Dictionary<string, Linq2SQL.SQLStorageItem>();
      Linq2SQL.SQLStorageItem.FillUpStorageInfoDictionary(typeof(ClinetLinqSQL.Disposal), ClinetLinqSP.Disposal.GetMappings(), _storageListSQL);
      Assert.AreEqual<int>(_storageSPDictionary.Count, _storageListSQL.Count, "Tables must be equal, if not loss of data may occur");
      foreach (string _sqlItem in _storageListSQL.Keys)
      {
        if (!_storageSPDictionary.ContainsKey(_sqlItem))
          Assert.Fail(String.Format("Storage SP does not contain property {0}", _sqlItem));
      }
    }
  }
}
