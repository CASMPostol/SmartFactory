CREATE TABLE [dbo].[Dust] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [DustRatio]			FLOAT (53)     NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NULL,
    [Version]			INT            NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Dust_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

