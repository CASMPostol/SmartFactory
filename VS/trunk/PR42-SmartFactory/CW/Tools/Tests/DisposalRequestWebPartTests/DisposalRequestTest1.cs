using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Test
{
  [TestClass]
  public class DisposalRequestTest1
  {
    private static List<CustomsWarehouse> listOfAccounts = new List<CustomsWarehouse>();
    private static IGrouping<string, CustomsWarehouseDisposal> groupOfDisposals = null;

    [ClassInitialize]
    public static void ClassInitializeMethod(TestContext context)
    {
      CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.SampleData.RequestSampleData.GetData(listOfAccounts, out groupOfDisposals);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateDisposalRequestInstanceWrongDataTest()
    {
      List<CustomsWarehouse> _listOfAccounts = new List<CustomsWarehouse>();
      List<CustomsWarehouseDisposal> _listOfDisposals = new List<CustomsWarehouseDisposal>();
      IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> _groupOfDisposals = _listOfDisposals.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch);
      DisposalRequest _newItem = DisposalRequest.Create(_listOfAccounts, _groupOfDisposals.First(), (x, y) => { });
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceDemoDataTest()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      Assert.AreEqual(_newItem.AutoCalculation, false);
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 7820.0, 99, 17820.0, 17820.0, 17280.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstance0Down()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      Assert.AreEqual(_newItem.AutoCalculation, false);
      _newItem.AutoCalculation = true;
      DisposalRequestDetails _firs = _newItem.Items[0];
      _newItem.Items[0].MoveDown.Execute(null);
      Assert.AreSame(_firs, _newItem.Items[1]);
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 7820.0, 99, 17820.0, 17820.0, 17280.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstance1Up()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      Assert.AreEqual(_newItem.AutoCalculation, false);
      _newItem.AutoCalculation = true;
      DisposalRequestDetails _firs = _newItem.Items[0];
      _newItem.Items[1].MoveUp.Execute(null);
      Assert.AreSame(_firs, _newItem.Items[1]);
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 7820.0, 99, 17820.0, 17820.0, 17280.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceBottom2Top()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      Assert.AreEqual(_newItem.AutoCalculation, false);
      _newItem.AutoCalculation = true;
      DisposalRequestDetails _firs = _newItem.Items[4];
      _newItem.Items[4].MoveUp.Execute(null);
      _newItem.Items[3].MoveUp.Execute(null);
      _newItem.Items[2].MoveUp.Execute(null);
      _newItem.Items[1].MoveUp.Execute(null);
      Assert.AreSame(_firs, _newItem.Items[0]);
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 7820.0, 99, 17820.0, 17820.0, 17280.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceEndOfOgl()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      AssertConstantValues(_newItem);
      _newItem.AutoCalculation = true;
      _newItem.EndOfOgl();
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 6020.0, 89, 16020.0, 16020.0, 19080.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceUnavailable()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      AssertConstantValues(_newItem);
      _newItem.AutoCalculation = true;
      _newItem.AddedKg = 999999; //Tobacco unavailable
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 25100.0, 195, 35100.0, 35100.0, 0.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceEndOfBatch()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => { });
      AssertConstantValues(_newItem);
      _newItem.AutoCalculation = true;
      _newItem.EndOfBatch();
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 25100.0, 195, 35100.0, 35100.0, 0.0);
    }
    private static void AssertValues
      (DisposalRequest _newItem, double AddedKg, int PackagesToDispose, double QuantityyToClearSum, double QuantityyToClearSumRounded, double RemainingOnStock)
    {
      Assert.AreEqual(AddedKg, _newItem.AddedKg);
      Assert.AreEqual(PackagesToDispose, _newItem.PackagesToDispose);
      Assert.AreEqual(QuantityyToClearSum, _newItem.QuantityyToClearSum);
      Assert.AreEqual(QuantityyToClearSumRounded, _newItem.QuantityyToClearSumRounded);
      Assert.AreEqual(RemainingOnStock, _newItem.RemainingOnStock);
    }
    private static void AssertConstantValues(DisposalRequest _newItem)
    {
      Assert.AreEqual(5, _newItem.Items.Count);
      Assert.AreEqual(_newItem.Batch, "0003808069");
      Assert.AreEqual(_newItem.CustomsProcedure, "4071");
      Assert.AreEqual(_newItem.MassPerPackage, 180);
      Assert.AreEqual(_newItem.SKU, "12607453");
      Assert.AreEqual(_newItem.Units, "kg");
      Assert.AreEqual(_newItem.TotalStock, 35100.0);
      Assert.AreEqual(_newItem.DeclaredNetMass, 10000.0);
    }
    private static void AssertButtonsCanExecute(DisposalRequest _newItem)
    {
      Assert.AreEqual(_newItem.IsBottomActive(0), false);
      Assert.AreEqual(_newItem.IsBottomActive(4), true);
      Assert.AreEqual(_newItem.IsTopActive(0), true);
      Assert.AreEqual(_newItem.IsTopActive(4), false);
      Assert.AreEqual(false, _newItem.Items[0].MoveUp.CanExecute(null));
      Assert.AreEqual(true, _newItem.Items[0].MoveDown.CanExecute(null));
      Assert.AreEqual(true, _newItem.Items[4].MoveUp.CanExecute(null));
      Assert.AreEqual(false, _newItem.Items[4].MoveDown.CanExecute(null));
    }
  }
}
