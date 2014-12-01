if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Partner')
  drop table  Partner;
CREATE TABLE [dbo].[Partner] (
    [Author]                 NVARCHAR(max)   NULL,
    [CellPhone]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Partner2WarehouseTitle] INT             NULL,
    [ServiceType]            NVARCHAR(max)   NULL,
    [ShepherdUser]           NVARCHAR(max)   NOT NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [VendorNumber]           NVARCHAR(max)   NULL,
    [WorkPhone]              NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Partner_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Partner_Warehouse] FOREIGN KEY ([Partner2WarehouseTitle]) REFERENCES [dbo].[Warehouse] ([ID]),
);
