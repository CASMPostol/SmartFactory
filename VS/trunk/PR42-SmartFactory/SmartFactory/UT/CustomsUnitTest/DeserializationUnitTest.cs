using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.SmartFactory.Customs.UnitTest
{
  [TestClass]
  public class DeserializationUnitTest
  {
    [TestMethod]
    //TODO this doesn't pass if executed as "Run All" - the test data must be copied to the working directory. 
    public void DeserializationTestMethod()
    {
      SAD _sad = XmlFile.ReadXmlFile<SAD>(@"TestData\TestSAD.xml");
      Assert.IsNotNull(_sad);
      SADCollection _sc = new SADCollection() { ListOfSAD = new SAD[] {_sad, _sad}};
      XmlFile.WriteXmlFile<SADCollection>(_sc, @"TestData\TestSADCollection.xml", System.IO.FileMode.Create, "SADCollection.xls");
      SADCollection _new = XmlFile.ReadXmlFile<SADCollection>(@"TestData\TestSADCollection.xml");
      Assert.IsNotNull(_new);
    }
  }
}
