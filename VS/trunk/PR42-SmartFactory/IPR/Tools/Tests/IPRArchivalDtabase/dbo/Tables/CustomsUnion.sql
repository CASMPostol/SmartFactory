CREATE TABLE [dbo].[CustomsUnion] (
    [Author]        NVARCHAR (MAX) NULL,
    [Created]       DATETIME       NULL,
    [Editor]        NVARCHAR (MAX) NULL,
    [EUPrimeMarket] NVARCHAR (MAX) NULL,
    [ID]            INT            NOT NULL,
    [Modified]      DATETIME       NULL,
    [Title]         NVARCHAR (MAX) NOT NULL,
    [Version]       INT            NULL,
	[OnlySQL]		BIT			   NOT NULL,
    CONSTRAINT [PK_CustomsUnion_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

