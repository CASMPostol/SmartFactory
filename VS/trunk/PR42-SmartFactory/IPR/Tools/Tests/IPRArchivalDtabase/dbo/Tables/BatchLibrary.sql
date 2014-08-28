CREATE TABLE [dbo].[BatchLibrary] (
    [Author]               NVARCHAR (MAX) NULL,
    [BatchLibraryComments] NVARCHAR (MAX) NULL,
    [BatchLibraryOK]       BIT            NULL,
    [Created]              DATETIME       NULL,
    [DocumentCreatedBy]    NVARCHAR (MAX) NULL,
    [Editor]               NVARCHAR (MAX) NULL,
    [FileName]             NVARCHAR (MAX) NOT NULL,
    [ID]                   INT            NOT NULL,
    [Modified]             DATETIME       NULL,
    [DocumentModifiedBy]   NVARCHAR (MAX) NULL,
    [Title]                NVARCHAR (MAX) NULL,
    [Version]              INT            NULL,
	[OnlySQL]			   BIT			  NOT NULL,
	[UIVersionString]	   NVARCHAR(max)  NULL,	
    CONSTRAINT [PK_BatchLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

