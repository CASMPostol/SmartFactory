CREATE TABLE [dbo].[PCNCode] (
    [CompensationGood]  NVARCHAR (255) NOT NULL,
    [Created]           DATETIME       NOT NULL,
    [CreatedBy]         NVARCHAR (255) NOT NULL,
    [Disposal]          BIT            NOT NULL,
    [ID]                INT            NOT NULL,
    [Modified]          DATETIME       NOT NULL,
    [ModifiedBy]        NVARCHAR (255) NOT NULL,
    [ProductCodeNumber] NVARCHAR (255) NOT NULL,
    [Title]             NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_PCNCode_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

