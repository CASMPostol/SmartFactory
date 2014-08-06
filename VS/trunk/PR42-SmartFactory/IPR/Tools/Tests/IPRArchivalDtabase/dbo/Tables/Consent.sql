CREATE TABLE [dbo].[Consent] (
    [ConsentDate]            DATETIME        NULL,
    [ConsentPeriod]          FLOAT           NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [IsIPR]                  BIT             NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [ProductivityRateMax]    FLOAT           NULL,
    [ProductivityRateMin]    FLOAT           NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [ValidFromDate]          DATETIME        NULL,
    [ValidToDate]            DATETIME        NULL,
    CONSTRAINT [PK_Consent_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

