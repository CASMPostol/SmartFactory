CREATE TABLE [dbo].[CustomsUnion] (
    [Created]       DATETIME       NOT NULL,
    [CreatedBy]     NVARCHAR (255) NOT NULL,
    [EUPrimeMarket] NVARCHAR (255) NOT NULL,
    [ID]            INT            NOT NULL,
    [Modified]      DATETIME       NOT NULL,
    [ModifiedBy]    NVARCHAR (255) NOT NULL,
    [Title]         NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_CustomsUnion_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

