if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DriversTeam')
  drop table  DriversTeam;
CREATE TABLE [dbo].[DriversTeam] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DriverTitle]            INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [ShippingIndex]          INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_DriversTeam_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_DriversTeam_Driver] FOREIGN KEY ([DriverTitle]) REFERENCES [dbo].[Driver] ([ID]),
    CONSTRAINT [FK_DriversTeam_Shipping] FOREIGN KEY ([ShippingIndex]) REFERENCES [dbo].[Shipping] ([ID]),
);
