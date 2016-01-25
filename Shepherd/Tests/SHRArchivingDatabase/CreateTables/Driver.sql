if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Driver')
  drop table  Driver;
CREATE TABLE [dbo].[Driver] (
    [Author]                 NVARCHAR(max)   NULL,
    [CellPhone]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Driver2PartnerTitle]    INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [IdentityDocumentNumber] NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Driver_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Driver_Partner] FOREIGN KEY ([Driver2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
);
