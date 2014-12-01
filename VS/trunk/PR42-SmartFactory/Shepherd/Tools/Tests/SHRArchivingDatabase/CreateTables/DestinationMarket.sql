if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DestinationMarket')
  drop table  DestinationMarket;
CREATE TABLE [dbo].[DestinationMarket] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DestinationMarket2CityTitle] INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [MarketTitle]            INT             NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_DestinationMarket_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_DestinationMarket_City] FOREIGN KEY ([DestinationMarket2CityTitle]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_DestinationMarket_Market] FOREIGN KEY ([MarketTitle]) REFERENCES [dbo].[Market] ([ID]),
);
