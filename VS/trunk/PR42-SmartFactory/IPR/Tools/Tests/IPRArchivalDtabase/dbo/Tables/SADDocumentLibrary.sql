CREATE TABLE [dbo].[SADDocumentLibrary] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                         INT            NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [SADDocumentLibraryComments] NVARCHAR(255)   NULL,
    [SADDocumentLibraryOK]   BIT             NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_SADDocumentLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

