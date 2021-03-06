if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TimeSlot')
  drop table  TimeSlot;
CREATE TABLE [dbo].[TimeSlot] (
    [Author]                 NVARCHAR(max)   NULL,
    [Category]               NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Description]            NVARCHAR(max)   NULL,
    [Duration]               INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EndDate]                DATETIME        NOT NULL,
    [EntryTime]              DATETIME        NULL,
    [EventCanceled]          BIT             NULL,
    [EventDate]              DATETIME        NOT NULL,
    [EventType]              INT             NULL,
    [ExitTime]               DATETIME        NULL,
    [fAllDayEvent]           BIT             NULL,
    [fRecurrence]            BIT             NULL,
    [ID]                     INT             NOT NULL,
    [IsDouble]               BIT             NULL,
    [Location]               NVARCHAR(max)   NULL,
    [MasterSeriesItemID]     INT             NULL,
    [Modified]               DATETIME        NULL,
    [Occupied]               NVARCHAR(max)   NULL,
    [owshiddenversion]       INT             NULL,
    [RecurrenceData]         NVARCHAR(max)   NULL,
    [RecurrenceID]           DATETIME        NULL,
    [TimeSlot2ShippingIndex] INT             NULL,
    [TimeSlot2ShippingPointLookup] INT             NULL,
    [TimeSpan]               FLOAT           NULL,
    [TimeZone]               INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WorkspaceLink]          BIT             NULL,
    [XMLTZone]               NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_TimeSlot_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_TimeSlot_Shipping] FOREIGN KEY ([TimeSlot2ShippingIndex]) REFERENCES [dbo].[Shipping] ([ID]),
    CONSTRAINT [FK_TimeSlot_ShippingPoint] FOREIGN KEY ([TimeSlot2ShippingPointLookup]) REFERENCES [dbo].[ShippingPoint] ([ID]),
);
