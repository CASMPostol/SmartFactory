USE SHRARCHIVE
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Commodity')
  drop table  Commodity;
CREATE TABLE [dbo].[Commodity] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Commodity_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Warehouse')
  drop table  Warehouse;
CREATE TABLE [dbo].[Warehouse] (
    [Author]                 NVARCHAR(max)   NULL,
    [CommodityID]			 INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WarehouseAddress]       NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Warehouse_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Warehouse_Commodity] FOREIGN KEY ([CommodityID]) REFERENCES [dbo].[Commodity] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Partner')
  drop table  Partner;
CREATE TABLE [dbo].[Partner] (
    [Author]                 NVARCHAR(max)   NULL,
    [CellPhone]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [WarehouseID]    INT             NULL,
    [ServiceType]            NVARCHAR(max)   NULL,
    [ShepherdUser]           NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [VendorNumber]           NVARCHAR(max)   NULL,
    [WorkPhone]              NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Partner_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Partner_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Currency')
  drop table  Currency;
CREATE TABLE [dbo].[Currency] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ExchangeRate]           FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Currency_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
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
    [PayerName]              NVARCHAR(max)   NULL,
    [SendInvoiceToMultiline] NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WorkCity]               NVARCHAR(max)   NULL,
    [WorkCountry]            NVARCHAR(max)   NULL,
    [WorkZip]                NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_FreightPayer_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'BusinessDescription')
  drop table  BusinessDescription;
CREATE TABLE [dbo].[BusinessDescription] (
    [AdditionalComments]     NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_BusinessDescription_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SecurityEscortRoute')
  drop table  SecurityEscortRoute;
CREATE TABLE [dbo].[SecurityEscortRoute] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [CurrencyID]             INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EscortDestination]      NVARCHAR(max)   NULL,
    [FreightPayerID]         INT             NULL,
    [ID]                     INT             NOT NULL,
    [MaterialMaster]         NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [PartnerID]              INT             NULL,
    [RemarkMM]               NVARCHAR(max)   NULL,
    [SecurityCost]           FLOAT           NULL,
    [BusinessDescriptionID] INT             NULL,
    [SecurityEscrotPO]       NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_SecurityEscortRoute_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_SecurityEscortRoute_Currency] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_SecurityEscortRoute_FreightPayer] FOREIGN KEY ([FreightPayerID]) REFERENCES [dbo].[FreightPayer] ([ID]),
    CONSTRAINT [FK_SecurityEscortRoute_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_SecurityEscortRoute_BusinessDescription] FOREIGN KEY ([BusinessDescriptionID]) REFERENCES [dbo].[BusinessDescription] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SealProtocolLibrary')
  drop table  SealProtocolLibrary;
CREATE TABLE [dbo].[SealProtocolLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DocumentCreatedBy]       NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [FileName]            NVARCHAR(max)   NOT NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [DocumentModifiedBy]      NVARCHAR(max)   NULL,
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
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Country')
  drop table  Country;
CREATE TABLE [dbo].[Country] (
    [Author]                 NVARCHAR(max)   NULL,
    [CountryGroup]           NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Country_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'City')
  drop table  City;
CREATE TABLE [dbo].[City] (
    [Author]                 NVARCHAR(max)   NULL,
    [CountryID]              INT             NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_City_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_City_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'EscortPOLibrary')
  drop table  EscortPOLibrary;
CREATE TABLE [dbo].[EscortPOLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DocumentCreatedBy]       NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [FileName]               NVARCHAR(max)   NOT NULL,
    [FPOWarehouseAddress]    NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [DocumentModifiedBy]      NVARCHAR(max)   NULL,
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
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'FreightPOLibrary')
  drop table  FreightPOLibrary;
CREATE TABLE [dbo].[FreightPOLibrary] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DocumentCreatedBy]      NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EmailAddress]           NVARCHAR(max)   NULL,
    [FileName]               NVARCHAR(max)   NOT NULL,
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
    [DocumentModifiedBy]     NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_FreightPOLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Carrier')
  drop table  Carrier;
CREATE TABLE [dbo].[Carrier] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Carrier_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SAPDestinationPlant')
  drop table  SAPDestinationPlant;
CREATE TABLE [dbo].[SAPDestinationPlant] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_SAPDestinationPlant_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ShipmentType')
  drop table  ShipmentType;
CREATE TABLE [dbo].[ShipmentType] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ShipmentType_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TransportUnitType')
  drop table  TransportUnitType;
CREATE TABLE [dbo].[TransportUnitType] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_TransportUnitType_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Route')
  drop table  Route;
