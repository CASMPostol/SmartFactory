﻿CREATE TABLE [dbo].[Warehouse] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductType]            NVARCHAR(255)   NOT NULL,
    [SPProcedure]            NVARCHAR(255)   NOT NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [WarehouseName]          NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_Warehouse_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

