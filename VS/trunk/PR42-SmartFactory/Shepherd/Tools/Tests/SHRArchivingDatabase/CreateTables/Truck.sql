if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Truck')
  drop table  Truck;
CREATE TABLE [dbo].[Truck] (
    [AdditionalComments]     NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [Truck2PartnerTitle]     INT             NULL,
    [VehicleType]            NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Truck_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Truck_Partner] FOREIGN KEY ([Truck2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
);
