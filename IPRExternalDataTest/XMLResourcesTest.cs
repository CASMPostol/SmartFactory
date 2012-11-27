using System;
using CAS.SmartFactory.xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPRExternalDataTest
{
  /// <summary>
  ///This is a test class for XMLResourcesTest and is intended
  ///to contain all XMLResourcesTest Unit Tests
  ///</summary>
  [TestClass()]
  public class XMLResourcesTest
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
    private const string m_Pattern = @"\bP\w+\s+t\w+\s+(\d{7})";
    /// <summary>
    ///A test for FinishedGoodsExportFormFileName
    ///</summary>
    [TestMethod()]
    public void FinishedGoodsExportFormFileNameTest()
    {
      int number = new Random().Next( 1, 9999999 ); 
      string documentName = XMLResources.FinishedGoodsExportFormFileName( number );
      Nullable<int> actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( documentName, m_Pattern );
      Assert.AreEqual( number, actual );
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( "sasa sasasa sasasa", m_Pattern );
      Assert.IsFalse( actual.HasValue );
    }
  }
}
