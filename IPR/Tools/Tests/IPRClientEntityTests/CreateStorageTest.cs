using CAS.SharePoint.Client.Linq2SP;
using CAS.SmartFactory.IPR.Client.DataManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagementCAS.SmartFactory.IPR.Client.DataManagement
{

  [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
  public class CreateStorageTest
  {
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CreateStorageDescription4SP()
    {
      List<StorageItem> _storageList = new List<StorageItem>();
      StorageItem.CreateStorageDescription(typeof(Disposal), _storageList);
      Dictionary<string, StorageItem> _storageDictionary = _storageList.ToDictionary<StorageItem, string>(si => si.Description.Name);
      Assert.AreEqual(32, _storageDictionary.Count );
      _storageDictionary = _storageList.ToDictionary<StorageItem, string>(si => si.PropertyName);
      Assert.AreEqual(32, _storageDictionary.Count);
    }
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CreateStorageDescription4SP()
    {
      List<StorageItem> _storageList = new List<StorageItem>();
      StorageItem.CreateStorageDescription(typeof(Disposal), _storageList);
      Dictionary<string, StorageItem> _storageDictionary = _storageList.ToDictionary<StorageItem, string>(si => si.Description.Name);
      Assert.AreEqual(32, _storageDictionary.Count);
      _storageDictionary = _storageList.ToDictionary<StorageItem, string>(si => si.PropertyName);
      Assert.AreEqual(32, _storageDictionary.Count);
    }
  }
}
