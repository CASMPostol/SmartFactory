CREATE TABLE [dbo].[Waste] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductType]            NVARCHAR(255)   NULL,
    [WasteRatio]             FLOAT           NULL,
    CONSTRAINT [PK_Waste_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

