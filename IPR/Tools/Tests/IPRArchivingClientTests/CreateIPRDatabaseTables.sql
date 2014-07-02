﻿USE IPRDEV
CREATE TABLE ActivityLog (
      ActivityPriority nvarchar(255) NOT NULL,
      ActivitySource nvarchar(255) NOT NULL,
      Body nvarchar(max) NOT NULL,
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Expires datetime NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_ActivityLog_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE JSOXLibrary (
      BalanceDate datetime NOT NULL,
      BalanceQuantity float NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      IntroducingDateEnd datetime NOT NULL,
      IntroducingDateStart datetime NOT NULL,
      IntroducingQuantity float NOT NULL,
      JSOXLibraryReadOnly bit NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      OutboundDateEnd datetime NOT NULL,
      OutboundDateStart datetime NOT NULL,
      OutboundQuantity float NOT NULL,
      PreviousMonthDate datetime NOT NULL,
      PreviousMonthQuantity float NOT NULL,
      ReassumeQuantity float NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      SituationDate datetime NOT NULL,
      SituationQuantity float NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_JSOXLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE BalanceBatch (
      Archival bit NOT NULL,
      Balance float NOT NULL,
      Balance2JSOXLibraryIndex int NOT NULL,
      Batch nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      DustCSNotStarted float NOT NULL,
      DustCSStarted float NOT NULL,
      ID int NOT NULL,
      IPRBook float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      OveruseCSNotStarted float NOT NULL,
      OveruseCSStarted float NOT NULL,
      PureTobaccoCSNotStarted float NOT NULL,
      PureTobaccoCSStarted float NOT NULL,
      SHMentholCSNotStarted float NOT NULL,
      SHMentholCSStarted float NOT NULL,
      SHWasteOveruseCSNotStarted float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      TobaccoAvailable float NOT NULL,
      TobaccoCSFinished float NOT NULL,
      TobaccoEnteredIntoIPR float NOT NULL,
      TobaccoInCigarettesProduction float NOT NULL,
      TobaccoInCigarettesWarehouse float NOT NULL,
      TobaccoInCutfillerWarehouse float NOT NULL,
      TobaccoInFGCSNotStarted float NOT NULL,
      TobaccoInFGCSStarted float NOT NULL,
      TobaccoInWarehouse float NOT NULL,
      TobaccoStarted float NOT NULL,
      TobaccoToBeUsedInTheProduction float NOT NULL,
      TobaccoUsedInTheProduction float NOT NULL,
      WasteCSNotStarted float NOT NULL,
      WasteCSStarted float NOT NULL,
      CONSTRAINT PK_BalanceBatch_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_BalanceBatch_JSOXLibrary_Balance2JSOXLibraryIndex FOREIGN KEY (Balance2JSOXLibraryIndex) REFERENCES JSOXLibrary (ID),
);
CREATE TABLE SADDocumentLibrary (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SADDocumentLibraryComments nvarchar(255) NOT NULL,
      SADDocumentLibraryOK bit NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADDocumentLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SADDocument (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Currency nvarchar(255) NOT NULL,
      CustomsDebtDate datetime NOT NULL,
      DocumentNumber nvarchar(255) NOT NULL,
      ExchangeRate float NOT NULL,
      GrossMass float NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      NetMass float NOT NULL,
      ReferenceNumber nvarchar(255) NOT NULL,
      SADDocumenLibrarytIndex int NOT NULL,
      SystemID nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADDocument_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADDocument_SADDocumentLibrary_SADDocumenLibrarytIndex FOREIGN KEY (SADDocumenLibrarytIndex) REFERENCES SADDocumentLibrary (ID),
);
CREATE TABLE SADGood (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      GoodsDescription nvarchar(255) NOT NULL,
      GrossMass float NOT NULL,
      ID int NOT NULL,
      ItemNo float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      NetMass float NOT NULL,
      PCNTariffCode nvarchar(255) NOT NULL,
      SADDocumentIndex int NOT NULL,
      SPProcedure nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      TotalAmountInvoiced float NOT NULL,
      CONSTRAINT PK_SADGood_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADGood_SADDocument_SADDocumentIndex FOREIGN KEY (SADDocumentIndex) REFERENCES SADDocument (ID),
);
CREATE TABLE SADConsignment (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADConsignment_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE Clearence (
      Archival bit NOT NULL,
      Clearence2SadGoodID int NOT NULL,
      ClearenceProcedure nvarchar(255) NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProcedureCode nvarchar(255) NOT NULL,
      ReferenceNumber nvarchar(255) NOT NULL,
      SADConsignmentLibraryIndex int NOT NULL,
      SPStatus bit NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_Clearence_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_Clearence_SADGood_Clearence2SadGoodID FOREIGN KEY (Clearence2SadGoodID) REFERENCES SADGood (ID),
      CONSTRAINT FK_Clearence_SADConsignment_SADConsignmentLibraryIndex FOREIGN KEY (SADConsignmentLibraryIndex) REFERENCES SADConsignment (ID),
);
CREATE TABLE Consent (
      ConsentDate datetime NOT NULL,
      ConsentPeriod float NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      IsIPR bit NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductivityRateMax float NOT NULL,
      ProductivityRateMin float NOT NULL,
      Title nvarchar(255) NOT NULL,
      ValidFromDate datetime NOT NULL,
      ValidToDate datetime NOT NULL,
      CONSTRAINT PK_Consent_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE PCNCode (
      CompensationGood nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Disposal bit NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductCodeNumber nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_PCNCode_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE IPRLibrary (     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_IPRLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE IPR (
      AccountBalance float NOT NULL,
      AccountClosed bit NOT NULL,
      Archival bit NOT NULL,
      Batch nvarchar(255) NOT NULL,
      Cartons float NOT NULL,
      ClearenceIndex int NOT NULL,
      ClosingDate datetime NOT NULL,
      ConsentPeriod float NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Currency nvarchar(255) NOT NULL,
      CustomsDebtDate datetime NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      Duty float NOT NULL,
      DutyName nvarchar(255) NOT NULL,
      Grade nvarchar(255) NOT NULL,
      GrossMass float NOT NULL,
      ID int NOT NULL,
      InvoiceNo nvarchar(255) NOT NULL,
      IPR2ConsentTitle int NOT NULL,
      IPR2JSOXIndex int NOT NULL,
      IPR2PCNPCN int NOT NULL,
      IPRDutyPerUnit float NOT NULL,
      IPRLibraryIndex int NOT NULL,
      IPRUnitPrice float NOT NULL,
      IPRVATPerUnit float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      NetMass float NOT NULL,
      OGLValidTo datetime NOT NULL,
      ProductivityRateMax float NOT NULL,
      ProductivityRateMin float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      TobaccoName nvarchar(255) NOT NULL,
      TobaccoNotAllocated float NOT NULL,
      ValidFromDate datetime NOT NULL,
      ValidToDate datetime NOT NULL,
      Value float NOT NULL,
      VAT float NOT NULL,
      VATName nvarchar(255) NOT NULL,
      CONSTRAINT PK_IPR_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_IPR_Clearence_ClearenceIndex FOREIGN KEY (ClearenceIndex) REFERENCES Clearence (ID),
      CONSTRAINT FK_IPR_Consent_IPR2ConsentTitle FOREIGN KEY (IPR2ConsentTitle) REFERENCES Consent (ID),
      CONSTRAINT FK_IPR_JSOXLibrary_IPR2JSOXIndex FOREIGN KEY (IPR2JSOXIndex) REFERENCES JSOXLibrary (ID),
      CONSTRAINT FK_IPR_PCNCode_IPR2PCNPCN FOREIGN KEY (IPR2PCNPCN) REFERENCES PCNCode (ID),
      CONSTRAINT FK_IPR_IPRLibrary_IPRLibraryIndex FOREIGN KEY (IPRLibraryIndex) REFERENCES IPRLibrary (ID),
);
CREATE TABLE BalanceIPR (
      Archival bit NOT NULL,
      Balance float NOT NULL,
      BalanceBatchIndex int NOT NULL,
      BalanceIPR2JSOXIndex int NOT NULL,
      Batch nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      CustomsProcedure nvarchar(255) NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      DustCSNotStarted float NOT NULL,
      DustCSStarted float NOT NULL,
      ID int NOT NULL,
      InvoiceNo nvarchar(255) NOT NULL,
      IPRBook float NOT NULL,
      IPRIndex int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      OGLIntroduction nvarchar(255) NOT NULL,
      OveruseCSNotStarted float NOT NULL,
      OveruseCSStarted float NOT NULL,
      PureTobaccoCSNotStarted float NOT NULL,
      PureTobaccoCSStarted float NOT NULL,
      SHMentholCSNotStarted float NOT NULL,
      SHMentholCSStarted float NOT NULL,
      SHWasteOveruseCSNotStarted float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      TobaccoAvailable float NOT NULL,
      TobaccoCSFinished float NOT NULL,
      TobaccoEnteredIntoIPR float NOT NULL,
      TobaccoInFGCSNotStarted float NOT NULL,
      TobaccoInFGCSStarted float NOT NULL,
      TobaccoStarted float NOT NULL,
      TobaccoToBeUsedInTheProduction float NOT NULL,
      TobaccoUsedInTheProduction float NOT NULL,
      WasteCSNotStarted float NOT NULL,
      WasteCSStarted float NOT NULL,
      CONSTRAINT PK_BalanceIPR_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_BalanceIPR_BalanceBatch_BalanceBatchIndex FOREIGN KEY (BalanceBatchIndex) REFERENCES BalanceBatch (ID),
      CONSTRAINT FK_BalanceIPR_JSOXLibrary_BalanceIPR2JSOXIndex FOREIGN KEY (BalanceIPR2JSOXIndex) REFERENCES JSOXLibrary (ID),
      CONSTRAINT FK_BalanceIPR_IPR_IPRIndex FOREIGN KEY (IPRIndex) REFERENCES IPR (ID),
);
CREATE TABLE BatchLibrary (
      BatchLibraryComments nvarchar(255) NOT NULL,
      BatchLibraryOK bit NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_BatchLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SPFormat (
      CigaretteLenght nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FilterLenght nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SPFormat_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SKULibrary (      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SKULibrary_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SKU (
      Archival bit NOT NULL,
      BlendPurpose nvarchar(255) NOT NULL,
      Brand nvarchar(255) NOT NULL,
      CigaretteLenght nvarchar(255) NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Family nvarchar(255) NOT NULL,
      FilterLenght nvarchar(255) NOT NULL,
      FormatIndex int NOT NULL,
      ID int NOT NULL,
      IPRMaterial bit NOT NULL,
      Menthol nvarchar(255) NOT NULL,
      MentholMaterial bit NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      PrimeMarket nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      SKU nvarchar(255) NOT NULL,
      SKULibraryIndex int NOT NULL,
      Title nvarchar(255) NOT NULL,
      Units nvarchar(255) NOT NULL,
      CONSTRAINT PK_SKU_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SKU_SPFormat_FormatIndex FOREIGN KEY (FormatIndex) REFERENCES SPFormat (ID),
      CONSTRAINT FK_SKU_SKULibrary_SKULibraryIndex FOREIGN KEY (SKULibraryIndex) REFERENCES SKULibrary (ID),
);
CREATE TABLE Batch (
      Archival bit NOT NULL,
      Batch nvarchar(255) NOT NULL,
      BatchDustCooeficiency float NOT NULL,
      BatchLibraryIndex int NOT NULL,
      BatchSHCooeficiency float NOT NULL,
      BatchStatus nvarchar(255) NOT NULL,
      BatchWasteCooeficiency float NOT NULL,
      CalculatedOveruse float NOT NULL,
      CFTProductivityNormMax nvarchar(255) NOT NULL,
      CFTProductivityNormMin nvarchar(255) NOT NULL,
      CFTProductivityRateMax float NOT NULL,
      CFTProductivityRateMin float NOT NULL,
      CFTProductivityVersion float NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      CTFUsageMax float NOT NULL,
      CTFUsageMin float NOT NULL,
      Dust float NOT NULL,
      DustCooeficiencyVersion float NOT NULL,
      FGQuantity float NOT NULL,
      FGQuantityAvailable float NOT NULL,
      FGQuantityBlocked float NOT NULL,
      FGQuantityPrevious float NOT NULL,
      ID int NOT NULL,
      MaterialQuantity float NOT NULL,
      MaterialQuantityPrevious float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Overuse float NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      SHCooeficiencyVersion float NOT NULL,
      SHMenthol float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      SKUIndex int NOT NULL,
      Title nvarchar(255) NOT NULL,
      Tobacco float NOT NULL,
      UsageMax float NOT NULL,
      UsageMin float NOT NULL,
      UsageVersion float NOT NULL,
      Waste float NOT NULL,
      WasteCooeficiencyVersion float NOT NULL,
      CONSTRAINT PK_Batch_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_Batch_BatchLibrary_BatchLibraryIndex FOREIGN KEY (BatchLibraryIndex) REFERENCES BatchLibrary (ID),
      CONSTRAINT FK_Batch_SKU_SKUIndex FOREIGN KEY (SKUIndex) REFERENCES SKU (ID),
);
CREATE TABLE CustomsUnion (    
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      EUPrimeMarket nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_CustomsUnion_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE CustomsWarehouse (
      AccountBalance float NOT NULL,
      AccountClosed bit NOT NULL,
      Archival bit NOT NULL,
      Batch nvarchar(255) NOT NULL,
      ClosingDate datetime NOT NULL,    
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Currency nvarchar(255) NOT NULL,
      CustomsDebtDate datetime NOT NULL,
      CW_CertificateOfAuthenticity nvarchar(255) NOT NULL,
      CW_CertificateOfOrgin nvarchar(255) NOT NULL,
      CW_COADate datetime NOT NULL,
      CW_CODate datetime NOT NULL,
      CW_MassPerPackage float NOT NULL,
      CW_PackageKg float NOT NULL,
      CW_PackageUnits float NOT NULL,
      CW_PzNo nvarchar(255) NOT NULL,
      CW_Quantity float NOT NULL,
      CW_UnitPrice float NOT NULL,
      CWC_EntryDate datetime NOT NULL,
      CWL_CW2BinCardTitle int NOT NULL,
      CWL_CW2ClearenceID int NOT NULL,
      CWL_CW2ConsentTitle int NOT NULL,
      CWL_CW2CWLibraryID int NOT NULL,
      CWL_CW2PCNID int NOT NULL,
      CWL_CW2VendorTitle int NOT NULL,
      DocumentNo nvarchar(255) NOT NULL,
      Grade nvarchar(255) NOT NULL,
      GrossMass float NOT NULL,
      ID int NOT NULL,
      InvoiceNo nvarchar(255) NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      NetMass float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      TobaccoName nvarchar(255) NOT NULL,
      TobaccoNotAllocated float NOT NULL,
      Units nvarchar(255) NOT NULL,
      ValidToDate datetime NOT NULL,
      Value float NOT NULL,
      CONSTRAINT PK_CustomsWarehouse_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_CustomsWarehouse_Clearence_CWL_CW2ClearenceID FOREIGN KEY (CWL_CW2ClearenceID) REFERENCES Clearence (ID),
      CONSTRAINT FK_CustomsWarehouse_Consent_CWL_CW2ConsentTitle FOREIGN KEY (CWL_CW2ConsentTitle) REFERENCES Consent (ID),
      CONSTRAINT FK_CustomsWarehouse_PCNCode_CWL_CW2PCNID FOREIGN KEY (CWL_CW2PCNID) REFERENCES PCNCode (ID),
);
CREATE TABLE CutfillerCoefficient (
      CFTProductivityNormMax nvarchar(255) NOT NULL,
      CFTProductivityNormMin nvarchar(255) NOT NULL,
      CFTProductivityRateMax float NOT NULL,
      CFTProductivityRateMin float NOT NULL,
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_CutfillerCoefficient_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE InvoiceLibrary (
      BillDoc nvarchar(255) NOT NULL,
      ClearenceIndex int NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      InvoiceCreationDate datetime NOT NULL,
      InvoiceLibraryReadOnly bit NOT NULL,
      InvoiceLibraryStatus bit NOT NULL,
      IsExport bit NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_InvoiceLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_InvoiceLibrary_Clearence_ClearenceIndex FOREIGN KEY (ClearenceIndex) REFERENCES Clearence (ID),
);
CREATE TABLE InvoiceContent (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      InvoiceContent2BatchIndex int NOT NULL,
      InvoiceContentStatus nvarchar(255) NOT NULL,
      InvoiceIndex int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      Quantity float NOT NULL,
      SKUDescription nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      Units nvarchar(255) NOT NULL,
      CONSTRAINT PK_InvoiceContent_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_InvoiceContent_Batch_InvoiceContent2BatchIndex FOREIGN KEY (InvoiceContent2BatchIndex) REFERENCES Batch (ID),
      CONSTRAINT FK_InvoiceContent_InvoiceLibrary_InvoiceIndex FOREIGN KEY (InvoiceIndex) REFERENCES InvoiceLibrary (ID),
);
CREATE TABLE Material (
      Archival bit NOT NULL,
      Batch nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      Dust float NOT NULL,
      FGQuantity float NOT NULL,
      ID int NOT NULL,
      Material2BatchIndex int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Overuse float NOT NULL,
      ProductID nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      SHMenthol float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      SKUDescription nvarchar(255) NOT NULL,
      StorLoc nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      Tobacco float NOT NULL,
      TobaccoQuantity float NOT NULL,
      Units nvarchar(255) NOT NULL,
      Waste float NOT NULL,
      CONSTRAINT PK_Material_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_Material_Batch_Material2BatchIndex FOREIGN KEY (Material2BatchIndex) REFERENCES Batch (ID),
);
CREATE TABLE JSOXCustomsSummary (
      CompensationGood nvarchar(255) NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      CustomsProcedure nvarchar(255) NOT NULL,
      ExportOrFreeCirculationSAD nvarchar(255) NOT NULL,
      ID int NOT NULL,
      IntroducingSADDate datetime NOT NULL,
      IntroducingSADNo nvarchar(255) NOT NULL,
      InvoiceNo nvarchar(255) NOT NULL,
      JSOXCustomsSummary2JSOXIndex int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      RemainingQuantity float NOT NULL,
      SADDate datetime NOT NULL,
      Title nvarchar(255) NOT NULL,
      TotalAmount float NOT NULL,
      CONSTRAINT PK_JSOXCustomsSummary_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_JSOXCustomsSummary_JSOXLibrary_JSOXCustomsSummary2JSOXIndex FOREIGN KEY (JSOXCustomsSummary2JSOXIndex) REFERENCES JSOXLibrary (ID),
);
CREATE TABLE Disposal (
      Archival bit NOT NULL,
      ClearingType nvarchar(255) NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      CustomsProcedure nvarchar(255) NOT NULL,
      CustomsStatus nvarchar(255) NOT NULL,
      Disposal2BatchIndex int NOT NULL,
      Disposal2ClearenceIndex int NOT NULL,
      Disposal2InvoiceContentIndex int NOT NULL,
      Disposal2IPRIndex int NOT NULL,
      Disposal2MaterialIndex int NOT NULL,
      Disposal2PCNID int NOT NULL,
      DisposalStatus nvarchar(255) NOT NULL,
      DutyAndVAT float NOT NULL,
      DutyPerSettledAmount float NOT NULL,
      ID int NOT NULL,
      InvoiceNo nvarchar(255) NOT NULL,
      IPRDocumentNo nvarchar(255) NOT NULL,
      JSOXCustomsSummaryIndex int NOT NULL,
      JSOXReportID float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      RemainingQuantity float NOT NULL,
      SadConsignmentNo nvarchar(255) NOT NULL,
      SADDate datetime NOT NULL,
      SADDocumentNo nvarchar(255) NOT NULL,
      SettledQuantity float NOT NULL,
      SPNo float NOT NULL,
      Title nvarchar(255) NOT NULL,
      TobaccoValue float NOT NULL,
      VATPerSettledAmount float NOT NULL,
      CONSTRAINT PK_Disposal_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_Disposal_Batch_Disposal2BatchIndex FOREIGN KEY (Disposal2BatchIndex) REFERENCES Batch (ID),
      CONSTRAINT FK_Disposal_Clearence_Disposal2ClearenceIndex FOREIGN KEY (Disposal2ClearenceIndex) REFERENCES Clearence (ID),
      CONSTRAINT FK_Disposal_InvoiceContent_Disposal2InvoiceContentIndex FOREIGN KEY (Disposal2InvoiceContentIndex) REFERENCES InvoiceContent (ID),
      CONSTRAINT FK_Disposal_IPR_Disposal2IPRIndex FOREIGN KEY (Disposal2IPRIndex) REFERENCES IPR (ID),
      CONSTRAINT FK_Disposal_Material_Disposal2MaterialIndex FOREIGN KEY (Disposal2MaterialIndex) REFERENCES Material (ID),
      CONSTRAINT FK_Disposal_PCNCode_Disposal2PCNID FOREIGN KEY (Disposal2PCNID) REFERENCES PCNCode (ID),
      CONSTRAINT FK_Disposal_JSOXCustomsSummary_JSOXCustomsSummaryIndex FOREIGN KEY (JSOXCustomsSummaryIndex) REFERENCES JSOXCustomsSummary (ID),
);
CREATE TABLE Dust (
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      DustRatio float NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_Dust_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SADDuties (
      Amount float NOT NULL,
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      DutyType nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SADDuties2SADGoodID int NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADDuties_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADDuties_SADGood_SADDuties2SADGoodID FOREIGN KEY (SADDuties2SADGoodID) REFERENCES SADGood (ID),
);
CREATE TABLE SADPackage (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      ItemNo float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Package nvarchar(255) NOT NULL,
      SADPackage2SADGoodID int NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADPackage_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADPackage_SADGood_SADPackage2SADGoodID FOREIGN KEY (SADPackage2SADGoodID) REFERENCES SADGood (ID),
);
CREATE TABLE SADQuantity (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      ItemNo float NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      NetMass float NOT NULL,
      SADQuantity2SADGoodID int NOT NULL,
      Title nvarchar(255) NOT NULL,
      Units nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADQuantity_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADQuantity_SADGood_SADQuantity2SADGoodID FOREIGN KEY (SADQuantity2SADGoodID) REFERENCES SADGood (ID),
);
CREATE TABLE SADRequiredDocuments (
      Archival bit NOT NULL,
      Code nvarchar(255) NOT NULL,    
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Number nvarchar(255) NOT NULL,
      SADRequiredDoc2SADGoodID int NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SADRequiredDocuments_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_SADRequiredDocuments_SADGood_SADRequiredDoc2SADGoodID FOREIGN KEY (SADRequiredDoc2SADGoodID) REFERENCES SADGood (ID),
);
CREATE TABLE Settings (     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      KeyValue nvarchar(255) NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_Settings_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE SHMenthol (
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      SHMentholRatio float NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_SHMenthol_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE StockLibrary (
      Archival bit NOT NULL,      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      FileLeafRef nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      SelectFilename nvarchar(255) NOT NULL,
      Stock2JSOXLibraryIndex int NOT NULL,
      Title nvarchar(255) NOT NULL,
      CONSTRAINT PK_StockLibrary_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_StockLibrary_JSOXLibrary_Stock2JSOXLibraryIndex FOREIGN KEY (Stock2JSOXLibraryIndex) REFERENCES JSOXLibrary (ID),
);
CREATE TABLE StockEntry (
      Archival bit NOT NULL,
      Batch nvarchar(255) NOT NULL,
      BatchIndex int NOT NULL,
      Blocked float NOT NULL,     
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      InQualityInsp float NOT NULL,
      IPRType bit NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      Quantity float NOT NULL,
      RestrictedUse float NOT NULL,
      SKU nvarchar(255) NOT NULL,
      StockLibraryIndex int NOT NULL,
      StorLoc nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      Units nvarchar(255) NOT NULL,
      Unrestricted float NOT NULL,
      CONSTRAINT PK_StockEntry_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_StockEntry_Batch_BatchIndex FOREIGN KEY (BatchIndex) REFERENCES Batch (ID),
      CONSTRAINT FK_StockEntry_StockLibrary_StockLibraryIndex FOREIGN KEY (StockLibraryIndex) REFERENCES StockLibrary (ID),
);
CREATE TABLE Usage (
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      CTFUsageMax float NOT NULL,
      CTFUsageMin float NOT NULL,
      FormatIndex int NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      UsageMax float NOT NULL,
      UsageMin float NOT NULL,
      CONSTRAINT PK_Usage_ID PRIMARY KEY CLUSTERED (ID ASC) ,
      CONSTRAINT FK_Usage_SPFormat_FormatIndex FOREIGN KEY (FormatIndex) REFERENCES SPFormat (ID),
);
CREATE TABLE Warehouse (      
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      SPExternal bit NOT NULL,
      SPProcedure nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      WarehouseName nvarchar(255) NOT NULL,
      CONSTRAINT PK_Warehouse_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
CREATE TABLE Waste (
      Created datetime NOT NULL,
      Created_x0020_By nvarchar(255) NOT NULL,
      ID int NOT NULL,
      Modified datetime NOT NULL,
      Modified_x0020_By nvarchar(255) NOT NULL,
      ProductType nvarchar(255) NOT NULL,
      Title nvarchar(255) NOT NULL,
      WasteRatio float NOT NULL,
      CONSTRAINT PK_Waste_ID PRIMARY KEY CLUSTERED (ID ASC) 
);
