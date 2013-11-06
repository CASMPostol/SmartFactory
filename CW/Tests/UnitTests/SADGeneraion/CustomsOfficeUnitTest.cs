using System;
using CAS.SmartFactory.Customs.Messages.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CW.UnitTests.SADGeneraion
{
  [TestClass]
  public class CustomsOfficeUnitTest
  {
    CustomsOffice NewObject = new CustomsOffice()
    {

      UCZgloszenia = "PL362010",
      UCGraniczny = "PL362010",
      Lokalizacja = new CustomsOfficeUCLokalizacja() { Miejsce = "PL360000SC0002" },
      SkladCelny = new CustomsOfficeUCSkladCelny() { Typ = "C", Miejsce = "PL360000SC0002", Kraj = "PL" }
    };
    string SerializedObject =
      @"{""Lokalizacja"":{""Miejsce"":""PL360000SC0002"",""Opis"":null,""UC"":null},""SkladCelny"":{""Kraj"":""PL"",""Miejsce"":""PL360000SC0002"",""Typ"":""C""},""UCGraniczny"":""PL362010"",""UCKontrolny"":null,""UCPrzeznaczenia"":null,""UCTranzytowy"":null,""UCZgloszenia"":""PL362010""}";
    [TestMethod]
    public void CustomsOfficeObjectTestMethod()
    {
      string _out = NewObject.Serialize();
      CustomsOffice _copy = CustomsOffice.Deserialize( _out );
      Assert.AreEqual<string>( NewObject.UCZgloszenia, _copy.UCZgloszenia, "Serialization failed - object are not the same." );
    }
    [TestMethod]
    public void CustomsOfficeStringTestMethod()
    {
      string _out = NewObject.Serialize();
      Assert.AreEqual<string>( SerializedObject, _out, "Serialization failed - object are not the same." );
    }
    [TestMethod]
    public void SADZgloszenieUCTestMethod()
    {
      CAS.SmartFactory.Customs.Messages.CELINA.SAD.SADZgloszenieUC customsOffice = CAS.SmartFactory.Customs.Messages.CELINA.SAD.SADZgloszenieUC.Create( SerializedObject );
    }
  }
}
