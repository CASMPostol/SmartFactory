﻿CREATE TABLE [dbo].[SADConsignment] (
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
    CONSTRAINT [PK_SADConsignment_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

