using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPRWorkflowsUnitTests
{

  /// <summary>
  ///This is a test class for BatchTest and is intended
  ///to contain all BatchTest Unit Tests
  ///</summary>
  [TestClass()]
  public class BatchTest
  {
    private TestContext testContextInstance;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for GetOverusage
    ///</summary>
    [TestMethod()]
    [DeploymentItem( "CAS.IPRWorkflows.dll" )]
    public void GetOverusageTest()
    {
      //double _materialQuantity = 669.873;
      //double _fGQuantity = 1530;
      //double _ctfUsageMax = 424;
      //double _ctfUsageMin = 394;
      //double expected = 0.03157762740101485;
      //double actual;
      //actual = GetOverusage(_materialQuantity, _fGQuantity, _ctfUsageMax, _ctfUsageMin);
      //Assert.AreEqual(expected, actual);
      //_materialQuantity = 5045;
      //_fGQuantity = 5367;
      //_ctfUsageMax = 10000;
      //_ctfUsageMin = 0;
      //expected = 0;
      //actual = GetOverusage(_materialQuantity, _fGQuantity, _ctfUsageMax, _ctfUsageMin);
      //Assert.AreEqual(expected, actual);
      //_materialQuantity = 5045;
      //_fGQuantity = 5367;
      //_ctfUsageMax = 10000;
      //_ctfUsageMin = 1000;
      //expected = -0.06382556987115956;
      //actual = GetOverusage(_materialQuantity, _fGQuantity, _ctfUsageMax, _ctfUsageMin);
      //Assert.AreEqual(expected, actual);
      //_materialQuantity = 5045;
      //_fGQuantity = 5367;
      //_ctfUsageMax = 1200;
      //_ctfUsageMin = 900;
      //expected = 0;
      //actual = GetOverusage(_materialQuantity, _fGQuantity, _ctfUsageMax, _ctfUsageMin);
      //Assert.AreEqual(expected, actual);
    }
  }
}
