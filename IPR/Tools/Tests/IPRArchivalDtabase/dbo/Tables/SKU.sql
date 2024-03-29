﻿CREATE TABLE [dbo].[SKU] (
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

