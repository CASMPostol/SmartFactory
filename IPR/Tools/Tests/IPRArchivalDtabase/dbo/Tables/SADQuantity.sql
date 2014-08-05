CREATE TABLE [dbo].[SADQuantity] (
    [Created]               DATETIME       NOT NULL,
    [CreatedBy]             NVARCHAR (255) NOT NULL,
    [ID]                    INT            NOT NULL,
    [ItemNo]                FLOAT (53)     NOT NULL,
    [Modified]              DATETIME       NOT NULL,
    [ModifiedBy]            NVARCHAR (255) NOT NULL,
    [NetMass]               FLOAT (53)     NOT NULL,
    [SADQuantity2SADGoodID] INT            NOT NULL,
    [Title]                 NVARCHAR (255) NOT NULL,
    [Units]                 NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADQuantity_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADQuantity_SADGood] FOREIGN KEY ([SADQuantity2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

