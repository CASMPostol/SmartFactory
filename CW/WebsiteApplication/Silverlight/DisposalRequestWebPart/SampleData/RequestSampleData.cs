
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.SampleData
{
  /// <summary>
  /// RequestSampleData provides sample data for the testing purpose
  /// </summary>
  public static class RequestSampleData
  {
    /// <summary>
    /// Gets the sample data.
    /// </summary>
    /// <param name="listOfAccounts">The list of accounts.</param>
    /// <param name="groupOfDisposals">The group of disposals.</param>
    public static void GetData(List<CustomsWarehouse> listOfAccounts, out IGrouping<string, CustomsWarehouseDisposal> groupOfDisposals)
    {
      List<CustomsWarehouseDisposal> listOfDisposals = new List<CustomsWarehouseDisposal>();
      groupOfDisposals = null;
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
        CustomsStatus = CustomsStatus.NotStarted
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
        AccountBalance = 4140,
        TobaccoNotAllocated = 4140,
        NetMass = 4140,
        GrossMass = 4485,
        CW_PackageUnits = 23,
        CW_MassPerPackage = 180,
        Value = 35144.46,
        InvoiceNo = "82222800",
        AccountClosed = false,
        Units = "m3",
      };
      listOfAccounts.Add(_newCW);
      _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        CW_AddedKg = 0,
        CW_DeclaredNetMass = 0,
        CW_SettledGrossMass = 0,
        CW_PackageToClear = 0,
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
        AccountBalance = 6120,
        TobaccoNotAllocated = 4320,
        NetMass = 6120,
        GrossMass = 6630,
        CW_PackageUnits = 34,
        CW_MassPerPackage = 180,
        Value = 51952.68,
        InvoiceNo = "82223962",
        AccountClosed = false,
        Units = "m4",
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
        AccountClosed = false,
        Units = "m5",
      };
      listOfAccounts.Add(_newCW);
      groupOfDisposals = listOfDisposals.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch).First < IGrouping<string, CustomsWarehouseDisposal>>();
    }

  }
}
