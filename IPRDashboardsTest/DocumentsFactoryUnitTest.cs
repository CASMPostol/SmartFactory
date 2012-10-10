using System;
using System.Linq;
using System.Collections.Generic;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.SharePoint;

namespace IPRDashboardsTest
{
  [TestClass]
  public class DocumentsFactoryUnitTest
  {

    [TestMethod]
    public void DustWasteFormFactoryTestMethod()
    {
      IPR _ipr1 = new IPR()
      {
        Currency = "EUR",
        DocumentNo = "OGL/362010/00/001308/2012 ",
        GrossMass = 430,
        IPRUnitPrice = 7.16,
        NetMass = 400,
        Title = "IPR - 2012000014"
      };
      List<Disposal> _disposals = new List<Disposal>();
      _disposals.Add(
        ( new Disposal()
        {
          ClearenceIndex = null,
          ClearingType = ClearingType.PartialWindingUp,
          CustomsProcedure = String.Empty,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr1,
          Disposal2MaterialIndex = null,
          Disposal2PCNCompensationGood = null,
          DisposalStatus = DisposalStatus.Dust,
          DutyAndVAT = 0,
          DutyPerSettledAmount = 0,
          InvoiceNo = String.Empty,
          IPRDocumentNo = string.Empty,
          JSOXCustomsSummaryIndex = null,
          No = 0,
          RemainingQuantity = 0,
          SADDate = Extensions.DateTimeNull,
          SADDocumentNo = String.Empty,
          SettledQuantity = 123.56,
          Title = "Disposal: Dust of FG Cigarette SKU: 12650149; Batch: 0007444111",
          TobaccoValue = 654.321,
          VATPerSettledAmount = 5678.987,
        }
        ) );
      DocumentContent _newDoc = DustWasteFormFactory.GetDocumentContent( _disposals.AsQueryable<Disposal>(), "4051", "OGL Number" );
      Assert.AreEqual( _newDoc.DocumentNo, "OGL Number" );
      ;
    }
  }
}
