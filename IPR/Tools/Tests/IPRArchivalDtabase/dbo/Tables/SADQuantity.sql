CREATE TABLE [dbo].[SADQuantity] (
    [Archival]              BIT            NULL,
    [Author]                NVARCHAR (MAX) NULL,
    [Created]               DATETIME       NULL,
    [Editor]                NVARCHAR (MAX) NULL,
    [ID]                    INT            NOT NULL,
    [ItemNo]                FLOAT (53)     NULL,
    [Modified]              DATETIME       NULL,
    [NetMass]               FLOAT (53)     NULL,
    [SADQuantity2SADGoodID] INT            NULL,
    [Title]                 NVARCHAR (MAX) NOT NULL,
    [Units]                 NVARCHAR (MAX) NULL,
    [Version]               INT            NULL,
	[OnlySQL]			    BIT			   NOT NULL,
    CONSTRAINT [PK_SADQuantity_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADQuantity_SADGood] FOREIGN KEY ([SADQuantity2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

