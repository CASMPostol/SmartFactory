if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SealProtocolLibrary')
  drop table  SealProtocolLibrary;
CREATE TABLE [dbo].[SealProtocolLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Created_x0020_By]       NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [FileLeafRef]            NVARCHAR(max)   NOT NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Modified_x0020_By]      NVARCHAR(max)   NULL,
    [owshiddenversion]       INT             NULL,
    [SealProtocol1stDriver]  NVARCHAR(max)   NULL,
    [SealProtocol1stEscort]  NVARCHAR(max)   NULL,
    [SealProtocol2ndDriver]  NVARCHAR(max)   NULL,
    [SealProtocol2ndEscort]  NVARCHAR(max)   NULL,
    [SealProtocolCity]       NVARCHAR(max)   NULL,
    [SealProtocolContainersNo] NVARCHAR(max)   NULL,
    [SealProtocolCountry]    NVARCHAR(max)   NULL,
    [SealProtocolDispatchDate] DATETIME        NULL,
    [SealProtocolDispatchDateActual] DATETIME        NULL,
    [SealProtocolDriverPhone] NVARCHAR(max)   NULL,
    [SealProtocolEscortCarNo] NVARCHAR(max)   NULL,
    [SealProtocolEscortPhone] NVARCHAR(max)   NULL,
    [SealProtocolForwarder]  NVARCHAR(max)   NULL,
    [SealProtocolSecurityEscortProvider] NVARCHAR(max)   NULL,
    [SealProtocolTrailerNo]  NVARCHAR(max)   NULL,
    [SealProtocolTruckNo]    NVARCHAR(max)   NULL,
    [SealProtocolWarehouse]  NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_SealProtocolLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
