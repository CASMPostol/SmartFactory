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
      groupOfDisposals = listOfDisposals.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch);
      CustomsWarehouse _newCW = new CustomsWarehouse()
      {
        //TODO
      };
      listOfAccounts.Add(_newCW);
      CustomsWarehouseDisposal _newCWD = new CustomsWarehouseDisposal()
      {
        CWL_CWDisposal2CustomsWarehouseID = _newCW,
        //TODO
      };
      listOfDisposals.Add(_newCWD);
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
