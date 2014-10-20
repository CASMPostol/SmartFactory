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
         Gradegrade = "XIDSME",
         SKU = "12607453",
         Batch = "0003808069",
         AccountBalance = 9000,
         TobaccoNotAllocated = 0,
         NetMass = 9000,
         _grossMass = 9750,
         _cW_PackageUnits = 50,
         _cW_PackageKg = 180,
         _value = 76401,
         _invoiceNo = "82203825",
         _accountClosed = false
      };
      listOfAccounts.Add(_newCW);
      CustomsWarehouseDisposal _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        _cW_AddedKg = 9000,
        _cW_DeclaredNetMass = 0,
        _cW_SettledGrossMass = 9750,
        _cW_PackageToClear = 50,
        _tobaccoValue = 76401,
        _customsProcedure = "4071",        
      };
      listOfDisposals.Add(_newCWD);
       _newCW = new CustomsWarehouse()
      {
        _documentNo = "OGL/362010/00/013937/2014",
        _grade = "XIDSME",
        _sKU = "12607453",
        _batch = "0003808069",
        _accountBalance = 7020,
        _tobaccoNotAllocated = 0,
        _netMass = 7020,
        _grossMass = 7605,
        _cW_PackageUnits = 39,
        _cW_PackageKg = 180,
        _value = 59592.78,
        _invoiceNo = "82219877",
        _accountClosed = false
      };
      listOfAccounts.Add(_newCW);
       _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        _cW_AddedKg = 1000,
        _cW_DeclaredNetMass = 6020,
        _cW_SettledGrossMass = 7605,
        _cW_PackageToClear = 39,
        _tobaccoValue = 59592.78,
        _customsProcedure = "4071",
      };
      listOfDisposals.Add(_newCWD);
      _newCW = new CustomsWarehouse()
      {
        _documentNo = "OGL/362010/00/015363/2014",
        _grade = "XIDSME",
        _sKU = "12607453",
        _batch = "0003808069",
        _accountBalance = 6120,
        _tobaccoNotAllocated = 5320,
        _netMass = 6120,
        _grossMass = 6630,
        _cW_PackageUnits = 34,
        _cW_PackageKg = 180,
        _value = 51952.68,
        _invoiceNo = "82222800",
        _accountClosed = false
      };
      listOfAccounts.Add(_newCW);
      _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        _cW_AddedKg = 0,
        _cW_DeclaredNetMass = 1800,
        _cW_SettledGrossMass = 1950,
        _cW_PackageToClear = 10,
        _tobaccoValue = 15280.2,
        _customsProcedure = "4071",
      };
      listOfDisposals.Add(_newCWD);
      _newCW = new CustomsWarehouse()
      {
        _documentNo = "OGL/362010/00/016193/2014",
        _grade = "XIDSME",
        _sKU = "12607453",
        _batch = "0003808069",
        _accountBalance = 4140,
        _tobaccoNotAllocated = 4140,
        _netMass = 4140,
        _grossMass = 4485,
        _cW_PackageUnits = 23,
        _cW_PackageKg = 180,
        _value = 35144.46,
        _invoiceNo = "82223962",
        _accountClosed = false
      };
      listOfAccounts.Add(_newCW);
      _newCW = new CustomsWarehouse()
      {
        _documentNo = "OGL/362010/00/017253/2014",
        _grade = "XIDSME",
        _sKU = "12607453",
        _batch = "0003808069",
        _accountBalance = 8820,
        _tobaccoNotAllocated = 8820,
        _netMass = 8820,
        _grossMass = 9555,
        _cW_PackageUnits = 49,
        _cW_PackageKg = 180,
        _value = 74872.98,
        _invoiceNo = "82225744",
        _accountClosed = false
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
    }
  }
}
