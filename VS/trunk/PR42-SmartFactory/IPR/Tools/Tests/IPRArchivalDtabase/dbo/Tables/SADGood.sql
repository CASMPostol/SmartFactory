CREATE TABLE [dbo].[SADGood] (
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Created]             DATETIME       NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [GoodsDescription]    NVARCHAR (MAX) NULL,
    [GrossMass]           FLOAT (53)     NULL,
    [ID]                  INT            NOT NULL,
    [ItemNo]              FLOAT (53)     NULL,
    [Modified]            DATETIME       NULL,
    [NetMass]             FLOAT (53)     NULL,
    [PCNTariffCode]       NVARCHAR (MAX) NULL,
    [SADDocumentIndex]    INT            NULL,
    [SPProcedure]         NVARCHAR (MAX) NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [TotalAmountInvoiced] FLOAT (53)     NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
    CONSTRAINT [PK_SADGood_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADGood_SADDocument] FOREIGN KEY ([SADDocumentIndex]) REFERENCES [dbo].[SADDocument] ([ID])
);

