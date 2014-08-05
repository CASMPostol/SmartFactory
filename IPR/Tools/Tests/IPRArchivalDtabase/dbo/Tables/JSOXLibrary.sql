﻿CREATE TABLE [dbo].[JSOXLibrary] (
    [BalanceDate]           DATETIME       NOT NULL,
    [BalanceQuantity]       FLOAT (53)     NOT NULL,
    [Created]               DATETIME       NOT NULL,
    [CreatedBy]             NVARCHAR (255) NOT NULL,
    [ID]                    INT            NOT NULL,
    [IntroducingDateEnd]    DATETIME       NOT NULL,
    [IntroducingDateStart]  DATETIME       NOT NULL,
    [IntroducingQuantity]   FLOAT (53)     NOT NULL,
    [JSOXLibraryReadOnly]   BIT            NOT NULL,
    [Modified]              DATETIME       NOT NULL,
    [ModifiedBy]            NVARCHAR (255) NOT NULL,
    [OutboundDateEnd]       DATETIME       NOT NULL,
    [OutboundDateStart]     DATETIME       NOT NULL,
    [OutboundQuantity]      FLOAT (53)     NOT NULL,
    [PreviousMonthDate]     DATETIME       NOT NULL,
    [PreviousMonthQuantity] FLOAT (53)     NOT NULL,
    [ReassumeQuantity]      FLOAT (53)     NOT NULL,
    [SituationDate]         DATETIME       NOT NULL,
    [SituationQuantity]     FLOAT (53)     NOT NULL,
    [Title]                 NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_JSOXLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

