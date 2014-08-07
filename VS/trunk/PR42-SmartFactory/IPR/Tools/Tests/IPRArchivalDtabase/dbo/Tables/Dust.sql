CREATE TABLE [dbo].[Dust] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [DustRatio]              FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductType]            NVARCHAR(255)   NULL,
    CONSTRAINT [PK_Dust_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

