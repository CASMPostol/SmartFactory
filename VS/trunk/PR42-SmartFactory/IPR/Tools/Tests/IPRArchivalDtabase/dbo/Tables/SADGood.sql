CREATE TABLE [dbo].[SADGood] (
    [Created]             DATETIME       NOT NULL,
    [CreatedBy]           NVARCHAR (255) NOT NULL,
    [GoodsDescription]    NVARCHAR (255) NOT NULL,
    [GrossMass]           FLOAT (53)     NOT NULL,
    [ID]                  INT            NOT NULL,
    [ItemNo]              FLOAT (53)     NOT NULL,
    [Modified]            DATETIME       NOT NULL,
    [ModifiedBy]          NVARCHAR (255) NOT NULL,
    [NetMass]             FLOAT (53)     NOT NULL,
    [PCNTariffCode]       NVARCHAR (255) NOT NULL,
    [SADDocumentIndex]    INT            NOT NULL,
    [SPProcedure]         NVARCHAR (255) NOT NULL,
    [Title]               NVARCHAR (255) NOT NULL,
    [TotalAmountInvoiced] FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_SADGood_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADGood_SADDocument] FOREIGN KEY ([SADDocumentIndex]) REFERENCES [dbo].[SADDocument] ([ID])
);

