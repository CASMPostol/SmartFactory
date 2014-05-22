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
    /// <summary>
    ///A test for FinishedGoodsExportFormFileName
    ///</summary>
    [TestMethod()]
    public void FinishedGoodsExportFormFileNameTest()
    {
      string m_Pattern = @"\bP\w+\st\w+\s+(\d{7})";
      int number = new Random().Next(1, 9999999);
      string _pattern = "Proces technologiczny {0:D7}"; // Settings.FinishedGoodsExportFormFileName( number );
      string _documentName = String.Format(_pattern, number);
      Nullable<int> actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_documentName, m_Pattern);
      Assert.AreEqual(number, actual);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber("Proces xechnologiczny 1234567", m_Pattern);
      Assert.IsFalse(actual.HasValue);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber("Proces Technologiczny 123456", m_Pattern);
      Assert.IsFalse(actual.HasValue);
    }
    /// <summary>
    ///A test for FinishedGoodsExportFormFileName
    ///</summary>
    [TestMethod()]
    public void CWWyprowadzenieFileNameTest()
    {
      string m_Pattern = @"\bC\w+\sW\w+\s+(\d{7})";
      int number = new Random().Next(1, 9999999);
      string _pattern = "CW Wyprowadzenie {0:D7}"; // Settings.FinishedGoodsExportFormFileName( number );
      string _documentName = String.Format(_pattern, number);
      Nullable<int> actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_documentName, m_Pattern);
      Assert.AreEqual(number, actual);
      _pattern = "CW    Wyprowadzenie     {0:D7}"; // Settings.FinishedGoodsExportFormFileName( number );
      _documentName = String.Format(_pattern, number);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_documentName, m_Pattern);
      Assert.AreEqual(number, actual);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber("CW xyprowadzenie 1234567", m_Pattern);
      Assert.IsFalse(actual.HasValue);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber("CW xyprowadzenie 123456", m_Pattern);
      Assert.IsFalse(actual.HasValue);
      actual = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber("CW xyprowadzenie1234567", m_Pattern);
      Assert.IsFalse(actual.HasValue);
    }
  }
}
