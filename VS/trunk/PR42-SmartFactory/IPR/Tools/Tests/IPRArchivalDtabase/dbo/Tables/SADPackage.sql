CREATE TABLE [dbo].[SADPackage] (
    [Created]              DATETIME       NOT NULL,
    [CreatedBy]            NVARCHAR (255) NOT NULL,
    [ID]                   INT            NOT NULL,
    [ItemNo]               FLOAT (53)     NOT NULL,
    [Modified]             DATETIME       NOT NULL,
    [ModifiedBy]           NVARCHAR (255) NOT NULL,
    [Package]              NVARCHAR (255) NOT NULL,
    [SADPackage2SADGoodID] INT            NOT NULL,
    [Title]                NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADPackage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADPackage_SADGood] FOREIGN KEY ([SADPackage2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

