if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Commodity')
  drop table  Commodity;
CREATE TABLE [dbo].[Commodity] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Dummy]                  FLOAT           NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Commodity_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
