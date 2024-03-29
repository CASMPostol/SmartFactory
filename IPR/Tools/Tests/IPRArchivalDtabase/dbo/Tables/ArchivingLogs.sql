﻿CREATE TABLE [dbo].[ArchivingLogs] (
    [ID]       INT IDENTITY (1, 1) NOT NULL,
    [ListName] NVARCHAR (255) NOT NULL,
    [ItemID]   INT            NOT NULL,
    [Date]     DATETIME       NOT NULL,
    [UserName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ArchivingLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

