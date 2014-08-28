CREATE TABLE [dbo].[Waste] (
    [Author]			NVARCHAR (MAX) NULL,
    [Created]			DATETIME       NULL,
    [Editor]			NVARCHAR (MAX) NULL,
    [ID]				INT            NOT NULL,
    [Modified]			DATETIME       NULL,
    [ProductType]		NVARCHAR (MAX) NULL,
    [Version]			INT            NULL,
    [WasteRatio]		FLOAT (53)     NULL,
	[OnlySQL]			BIT			   NOT NULL,
	[UIVersionString]	NVARCHAR(max)  NULL,
    CONSTRAINT [PK_Waste_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

