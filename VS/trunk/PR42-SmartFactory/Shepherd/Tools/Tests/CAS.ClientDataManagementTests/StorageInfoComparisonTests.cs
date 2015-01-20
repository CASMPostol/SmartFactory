using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.SharePoint.Client.SP2SQLInteroperability;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Tests
{
  [TestClass]
  public class StorageInfoComparisonTests
  {
    [TestMethod]
    public void StorageInfoComparisonTestMethod()
    {
      Synchronizer.CompareSelectedStoragesContent<Linq.BusienssDescription, Linq2SQL.BusinessDescription>(Linq.BusienssDescription.GetMappings()); //TODO Documents fields must be resolved

    }
  }
}
