CREATE TABLE [dbo].[Usage] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [CTFUsageMax]            FLOAT           NULL,
    [CTFUsageMin]            FLOAT           NULL,
    [FormatIndex]            INT             NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [UsageMax]               FLOAT           NULL,
    [UsageMin]               FLOAT           NULL,
    CONSTRAINT [PK_Usage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Usage_SPFormat] FOREIGN KEY ([FormatIndex]) REFERENCES [dbo].[SPFormat] ([ID])
);

