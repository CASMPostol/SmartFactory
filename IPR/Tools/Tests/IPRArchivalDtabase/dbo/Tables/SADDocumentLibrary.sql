CREATE TABLE [dbo].[SADDocumentLibrary] (
    [Archival]                   BIT            NULL,
    [Author]                     NVARCHAR (MAX) NULL,
    [Created]                    DATETIME       NULL,
    [DocumentCreatedBy]          NVARCHAR (MAX) NULL,
    [Editor]                     NVARCHAR (MAX) NULL,
    [FileName]                   NVARCHAR (MAX) NOT NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NULL,
    [DocumentModifiedBy]         NVARCHAR (MAX) NULL,
    [SADDocumentLibraryComments] NVARCHAR (MAX) NULL,
    [SADDocumentLibraryOK]       BIT            NULL,
    [Title]                      NVARCHAR (MAX) NULL,
    [Version]                    INT            NULL,
	[OnlySQL]					 BIT			NOT NULL,
    CONSTRAINT [PK_SADDocumentLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

