CREATE TABLE [dbo].[SADDocumentLibrary] (
    [Created]                    DATETIME       NOT NULL,
    [CreatedBy]                  NVARCHAR (255) NOT NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NOT NULL,
    [ModifiedBy]                 NVARCHAR (255) NOT NULL,
    [SADDocumentLibraryComments] NVARCHAR (255) NOT NULL,
    [SADDocumentLibraryOK]       BIT            NOT NULL,
    [Title]                      NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADDocumentLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

