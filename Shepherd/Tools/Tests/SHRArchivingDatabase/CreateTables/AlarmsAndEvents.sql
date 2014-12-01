if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'AlarmsAndEvents')
  drop table  AlarmsAndEvents;
CREATE TABLE [dbo].[AlarmsAndEvents] (
    [AlarmAndEventDetails]   NVARCHAR(max)   NULL,
    [AlarmAndEventOwner]     NVARCHAR(max)   NULL,
    [AlarmPriority]          NVARCHAR(max)   NULL,
    [AlarmsAndEventsList2PartnerTitle] INT             NULL,
    [AlarmsAndEventsList2Shipping] INT             NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_AlarmsAndEvents_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_AlarmsAndEvents_Partner] FOREIGN KEY ([AlarmsAndEventsList2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_AlarmsAndEvents_Shipping] FOREIGN KEY ([AlarmsAndEventsList2Shipping]) REFERENCES [dbo].[Shipping] ([ID]),
);
