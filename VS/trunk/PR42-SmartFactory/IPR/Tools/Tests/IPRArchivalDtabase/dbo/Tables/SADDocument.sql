CREATE TABLE [dbo].[SADDocument] (
    [Created]                 DATETIME       NOT NULL,
    [CreatedBy]               NVARCHAR (255) NOT NULL,
    [Currency]                NVARCHAR (255) NOT NULL,
    [CustomsDebtDate]         DATETIME       NOT NULL,
    [DocumentNumber]          NVARCHAR (255) NOT NULL,
    [ExchangeRate]            FLOAT (53)     NOT NULL,
    [GrossMass]               FLOAT (53)     NOT NULL,
    [ID]                      INT            NOT NULL,
    [Modified]                DATETIME       NOT NULL,
    [ModifiedBy]              NVARCHAR (255) NOT NULL,
    [NetMass]                 FLOAT (53)     NOT NULL,
    [ReferenceNumber]         NVARCHAR (255) NOT NULL,
    [SADDocumenLibrarytIndex] INT            NOT NULL,
    [SystemID]                NVARCHAR (255) NOT NULL,
    [Title]                   NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADDocument_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDocument_SADDocumentLibrary] FOREIGN KEY ([SADDocumenLibrarytIndex]) REFERENCES [dbo].[SADDocumentLibrary] ([ID])
);

