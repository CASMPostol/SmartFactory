using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards.Clearance;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
        Currency = "USD",
        DocumentNo = "OGL/362010/00/001295/2012",
        GrossMass = 854,
        IPRUnitPrice = 8.06,
        NetMass = 800,
        Title = "IPR-2012000003"
      };
      IPR _ipr2 = new IPR()
      {
        Currency = "USD",
        DocumentNo = "OGL/362010/00/001300/2012",
        GrossMass = 428,
        IPRUnitPrice = 8.24,
        NetMass = 400,
        Title = "IPR-2012000006",
      };
      IPR _ipr3 = new IPR()
      {
        Currency = "USD",
        DocumentNo = "OGL/362010/00/001302/2012",
        GrossMass = 1025,
        IPRUnitPrice = 6.73,
        NetMass = 950,
        Title = "IPR-2012000008",
      };
      IPR _ipr4 = new IPR()
      {
        Currency = "USD",
        DocumentNo = "OGL/362010/00/001303/2012",
        GrossMass = 780,
        IPRUnitPrice = 5.06,
        NetMass = 720,
        Title = "IPR-2012000009",
      };
      List<Disposal> _disposals = new List<Disposal>();
      _disposals.Add(
        (new Disposal()
        {
          Disposal2ClearenceIndex = null,
          ClearingType = ClearingType.PartialWindingUp,
          Created = DateTime.Now,
          CustomsProcedure = String.Empty,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr1,
          Disposal2MaterialIndex = null,
          Disposal2PCNID = null,
          DisposalStatus = DisposalStatus.Dust,
          DutyAndVAT = 392.70,
          DutyPerSettledAmount = 125.992,
          InvoiceNo = String.Empty,
          IPRDocumentNo = string.Empty,
          JSOXCustomsSummaryIndex = null,
          SPNo = 0,
          RemainingQuantity = 0,
          SADDate = Extensions.SPMinimum,
          SADDocumentNo = String.Empty,
          SettledQuantity = 117.75,
          Title = "Disposal: Dust of FG Cigarette SKU: 12650149; Batch: 0007444111",
          TobaccoValue = 949.418,
          VATPerSettledAmount = 266.704,
        }
        ));
      _disposals.Add(
        (new Disposal()
        {
          Disposal2ClearenceIndex = null,
          ClearingType = ClearingType.PartialWindingUp,
          CustomsProcedure = String.Empty,
          Created = DateTime.Now,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr1,
          Disposal2MaterialIndex = null,
          Disposal2PCNID = null,
          DisposalStatus = DisposalStatus.Waste,
          DutyAndVAT = 13.84,
          DutyPerSettledAmount = 4.44,
          InvoiceNo = String.Empty,
          IPRDocumentNo = String.Empty,
          JSOXCustomsSummaryIndex = null,
          SPNo = 0,
          RemainingQuantity = 0,
          SADDate = Extensions.SPMinimum,
          SADDocumentNo = String.Empty,
          SettledQuantity = 4.15,
          Title = "Disposal: Waste of FG Cigarette SKU: 12650149; Batch: 0007444111",
          TobaccoValue = 33.461,
          VATPerSettledAmount = 9.4,
        }
        ));
      _disposals.Add(
          (new Disposal()
          {
            Disposal2ClearenceIndex = null,
            ClearingType = ClearingType.PartialWindingUp,
            CustomsProcedure = String.Empty,
            CustomsStatus = CustomsStatus.NotStarted,
            Created = DateTime.Now,
            Disposal2BatchIndex = null,
            Disposal2IPRIndex = _ipr2,
            Disposal2MaterialIndex = null,
            Disposal2PCNID = null,
            DisposalStatus = DisposalStatus.Dust,
            DutyAndVAT = 30.65,
            DutyPerSettledAmount = 0,
            InvoiceNo = String.Empty,
            IPRDocumentNo = String.Empty,
            JSOXCustomsSummaryIndex = null,
            SPNo = 0,
            RemainingQuantity = 0,
            SADDate = Extensions.SPMinimum,
            SADDocumentNo = String.Empty,
            Title = "Disposal: Dust of FG Cigarette SKU: 12988459; Batch: 0007403609",
            TobaccoValue = 113.327,
            SettledQuantity = 13.76,
            VATPerSettledAmount = 30.65,
          }
          ));
      _disposals.Add(
          (new Disposal()
          {
            Disposal2ClearenceIndex = null,
            ClearingType = ClearingType.PartialWindingUp,
            CustomsProcedure = String.Empty,
            CustomsStatus = CustomsStatus.NotStarted,
            Created = DateTime.Now,
            Disposal2BatchIndex = null,
            Disposal2IPRIndex = _ipr2,
            Disposal2MaterialIndex = null,
            Disposal2PCNID = null,
            DisposalStatus = DisposalStatus.Waste,
            DutyAndVAT = 1.09,
            DutyPerSettledAmount = 0,
            InvoiceNo = String.Empty,
            IPRDocumentNo = String.Empty,
            JSOXCustomsSummaryIndex = null,
            SPNo = 0,
            RemainingQuantity = 0,
            SADDate = Extensions.SPMinimum,
            SADDocumentNo = String.Empty,
            Title = "Disposal: Waste of FG Cigarette SKU: 12988459; Batch: 0007403609",
            TobaccoValue = 4.036,
            SettledQuantity = 0.49,
            VATPerSettledAmount = 1.091,
          }
          ));
      _disposals.Add(
          (new Disposal()
          {
            Disposal2ClearenceIndex = null,
            ClearingType = ClearingType.PartialWindingUp,
            CustomsProcedure = String.Empty,
            CustomsStatus = CustomsStatus.NotStarted,
            Created = DateTime.Now,
            Disposal2BatchIndex = null,
            Disposal2IPRIndex = _ipr3,
            Disposal2MaterialIndex = null,
            Disposal2PCNID = null,
            DisposalStatus = DisposalStatus.Dust,
            DutyAndVAT = 48.19,
            DutyPerSettledAmount = 17.343,
            InvoiceNo = String.Empty,
            IPRDocumentNo = String.Empty,
            JSOXCustomsSummaryIndex = null,
            SPNo = 0,
            RemainingQuantity = 0,
            SADDate = Extensions.SPMinimum,
            SADDocumentNo = String.Empty,
            Title = "Disposal: Dust of FG Cigarette SKU: 12988459; Batch: 0007403609",
            TobaccoValue = 108.977,
            SettledQuantity = 16.2,
            VATPerSettledAmount = 30.848,
          }
          ));
      _disposals.Add(
          (new Disposal()
          {
            Disposal2ClearenceIndex = null,
            ClearingType = ClearingType.PartialWindingUp,
            CustomsProcedure = String.Empty,
            CustomsStatus = CustomsStatus.NotStarted,
            Created = DateTime.Now,
            Disposal2BatchIndex = null,
            Disposal2IPRIndex = _ipr3,
            Disposal2MaterialIndex = null,
            Disposal2PCNID = null,
            DisposalStatus = DisposalStatus.Waste,
            DutyAndVAT = 1.73,
            DutyPerSettledAmount = 0.621,
            InvoiceNo = String.Empty,
            IPRDocumentNo = String.Empty,
            JSOXCustomsSummaryIndex = null,
            SPNo = 0,
            RemainingQuantity = 0,
            SADDate = Extensions.SPMinimum,
            SADDocumentNo = String.Empty,
            Title = "Disposal: Waste of FG Cigarette SKU: 12988459; Batch: 0007403609",
            TobaccoValue = 3.902,
            SettledQuantity = 0.58,
            VATPerSettledAmount = 1.104,
          }
          ));
      _disposals.Add(
        (new Disposal()
        {
          Disposal2ClearenceIndex = null,
          ClearingType = ClearingType.PartialWindingUp,
          Created = DateTime.Now,
          CustomsProcedure = String.Empty,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr4,
          Disposal2MaterialIndex = null,
          Disposal2PCNID = null,
          DisposalStatus = DisposalStatus.Dust,
          DutyAndVAT = 6.18,
          DutyPerSettledAmount = 1.973,
          InvoiceNo = String.Empty,
          IPRDocumentNo = String.Empty,
          JSOXCustomsSummaryIndex = null,
          SPNo = 0,
          RemainingQuantity = 0,
          SADDate = Extensions.SPMinimum,
          SADDocumentNo = String.Empty,
          Title = "Disposal: Dust of FG Cigarette SKU: 12988459; Batch: 0007403609",
          TobaccoValue = 14.975,
          SettledQuantity = 2.96,
          VATPerSettledAmount = 4.206,
        }
        ));
      _disposals.Add(
        (new Disposal()
        {
          Disposal2ClearenceIndex = null,
          ClearingType = ClearingType.PartialWindingUp,
          Created = DateTime.Now,
          CustomsProcedure = String.Empty,
          CustomsStatus = CustomsStatus.NotStarted,
          Disposal2BatchIndex = null,
          Disposal2IPRIndex = _ipr4,
          Disposal2MaterialIndex = null,
          Disposal2PCNID = null,
          DisposalStatus = DisposalStatus.Waste,
          DutyAndVAT = 0.23,
          DutyPerSettledAmount = 0.073,
          InvoiceNo = String.Empty,
          IPRDocumentNo = String.Empty,
          JSOXCustomsSummaryIndex = null,
          SPNo = 0,
          RemainingQuantity = 0,
          SADDate = Extensions.SPMinimum,
          SADDocumentNo = String.Empty,
          Title = "Disposal: Waste of FG Cigarette SKU: 12988459; Batch: 0007403609",
          TobaccoValue = 0.556,
          SettledQuantity = 0.11,
          VATPerSettledAmount = 0.156,
        }
        ));
      //TODO refers to environment: DocumentContent _newDoc = DisposalsFormFactory.GetDustWasteFormContent(_disposals.AsQueryable<Disposal>(), ClearenceProcedure._4051, "OGL Number");
      //Assert.AreEqual(_newDoc.DocumentNo, "OGL Number");
    }
  }
}
