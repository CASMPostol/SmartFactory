using CAS.SmartFactory.IPR.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
    [DeploymentItem("CAS.SmartFactory.IPR.dll")]
    public void GetOverusageTest()
    {
      Nullable<double> _materialQuantity = 669.873;
      Nullable<double> _fGQuantity = 1530;
      Nullable<double> _ctfUsageMax = 424;
      Nullable<double> _ctfUsageMin = 394; 
      double expected = 0.03157762740101485; // TODO: Initialize to an appropriate value
      double actual;
      actual = Batch_Accessor.GetOverusage(_materialQuantity, _fGQuantity, _ctfUsageMax, _ctfUsageMin);
      Assert.AreEqual(expected, actual);
    }
  }
}
