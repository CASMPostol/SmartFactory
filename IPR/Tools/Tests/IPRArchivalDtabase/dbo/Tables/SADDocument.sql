CREATE TABLE [dbo].[SADDocument] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [Currency]               NVARCHAR(255)   NULL,
    [CustomsDebtDate]        DATETIME        NULL,
    [DocumentNumber]         NVARCHAR(255)   NULL,
    [ExchangeRate]           FLOAT           NULL,
    [GrossMass]              FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [NetMass]                FLOAT           NULL,
    [owshiddenversion]       INT             NULL,
    [ReferenceNumber]        NVARCHAR(255)   NULL,
    [SADDocumenLibrarytIndex] INT             NULL,
    [SystemID]               NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_SADDocument_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDocument_SADDocumentLibrary] FOREIGN KEY ([SADDocumenLibrarytIndex]) REFERENCES [dbo].[SADDocumentLibrary] ([ID])
);

