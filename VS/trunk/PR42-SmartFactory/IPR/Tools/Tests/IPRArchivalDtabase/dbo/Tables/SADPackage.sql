CREATE TABLE [dbo].[SADPackage] (
    [Archival]             BIT            NULL,
    [Author]               NVARCHAR (MAX) NULL,
    [Created]              DATETIME       NULL,
    [Editor]               NVARCHAR (MAX) NULL,
    [ID]                   INT            NOT NULL,
    [ItemNo]               FLOAT (53)     NULL,
    [Modified]             DATETIME       NULL,
    [Package]              NVARCHAR (MAX) NULL,
    [SADPackage2SADGoodID] INT            NULL,
    [Title]                NVARCHAR (MAX) NOT NULL,
    [Version]              INT            NULL,
    CONSTRAINT [PK_SADPackage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADPackage_SADGood] FOREIGN KEY ([SADPackage2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

