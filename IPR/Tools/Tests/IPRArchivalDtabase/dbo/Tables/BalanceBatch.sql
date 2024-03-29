﻿CREATE TABLE [dbo].[BalanceBatch] (
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

