CREATE TABLE [dbo].[Consent] (
    [ConsentDate]         DATETIME       NOT NULL,
    [ConsentPeriod]       FLOAT (53)     NOT NULL,
    [Created]             DATETIME       NOT NULL,
    [CreatedBy]           NVARCHAR (255) NOT NULL,
    [ID]                  INT            NOT NULL,
    [IsIPR]               BIT            NOT NULL,
    [Modified]            DATETIME       NOT NULL,
    [ModifiedBy]          NVARCHAR (255) NOT NULL,
    [ProductivityRateMax] FLOAT (53)     NOT NULL,
    [ProductivityRateMin] FLOAT (53)     NOT NULL,
    [Title]               NVARCHAR (255) NOT NULL,
    [ValidFromDate]       DATETIME       NOT NULL,
    [ValidToDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_Consent_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

