CREATE TABLE [dbo].[BatchLibrary] (
    [BatchLibraryComments] NVARCHAR (255) NOT NULL,
    [BatchLibraryOK]       BIT            NOT NULL,
    [Created]              DATETIME       NOT NULL,
    [CreatedBy]            NVARCHAR (255) NOT NULL,
    [ID]                   INT            NOT NULL,
    [Modified]             DATETIME       NOT NULL,
    [ModifiedBy]           NVARCHAR (255) NOT NULL,
    [Title]                NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_BatchLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

