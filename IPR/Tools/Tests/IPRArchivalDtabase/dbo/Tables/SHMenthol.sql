CREATE TABLE [dbo].[SHMenthol] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductType]            NVARCHAR(255)   NOT NULL,
    [SHMentholRatio]         FLOAT           NOT NULL,
    CONSTRAINT [PK_SHMenthol_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

