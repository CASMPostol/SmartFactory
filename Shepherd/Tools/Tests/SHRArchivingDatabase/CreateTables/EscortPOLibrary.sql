if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'EscortPOLibrary')
  drop table  EscortPOLibrary;
CREATE TABLE [dbo].[EscortPOLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Created_x0020_By]       NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [FileLeafRef]            NVARCHAR(max)   NOT NULL,
    [FPOWarehouseAddress]    NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Modified_x0020_By]      NVARCHAR(max)   NULL,
    [owshiddenversion]       INT             NULL,
    [SecurityPOCity]         NVARCHAR(max)   NULL,
    [SecurityPOCommodity]    NVARCHAR(max)   NULL,
    [SecurityPOCountry]      NVARCHAR(max)   NULL,
    [SecurityPOEscortCosts]  FLOAT           NULL,
    [SecurityPOEscortCurrency] NVARCHAR(max)   NULL,
    [SecurityPOEscortPayerAddress] NVARCHAR(max)   NULL,
    [SecurityPOEscortPayerCity] NVARCHAR(max)   NULL,
    [SecurityPOEscortPayerName] NVARCHAR(max)   NULL,
    [SecurityPOEscortPayerNIP] NVARCHAR(max)   NULL,
    [SecurityPOEscortPayerZip] NVARCHAR(max)   NULL,
    [SecurityPOEscortProvider] NVARCHAR(max)   NULL,
    [SecurityPOSentInvoiceToMultiline] NVARCHAR(max)   NULL,
    [SPODispatchDate]        DATETIME        NULL,
    [SPOFreightPO]           NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_EscortPOLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
