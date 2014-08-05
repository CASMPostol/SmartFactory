CREATE TABLE [dbo].[Dust] (
    [Created]     DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [DustRatio]   FLOAT (53)     NOT NULL,
    [ID]          INT            NOT NULL,
    [Modified]    DATETIME       NOT NULL,
    [ModifiedBy]  NVARCHAR (255) NOT NULL,
    [ProductType] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Dust_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