CREATE TABLE [dbo].[Route] (
    [Author]                 NVARCHAR(max)   NULL,
    [CarrierID]           INT             NULL,
    [Created]                DATETIME        NULL,
    [CurrencyID]          INT             NULL,
    [DepartureCity]          NVARCHAR(max)   NULL,
    [DeparturePort]          NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [FreightPayerID]      INT             NULL,
    [GoodsHandlingPO]        NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Incoterm]               NVARCHAR(max)   NULL,
    [MaterialMaster]         NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [PartnerID]           INT             NULL,
    [RemarkMM]               NVARCHAR(max)   NULL,
    [BusinessDescriptionID] INT             NULL,
    [CityID]        INT             NULL,
    [CommodityID]        INT             NULL,
    [SAPDestinationPlantID] INT             NULL,
    [ShipmentTypeID]      INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [TransportCosts]         FLOAT           NULL,
    [TransportUnitTypeID] INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Route_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Route_Carrier] FOREIGN KEY ([CarrierID]) REFERENCES [dbo].[Carrier] ([ID]),
    CONSTRAINT [FK_Route_Currency] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Route_FreightPayer] FOREIGN KEY ([FreightPayerID]) REFERENCES [dbo].[FreightPayer] ([ID]),
    CONSTRAINT [FK_Route_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_Route_BusinessDescription] FOREIGN KEY ([BusinessDescriptionID]) REFERENCES [dbo].[BusinessDescription] ([ID]),
    CONSTRAINT [FK_Route_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_Route_Commodity] FOREIGN KEY ([CommodityID]) REFERENCES [dbo].[Commodity] ([ID]),
    CONSTRAINT [FK_Route_SAPDestinationPlant] FOREIGN KEY ([SAPDestinationPlantID]) REFERENCES [dbo].[SAPDestinationPlant] ([ID]),
    CONSTRAINT [FK_Route_ShipmentType] FOREIGN KEY ([ShipmentTypeID]) REFERENCES [dbo].[ShipmentType] ([ID]),
    CONSTRAINT [FK_Route_TransportUnitType] FOREIGN KEY ([TransportUnitTypeID]) REFERENCES [dbo].[TransportUnitType] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Truck')
  drop table  Truck;
