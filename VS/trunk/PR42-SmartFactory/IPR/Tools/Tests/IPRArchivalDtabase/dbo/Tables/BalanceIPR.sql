﻿CREATE TABLE [dbo].[BalanceIPR] (
    [Balance]                        FLOAT (53)     NOT NULL,
    [BalanceBatchIndex]              INT            NOT NULL,
    [BalanceIPR2JSOXIndex]           INT            NOT NULL,
    [Batch]                          NVARCHAR (255) NOT NULL,
    [Created]                        DATETIME       NOT NULL,
    [CreatedBy]                      NVARCHAR (255) NOT NULL,
    [CustomsProcedure]               NVARCHAR (255) NOT NULL,
    [DocumentNo]                     NVARCHAR (255) NOT NULL,
    [DustCSNotStarted]               FLOAT (53)     NOT NULL,
    [DustCSStarted]                  FLOAT (53)     NOT NULL,
    [ID]                             INT            NOT NULL,
    [InvoiceNo]                      NVARCHAR (255) NOT NULL,
    [IPRBook]                        FLOAT (53)     NOT NULL,
    [IPRIndex]                       INT            NOT NULL,
    [Modified]                       DATETIME       NOT NULL,
    [ModifiedBy]                     NVARCHAR (255) NOT NULL,
    [OGLIntroduction]                NVARCHAR (255) NOT NULL,
    [OveruseCSNotStarted]            FLOAT (53)     NOT NULL,
    [OveruseCSStarted]               FLOAT (53)     NOT NULL,
    [PureTobaccoCSNotStarted]        FLOAT (53)     NOT NULL,
    [PureTobaccoCSStarted]           FLOAT (53)     NOT NULL,
    [SHMentholCSNotStarted]          FLOAT (53)     NOT NULL,
    [SHMentholCSStarted]             FLOAT (53)     NOT NULL,
    [SHWasteOveruseCSNotStarted]     FLOAT (53)     NOT NULL,
    [SKU]                            NVARCHAR (255) NOT NULL,
    [Title]                          NVARCHAR (255) NOT NULL,
    [TobaccoAvailable]               FLOAT (53)     NOT NULL,
    [TobaccoCSFinished]              FLOAT (53)     NOT NULL,
    [TobaccoEnteredIntoIPR]          FLOAT (53)     NOT NULL,
    [TobaccoInFGCSNotStarted]        FLOAT (53)     NOT NULL,
    [TobaccoInFGCSStarted]           FLOAT (53)     NOT NULL,
    [TobaccoStarted]                 FLOAT (53)     NOT NULL,
    [TobaccoToBeUsedInTheProduction] FLOAT (53)     NOT NULL,
    [TobaccoUsedInTheProduction]     FLOAT (53)     NOT NULL,
    [WasteCSNotStarted]              FLOAT (53)     NOT NULL,
    [WasteCSStarted]                 FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_BalanceIPR_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BalanceIPR_BalanceBatch] FOREIGN KEY ([BalanceBatchIndex]) REFERENCES [dbo].[BalanceBatch] ([ID]),
    CONSTRAINT [FK_BalanceIPR_IPR] FOREIGN KEY ([IPRIndex]) REFERENCES [dbo].[IPR] ([ID]),
    CONSTRAINT [FK_BalanceIPR_JSOXLibrary] FOREIGN KEY ([BalanceIPR2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

