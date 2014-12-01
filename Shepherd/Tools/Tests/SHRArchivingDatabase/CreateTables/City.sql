if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'City')
  drop table  City;
CREATE TABLE [dbo].[City] (
    [Author]                 NVARCHAR(max)   NULL,
    [CountryTitle]           INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_City_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_City_Country] FOREIGN KEY ([CountryTitle]) REFERENCES [dbo].[Country] ([ID]),
);
