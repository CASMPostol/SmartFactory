CREATE TABLE [dbo].[SADDocument] (
    [Archival]                BIT            NULL,
    [Author]                  NVARCHAR (MAX) NULL,
    [Created]                 DATETIME       NULL,
    [Currency]                NVARCHAR (MAX) NULL,
    [CustomsDebtDate]         DATETIME       NULL,
    [DocumentNumber]          NVARCHAR (MAX) NULL,
    [Editor]                  NVARCHAR (MAX) NULL,
    [ExchangeRate]            FLOAT (53)     NULL,
    [GrossMass]               FLOAT (53)     NULL,
    [ID]                      INT            NOT NULL,
    [Modified]                DATETIME       NULL,
    [NetMass]                 FLOAT (53)     NULL,
    [ReferenceNumber]         NVARCHAR (MAX) NULL,
    [SADDocumenLibrarytIndex] INT            NULL,
    [SystemID]                NVARCHAR (MAX) NULL,
    [Title]                   NVARCHAR (MAX) NOT NULL,
    [Version]                 INT            NULL,
    CONSTRAINT [PK_SADDocument_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDocument_SADDocumentLibrary] FOREIGN KEY ([SADDocumenLibrarytIndex]) REFERENCES [dbo].[SADDocumentLibrary] ([ID])
);

