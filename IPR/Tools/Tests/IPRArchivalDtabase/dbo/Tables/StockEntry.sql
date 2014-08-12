﻿CREATE TABLE [dbo].[StockEntry] (
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
    CONSTRAINT [PK_StockEntry_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockEntry_Batch] FOREIGN KEY ([BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_StockEntry_StockLibrary] FOREIGN KEY ([StockLibraryIndex]) REFERENCES [dbo].[StockLibrary] ([ID])
);

