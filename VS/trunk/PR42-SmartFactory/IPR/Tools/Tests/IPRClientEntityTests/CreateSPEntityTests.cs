using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.SharePoint.Client.Reflection;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Tests
{
  /// <summary>
  /// Summary description for CreateSPEntityTests
  /// </summary>
  [TestClass]
  public class CreateSPEntityTests
  {
    public CreateSPEntityTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

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
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void Createsku()
    {
      Dictionary<string, Type> _types = typeof(SKUCommonPart).GetDerivetTypes();
      Assert.AreEqual<int>(3, _types.Count);
      foreach (var _item in _types)
      {
        System.Reflection.ConstructorInfo _constructor = _item.Value.GetConstructor(new Type[0]);
        Assert.IsNotNull(_constructor);
        var _obj = _constructor.Invoke(new object[0]);
        Assert.IsNotNull(_obj);
      }
    }
    [TestMethod]
    public void GetContentTypeAttributeTest()
    {
      ContentTypeAttribute _type = typeof(SKUCommonPart).GetContentType();
      Assert.AreEqual<string>("0x010014C98F440FB04C679F1D9D39ACC92D8A", _type.Id);
      _type = typeof(SKUCigarette).GetContentType();
      Assert.AreEqual<string>("0x010014C98F440FB04C679F1D9D39ACC92D8A0029E2E9BC132C4633B01B0DF733A64ADA", _type.Id);
      _type = typeof(SKUCutfiller).GetContentType();
      Assert.AreEqual<string>("0x010014C98F440FB04C679F1D9D39ACC92D8A00AE74B7010F534DB980D221D41D0BCBA1", _type.Id);
    }
  }
}
