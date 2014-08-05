CREATE TABLE [dbo].[Waste] (
    [Created]     DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (255) NOT NULL,
    [ID]          INT            NOT NULL,
    [Modified]    DATETIME       NOT NULL,
    [ModifiedBy]  NVARCHAR (255) NOT NULL,
    [ProductType] NVARCHAR (255) NOT NULL,
    [WasteRatio]  FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_Waste_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

