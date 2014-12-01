if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ShippingPoint')
  drop table  ShippingPoint;
CREATE TABLE [dbo].[ShippingPoint] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Direction]              NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [ShippingPointDescription] NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WarehouseTitle]         INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ShippingPoint_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_ShippingPoint_Warehouse] FOREIGN KEY ([WarehouseTitle]) REFERENCES [dbo].[Warehouse] ([ID]),
);
