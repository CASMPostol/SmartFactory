CREATE TABLE [dbo].[SADRequiredDocuments] (
    [Archival]               BIT             NULL,
    [Code]                   NVARCHAR(255)   NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Number]                 NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [SADRequiredDoc2SADGoodID] INT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_SADRequiredDocuments_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADRequiredDocuments_SADGood] FOREIGN KEY ([SADRequiredDoc2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

