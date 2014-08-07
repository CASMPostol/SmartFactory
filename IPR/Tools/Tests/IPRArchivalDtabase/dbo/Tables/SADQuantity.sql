CREATE TABLE [dbo].[SADQuantity] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [ItemNo]                 FLOAT           NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [NetMass]                FLOAT           NULL,
    [owshiddenversion]       INT             NULL,
    [SADQuantity2SADGoodID]  INT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [Units]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_SADQuantity_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADQuantity_SADGood] FOREIGN KEY ([SADQuantity2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

