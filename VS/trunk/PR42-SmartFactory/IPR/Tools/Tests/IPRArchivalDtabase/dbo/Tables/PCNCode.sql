CREATE TABLE [dbo].[PCNCode] (
    [CompensationGood]       NVARCHAR(255)   NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [Disposal]               BIT             NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductCodeNumber]      NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_PCNCode_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

