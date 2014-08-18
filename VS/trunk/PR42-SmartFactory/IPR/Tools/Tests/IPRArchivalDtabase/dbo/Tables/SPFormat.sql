CREATE TABLE [dbo].[SPFormat] (
    [Author]          NVARCHAR (MAX) NULL,
    [CigaretteLenght] NVARCHAR (MAX) NOT NULL,
    [Created]         DATETIME       NULL,
    [Editor]          NVARCHAR (MAX) NULL,
    [FilterLenght]    NVARCHAR (MAX) NOT NULL,
    [ID]              INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [Title]           NVARCHAR (MAX) NOT NULL,
    [Version]         INT            NULL,
	[OnlySQL]		  BIT			 NOT NULL,
    CONSTRAINT [PK_SPFormat_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

