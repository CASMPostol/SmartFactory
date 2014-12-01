if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CarrierPerformanceReport')
  drop table  CarrierPerformanceReport;
CREATE TABLE [dbo].[CarrierPerformanceReport] (
    [Author]                 NVARCHAR(max)   NULL,
    [CPR2PartnerTitle]       INT             NULL,
    [CPRDate]                DATETIME        NULL,
    [CPRNumberDelayed]       FLOAT           NULL,
    [CPRNumberDelayed1h]     FLOAT           NULL,
    [CPRNumberNotShowingUp]  FLOAT           NULL,
    [CPRNumberOnTime]        FLOAT           NULL,
    [CPRNumberOrdered]       FLOAT           NULL,
    [CPRNumberRejectedBadQuality] FLOAT           NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [ReportPeriod]           NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_CarrierPerformanceReport_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_CarrierPerformanceReport_Partner] FOREIGN KEY ([CPR2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
);
