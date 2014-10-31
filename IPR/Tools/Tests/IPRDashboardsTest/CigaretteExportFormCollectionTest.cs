using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using CAS.SmartFactory.IPR.Dashboards.Clearance;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlCigaretteExportForm = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm;

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
      double _quantity = 1000;
      double _portion = 0.5;
      SKUCigarette _sku = new SKUCigarette()
      {
        FormatIndex = new Format() { Title = "Format title", CigaretteLenght = "100", FilterLenght = "20" },
        Brand = "Slims Menthol ",
        Family = "Salem",
        CigaretteLenght = "99.00 mm ",
        FilterLenght = "27.00 mm",
        IPRMaterial = true,
        Menthol = "M",
        MentholMaterial = true,
        PrimeMarket = "PL",
        ProductType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette,
        SKU = "12419574 ",
        Title = "SKU SA MXSR  99 CPB 20  5000 05  N PL1"
      };
      Batch _batch = new Batch()
      {
        Batch0 = "99999999",
        BatchLibraryIndex = null,
        BatchStatus = BatchStatus.Final,
        CalculatedOveruse = 0,
        DustCooeficiencyVersion = 1,
        BatchDustCooeficiency = 1,
        Dust = 1.1,
        FGQuantityAvailable = 1000,
        FGQuantity = _quantity,
        MaterialQuantity = 5000.1234567,
        MaterialQuantityPrevious = 0,
        Overuse = 0,
        ProductType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette,
        SHCooeficiencyVersion = 1,
        BatchSHCooeficiency = 0.01234567,
        SHMenthol = 1.3,
        SKU = "SKU1234567890",
        SKUIndex = _sku,
        WasteCooeficiencyVersion = 1,
        Waste = 1.3,
        BatchWasteCooeficiency = 0.012345678,
        Title = "Testing batch"
      };
      _batch.Tobacco = _batch.MaterialQuantity.Value - _batch.SHMenthol.Value - _batch.Dust.Value - _batch.Waste.Value;
      InvoiceContent invoice = new InvoiceContent()
      {
        InvoiceContent2BatchIndex = _batch,
        InvoiceIndex = null,
        ProductType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette,
        Quantity = _quantity * _portion,
        SKUDescription = _batch.SKU,
        InvoiceContentStatus = InvoiceContentStatus.OK,
        Title = "Testing Invoice",
        Units = "kU",
      };
      List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient> ingridients = new List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient>();
      IPR _ipr1 = new IPR()
      {
        AccountBalance = 0,
        AccountClosed = false,
        Batch = "TobaccoBatch",
        Cartons = 0,
        ClearenceIndex = null,
        ClosingDate = DateTime.Today,
        IPR2ConsentTitle = null,
        Currency = "PLN",
        CustomsDebtDate = DateTime.Today,
        DocumentNo = "SADDocumentNomber",
        Duty = 123.45,
        DutyName = "DustyName",
        IPRDutyPerUnit = 67.89,
        Grade = "GradeName",
        GrossMass = 12345.67,
        InvoiceNo = "InvoiceNo",
        IPRLibraryIndex = null,
        IPR2JSOXIndex = null,
        NetMass = 23456.78,
        OGLValidTo = DateTime.Today + TimeSpan.FromDays( 364 ),
        IPR2PCNPCN = null,
        SKU = "IPR Tobacco SKU",
        TobaccoName = "TobaccoName",
        TobaccoNotAllocated = 9876.54,
        IPRUnitPrice = 5.67,
        Title = "Testing IPR",
        Value = 89012.34,
        VAT = 56.78,
        VATName = "VATName",
        IPRVATPerUnit = 9.12
      };
      IPR _ipr2 = new IPR()
      {
        AccountBalance = 0,
        AccountClosed = false,
        Batch = "TobaccoBatch",
        Cartons = 0,
        ClearenceIndex = null,
        ClosingDate = DateTime.Today,
        IPR2ConsentTitle = null,
        Currency = "USD",
        CustomsDebtDate = DateTime.Today,
        DocumentNo = "SADDocumentNomber",
        Duty = 123.45,
        DutyName = "DustyName",
        IPRDutyPerUnit = 67.89,
        Grade = "GradeName",
        GrossMass = 12345.67,
        InvoiceNo = "InvoiceNo",
        IPRLibraryIndex = null,
        IPR2JSOXIndex = null,
        NetMass = 23456.78,
        OGLValidTo = DateTime.Today + TimeSpan.FromDays( 364 ),
        IPR2PCNPCN = null,
        SKU = "IPR Tobacco SKU",
        TobaccoName = "TobaccoName",
        TobaccoNotAllocated = 9876.54,
        IPRUnitPrice = 5.67,
        Title = "Testing IPR",
        Value = 89012.34,
        VAT = 56.78,
        VATName = "VATName",
        IPRVATPerUnit = 9.12
      };
      Disposal _disposal1 = new Disposal()
      {
        Disposal2BatchIndex = _batch,
        Disposal2ClearenceIndex = null,
        ClearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp,
        CustomsProcedure = "5100",
        CustomsStatus = CustomsStatus.NotStarted,
        DisposalStatus = DisposalStatus.TobaccoInCigaretes,
        DutyAndVAT = 123.4321,
        DutyPerSettledAmount = 345.6789,
        IPRDocumentNo = null,
        InvoiceNo = "InvoiceNomber",
        Disposal2IPRIndex = _ipr1,
        JSOXCustomsSummaryIndex = null,
        Disposal2MaterialIndex = null,
        SPNo = 7.8,
        RemainingQuantity = 0,
        SADDate = new Nullable<DateTime>(),
        SADDocumentNo = "N/A",
        SettledQuantity = 9.12,
        TobaccoValue = 34.567,
        Title = "Testing disposal",
        VATPerSettledAmount = 78.901,
      };
      Disposal _disposal2 = new Disposal()
      {
        Disposal2BatchIndex = _batch,
        Disposal2ClearenceIndex = null,
        ClearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp,
        CustomsProcedure = "5100",
        CustomsStatus = CustomsStatus.NotStarted,
        DisposalStatus = DisposalStatus.TobaccoInCigaretes,
        DutyAndVAT = 123.4567,
        DutyPerSettledAmount = 345.67891234,
        IPRDocumentNo = null,
        InvoiceNo = "InvoiceNomber",
        Disposal2IPRIndex = _ipr2,
        JSOXCustomsSummaryIndex = null,
        Disposal2MaterialIndex = null,
        SPNo = 7.8,
        RemainingQuantity = 0,
        SADDate = new Nullable<DateTime>(),
        SADDocumentNo = "N/A",
        SettledQuantity = 9.12345678,
        TobaccoValue = 34.56789012,
        Title = "Testing disposal",
        VATPerSettledAmount = 78.9012345,
      };
      CutfillerCoefficient _cc = new CutfillerCoefficient
      {
        CFTProductivityNormMax = 995,
        CFTProductivityNormMin = 985,
        CFTProductivityRateMax = 0.995,
        CFTProductivityRateMin = 0.985
      };
      ingridients.Add( CAS.SmartFactory.IPR.Dashboards.Clearance.FinishedGoodsFormFactory.GetIPRIngredient( _disposal1 ) );
      ingridients.Add( CAS.SmartFactory.IPR.Dashboards.Clearance.FinishedGoodsFormFactory.GetIPRIngredient( _disposal2 ) );
      ingridients.Add( new CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.RegularIngredient( "Reg Batch 54321", "Reg SKU 12345", 1234.56789 ) );
      //string _masterDocumentName = "CigaretteExportFormCollection";
      //string _invoiceNumber = "INV987654";
      //int _position = 1;
      //List<XmlCigaretteExportForm> cigaretteExportFormList = new List<XmlCigaretteExportForm>();
      //CigaretteExportForm _cigaretteExportForm = FinishedGoodsFormFactory.GetCigaretteExportForm( _cc, _batch, invoice, 0.5, ingridients, _masterDocumentName, ref _position, ClearenceProcedure._3151 );
      //cigaretteExportFormList.Add( _cigaretteExportForm );
      //_cigaretteExportForm = FinishedGoodsFormFactory.GetCigaretteExportForm( _cc, _batch, invoice, 0.5, ingridients, _masterDocumentName, ref _position, ClearenceProcedure._4071 );
      //cigaretteExportFormList.Add( _cigaretteExportForm );
      //CigaretteExportFormCollection target = FinishedGoodsFormFactory.GetCigaretteExportFormCollection( cigaretteExportFormList, _masterDocumentName, _invoiceNumber );
      //XmlSerializer _srlzr = new XmlSerializer( typeof( CigaretteExportFormCollection ) );
      //XmlWriterSettings _setting = new XmlWriterSettings()
      //{
      //  Indent = true,
      //  IndentChars = "  ",
      //  NewLineChars = "\r\n"
      //};
      //using ( XmlWriter file = XmlWriter.Create( _masterDocumentName + ".xml", _setting ) )
      //{
      //  file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" href=\"CigaretteExportFormCollection.xslt\"" );
      //  _srlzr.Serialize( file, target );
      //  Assert.IsTrue( true, "Success" );
      //}
    }
  }
}
