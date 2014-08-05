CREATE TABLE [dbo].[StockLibrary] (
    [Created]                DATETIME       NOT NULL,
    [CreatedBy]              NVARCHAR (255) NOT NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NOT NULL,
    [ModifiedBy]             NVARCHAR (255) NOT NULL,
    [Stock2JSOXLibraryIndex] INT            NOT NULL,
    [Title]                  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_StockLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockLibrary_JSOXLibrary] FOREIGN KEY ([Stock2JSOXLibraryIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

