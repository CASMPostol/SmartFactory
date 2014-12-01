if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Market')
  drop table  Market;
CREATE TABLE [dbo].[Market] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Dummy]                  FLOAT           NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Market_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
