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
      TestToDictionary<ClinetLinqSP.JSOXLib>(24);
      TestToDictionary<ClinetLinqSP.BalanceBatch>(37);
      TestToDictionary<ClinetLinqSP.SADDocumentLib>(13);
      TestToDictionary<ClinetLinqSP.SADDocumentType>(17);
      TestToDictionary<ClinetLinqSP.SADGood>(16);
      TestToDictionary<ClinetLinqSP.SADConsignment>(11);
      TestToDictionary<ClinetLinqSP.Clearence>(15);
      TestToDictionary<ClinetLinqSP.Consent>(14);
      TestToDictionary<ClinetLinqSP.PCNCode>(10);
      TestToDictionary<ClinetLinqSP.IPRLib>(11);
      TestToDictionary<ClinetLinqSP.IPR>(42);
      TestToDictionary<ClinetLinqSP.BalanceIPR>(38);
      TestToDictionary<ClinetLinqSP.BatchLib>(12);
      TestToDictionary<ClinetLinqSP.Format>(9);
      TestToDictionary<ClinetLinqSP.Document>(10);
      TestToDictionary<ClinetLinqSP.SKUCommonPart>(13);
      TestToDictionary<ClinetLinqSP.Batch>(42);
      TestToDictionary<ClinetLinqSP.CustomsUnion>(8);
      TestToDictionary<ClinetLinqSP.CutfillerCoefficient>(10);
      TestToDictionary<ClinetLinqSP.InvoiceLib>(16);
      TestToDictionary<ClinetLinqSP.InvoiceContent>(15);
      TestToDictionary<ClinetLinqSP.Material>(23);
      TestToDictionary<ClinetLinqSP.JSOXCustomsSummary>(17);
      TestToDictionary<ClinetLinqSP.Disposal>(32);
      TestToDictionary<ClinetLinqSP.Dust>(8);
      TestToDictionary<ClinetLinqSP.SADDuties>(11);
      TestToDictionary<ClinetLinqSP.SADPackage>(11);
      TestToDictionary<ClinetLinqSP.SADQuantity>(12);
      TestToDictionary<ClinetLinqSP.SADRequiredDocuments>(11);
      TestToDictionary<ClinetLinqSP.Settings>(8);
      TestToDictionary<ClinetLinqSP.SHMenthol>(8);
      TestToDictionary<ClinetLinqSP.StockLib>(12);
      TestToDictionary<ClinetLinqSP.StockEntry>(21);
      TestToDictionary<ClinetLinqSP.Usage>(11);
      TestToDictionary<ClinetLinqSP.Warehouse>(10);
      TestToDictionary<ClinetLinqSP.Waste>(8);
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void CompareStorageContent()
    {
      ComareSelectedStoragesContent<ClinetLinqSP.JSOXLib, ClinetLinqSQL.JSOXLibrary>(ClinetLinqSP.JSOXLib.GetMappings()); //TODO Documents fields must be resolved
      ComareSelectedStoragesContent<ClinetLinqSP.BalanceBatch, ClinetLinqSQL.BalanceBatch>(ClinetLinqSP.BalanceBatch.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.SADDocumentLib, ClinetLinqSQL.SADDocumentLibrary>(); //TODO Documents fields must be resolved
      ComareSelectedStoragesContent<ClinetLinqSP.SADDocumentType, ClinetLinqSQL.SADDocument>(ClinetLinqSP.SADDocumentType.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SADGood, ClinetLinqSQL.SADGood>(ClinetLinqSP.SADGood.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.SADConsignment, ClinetLinqSQL.SADConsignment>();
      ComareSelectedStoragesContent<ClinetLinqSP.Clearence, ClinetLinqSQL.Clearence>(ClinetLinqSP.Clearence.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Consent, ClinetLinqSQL.Consent>(ClinetLinqSP.Consent.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.PCNCode, ClinetLinqSQL.PCNCode>(ClinetLinqSP.PCNCode.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.IPRLib, ClinetLinqSQL.IPRLibrary>(ClinetLinqSP.IPRLib.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.IPR, ClinetLinqSQL.IPR>(); IPRClosi ?? is not present for SP model 
      ComareSelectedStoragesContent<ClinetLinqSP.BalanceIPR, ClinetLinqSQL.BalanceIPR>(ClinetLinqSP.BalanceIPR.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.BatchLib, ClinetLinqSQL.BatchLibrary>();
      ComareSelectedStoragesContent<ClinetLinqSP.Format, ClinetLinqSQL.SPFormat>(ClinetLinqSP.Format.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.Document, ClinetLinqSQL.SKULibrary>();
      //TODO define task ComareSelectedStoragesContent<ClinetLinqSP.SKUCigarette, ClinetLinqSQL.SKU>(); -resolve inherited types
      ComareSelectedStoragesContent<ClinetLinqSP.Batch, ClinetLinqSQL.Batch>(ClinetLinqSP.Batch.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.CustomsUnion, ClinetLinqSQL.CustomsUnion>(ClinetLinqSP.CustomsUnion.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.CutfillerCoefficient, ClinetLinqSQL.CutfillerCoefficient>(ClinetLinqSP.CutfillerCoefficient.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.InvoiceLib, ClinetLinqSQL.InvoiceLibrary>(ClinetLinqSP.InvoiceLib.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.InvoiceContent, ClinetLinqSQL.InvoiceContent>(ClinetLinqSP.InvoiceContent.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Material, ClinetLinqSQL.Material>(ClinetLinqSP.Material.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.JSOXCustomsSummary, ClinetLinqSQL.JSOXCustomsSummary>(ClinetLinqSP.JSOXCustomsSummary.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Disposal, ClinetLinqSQL.Disposal>(ClinetLinqSP.Disposal.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Dust, ClinetLinqSQL.Dust>(ClinetLinqSP.Dust.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SADDuties, ClinetLinqSQL.SADDuties>(ClinetLinqSP.SADDuties.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SADPackage, ClinetLinqSQL.SADPackage>(ClinetLinqSP.SADPackage.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SADQuantity, ClinetLinqSQL.SADQuantity>(ClinetLinqSP.SADQuantity.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SADRequiredDocuments, ClinetLinqSQL.SADRequiredDocuments>(ClinetLinqSP.SADRequiredDocuments.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Settings, ClinetLinqSQL.Settings>(ClinetLinqSP.Settings.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.SHMenthol, ClinetLinqSQL.SHMenthol>(ClinetLinqSP.SHMenthol.GetMappings());
      //TODO ComareSelectedStoragesContent<ClinetLinqSP.StockLib, ClinetLinqSQL.StockLibrary>(); Documents fields must be resolved
      ComareSelectedStoragesContent<ClinetLinqSP.StockEntry, ClinetLinqSQL.StockEntry>(ClinetLinqSP.StockEntry.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Usage, ClinetLinqSQL.Usage>(ClinetLinqSP.Usage.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Warehouse, ClinetLinqSQL.Warehouse>(ClinetLinqSP.Warehouse.GetMappings());
      ComareSelectedStoragesContent<ClinetLinqSP.Waste, ClinetLinqSQL.Waste>(ClinetLinqSP.Waste.GetMappings());
    }

    private static void TestToDictionary<TEntity>(int expectedCount)
    {
      List<Linq2SP.StorageItem> _storageList = Linq2SP.StorageItem.CreateStorageDescription(typeof(TEntity), false);
      Dictionary<string, Linq2SP.StorageItem> _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.Description.Name);
      Assert.AreEqual(expectedCount, _storageDictionary.Count, String.Format("See list {0}", typeof(TEntity).Name));
      _storageDictionary = _storageList.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      Assert.AreEqual(expectedCount, _storageDictionary.Count);
    }
    private static void ComareSelectedStoragesContent<TSP, TSQL>(Dictionary<string, string> mapping)
    {
      //Get SP stage info
      List<Linq2SP.StorageItem> _storageListSP = Linq2SP.StorageItem.CreateStorageDescription(typeof(TSP), false);
      Dictionary<string, Linq2SP.StorageItem> _storageSPDictionary = _storageListSP.ToDictionary<Linq2SP.StorageItem, string>(si => si.PropertyName);
      //Get SQL stage info
      Dictionary<string, Linq2SQL.SQLStorageItem> _storageListSQLDictionary = new Dictionary<string, Linq2SQL.SQLStorageItem>();
      Linq2SQL.SQLStorageItem.FillUpStorageInfoDictionary(typeof(TSQL), mapping, _storageListSQLDictionary);
      //Assert.AreEqual<int>(_storageSPDictionary.Count, _storageListSQL.Count, String.Format("Storage length of {0} must be equal, if not loss of data may occur", typeof(TSP).Name));
      foreach (string _item in _storageListSQLDictionary.Keys)
      {
        if (!_storageSPDictionary.ContainsKey(_item))
          Assert.Fail(String.Format("Storage SP of {1} does not contain property {0}", _item, typeof(TSP).Name));
      }
      foreach (string _item in _storageSPDictionary.Keys)
      {
        if (!_storageListSQLDictionary.ContainsKey(_item))
          Assert.Fail(String.Format("Storage SQL of {1} does not contain property {0}", _item, typeof(TSQL).Name));
      }
    }
  }
}
