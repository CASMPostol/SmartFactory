CREATE TABLE [dbo].[StockLibrary] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [Stock2JSOXLibraryIndex] INT             NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_StockLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockLibrary_JSOXLibrary] FOREIGN KEY ([Stock2JSOXLibraryIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

