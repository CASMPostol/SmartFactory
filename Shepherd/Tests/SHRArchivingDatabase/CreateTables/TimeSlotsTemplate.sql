if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TimeSlotsTemplate')
  drop table  TimeSlotsTemplate;
CREATE TABLE [dbo].[TimeSlotsTemplate] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [ScheduleTemplateTitle]  INT             NULL,
    [TimeSlotsTemplateDay]   NVARCHAR(max)   NULL,
    [TimeSlotsTemplateEndHour] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateEndMinute] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateStartHour] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateStartMinute] NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_TimeSlotsTemplate_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_TimeSlotsTemplate_ScheduleTemplate] FOREIGN KEY ([ScheduleTemplateTitle]) REFERENCES [dbo].[ScheduleTemplate] ([ID]),
);
