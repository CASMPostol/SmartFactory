if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ScheduleTemplate')
  drop table  ScheduleTemplate;
CREATE TABLE [dbo].[ScheduleTemplate] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [ShippingPointLookupTitle] INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ScheduleTemplate_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_ScheduleTemplate_ShippingPoint] FOREIGN KEY ([ShippingPointLookupTitle]) REFERENCES [dbo].[ShippingPoint] ([ID]),
);
