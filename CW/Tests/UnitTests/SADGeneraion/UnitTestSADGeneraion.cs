using System;
using CAS.SmartFactory.Customs.Messages.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.UnitTests.SADGeneraion
{
  [TestClass]
  public class UnitTestSADGeneraion
  {
    [TestMethod]
    public void CheckSerialization()
    {
      Organization _new = new Organization()
      {
        Id = 1,
        Nazwa = "JTI POLSKA SP. Z O.O.",
        UlicaNr = "GOSTKOW STARY 42",
        Kod = "99-220",
        Miejscowosc = "WARTKOWICE",
        Kraj = "PL",
        TIN = "PL8280001819",
        Regon = "00130199100000",
        EORI = "PL828000181900000"
      };
      string _out = _new.Serialize();
      Organization _copy = Organization.Deserialize( _out );
      Assert.AreEqual<string>( _new.Nazwa, _copy.Nazwa, "Serialization failed - object are not the same." );
    }
    [TestMethod]
    public void CheckSerializationSender()
    {
      Organization _new = new Organization()
      {
        Id = 1,
        Nazwa = "JT INTERNATIONAL SA A MEMBER OF THE",
        UlicaNr = "1,RUE DE LA GABELLE",
        Kod = "1211",
        Miejscowosc = "GENEVA",
        Kraj = "CH"
      };
      string _out = _new.Serialize();
      Organization _copy = Organization.Deserialize( _out );
      Assert.AreEqual<string>( _new.Nazwa, _copy.Nazwa, "Serialization failed - object are not the same." );
    }
    private string ObjectSerialized = @"{""EORI"":""PL828000181900000"",""Id"":1,""Kod"":""99-220"",""Kraj"":""PL"",""Miejscowosc"":""WARTKOWICE"",""Nazwa"":""JTI POLSKA SP. Z O.O."",""Pesel"":null,""Regon"":""00130199100000"",""TIN"":""PL8280001819"",""UlicaNr"":""GOSTKOW STARY 42""}";
    private Organization ObjectNotSerialized = new Organization()
      {
        Id = 1,
        Nazwa = "JTI POLSKA SP. Z O.O.",
        UlicaNr = "GOSTKOW STARY 42",
        Kod = "99-220",
        Miejscowosc = "WARTKOWICE",
        Kraj = "PL",
        TIN = "PL8280001819",
        Regon = "00130199100000",
        EORI = "PL828000181900000"
      };
    [TestMethod]
    public void CheckStringContent()
    {
      string _out = ObjectNotSerialized.Serialize();
      Organization _copy = Organization.Deserialize( ObjectSerialized );
      Assert.AreEqual<string>( ObjectNotSerialized.Nazwa, _copy.Nazwa, "Serialization failed - object are not the same." );
    }
  }
}
