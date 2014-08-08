CREATE TABLE [dbo].[BatchLibrary] (
    [BatchLibraryComments]   NVARCHAR(255)   NULL,
    [BatchLibraryOK]         BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_BatchLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

