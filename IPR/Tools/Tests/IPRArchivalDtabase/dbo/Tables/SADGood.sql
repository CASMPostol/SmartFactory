CREATE TABLE [dbo].[SADGood] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [GoodsDescription]       NVARCHAR(255)   NULL,
    [GrossMass]              FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [ItemNo]                 FLOAT           NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [NetMass]                FLOAT           NULL,
    [owshiddenversion]       INT             NULL,
    [PCNTariffCode]          NVARCHAR(255)   NULL,
    [SADDocumentIndex]       INT             NULL,
    [SPProcedure]            NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [TotalAmountInvoiced]    FLOAT           NULL,
    CONSTRAINT [PK_SADGood_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADGood_SADDocument] FOREIGN KEY ([SADDocumentIndex]) REFERENCES [dbo].[SADDocument] ([ID])
);

