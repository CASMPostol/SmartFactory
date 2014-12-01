if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'FreightPayer')
  drop table  FreightPayer;
CREATE TABLE [dbo].[FreightPayer] (
    [Author]                 NVARCHAR(max)   NULL,
    [CompanyAddress]         NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [NIP]                    NVARCHAR(max)   NULL,
    [owshiddenversion]       INT             NULL,
    [PayerName]              NVARCHAR(max)   NULL,
    [SendInvoiceToMultiline] NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WorkCity]               NVARCHAR(max)   NULL,
    [WorkCountry]            NVARCHAR(max)   NULL,
    [WorkZip]                NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_FreightPayer_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
