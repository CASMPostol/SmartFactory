CREATE TABLE [dbo].[IPRLibrary] (
    [Author]             NVARCHAR (MAX) NULL,
    [Created]            DATETIME       NULL,
    [DocumentCreatedBy]  NVARCHAR (MAX) NULL,
    [DocumentNo]         NVARCHAR (MAX) NULL,
    [Editor]             NVARCHAR (MAX) NULL,
    [FileName]           NVARCHAR (MAX) NOT NULL,
    [ID]                 INT            NOT NULL,
    [Modified]           DATETIME       NULL,
    [DocumentModifiedBy] NVARCHAR (MAX) NULL,
    [Title]              NVARCHAR (MAX) NULL,
    [Version]            INT            NULL,
	[OnlySQL]			 BIT			NOT NULL,
	[UIVersionString]	 NVARCHAR(max)	NULL,	
    CONSTRAINT [PK_IPRLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

