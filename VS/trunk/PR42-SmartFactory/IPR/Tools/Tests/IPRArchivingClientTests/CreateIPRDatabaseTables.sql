USE IPRDEV
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ActivityLog')
  drop table  ActivityLog;
CREATE TABLE [dbo].[ActivityLog] (
    [ActivityPriority]       NVARCHAR(max)   NULL,
    [ActivitySource]         NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [Expires]                DATETIME        NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [Version]                INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	 NULL,	
	CONSTRAINT [PK_ActivityLog_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JSOXLibrary')
  drop table  JSOXLibrary;
CREATE TABLE [dbo].[JSOXLibrary] (
    [Author]                NVARCHAR (MAX) NULL,
    [BalanceDate]           DATETIME       NULL,
    [BalanceQuantity]       FLOAT (53)     NULL,
    [Created]               DATETIME       NULL,
    [DocumentCreatedBy]     NVARCHAR (MAX) NULL,
    [Editor]                NVARCHAR (MAX) NULL,
    [FileName]              NVARCHAR (MAX) NOT NULL,
    [ID]                    INT            NOT NULL,
    [IntroducingDateEnd]    DATETIME       NULL,
    [IntroducingDateStart]  DATETIME       NULL,
    [IntroducingQuantity]   FLOAT (53)     NULL,
    [JSOXLibraryReadOnly]   BIT            NULL,
    [Modified]              DATETIME       NULL,
    [DocumentModifiedBy]    NVARCHAR (MAX) NULL,
    [OutboundDateEnd]       DATETIME       NULL,
    [OutboundDateStart]     DATETIME       NULL,
    [OutboundQuantity]      FLOAT (53)     NULL,
    [PreviousMonthDate]     DATETIME       NULL,
    [PreviousMonthQuantity] FLOAT (53)     NULL,
    [ReassumeQuantity]      FLOAT (53)     NULL,
    [SituationDate]         DATETIME       NULL,
    [SituationQuantity]     FLOAT (53)     NULL,
    [Title]                 NVARCHAR (MAX) NULL,
    [Version]               INT            NULL,
	[OnlySQL]				BIT			   NOT NULL,
	[UIVersionString]		NVARCHAR(max)  NULL,	
    CONSTRAINT [PK_JSOXLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'BalanceBatch')
  drop table  BalanceBatch;
CREATE TABLE [dbo].[BalanceBatch] (
    [Archival]                       BIT            NULL,
    [Author]                         NVARCHAR (MAX) NULL,
    [Balance]                        FLOAT (53)     NULL,
    [Balance2JSOXLibraryIndex]       INT            NULL,
    [Batch]                          NVARCHAR (MAX) NULL,
    [Created]                        DATETIME       NULL,
    [DocumentNo]                     NVARCHAR (MAX) NULL,
    [DustCSNotStarted]               FLOAT (53)     NULL,
    [DustCSStarted]                  FLOAT (53)     NULL,
    [Editor]                         NVARCHAR (MAX) NULL,
    [ID]                             INT            NOT NULL,
    [IPRBook]                        FLOAT (53)     NULL,
    [Modified]                       DATETIME       NULL,
    [OveruseCSNotStarted]            FLOAT (53)     NULL,
    [OveruseCSStarted]               FLOAT (53)     NULL,
    [PureTobaccoCSNotStarted]        FLOAT (53)     NULL,
    [PureTobaccoCSStarted]           FLOAT (53)     NULL,
    [SHMentholCSNotStarted]          FLOAT (53)     NULL,
    [SHMentholCSStarted]             FLOAT (53)     NULL,
    [SHWasteOveruseCSNotStarted]     FLOAT (53)     NULL,
    [SKU]                            NVARCHAR (MAX) NULL,
    [Title]                          NVARCHAR (MAX) NOT NULL,
    [TobaccoAvailable]               FLOAT (53)     NULL,
    [TobaccoCSFinished]              FLOAT (53)     NULL,
    [TobaccoEnteredIntoIPR]          FLOAT (53)     NULL,
    [TobaccoInCigarettesProduction]  FLOAT (53)     NULL,
    [TobaccoInCigarettesWarehouse]   FLOAT (53)     NULL,
    [TobaccoInCutfillerWarehouse]    FLOAT (53)     NULL,
    [TobaccoInFGCSNotStarted]        FLOAT (53)     NULL,
    [TobaccoInFGCSStarted]           FLOAT (53)     NULL,
    [TobaccoInWarehouse]             FLOAT (53)     NULL,
    [TobaccoStarted]                 FLOAT (53)     NULL,
    [TobaccoToBeUsedInTheProduction] FLOAT (53)     NULL,
    [TobaccoUsedInTheProduction]     FLOAT (53)     NULL,
    [Version]                        INT            NULL,
    [WasteCSNotStarted]              FLOAT (53)     NULL,
    [WasteCSStarted]                 FLOAT (53)     NULL,
    [OnlySQL]						 BIT			NOT NULL,
	[UIVersionString]				 NVARCHAR(max)	NULL, 
    CONSTRAINT [PK_BalanceBatch_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BalanceBatch_JSOXLibrary] FOREIGN KEY ([Balance2JSOXLibraryIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADDocumentLibrary')
  drop table  SADDocumentLibrary;
CREATE TABLE [dbo].[SADDocumentLibrary] (
    [Archival]                   BIT            NULL,
    [Author]                     NVARCHAR (MAX) NULL,
    [Created]                    DATETIME       NULL,
    [DocumentCreatedBy]          NVARCHAR (MAX) NULL,
    [Editor]                     NVARCHAR (MAX) NULL,
    [FileName]                   NVARCHAR (MAX) NOT NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NULL,
    [DocumentModifiedBy]         NVARCHAR (MAX) NULL,
    [SADDocumentLibraryComments] NVARCHAR (MAX) NULL,
    [SADDocumentLibraryOK]       BIT            NULL,
    [Title]                      NVARCHAR (MAX) NULL,
    [Version]                    INT            NULL,
	[OnlySQL]					 BIT			NOT NULL,
	[UIVersionString]			 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_SADDocumentLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADDocument')
  drop table  SADDocument;
CREATE TABLE [dbo].[SADDocument] (
    [Archival]                BIT            NULL,
    [Author]                  NVARCHAR (MAX) NULL,
    [Created]                 DATETIME       NULL,
    [Currency]                NVARCHAR (MAX) NULL,
    [CustomsDebtDate]         DATETIME       NULL,
    [DocumentNumber]          NVARCHAR (MAX) NULL,
    [Editor]                  NVARCHAR (MAX) NULL,
    [ExchangeRate]            FLOAT (53)     NULL,
    [GrossMass]               FLOAT (53)     NULL,
    [ID]                      INT            NOT NULL,
    [Modified]                DATETIME       NULL,
    [NetMass]                 FLOAT (53)     NULL,
    [ReferenceNumber]         NVARCHAR (MAX) NULL,
    [SADDocumenLibrarytIndex] INT            NULL,
    [SystemID]                NVARCHAR (MAX) NULL,
    [Title]                   NVARCHAR (MAX) NOT NULL,
    [Version]                 INT            NULL,
	[OnlySQL]				  BIT			 NOT NULL,
	[UIVersionString]		  NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_SADDocument_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDocument_SADDocumentLibrary] FOREIGN KEY ([SADDocumenLibrarytIndex]) REFERENCES [dbo].[SADDocumentLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADGood')
  drop table  SADGood;
CREATE TABLE [dbo].[SADGood] (
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Created]             DATETIME       NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [GoodsDescription]    NVARCHAR (MAX) NULL,
    [GrossMass]           FLOAT (53)     NULL,
    [ID]                  INT            NOT NULL,
    [ItemNo]              FLOAT (53)     NULL,
    [Modified]            DATETIME       NULL,
    [NetMass]             FLOAT (53)     NULL,
    [PCNTariffCode]       NVARCHAR (MAX) NULL,
    [SADDocumentIndex]    INT            NULL,
    [SPProcedure]         NVARCHAR (MAX) NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [TotalAmountInvoiced] FLOAT (53)     NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
	[UIVersionString]     NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_SADGood_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADGood_SADDocument] FOREIGN KEY ([SADDocumentIndex]) REFERENCES [dbo].[SADDocument] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADConsignment')
  drop table  SADConsignment;
CREATE TABLE [dbo].[SADConsignment] (
    [Archival]           BIT            NULL,
    [Author]             NVARCHAR (MAX) NULL,
    [Created]            DATETIME       NULL,
    [DocumentCreatedBy]  NVARCHAR (MAX) NULL,
    [Editor]             NVARCHAR (MAX) NULL,
    [FileName]           NVARCHAR (MAX) NOT NULL,
    [ID]                 INT            NOT NULL,
    [Modified]           DATETIME       NULL,
    [DocumentModifiedBy] NVARCHAR (MAX) NULL,
    [Title]              NVARCHAR (MAX) NULL,
    [Version]            INT            NULL,
	[OnlySQL]			 BIT			NOT NULL,
	[UIVersionString]	 NVARCHAR(max)	NULL,
    CONSTRAINT [PK_SADConsignment_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Clearence')
  drop table  Clearence;
CREATE TABLE [dbo].[Clearence] (
    [Archival]                   BIT            NULL,
    [Author]                     NVARCHAR (MAX) NULL,
    [Clearence2SadGoodID]        INT            NULL,
    [ClearenceProcedure]         NVARCHAR (MAX) NULL,
    [Created]                    DATETIME       NULL,
    [DocumentNo]                 NVARCHAR (MAX) NULL,
    [Editor]                     NVARCHAR (MAX) NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NULL,
    [ProcedureCode]              NVARCHAR (MAX) NULL,
    [ReferenceNumber]            NVARCHAR (MAX) NULL,
    [SADConsignmentLibraryIndex] INT            NULL,
    [SPStatus]                   BIT            NULL,
    [Title]                      NVARCHAR (MAX) NOT NULL,
    [Version]                    INT            NULL,
	[OnlySQL]					 BIT			NOT NULL,
	[UIVersionString]		     NVARCHAR(max)	NULL,
    CONSTRAINT [PK_Clearence_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Clearence_SADConsignment] FOREIGN KEY ([SADConsignmentLibraryIndex]) REFERENCES [dbo].[SADConsignment] ([ID]),
    CONSTRAINT [FK_Clearence_SADGood] FOREIGN KEY ([Clearence2SadGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Consent')
  drop table  Consent;
CREATE TABLE [dbo].[Consent] (
    [Author]              NVARCHAR (MAX) NULL,
    [ConsentDate]         DATETIME       NULL,
    [ConsentPeriod]       FLOAT (53)     NULL,
    [Created]             DATETIME       NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [ID]                  INT            NOT NULL,
    [IsIPR]               BIT            NULL,
    [Modified]            DATETIME       NULL,
    [ProductivityRateMax] FLOAT (53)     NULL,
    [ProductivityRateMin] FLOAT (53)     NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [ValidFromDate]       DATETIME       NULL,
    [ValidToDate]         DATETIME       NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
	[UIVersionString]	  NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_Consent_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PCNCode')
  drop table  PCNCode;
CREATE TABLE [dbo].[PCNCode] (
    [Author]            NVARCHAR (MAX) NULL,
    [CompensationGood]  NVARCHAR (MAX) NULL,
    [Created]           DATETIME       NULL,
    [Disposal]          BIT            NULL,
    [Editor]            NVARCHAR (MAX) NULL,
    [ID]                INT            NOT NULL,
    [Modified]          DATETIME       NULL,
    [ProductCodeNumber] NVARCHAR (MAX) NULL,
    [Title]             NVARCHAR (MAX) NOT NULL,
    [Version]           INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_PCNCode_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'IPRLibrary')
  drop table  IPRLibrary;
CREATE TABLE [dbo].[IPRLibrary] (
    [Author]             NVARCHAR (MAX) NULL,
    [Created]            DATETIME       NULL,
    [DocumentCreatedBy]  NVARCHAR (MAX) NULL,
    [DocumentNo]         NVARCHAR (MAX) NULL,
    [Editor]             NVARCHAR (MAX) NULL,
    [FileName]           NVARCHAR (MAX) NOT NULL,
    [ID]                 INT            NOT NULL,
    [Modified]           DATETIME       NULL,
    [DocumentModifiedBy] NVARCHAR (MAX) NULL,
    [Title]              NVARCHAR (MAX) NULL,
    [Version]            INT            NULL,
	[OnlySQL]			 BIT			NOT NULL,
	[UIVersionString]	 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_IPRLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'IPR')
  drop table  IPR;
CREATE TABLE [dbo].[IPR] (
    [AccountBalance]      FLOAT (53)     NOT NULL,
    [AccountClosed]       BIT            NOT NULL,
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Batch]               NVARCHAR (MAX) NULL,
    [Cartons]             FLOAT (53)     NULL,
    [ClearenceIndex]      INT            NULL,
    [ClosingDate]         DATETIME       NULL,
    [ConsentPeriod]       FLOAT (53)     NULL,
    [Created]             DATETIME       NULL,
    [Currency]            NVARCHAR (MAX) NULL,
    [CustomsDebtDate]     DATETIME       NULL,
    [DocumentNo]          NVARCHAR (MAX) NULL,
    [Duty]                FLOAT (53)     NULL,
    [DutyName]            NVARCHAR (MAX) NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [Grade]               NVARCHAR (MAX) NULL,
    [GrossMass]           FLOAT (53)     NULL,
    [ID]                  INT            NOT NULL,
    [InvoiceNo]           NVARCHAR (MAX) NULL,
    [IPR2ConsentTitle]    INT            NULL,
    [IPR2JSOXIndex]       INT            NULL,
    [IPR2PCNPCN]          INT            NULL,
    [IPRDutyPerUnit]      FLOAT (53)     NULL,
    [IPRLibraryIndex]     INT            NULL,
    [IPRUnitPrice]        FLOAT (53)     NULL,
    [IPRVATPerUnit]       FLOAT (53)     NULL,
    [Modified]            DATETIME       NULL,
    [NetMass]             FLOAT (53)     NULL,
    [OGLValidTo]          DATETIME       NULL,
    [ProductivityRateMax] FLOAT (53)     NULL,
    [ProductivityRateMin] FLOAT (53)     NULL,
    [SKU]                 NVARCHAR (MAX) NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [TobaccoName]         NVARCHAR (MAX) NULL,
    [TobaccoNotAllocated] FLOAT (53)     NULL,
    [ValidFromDate]       DATETIME       NULL,
    [ValidToDate]         DATETIME       NULL,
    [Value]               FLOAT (53)     NULL,
    [VAT]                 FLOAT (53)     NULL,
    [VATName]             NVARCHAR (MAX) NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
	[UIVersionString]	  NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_IPR_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_IPR_Clearence] FOREIGN KEY ([ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID]),
    CONSTRAINT [FK_IPR_Consent] FOREIGN KEY ([IPR2ConsentTitle]) REFERENCES [dbo].[Consent] ([ID]),
    CONSTRAINT [FK_IPR_IPRLibrary] FOREIGN KEY ([IPRLibraryIndex]) REFERENCES [dbo].[IPRLibrary] ([ID]),
    CONSTRAINT [FK_IPR_JSOXLibrary] FOREIGN KEY ([IPR2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID]),
    CONSTRAINT [FK_IPR_PCNCode] FOREIGN KEY ([IPR2PCNPCN]) REFERENCES [dbo].[PCNCode] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'BalanceIPR')
  drop table  BalanceIPR;
CREATE TABLE [dbo].[BalanceIPR] (
    [Archival]                       BIT            NULL,
    [Author]                         NVARCHAR (MAX) NULL,
    [Balance]                        FLOAT (53)     NULL,
    [BalanceBatchIndex]              INT            NULL,
    [BalanceIPR2JSOXIndex]           INT            NULL,
    [Batch]                          NVARCHAR (MAX) NULL,
    [Created]                        DATETIME       NULL,
    [CustomsProcedure]               NVARCHAR (MAX) NULL,
    [DocumentNo]                     NVARCHAR (MAX) NULL,
    [DustCSNotStarted]               FLOAT (53)     NULL,
    [DustCSStarted]                  FLOAT (53)     NULL,
    [Editor]                         NVARCHAR (MAX) NULL,
    [ID]                             INT            NOT NULL,
    [InvoiceNo]                      NVARCHAR (MAX) NULL,
    [IPRBook]                        FLOAT (53)     NULL,
    [IPRIndex]                       INT            NULL,
    [Modified]                       DATETIME       NULL,
    [OGLIntroduction]                NVARCHAR (MAX) NULL,
    [OveruseCSNotStarted]            FLOAT (53)     NULL,
    [OveruseCSStarted]               FLOAT (53)     NULL,
    [PureTobaccoCSNotStarted]        FLOAT (53)     NULL,
    [PureTobaccoCSStarted]           FLOAT (53)     NULL,
    [SHMentholCSNotStarted]          FLOAT (53)     NULL,
    [SHMentholCSStarted]             FLOAT (53)     NULL,
    [SHWasteOveruseCSNotStarted]     FLOAT (53)     NULL,
    [SKU]                            NVARCHAR (MAX) NULL,
    [Title]                          NVARCHAR (MAX) NOT NULL,
    [TobaccoAvailable]               FLOAT (53)     NULL,
    [TobaccoCSFinished]              FLOAT (53)     NULL,
    [TobaccoEnteredIntoIPR]          FLOAT (53)     NULL,
    [TobaccoInFGCSNotStarted]        FLOAT (53)     NULL,
    [TobaccoInFGCSStarted]           FLOAT (53)     NULL,
    [TobaccoStarted]                 FLOAT (53)     NULL,
    [TobaccoToBeUsedInTheProduction] FLOAT (53)     NULL,
    [TobaccoUsedInTheProduction]     FLOAT (53)     NULL,
    [Version]                        INT            NULL,
    [WasteCSNotStarted]              FLOAT (53)     NULL,
    [WasteCSStarted]                 FLOAT (53)     NULL,
	[OnlySQL]						 BIT			NOT NULL,
	[UIVersionString]		         NVARCHAR(max)	NULL,
    CONSTRAINT [PK_BalanceIPR_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BalanceIPR_BalanceBatch] FOREIGN KEY ([BalanceBatchIndex]) REFERENCES [dbo].[BalanceBatch] ([ID]),
    CONSTRAINT [FK_BalanceIPR_IPR] FOREIGN KEY ([IPRIndex]) REFERENCES [dbo].[IPR] ([ID]),
    CONSTRAINT [FK_BalanceIPR_JSOXLibrary] FOREIGN KEY ([BalanceIPR2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'BatchLibrary')
  drop table  BatchLibrary;
CREATE TABLE [dbo].[BatchLibrary] (
    [Author]               NVARCHAR (MAX) NULL,
    [BatchLibraryComments] NVARCHAR (MAX) NULL,
    [BatchLibraryOK]       BIT            NULL,
    [Created]              DATETIME       NULL,
    [DocumentCreatedBy]    NVARCHAR (MAX) NULL,
    [Editor]               NVARCHAR (MAX) NULL,
    [FileName]             NVARCHAR (MAX) NOT NULL,
    [ID]                   INT            NOT NULL,
    [Modified]             DATETIME       NULL,
    [DocumentModifiedBy]   NVARCHAR (MAX) NULL,
    [Title]                NVARCHAR (MAX) NULL,
    [Version]              INT            NULL,
	[OnlySQL]			   BIT			  NOT NULL,
	[UIVersionString]	   NVARCHAR(max)  NULL,	
    CONSTRAINT [PK_BatchLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPFormat')
  drop table  SPFormat;
CREATE TABLE [dbo].[SPFormat] (
    [Author]          NVARCHAR (MAX) NULL,
    [CigaretteLenght] NVARCHAR (MAX) NOT NULL,
    [Created]         DATETIME       NULL,
    [Editor]          NVARCHAR (MAX) NULL,
    [FilterLenght]    NVARCHAR (MAX) NOT NULL,
    [ID]              INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [Title]           NVARCHAR (MAX) NOT NULL,
    [Version]         INT            NULL,
	[OnlySQL]		  BIT			 NOT NULL,
	[UIVersionString] NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_SPFormat_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SKULibrary')
  drop table  SKULibrary;
CREATE TABLE [dbo].[SKULibrary] (
    [Author]             NVARCHAR (MAX) NULL,
    [Created]            DATETIME       NULL,
    [DocumentCreatedBy]  NVARCHAR (MAX) NULL,
    [Editor]             NVARCHAR (MAX) NULL,
    [FileName]           NVARCHAR (MAX) NOT NULL,
    [ID]                 INT            NOT NULL,
    [Modified]           DATETIME       NULL,
    [DocumentModifiedBy] NVARCHAR (MAX) NULL,
    [Title]              NVARCHAR (MAX) NULL,
    [Version]            INT            NULL,
	[OnlySQL]			 BIT			NOT NULL,
	[UIVersionString]	 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_SKULibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SKU')
  drop table  SKU;
CREATE TABLE [dbo].[SKU] (
    [Archival]        BIT            NULL,
    [Author]          NVARCHAR (MAX) NULL,
    [BlendPurpose]    NVARCHAR (MAX) NULL,
    [Brand]           NVARCHAR (MAX) NULL,
    [CigaretteLenght] NVARCHAR (MAX) NULL,
    [Created]         DATETIME       NULL,
    [Editor]          NVARCHAR (MAX) NULL,
    [Family]          NVARCHAR (MAX) NULL,
    [FilterLenght]    NVARCHAR (MAX) NULL,
    [FormatIndex]     INT            NULL,
    [ID]              INT            NOT NULL,
    [IPRMaterial]     BIT            NULL,
    [Menthol]         NVARCHAR (MAX) NULL,
    [MentholMaterial] BIT            NULL,
    [Modified]        DATETIME       NULL,
    [PrimeMarket]     NVARCHAR (MAX) NULL,
    [ProductType]     NVARCHAR (MAX) NULL,
    [SKU]             NVARCHAR (MAX) NULL,
    [SKULibraryIndex] INT            NULL,
    [Title]           NVARCHAR (MAX) NOT NULL,
    [Units]           NVARCHAR (MAX) NULL,
    [Version]         INT            NULL,
	[OnlySQL]		  BIT			 NOT NULL,
	[UIVersionString] NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_SKU_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SKU_SKULibrary] FOREIGN KEY ([SKULibraryIndex]) REFERENCES [dbo].[SKULibrary] ([ID]),
    CONSTRAINT [FK_SKU_SPFormat] FOREIGN KEY ([FormatIndex]) REFERENCES [dbo].[SPFormat] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Batch')
  drop table  Batch;
CREATE TABLE [dbo].[Batch] (
    [Archival]                 BIT            NULL,
    [Author]                   NVARCHAR (MAX) NULL,
    [Batch]                    NVARCHAR (MAX) NULL,
    [BatchDustCooeficiency]    FLOAT (53)     NULL,
    [BatchLibraryIndex]        INT            NULL,
    [BatchSHCooeficiency]      FLOAT (53)     NULL,
    [BatchStatus]              NVARCHAR (MAX) NULL,
    [BatchWasteCooeficiency]   FLOAT (53)     NULL,
    [CalculatedOveruse]        FLOAT (53)     NULL,
    [CFTProductivityNormMax]   FLOAT (53)     NULL,
    [CFTProductivityNormMin]   FLOAT (53)     NULL,
    [CFTProductivityRateMax]   FLOAT (53)     NULL,
    [CFTProductivityRateMin]   FLOAT (53)     NULL,
    [CFTProductivityVersion]   FLOAT (53)     NULL,
    [Created]                  DATETIME       NULL,
    [CTFUsageMax]              FLOAT (53)     NULL,
    [CTFUsageMin]              FLOAT (53)     NULL,
    [Dust]                     FLOAT (53)     NULL,
    [DustCooeficiencyVersion]  FLOAT (53)     NULL,
    [Editor]                   NVARCHAR (MAX) NULL,
    [FGQuantity]               FLOAT (53)     NULL,
    [FGQuantityAvailable]      FLOAT (53)     NULL,
    [FGQuantityBlocked]        FLOAT (53)     NULL,
    [FGQuantityPrevious]       FLOAT (53)     NULL,
    [ID]                       INT            NOT NULL,
    [MaterialQuantity]         FLOAT (53)     NULL,
    [MaterialQuantityPrevious] FLOAT (53)     NULL,
    [Modified]                 DATETIME       NULL,
    [Overuse]                  FLOAT (53)     NULL,
    [ProductType]              NVARCHAR (MAX) NULL,
    [SHCooeficiencyVersion]    FLOAT (53)     NULL,
    [SHMenthol]                FLOAT (53)     NULL,
    [SKU]                      NVARCHAR (MAX) NULL,
    [SKUIndex]                 INT            NULL,
    [Title]                    NVARCHAR (MAX) NOT NULL,
    [Tobacco]                  FLOAT (53)     NULL,
    [UsageMax]                 FLOAT (53)     NULL,
    [UsageMin]                 FLOAT (53)     NULL,
    [UsageVersion]             FLOAT (53)     NULL,
    [Version]                  INT            NULL,
    [Waste]                    FLOAT (53)     NULL,
    [WasteCooeficiencyVersion] FLOAT (53)     NULL,
	[OnlySQL]				   BIT			  NOT NULL,
	[UIVersionString]		   NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Batch_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Batch_BatchLibrary] FOREIGN KEY ([BatchLibraryIndex]) REFERENCES [dbo].[BatchLibrary] ([ID]),
    CONSTRAINT [FK_Batch_SKU] FOREIGN KEY ([SKUIndex]) REFERENCES [dbo].[SKU] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CustomsUnion')
  drop table  CustomsUnion;
CREATE TABLE [dbo].[CustomsUnion] (
    [Author]				NVARCHAR (MAX) NULL,
    [Created]				DATETIME       NULL,
    [Editor]				NVARCHAR (MAX) NULL,
    [EUPrimeMarket]			NVARCHAR (MAX) NULL,
    [ID]					INT            NOT NULL,
    [Modified]				DATETIME       NULL,
    [Title]					NVARCHAR (MAX) NOT NULL,
    [Version]				INT            NULL,
	[OnlySQL]				BIT			   NOT NULL,
	[UIVersionString]		NVARCHAR(max)  NULL,
    CONSTRAINT [PK_CustomsUnion_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CutfillerCoefficient')
  drop table  CutfillerCoefficient;
CREATE TABLE [dbo].[CutfillerCoefficient] (
    [Author]                 NVARCHAR (MAX) NULL,
    [CFTProductivityNormMax] FLOAT (53)     NULL,
    [CFTProductivityNormMin] FLOAT (53)     NULL,
    [CFTProductivityRateMax] FLOAT (53)     NULL,
    [CFTProductivityRateMin] FLOAT (53)     NULL,
    [Created]                DATETIME       NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NULL,
    [Version]                INT            NULL,
	[OnlySQL]				 BIT			NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	NULL,
    CONSTRAINT [PK_CutfillerCoefficient_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'InvoiceLibrary')
  drop table  InvoiceLibrary;
CREATE TABLE [dbo].[InvoiceLibrary] (
    [Author]                 NVARCHAR (MAX) NULL,
    [BillDoc]                NVARCHAR (MAX) NULL,
    [ClearenceIndex]         INT            NULL,
    [Created]                DATETIME       NULL,
    [DocumentCreatedBy]      NVARCHAR (MAX) NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [FileName]               NVARCHAR (MAX) NOT NULL,
    [ID]                     INT            NOT NULL,
    [InvoiceCreationDate]    DATETIME       NULL,
    [InvoiceLibraryReadOnly] BIT            NULL,
    [InvoiceLibraryStatus]   BIT            NULL,
    [IsExport]               BIT            NULL,
    [Modified]               DATETIME       NULL,
    [DocumentModifiedBy]     NVARCHAR (MAX) NULL,
    [Title]                  NVARCHAR (MAX) NULL,
    [Version]                INT            NULL,
	[OnlySQL]				 BIT			NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_InvoiceLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceLibrary_Clearence] FOREIGN KEY ([ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'InvoiceContent')
  drop table  InvoiceContent;
CREATE TABLE [dbo].[InvoiceContent] (
    [Archival]                  BIT            NULL,
    [Author]                    NVARCHAR (MAX) NULL,
    [Created]                   DATETIME       NULL,
    [Editor]                    NVARCHAR (MAX) NULL,
    [ID]                        INT            NOT NULL,
    [InvoiceContent2BatchIndex] INT            NULL,
    [InvoiceContentStatus]      NVARCHAR (MAX) NULL,
    [InvoiceIndex]              INT            NULL,
    [Modified]                  DATETIME       NULL,
    [ProductType]               NVARCHAR (MAX) NULL,
    [Quantity]                  FLOAT (53)     NULL,
    [SKUDescription]            NVARCHAR (MAX) NULL,
    [Title]                     NVARCHAR (MAX) NOT NULL,
    [Units]                     NVARCHAR (MAX) NULL,
    [Version]                   INT            NULL,
	[OnlySQL]					BIT			   NOT NULL,
	[UIVersionString]		    NVARCHAR(max)  NULL,
    CONSTRAINT [PK_InvoiceContent_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceContent_Batch] FOREIGN KEY ([InvoiceContent2BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_InvoiceContent_InvoiceLibrary] FOREIGN KEY ([InvoiceIndex]) REFERENCES [dbo].[InvoiceLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Material')
  drop table  Material;
CREATE TABLE [dbo].[Material] (
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Batch]               NVARCHAR (MAX) NULL,
    [Created]             DATETIME       NULL,
    [Dust]                FLOAT (53)     NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [FGQuantity]          FLOAT (53)     NULL,
    [ID]                  INT            NOT NULL,
    [Material2BatchIndex] INT            NULL,
    [Modified]            DATETIME       NULL,
    [Overuse]             FLOAT (53)     NULL,
    [ProductID]           NVARCHAR (MAX) NULL,
    [ProductType]         NVARCHAR (MAX) NULL,
    [SHMenthol]           FLOAT (53)     NULL,
    [SKU]                 NVARCHAR (MAX) NULL,
    [SKUDescription]      NVARCHAR (MAX) NULL,
    [StorLoc]             NVARCHAR (MAX) NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [Tobacco]             FLOAT (53)     NULL,
    [TobaccoQuantity]     FLOAT (53)     NULL,
    [Units]               NVARCHAR (MAX) NULL,
    [Version]             INT            NULL,
    [Waste]               FLOAT (53)     NULL,
	[OnlySQL]			  BIT			 NOT NULL,
	[UIVersionString]	  NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_Material_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Material_Batch] FOREIGN KEY ([Material2BatchIndex]) REFERENCES [dbo].[Batch] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JSOXCustomsSummary')
  drop table  JSOXCustomsSummary;
CREATE TABLE [dbo].[JSOXCustomsSummary] (
    [Author]                       NVARCHAR (MAX) NULL,
    [CompensationGood]             NVARCHAR (MAX) NULL,
    [Created]                      DATETIME       NULL,
    [CustomsProcedure]             NVARCHAR (MAX) NULL,
    [Editor]                       NVARCHAR (MAX) NULL,
    [ExportOrFreeCirculationSAD]   NVARCHAR (MAX) NULL,
    [ID]                           INT            NOT NULL,
    [IntroducingSADDate]           DATETIME       NULL,
    [IntroducingSADNo]             NVARCHAR (MAX) NULL,
    [InvoiceNo]                    NVARCHAR (MAX) NULL,
    [JSOXCustomsSummary2JSOXIndex] INT            NULL,
    [Modified]                     DATETIME       NULL,
    [RemainingQuantity]            FLOAT (53)     NULL,
    [SADDate]                      DATETIME       NULL,
    [Title]                        NVARCHAR (MAX) NOT NULL,
    [TotalAmount]                  FLOAT (53)     NULL,
    [Version]                      INT            NULL,
	[OnlySQL]					   BIT			  NOT NULL,
	[UIVersionString]		       NVARCHAR(max)  NULL,
    CONSTRAINT [PK_JSOXCustomsSummary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_JSOXCustomsSummary_JSOXLibrary] FOREIGN KEY ([JSOXCustomsSummary2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Disposal')
  drop table  Disposal;
CREATE TABLE [dbo].[Disposal] (
    [Archival]                     BIT            NULL,
    [Author]                       NVARCHAR (MAX) NULL,
    [ClearingType]                 NVARCHAR (MAX) NULL,
    [Created]                      DATETIME       NULL,
    [CustomsProcedure]             NVARCHAR (MAX) NULL,
    [CustomsStatus]                NVARCHAR (MAX) NULL,
    [Disposal2BatchIndex]          INT            NULL,
    [Disposal2ClearenceIndex]      INT            NULL,
    [Disposal2InvoiceContentIndex] INT            NULL,
    [Disposal2IPRIndex]            INT            NULL,
    [Disposal2MaterialIndex]       INT            NULL,
    [Disposal2PCNID]               INT            NULL,
    [DisposalStatus]               NVARCHAR (MAX) NULL,
    [DutyAndVAT]                   FLOAT (53)     NULL,
    [DutyPerSettledAmount]         FLOAT (53)     NULL,
    [Editor]                       NVARCHAR (MAX) NULL,
    [ID]                           INT            NOT NULL,
    [InvoiceNo]                    NVARCHAR (MAX) NULL,
    [IPRDocumentNo]                NVARCHAR (MAX) NULL,
    [JSOXCustomsSummaryIndex]      INT            NULL,
    [JSOXReportID]                 FLOAT (53)     NULL,
    [Modified]                     DATETIME       NULL,
    [RemainingQuantity]            FLOAT (53)     NULL,
    [SadConsignmentNo]             NVARCHAR (MAX) NULL,
    [SADDate]                      DATETIME       NULL,
    [SADDocumentNo]                NVARCHAR (MAX) NULL,
    [SettledQuantity]              FLOAT (53)     NULL,
    [SPNo]                         FLOAT (53)     NULL,
    [Title]                        NVARCHAR (MAX) NOT NULL,
    [TobaccoValue]                 FLOAT (53)     NULL,
    [VATPerSettledAmount]          FLOAT (53)     NULL,
    [Version]                      INT            NULL,
	[OnlySQL]					   BIT			  NOT NULL,
	[UIVersionString]		       NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Disposal_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Disposal_Batch] FOREIGN KEY ([Disposal2BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_Disposal_Clearence] FOREIGN KEY ([Disposal2ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID]),
    CONSTRAINT [FK_Disposal_InvoiceContent] FOREIGN KEY ([Disposal2InvoiceContentIndex]) REFERENCES [dbo].[InvoiceContent] ([ID]),
    CONSTRAINT [FK_Disposal_IPR] FOREIGN KEY ([Disposal2IPRIndex]) REFERENCES [dbo].[IPR] ([ID]),
    CONSTRAINT [FK_Disposal_JSOXCustomsSummary] FOREIGN KEY ([JSOXCustomsSummaryIndex]) REFERENCES [dbo].[JSOXCustomsSummary] ([ID]),
    CONSTRAINT [FK_Disposal_Material] FOREIGN KEY ([Disposal2MaterialIndex]) REFERENCES [dbo].[Material] ([ID]),
    CONSTRAINT [FK_Disposal_PCNCode] FOREIGN KEY ([Disposal2PCNID]) REFERENCES [dbo].[PCNCode] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Dust')
  drop table  Dust;
CREATE TABLE [dbo].[Dust] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [DustRatio]			FLOAT (53)     NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NULL,
    [Version]			INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Dust_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADDuties')
  drop table  SADDuties;
CREATE TABLE [dbo].[SADDuties] (
    [Amount]              FLOAT (53)     NULL,
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Created]             DATETIME       NULL,
    [DutyType]            NVARCHAR (MAX) NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [ID]                  INT            NOT NULL,
    [Modified]            DATETIME       NULL,
    [SADDuties2SADGoodID] INT            NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
	[UIVersionString]	  NVARCHAR(max)	 NULL,
    CONSTRAINT [PK_SADDuties_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDuties_SADGood] FOREIGN KEY ([SADDuties2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADPackage')
  drop table  SADPackage;
CREATE TABLE [dbo].[SADPackage] (
    [Archival]             BIT            NULL,
    [Author]               NVARCHAR (MAX) NULL,
    [Created]              DATETIME       NULL,
    [Editor]               NVARCHAR (MAX) NULL,
    [ID]                   INT            NOT NULL,
    [ItemNo]               FLOAT (53)     NULL,
    [Modified]             DATETIME       NULL,
    [Package]              NVARCHAR (MAX) NULL,
    [SADPackage2SADGoodID] INT            NULL,
    [Title]                NVARCHAR (MAX) NOT NULL,
    [Version]              INT            NULL,
	[OnlySQL]			   BIT			  NOT NULL,
	[UIVersionString]	   NVARCHAR(max)  NULL,
    CONSTRAINT [PK_SADPackage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADPackage_SADGood] FOREIGN KEY ([SADPackage2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADQuantity')
  drop table  SADQuantity;
CREATE TABLE [dbo].[SADQuantity] (
    [Archival]              BIT            NULL,
    [Author]                NVARCHAR (MAX) NULL,
    [Created]               DATETIME       NULL,
    [Editor]                NVARCHAR (MAX) NULL,
    [ID]                    INT            NOT NULL,
    [ItemNo]                FLOAT (53)     NULL,
    [Modified]              DATETIME       NULL,
    [NetMass]               FLOAT (53)     NULL,
    [SADQuantity2SADGoodID] INT            NULL,
    [Title]                 NVARCHAR (MAX) NOT NULL,
    [Units]                 NVARCHAR (MAX) NULL,
    [Version]               INT            NULL,
	[OnlySQL]			    BIT			   NOT NULL,
	[UIVersionString]		NVARCHAR(max)  NULL,
    CONSTRAINT [PK_SADQuantity_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADQuantity_SADGood] FOREIGN KEY ([SADQuantity2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SADRequiredDocuments')
  drop table  SADRequiredDocuments;
CREATE TABLE [dbo].[SADRequiredDocuments] (
    [Archival]                 BIT            NULL,
    [Author]                   NVARCHAR (MAX) NULL,
    [Code]                     NVARCHAR (MAX) NULL,
    [Created]                  DATETIME       NULL,
    [Editor]                   NVARCHAR (MAX) NULL,
    [ID]                       INT            NOT NULL,
    [Modified]                 DATETIME       NULL,
    [Number]                   NVARCHAR (MAX) NULL,
    [SADRequiredDoc2SADGoodID] INT            NULL,
    [Title]                    NVARCHAR (MAX) NOT NULL,
    [Version]                  INT            NULL,
	[OnlySQL]				   BIT			  NOT NULL,
	[UIVersionString]		   NVARCHAR(max)  NULL,
    CONSTRAINT [PK_SADRequiredDocuments_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADRequiredDocuments_SADGood] FOREIGN KEY ([SADRequiredDoc2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Settings')
  drop table  Settings;
CREATE TABLE [dbo].[Settings] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [KeyValue]			NVARCHAR (MAX) NOT NULL,
    [Modified]			DATETIME       NULL,
    [Title]				NVARCHAR (MAX) NOT NULL,
    [Version]			INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Settings_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SHMenthol')
  drop table  SHMenthol;
CREATE TABLE [dbo].[SHMenthol] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NOT NULL,
    [SHMentholRatio]	FLOAT (53)     NOT NULL,
    [Version]			INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_SHMenthol_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'StockLibrary')
  drop table  StockLibrary;
CREATE TABLE [dbo].[StockLibrary] (
    [Archival]               BIT            NULL,
    [Author]                 NVARCHAR (MAX) NULL,
    [Created]                DATETIME       NULL,
    [DocumentCreatedBy]      NVARCHAR (MAX) NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [FileName]               NVARCHAR (MAX) NOT NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NULL,
    [DocumentModifiedBy]     NVARCHAR (MAX) NULL,
    [Stock2JSOXLibraryIndex] INT            NULL,
    [Title]                  NVARCHAR (MAX) NULL,
    [Version]                INT            NULL,
	[OnlySQL]				 BIT			NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_StockLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockLibrary_JSOXLibrary] FOREIGN KEY ([Stock2JSOXLibraryIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'StockEntry')
  drop table  StockEntry;
CREATE TABLE [dbo].[StockEntry] (
    [Archival]          BIT            NULL,
    [Author]            NVARCHAR (MAX) NULL,
    [Batch]             NVARCHAR (MAX) NULL,
    [BatchIndex]        INT            NULL,
    [Blocked]           FLOAT (53)     NULL,
    [Created]           DATETIME       NULL,
    [Editor]            NVARCHAR (MAX) NULL,
    [ID]                INT            NOT NULL,
    [InQualityInsp]     FLOAT (53)     NULL,
    [IPRType]           BIT            NULL,
    [Modified]          DATETIME       NULL,
    [ProductType]       NVARCHAR (MAX) NULL,
    [Quantity]          FLOAT (53)     NULL,
    [RestrictedUse]     FLOAT (53)     NULL,
    [SKU]               NVARCHAR (MAX) NULL,
    [StockLibraryIndex] INT            NULL,
    [StorLoc]           NVARCHAR (MAX) NULL,
    [Title]             NVARCHAR (MAX) NOT NULL,
    [Units]             NVARCHAR (MAX) NULL,
    [Unrestricted]      FLOAT (53)     NULL,
    [Version]           INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_StockEntry_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockEntry_Batch] FOREIGN KEY ([BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_StockEntry_StockLibrary] FOREIGN KEY ([StockLibraryIndex]) REFERENCES [dbo].[StockLibrary] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Usage')
  drop table  Usage;
CREATE TABLE [dbo].[Usage] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [CTFUsageMax]		FLOAT (53)     NULL,
    [CTFUsageMin]		FLOAT (53)     NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [FormatIndex]		INT            NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [UsageMax]			FLOAT (53)     NULL,
    [UsageMin]			FLOAT (53)     NULL,
    [Version]			INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Usage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Usage_SPFormat] FOREIGN KEY ([FormatIndex]) REFERENCES [dbo].[SPFormat] ([ID])
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Warehouse')
  drop table  Warehouse;
CREATE TABLE [dbo].[Warehouse] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NOT NULL,
    [SPProcedure]		NVARCHAR (MAX) NOT NULL,
    [Title]				NVARCHAR (MAX) NOT NULL,
    [Version]			INT            NULL,
    [WarehouseName]		NVARCHAR (MAX) NOT NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Warehouse_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Waste')
  drop table  Waste;
CREATE TABLE [dbo].[Waste] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NULL,
    [Version]			INT            NULL,
    [WasteRatio]		FLOAT (53)     NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Waste_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'History')
  drop table  History;
CREATE TABLE [dbo].[History] (
    [ID]				INT IDENTITY (1, 1) NOT NULL,
    [ListName]			NVARCHAR (255) NOT NULL,
    [ItemID]			INT            NOT NULL,
    [FieldName]			NVARCHAR (255) NOT NULL,
    [FieldValue]		NVARCHAR (255) NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    [Modified]			DATETIME       NOT NULL,
    [ModifiedBy]		NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_History_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingLogs')
  drop table  ArchivingLogs;
CREATE TABLE [dbo].[ArchivingLogs] (
    [ID]       INT IDENTITY (1, 1) NOT NULL,
    [ListName] NVARCHAR (255) NOT NULL,
    [ItemID]   INT            NOT NULL,
    [Date]     DATETIME       NOT NULL,
    [UserName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ArchivingLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingOperationLogs')
  drop table  ArchivingOperationLogs;
CREATE TABLE [dbo].[ArchivingOperationLogs] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [Operation] NVARCHAR (255) NOT NULL,
    [Date]      DATETIME       NOT NULL,
    [UserName]  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ArchivingOperationLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);