CREATE TABLE [dbo].[Truck] (
    [AdditionalComments]     NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [PartnerID]     INT             NULL,
    [VehicleType]            NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Truck_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Truck_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Trailer')
  drop table  Trailer;
CREATE TABLE [dbo].[Trailer] (
    [AdditionalComments]     NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [PartnerID]   INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Trailer_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Trailer_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Shipping')
  drop table  Shipping;
CREATE TABLE [dbo].[Shipping] (
    [AdditionalCosts]        FLOAT           NULL,
    [ArrivalTime]            DATETIME        NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [BusinessDescription]    NVARCHAR(max)   NULL,
    [Comments]      NVARCHAR(max)   NULL,
    [ContainerNo]            NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DockNumber]             NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EndTime]                DATETIME        NULL,
    [EstimateDeliveryTime]   DATETIME        NULL,
    [EuroPalletsQuantity]    FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [InduPalletsQuantity]    FLOAT           NULL,
    [IsOutbound]             BIT             NULL,
    [LoadingType]            NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [EscortPartnerID]           INT             NULL,
    [PoLastModification]     DATETIME        NULL,
    [PoNumberMultiline]      NVARCHAR(max)   NULL,
    [ReportPeriod]           NVARCHAR(max)   NULL,
    [SecurityEscortCatalogID] INT             NULL,
    [SecuritySealProtocolID] INT             NULL,
    [CityID]          INT             NULL,
    [Currency4AddCostsID] INT             NULL,
    [Currency4CostsPerKUID] INT             NULL,
    [CurrencyForEscortID] INT             NULL,
    [CurrencyForFreightID] INT             NULL,
    [EscortPOID] INT             NULL,
    [FreightPOID] INT             NULL,
    [PartnerID]  INT             NULL,
    [RouteID]    INT             NULL,
    [TransportUnitTypeID] INT             NULL,
    [TruckID]    INT             NULL,
    [WarehouseID] INT             NULL,
    [ShippingCarrierTitle]   NVARCHAR(max)   NULL,
    [ShippingCommodityTitle] NVARCHAR(max)   NULL,
    [ShippingCountryTitle]   NVARCHAR(max)   NULL,
    [ShippingDuration]       FLOAT           NULL,
    [ShippingFreightCost]    FLOAT           NULL,
    [ShippingFreightPayerTitle] NVARCHAR(max)   NULL,
    [ShippingRouteDepartureCity] NVARCHAR(max)   NULL,
    [ShippingSecurityCost]   FLOAT           NULL,
    [ShippingState]          NVARCHAR(max)   NULL,
    [ShippingState2]         NVARCHAR(max)   NULL,
    [StartTime]              DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [TotalCostsPerKU]        FLOAT           NULL,
    [TotalQuantityKU]        FLOAT           NULL,
    [TrailerCondition]       NVARCHAR(max)   NULL,
    [TrailerConditionComments] NVARCHAR(max)   NULL,
    [TrailerID]           INT             NULL,
    [TruckAwaiting]          BIT             NULL,
    [EscortCarID]             INT             NULL,
    [TSEndTime]              DATETIME        NULL,
    [TSStartTime]            DATETIME        NULL,
    [WarehouseEndTime]       DATETIME        NULL,
    [WarehouseStartTime]     DATETIME        NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Shipping_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Shipping_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_Shipping_SecurityEscortRoute] FOREIGN KEY ([SecurityEscortCatalogID]) REFERENCES [dbo].[SecurityEscortRoute] ([ID]),
    CONSTRAINT [FK_Shipping_SealProtocolLibrary] FOREIGN KEY ([SecuritySealProtocolID]) REFERENCES [dbo].[SealProtocolLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_Shipping_Currency4AddCosts] FOREIGN KEY ([Currency4AddCostsID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_Currency4CostsPerKU] FOREIGN KEY ([Currency4CostsPerKUID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_CurrencyForEscort] FOREIGN KEY ([CurrencyForEscortID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_CurrencyForFreight] FOREIGN KEY ([CurrencyForFreightID]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_EscortPOLibrary] FOREIGN KEY ([EscortPOID]) REFERENCES [dbo].[EscortPOLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_FreightPOLibrary] FOREIGN KEY ([FreightPOID]) REFERENCES [dbo].[FreightPOLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_PartnerEscort] FOREIGN KEY ([EscortPartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_Shipping_Route] FOREIGN KEY ([RouteID]) REFERENCES [dbo].[Route] ([ID]),
    CONSTRAINT [FK_Shipping_TransportUnitType] FOREIGN KEY ([TransportUnitTypeID]) REFERENCES [dbo].[TransportUnitType] ([ID]),
    CONSTRAINT [FK_Shipping_Truck] FOREIGN KEY ([TruckID]) REFERENCES [dbo].[Truck] ([ID]),
    CONSTRAINT [FK_Shipping_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID]),
    CONSTRAINT [FK_Shipping_Trailer] FOREIGN KEY ([TrailerID]) REFERENCES [dbo].[Trailer] ([ID]),
    CONSTRAINT [FK_Shipping_Escort] FOREIGN KEY ([EscortCarID]) REFERENCES [dbo].[Truck] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'AlarmsAndEvents')
  drop table  AlarmsAndEvents;
CREATE TABLE [dbo].[AlarmsAndEvents] (
    [AlarmAndEventDetails]   NVARCHAR(max)   NULL,
    [AlarmAndEventOwner]     NVARCHAR(max)   NULL,
    [AlarmPriority]          NVARCHAR(max)   NULL,
    [PartnerID] INT             NULL,
    [ShippingID] INT             NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_AlarmsAndEvents_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_AlarmsAndEvents_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_AlarmsAndEvents_Shipping] FOREIGN KEY ([ShippingID]) REFERENCES [dbo].[Shipping] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CarrierPerformanceReport')
  drop table  CarrierPerformanceReport;
CREATE TABLE [dbo].[CarrierPerformanceReport] (
    [Author]                 NVARCHAR(max)   NULL,
    [PartnerID]       INT             NULL,
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
    [ReportPeriod]           NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_CarrierPerformanceReport_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_CarrierPerformanceReport_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Market')
  drop table  Market;
CREATE TABLE [dbo].[Market] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Market_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DestinationMarket')
  drop table  DestinationMarket;
CREATE TABLE [dbo].[DestinationMarket] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [CityID] INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [MarketID]            INT             NULL,
    [Modified]               DATETIME        NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_DestinationMarket_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_DestinationMarket_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_DestinationMarket_Market] FOREIGN KEY ([MarketID]) REFERENCES [dbo].[Market] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Driver')
  drop table  Driver;
CREATE TABLE [dbo].[Driver] (
    [Author]                 NVARCHAR(max)   NULL,
    [CellPhone]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [PartnerID]    INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [IdentityDocumentNumber] NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [ToBeDeleted]            BIT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Driver_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Driver_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DriversTeam')
  drop table  DriversTeam;
CREATE TABLE [dbo].[DriversTeam] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DriverID]            INT             NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ShippingID]          INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_DriversTeam_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_DriversTeam_Driver] FOREIGN KEY ([DriverID]) REFERENCES [dbo].[Driver] ([ID]),
    CONSTRAINT [FK_DriversTeam_Shipping] FOREIGN KEY ([ShippingID]) REFERENCES [dbo].[Shipping] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'LoadDescription')
  drop table  LoadDescription;
