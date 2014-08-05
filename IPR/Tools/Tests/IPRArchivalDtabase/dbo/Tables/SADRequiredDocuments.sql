CREATE TABLE [dbo].[SADRequiredDocuments] (
    [Code]                     NVARCHAR (255) NOT NULL,
    [Created]                  DATETIME       NOT NULL,
    [CreatedBy]                NVARCHAR (255) NOT NULL,
    [ID]                       INT            NOT NULL,
    [Modified]                 DATETIME       NOT NULL,
    [ModifiedBy]               NVARCHAR (255) NOT NULL,
    [Number]                   NVARCHAR (255) NOT NULL,
    [SADRequiredDoc2SADGoodID] INT            NOT NULL,
    [Title]                    NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADRequiredDocuments_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADRequiredDocuments_SADGood] FOREIGN KEY ([SADRequiredDoc2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

