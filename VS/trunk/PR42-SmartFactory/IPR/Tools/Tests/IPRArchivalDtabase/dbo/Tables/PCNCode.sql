CREATE TABLE [dbo].[PCNCode] (
    [Author]            NVARCHAR (MAX) NULL,
    [CompensationGood]  NVARCHAR (MAX) NULL,
    [Created]           DATETIME       NULL,
    [Disposal]          BIT            NULL,
    [Editor]            NVARCHAR (MAX) NULL,
    [ID]                INT            NOT NULL,
    [Modified]          DATETIME       NULL,
    [ProductCodeNumber] NVARCHAR (MAX) NULL,
    [Title]             NVARCHAR (MAX) NOT NULL,
    [Version]           INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
    CONSTRAINT [PK_PCNCode_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

