CREATE TABLE [dbo].[SADRequiredDocuments] (
    [Archival]                 BIT            NULL,
    [Author]                   NVARCHAR (MAX) NULL,
    [Code]                     NVARCHAR (MAX) NULL,
    [Created]                  DATETIME       NULL,
    [Editor]                   NVARCHAR (MAX) NULL,
    [ID]                       INT            NOT NULL,
    [Modified]                 DATETIME       NULL,
    [Number]                   NVARCHAR (MAX) NULL,
    [SADRequiredDoc2SADGoodID] INT            NULL,
    [Title]                    NVARCHAR (MAX) NOT NULL,
    [Version]                  INT            NULL,
	[OnlySQL]				   BIT			  NOT NULL,
	[UIVersionString]		   NVARCHAR(max)  NULL,
    CONSTRAINT [PK_SADRequiredDocuments_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADRequiredDocuments_SADGood] FOREIGN KEY ([SADRequiredDoc2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

