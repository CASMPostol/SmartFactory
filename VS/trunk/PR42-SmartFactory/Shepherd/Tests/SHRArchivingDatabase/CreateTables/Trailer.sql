if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Trailer')
  drop table  Trailer;
CREATE TABLE [dbo].[Trailer] (
    [AdditionalComments]     NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [Trailer2PartnerTitle]   INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Trailer_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Trailer_Partner] FOREIGN KEY ([Trailer2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
);
