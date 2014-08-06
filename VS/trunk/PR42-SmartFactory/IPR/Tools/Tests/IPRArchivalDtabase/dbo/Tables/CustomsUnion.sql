CREATE TABLE [dbo].[CustomsUnion] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [EUPrimeMarket]          NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_CustomsUnion_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

