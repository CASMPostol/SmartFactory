using CAS.SmartFactory.Linq.IPR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XmlCigaretteExportForm = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Xml.Serialization;
using System.IO;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using System.Xml;

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
        FormatLookup = new Format() { Tytuł = "Format title", CigaretteLenght = "100", FilterLenght = "20" },
        Brand = "Slims Menthol ",
        Family = "Salem",
        CigaretteLenght = "99.00 mm ",
        FilterLenght = "27.00 mm",
        IPRMaterial = true,
        Menthol = "M",
        MentholMaterial = true,
        PrimeMarket = "PL",
        ProductType = CAS.SmartFactory.Linq.IPR.ProductType.Cigarette,
        SKU = "12419574 ",
        Tytuł = "SKU SA MXSR  99 CPB 20  5000 05  N PL1"
      };
      Batch _batch = new Batch()
      {
        Batch0 = "99999999",
        BatchLibraryLookup = null,
        BatchStatus = BatchStatus.Final,
        CalculatedOveruse = 0,
        CutfillerCoefficientLookup = null,
        DustCoeficiencyVersion = 1,
        DustCooeficiency = 1,
        DustKg = 1.1,
        DustLookup = null,
        FGQuantityAvailable = 1000,
        FGQuantityBlocked = 0,
        FGQuantityKUKg = _quantity,
        FGQuantityPrevious = 0,
        MaterialQuantity = 5000.1234567,
        MaterialQuantityPrevious = 0,
        OveruseKg = 0,
        ProductType = CAS.SmartFactory.Linq.IPR.ProductType.Cigarette,
        SHCoeficiencyVersion = 1,
        SHCooeficiency = 0.01234567,
        SHMentholKg = 1.3,
        SHMentholLookup = null,
        SKU = "SKU1234567890",
        SKULookup = _sku,
        StockEntry = null,
        WasteCoeficiencyVersion = 1,
        WasteKg = 1.3,
        UsageLookup = new Usage() { CTFUsageMax = 1.23, CTFUsageMin = 0.9876, UsageMax = 1234.45, UsageMin = 987.65, FormatLookup = null },
        WasteCooeficiency = 0.012345678,
        WasteLookup = null,
        Tytuł = "Testing batch"
      };
      _batch.TobaccoKg = _batch.MaterialQuantity.Value - _batch.SHMentholKg.Value - _batch.DustKg.Value - _batch.WasteKg.Value;
      InvoiceContent invoice = new InvoiceContent()
      {
        BatchID = _batch,
        InvoiceLookup = null,
        ProductType = CAS.SmartFactory.Linq.IPR.ProductType.Cigarette,
        Quantity = _quantity * _portion,
        SKUDescription = _batch.SKU,
        Status = Status.OK,
        Tytuł = "Testing Invoice",
        Units = "kU",
      };
      List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient> ingridients = new List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient>();
      IPR _ipr1 = new IPR()
      {
        AccountBalance = 0,
        AccountClosed = false,
        Batch = "TobaccoBatch",
        Cartons = 0,
        ClearenceListLookup = null,
        ClosingDate = DateTime.Today,
        ConsentNo = null,
        Currency = "PLN",
        CustomsDebtDate = DateTime.Today,
        DocumentNo = "SADDocumentNomber",
        Duty = 123.45,
        DutyName = "DustyName",
        DutyPerUnit = 67.89,
        Grade = "GradeName",
        GrossMass = 12345.67,
        InvoiceNo = "InvoiceNo",
        IPRLibraryLookup = null,
        JSOXListLookup = null,
        NetMass = 23456.78,
        OGLValidTo = DateTime.Today + TimeSpan.FromDays( 364 ),
        PCNTariffCode = null,
        SKU = "IPR Tobacco SKU",
        TobaccoName = "TobaccoName",
        TobaccoNotAllocated = 9876.54,
        UnitPrice = 5.67,
        Tytuł = "Testing IPR",
        Value = 89012.34,
        VAT = 56.78,
        VATName = "VATName",
        VATPerUnit = 9.12
      };
      IPR _ipr2 = new IPR()
      {
        AccountBalance = 0,
        AccountClosed = false,
        Batch = "TobaccoBatch",
        Cartons = 0,
        ClearenceListLookup = null,
        ClosingDate = DateTime.Today,
        ConsentNo = null,
        Currency = "USD",
        CustomsDebtDate = DateTime.Today,
        DocumentNo = "SADDocumentNomber",
        Duty = 123.45,
        DutyName = "DustyName",
        DutyPerUnit = 67.89,
        Grade = "GradeName",
        GrossMass = 12345.67,
        InvoiceNo = "InvoiceNo",
        IPRLibraryLookup = null,
        JSOXListLookup = null,
        NetMass = 23456.78,
        OGLValidTo = DateTime.Today + TimeSpan.FromDays( 364 ),
        PCNTariffCode = null,
        SKU = "IPR Tobacco SKU",
        TobaccoName = "TobaccoName",
        TobaccoNotAllocated = 9876.54,
        UnitPrice = 5.67,
        Tytuł = "Testing IPR",
        Value = 89012.34,
        VAT = 56.78,
        VATName = "VATName",
        VATPerUnit = 9.12
      };
      Disposal _disposal1 = new Disposal()
      {
        BatchLookup = _batch,
        ClearenceListLookup = null,
        ClearingType = CAS.SmartFactory.Linq.IPR.ClearingType.PartialWindingUp,
        CustomsProcedure = "5100",
        CustomsStatus = CustomsStatus.NotStarted,
        DisposalStatus = DisposalStatus.TobaccoInCigaretesWarehouse,
        DutyAndVAT = 123.4,
        DutyPerSettledAmount = 345.6,
        IPRDocumentNo = null,
        InvoiceNo = "InvoiceNomber",
        IPRID = _ipr1,
        JSOXCustomsSummaryListLookup = null,
        MaterialLookup = null,
        No = 7.8,
        RemainingQuantity = 0,
        SADDate = new Nullable<DateTime>(),
        SADDocumentNo = "N/A",
        SettledQuantity = 9.12,
        TobaccoValue = 34.56,
        Tytuł = "Testing disposal",
        VATPerSettledAmount = 78.90,
      };
      Disposal _disposal2 = new Disposal()
      {
        BatchLookup = _batch,
        ClearenceListLookup = null,
        ClearingType = CAS.SmartFactory.Linq.IPR.ClearingType.PartialWindingUp,
        CustomsProcedure = "5100",
        CustomsStatus = CustomsStatus.NotStarted,
        DisposalStatus = DisposalStatus.TobaccoInCigaretesWarehouse,
        DutyAndVAT = 123.4,
        DutyPerSettledAmount = 345.6,
        IPRDocumentNo = null,
        InvoiceNo = "InvoiceNomber",
        IPRID = _ipr2,
        JSOXCustomsSummaryListLookup = null,
        MaterialLookup = null,
        No = 7.8,
        RemainingQuantity = 0,
        SADDate = new Nullable<DateTime>(),
        SADDocumentNo = "N/A",
        SettledQuantity = 9.12,
        TobaccoValue = 34.56,
        Tytuł = "Testing disposal",
        VATPerSettledAmount = 78.90,
      };
      CutfillerCoefficient _cc = new CutfillerCoefficient
      {
        CFTProductivityNormMax = 995,
        CFTProductivityNormMin = 985,
        CFTProductivityRateMax = 0.995,
        CFTProductivityRateMin = 0.985
      };
      ingridients.Add( IPRIngredientFactory.IPRIngredient( _disposal1 ) );
      ingridients.Add( IPRIngredientFactory.IPRIngredient( _disposal2 ) );
      ingridients.Add( RegularIngredientFactory.RegularIngredient( "Reg Batch 54321", "Reg SKU 12345", 1234.56789 ) );
      string _masterDocumentName = "CigaretteExportFormCollection";
      string _invoiceNumber = "INV987654";
      int _position = 1;
      List<XmlCigaretteExportForm> cigaretteExportFormList = new List<XmlCigaretteExportForm>();
      CigaretteExportForm _cigaretteExportForm = CigaretteExportFormFactory.CigaretteExportForm( _cc, _batch, invoice, 0.5, ingridients, _masterDocumentName, ref _position, "3951" );
      cigaretteExportFormList.Add( _cigaretteExportForm );
      _cigaretteExportForm = CigaretteExportFormFactory.CigaretteExportForm( _cc, _batch, invoice, 0.5, ingridients, _masterDocumentName, ref _position, "3951" );
      cigaretteExportFormList.Add( _cigaretteExportForm );
      CigaretteExportFormCollection target = CigaretteExportFormCollectionFactory.CigaretteExportFormCollection( cigaretteExportFormList, _masterDocumentName, _invoiceNumber );
      XmlSerializer _srlzr = new XmlSerializer( typeof( CigaretteExportFormCollection ) );
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using ( XmlWriter file = XmlWriter.Create( _masterDocumentName + ".xml", _setting ) )
      {
        file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" href=\"CigaretteExportFormCollection.xslt\"" );
        _srlzr.Serialize( file, target );
        Assert.IsTrue( true, "Success" );
      }
    }
  }
}
