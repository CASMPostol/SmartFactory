CREATE TABLE [dbo].[SADPackage] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [ItemNo]                 FLOAT           NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [Package]                NVARCHAR(255)   NULL,
    [SADPackage2SADGoodID]   INT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_SADPackage_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADPackage_SADGood] FOREIGN KEY ([SADPackage2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

