if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Warehouse')
  drop table  Warehouse;
CREATE TABLE [dbo].[Warehouse] (
    [Author]                 NVARCHAR(max)   NULL,
    [CommodityTitle]         INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WarehouseAddress]       NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Warehouse_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Warehouse_Commodity] FOREIGN KEY ([CommodityTitle]) REFERENCES [dbo].[Commodity] ([ID]),
);
