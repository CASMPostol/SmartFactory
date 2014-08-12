CREATE TABLE [dbo].[Usage] (
    [Author]      NVARCHAR (MAX) NULL,
    [Created]     DATETIME       NULL,
    [CTFUsageMax] FLOAT (53)     NULL,
    [CTFUsageMin] FLOAT (53)     NULL,
    [Editor]      NVARCHAR (MAX) NULL,
    [FormatIndex] INT            NULL,
    [ID]          INT            NOT NULL,
    [Modified]    DATETIME       NULL,
    [UsageMax]    FLOAT (53)     NULL,
    [UsageMin]    FLOAT (53)     NULL,
    [Version]     INT            NULL,
    CONSTRAINT [PK_Usage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Usage_SPFormat] FOREIGN KEY ([FormatIndex]) REFERENCES [dbo].[SPFormat] ([ID])
);

