CREATE TABLE [dbo].[Warehouse] (
    [Author]        NVARCHAR (MAX) NULL,
    [Created]       DATETIME       NULL,
    [Editor]        NVARCHAR (MAX) NULL,
    [ID]            INT            NOT NULL,
    [Modified]      DATETIME       NULL,
    [ProductType]   NVARCHAR (MAX) NOT NULL,
    [SPProcedure]   NVARCHAR (MAX) NOT NULL,
    [Title]         NVARCHAR (MAX) NOT NULL,
    [Version]       INT            NULL,
    [WarehouseName] NVARCHAR (MAX) NOT NULL,
	[OnlySQL]		BIT			   NOT NULL,
    CONSTRAINT [PK_Warehouse_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

