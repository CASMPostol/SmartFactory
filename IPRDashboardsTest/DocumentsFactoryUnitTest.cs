using System;
using System.Linq;
using System.Collections.Generic;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IPRDashboardsTest
{
  [TestClass]
  public class DocumentsFactoryUnitTest
  {

    [TestMethod]
    public void DustWasteFormFactoryTestMethod()
    {
      List<Disposal> _disposals = new List<Disposal>();
      DocumentContent _newDoc = DustWasteFormFactory.GetDocumentContent(_disposals.AsQueryable<Disposal>(), "4051", "OGL Number");
    }
  }
}
