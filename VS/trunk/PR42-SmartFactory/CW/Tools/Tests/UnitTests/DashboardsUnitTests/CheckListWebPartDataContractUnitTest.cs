using System;
using CAS.SmartFactory.CW.Dashboards.Webparts.CheckListHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.SmartFactory.CW.UnitTests.DashboardsUnitTests
{
  [TestClass]
  public class CheckListWebPartDataContractUnitTest
  {
    [TestMethod]
    public void CheckListWebPartTestMethod()
    {
      System.Collections.Generic.List<DisposalDescription> _list = new System.Collections.Generic.List<DisposalDescription>();
      DisposalDescription _ndd = new DisposalDescription()
      {
        OGLDate = DateTime.Today,
        OGLNumber = "hkjashkjhkjshakhs",
        PackageToClear = 12345
      };
      _list.Add( _ndd );
      _ndd = new DisposalDescription()
      {
        OGLDate = DateTime.Today,
        OGLNumber = "hkjashkjhkjshakhs",
        PackageToClear = 12345
      };
      _list.Add( _ndd );
      CheckListWebPartDataContract _newObject = new CheckListWebPartDataContract()
      {
        Today = DateTime.Today,
        DisposalsList = _list.ToArray()
      };
      string _cont = _newObject.Serialize();
      CheckListWebPartDataContract _dsrlzd = CheckListWebPartDataContract.Deserialize( _cont );
      Assert.AreEqual<DateTime>( _newObject.Today, _dsrlzd.Today, "Deserialization failed" );
    }
  }
}
