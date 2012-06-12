using CAS.SmartFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace IPRWorkflowsUnitTests
{
    
    
    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
  [TestClass()]
  public class ExtensionsTest
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
    ///A test for SPValidSubstring
    ///</summary>
    [TestMethod()]
    public void SPValidSubstringTest()
    {
      string _value = "TYTON ODZYLOWANY BRAZIL FLUE CURED VIRGINIA STRIPS                                                                                 GRADE:XBRFCB SKU:12352149 BATCH:0003814564                                       czesciowa likwidacja OGL/362010/00/002494/12";
      string _cchar = "\0x10";
      _value.Insert(10, _cchar);
      string actual;
      actual = Extensions.SPValidSubstring(_value);
      Assert.IsFalse(actual.Contains("  "), "Text should not contain double spaces");
      Assert.IsTrue(actual.Length <= 254, "Text is too long");
      Assert.IsTrue(actual.Min<char>( ) >= 0x20, "Text is too long");
    }
  }
}
