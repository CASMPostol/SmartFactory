using CAS.SmartFactory.Linq.IPR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XmlCigaretteExportForm = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm;
using System.Collections.Generic;

namespace IPRDashboardsTest
{
    
    
    /// <summary>
    ///This is a test class for CigaretteExportFormCollectionTest and is intended
    ///to contain all CigaretteExportFormCollectionTest Unit Tests
    ///</summary>
  [TestClass()]
  public class CigaretteExportFormCollectionTest
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
    ///A test for CigaretteExportFormCollection Constructor
    ///</summary>
    [TestMethod()]
    public void CigaretteExportFormCollectionConstructorTest()
    {
      List<XmlCigaretteExportForm> cigaretteExportForms = null; // TODO: Initialize to an appropriate value
      CigaretteExportFormCollection target = new CigaretteExportFormCollection( cigaretteExportForms );
      Assert.Inconclusive( "TODO: Implement code to verify target" );
    }
  }
}
