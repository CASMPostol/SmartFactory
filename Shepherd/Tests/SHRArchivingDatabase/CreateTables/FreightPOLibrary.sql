if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'FreightPOLibrary')
  drop table  FreightPOLibrary;
CREATE TABLE [dbo].[FreightPOLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Created_x0020_By]       NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [FileLeafRef]            NVARCHAR(max)   NOT NULL,
    [FPODispatchDate]        DATETIME        NULL,
    [FPOFreightPO]           NVARCHAR(max)   NULL,
    [FPOLoadingDate]         DATETIME        NULL,
    [FPOWarehouseAddress]    NVARCHAR(max)   NULL,
    [FreightPOCity]          NVARCHAR(max)   NULL,
    [FreightPOCommodity]     NVARCHAR(max)   NULL,
    [FreightPOCountry]       NVARCHAR(max)   NULL,
    [FreightPOCurrency]      NVARCHAR(max)   NULL,
    [FreightPOForwarder]     NVARCHAR(max)   NULL,
    [FreightPOPayerAddress]  NVARCHAR(max)   NULL,
    [FreightPOPayerCity]     NVARCHAR(max)   NULL,
    [FreightPOPayerName]     NVARCHAR(max)   NULL,
    [FreightPOPayerNIP]      NVARCHAR(max)   NULL,
    [FreightPOPayerZip]      NVARCHAR(max)   NULL,
    [FreightPOSendInvoiceToMultiline] NVARCHAR(max)   NULL,
    [FreightPOTransportCosts] FLOAT           NULL,
    [FreightPOTransportUnitType] NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Modified_x0020_By]      NVARCHAR(max)   NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_FreightPOLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
