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
    private static List<CustomsWarehouseDisposal> listOfDisposals = new List<CustomsWarehouseDisposal>();
    private static IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> groupOfDisposals = null;

    [ClassInitialize]
    public static void ClassInitializeMethod(TestContext context)
    {
      CustomsWarehouse _newCW = new CustomsWarehouse()
      {
        DocumentNo = "OGL/362010/00/003231/2014",
        CustomsDebtDate = new DateTime(2014, 02, 14),
        Grade = "XIDSME",
        SKU = "12607453",
        Batch = "0003808069",
        AccountBalance = 9000,
        TobaccoNotAllocated = 0,
        NetMass = 9000,
        GrossMass = 9750,
        CW_PackageUnits = 50,
        CW_MassPerPackage = 180,
        Value = 76401,
        InvoiceNo = "82203825",
        AccountClosed = false,
        Units = "kg",
      };
      listOfAccounts.Add(_newCW);
      CustomsWarehouseDisposal _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        CW_AddedKg = 0,
        CW_DeclaredNetMass = 9000,
        CW_SettledGrossMass = 9750,
        CW_PackageToClear = 50,
        TobaccoValue = 76401,
        CustomsProcedure = "4071",
      };
      listOfDisposals.Add(_newCWD);
      _newCW = new CustomsWarehouse()
     {
       DocumentNo = "OGL/362010/00/013937/2014",
       CustomsDebtDate = new DateTime(2014, 06, 23),
       Grade = "XIDSME",
       SKU = "12607453",
       Batch = "0003808069",
       AccountBalance = 7020,
       TobaccoNotAllocated = 0,
       NetMass = 7020,
       GrossMass = 7605,
       CW_PackageUnits = 39,
       CW_MassPerPackage = 180,
       Value = 59592.78,
       InvoiceNo = "82219877",
       AccountClosed = false,
       Units = "kg",
     };
      listOfAccounts.Add(_newCW);
      _newCWD = new CustomsWarehouseDisposal()
     {
       CWL_CWDisposal2CustomsWarehouseID = _newCW,
       CW_AddedKg = 6020,
       CW_DeclaredNetMass = 1000,
       CW_SettledGrossMass = 7605,
       CW_PackageToClear = 39,
       TobaccoValue = 59592.78,
       CustomsProcedure = "4071",
     };
      listOfDisposals.Add(_newCWD);
      _newCW = new CustomsWarehouse()
      {
        DocumentNo = "OGL/362010/00/015363/2014",
        CustomsDebtDate = new DateTime(2014, 07, 14),
        Grade = "XIDSME",
        SKU = "12607453",
        Batch = "0003808069",
        AccountBalance = 6120,
        TobaccoNotAllocated = 4320,
        NetMass = 6120,
        GrossMass = 6630,
        CW_PackageUnits = 34,
        CW_MassPerPackage = 180,
        Value = 51952.68,
        InvoiceNo = "82222800",
        AccountClosed = false
      };
      listOfAccounts.Add(_newCW);
      _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        CW_AddedKg = 1800,
        CW_DeclaredNetMass = 0,
        CW_SettledGrossMass = 1950,
        CW_PackageToClear = 10,
        TobaccoValue = 15280.2,
        CustomsProcedure = "4071",
      };
      listOfDisposals.Add(_newCWD);
      _newCW = new CustomsWarehouse()
      {
        DocumentNo = "OGL/362010/00/016193/2014",
        CustomsDebtDate = new DateTime(2014, 07, 15),
        Grade = "XIDSME",
        SKU = "12607453",
        Batch = "0003808069",
        AccountBalance = 4140,
        TobaccoNotAllocated = 4140,
        NetMass = 4140,
        GrossMass = 4485,
        CW_PackageUnits = 23,
        CW_MassPerPackage = 180,
        Value = 35144.46,
        InvoiceNo = "82223962",
        AccountClosed = false
      };
      listOfAccounts.Add(_newCW);
      _newCW = new CustomsWarehouse()
      {
        DocumentNo = "OGL/362010/00/017253/2014",
        CustomsDebtDate = new DateTime(2014, 07, 16),
        Grade = "XIDSME",
        SKU = "12607453",
        Batch = "0003808069",
        AccountBalance = 8820,
        TobaccoNotAllocated = 8820,
        NetMass = 8820,
        GrossMass = 9555,
        CW_PackageUnits = 49,
        CW_MassPerPackage = 180,
        Value = 74872.98,
        InvoiceNo = "82225744",
        AccountClosed = false
      };
      listOfAccounts.Add(_newCW);
      groupOfDisposals = listOfDisposals.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateDisposalRequestInstanceWrongDataTest()
    {
      List<CustomsWarehouse> _listOfAccounts = new List<CustomsWarehouse>();
      List<CustomsWarehouseDisposal> _listOfDisposals = new List<CustomsWarehouseDisposal>();
      IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> _groupOfDisposals = _listOfDisposals.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch);
      DisposalRequest _newItem = DisposalRequest.Create(_listOfAccounts, _groupOfDisposals.First());
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceDemoDataTest()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
      Assert.AreEqual(_newItem.AutoCalculation, false);
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 7820.0, 99, 17820.0, 17820.0, 17280.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstance0Down()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
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
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
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
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
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
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
      AssertConstantValues(_newItem);
      _newItem.AutoCalculation = true;
      _newItem.EndOfOgl();
      AssertButtonsCanExecute(_newItem);
      AssertConstantValues(_newItem);
      AssertValues(_newItem, 12140.0, 123, 22140.0, 22140.0, 12960.0);
    }
    [TestMethod]
    public void CreateDisposalRequestInstanceUnavailable()
    {
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
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
      DisposalRequest _newItem = DisposalRequest.Create(listOfAccounts, groupOfDisposals.First());
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
      Assert.AreEqual(_newItem.IsBottom(0), false);
      Assert.AreEqual(_newItem.IsBottom(4), true);
      Assert.AreEqual(_newItem.IsTop(0), true);
      Assert.AreEqual(_newItem.IsTop(4), false);
      Assert.AreEqual(false, _newItem.Items[0].MoveUp.CanExecute(null));
      Assert.AreEqual(true, _newItem.Items[0].MoveDown.CanExecute(null));
      Assert.AreEqual(true, _newItem.Items[4].MoveUp.CanExecute(null));
      Assert.AreEqual(false, _newItem.Items[4].MoveDown.CanExecute(null));
    }
  }
}
