CREATE TABLE [dbo].[StockLibrary] (
    [Archival]               BIT            NULL,
    [Author]                 NVARCHAR (MAX) NULL,
    [Created]                DATETIME       NULL,
    [DocumentCreatedBy]      NVARCHAR (MAX) NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [FileName]               NVARCHAR (MAX) NOT NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NULL,
    [DocumentModifiedBy]     NVARCHAR (MAX) NULL,
    [Stock2JSOXLibraryIndex] INT            NULL,
    [Title]                  NVARCHAR (MAX) NULL,
    [Version]                INT            NULL,
	[OnlySQL]				 BIT			NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_StockLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StockLibrary_JSOXLibrary] FOREIGN KEY ([Stock2JSOXLibraryIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