CREATE TABLE [dbo].[LoadDescription] (
    [Author]                 NVARCHAR(max)   NULL,
    [CMRNumber]              NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DeliveryNumber]         NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [GoodsQuantity]          FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [InvoiceNumber]          NVARCHAR(max)   NULL,
    [CommodityID] INT             NULL,
    [PartnerID] INT             NULL,
    [ShippingID] INT             NULL,
    [MarketID]            INT             NULL,
    [Modified]               DATETIME        NULL,
    [NumberOfPallets]        FLOAT           NULL,
    [PalletType]             NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_LoadDescription_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_LoadDescription_Commodity] FOREIGN KEY ([CommodityID]) REFERENCES [dbo].[Commodity] ([ID]),
    CONSTRAINT [FK_LoadDescription_Partner] FOREIGN KEY ([PartnerID]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_LoadDescription_Shipping] FOREIGN KEY ([ShippingID]) REFERENCES [dbo].[Shipping] ([ID]),
    CONSTRAINT [FK_LoadDescription_Market] FOREIGN KEY ([MarketID]) REFERENCES [dbo].[Market] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ShippingPoint')
  drop table  ShippingPoint;
CREATE TABLE [dbo].[ShippingPoint] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Direction]              NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ShippingPointDescription] NVARCHAR(max)   NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [WarehouseID]         INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ShippingPoint_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_ShippingPoint_Warehouse] FOREIGN KEY ([WarehouseID]) REFERENCES [dbo].[Warehouse] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ScheduleTemplate')
  drop table  ScheduleTemplate;
CREATE TABLE [dbo].[ScheduleTemplate] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ShippingPointLookupTitle] INT             NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ScheduleTemplate_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_ScheduleTemplate_ShippingPoint] FOREIGN KEY ([ShippingPointLookupTitle]) REFERENCES [dbo].[ShippingPoint] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TimeSlot')
  drop table  TimeSlot;
CREATE TABLE [dbo].[TimeSlot] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EndTime]                DATETIME        NOT NULL,
    [EntryTime]              DATETIME        NULL,
    [ExitTime]               DATETIME        NULL,
	[ID]                     INT             NOT NULL,
    [IsDouble]               BIT             NULL,
    [Modified]               DATETIME        NULL,
    [Occupied]               NVARCHAR(max)   NULL,
	[StartTime]              DATETIME        NOT NULL,
    [ShippingID] INT             NULL,
    [ShippingPointID] INT             NULL,
    [TimeSpan]               FLOAT           NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_TimeSlot_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_TimeSlot_Shipping] FOREIGN KEY ([ShippingID]) REFERENCES [dbo].[Shipping] ([ID]),
    CONSTRAINT [FK_TimeSlot_ShippingPoint] FOREIGN KEY ([ShippingPointID]) REFERENCES [dbo].[ShippingPoint] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'TimeSlotsTemplate')
  drop table  TimeSlotsTemplate;
CREATE TABLE [dbo].[TimeSlotsTemplate] (
    [Author]                 NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ScheduleTemplateID]  INT             NULL,
    [TimeSlotsTemplateDay]   NVARCHAR(max)   NULL,
    [TimeSlotsTemplateEndHour] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateEndMinute] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateStartHour] NVARCHAR(max)   NULL,
    [TimeSlotsTemplateStartMinute] NVARCHAR(max)   NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_TimeSlotsTemplate_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_TimeSlotsTemplate_ScheduleTemplate] FOREIGN KEY ([ScheduleTemplateID]) REFERENCES [dbo].[ScheduleTemplate] ([ID]),
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'History')
  drop table  History;
CREATE TABLE [dbo].[History] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [ListName]               NVARCHAR(255)   NOT NULL,
    [ItemID]                 INT             NOT NULL,
    [FieldName]              NVARCHAR(255)   NOT NULL,
    [FieldValue]             NVARCHAR(255)   NOT NULL,
    [Modified]               DATETIME        NOT NULL,
    [ModifiedBy]             NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_History_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingLogs')
  drop table  ArchivingLogs;
CREATE TABLE [dbo].[ArchivingLogs] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [ListName]               NVARCHAR(255)   NOT NULL,
    [ItemID]                 INT             NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    [UserName]               NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ArchivingLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ArchivingOperationLogs')
  drop table  ArchivingOperationLogs;
CREATE TABLE [dbo].[ArchivingOperationLogs] (
    [ID]                     INT IDENTITY(1,1) NOT NULL,
    [Operation]              NVARCHAR(255)   NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    [UserName]               NVARCHAR(255)   NOT NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_ArchivingOperationLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);
