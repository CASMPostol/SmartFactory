CREATE TABLE [dbo].[Usage] (
    [Created]     DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [CTFUsageMax] FLOAT (53)     NOT NULL,
    [CTFUsageMin] FLOAT (53)     NOT NULL,
    [FormatIndex] INT            NOT NULL,
    [ID]          INT            NOT NULL,
    [Modified]    DATETIME       NOT NULL,
    [ModifiedBy]  NVARCHAR (255) NOT NULL,
    [UsageMax]    FLOAT (53)     NOT NULL,
    [UsageMin]    FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_Usage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Usage_SPFormat] FOREIGN KEY ([FormatIndex]) REFERENCES [dbo].[SPFormat] ([ID])
);

